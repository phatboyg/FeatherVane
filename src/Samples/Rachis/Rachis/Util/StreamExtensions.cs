namespace Rachis.Util
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;


    static class StreamExtensions
    {
        /// <summary>
        /// Write the buffer to the stream asynchronously, returning a Task to use for completion
        /// </summary>
        /// <param name="stream">The target stream</param>
        /// <param name="buffer">The buffer to write</param>
        /// <param name="offset">The offset into the buffer</param>
        /// <param name="count">The count of bytes to write</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public static Task WriteAsync(this Stream stream, byte[] buffer, int offset, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var source = new TaskCompletionSource<bool>();
            if (cancellationToken.IsCancellationRequested)
                source.TrySetCanceled();
            else
            {
                AsyncCallback complete = asyncResult =>
                    {
                        try
                        {
                            stream.EndWrite(asyncResult);
                            source.SetResult(true);
                        }
                        catch (OperationCanceledException ex)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                source.TrySetCanceled();
                            else
                                source.SetException(ex);
                        }
                        catch (Exception ex)
                        {
                            source.TrySetException(ex);
                        }
                    };

                AsyncCallback callback = asyncResult =>
                    {
                        if (asyncResult.CompletedSynchronously)
                            return;

                        complete(asyncResult);
                    };


                IAsyncResult result = stream.BeginWrite(buffer, offset, count, callback, null);
                if (result.CompletedSynchronously)
                    complete(result);
            }
            return source.Task;
        }
    }
}