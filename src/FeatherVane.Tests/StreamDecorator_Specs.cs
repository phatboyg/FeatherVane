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
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Remoting;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class StreamDecorator_Specs
    {
        [Test]
        public void FirstTestName()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streams = new StreamDecorator(memoryStream))
                {
                    streams.Push(x => new GZipStream(x, CompressionMode.Compress, true));

                    var bytes = Encoding.UTF8.GetBytes(new string('-', 1000));
                    streams.Write(bytes, 0, bytes.Length);
                }

                Console.WriteLine("Bytes Used: {0}", memoryStream.ToArray().Length);
            }
        }

        public class StreamDecorator :
            Stream
        {
            Stack<Stream> _streams;
            bool _leaveOriginalStreamOpen;


            public StreamDecorator(Stream stream)
            {
                _streams = new Stack<Stream>();
                _streams.Push(stream);

                _leaveOriginalStreamOpen = true;
            }

            public override bool CanRead
            {
                get { return _streams.Peek().CanRead; }
            }

            public override bool CanSeek
            {
                get { return _streams.Peek().CanSeek; }
            }

            public override bool CanTimeout
            {
                get { return _streams.Peek().CanTimeout; }
            }

            public override bool CanWrite
            {
                get { return _streams.Peek().CanWrite; }
            }

            public override long Length
            {
                get { return _streams.Peek().Length; }
            }

            public override long Position
            {
                get { return _streams.Peek().Position; }
                set { _streams.Peek().Position = value; }
            }

            public override int ReadTimeout
            {
                get { return _streams.Peek().ReadTimeout; }
                set { _streams.Peek().ReadTimeout = value; }
            }

            public override int WriteTimeout
            {
                get { return _streams.Peek().WriteTimeout; }
                set { _streams.Peek().WriteTimeout = value; }
            }

            public void Push(Func<Stream, Stream> decorator)
            {
                Stream next = decorator(_streams.Peek());

                _streams.Push(next);
            }

            protected override void Dispose(bool disposing)
            {
                while (_streams.Count > 0)
                {
                    Stream stream = _streams.Pop();

                    if (_leaveOriginalStreamOpen && _streams.Count == 0)
                        break;

                    stream.Close();
                    stream.Dispose();
                }

                base.Dispose(disposing);
            }

            public override object InitializeLifetimeService()
            {
                return _streams.Peek().InitializeLifetimeService();
            }

            public override ObjRef CreateObjRef(Type requestedType)
            {
                return _streams.Peek().CreateObjRef(requestedType);
            }

            public void CopyTo(Stream destination)
            {
                _streams.Peek().CopyTo(destination);
            }

            public override void Close()
            {
                _streams.Peek().Close();
            }

            public override void Flush()
            {
                _streams.Peek().Flush();
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback,
                object state)
            {
                return _streams.Peek().BeginRead(buffer, offset, count, callback, state);
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                return _streams.Peek().EndRead(asyncResult);
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
                object state)
            {
                return _streams.Peek().BeginWrite(buffer, offset, count, callback, state);
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                _streams.Peek().EndWrite(asyncResult);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _streams.Peek().Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _streams.Peek().SetLength(value);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _streams.Peek().Read(buffer, offset, count);
            }

            public override int ReadByte()
            {
                return _streams.Peek().ReadByte();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _streams.Peek().Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                _streams.Peek().WriteByte(value);
            }
        }
    }
}