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
namespace FeatherVane.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using NUnit.Framework;


    [TestFixture]
    public class When_a_transaction_spans_threads
    {
        [Test]
        public void Should_properly_succeed()
        {
            Vane<int> vane = VaneFactory.New<int>(x =>
                {
                    x.Transaction();
                    x.Execute(payload => Console.WriteLine("Execute: {0}", Thread.CurrentThread.ManagedThreadId));
                    x.ExecuteTask(payload => Task.Factory.StartNew(() =>
                        {
                            using (TransactionScope scope = payload.CreateTransactionScope())
                            {
                                Console.WriteLine("ExecuteTask: {0}", Thread.CurrentThread.ManagedThreadId);

                                Assert.IsNotNull(Transaction.Current);
                                scope.Complete();
                            }
                        }));
                });

            vane.Execute(27);
        }

        [Test, Explicit("Too slow for regular test run")]
        public void Should_succeed_when_task_is_slow()
        {
            Vane<int> vane = VaneFactory.New<int>(x =>
                {
                    x.Transaction();
                    x.Execute(payload => Console.WriteLine("Execute: {0}", Thread.CurrentThread.ManagedThreadId));
                    x.ExecuteTask(payload => Task.Factory.StartNew(() =>
                        {
                            using (TransactionScope scope = payload.CreateTransactionScope())
                            {
                                Console.WriteLine("ExecuteTask: {0}", Thread.CurrentThread.ManagedThreadId);
                                Thread.SpinWait(5000);
                                scope.Complete();
                            }
                        }));
                });

            vane.Execute(27);
        }

        [Test]
        public void Should_properly_fail()
        {
            Vane<int> vane = VaneFactory.New<int>(x =>
                {
                    x.Transaction();
                    x.Execute(payload => Console.WriteLine("Execute: {0}", Thread.CurrentThread.ManagedThreadId));
                    x.ExecuteTask(payload => Task.Factory.StartNew(() =>
                        {
                            using (TransactionScope scope = payload.CreateTransactionScope())
                            {
                                Console.WriteLine("ExecuteTask: {0}", Thread.CurrentThread.ManagedThreadId);
                            }
                            Console.WriteLine("Exited Scope");
                        }));
                    x.Execute(payload => Console.WriteLine("Part 3"));
                });

            var exception = Assert.Throws<AggregateException>(() => vane.Execute(27));
            
            Assert.IsInstanceOf<TransactionAbortedException>(exception.InnerException);
        }

        [Test, Explicit("Too slow for regular test run")]
        public void Should_timeout()
        {
            Vane<int> vane = VaneFactory.New<int>(x =>
                {
                    x.Transaction(t => t.SetTimeout(TimeSpan.FromSeconds(1)));
                    x.Execute(payload => Console.WriteLine("Execute: {0}", Thread.CurrentThread.ManagedThreadId));
                    x.ExecuteTask(payload => Task.Factory.StartNew(() =>
                        {
                            using (TransactionScope scope = payload.CreateTransactionScope())
                            {
                                Console.WriteLine("ExecuteTask: {0}", Thread.CurrentThread.ManagedThreadId);
                                Thread.Sleep(2000);
                                scope.Complete();
                            }
                            Console.WriteLine("Exited Scope");
                        }));
                    x.Execute(payload => Console.WriteLine("Part 3"));
                });

            var exception = Assert.Throws<AggregateException>(() => vane.Execute(27));
            
            Assert.IsInstanceOf<TransactionAbortedException>(exception.InnerException);

            Console.WriteLine(exception.InnerException.Message);
        }

        [Test]
        public void Should_properly_handle_exception()
        {
            Vane<int> vane = VaneFactory.New<int>(x =>
                {
                    x.Transaction();
                    x.Execute(payload => Console.WriteLine("Execute: {0}", Thread.CurrentThread.ManagedThreadId));
                    x.ExecuteTask(payload => Task.Factory.StartNew(() =>
                        {
                            using (TransactionScope scope = payload.CreateTransactionScope())
                            {
                                Console.WriteLine("ExecuteTask: {0}", Thread.CurrentThread.ManagedThreadId);
                                scope.Complete();
                            }
                            throw new InvalidOperationException("This is a friendly boom");
                        }));
                });

            var exception = Assert.Throws<AggregateException>(() => vane.Execute(27));
            
            Assert.IsInstanceOf<InvalidOperationException>(exception.InnerException);

        }
    }
}