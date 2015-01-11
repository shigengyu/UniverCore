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
    public class AssemblyVersion
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version Version { get; private set; }

        /// <summary>
        /// Increments the major part.
        /// </summary>
        public void IncrementMajor()
        {
            Version = CreateVersion(Version.Major + 1, 0, 0, 0);
        }

        /// <summary>
        /// Increments the minor part.
        /// </summary>
        public void IncrementMinor()
        {
            Version = CreateVersion(Version.Major, Version.Minor + 1, 0, 0);
        }

        /// <summary>
        /// Increments the build part.
        /// </summary>
        public void IncrementBuild()
        {
            Version = CreateVersion(Version.Major, Version.Minor, Version.Build + 1, 0);
        }

        /// <summary>
        /// Increments the revision part.
        /// </summary>
        public void IncrementRevision()
        {
            Version = CreateVersion(Version.Minor, Version.Minor, Version.Build, Version.Revision + 1);
        }

        /// <summary>
        /// Creates the version.
        /// </summary>
        /// <param name="major">The major.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="build">The build.</param>
        /// <param name="revision">The revision.</param>
        /// <returns></returns>
        private static Version CreateVersion(int major, int minor, int build, int revision)
        {
            return Version.Parse(string.Join(".", Enumerables.With(major, minor, build, revision).ToArray()));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class AssemblyVersionTest
        {
            /// <summary>
            /// Creates the version.
            /// </summary>
            /// <returns></returns>
            public static Version CreateVersion()
            {
                return Version.Parse("1.2.3.4");
            }

            /// <summary>
            /// Tests the version.
            /// </summary>
            [Test]
            public void TestVersion()
            {
                var version = CreateVersion();
                Assert.AreEqual(1, version.Major);
                Assert.AreEqual(2, version.Minor);
                Assert.AreEqual(3, version.Build);
                Assert.AreEqual(4, version.Revision);
            }
        }
    }
}