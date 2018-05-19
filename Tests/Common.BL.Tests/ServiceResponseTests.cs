using NUnit.Framework;

namespace Common.BL.Tests
{
    [TestFixture]
    public class ServiceResponseTests
    {
        class MyData
        {
            public string Val { get; set; }
        }

        [Test]
        public void DataTest()
        {
            var myData = new MyData() {Val = "123"};
            var resp = new ServiceResponse<MyData>(myData);
            Assert.AreEqual(ServiceResponseStatus.Success, resp.Status);
            Assert.IsNull(resp.Message);
            Assert.IsNotNull(resp.Data);
            Assert.AreEqual(myData.Val, resp.Data.Val);
        }

    }
}
