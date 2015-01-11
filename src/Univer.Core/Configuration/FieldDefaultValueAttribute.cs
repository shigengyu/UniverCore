using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldDefaultValueAttribute : Attribute
    {
        public object Value { get; private set; }

        public FieldDefaultValueAttribute(object value)
        {
            this.Value = value;
        }
    }
}
