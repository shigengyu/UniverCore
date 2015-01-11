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
using System.Runtime.Serialization;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class ListDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListDictionary{TKey, TValue}"/> class.
        /// </summary>
        public ListDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected ListDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Determines whether [contains list item] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool ContainsListItem(TKey key, TValue value)
        {
            return ContainsKey(key) && this[key].Contains(value);
        }

        /// <summary>
        /// Adds the list item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddListItem(TKey key, TValue value)
        {
            SetupKey(key);

            this[key].Add(value);
        }

        /// <summary>
        /// Adds the list items.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public void AddListItems(TKey key, IEnumerable<TValue> values)
        {
            SetupKey(key);

            this[key].AddRange(values);
        }

        /// <summary>
        /// Removes the list item at.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public bool RemoveListItemAt(TKey key, int index)
        {
            if (!ContainsKey(key))
                return false;

            if (this[key].Count <= index)
            {
                return false;
            }

            this[key].RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes the list item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool RemoveListItem(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                return false;

            return this[key].Remove(value);
        }

        /// <summary>
        /// Removes the list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool RemoveList(TKey key)
        {
            return Remove(key);
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        /// <param name="key">The key.</param>
        public void ClearList(TKey key)
        {
            if (!ContainsKey(key))
                return;

            this[key].Clear();
        }

        /// <summary>
        /// Setups the key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SetupKey(TKey key)
        {
            if (!ContainsKey(key))
                Add(key, new List<TValue>());
        }
    }
}