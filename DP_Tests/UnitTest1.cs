using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLib;

namespace DP_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            ObjectContext context = new ObjectContext();
            DataService service = context.GetComponent<DataService>();
           Assert.AreEqual("SAMPLE DATA TEST",service.ProcessData("TEST")); 
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exñeption()
        {
            ObjectContext context = new ObjectContext();
            DataService service = context.GetComponent<DataService>();
            service.ProcessData(null);
        }
    }
}
