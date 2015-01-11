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

namespace Univer.Common
{
    /// <summary>
    /// </summary>
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentAttribute" /> class.
        /// </summary>
        public ArgumentAttribute()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ArgumentAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="optional">if set to <c>true</c> [optional].</param>
        public ArgumentAttribute(string name, bool optional = false)
            : this(name)
        {
            Optional = optional;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ArgumentAttribute" /> is optional.
        /// </summary>
        /// <value>
        ///     <c>true</c> if optional; otherwise, <c>false</c>.
        /// </value>
        public bool Optional { get; set; }
    }
}