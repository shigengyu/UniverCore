using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using Univer.Common;

namespace Univer.Core.Dynamic
{
    public class DynamicCustomAttributeBuilder
    {
        public CustomAttributeBuilder BuildCustomAttribute(Attribute attribute, params object[] constructorArgs)
        {
            ConstructorInfo attributeConstructor = null;
            if (constructorArgs == null || constructorArgs.Length == 0)
            {
                attributeConstructor = attribute.GetType().GetConstructor(Type.EmptyTypes);
                constructorArgs = new object[0];

                // No argument constructor does not exist
                if (attributeConstructor == null)
                {
                    TryInferConstructorArgumentValues(attribute, out attributeConstructor, out constructorArgs);
                }
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

        public IEnumerable<CustomAttributeBuilder> BuildCustomAttributes(IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                yield return BuildCustomAttribute(attribute, null);
            }
        }

        private static bool TryInferConstructorArgumentValues(Attribute attribute, out ConstructorInfo constructor, out object[] constructorArgs)
        {
            var argumentValues = new List<object>();
            constructor = null;

            var attributeType = attribute.GetType();
            constructor = attributeType.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                constructorArgs = argumentValues.ToArray();
                return true;
            }

            foreach (var constructorInfo in attributeType.GetConstructors().OrderBy(item => item.GetParameters().Count()))
            {
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    var property = attributeType.GetProperty(StringUtil.ToPascalCase(parameterInfo.Name));
                    if (property != null)
                    {
                        argumentValues.Add(property.GetValue(attribute, null));
                    }
                    else
                    {
                        argumentValues.Clear();
                        break;
                    }
                }

                if (argumentValues.Count > 0)
                {
                    constructor = constructorInfo;
                    constructorArgs = argumentValues.ToArray();
                    return true;
                }
            }

            constructorArgs = null;
            return false;
        }
    }
}
