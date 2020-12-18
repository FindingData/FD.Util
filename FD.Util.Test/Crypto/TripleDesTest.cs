using System;
using System.Security.Cryptography;
using System.Text;
using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FD.Util.Test
{
    [TestClass]
    public class TripleDesTest
    {

        [TestMethod]
        public void EncryptTripleDesTest()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 16); //16 24
            var strKey = Convert.ToBase64String(key);

            var cipher = TripleDesHelper.Encrypt("cszfp.com", strKey);
            var clear = TripleDesHelper.Decrypt(cipher, strKey);

            Assert.AreEqual("cszfp.com", clear);

        }
        
        [TestMethod]
        public void EncrytpTripleDesTest2()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 24);
            var iv = CryptoHelper.GenerateIv(8);

            var cipher = TripleDesHelper.Encrypt("cszfp.com", key, iv);

            var clear = TripleDesHelper.Decrypt(cipher, key, iv);

            Assert.AreEqual("cszfp.com", clear);

        }

    }
}
