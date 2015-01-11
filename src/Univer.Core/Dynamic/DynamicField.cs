using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Univer.Core.Dynamic
{
    public class DynamicField : DynamicTypeComponent
    {
        private FieldAttributes _fieldAttributes;
        internal FieldBuilder FieldBuilder;
        public Type FieldType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicField"/> class.
        /// </summary>
        /// <param name="dynamicType">Type of the dynamic.</param>
        public DynamicField(DynamicType dynamicType, string fieldName, Type fieldType, FieldAttributes fieldAttributes = FieldAttributes.Private)
            : base(dynamicType, fieldName)
        {
            FieldType = fieldType;
            _fieldAttributes = fieldAttributes;

            FieldBuilder = TypeBuilder.DefineField(fieldName, fieldType, fieldAttributes);
        }

        public DynamicField AddCustomAttribute(Attribute attribute, params object[] constructorArgs)
        {
            var customAttribute = this.BuildCustomAttribute(attribute, constructorArgs);
            if (customAttribute != null)
            {
                FieldBuilder.SetCustomAttribute(customAttribute);
            }

            return this;
        }

        public DynamicField AddCustomAttributes(IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var customAttribute = this.BuildCustomAttribute(attribute, null);
                if (customAttribute != null)
                {
                    FieldBuilder.SetCustomAttribute(customAttribute);
                }
            }

            return this;
        }
    }
}
