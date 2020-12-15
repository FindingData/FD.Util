using System;
using System.Security.Cryptography;
using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FD.Util.Test
{
    [TestClass]
    public class DesTest
    {
        [TestMethod]
        public void EncryptKeyTest()
        {
            var key1 = DESHelper.GetDesKey();
            var key2 = DESHelper.GetDesKey();
            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void EncryptTest()
        {
            var key = "@fddata@";
            
            var cipher = DESHelper.Encrypt("cszfp.com", key);

            var clear = DESHelper.Decrypt(cipher, key);

            Assert.AreEqual("cszfp.com", clear);
            
        }

        [TestMethod]
        public void EncrytpTest2()
        {
            var des = DESHelper.GetDesKey();

            var cipher = DESHelper.Encrypt("cszfp.com", des.Key,des.IV);

            var clear = DESHelper.Decrypt(cipher, des.Key, des.IV);

            Assert.AreEqual("cszfp.com", clear);

        }

        [TestMethod]
        public void Encrypt3DesKeyTest()
        {
            var key1 = DESHelper.Get3DesKey();
            var key2 = DESHelper.Get3DesKey();
            Assert.AreNotEqual(key1, key2);
        }


        [TestMethod]
        public void Encrypt3DesTest()
        {
            var key = "1234567890@data@"; //128 bits,16 bytes

            var cipher = DESHelper.Encrypt3Des("cszfp.com", key, CipherMode.ECB);
            var clear = DESHelper.Decrypt3Des(cipher, key, CipherMode.ECB);
            Assert.AreEqual("cszfp.com", clear);
        }

        [TestMethod]
        public void Encrypt3DesTest2()
        {
            var key = "1234567890@data@"; //192 bits,16 bytes

            var cipher = DESHelper.Encrypt3Des("cszfp.com", key, CipherMode.CBC);
            var clear = DESHelper.Decrypt3Des(cipher, key, CipherMode.CBC);
            Assert.AreEqual("cszfp.com", clear);
        }

        [TestMethod]
        public void Encrytp3DesTest3()
        {
            var des = DESHelper.Get3DesKey();

            var cipher = DESHelper.Encrypt3Des("cszfp.com", des.Key, des.IV, CipherMode.CBC);

            var clear = DESHelper.Decrypt3Des(cipher, des.Key, des.IV, CipherMode.CBC);

            Assert.AreEqual("cszfp.com", clear);

        }
    }
}
