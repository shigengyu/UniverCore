using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Castle.DynamicProxy;
using NUnit.Framework;
using Univer.Core.Configuration;
using Univer.Core.Dynamic;

namespace Univer.Core
{
    public class XmlConfiguration<T>
    {
        private Type _proxyType;
        public T Data { get; private set; }

        public XmlConfiguration()
        {
            this.Data = XmlConfigurationProxy.CreateProxyInstance<T>();
            _proxyType = this.Data.GetType();
        }

        public void Save(string fileName)
        {
            var serialzier = new XmlSerializer(_proxyType);
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                serialzier.Serialize(fileStream, this.Data);
            }
        }

        public void Load()
        {
        }

        public string GetXml()
        {
            var serialzier = new XmlSerializer(_proxyType);
            using (var memoryStream = new MemoryStream())
            {
                serialzier.Serialize(memoryStream, this.Data);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }
        }
    }

    [TestFixture]
    public class XmlConfigurationTest
    {
        [XmlRoot("ProjectConfig")]
        public class ProjectConfig
        {
            [DefaultValue("UniverCore")]
            public virtual string ProjectName { get; set; }

            public virtual IEnumerable<ModuleConfig> Modules { get; set; }

            public virtual StartupConfig StartUp { get; set; }
            public virtual SecurityConfig Security { get; set; }
        }

        public class StartupConfig
        {
            [DefaultValue(true)]
            public virtual bool UseLocal { get; set; }
        }

        public class SecurityConfig
        {
            [DefaultValue(false)]
            public virtual bool Encrypt { get; set; }
        }

        public class ModuleConfig
        {
            public virtual string ModuleName { get; set; }
        }

        //[XmlRoot("ProjectConfig")]
        //public interface ProjectConfig
        //{
        //    [DefaultValue("UniverCore")]
        //    string ProjectName { get; set; }

        //    IEnumerable<ModuleConfig> Modules { get; set; }

        //    StartupConfig StartUp { get; set; }
        //    SecurityConfig Security { get; set; }
        //}

        //public interface StartupConfig
        //{
        //    [DefaultValue(true)]
        //    bool UseLocal { get; set; }
        //}

        //public interface SecurityConfig
        //{
        //    [DefaultValue(false)]
        //    bool Encrypt { get; set; }
        //}

        //public interface ModuleConfig
        //{
        //    string ModuleName { get; set; }
        //}

        [Test]
        public void TestCreateInstance()
        {
            var xmlConfiguration = new XmlConfiguration<ProjectConfig>();
            Console.WriteLine(xmlConfiguration.GetXml());
        }

        [Test]
        public void TestSetValue()
        {
            var xmlConfiguration = new XmlConfiguration<ProjectConfig>();

            DynamicAssembly.Default.Value.Save();

            Assert.AreEqual("UniverCore", xmlConfiguration.Data.ProjectName);
            xmlConfiguration.Data.ProjectName = "Univer.Core";
            Assert.AreEqual("Univer.Core", xmlConfiguration.Data.ProjectName);

            Assert.True(xmlConfiguration.Data.StartUp.UseLocal);
            xmlConfiguration.Data.StartUp.UseLocal = false;
            Assert.False(xmlConfiguration.Data.StartUp.UseLocal);

            Assert.False(xmlConfiguration.Data.Security.Encrypt);
            xmlConfiguration.Data.Security.Encrypt = true;
            Assert.True(xmlConfiguration.Data.Security.Encrypt);

            Console.WriteLine(xmlConfiguration.GetXml());
        }

        [Test]
        public void TestAddItemsToList()
        {
            var xmlConfiguration = new XmlConfiguration<ProjectConfig>();

            xmlConfiguration.Data.WrapAsList(item => item.Modules)
                .CreateNew().SetProperty(item => item.ModuleName, "Module1").AddToList();
            //    .CreateNew().SetProperty(item => item.ModuleName, "Module2").AddToList()
            //    .CreateNew().SetProperty(item => item.ModuleName, "Module3").AddToList();

            Assert.AreEqual(1, xmlConfiguration.Data.Modules.Count());

            Console.WriteLine(xmlConfiguration.GetXml());
        }
    }
}
