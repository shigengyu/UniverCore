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
using System.Reflection;
using System.Text;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParameterInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified parameter information is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="parameterInfo">The parameter information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static bool IsDefined<TAttribute>(this ParameterInfo parameterInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            return parameterInfo.IsDefined(typeof (TAttribute), inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="parameterInfo">The parameter information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this ParameterInfo parameterInfo,
            bool inherit = true) where TAttribute : Attribute
        {
            return parameterInfo.GetCustomAttributes(typeof (TAttribute), inherit).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="parameterInfo">The parameter information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Cardinality mismatch for [{0}]. Expected count = [{1}]. Actual count = [{2}].FormatWith(
        ///                         typeof(TAttribute).FullName,
        ///                         1,
        ///                         attributes.Count())</exception>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static TAttribute GetCustomAttribute<TAttribute>(this ParameterInfo parameterInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            var attributes = parameterInfo.GetCustomAttributes<TAttribute>(inherit).ToList();

            if (attributes.Count() > 1)
            {
                throw new InvalidOperationException(
                    "Cardinality mismatch for [{0}]. Expected count = [{1}]. Actual count = [{2}]".FormatWith(
                        typeof (TAttribute).FullName,
                        1,
                        attributes.Count()));
            }

            var attribute = attributes.SingleOrDefault();

            if (attribute == null)
            {
                throw new AttributeNotFoundException(typeof (TAttribute).FullName, parameterInfo.Name);
            }

            return attribute;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <param name="withNamespace">if set to <c>true</c> [with namespace].</param>
        /// <returns></returns>
        public static string GetDisplayName(this ParameterInfo parameterInfo, bool withNamespace)
        {
            var sb = new StringBuilder();

            foreach (var attribute in parameterInfo.GetCustomAttributes(true).Cast<Attribute>())
            {
                sb.Append(attribute.GetDisplayName(withNamespace));
                sb.Append(" ");
            }

            sb.Append(parameterInfo.ParameterType.GetDisplayName(withNamespace) + " " + parameterInfo.Name);

            return sb.ToString();
        }
    }
}