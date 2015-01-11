// The MIT License (MIT)
// 
// Copyright (c) 2012-2013 Univer Shi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class XmlSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Constants

        /// <summary>
        /// The default item string
        /// </summary>
        public const string DefaultItemString = "KeyValuePair";

        /// <summary>
        /// The default key string
        /// </summary>
        public const string DefaultKeyString = "Key";

        /// <summary>
        /// The default value string
        /// </summary>
        public const string DefaultValueString = "Value";

        #endregion

        #region Private Fields

        private readonly string _itemString;
        private readonly string _keyString;
        private readonly string _valueString;

        #endregion

        private string ItemString
        {
            get { return _itemString ?? DefaultItemString; }
        }

        private string KeyString
        {
            get { return _keyString ?? DefaultKeyString; }
        }

        private string ValueString
        {
            get { return _valueString ?? DefaultValueString; }
        }

        /// <summary>
        /// Gets the XML schema URL.
        /// </summary>
        /// <value>
        /// The XML schema URL.
        /// </value>
        public string XmlSchemaUrl { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        public XmlSerializableDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected XmlSerializableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="xmlSchemaUrl">The XML schema URL.</param>
        public XmlSerializableDictionary(string xmlSchemaUrl)
            : this()
        {
            XmlSchemaUrl = xmlSchemaUrl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="itemString">The item string.</param>
        /// <param name="keyString">The key string.</param>
        /// <param name="valueString">The value string.</param>
        public XmlSerializableDictionary(string itemString, string keyString, string valueString)
            : this()
        {
            _itemString = itemString;
            _keyString = keyString;
            _valueString = valueString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="xmlSchemaUrl">The XML schema URL.</param>
        /// <param name="itemString">The item string.</param>
        /// <param name="keyString">The key string.</param>
        /// <param name="valueString">The value string.</param>
        public XmlSerializableDictionary(string xmlSchemaUrl, string itemString, string keyString, string valueString)
            : this(itemString, keyString, valueString)
        {
            XmlSchemaUrl = xmlSchemaUrl;
        }

        #endregion

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            if (!string.IsNullOrEmpty(XmlSchemaUrl))
            {
                using (var reader = new XmlTextReader(XmlSchemaUrl))
                {
                    return XmlSchema.Read(reader, delegate(object sender, ValidationEventArgs e) { throw e.Exception; });
                }
            }
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));
            if (!reader.IsEmptyElement && reader.Read())
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement(ItemString);
                    reader.ReadStartElement(KeyString);
                    var key = (TKey) keySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadStartElement(ValueString);
                    var value = (TValue) valueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                    reader.MoveToContent();

                    Add(key, value);
                }
                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));
            foreach (var key in Keys)
            {
                writer.WriteStartElement(ItemString);
                writer.WriteStartElement(KeyString);
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement(ValueString);
                valueSerializer.Serialize(writer, base[key]);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class XmlSerializableDictionaryTest
    {
        private const string Expected = @"<?xml version=""1.0""?>
<MyClass xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Items>
    <KeyValuePair>
      <Key>
        <int>1</int>
      </Key>
      <Value>
        <string>A</string>
      </Value>
    </KeyValuePair>
    <KeyValuePair>
      <Key>
        <int>2</int>
      </Key>
      <Value>
        <string>B</string>
      </Value>
    </KeyValuePair>
  </Items>
</MyClass>";

        /// <summary>
        /// 
        /// </summary>
        public class MyClass
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MyClass"/> class.
            /// </summary>
            public MyClass()
            {
                Items = new XmlSerializableDictionary<int, string>
                {
                    { 1, "A" },
                    { 2, "B" }
                };
            }

            /// <summary>
            /// Gets or sets the items.
            /// </summary>
            /// <value>
            /// The items.
            /// </value>
            public XmlSerializableDictionary<int, string> Items { get; set; }
        }

        /// <summary>
        /// XMLs the deserialization test.
        /// </summary>
        [Test]
        public void XmlDeserializationTest()
        {
            var serializer = new XmlSerializer(typeof (MyClass));
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(Expected.ToCharArray(), 0, Expected.Length);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    var obj = serializer.Deserialize(stream) as MyClass;
                    Assert.NotNull(obj);
                    Assert.AreEqual("A", obj.Items[1]);
                    Assert.AreEqual("B", obj.Items[2]);
                }
            }
        }

        /// <summary>
        /// XMLs the serialization test.
        /// </summary>
        [Test]
        public void XmlSerializationTest()
        {
            var serializer = new XmlSerializer(typeof (MyClass));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, new MyClass());
                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var xml = reader.ReadToEnd();
                    Assert.AreEqual(Expected, xml);
                    reader.Close();
                }

                stream.Close();
            }
        }
    }
}