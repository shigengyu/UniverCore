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
using System.Text;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class AttributeExtensions
    {
        private const string AttributeSuffix = "Attribute";

        /// <summary>
        /// Gets the display name of the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="withNamespace">if set to <c>true</c> [with namespace].</param>
        /// <returns></returns>
        public static string GetDisplayName(this Attribute attribute, bool withNamespace = false)
        {
            var sb = new StringBuilder("[");
            var attributeName = attribute.GetType().GetDisplayName(withNamespace);
            if (attributeName.EndsWith(AttributeSuffix))
            {
                attributeName = attributeName.Substring(0, attributeName.Length - AttributeSuffix.Length);
            }

            var properties = attribute.GetType().GetProperties().Where(item => item.DeclaringType != typeof (Attribute));

            sb.Append(attributeName);
            sb.Append("(");
            sb.Append(string.Join(", ",
                properties.Select(
                    item => item.Name + " = " + GetValueString(item.GetValue(attribute, null), withNamespace)).ToArray()));
            sb.Append(")]");

            return sb.ToString();
        }

        private static string GetValueString(object value, bool withNamespace)
        {
            if (value is string)
            {
                return "\"" + value + "\"";
            }
            if (value.GetType() == typeof (Type))
            {
                return "typeof(" + ((Type) value).GetDisplayName(withNamespace) + ")";
            }
            if (value is bool)
            {
                return ((bool) value) ? "true" : "false";
            }
            if (value.GetType().IsEnum)
            {
                var enumValues =
                    value.ToString()
                        .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(item => item.SafeTrim());
                var enumName = value.GetType().GetDisplayName(withNamespace);
                return string.Join(" | ", enumValues.Select(item => enumName + "." + item).ToArray());
            }
            return value.ToString();
        }
    }
}