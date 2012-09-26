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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;

    /// <summary>
    /// Throws when an AgendaItem within an Agenda throws an exception, and includes any additional
    /// exceptions that may have been thrown by compensations
    /// </summary>
    [Serializable, DebuggerDisplay("Count = {InnerExceptionCount}")]
    public class AgendaExecutionException :
        Exception
    {
        const string DefaultMessage = "An exception occurred executing the Agenda";
        readonly Exception[] _innerExceptions;

        public AgendaExecutionException()
            : base(DefaultMessage)
        {
            _innerExceptions = new Exception[0];
        }

        public AgendaExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (innerException == null)
                throw new ArgumentNullException("innerException");

            _innerExceptions = new[] {innerException};
        }

        public AgendaExecutionException(IEnumerable<Exception> innerExceptions)
            : this(DefaultMessage, innerExceptions)
        {
        }

        public AgendaExecutionException(params Exception[] innerExceptions)
            : this(DefaultMessage, innerExceptions)
        {
        }

        public AgendaExecutionException(string message, IEnumerable<Exception> innerExceptions)
            : this(message, innerExceptions == null
                                ? null
                                : new List<Exception>(innerExceptions))
        {
        }

        public AgendaExecutionException(string message, params Exception[] innerExceptions)
            : this(message, (IList<Exception>)innerExceptions)
        {
        }

        AgendaExecutionException(string message, IList<Exception> innerExceptions)
            : base(message, innerExceptions != null && innerExceptions.Count > 0
                                ? innerExceptions[0]
                                : null)
        {
            if (innerExceptions == null)
                throw new ArgumentNullException("innerExceptions");

            var exceptions = new Exception[innerExceptions.Count];
            for (int i = 0; i < exceptions.Length; i++)
            {
                exceptions[i] = innerExceptions[i];

                if (exceptions[i] == null)
                    throw new ArgumentException(string.Format("A null exception was specified: innerExceptions[{0}]", i));
            }

            _innerExceptions = exceptions;
        }

        [SecurityCritical]
        protected AgendaExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            var innerExceptions = info.GetValue("InnerExceptions", typeof(Exception[])) as Exception[];
            if (innerExceptions == null)
                throw new SerializationException("The inner exceptions could not be deserialized");

            _innerExceptions = innerExceptions;
        }

        public IEnumerable<Exception> InnerExceptions
        {
            get { return _innerExceptions; }
        }

        public int InnerExceptionCount
        {
            get { return _innerExceptions.Length; }
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            base.GetObjectData(info, context);

            var innerExceptions = new Exception[_innerExceptions.Length];
            _innerExceptions.CopyTo(innerExceptions, 0);

            info.AddValue("InnerExceptions", innerExceptions, typeof(Exception[]));
        }

        public override Exception GetBaseException()
        {
            Exception baseException = this;
            AgendaExecutionException agendaExecutionException = this;
            while (agendaExecutionException != null && agendaExecutionException._innerExceptions.Length == 1)
            {
                baseException = baseException.InnerException;
                agendaExecutionException = baseException as AgendaExecutionException;
            }
            return baseException;
        }

        public void Each(Func<Exception, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            List<Exception> unhandledExceptions = null;
            for (int i = 0; i < _innerExceptions.Length; i++)
            {
                if (!predicate(_innerExceptions[i]))
                {
                    if (unhandledExceptions == null)
                        unhandledExceptions = new List<Exception>();

                    unhandledExceptions.Add(_innerExceptions[i]);
                }
            }

            if (unhandledExceptions != null)
            {
                throw new AgendaExecutionException(Message, unhandledExceptions);
            }
        }

        public AgendaExecutionException Flatten()
        {
            var exceptions = new List<Exception>();

            var executionExceptions = new List<AgendaExecutionException>();
            executionExceptions.Add(this);

            int nDequeueIndex = 0;
            while (executionExceptions.Count > nDequeueIndex)
            {
                Exception[] innerExceptions = executionExceptions[nDequeueIndex++]._innerExceptions;
                for (int i = 0; i < innerExceptions.Length; i++)
                {
                    Exception exception = innerExceptions[i];
                    if (exception == null)
                        continue;

                    var executionException = exception as AgendaExecutionException;
                    if (executionException != null)
                        executionExceptions.Add(executionException);
                    else
                        exceptions.Add(exception);
                }
            }

            return new AgendaExecutionException(Message, exceptions);
        }

        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString());

            for (int i = 0; i < _innerExceptions.Length; i++)
            {
                sb.AppendFormat("{0}<---{0}[{1}] {2}", Environment.NewLine, i, _innerExceptions[i]);
            }

            return sb.ToString();
        }
    }
}