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
namespace FeatherVane.Tests
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
#if !NETFX_CORE
    using NUnit.Framework;
    using Web.Util;

#if !NETFX_CORE
    [TestFixture]
#else
    [TestClass]
#endif
    public class StreamDecorator_Specs
    {
#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void FirstTestName()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streams = new StreamStack(memoryStream))
                {
                    streams.Push(x => new GZipStream(x, CompressionMode.Compress, true));

                    byte[] bytes = Encoding.UTF8.GetBytes(new string('-', 1000));
                    streams.Write(bytes, 0, bytes.Length);
                }

                Console.WriteLine("Bytes Used: {0}", memoryStream.ToArray().Length);
            }
        }
    }
#endif
}