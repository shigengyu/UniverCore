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
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T> where T : new()
    {
        private BinaryTreeNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether [has value].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has value]; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue
        {
            get { return !Equals(Value, default(T)); }
        }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public BinaryTreeNode<T> Left { get; set; }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public BinaryTreeNode<T> Right { get; set; }

        /// <summary>
        /// Binaries the tree node.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static implicit operator BinaryTreeNode<T>(T value)
        {
            return new BinaryTreeNode<T>(value);
        }

        /// <summary>
        /// Ts the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static implicit operator T(BinaryTreeNode<T> node)
        {
            return node.Value;
        }

        /// <summary>
        /// Withes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static BinaryTreeNode<T> With(T value, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            return new BinaryTreeNode<T>(value)
            {
                Left = left,
                Right = right
            };
        }

        /// <summary>
        /// Withes the left.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="left">The left.</param>
        /// <returns></returns>
        public static BinaryTreeNode<T> WithLeft(T value, BinaryTreeNode<T> left)
        {
            return new BinaryTreeNode<T>(value) { Left = left };
        }

        /// <summary>
        /// Withes the right.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static BinaryTreeNode<T> WithRight(T value, BinaryTreeNode<T> right)
        {
            return new BinaryTreeNode<T>(value) { Right = right };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class BinaryTreeTests
    {
        /// <summary>
        /// Tests the create tree.
        /// </summary>
        [Test]
        public void TestCreateTree()
        {
            var root = BinaryTreeNode<int>.With(1,
                BinaryTreeNode<int>.With(2, 3, 4),
                BinaryTreeNode<int>.WithRight(5, 6));

            Assert.AreEqual(1, root);
            Assert.AreEqual(2, root.Left);
            Assert.AreEqual(3, root.Left.Left);
            Assert.AreEqual(4, root.Left.Right);
            Assert.AreEqual(5, root.Right);
            Assert.AreEqual(6, root.Right.Right);
        }
    }
}