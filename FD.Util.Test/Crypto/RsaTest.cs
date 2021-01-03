using System;
using System.Security.Cryptography;
using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FD.Util.Test
{
    [TestClass]
    public class RsaTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            var publicKey = CryptoHelper.GenerateRsaPublicKey("fd@123.com");
            var privateKey = CryptoHelper.GenerateRsaPrivateKey("fd@123.com");

            var cipher = RSAHelper.Encrypt("cszfp.com", publicKey);
            var clear = RSAHelper.Decrypt(cipher, privateKey);
            
            Assert.AreEqual("cszfp.com", clear);
        }


        [TestMethod]
        public void EncryptTest2()
        {

        }


        [TestCleanup]
        public void ClearTest()
        {
            CryptoHelper.DeleteRsaKey("fd@123.com");
        }

        
    }
}
