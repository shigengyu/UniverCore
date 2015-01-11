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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Univer.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class FileContentReplacer : IDisposable
    {
        private readonly ICollection<ReplaceRule> _rules = new List<ReplaceRule>();
        private bool _isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileContentReplacer"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileContentReplacer(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDirty)
                Flush();
        }

        /// <summary>
        /// Replaces the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public FileContentReplacer Replace(string regex, string replacement)
        {
            return Replace(new Regex(regex), replacement);
        }

        /// <summary>
        /// Replaces the specified regex.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public FileContentReplacer Replace(Regex regex, string replacement)
        {
            if (_rules.All(item => item.Regex.ToString() != regex.ToString()))
            {
                _rules.Add(new ReplaceRule { Regex = regex, Replacement = replacement });
            }
            else
            {
                _rules.Single(item => item.Regex.ToString() == regex.ToString()).Replacement = replacement;
            }

            _isDirty = true;

            return this;
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            var buffer = new StringBuilder();

            using (var reader = new StreamReader(FileName))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    foreach (var rule in _rules)
                    {
                        if (rule.Regex.IsMatch(line))
                        {
                            line = rule.Regex.Replace(line, rule.Replacement);
                        }
                    }

                    buffer.AppendLine(line);
                }
            }

            using (var writer = new StreamWriter(FileName))
            {
                writer.Write(buffer.ToString());
            }

            _isDirty = false;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _rules.Clear();
            _isDirty = false;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class FileContentReplacerTest
        {
            /// <summary>
            /// Sets up.
            /// </summary>
            [SetUp]
            public void SetUp()
            {
                FileName = Path.GetTempFileName();

                using (var writer = new StreamWriter(FileName))
                {
                    writer.Write(("Hello, World!" + Environment.NewLine).Duplicate(10));
                    writer.Close();
                }
            }

            /// <summary>
            /// Tears down.
            /// </summary>
            [TearDown]
            public void TearDown()
            {
                File.Delete(FileName);
            }

            private string FileName { get; set; }

            /// <summary>
            /// Tests the multiple replace.
            /// </summary>
            [Test]
            public void TestMultipleReplace()
            {
                using (var replacer = new FileContentReplacer(FileName))
                {
                    replacer.Replace("World", "Univer").Replace("Univer", "Jessie");
                }

                Assert.True(File.OpenRead(FileName).GetLines().All(item => item == "Hello, Jessie!"));
            }

            /// <summary>
            /// Tests the single replace.
            /// </summary>
            [Test]
            public void TestSingleReplace()
            {
                using (var replacer = new FileContentReplacer(FileName))
                {
                    replacer.Replace("World", "Univer");
                }

                Assert.True(File.OpenRead(FileName).GetLines().All(item => item == "Hello, Univer!"));
            }
        }

        private class ReplaceRule
        {
            public Regex Regex { get; internal set; }
            public string Replacement { get; internal set; }
        }
    }
}