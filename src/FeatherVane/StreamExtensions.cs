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
namespace FeatherVane
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
        internal static Task WriteAsync(this Stream stream, byte[] buffer, int offset, int count,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return TaskUtil.Cancelled();

            var source = new TaskCompletionSource<object>();

            AsyncCallback complete = asyncResult =>
                {
                    try
                    {
                        stream.EndWrite(asyncResult);
                        source.SetResult(null);
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
                        source.SetException(ex);
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

            return source.Task;
        }
    }
}