using System;
using NUnit.Framework;

namespace Common.WebService.Tests
{
    [TestFixture]
    public class WebServiceResponseTests
    {
        class MyData
        {
            public string Val { get; set; }
        }

        [Test]
        public void DataTest()
        {
            var myData = new MyData() {Val = "123"};
            var resp = new WebServiceResponse<MyData>(myData);
            Assert.AreEqual(WebServiceResponseStatus.Success, resp.Status);
            Assert.IsNull(resp.Message);
            Assert.IsNotNull(resp.Data);
            Assert.AreEqual(myData.Val, resp.Data.Val);
        }

    }
}
