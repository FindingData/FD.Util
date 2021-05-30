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
    public class MacHelperTests
    {
        [TestMethod()]
        public void GetMacAddressTest()
        {
            var macStr = "04-D9-F5-AC-87-D4";
             var mac = MacHelper.GetMacAddress();
            Assert.AreEqual(macStr, mac);
        }
    }
}