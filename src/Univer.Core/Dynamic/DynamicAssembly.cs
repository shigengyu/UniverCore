/*
 * Copyright (c) 2013-2015 Univer Shi
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
using Univer.Common.IO;

namespace Univer.Core.Dynamic
{
    public class DynamicAssembly
    {
        public const string DynamicAssemblyDefaultNamespace = "Univer.Core.DynamicAssembly";
        public const string DynamicAssemblyDefaultAssemblyName = "Univer.Core.DynamicAssembly";
        public const string DynamicAssemblyDefaultAssemblyFileName = DynamicAssemblyDefaultAssemblyName + ".dll";

        public static Lazy<DynamicAssembly> Default = new Lazy<DynamicAssembly>(
            () => new DynamicAssembly(DynamicAssemblyDefaultAssemblyName, DynamicAssemblyDefaultNamespace));

        private AssemblyName _assemblyName;
        private AssemblyBuilder _assemblyBuilder;
        private AppDomain _appDomain;

        public string DefaultNamespace { get; private set; }

        public AssemblyBuilder AssemblyBuilder
        {
            get
            {
                if (_assemblyBuilder == null)
                {
                    _assemblyBuilder = _appDomain.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave,
                        Paths.GetAppDataFolder("Univer.Core", Guid.NewGuid().ToString()));
                }

                return _assemblyBuilder;
            }
        }

        public DynamicAssembly(string assemblyName, string defaultNamespace, AppDomain appDomain = null)
        {
            _appDomain = appDomain ?? AppDomain.CurrentDomain;
            _assemblyName = new AssemblyName(assemblyName);
            DefaultNamespace = defaultNamespace;
        }

        public DynamicModule CreateModule(string moduleName)
        {
            return new DynamicModule(AssemblyBuilder, moduleName, this.DefaultNamespace);
        }

        public void Save()
        {
            var savePath = DynamicAssemblyDefaultAssemblyFileName;
            _assemblyBuilder.Save(savePath);
        }

        [TestFixture]
        public class DynamicAssemblyTest
        {
            [Test]
            public void TestDefaultDynamicAssembly()
            {
                Assert.NotNull(DynamicAssembly.Default);

                Assert.AreEqual(
                    DynamicAssembly.DynamicAssemblyDefaultAssemblyName,
                    DynamicAssembly.Default.Value.AssemblyBuilder.GetName().Name);
            }
        }
    }
}
