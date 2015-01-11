using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.Core.Dynamic;
using System.Reflection;
using System.Xml.Serialization;
using Univer.Common;
using System.ComponentModel;
using NUnit.Framework;

namespace Univer.Core.Configuration
{
    public class XmlConfigurationProxy
    {
        public static T CreateProxyInstance<T>(bool initialize = true)
        {
            if (!typeof(T).IsInterface)
            {
                throw new ArgumentException("Only interfaces are supported for creating proxies");
            }

            var instance = (T)Activator.CreateInstance(GetProxyClassType(typeof(T)));

            if (initialize)
            {
                InitializeFields(instance);
            }

            return instance;
        }

        public static Type GetProxyClassType(Type type, string proxyClassNamePostfix = "_Proxy")
        {
            if (type.IsGenericType)
            {
                // If the type is generic type, check if the type is assignable from List<T>.
                var isListType = type.IsList();
                if (isListType)
                {
                    var genericArgument = type.GetGenericArguments()[0];
                    if (genericArgument.IsInterface)
                    {
                        genericArgument = GetProxyClassType(genericArgument);
                    }

                    // Create proxy for both generic definition and generic argument.
                    // IList<IItem> will be proxied as List<IItem_Proxy>.
                    return typeof(List<>).MakeGenericType(genericArgument);
                }
            }
            else if (type.IsInterface)
            {
                // Check if the dynamic type is already created.
                var dynamicType = DynamicModule.Default.Value.GetDynamicType(type.Name + proxyClassNamePostfix);
                if (dynamicType != null)
                    return dynamicType.Type;

                // Create the dynamic type if not yet created.
                dynamicType = DynamicModule.Default.Value
                    .CreateDynamicType(type.Name + proxyClassNamePostfix, TypeAttributes.Class | TypeAttributes.Public)
                    .AddCustomAttributes(type.GetCustomAttributes(true).Cast<Attribute>().ToArray())
                    .AddInterface(type);

                // Add properties
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    Type fieldType = GetProxyClassType(property.PropertyType);

                    // Create a public field as the backing field of the property.
                    var dynamicField = dynamicType.AddField("_" + StringUtil.ToCamelCase(property.Name), fieldType, FieldAttributes.Public);
                    if (fieldType.IsList())
                    {
                        dynamicField.AddCustomAttribute(new XmlArrayAttribute(property.Name));
                        dynamicField.AddCustomAttribute(new XmlArrayItemAttribute(property.PropertyType.GetGenericArguments()[0].Name));
                    }
                    else
                    {
                        dynamicField.AddCustomAttribute(new XmlElementAttribute(property.Name));
                    }

                    var attributes = property.GetCustomAttributes(true).Cast<Attribute>().ToList();

                    var dynamicProperty = dynamicType.AddProperty(property.Name, property.PropertyType)
                        .SetBackingField(dynamicField)
                        .AddCustomAttributes(attributes)
                        .AddCustomAttribute(new XmlIgnoreAttribute());

                    // Add the default value attribute as it does not have no argument constructor.
                    var defaultValueAttribute = (DefaultValueAttribute)attributes.SingleOrDefault(item => item.GetType() == typeof(DefaultValueAttribute));
                    if (defaultValueAttribute != null)
                    {
                        // Cannot add DefaultValueAttribute to field, otherwise XmlSerializer cannot work properly.
                        dynamicField.AddCustomAttribute(new FieldDefaultValueAttribute(defaultValueAttribute.Value), defaultValueAttribute.Value);

                        dynamicProperty.AddCustomAttribute(defaultValueAttribute, defaultValueAttribute.Value);
                    }
                }

                return dynamicType.Type;
            }

            return type;
        }

        public static Type GetProxyClassType<T>()
        {
            return GetProxyClassType(typeof(T));
        }

        private static void InitializeFields(object obj)
        {
            foreach (var field in obj.GetType().GetFields())
            {
                if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                {
                    if (field.IsDefined<FieldDefaultValueAttribute>())
                    {
                        var defaultValue = field.GetCustomAttribute<FieldDefaultValueAttribute>().Value;
                        field.SetValue(obj, defaultValue);
                    }
                }
                else if (field.GetValue(obj) == null)
                {
                    var fieldValue = Activator.CreateInstance(field.FieldType);
                    field.SetValue(obj, fieldValue);
                    InitializeFields(fieldValue);
                }
            }
        }

        #region Unit Tests

        [TestFixture]
        public class XmlConfigurationProxyTest
        {
            public interface IArgument
            {
                [DefaultValue("Univer")]
                [XmlElement("Name")]
                string Name { get; set; }
            }

            [Test]
            public void Test()
            {
                var argument = XmlConfigurationProxy.CreateProxyInstance<IArgument>();
                Assert.NotNull(argument);

                var property = argument.GetType().GetProperty("Name");
                Assert.NotNull(property);

                Assert.True(property.IsDefined<DefaultValueAttribute>());
                Assert.True(property.IsDefined<XmlElementAttribute>());
                Assert.AreEqual("Name", property.GetCustomAttribute<XmlElementAttribute>().ElementName);
            }
        }

        #endregion
    }

    public static class XmlConfigurationProxyExtension
    {
        public static bool IsList(this Type type)
        {
            return type.IsGenericType
                && type.GetGenericArguments().Length == 1
                && type.IsAssignableFrom(typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]));
        }
    }
}
