using Microsoft.VisualStudio.TestTools.UnitTesting;
using FD.Util.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Network.Tests
{
    [TestClass()]
    public class IPHelperTests
    {
        [TestMethod()]
        public void GetIPAddressesTest()
        {
            var ipStr = "192.168.0.188";
            var ip = IPHelper.GetLocalIPAddress();
            Assert.AreEqual(ipStr, ip.ToString());
        }

        [TestMethod()]
        public void GetIPAddressTest()
        {

        }
    }
}