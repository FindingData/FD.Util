using Microsoft.VisualStudio.TestTools.UnitTesting;
using FD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Tests
{
    [TestClass()]
    public class HexMixHelperTests
    {
        [TestMethod()]
        public void ConvertDecTo62MixupTest()
        {
            long num = 123456789;
            var num62 = HexMixHelper.ConvertDecTo62(num);
            var cipher = HexMixHelper.Mixup(num62);
            var clear = HexMixHelper.UnMixUp(cipher);
            var num10 = HexMixHelper.Convert62ToDec(clear);

            Assert.AreEqual(num, num10);
        }


        [TestMethod()]
        public void MixupTest()
        {
            var cipher = HexMixHelper.Mixup("RYswX");
            var clear = HexMixHelper.UnMixUp(cipher);

            Assert.AreEqual(clear, "RYswX");
        }

        [TestMethod()]
        public void ConvertDecTo62Test()
        {
            long num = 123456;
            var num62 = HexMixHelper.ConvertDecTo62(num);
            var num10 = HexMixHelper.Convert62ToDec(num62);

            Assert.AreEqual(num, num10);
        }
    }
}