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
using System.Security.Cryptography;
using NUnit.Framework;

namespace Univer.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileInfoExtensions
    {
        private static readonly MD5CryptoServiceProvider Md5CryptoServiceProvider = new MD5CryptoServiceProvider();

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <returns></returns>
        public static string ComputeHash(this FileInfo fileInfo)
        {
            using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = Md5CryptoServiceProvider.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class FileInfoExtensionsTest
        {
            /// <summary>
            /// Tests the compute hash.
            /// </summary>
            [Test]
            public void TestComputeHash()
            {
                var tempFile = Path.GetTempFileName();
                try
                {
                    using (var writer = File.CreateText(tempFile))
                    {
                        writer.WriteLine("Hello, World!");
                    }

                    var hash = new FileInfo(tempFile).ComputeHash();
                    Assert.AreEqual("KbkzqNmg/O8K918XE/SUDg==", hash);
                }
                finally
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
        }
    }
}