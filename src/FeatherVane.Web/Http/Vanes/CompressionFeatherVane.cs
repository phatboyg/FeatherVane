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
    using System.IO.Compression;


    public class CompressionFeatherVane :
        FeatherVane<ConnectionContext>
    {
        public void Build(Builder<ConnectionContext> builder, Payload<ConnectionContext> payload,
            Vane<ConnectionContext> next)
        {
            var request = payload.Get<RequestContext>();
            var response = payload.Get<ResponseContext>();

            response.Headers["Vary"] = "Accept-Encoding";

            ApplyCompressionIfAppropriate(request, builder, payload);

            next.Build(builder, payload);
        }

        void ApplyCompressionIfAppropriate(RequestContext request, Builder<ConnectionContext> builder,
            Payload<ConnectionContext> payload)
        {
            if (request.HttpMethod == "HEAD")
                return;

            string accept = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(accept))
                return;

            if ((accept.IndexOf("gzip", StringComparison.OrdinalIgnoreCase) != -1)
                || accept.Trim().Equals("*"))
            {
                builder.Execute(() => GzipResponse(payload));
                return;
            }

            if (accept.IndexOf("deflate", StringComparison.OrdinalIgnoreCase) != -1)
                builder.Execute(() => DeflateResponse(payload));
        }

        void DeflateResponse(Payload<ConnectionContext> payload)
        {
            var response = payload.Get<ResponseContext>();
            response.Headers["Content-Encoding"] = "deflate";
            response.AddBodyStreamFilter(x => new DeflateStream(x, CompressionMode.Compress, true));
        }

        void GzipResponse(Payload<ConnectionContext> payload)
        {
            var response = payload.Get<ResponseContext>();
            response.Headers["Content-Encoding"] = "gzip";
            response.AddBodyStreamFilter(x => new GZipStream(x, CompressionMode.Compress, true));
        }
    }
}