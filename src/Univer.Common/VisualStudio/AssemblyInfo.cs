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
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Univer.Common.IO;

namespace Univer.Common.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyInfo : IDisposable
    {
        private static readonly Regex AssemblyInfoFieldRegex = new Regex("(?<=\\[assembly: Assembly)\\w+(?=\\()");

        private readonly FileContentReplacer _replacer;
        private string _configuration;
        private string _copyright;
        private string _culture;
        private string _description;
        private string _product;

        //
        // AssemblyInfo metadata fields
        //
        private string _title;
        private string _trademark;
        private string _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public AssemblyInfo(string fileName)
        {
            FileName = fileName;
            _replacer = new FileContentReplacer(FileName);

            ParseAssemblyInfo();
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                SetAssemblyFieldValue("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                SetAssemblyFieldValue("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public string Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                SetAssemblyFieldValue("Configuration", value);
            }
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Product
        {
            get { return _product; }
            set
            {
                _product = value;
                SetAssemblyFieldValue("Product", value);
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public string Copyright
        {
            get { return _copyright; }
            set
            {
                _copyright = value;
                SetAssemblyFieldValue("Copyright", value);
            }
        }

        /// <summary>
        /// Gets or sets the trademark.
        /// </summary>
        /// <value>
        /// The trademark.
        /// </value>
        public string Trademark
        {
            get { return _trademark; }
            set
            {
                _trademark = value;
                SetAssemblyFieldValue("Trademark", value);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public string Culture
        {
            get { return _culture; }
            set
            {
                _culture = value;
                SetAssemblyFieldValue("Culture", value);
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                SetAssemblyFieldValue("Version", value);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _replacer.Reset();
            _replacer.Dispose();
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        public void Reload()
        {
            ParseAssemblyInfo();
        }

        /// <summary>
        /// Updates the file.
        /// </summary>
        public void UpdateFile()
        {
            _replacer.Flush();
        }

        private void ParseAssemblyInfo()
        {
            using (var stream = File.OpenRead(FileName))
            {
                foreach (var line in stream.GetLines())
                {
                    if (AssemblyInfoFieldRegex.IsMatch(line))
                    {
                        var assemblyInfoField = AssemblyInfoFieldRegex.Match(line).Value;
                        var value = GetAssemblyInfoFieldValueRegex(assemblyInfoField).Match(line).Value;

                        var fieldName = "_" + char.ToLower(assemblyInfoField[0]) + assemblyInfoField.Substring(1);
                        var fieldInfo = GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo != null)
                        {
                            fieldInfo.SetValue(this, value);
                        }
                    }
                }

                stream.Close();
            }
        }

        private void SetAssemblyFieldValue(string field, string value)
        {
            _replacer.Replace(GetAssemblyInfoFieldValueRegex(field), value);
        }

        private Regex GetAssemblyInfoFieldValueRegex(string field)
        {
            return new Regex("(?<=\\[assembly: Assembly{0}\\(\\\").*(?=\\\")".FormatWith(field));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class AssemblyInfoTest
        {
            /// <summary>
            /// Sets up.
            /// </summary>
            [SetUp]
            public void SetUp()
            {
                var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent;
                if (directoryInfo != null)
                {
                    if (directoryInfo.Parent != null)
                    {
                        var solutionDirectory = directoryInfo.Parent.FullName;
                        var assemblyInfoFileName = Path.Combine(solutionDirectory, "Univer.Common", "Properties",
                            "AssemblyInfo.cs");

                        FileName = Path.GetTempFileName();
                        File.Copy(assemblyInfoFileName, FileName, true);
                    }
                }
                File.SetAttributes(FileName, FileAttributes.Normal);
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
            /// Tests the parse.
            /// </summary>
            [Test]
            public void TestParse()
            {
                var assemblyInfo = new AssemblyInfo(FileName);
                Assert.AreEqual("Univer.Common", assemblyInfo.Title);
            }

            /// <summary>
            /// Tests the update.
            /// </summary>
            [Test]
            public void TestUpdate()
            {
                const string newVersion = "1.2.3.4";

                using (var assemblyInfo = new AssemblyInfo(FileName))
                {
                    var oldVersion = assemblyInfo.Version;

                    assemblyInfo.Version = newVersion;
                    assemblyInfo.UpdateFile();

                    Assert.AreEqual(newVersion, assemblyInfo.Version);

                    assemblyInfo.Reload();
                    Assert.AreEqual(newVersion, assemblyInfo.Version);

                    assemblyInfo.Version = oldVersion;
                    assemblyInfo.UpdateFile();
                    Assert.AreEqual(oldVersion, assemblyInfo.Version);

                    assemblyInfo.Reload();
                    Assert.AreEqual(oldVersion, assemblyInfo.Version);
                }
            }
        }
    }
}