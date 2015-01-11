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
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Checks whether the value of this instance is between the two specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <param name="rangeStartInclusive">if set to <c>true</c> [range start inclusive].</param>
        /// <param name="rangeEndInclusive">if set to <c>true</c> [range end inclusive].</param>
        /// <returns></returns>
        public static bool Between<T>(this T value, T rangeStart, T rangeEnd, bool rangeStartInclusive = true,
            bool rangeEndInclusive = true)
            where T : IComparable
        {
            var compareToStart = value.CompareTo(rangeStart);
            if (compareToStart < 0 || (compareToStart == 0 && !rangeStartInclusive))
                return false;

            var compareToEnd = value.CompareTo(rangeEnd);
            if (compareToEnd > 0 || (compareToEnd == 0 && !rangeEndInclusive))
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class ComparableExtensionsTest
        {
            /// <summary>
            /// Tests the between.
            /// </summary>
            [Test]
            public void TestBetween()
            {
                Assert.IsTrue(3.0.Between(2.99, 3.01));
                Assert.IsTrue(3.0.Between(3.0, 3.0));
                Assert.IsFalse(3.0.Between(3.0, 3.0, false, false));
                Assert.IsFalse(3.0.Between(3.0, 3.0, true, false));
                Assert.IsFalse(3.0.Between(3.0, 3.0, false));
            }
        }
    }
}