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
            var rsa = new RSACryptoServiceProvider();


            var pubK = rsa.ToXmlString(false);

            var priK = rsa.ToXmlString(true);

            var cipher = RSAHelper.Encrypt("cszfp.com", pubK);

            var clear = RSAHelper.Decrypt(cipher, priK);

            Assert.AreEqual("cszfp.com", clear);

        }

        [TestMethod]
        public void ExportPublicKeyTest()
        {
            
            

        }
    }
}
