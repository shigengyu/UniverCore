using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Univer.Core.Dynamic
{
    public abstract class DynamicTypeComponent
    {
        public DynamicType DynamicType { get; private set; }

        public TypeBuilder TypeBuilder
        {
            get
            {
                return DynamicType.TypeBuilder;
            }
        }

        public string Name { get; private set; }

        protected DynamicTypeComponent(DynamicType dynamicType, string name)
        {
            this.DynamicType = dynamicType;
            this.Name = name;
        }

        protected CustomAttributeBuilder BuildCustomAttribute(Attribute attribute, params object[] constructorArgs)
        {
            ConstructorInfo attributeConstructor = null;
            if (constructorArgs == null || constructorArgs.Length == 0)
            {
                attributeConstructor = attribute.GetType().GetConstructor(Type.EmptyTypes);
                constructorArgs = new object[0];
            }
            else
            {
                attributeConstructor = attribute.GetType().GetConstructor(constructorArgs.Select(item => item.GetType()).ToArray());
            }

            if (attributeConstructor == null)
                return null;

            var propertyInfoList = new List<PropertyInfo>();
            var propertyValueList = new List<object>();

            var attributeProperties = (from item in attribute.GetType().GetProperties()
                                       where item.GetIndexParameters().Length == 0 && item.Name != "TypeId"
                                       select item).ToArray();

            var defaultValueAttribute = attributeConstructor.Invoke(constructorArgs);

            foreach (var property in attributeProperties)
            {
                var attributeValue = property.GetValue(attribute, null);
                var defaultValue = property.GetValue(defaultValueAttribute, null);

                // If attribute value is same with default value, skip it.
                if (attributeValue != null && !attributeValue.Equals(defaultValue))
                {
                    propertyInfoList.Add(property);
                    propertyValueList.Add(attributeValue);
                }
            }

            var customAttributeBuilder = new CustomAttributeBuilder(attributeConstructor,
                constructorArgs,
                propertyInfoList.ToArray(),
                propertyValueList.ToArray());

            return customAttributeBuilder;
        }
    }
}
