using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Univer.Core.Reflection;

namespace Univer.Core.Configuration
{
    public class ListItemWrapper<T>
    {
        private ListWrapper<T> _listWrapper;
        public T Value { get; private set; }

        public ListItemWrapper(ListWrapper<T> listWrapper, T value)
        {
            _listWrapper = listWrapper;
            this.Value = value;
        }

        public ListItemWrapper<T> SetProperty<TValue>(Expression<Func<T, TValue>> selector, TValue value)
        {
            var propertyInfo = this.Value.GetPropertyInfo(selector);
            propertyInfo.SetValue(this.Value, value, null);

            return this;
        }

        public ListWrapper<T> AddToList()
        {
            _listWrapper.Add(this.Value);
            return _listWrapper;
        }
    }
}
