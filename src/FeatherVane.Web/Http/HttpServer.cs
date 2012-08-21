// Copyright 2012-2012 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
// ANY KIND, either express or implied. See the License for the specific language governing
// permissions and limitations under the License.
namespace FeatherVane.Web.Http
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Contexts;

    public class HttpServer :
        ServerContext
    {
        readonly Uri _uri;
        readonly NextVane<ConnectionContext> _vane;
        bool _closing;
        int _concurrentConnectionLimit = 100000;
        int _connectionCount;
        HttpListener _httpListener;

        public HttpServer(Uri uri, NextVane<ConnectionContext> vane)
        {
            _uri = uri;
            _vane = vane;
        }

        public int ConnectionCount
        {
            get { return _connectionCount; }
        }

        public Uri BaseUri
        {
            get { return _uri; }
        }

        protected void StartListener(Uri uri)
        {
            try
            {
                string prefix = GetPrefixForUri(uri);

                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add(prefix);

                // TODO consider mapping the access types/schemes
                _httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

                _httpListener.Start();

                QueueAccept();
            }
            catch (HttpListenerException ex)
            {
                throw new HttpServerException("The server could not be started", ex);
            }
        }

        void QueueAccept()
        {
            if (_closing)
                return;

            _httpListener.BeginGetContext(GetContext, null);
        }

        void GetContext(IAsyncResult asyncResult)
        {
            try
            {
                HttpListenerContext context = _httpListener.EndGetContext(asyncResult);

                DateTime acceptedAt = DateTime.UtcNow;

                Interlocked.Increment(ref _connectionCount);

                var connectionTask = new Task<ConnectionContext>(() => HandleConnection(acceptedAt, context));
                connectionTask.ContinueWith(task => ConnectionComplete(task.Result));
                connectionTask.Start();
            }
            catch (InvalidOperationException)
            {
            }
            catch (HttpListenerException)
            {
            }
            finally
            {
                QueueAccept();
            }
        }

        ConnectionContext HandleConnection(DateTime acceptedAt, HttpListenerContext httpContext)
        {
            var connectionContext = new HttpListenerConnectionContext(this, httpContext, acceptedAt);

            VaneHandler<ConnectionContext> handler = _vane.GetHandler(connectionContext);

            handler.Handle(connectionContext);

            return connectionContext;
        }

        void ShutdownListener()
        {
            try
            {
                _httpListener.Close();
            }
            catch (HttpListenerException ex)
            {
                throw new HttpServerException("An error occurred while shutting down the server", ex);
            }
        }

        static string GetPrefixForUri(Uri uri)
        {
            if (false == new[] {"http", "https"}.Contains(uri.Scheme.ToLowerInvariant()))
                throw new ArgumentException("The Uri must be an http/https address: " + uri, "uri");

            string prefix = uri.ToString();

            if (UriHostNameType.IPv4 == uri.HostNameType
                && IPAddress.Any.GetAddressBytes().SequenceEqual(IPAddress.Parse(uri.Host).GetAddressBytes()))
                prefix = prefix.Replace("://0.0.0.0", "://+");

            if (!prefix.EndsWith("/"))
                prefix += "/";

            return prefix;
        }

        public void Start()
        {
            StartListener(_uri);
        }

        public void Stop()
        {
            _closing = true;

            if (_connectionCount == 0)
                ShutdownListener();

            for (int i = 0; i < 30; i++)
            {
                if(!_httpListener.IsListening)
                    break;

                Thread.Sleep(1000);
            }

            if(_httpListener.IsListening)
                _httpListener.Abort();
        }

        void ConnectionComplete(ConnectionContext connectionContext)
        {
            int count = Interlocked.Decrement(ref _connectionCount);

            if (_closing)
            {
                if (count == 0)
                    ShutdownListener();
            }
        }
    }
}