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
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using NUnit.Framework;
using Univer.Core.Caching;

namespace Univer.Core.Dynamic
{
    public class DynamicModule
    {
        #region Constants & Readonly Fields

        public const string DefaultDynamicModuleName = "DynamicModule";

        public static Lazy<DynamicModule> Default = new Lazy<DynamicModule>(
            () => new DynamicModule(DynamicAssembly.Default.Value.AssemblyBuilder, DefaultDynamicModuleName, DynamicAssembly.DynamicAssemblyDefaultNamespace));

        #endregion

        #region Private Fields

        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private DictionaryCacheContainer<string, DynamicType> _dynamicTypeCache = new DictionaryCacheContainer<string, DynamicType>();
        private string _defaultNamespace;

        #endregion

        #region Constructor(s)

        public DynamicModule(AssemblyBuilder assemblyBuilder, string name, string defaultNamespace)
        {
            _assemblyBuilder = assemblyBuilder;
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(name, DynamicAssembly.DynamicAssemblyDefaultAssemblyFileName);
            _defaultNamespace = defaultNamespace;
        }

        #endregion

        #region Public Methods

        public DynamicType GetDynamicType(string name)
        {
            var fullName = name.Contains(".") ? name : _defaultNamespace + "." + name;
            if (_dynamicTypeCache.ContainsKey(fullName))
                return _dynamicTypeCache[fullName];

            return null;
        }

        public DynamicType CreateDynamicType(string name, TypeAttributes typeAttributes)
        {
            var fullName = name.Contains(".") ? name : _defaultNamespace + "." + name;
            var dynamicType = new DynamicType(_moduleBuilder, fullName, typeAttributes);
            _dynamicTypeCache[fullName] = dynamicType;
            return _dynamicTypeCache[fullName];
        }

        #endregion

        #region Unit Tests

        [TestFixture]
        public class DynamicModuleTest
        {
            [Test]
            public void TestDefaultDynamicModule()
            {
                Assert.NotNull(DynamicModule.Default);
            }
        }

        #endregion
    }
}
