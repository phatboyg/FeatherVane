// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.RabbitMQIntegration.Tests
{
    using System.Collections;
    using System.Text;


    public static class TestExtensions
    {
        public static string FormatKeyValue(this IDictionary dictionary, string separator, string valueSeparator)
        {
            var builder = new StringBuilder();
            foreach (object key in dictionary.Keys)
            {
                object value = dictionary[key];
                var bytes = value as byte[];
                string text = bytes != null
                                  ? Encoding.UTF8.GetString(bytes)
                                  : value is Hashtable
                                        ? FormatKeyValue((IDictionary)value, "=", ", ")
                                        : value.ToString();
                builder.Append(key).Append(separator).Append(text).Append(valueSeparator);
            }

            return builder.ToString();
        }
    }
}