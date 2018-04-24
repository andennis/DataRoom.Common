using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Common.Extensions.Tests
{
    [TestFixture]
    public class ExpressionExtensionsTests
    {
        private class MyClass
        {
            public string Member1;
            public string Func1()
            {
                return string.Empty;

            }
            public void Action1()
            {
                
            }
            public int P1 => 0;
            public string P2 { get; set; }
        }

        [Test]
        public void GetPropertyNameTest()
        {
            Expression<Func<MyClass, string>> expFunc1 = x => x.Func1();
            Assert.Throws<ArgumentException>(() => expFunc1.GetPropertyName());

            Expression<Func<MyClass, int>> expP1 = x => x.P1;
            string name = expP1.GetPropertyName();
            Assert.AreEqual("P1", name);

            Expression<Func<MyClass, string>> expMemeber1 = x => x.Member1;
            Assert.Throws<ArgumentException>(() => expMemeber1.GetPropertyName());
        }

        [Test]
        public void GetPropertyValueTest()
        {
            var mc = new MyClass(){P2 = "123"};
            Expression<Func<MyClass, string>> expP2 = x => x.P2;
            string val = expP2.GetPropertyValue(mc);
            Assert.AreEqual("123", val);
        }

        [Test]
        public void GetMethodNameTest()
        {
            Expression<Action<MyClass>> expAction = x => x.Action1();
            string name = expAction.GetMethodName();
            Assert.AreEqual("Action1", name);

            expAction = x => x.Func1();
            name = expAction.GetMethodName();
            Assert.AreEqual("Func1", name);
        }
    }
}
