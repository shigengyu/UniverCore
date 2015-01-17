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
using System.Data;
using NUnit.Framework;

namespace Univer.Core.DataMapping
{
    public static class DataTableExtensions
    {
        public static IEnumerable<TEntity> ExtractEntities<TEntity>(this DataTable dataTable) where TEntity : new()
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                yield return dataRow.ExtractEntity<TEntity>();
            }
        }

        [TestFixture]
        public class DataMappingTest
        {
            private class MyEntityA
            {
                public int? Number { get; set; }
                public string Name { get; set; }
            }

            private class MyEntityB
            {
                [DataMappingField]
                public int? Number { get; set; }

                [DataMappingField]
                public string Name { get; set; }
            }

            private class MyEntityC
            {
                [DataMappingField]
                public int Number { get; set; }

                [DataMappingField]
                public string Name { get; set; }
            }

            private class MyEntityD
            {
                [DataMappingField]
                public double? Number { get; set; }

                [DataMappingField]
                public string Name { get; set; }
            }

            private DataTable _dataTable;

            [SetUp]
            public void Setup()
            {
                _dataTable = new DataTable();
                _dataTable.Columns.Add("Number", typeof(int));
                _dataTable.Columns.Add("Name", typeof(string));
                _dataTable.Rows.Add(1, "A");
                _dataTable.Rows.Add(2, "B");
                _dataTable.Rows.Add(DBNull.Value, "C");
            }

            [Test]
            public void ExtractEntityImplicitColumnNameTest()
            {
                var entities = _dataTable.ExtractEntities<MyEntityA>().ToList();
                Assert.IsTrue(entities.Count == 3);
                Assert.IsTrue(entities.All(item => item.Number == null && item.Name == null));
            }

            [Test]
            public void ExtractEntityOverriddenColumnNameTest()
            {
                var entities = _dataTable.ExtractEntities<MyEntityB>().ToList();
                Assert.IsTrue(entities.Count == 3);
                Assert.IsNull(entities.Last().Number);

                Assert.AreEqual(1, entities[0].Number);
                Assert.AreEqual("A", entities[0].Name);

                Assert.AreEqual(2, entities[1].Number);
                Assert.AreEqual("B", entities[1].Name);

                Assert.AreEqual(null, entities[2].Number);
                Assert.AreEqual("C", entities[2].Name);
            }

            [Test]
            public void ExtractEntitiesAssignToNonNullableTest()
            {
                var entities = _dataTable.ExtractEntities<MyEntityC>().ToList();
                Assert.AreEqual(3, entities.Count);
                Assert.AreEqual(default(int), entities.Last().Number);
            }

            [Test]
            public void ExtractEntitiesTypeConversionTest()
            {
                var entities = _dataTable.ExtractEntities<MyEntityD>().ToList();
                Assert.AreEqual(1.0, entities[0].Number);
                Assert.AreEqual("A", entities[0].Name);
            }
        }
    }
}
