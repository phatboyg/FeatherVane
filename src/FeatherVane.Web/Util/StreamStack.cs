namespace FeatherVane.Web.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Remoting;
    public class StreamStack :
        Stream
    {
        Stack<Stream> _streams;
        bool _leaveOriginalStreamOpen;


        public StreamStack(Stream stream)
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