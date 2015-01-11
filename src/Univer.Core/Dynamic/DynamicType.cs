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
using System.Reflection.Emit;
using System.Reflection;
using NUnit.Framework;
using Univer.Common;

namespace Univer.Core.Dynamic
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicType
    {
        private ModuleBuilder _moduleBuilder;
        private Type _type;
        internal TypeBuilder TypeBuilder;

        /// <summary>
        /// Gets the built type. The type will build automatically if not yet built.
        /// </summary>
        /// <value>
        /// The type built based on the definitions.
        /// </value>
        public Type Type
        {
            get
            {
                if (_type == null)
                {
                    _type = TypeBuilder.CreateType();
                }

                return _type;
            }
        }

        public DynamicType(ModuleBuilder moduleBuilder, string name, TypeAttributes typeAttributes)
        {
            _moduleBuilder = moduleBuilder;
            TypeBuilder = _moduleBuilder.DefineType(name, typeAttributes);
        }

        /// <summary>
        /// Sets the parent type.
        /// </summary>
        /// <param name="parent">The parent type.</param>
        /// <returns></returns>
        public DynamicType SetParent(Type parent)
        {
            TypeBuilder.SetParent(parent);
            return this;
        }

        /// <summary>
        /// Adds an interface to the type's implementation list.
        /// </summary>
        /// <param name="interfaceType">Type of the interface to add.</param>
        /// <returns></returns>
        public DynamicType AddInterface(Type interfaceType)
        {
            TypeBuilder.AddInterfaceImplementation(interfaceType);
            return this;
        }

        /// <summary>
        /// Adds interfaces to the type's implementation list.
        /// </summary>
        /// <param name="interfaceTypes">The interface types to add.</param>
        /// <returns></returns>
        public DynamicType AddInterfaces(params Type[] interfaceTypes)
        {
            foreach (var type in interfaceTypes)
            {
                TypeBuilder.AddInterfaceImplementation(type);
            }

            return this;
        }

        public DynamicField AddField(string name, Type type, FieldAttributes fieldAttributes)
        {
            if (_type != null)
            {
                throw new DynamicTypeException("Properties must be added before type is built");
            }

            var dynamicField = new DynamicField(this, name, type, fieldAttributes);
            return dynamicField;
        }

        public DynamicProperty AddProperty(string name, Type type)
        {
            if (_type != null)
            {
                throw new DynamicTypeException("Properties must be added before type is built");
            }

            var dynamicProperty = new DynamicProperty(this, name, type);
            return dynamicProperty;
        }

        /// <summary>
        /// Adds the auto property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        /// <exception cref="DynamicTypeException">Properties must be added before type is built</exception>
        public DynamicProperty AddAutoProperty(string name, Type type, params Attribute[] attributes)
        {
            if (_type != null)
            {
                throw new DynamicTypeException("Properties must be added before type is built");
            }

            var dynamicField = new DynamicField(this, "_" + StringUtil.ToCamelCase(name), type);

            return new DynamicProperty(this, name, type)
                .SetBackingField(dynamicField)
                .AddCustomAttributes(attributes);
        }

        /// <summary>
        /// Adds the custom attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public DynamicType AddCustomAttributes(params Attribute[] attributes)
        {
            new DynamicCustomAttributeBuilder().BuildCustomAttributes(attributes)
                .Where(item => item != null)
                .ToList()
                .ForEach(item => TypeBuilder.SetCustomAttribute(item));

            return this;
        }

        [TestFixture]
        public class DynamicTypeTest
        {
            public interface IDummy
            {
                string Name { get; set; }
            }

            [Test]
            public void TestCreateType()
            {
                var type = DynamicModule.Default.Value
                    .CreateDynamicType("UniverTypeA", TypeAttributes.Class)
                    .Type;

                Assert.AreEqual("Univer.Core.DynamicAssembly.UniverTypeA", type.FullName);

                var instance = Activator.CreateInstance(type);

                Assert.NotNull(instance);
            }

            [Test]
            public void TestCreateTypeWithBaseClass()
            {
                var type = DynamicModule.Default.Value
                    .CreateDynamicType("UniverTypeB", TypeAttributes.Class)
                    .SetParent(typeof(DynamicTypeTest))
                    .Type;

                Assert.NotNull(type.BaseType);
                Assert.AreEqual(this.GetType().FullName, type.BaseType.FullName);
            }

            [Test]
            public void TestCreateTypeWithInterface()
            {
                var type = DynamicModule.Default.Value
                   .CreateDynamicType("UniverTypeC", TypeAttributes.Class)
                   .AddInterface(typeof(IDummy))
                   .AddAutoProperty("Name", typeof(string)).DynamicType
                   .Type;

                Assert.AreEqual(1, type.GetInterfaces().Length);
                Assert.AreEqual("IDummy", type.GetInterfaces()[0].Name);
            }

            [Test]
            public void TestCreateTypeWithProperty()
            {
                var type = DynamicModule.Default.Value
                    .CreateDynamicType("UniverTypeD", TypeAttributes.Class)
                    .AddAutoProperty("Name", typeof(string), new ParameterAttribute("MyName")).DynamicType
                    .Type;

                var instance = Activator.CreateInstance(type);
                Assert.NotNull(instance);

                var nameProperty = instance.GetType().GetProperty("Name");
                Assert.NotNull(nameProperty);

                Assert.True(nameProperty.IsDefined<ParameterAttribute>());
                Assert.AreEqual("MyName", nameProperty.GetCustomAttribute<ParameterAttribute>().Name);
            }
        }
    }
}
