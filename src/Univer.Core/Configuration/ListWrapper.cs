using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Univer.Core.Configuration
{
    public class ListWrapper<T>
    {
        private object _list;

        public ListWrapper(object list)
        {
            _list = list;
        }

        public ListItemWrapper<T> CreateNew()
        {
            var proxyType = XmlConfigurationProxy.GetProxyClassType<T>();
            var value = (T)Activator.CreateInstance(proxyType);
            return new ListItemWrapper<T>(this, value);
        }

        internal void Add(T value)
        {
            _list.GetType().GetMethod("Add", new Type[] { value.GetType() }).Invoke(_list, new object[] { value });
        }
    }
}
