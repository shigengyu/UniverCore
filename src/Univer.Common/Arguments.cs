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
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Parses the specified string array into argument object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Argument name cannot be empty
        /// or
        /// Duplicated argument
        /// or
        /// Argument name must start with [-]
        /// or
        /// Argument list must be even number
        /// or
        /// Mandatory argument not specified
        /// </exception>
        public static T Parse<T>(string[] args) where T : new()
        {
            var nameValueDictionary = new Dictionary<string, string>();

            if (args.Length % 2 == 0)
            {
                for (var i = 0; i < args.Length; i += 2)
                {
                    var nameIndex = i;
                    var valueIndex = i + 1;

                    if (args[nameIndex].StartsWith("-"))
                    {
                        var name = args[i].Substring(1);
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            throw new ArgumentException("Argument name cannot be empty");
                        }

                        var value = args[valueIndex];
                        if (nameValueDictionary.ContainsKey(name))
                        {
                            throw new ArgumentException("Duplicated argument [{0}]".FormatWith(name));
                        }

                        nameValueDictionary.Add(name.ToLower(), value);
                    }
                    else
                    {
                        throw new ArgumentException("Argument name must start with [-]");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Argument list must be even number");
            }

            var obj = new T();
            var properties =
                typeof (T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties.Where(item => item.IsDefined<ArgumentAttribute>()))
            {
                var attribute = property.GetCustomAttribute<ArgumentAttribute>();
                var argumentName = attribute.Name;
                if (string.IsNullOrWhiteSpace(argumentName))
                {
                    argumentName = property.Name;
                }
                argumentName = argumentName.ToLower();

                if (nameValueDictionary.ContainsKey(argumentName))
                {
                    property.SetPropertyValue(obj, nameValueDictionary[argumentName], null);
                }
                else if (!nameValueDictionary.ContainsKey(argumentName) && !attribute.Optional)
                {
                    throw new ArgumentException("Mandatory argument [{0}] not specified".FormatWith(argumentName));
                }
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class ArgumentsTest
        {
            /// <summary>
            /// 
            /// </summary>
            public class MyArgument
            {
                /// <summary>
                /// Gets or sets the name.
                /// </summary>
                /// <value>
                /// The name.
                /// </value>
                [Argument]
                public string Name { get; set; }

                /// <summary>
                /// Gets or sets the age.
                /// </summary>
                /// <value>
                /// The age.
                /// </value>
                [Argument]
                public int Age { get; set; }
            }

            /// <summary>
            /// Tests the parse.
            /// </summary>
            [Test]
            public void TestParse()
            {
                var myArgument = Parse<MyArgument>(new[] { "-name", "Univer", "-age", "28" });
                Assert.AreEqual("Univer", myArgument.Name);
                Assert.AreEqual(28, myArgument.Age);
            }
        }
    }
}