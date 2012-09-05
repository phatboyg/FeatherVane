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
    using System.Net;
    using System.Security.Principal;

    public class HttpListenerConnectionContext :
        ConnectionContext
    {
        readonly DateTime _acceptedAt;
        readonly HttpListenerContext _httpContext;
        readonly RequestContext _request;
        readonly ResponseContext _response;
        readonly ServerContext _server;

        public HttpListenerConnectionContext(ServerContext server, HttpListenerContext httpContext, DateTime acceptedAt)
        {
            _server = server;
            _httpContext = httpContext;
            _acceptedAt = acceptedAt;

            _request = new HttpListenerRequestContext(httpContext.Request);
            _response = new HttpListenerResponseContext(httpContext.Response);
        }

        public RequestContext Request
        {
            get { return _request; }
        }

        public ResponseContext Response
        {
            get { return _response; }
        }

        public IPrincipal User
        {
            get { return _httpContext.User; }
        }

        public ServerContext Server
        {
            get { return _server; }
        }

        public void End()
        {
            _response.Close();
            _httpContext.Response.Close();
        }
    }
}