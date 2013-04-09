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
namespace FeatherVane.Web.Http.Feathers
{
    using System.Text;
    using System.Text.RegularExpressions;
    using Taskell;


    public class TextEncoding :
        Feather<ConnectionContext>
    {
        TextEncodingSettings _settings;

        public TextEncoding()
        {
            _settings = new TextEncodingSettings
                {
                    CharSetPattern = new Regex(@"charset=([\w-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    DefaultTextEncoding = Encoding.GetEncoding("ISO-8859-1"),
                };
        }


        public void Compose(Composer composer, Payload<ConnectionContext> payload,
            Vane<ConnectionContext> next)
        {
            RequestContext request;
            if (payload.TryGet(out request))
            {
                Encoding encoding = GetEncoding(request);
            }

            // this is sad, because HttpListener does this by default
            // context.Request.ContentEncoding = encoding;

            // probably need to decorate the forms/body with a decoder to convert to the proper encoding
            next.Compose(composer, payload);
        }

        Encoding GetEncoding(RequestContext context)
        {
            string contentType = context.Headers["Content-Type"];
            if (contentType == null)
                return _settings.DefaultTextEncoding;

            Match match = _settings.CharSetPattern.Match(contentType);
            if (match.Success == false)
                return _settings.DefaultTextEncoding;

            return Encoding.GetEncoding(match.Groups[1].Value);
        }


        class TextEncodingSettings
        {
            public Regex CharSetPattern;
            public Encoding DefaultTextEncoding;
        }
    }
}