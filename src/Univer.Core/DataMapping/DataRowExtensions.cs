/*
 * Copyright (c) 2013 Univer Shi
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
using System.Reflection;
using Univer.Common;

namespace Univer.Core.DataMapping
{
    public static class DataRowExtensions
    {
        public static TEntity ExtractEntity<TEntity>(this DataRow dataRow) where TEntity : new()
        {
            var entity = new TEntity();
            var columnProperties = GetColumnProperties<TEntity>().ToList();

            if (columnProperties.ContainsDuplicates(item => item.Item1))
            {
                throw new DataMappingException("Entity contains duplicated column names.");
            }

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                var columnProperty = columnProperties.SingleOrDefault(item => item.Item1 == column.ColumnName);
                if (columnProperty != null)
                {
                    var data = dataRow[column];
                    var propertyInfo = columnProperty.Item2;
                    propertyInfo.SetPropertyValue(entity, data);
                }
            }

            return entity;
        }

        private static IEnumerable<Tuple<string, PropertyInfo>> GetColumnProperties<TEntity>() where TEntity : new()
        {
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return from propertyInfo in properties
                   let attribute = propertyInfo.GetCustomAttribute<DataMappingFieldAttribute>()
                   where attribute != null
                   let columnName = attribute.ColumnName ?? propertyInfo.Name
                   select new Tuple<string, PropertyInfo>(columnName, propertyInfo);
        }
    }
}
