using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using hwj.CommonLibrary;

namespace UnitTest.CommonLibrary
{
    [TestClass]
    public class Test_Security
    {
        [TestMethod]
        public void TestEncrypt()
        {
            Assert.IsTrue(hwj.CommonLibrary.Object.SecurityHelper.Encrypt("111111", "VINSON") == "C2BC72DB1918C721");
        }

        [TestMethod]
        public void TestDecrypt()
        {
            Assert.IsTrue(hwj.CommonLibrary.Object.SecurityHelper.Decrypt("C2BC72DB1918C721", "VINSON") == "111111");
        }
    }
}
