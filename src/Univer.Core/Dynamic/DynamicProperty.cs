using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Univer.Core.Dynamic
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicProperty : DynamicTypeComponent
    {
        private PropertyBuilder _propertyBuilder;
        private PropertyAttributes _propertyAttributes;
        public Type PropertyType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProperty" /> class.
        /// </summary>
        /// <param name="dynamicType">Type of the dynamic.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyAttributes">The property attributes.</param>
        public DynamicProperty(DynamicType dynamicType,
            string propertyName,
            Type propertyType,
            PropertyAttributes propertyAttributes = PropertyAttributes.None)
            : base(dynamicType, propertyName)
        {
            this.PropertyType = propertyType;
            _propertyAttributes = propertyAttributes;

            _propertyBuilder = TypeBuilder.DefineProperty(propertyName, propertyAttributes, PropertyType, null);
        }

        /// <summary>
        /// Sets the backing field of the property.
        /// </summary>
        /// <param name="dynamicField">The dynamic field.</param>
        /// <returns></returns>
        public DynamicProperty SetBackingField(DynamicField dynamicField)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            // Create the get method of the property
            var propertyGetMethodBuilder = TypeBuilder.DefineMethod("get_" + Name,
                methodAttributes,
                PropertyType,
                Type.EmptyTypes);

            var propertyGetMethodILGenerator = propertyGetMethodBuilder.GetILGenerator();
            propertyGetMethodILGenerator.Emit(OpCodes.Ldarg_0);
            propertyGetMethodILGenerator.Emit(OpCodes.Ldfld, dynamicField.FieldBuilder);
            propertyGetMethodILGenerator.Emit(OpCodes.Ret);
            _propertyBuilder.SetGetMethod(propertyGetMethodBuilder);

            // Create the set method of the property
            var propertySetMethodBuilder = TypeBuilder.DefineMethod("set_" + Name,
                methodAttributes,
                null,
                new[] { PropertyType });

            var propertySetMethodILGenerator = propertySetMethodBuilder.GetILGenerator();
            propertySetMethodILGenerator.Emit(OpCodes.Ldarg_0);
            propertySetMethodILGenerator.Emit(OpCodes.Ldarg_1);
            if (dynamicField.FieldType.IsClass)
            {
                propertySetMethodILGenerator.Emit(OpCodes.Castclass, dynamicField.FieldType);
            }
            propertySetMethodILGenerator.Emit(OpCodes.Stfld, dynamicField.FieldBuilder);
            propertySetMethodILGenerator.Emit(OpCodes.Ret);
            _propertyBuilder.SetSetMethod(propertySetMethodBuilder);

            return this;
        }

        /// <summary>
        /// Adds the custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        /// <returns></returns>
        public DynamicProperty AddCustomAttribute(Attribute attribute, params object[] constructorArgs)
        {
            var customAttribute = this.BuildCustomAttribute(attribute, constructorArgs);
            if (customAttribute != null)
            {
                _propertyBuilder.SetCustomAttribute(customAttribute);
            }

            return this;
        }

        /// <summary>
        /// Adds the custom attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public DynamicProperty AddCustomAttributes(IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var customAttribute = this.BuildCustomAttribute(attribute, null);
                if (customAttribute != null)
                {
                    _propertyBuilder.SetCustomAttribute(customAttribute);
                }
            }

            return this;
        }
    }
}
