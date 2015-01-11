using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Univer.Common;
using Univer.Core.Reflection;

namespace Univer.Core.Configuration
{
    public static class XmlConfigurationExtension
    {
        public static ListWrapper<TItem> WrapAsList<T, TItem>(this T obj, Expression<Func<T, IEnumerable<TItem>>> selector)
        {
            var propertyInfo = obj.GetPropertyInfo(selector);
            var propertyName = propertyInfo.Name;
            var fieldName = "_" + StringUtil.ToCamelCase(propertyInfo.Name);
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldValue = fieldInfo.GetValue(obj);

            return new ListWrapper<TItem>(fieldValue);
        }
    }
}
