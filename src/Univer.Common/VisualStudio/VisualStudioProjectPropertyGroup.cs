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
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Univer.Common.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("PropertyGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class VisualStudioProjectPropertyGroup
    {
        private static readonly Regex ConfigurationNameRegex = new Regex(@"(?<=\=\= ').*(?=\|)");

        /// <summary>
        /// Gets a value indicating whether [is configuration group].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is configuration group]; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfigurationGroup
        {
            get { return ConfigurationName != null; }
        }

        /// <summary>
        /// Gets a value indicating whether [is base configuration group].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is base configuration group]; otherwise, <c>false</c>.
        /// </value>
        public bool IsBaseConfigurationGroup
        {
            get { return Configuration != null && Configuration.Value != null; }
        }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        [XmlAttribute]
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public ConfigurationElement Configuration { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>
        /// The type of the output.
        /// </value>
        public string OutputType { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets the root namespace.
        /// </summary>
        /// <value>
        /// The root namespace.
        /// </value>
        public string RootNamespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the target framework version.
        /// </summary>
        /// <value>
        /// The target framework version.
        /// </value>
        public string TargetFrameworkVersion { get; set; }

        /// <summary>
        /// Gets the name of the configuration.
        /// </summary>
        /// <value>
        /// The name of the configuration.
        /// </value>
        public string ConfigurationName
        {
            get
            {
                if (Configuration != null && !string.IsNullOrEmpty(Configuration.Value))
                    return Configuration.Value;

                if (Condition != null && ConfigurationNameRegex.IsMatch(Condition))
                {
                    return ConfigurationNameRegex.Match(Condition).Value;
                }

                return null;
            }
        }

        /// <summary>
        /// Merges the with.
        /// </summary>
        /// <param name="baseConfiguration">The base configuration.</param>
        /// <returns></returns>
        public VisualStudioProjectPropertyGroup MergeWith(VisualStudioProjectPropertyGroup baseConfiguration)
        {
            this.MergePropertyValuesWith(baseConfiguration, "Configuration", "Condition");
            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "[" + ConfigurationName + "] " + AssemblyName;
        }

        /// <summary>
        /// Parses the specified x element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <returns></returns>
        public static VisualStudioProjectPropertyGroup Parse(XElement xElement)
        {
            var serializer = new XmlSerializer(typeof (VisualStudioProjectPropertyGroup));
            using (var stream = new MemoryStream())
            {
                var content = xElement.ToString();
                stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);

                stream.Seek(0, SeekOrigin.Begin);

                var result = (VisualStudioProjectPropertyGroup) serializer.Deserialize(stream);
                var name = result.ConfigurationName;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ConfigurationElement
        {
            /// <summary>
            /// Gets or sets the condition.
            /// </summary>
            /// <value>
            /// The condition.
            /// </value>
            [XmlAttribute]
            public string Condition { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            [XmlText]
            public string Value { get; set; }
        }
    }
}