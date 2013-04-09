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
namespace FeatherVane
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Taskell;


    public static class CompositionExtensions
    {
        public static Task Compose<T>(Vane<T> vane, Payload<T> payload, CancellationToken cancellationToken,
            bool runSynchronously = true)
        {
            var composer = new TaskComposer<T>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload);

            return composer.Finish();
        }

        public static Task Compose<T>(Feather<T> vane, Payload<T> payload, Vane<T> next,
            CancellationToken cancellationToken, bool runSynchronously = true)
        {
            var composer = new TaskComposer<T>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload, next);

            return composer.Finish();
        }

        /// <summary>
        /// Compose a source vane into a Task for execution
        /// </summary>
        /// <typeparam name="TSource">The source vane type</typeparam>
        /// <typeparam name="T">The vane type</typeparam>
        /// <param name="vane">The source vane</param>
        /// <param name="payload">The payload for the source</param>
        /// <param name="next">The vane after the source</param>
        /// <param name="cancellationToken"></param>
        /// <param name="runSynchronously"></param>
        /// <returns></returns>
        public static Task Compose<T, TSource>(SourceVane<TSource> vane, Payload<T> payload,
            Vane<Tuple<T, TSource>> next, CancellationToken cancellationToken, bool runSynchronously = true)
        {
            var composer = new TaskComposer<TSource>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload, next);

            return composer.Finish();
        }

        public static Task Compose<T, TSource>(SourceVane<T, TSource> vane, Payload<T> payload,
            Vane<Tuple<T, TSource>> next, CancellationToken cancellationToken, bool runSynchronously = true)
        {
            var composer = new TaskComposer<TSource>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload, next);

            return composer.Finish();
        }

        public static Task Compose<T>(Payload<T> payload, CancellationToken cancellationToken,
            Action<Composer, Payload<T>> composeCallback,
            bool runSynchronously = true)
        {
            var composer = new TaskComposer<T>(cancellationToken, runSynchronously);

            composeCallback(composer, payload);

            return composer.Finish();
        }

        public static Task ComposeTask<T>(this Composer composer, Vane<T> next, Payload<T> payload,
            bool runSynchronously = true)
        {
            return Compose(next, payload, composer.CancellationToken, runSynchronously);
        }

        public static Task ComposeTask<T, TSource>(this Composer composer, SourceVane<T, TSource> source,
            Payload<T> payload, Vane<Tuple<T, TSource>> next, bool runSynchronously = true)
        {
            return Compose(source, payload, next, composer.CancellationToken, runSynchronously);
        }

        public static Task ComposeTask<TSource, T>(this Composer composer, SourceVane<TSource> vane, Payload<T> payload,
            Vane<Tuple<T, TSource>> next, bool runSynchronously = true)
        {
            return Compose(vane, payload, next, composer.CancellationToken, runSynchronously);
        }

        public static Task ComposeTask<TPayload>(this Composer composer, Payload<TPayload> payload,
            Action<Composer, Payload<TPayload>> callback,
            bool runSynchronously = true)
        {
            return Compose(payload, composer.CancellationToken, callback, runSynchronously);
        }

        public static Task ComposeTask<TPayload>(this Composer composer, Feather<TPayload> feather,
            Payload<TPayload> payload, Vane<TPayload> next, bool runSynchronously = true)
        {
            return Compose(feather, payload, next, composer.CancellationToken, runSynchronously);
        }
    }
}