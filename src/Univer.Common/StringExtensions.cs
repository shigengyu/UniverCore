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
using System.Text;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the with.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Safes the trim.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string SafeTrim(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Trim();
        }

        /// <summary>
        /// Duplicates the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Count must be greater than zero.</exception>
        public static string Duplicate(this string value, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count", "Count must be greater than zero.");

            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(value);
            }

            return sb.ToString();
        }
    }
}