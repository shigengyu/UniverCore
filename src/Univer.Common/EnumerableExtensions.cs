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
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Sorts to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static IList<T> SortToList<T>(this IEnumerable<T> enumerable) where T : IComparable
        {
            var list = enumerable.ToList();
            list.Sort();
            return list;
        }

        /// <summary>
        /// Sorts to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static IList<T> SortToList<T>(this IEnumerable<T> enumerable, Comparison<T> comparison)
        {
            var list = enumerable.ToList();
            list.Sort(comparison);
            return list;
        }

        /// <summary>
        /// Ases the string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string AsString<T>(this IEnumerable<T> enumerable, string separator = ",")
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// To the list dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static ListDictionary<TKey, TValue> ToListDictionary<TKey, TValue>(this IEnumerable<TValue> enumerable,
            Func<TValue, TKey> keySelector)
        {
            var listDictionary = new ListDictionary<TKey, TValue>();

            foreach (var item in enumerable)
            {
                var key = keySelector(item);
                listDictionary.AddListItem(key, item);
            }

            return listDictionary;
        }

        /// <summary>
        /// Determines whether the specified enumerable contains duplicates.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static bool ContainsDuplicates<TSource, TResult>(this IEnumerable<TSource> enumerable,
            Func<TSource, TResult> selector)
        {
            return enumerable.ToListDictionary(selector).Values.Any(item => item.Count > 1);
        }

        /// <summary>
        /// Gets the duplicates.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, List<TValue>>> GetDuplicates<TKey, TValue>(
            this IEnumerable<TValue> enumerable, Func<TValue, TKey> keySelector)
        {
            return enumerable.ToListDictionary(keySelector).Where(item => item.Value.Count > 1);
        }

        /// <summary>
        /// Distincts the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static IList<T> DistinctList<T>(this IEnumerable<T> enumerable, Func<T, T, bool> comparison)
        {
            var list = new List<T>();
            foreach (var item in enumerable)
            {
                if (!list.Any(existing => comparison(existing, item)))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Distincts the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static IList<T> DistinctList<T>(this IEnumerable<T> enumerable, Comparison<T> comparison)
        {
            return enumerable.DistinctList((a, b) => comparison(a, b) == 0);
        }

        /// <summary>
        /// Distincts the list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IList<TSource> DistinctList<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> keySelector)
        {
            return enumerable.DistinctList((a, b) => keySelector(a).Equals(keySelector(b)));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class EnumerableExtensionsTest
        {
            /// <summary>
            /// Setups this instance.
            /// </summary>
            [SetUp]
            public void Setup()
            {
                _entities = new List<Entity>
                {
                    new Entity { Number = 1, Name = "A" },
                    new Entity { Number = 2, Name = "B" },
                    new Entity { Number = 3, Name = "A" },
                    new Entity { Number = 4, Name = "B" }
                };
            }

            private class Entity
            {
                public int Number { get; set; }
                public string Name { get; set; }
            }

            private IList<Entity> _entities;

            /// <summary>
            /// Determines whether [contains duplicates test].
            /// </summary>
            [Test]
            public void ContainsDuplicatesTest()
            {
                Assert.IsFalse(_entities.ContainsDuplicates(item => item.Number));
                Assert.IsTrue(_entities.ContainsDuplicates(item => item.Name));
            }

            /// <summary>
            /// Distincts the list test.
            /// </summary>
            [Test]
            public void DistinctListTest()
            {
                var distinctList = _entities.DistinctList((a, b) => a.Number == b.Number);
                Assert.AreEqual(4, distinctList.Count);

                distinctList = _entities.DistinctList((a, b) => a.Name == b.Name);
                Assert.AreEqual(2, distinctList.Count);

                distinctList = _entities.DistinctList(item => item.Name);
                Assert.AreEqual(2, distinctList.Count);
            }
        }
    }
}