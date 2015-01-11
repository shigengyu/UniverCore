using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Univer.Core.Reflection
{
    public static class ObjectExtension
    {
        public static FieldInfo GetFieldInfo<T, TField>(this T obj, Expression<Func<T, TField>> selector)
        {
            var memberExpression = selector.Body as MemberExpression ?? (MemberExpression)((UnaryExpression)selector.Body).Operand;
            return obj.GetType().GetField(memberExpression.Member.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        public static PropertyInfo GetPropertyInfo<T, TProperty>(this T obj, Expression<Func<T, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression ?? (MemberExpression)((UnaryExpression)selector.Body).Operand;
            return obj.GetType().GetProperty(memberExpression.Member.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
    }

    [TestFixture]
    public class ObjectExtensionTest
    {
        class Person
        {
            public int IntField;
            public bool BoolField;
            public string StringField;
            public Person ClassField;

            public int IntProperty { get; set; }
            public bool BoolProperty { get; set; }
            public string StringProperty { get; set; }
            public Person ClassProperty { get; set; }
        }

        [Test]
        public void TestGetFieldAndProperty()
        {
            var person = new Person();

            Assert.NotNull(person.GetFieldInfo(item => item.IntField));
            Assert.NotNull(person.GetFieldInfo(item => item.BoolField));
            Assert.NotNull(person.GetFieldInfo(item => item.StringField));
            Assert.NotNull(person.GetFieldInfo(item => item.ClassField));

            Assert.NotNull(person.GetPropertyInfo(item => item.IntProperty));
            Assert.NotNull(person.GetPropertyInfo(item => item.BoolProperty));
            Assert.NotNull(person.GetPropertyInfo(item => item.StringProperty));
            Assert.NotNull(person.GetPropertyInfo(item => item.ClassProperty));
        }
    }
}
