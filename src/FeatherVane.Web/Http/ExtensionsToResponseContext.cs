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
    using System.IO;
    using System.Text;

    public static class ExtensionsToResponseContext
    {
        public static void SetJsonContentType(this ResponseContext response)
        {
            response.ContentType = "application/json; charset=\"utf-8\"";
        }

        public static void SetHtmlContentType(this ResponseContext response)
        {
            response.ContentType = "text/html; charset=\"utf-8\"";
        }

        public static void WriteHtml(this ResponseContext response, string text)
        {
            const string header = @"<!DOCTYPE html><html>";
            const string footer = @"</html>";

            response.SetHtmlContentType();
            response.Write(header, text, footer);
        }

        public static void Write(this ResponseContext response, params string[] text)
        {
            using (var writer = new StreamWriter(response.BodyStream, Encoding.UTF8))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    string s = text[i];

                    if (!string.IsNullOrEmpty(s))
                        writer.Write(s);
                }
                writer.Flush();
            }
        }
    }
}