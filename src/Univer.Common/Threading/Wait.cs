// The MIT License (MIT)
// 
// Copyright (c) 2012-2013 Univer Shi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Univer.Common.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class Wait
    {
        /// <summary>
        /// Non-blocking wait until the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns></returns>
        public static bool Until(Func<bool> condition, int millisecondsTimeout = Timeout.Infinite)
        {
            var startTick = Environment.TickCount;

            while (!condition())
            {
                if (millisecondsTimeout != Timeout.Infinite)
                {
                    if (TimeSpan.FromTicks(Environment.TickCount - startTick).TotalMilliseconds >= millisecondsTimeout)
                    {
                        return false;
                    }
                }

                Thread.Sleep(1);
            }

            return true;
        }

        [TestFixture]
        private class WaitTest
        {
            [Test]
            public void TestWait()
            {
                const int sleepDuration = 3000;

                var start = DateTime.Now;
                var condition = false;
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    Thread.Sleep(sleepDuration);
                    condition = true;
                }, null);
                Until(() => condition);

                var timeSpan = DateTime.Now - start;
                Assert.IsTrue(timeSpan.TotalMilliseconds.Between(sleepDuration, sleepDuration + 100));
            }
        }
    }
}