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
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class MultiDimensionDictionary<TValue> : Collection<TValue>, IList<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDimensionDictionary{TValue}"/> class.
        /// </summary>
        public MultiDimensionDictionary()
        {
            Dimensions = new Dictionary<string, IDimension>();
        }

        private Dictionary<string, IDimension> Dimensions { get; set; }

        /// <summary>
        /// Occurs when [item added].
        /// </summary>
        public event Action<TValue> ItemAdded;

        /// <summary>
        /// Occurs when [item removed].
        /// </summary>
        public event Action<TValue> ItemRemoved;

        /// <summary>
        /// Occurs when [items cleared].
        /// </summary>
        public event Action ItemsCleared;

        /// <summary>
        /// Creates the dimension.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="dimensionName">Name of the dimension.</param>
        /// <returns></returns>
        public MultiDimensionDictionary<TValue> CreateDimension<TKey>(Func<TValue, TKey> keySelector,
            string dimensionName = null)
        {
            dimensionName = dimensionName ?? typeof (TKey).FullName;
            var dimension = new Dimension<TKey>(dimensionName, keySelector);

            ItemAdded += dimension.AddItem;
            ItemRemoved += dimension.RemoveItem;
            ItemsCleared += dimension.ClearItems;

            Dimensions[dimensionName] = dimension;
            return this;
        }

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns></returns>
        public Dimension<TKey> GetDimension<TKey>()
        {
            return (Dimension<TKey>) Dimensions.SafeGet(typeof (TKey).FullName);
        }

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="dimensionName">Name of the dimension.</param>
        /// <returns></returns>
        public Dimension<TKey> GetDimension<TKey>(string dimensionName)
        {
            dimensionName = dimensionName ?? typeof (TKey).FullName;
            return (Dimension<TKey>) Dimensions.SafeGet(dimensionName);
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        protected override void InsertItem(int index, TValue item)
        {
            base.InsertItem(index, item);

            ItemAdded(item);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        protected override void SetItem(int index, TValue item)
        {
            var locations = Dimensions.Values.ToDictionary(value => value, value => value.LocateItem(Items[index]));

            base.SetItem(index, item);

            foreach (var location in locations)
            {
                location.Key.MoveItem(item, location.Value);
            }
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            var item = this[index];

            base.RemoveItem(index);

            ItemRemoved(item);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();

            ItemsCleared();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        public class Dimension<TKey> : IDimension
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Dimension{T}"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="keySelector">The key selector.</param>
            public Dimension(string name, Func<TValue, TKey> keySelector)
            {
                Name = name;
                Values = new ListDictionary<TKey, TValue>();
                KeySelector = keySelector;
            }

            /// <summary>
            /// Gets the values.
            /// </summary>
            /// <value>
            /// The values.
            /// </value>
            public ListDictionary<TKey, TValue> Values { get; private set; }

            /// <summary>
            /// Gets the key selector.
            /// </summary>
            /// <value>
            /// The key selector.
            /// </value>
            public Func<TValue, TKey> KeySelector { get; private set; }

            /// <summary>
            /// Gets the <see cref="IList{TValue}"/> with the specified key.
            /// </summary>
            /// <value>
            /// The <see cref="IList{TValue}"/>.
            /// </value>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public IList<TValue> this[TKey key]
            {
                get { return Values[key]; }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; private set; }

            /// <summary>
            /// Locates the item.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public ItemLocation LocateItem(TValue value)
            {
                var key = KeySelector(value);
                return new ItemLocation(key, Values[key].FindIndex(item => ReferenceEquals(item, value)));
            }

            /// <summary>
            /// Moves the item.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="originalLocation">The original location.</param>
            public void MoveItem(TValue value, ItemLocation originalLocation)
            {
                var key = KeySelector(value);
                if (!Equals(key, originalLocation.Key))
                {
                    Values.RemoveListItemAt((TKey) originalLocation.Key, originalLocation.Index);
                    Values.AddListItem(key, value);
                }
            }

            /// <summary>
            /// Adds the item.
            /// </summary>
            /// <param name="value">The value.</param>
            public void AddItem(TValue value)
            {
                var key = KeySelector(value);
                Values.AddListItem(key, value);
            }

            /// <summary>
            /// Removes the item.
            /// </summary>
            /// <param name="value">The value.</param>
            public void RemoveItem(TValue value)
            {
                var key = KeySelector(value);
            }

            /// <summary>
            /// Clears the items.
            /// </summary>
            public void ClearItems()
            {
                Values.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IDimension
        {
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            string Name { get; }

            /// <summary>
            /// Locates the item.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            ItemLocation LocateItem(TValue value);

            /// <summary>
            /// Moves the item.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="originalLocation">The original location.</param>
            void MoveItem(TValue value, ItemLocation originalLocation);
        }

        /// <summary>
        /// 
        /// </summary>
        public struct ItemLocation
        {
            /// <summary>
            /// The index
            /// </summary>
            public int Index;

            /// <summary>
            /// The key
            /// </summary>
            public object Key;

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemLocation"/> struct.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="index">The index.</param>
            public ItemLocation(object key, int index)
            {
                Key = key;
                Index = index;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class MultiDimensionDictionaryTest
    {
        private class Person
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Person"/> class.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="name">The name.</param>
            /// <param name="age">The age.</param>
            public Person(int id, string name, int age)
            {
                Id = id;
                Name = name;
                Age = age;
            }

            public Person(Person person)
                : this(person.Id, person.Name, person.Age)
            {
            }

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the age.
            /// </summary>
            /// <value>
            /// The age.
            /// </value>
            public int Age { get; set; }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return Id + ", " + Name + ", " + Age;
            }
        }

        /// <summary>
        /// Tests this instance.
        /// </summary>
        [Test]
        public void Test()
        {
            var multiDimensionDictionary = new MultiDimensionDictionary<Person>();

            multiDimensionDictionary.CreateDimension(item => item.Id, "ID");
            multiDimensionDictionary.CreateDimension(item => item.Name, "Name");
            multiDimensionDictionary.CreateDimension(item => item.Age, "Age");

            var alex = new Person(1, "Alex", 32);
            var bob = new Person(2, "Bob", 24);
            var david = new Person(3, "David", 32);
            var frank = new Person(4, "Frank", 26);
            var george = new Person(5, "George", 67);

            multiDimensionDictionary.Add(alex);
            multiDimensionDictionary.Add(bob);
            multiDimensionDictionary.Add(david);
            multiDimensionDictionary.Add(frank);
            multiDimensionDictionary.Add(george);

            var idDimension = multiDimensionDictionary.GetDimension<int>("ID");
            Assert.AreEqual(idDimension[1][0], alex);
            Assert.AreEqual(idDimension[2][0], bob);
            Assert.AreEqual(idDimension[3][0], david);
            Assert.AreEqual(idDimension[4][0], frank);
            Assert.AreEqual(idDimension[5][0], george);

            var ageDimension = multiDimensionDictionary.GetDimension<int>("Age");
            Assert.AreEqual(ageDimension[24][0], bob);
            Assert.AreEqual(ageDimension[26][0], frank);
            Assert.AreEqual(ageDimension[32][0], alex);
            Assert.AreEqual(ageDimension[32][1], david);
            Assert.AreEqual(ageDimension[67][0], george);

            var newBob = new Person(bob) { Age = 32 }; // Change Bob's age to 32
            multiDimensionDictionary[1] = newBob;
            Assert.AreEqual(ageDimension[32][0], alex);
            Assert.AreEqual(ageDimension[32][1], david);
            Assert.AreEqual(ageDimension[32][2], newBob);
        }
    }
}