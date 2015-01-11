using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Univer.Core.Dynamic;

namespace Univer.Core.DynamicProxyContrib
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProxyGenerationOptionsExtension
    {
        private static DynamicCustomAttributeBuilder _attributeBuilder = new DynamicCustomAttributeBuilder();

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        /// <returns></returns>
        public static ProxyGenerationOptions AddAttribute(this ProxyGenerationOptions options, Attribute attribute, params object[] constructorArgs)
        {
            options.AdditionalAttributes.Add(_attributeBuilder.BuildCustomAttribute(attribute, constructorArgs));
            return options;
        }

        /// <summary>
        /// Adds the attributes without constructor arguments.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public static ProxyGenerationOptions AddAttributesWithoutConstructorArgs(this ProxyGenerationOptions options, params Attribute[] attributes)
        {
            foreach (var attribute in attributes)
            {
                options.AdditionalAttributes.Add(_attributeBuilder.BuildCustomAttribute(attribute));
            }
            return options;
        }
    }
}
