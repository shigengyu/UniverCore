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
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Univer.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly int DefaultBufferSize = 1;

        /// <summary>
        /// Reads until the specified pattern.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="includePattern">if set to <c>true</c> [include pattern].</param>
        /// <returns></returns>
        public static string ReadUntil(this Stream stream, string pattern, bool includePattern = true)
        {
            var sb = new StringBuilder();
            string compareString = null;

            while (true)
            {
                var c = (char) stream.ReadByte();
                sb.Append(c);

                if (compareString != null && compareString.Length == pattern.Length)
                {
                    compareString = compareString.Substring(1, compareString.Length - 1);
                }
                compareString += c;

                if (compareString == pattern)
                {
                    break;
                }
            }

            if (!includePattern)
            {
                sb = sb.Remove(sb.Length - pattern.Length, pattern.Length);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads the stream for the specified length of data as string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Read(this Stream stream, int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length cannot be equal or less than 0");
            }

            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Writes the specified string to the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Stream Write(this Stream stream, string value)
        {
            stream.Write(Encoding.UTF8.GetBytes(value), 0, value.Length);
            return stream;
        }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetLines(this Stream stream)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyTo(this Stream source, Stream target)
        {
            var reader = new StreamReader(source);
            var writer = new StreamWriter(target);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);
            }
            writer.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class StreamExtensionsTest
        {
            /// <summary>
            /// Sets up.
            /// </summary>
            [SetUp]
            public void SetUp()
            {
                Stream = new MemoryStream();

                var writer = new StreamWriter(Stream);
                writer.Write(("Hello, World!" + Environment.NewLine).Duplicate(10));
                writer.Flush();

                Stream.Seek(0, SeekOrigin.Begin);
            }

            /// <summary>
            /// Tears down.
            /// </summary>
            [TearDown]
            public void TearDown()
            {
                Stream.Dispose();
            }

            private Stream Stream { get; set; }

            /// <summary>
            /// Tests the get lines.
            /// </summary>
            [Test]
            public void TestGetLines()
            {
                var lines = Stream.GetLines().ToList();
                Assert.AreEqual(10, lines.Count);
                Assert.True(lines.All(item => item == "Hello, World!"));
            }
        }
    }
}