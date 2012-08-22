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
namespace FeatherVane.Web.Http.Vanes
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.IO.Compression;
    using System.Security.Principal;

    public class CompressionVane :
        Vane<Connection>
    {
        public VaneHandler<Connection> GetHandler(VaneContext<Connection> context, NextVane<Connection> next)
        {
            VaneHandler<Connection> nextHandler = next.GetHandler(context);



            string acceptEncoding = context.GetContext<Request>().Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                if ((acceptEncoding.IndexOf("gzip", StringComparison.OrdinalIgnoreCase) != -1))
                {
                    return new GZipCompressHandler(nextHandler);
                }
                
                if (acceptEncoding.IndexOf("deflate", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return new DeflateCompressHandler(nextHandler);
                }
            }

            return nextHandler;
        }

        class CompressionContext :
            Connection
        {
            readonly Request _requestContext;
            readonly ResponseContext _responseContext;
            readonly Connection _context;

            public CompressionContext(Connection context, Request requestContext,
                ResponseContext responseContext)
            {
                _context = context;
                _requestContext = requestContext;
                _responseContext = responseContext;
            }

            public Request Request
            {
                get { return _requestContext; }
            }

            public ResponseContext Response
            {
                get { return _responseContext; }
            }

            public IPrincipal User
            {
                get { return _context.User; }
            }

            public ServerContext Server
            {
                get { return _context.Server; }
            }

            public void End()
            {
                _context.End();
            }
        }

        class CompressionResponseContext :
            ResponseContext
        {
            readonly ResponseContext _context;
            readonly Stream _outputStream;

            public CompressionResponseContext(ResponseContext context, Stream outputStream)
            {
                _context = context;
                _outputStream = outputStream;
            }

            public NameValueCollection Headers
            {
                get { return _context.Headers; }
            }

            public Stream OutputStream
            {
                get { return _outputStream; }
            }

            public long ContentLength64
            {
                get { return _context.ContentLength64; }
                set { _context.ContentLength64 = value; }
            }

            public int StatusCode
            {
                get { return _context.StatusCode; }
                set { _context.StatusCode = value; }
            }

            public string StatusDescription
            {
                get { return _context.StatusDescription; }
                set { _context.StatusDescription = value; }
            }

            public string ContentType
            {
                get { return _context.ContentType; }
                set { _context.ContentType = value; }
            }

            public void Redirect(string url)
            {
                _context.Redirect(url);
            }

            public void Close()
            {
                _context.Close();
            }
        }

        class DeflateCompressHandler :
            VaneHandler<Connection>
        {
            readonly VaneHandler<Connection> _next;

            public DeflateCompressHandler(VaneHandler<Connection> next)
            {
                _next = next;
            }

            public void Handle(VaneContext<Connection> connectionContext)
            {
//                using (
//                    var outputStream = new DeflateStream(connectionContext.Body.Response.OutputStream,
//                        CompressionMode.Compress, true))
//                {
//                    connectionContext.Body.Response.Headers["Content-Encoding"] = "deflate";

//                    var responseContext = new CompressionResponseContext(connectionContext.Body.Response, outputStream);
//                    var context = new CompressionContext(connectionContext, connectionContext.Body.Request, responseContext);
//
                    _next.Handle(connectionContext);

//                    outputStream.Flush();
//                }
            }
        }

        class GZipCompressHandler :
            VaneHandler<Connection>
        {
            readonly VaneHandler<Connection> _next;

            public GZipCompressHandler(VaneHandler<Connection> next)
            {
                _next = next;
            }

            public void Handle(VaneContext<Connection> connectionContext)
            {
//                using (
//                    var outputStream = new GZipStream(connectionContext.Response.OutputStream, CompressionMode.Compress,
//                        true))
//                {
//                    connectionContext.Response.Headers["Content-Encoding"] = "gzip";
//
//                    var responseContext = new CompressionResponseContext(connectionContext.Response, outputStream);
//                    var context = new CompressionContext(connectionContext, connectionContext.Request, responseContext);

                    _next.Handle(connectionContext);
//
//                    outputStream.Flush();
                }
            }
        }
    }
