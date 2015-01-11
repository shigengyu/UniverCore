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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TSubKey">The type of the sub key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class CompositeConcurrentDictionary<TKey, TSubKey, TValue> :
        ConcurrentDictionary<TKey, ConcurrentDictionary<TSubKey, TValue>>
    {
        /// <summary>
        /// Determines whether the composite dictionary contains an item with the specified key and sub key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains item; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsItem(TKey key, TSubKey subKey)
        {
            return ContainsKey(key) && this[key].ContainsKey(subKey);
        }

        /// <summary>
        /// Try the add the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryAddItem(TKey key, TSubKey subKey, TValue value)
        {
            return !ContainsKey(key)
                   && TryAdd(key, new ConcurrentDictionary<TSubKey, TValue>())
                   && this[key].TryAdd(subKey, value);
        }

        /// <summary>
        /// Try to remove the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <returns></returns>
        public bool TryRemoveItem(TKey key, TSubKey subKey)
        {
            TValue removed;
            return TryRemoveItem(key, subKey, out removed);
        }

        /// <summary>
        /// Try to the remove the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <param name="removed">The removed.</param>
        /// <returns></returns>
        public bool TryRemoveItem(TKey key, TSubKey subKey, out TValue removed)
        {
            if (!ContainsKey(key))
            {
                removed = default(TValue);
                return false;
            }

            return this[key].TryRemove(subKey, out removed);
        }
    }
}