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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified member information is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static bool IsDefined<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            return memberInfo.IsDefined(typeof (TAttribute), inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MemberInfo memberInfo,
            bool inherit = true) where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof (TAttribute), inherit).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <param name="throwIfNotExist">if set to <c>true</c> [throw if not exist].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Cardinality mismatch for [{0}]. Expected count = [{1}]. Actual count = [{2}].FormatWith(
        ///                         typeof(TAttribute).FullName,
        ///                         1,
        ///                         attributes.Count())</exception>
        /// <exception cref="AttributeNotFoundException"></exception>
        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true,
            bool throwIfNotExist = false) where TAttribute : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes<TAttribute>(inherit).ToList();

            if (attributes.Count() > 1)
            {
                throw new InvalidOperationException(
                    "Cardinality mismatch for [{0}]. Expected count = [{1}]. Actual count = [{2}]".FormatWith(
                        typeof (TAttribute).FullName,
                        1,
                        attributes.Count()));
            }

            var attribute = attributes.SingleOrDefault();

            if (attribute == null && throwIfNotExist)
            {
                throw new AttributeNotFoundException(typeof (TAttribute).FullName, memberInfo.Name);
            }

            return attribute;
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns></returns>
        public static string GetFullName(this MemberInfo memberInfo)
        {
            return memberInfo.DeclaringType + "." + memberInfo.Name;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="target">The target object.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="System.InvalidOperationException">Cannot assign value</exception>
        public static void SetPropertyValue(this PropertyInfo property, object target, object value,
            object[] index = null)
        {
            // Source value is null or DBNull, assign null value directly.
            // If property is value type, default value will be assigned.
            if (value == null || Convert.IsDBNull(value))
            {
                property.SetValue(target, null, index);
            }
            else
            {
                var valueType = value.GetType();
                if (valueType.IsNullable())
                {
                    valueType = Nullable.GetUnderlyingType(valueType);
                }

                var propertyType = property.PropertyType;
                if (propertyType.IsNullable())
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                // If directly assignable, assign the value.
                if (propertyType == valueType || propertyType.IsAssignableFrom(valueType))
                {
                    property.SetValue(target, value, index);
                    return;
                }

                // Check if the value can be assigned via type conversion.
                if (valueType.CanAssignToWithConversion(propertyType))
                {
                    // If both source and target types are IConvertable, convert directly.
                    if (new[] { propertyType, valueType }.All(item => typeof (IConvertible).IsAssignableFrom(item)))
                    {
                        property.SetValue(target, Convert.ChangeType(value, propertyType), index);
                        return;
                    }

                    // Check if proper type converter is registered. If yes, convert it and assign value.
                    var typeConverter = TypeDescriptor.GetConverter(valueType);
                    if (typeConverter.CanConvertFrom(valueType))
                    {
                        property.SetValue(target, typeConverter.ConvertFrom(value), index);
                        return;
                    }
                }

                throw new InvalidOperationException("Cannot assign value of type [{0}] to [{1}.{2}] of type [{3}]"
                    .FormatWith(
                        value.GetType().FullName,
                        property.DeclaringType.FullName,
                        property.Name,
                        propertyType.FullName));
            }
        }
    }
}