using System;
using System.Security.Cryptography;
using System.Text;
using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FD.Util.Test
{
    [TestClass]
    public class DesTest
    {
       
        [TestMethod]
        public void EncryptTest()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 8);          

            var strKey = Convert.ToBase64String(key);           

            var cipher = DESHelper.Encrypt("cszfp.com", strKey);

            var clear = DESHelper.Decrypt(cipher, strKey);

            Assert.AreEqual("cszfp.com", clear);
            
        }

        [TestMethod]
        public void EncrytpTest2()
        {
            var key = CryptoHelper.GenerateKey("fd@123.com", 8);
            var iv = CryptoHelper.GenerateIv(8);

            var cipher = DESHelper.Encrypt("cszfp.com", key,iv);

            var clear = DESHelper.Decrypt(cipher, key,iv);

            Assert.AreEqual("cszfp.com", clear);

        }



      
    }
}
