using System;
using System.Collections.Generic;
using KDR.Messages;
using NUnit.Framework;

namespace KDR.Unit.Tests.Messages
{
    public class MessageTypeConvertersTests
    {
        [Test]
        public void GetTypeName_ReturnTypeName_ThatIsRecoverable_SimpleClass()
        {
            var result = MessageTypeConverters.GetTypeName(typeof(TestClass));

            var resultType = Type.GetType(result);

            Assert.True(typeof(TestClass).Equals(resultType));
        }

        [Test]
        public void GetTypeName_ReturnTypeName_ThatIsRecoverable_GenericClass()
        {
            var result = MessageTypeConverters.GetTypeName(typeof(GenericTestClass<string>));

            var resultType = Type.GetType(result);

            Assert.True(typeof(GenericTestClass<string>).Equals(resultType));
        }

        [Test]
        public void GetTypeName_ReturnTypeName_ThatIsRecoverable_GenericClassMultiple()
        {
            var result = MessageTypeConverters.GetTypeName(typeof(GenericTestClass<string, IDictionary<object, DateTime>, int>));

            var resultType = Type.GetType(result);

            Assert.True(typeof(GenericTestClass<string, IDictionary<object, DateTime>, int>).Equals(resultType));
        }

        [Test]
        public void GetNameType_ReturnProperType_SimpleClass()
        {
            var typeString = "KDR.Unit.Tests.Messages.MessageTypeConvertersTests+TestClass, KDR.Unit.Tests";

            var resultType = MessageTypeConverters.GetNameType(typeString);

            var result = MessageTypeConverters.GetTypeName(resultType);

            Assert.AreEqual(typeString, result);
        }

        [Test]
        public void GetNameType_ReturnProperType_GenericClass()
        {
            var typeString = "KDR.Unit.Tests.Messages.MessageTypeConvertersTests+GenericTestClass`1[[System.String, System.Private.CoreLib]], KDR.Unit.Tests";

            var resultType = MessageTypeConverters.GetNameType(typeString);

            var result = MessageTypeConverters.GetTypeName(resultType);

            Assert.AreEqual(typeString, result);
        }

        [Test]
        public void GetNameType_ReturnProperType_GenericClassMultiple()
        {
            var typeString = "KDR.Unit.Tests.Messages.MessageTypeConvertersTests+GenericTestClass`3[[System.String, System.Private.CoreLib], [System.Collections.Generic.IDictionary`2[[System.Object, System.Private.CoreLib], [System.DateTime, System.Private.CoreLib]], System.Private.CoreLib], [System.Int32, System.Private.CoreLib]], KDR.Unit.Tests";

            var resultType = MessageTypeConverters.GetNameType(typeString);

            var result = MessageTypeConverters.GetTypeName(resultType);

            Assert.AreEqual(typeString, result);
        }

        private class TestClass { }

        private class GenericTestClass<T> { }

        private class GenericTestClass<TFirst, TSecond, TThird> { }
    }
}