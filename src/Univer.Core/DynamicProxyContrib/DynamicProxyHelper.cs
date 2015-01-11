using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Univer.Core.DynamicProxyContrib
{
    public static class DynamicProxyHelper
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static T CreateDynamicProxy<T>() where T : class
        {
            if (!typeof(T).IsClass)
            {
                throw new NotSupportedException("Only supports creating dynamic proxy on class types");
            }

            var result = _generator.CreateClassProxy<T>();
            return result;
        }
    }
}
