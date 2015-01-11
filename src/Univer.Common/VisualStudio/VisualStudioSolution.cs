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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Univer.Common.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class VisualStudioSolution
    {
        /// <summary>
        /// The solution version prefix
        /// </summary>
        public const string SolutionVersionPrefix = "Microsoft Visual Studio Solution File, Format Version ";

        /// <summary>
        /// The project regex
        /// </summary>
        public static readonly Regex ProjectRegex = new Regex("Project.*= \"(.*)\", \"(.*)\", \"");

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStudioSolution"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public VisualStudioSolution(string fileName)
        {
            FileName = fileName;
            Projects = new List<VisualStudioProject>();
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory
        {
            get { return new FileInfo(FileName).DirectoryName; }
        }

        /// <summary>
        /// Gets the format version.
        /// </summary>
        /// <value>
        /// The format version.
        /// </value>
        public string FormatVersion { get; private set; }

        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>
        /// The projects.
        /// </value>
        public ICollection<VisualStudioProject> Projects { get; set; }

        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <returns></returns>
        public VisualStudioSolution Parse()
        {
            using (var reader = new StreamReader(FileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (line.StartsWith(SolutionVersionPrefix))
                    {
                        FormatVersion = line.Substring(SolutionVersionPrefix.Length).Trim();
                    }
                    else if (ProjectRegex.IsMatch(line))
                    {
                        var groups = ProjectRegex.Match(line).Groups;
                        if (groups.Count == 3)
                        {
                            var projectName = groups[1].Value;
                            var projectFullFileName = Path.Combine(Directory, groups[2].Value);

                            if (!File.Exists(projectFullFileName))
                                continue;

                            var projectDirectory =
                                new FileInfo(Path.Combine(Directory, projectFullFileName)).DirectoryName;

                            Projects.Add(
                                new VisualStudioProject(projectName, projectDirectory, projectFullFileName).Parse());
                        }
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class VisualStudioSolutionTest
        {
            /// <summary>
            /// Tests the parse solution.
            /// </summary>
            [Test]
            public void TestParseSolution()
            {
                var directory = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
                var solutionFileName = Path.Combine(directory, "UniverCore.sln");
                Assert.True(File.Exists(solutionFileName));

                var solution = new VisualStudioSolution(solutionFileName).Parse();
                Assert.GreaterOrEqual(solution.Projects.Count(), 1);
            }
        }
    }
}