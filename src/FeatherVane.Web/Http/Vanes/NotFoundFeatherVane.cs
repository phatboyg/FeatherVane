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
    /// <summary>
    /// Handles a connection inside of a channel, allowing handlers to be injected
    /// along the channel network to handle things like authentication and ultimately
    /// routing
    /// </summary>
    public class NotFoundFeatherVane :
        FeatherVane<ConnectionContext>
    {
        public Handler<ConnectionContext> GetHandler(Payload<ConnectionContext> payload, Vane<ConnectionContext> next)
        {
            return new NotFoundHandler();
        }

        class NotFoundHandler :
            Handler<ConnectionContext>
        {
            public void Handle(Payload<ConnectionContext> payload)
            {
                ResponseContext response;
                if(payload.TryGet(out response))
                {
                    response.StatusCode = 404;
                    response.StatusDescription = "Not Found";
                    response.WriteHtml(
                        @"<body><h1>Your request was not processed</h1><p>The URI specified was not recognized by any registered handler.</p></body>");
                }
            }
        }
    }
}