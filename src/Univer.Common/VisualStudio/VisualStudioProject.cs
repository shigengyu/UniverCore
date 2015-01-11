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
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Univer.Common.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class VisualStudioProject
    {
        /// <summary>
        /// The inner items
        /// </summary>
        protected readonly List<VisualStudioProjectItem> InnerItems = new List<VisualStudioProjectItem>();

        /// <summary>
        /// The inner property groups
        /// </summary>
        protected readonly List<VisualStudioProjectPropertyGroup> InnerPropertyGroups =
            new List<VisualStudioProjectPropertyGroup>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStudioProject" /> class.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectDirectory">The project directory.</param>
        /// <param name="projectFileFullName">Full name of the project file.</param>
        public VisualStudioProject(string projectName, string projectDirectory, string projectFileFullName)
        {
            ProjectName = projectName;
            ProjectDirectory = projectDirectory;
            ProjectFileFullName = projectFileFullName;
        }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Gets the project directory.
        /// </summary>
        /// <value>
        /// The project directory.
        /// </value>
        public string ProjectDirectory { get; private set; }

        /// <summary>
        /// Gets the full name of the project file.
        /// </summary>
        /// <value>
        /// The full name of the project file.
        /// </value>
        public string ProjectFileFullName { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ReadOnlyCollection<VisualStudioProjectItem> Items
        {
            get { return new ReadOnlyCollection<VisualStudioProjectItem>(InnerItems.ToList()); }
        }

        /// <summary>
        /// Gets or sets the property groups.
        /// </summary>
        /// <value>
        /// The property groups.
        /// </value>
        public ReadOnlyCollection<VisualStudioProjectPropertyGroup> PropertyGroups
        {
            get { return new ReadOnlyCollection<VisualStudioProjectPropertyGroup>(InnerPropertyGroups.ToList()); }
        }

        /// <summary>
        /// Parses this project file.
        /// </summary>
        /// <returns></returns>
        public VisualStudioProject Parse()
        {
            var document = XDocument.Load(ProjectFileFullName);
            if (document.Root != null)
            {
                ParseItemGroups(document.Root.Elements().Where(item => item.Name.LocalName == "ItemGroup"));
                ParsePropertyGroups(document.Root.Elements().Where(item => item.Name.LocalName == "PropertyGroup"));
            }
            else
            {
                throw new Exception(string.Format("Failed to parse project file [{0}] as XML", ProjectFileFullName));
            }

            return this;
        }

        /// <summary>
        /// Parses the item groups.
        /// </summary>
        /// <param name="itemGroups">The item groups.</param>
        private void ParseItemGroups(IEnumerable<XElement> itemGroups)
        {
            foreach (var itemGroup in itemGroups)
            {
                InnerItems.AddRange(itemGroup.Elements().Select(VisualStudioProjectItem.Parse));
            }
        }

        /// <summary>
        /// Parses the property groups.
        /// </summary>
        /// <param name="propertyGroups">The property groups.</param>
        private void ParsePropertyGroups(IEnumerable<XElement> propertyGroups)
        {
            var groups =
                propertyGroups.Select(VisualStudioProjectPropertyGroup.Parse)
                    .Where(item => item.IsConfigurationGroup)
                    .ToList();
            var baseConfigurationGroup = groups.SingleOrDefault(item => item.IsBaseConfigurationGroup);
            groups.Where(item => !item.IsBaseConfigurationGroup).ForEach(item => item.MergeWith(baseConfigurationGroup));

            InnerPropertyGroups.AddRange(groups);
        }
    }
}