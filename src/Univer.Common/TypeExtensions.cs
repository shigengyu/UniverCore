/*
 * Copyright (c) 2013-2015 Univer Shi
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the display name of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="withNamespace">if set to <c>true</c> [with namespace].</param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type, bool withNamespace = false)
        {
            StringBuilder sb = new StringBuilder();
            string typeName;

            if (type.IsGenericParameter)
            {
                typeName = type.Name;
            }
            else if (withNamespace)
            {
                typeName = type.Name == "Void" ? "void" : type.FullName;

                if (type.IsNested)
                {
                    typeName = typeName.Replace('+', '.');
                }
            }
            else
            {
                typeName = type.Name;

                switch (typeName)
                {
                    case "String": return "string";
                    case "Int32": return "int";
                    case "Int64": return "long";
                    case "Double": return "double";
                    case "Decimal": return "decimal";
                    case "Object": return "object";
                    case "Void": return "void";
                    default: break;
                }

                if (type.IsNested)
                {
                    typeName = type.DeclaringType.GetDisplayName(withNamespace) + "." + typeName;
                }
            }

            if (type.IsNullable())
            {
                return type.GetNullableInnerType().GetDisplayName(withNamespace) + "?";
            }
            else if (type.IsGenericType)
            {
                typeName = typeName.Substring(0, typeName.IndexOf('`'));
                var genericParameters = string.Join(", ", type.GetGenericArguments().Select(item => item.GetDisplayName(withNamespace)));

                return typeName + "<" + genericParameters + ">";
            }
            else
            {
                return typeName;
            }
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the inner type of the nullable type.
        /// </summary>
        /// <param name="type">The nullable type.</param>
        /// <returns></returns>
        public static Type GetNullableInnerType(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type) : null;
        }

        /// <summary>
        /// Gets the inner type if the specified type is nullable. If not, return the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Type UnwrapIfNullable(this Type type)
        {
            return type.IsNullable() ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// Gets the methods with attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.IsDefined<TAttribute>())
                {
                    yield return method;
                }
            }
        }

        /// <summary>
        /// Determines whether this instance [can assign to] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public static bool CanAssignTo(this Type type, Type targetType)
        {
            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the type can be assigned to the target type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type can be assigned to the target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanAssignTo<T>(this Type type)
        {
            return type.CanAssignTo(typeof(T));
        }

        /// <summary>
        /// Determines whether the type can be assigned to the target type with conversion.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        ///   <c>true</c> if the type can be assigned to the target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanAssignToWithConversion(this Type type, Type targetType)
        {
            var underlyingSourceType = type.UnwrapIfNullable();
            var underlyingTargetType = targetType.UnwrapIfNullable();

            return underlyingTargetType.IsAssignableFrom(underlyingSourceType)
                || underlyingSourceType.ImplicitlyConvertsTo(underlyingTargetType)
                || underlyingSourceType.ExplicitlyConvertsTo(underlyingTargetType)
                || new[] { underlyingSourceType, underlyingTargetType }.All(item => typeof(IConvertible).IsAssignableFrom(item));
        }

        /// <summary>
        /// Determines whether the type can be assigned to the target type with conversion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The target type.</param>
        /// <returns>
        ///   <c>true</c> if the type can be assigned to the target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanAssignToWithConversion<T>(this Type type)
        {
            return type.CanAssignToWithConversion(typeof(T));
        }

        /// <summary>
        /// Determines whether the type can be implicitly converted to the target type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns></returns>
        public static bool ImplicitlyConvertsTo(this Type type, Type targetType)
        {
            if (type == targetType)
                return true;

            return (from method in type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    where method.Name == "op_Implicit" &&
                    method.ReturnType == targetType
                    select method).Any()
                    ||
                    (from method in targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                     where method.Name == "op_Implicit" &&
                     method.GetParameters()[0].ParameterType == type &&
                     method.ReturnType == targetType
                     select method).Any();
        }

        /// <summary>
        /// Determines whether the type can be explicitly converted to the target type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns></returns>
        public static bool ExplicitlyConvertsTo(this Type type, Type targetType)
        {
            if (type == targetType)
                return true;

            return (from method in type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    where method.Name == "op_Explicit" &&
                    method.ReturnType == targetType
                    select method).Any()
                    ||
                    (from method in targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                     where method.Name == "op_Explicit" &&
                     method.GetParameters()[0].ParameterType == type &&
                     method.ReturnType == targetType
                     select method).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        private class TypeExtensionsTest
        {
            [Test]
            public void TestIsNullable()
            {
                Assert.IsTrue(typeof(int?).IsNullable());
                Assert.IsFalse(typeof(int).IsNullable());
                Assert.IsFalse(typeof(string).IsNullable());
            }

            [Test]
            public void TestGetTypeDisplayName()
            {
                Assert.AreEqual("string", typeof(string).GetDisplayName());
                Assert.AreEqual("int?", typeof(int?).GetDisplayName());
                Assert.AreEqual("Action<int>", typeof(Action<int>).GetDisplayName());
                Assert.AreEqual("Action<Action<Func<int, string, Action<double>>>>", typeof(Action<Action<Func<int, string, Action<double>>>>).GetDisplayName());
            }
        }
    }
}
