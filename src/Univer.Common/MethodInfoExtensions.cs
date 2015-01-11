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
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Gets the named parameters.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, ParameterInfo>> GetNamedParameters(this MethodInfo method)
        {
            foreach (var parameter in method.GetParameters())
            {
                var parameterName = parameter.Name;
                var attribute = parameter.GetCustomAttribute<ParameterAttribute>();
                if (attribute != null)
                {
                    parameterName = attribute.Name;
                }

                yield return new KeyValuePair<string, ParameterInfo>(parameterName, parameter);
            }
        }

        /// <summary>
        /// Gets the method signature.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="withNamespace">if set to <c>true</c> [with namespace].</param>
        /// <returns></returns>
        public static string GetMethodSignature(this MethodInfo method, bool withNamespace = false)
        {
            var sb = new StringBuilder();

            // Access level
            if (method.IsPublic)
            {
                sb.Append("public ");
            }
            else if (method.IsPrivate)
            {
                sb.Append("private ");
            }
            else if (method.IsFamily)
            {
                sb.Append("protected ");
            }
            else if (method.IsAssembly)
            {
                sb.Append("internal ");
            }
            else if (method.IsFamilyOrAssembly)
            {
                sb.Append("protected internal ");
            }

            // Member or static
            if (method.IsStatic)
                sb.Append("static ");

            // Abstract or virtual
            if (method.IsAbstract)
            {
                sb.Append("abstract ");
            }
            else if (method.IsVirtual)
            {
                sb.Append("virtual ");
            }

            // Return type
            sb.Append(method.ReturnType.GetDisplayName(withNamespace) + " ");

            // Method name
            sb.Append(method.Name);

            // Generic method arguments
            if (method.IsGenericMethod)
            {
                sb.Append("<");
                sb.Append(string.Join(", ",
                    method.GetGenericArguments().Select(item => item.GetDisplayName(withNamespace))));
                sb.Append(">");
            }

            // Parameters
            sb.Append("(");
            sb.Append(string.Join(", ", method.GetParameters().Select(item => item.GetDisplayName(withNamespace))));
            sb.Append(")");

            return sb.ToString();
        }

        [TestFixture]
        private class MethodInfoExtensionsTest
        {
            private static readonly MethodInfo Test1MethodInfo = typeof (MethodInfoExtensionsTest).GetMethod("Test1",
                BindingFlags.Static | BindingFlags.Public);

            private static readonly MethodInfo Test2MethodInfo = typeof (MethodInfoExtensionsTest).GetMethod("Test2",
                BindingFlags.Static | BindingFlags.Public);

            private static readonly MethodInfo Test3MethodInfo = typeof (MethodInfoExtensionsTest).GetMethod("Test3",
                BindingFlags.Instance | BindingFlags.NonPublic);

            private static readonly MethodInfo Test4MethodInfo = typeof (MethodInfoExtensionsTest).GetMethod("Test4",
                BindingFlags.Static | BindingFlags.Public);

            /// <summary>
            /// Test1
            /// </summary>
            /// <param name="parameter">The parameter.</param>
            /// <returns></returns>
            /// <exception cref="System.NotImplementedException"></exception>
// ReSharper disable once UnusedMember.Local
            public static Func<string, int> Test1([Parameter("Test")] string parameter)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Test2
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="parameter">The parameter.</param>
            /// <exception cref="System.NotImplementedException"></exception>
// ReSharper disable once UnusedMember.Local
            public static void Test2<T>(string parameter)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Test3
            /// </summary>
            /// <typeparam name="T1">The type of the 1.</typeparam>
            /// <typeparam name="T2">The type of the 2.</typeparam>
            /// <typeparam name="T3">The type of the 3.</typeparam>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <param name="parameter1">The parameter1.</param>
            /// <param name="parameter2">The parameter2.</param>
            /// <param name="parameter3">The parameter3.</param>
            /// <returns></returns>
            /// <exception cref="System.NotImplementedException"></exception>
            protected internal virtual TResult Test3<T1, T2, T3, TResult>(T1 parameter1, T2 parameter2, T3 parameter3)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Test4
            /// </summary>
            /// <param name="parameter">The parameter.</param>
            /// <exception cref="System.NotImplementedException"></exception>
// ReSharper disable once UnusedMember.Local
            public static void Test4([Dummy(BindingFlags = BindingFlags.Public | BindingFlags.Static)] int parameter)
            {
                throw new NotImplementedException();
            }

            [AttributeUsage(AttributeTargets.Parameter)]
            private class DummyAttribute : Attribute
            {
                public BindingFlags BindingFlags { get; set; }
            }

            [Test]
            public void TestGetMethodSignature()
            {
                Assert.AreEqual(
                    "public static Func<string, int> Test1([Parameter(Name = \"Test\", Optional = true)] string parameter)",
                    Test1MethodInfo.GetMethodSignature());

                Assert.AreEqual("public static void Test2<T>(string parameter)",
                    Test2MethodInfo.GetMethodSignature());

                Assert.AreEqual(
                    "protected internal virtual TResult Test3<T1, T2, T3, TResult>(T1 parameter1, T2 parameter2, T3 parameter3)",
                    Test3MethodInfo.GetMethodSignature());

                Assert.AreEqual(
                    "public static void Test4([MethodInfoExtensions.MethodInfoExtensionsTest.Dummy(BindingFlags = BindingFlags.Static | BindingFlags.Public)] int parameter)",
                    Test4MethodInfo.GetMethodSignature());

                Assert.AreEqual(
                    "public static void Test4([Univer.Common.MethodInfoExtensions.MethodInfoExtensionsTest.Dummy(BindingFlags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)] System.Int32 parameter)",
                    Test4MethodInfo.GetMethodSignature(true));
            }
        }
    }
}