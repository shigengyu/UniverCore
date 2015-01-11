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
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Changes the type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// Source type [{0}] does not implement IConvertible..FormatWith(sourceType.Name)
        /// or
        /// Target type [{0}] does not implement IConvertible..FormatWith(sourceType.Name)
        /// </exception>
        public static object ChangeType(object value, Type targetType)
        {
            if (value == null || DBNull.Value.Equals(value))
                return null;

            var sourceType = value.GetType();
            var sourceValue = value;

            if (sourceType == targetType || targetType.IsAssignableFrom(sourceType))
            {
                return value;
            }

            targetType = targetType.UnwrapIfNullable();

            object targetValue = null;

            if (!sourceType.CanAssignTo<IConvertible>())
            {
                throw new InvalidOperationException(
                    "Source type [{0}] does not implement IConvertible.".FormatWith(sourceType.Name));
            }

            if (!targetType.CanAssignTo<IConvertible>())
            {
                throw new InvalidOperationException(
                    "Target type [{0}] does not implement IConvertible.".FormatWith(sourceType.Name));
            }

            targetValue = Convert.ChangeType(sourceValue, targetType);

            return targetValue;
        }

        /// <summary>
        /// Changes the type with conversion.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static object ChangeTypeWithConversion(object value, Type targetType)
        {
            if (value == null || DBNull.Value.Equals(value))
            {
                return null;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        private class TypesTest
        {
            private class Foo
            {
                /// <summary>
                /// Gets or sets the identifier.
                /// </summary>
                /// <value>
                /// The identifier.
                /// </value>
                public int Id { get; set; }
            }

            [TypeConverter(typeof (FooToBarConverter))]
            private class Bar
            {
                /// <summary>
                /// Gets or sets the identifier.
                /// </summary>
                /// <value>
                /// The identifier.
                /// </value>
                public int Id { get; set; }
            }

            /// <summary>
            /// 
            /// </summary>
            private class FooToBarConverter : TypeConverter
            {
                /// <summary>
                /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
                /// </summary>
                /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
                /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
                /// <returns>
                /// true if this converter can perform the conversion; otherwise, false.
                /// </returns>
                public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                {
                    return sourceType == typeof (Foo);
                }

                /// <summary>
                /// Returns whether this converter can convert the object to the specified type, using the specified context.
                /// </summary>
                /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
                /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
                /// <returns>
                /// true if this converter can perform the conversion; otherwise, false.
                /// </returns>
                public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
                {
                    return destinationType == typeof (Bar);
                }

                /// <summary>
                /// Converts the given object to the type of this converter, using the specified context and culture information.
                /// </summary>
                /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
                /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
                /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
                /// <returns>
                /// An <see cref="T:System.Object" /> that represents the converted value.
                /// </returns>
                public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
                {
                    return new Bar { Id = ((Foo) value).Id };
                }

                /// <summary>
                /// Converts the given value object to the specified type, using the specified context and culture information.
                /// </summary>
                /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
                /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
                /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
                /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
                /// <returns>
                /// An <see cref="T:System.Object" /> that represents the converted value.
                /// </returns>
                public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                    Type destinationType)
                {
                    return new Bar { Id = ((Foo) value).Id };
                }
            }

            /// <summary>
            /// Tests the convert int to string.
            /// </summary>
            [Test]
            public void TestConvertIntToString()
            {
                const int source = 1;
                var target = ChangeType(source, typeof (string));
                Assert.AreEqual("1", target);
            }

            /// <summary>
            /// Tests the convert nullable int to string.
            /// </summary>
            [Test]
            public void TestConvertNullableIntToString()
            {
                int? source = null;
// ReSharper disable once ExpressionIsAlwaysNull
                var target = ChangeType(source, typeof (string));
                Assert.IsNull(target);

                source = 123;
                target = ChangeType(source, typeof (string));
                Assert.AreEqual("123", target);
            }

            [Test]
            public void TestConvertStringToDouble()
            {
                var source = "12.345";
                var target = ChangeType(source, typeof (double));
                Assert.AreEqual(12.345, target);
            }

            [Test]
            public void TestConvertStringToNullableInt()
            {
                string source = null;
// ReSharper disable once ExpressionIsAlwaysNull
                var target = ChangeType(source, typeof (int?));
                Assert.IsNull(target);

                source = "123";
                target = ChangeType(source, typeof (int?));
                Assert.IsNotNull(target);
                Assert.AreEqual(123, target);
            }
        }
    }
}