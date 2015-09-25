using System;
using System.IO;
using System.Runtime.Remoting;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public class ReportableStream : Stream
    {
        private readonly Stream _stream;
        private readonly IProgress<double> _progress;
        private double _sent;

        public new object GetLifetimeService()
        {
            return _stream.GetLifetimeService();
        }

        public override object InitializeLifetimeService()
        {
            return _stream.InitializeLifetimeService();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return _stream.CreateObjRef(requestedType);
        }

        public new void CopyTo(Stream destination)
        {
            _stream.CopyTo(destination);
        }

        public new void CopyTo(Stream destination, int bufferSize)
        {
            _stream.CopyTo(destination, bufferSize);
        }

        public override void Close()
        {
            _stream.Close();
        }

        public new void Dispose()
        {
            _stream.Dispose();
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            int bytes = _stream.EndRead(asyncResult);
            if (_progress != null)
            {
                _progress.Report(((_sent += bytes) * 100) / _stream.Length);
            }

            return bytes;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return _stream.ReadByte();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return _stream.CanTimeout; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override int ReadTimeout
        {
            get { return _stream.ReadTimeout; }
            set { _stream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return _stream.WriteTimeout; }
            set { _stream.WriteTimeout = value; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ReportableStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _stream = stream;
            
        }
        public ReportableStream(Stream stream, IProgress<double> progress)
            : this(stream)
        {
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            _progress = progress;
        }
    }
}