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
using System.Xml.Linq;

namespace Univer.Common.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class VisualStudioProjectItem
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="VisualStudioProjectItem"/> class from being created.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="include">The include.</param>
        private VisualStudioProjectItem(VisualStudioItemType type, string include)
        {
            Type = type;
            Include = include;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public VisualStudioItemType Type { get; private set; }

        /// <summary>
        /// Gets the include.
        /// </summary>
        /// <value>
        /// The include.
        /// </value>
        public string Include { get; private set; }

        /// <summary>
        /// Parses the specified item XML element.
        /// </summary>
        /// <param name="xElement">The item XML element.</param>
        /// <returns></returns>
        public static VisualStudioProjectItem Parse(XElement xElement)
        {
            VisualStudioItemType type;
            if (Enum.TryParse(xElement.Name.LocalName, out type))
            {
                return new VisualStudioProjectItem(type, xElement.Attribute("Include").Value);
            }
            return null;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "[" + Type + "] " + Include;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum VisualStudioItemType
    {
        /// <summary>
        /// Reference type
        /// </summary>
        Reference,

        /// <summary>
        /// Compile type
        /// </summary>
        Compile,

        /// <summary>
        /// None type
        /// </summary>
        None
    }
}