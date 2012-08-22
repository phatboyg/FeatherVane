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
namespace FeatherVane.Web.Http.Contexts
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using Util;

    public class HttpListenerResponseContext :
        ResponseContext
    {
        readonly HttpListenerResponse _response;
        StreamStack _bodyStream;

        public HttpListenerResponseContext(HttpListenerResponse response)
        {
            _response = response;
            _bodyStream = new StreamStack(response.OutputStream);
        }

        public NameValueCollection Headers
        {
            get { return _response.Headers; }
        }

        public Stream BodyStream
        {
            get { return _bodyStream; }
        }

        public void AddBodyStreamFilter(Func<Stream, Stream> decoratorFactory)
        {
            _bodyStream.Push(decoratorFactory);
        }

        public long ContentLength
        {
            get { return _response.ContentLength64; }
            set { _response.ContentLength64 = value; }
        }

        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public string StatusDescription
        {
            get { return _response.StatusDescription; }
            set { _response.StatusDescription = value; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public void Redirect(string url)
        {
            _response.Redirect(url);
        }

        public void Close()
        {
            _bodyStream.Close();
            _bodyStream.Dispose();

            _response.Close();
        }
    }
}