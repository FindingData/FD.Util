using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace FD.Util.Test
{
    [TestClass]
    public class AESTest
    {
        

        [TestMethod]
        public void EncryptTest()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 16);           
            var strKey = Encoding.UTF8.GetString(key);
            var cipher = AESHelper.Encrypt("cszfp.com", strKey);

            var clear = AESHelper.Decrypt(cipher, strKey);

            Assert.AreEqual("cszfp.com", clear);

        }


        [TestMethod]
        public void EncrytpTest2()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 16);
            var iv = CryptoHelper.GenerateIv(16);

            var cipher = AESHelper.Encrypt("cszfp.com",key,iv);

            var clear = AESHelper.Decrypt(cipher,key,iv);

            Assert.AreEqual("cszfp.com", clear);

        }


    }
}
