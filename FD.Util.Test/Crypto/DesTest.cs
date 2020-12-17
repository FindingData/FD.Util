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

       

        //[TestMethod]
        //public void Encrypt3DesTest()
        //{
        //    var key = "1234567890@data@";  

        //    var cipher = DESHelper.Encrypt3Des("cszfp.com", key, CipherMode.ECB);
        //    var clear = DESHelper.Decrypt3Des(cipher, key, CipherMode.ECB);
        //    Assert.AreEqual("cszfp.com", clear);
        //}

        //[TestMethod]
        //public void Encrypt3DesTest2()
        //{
        //    var key = "1234567890@data@"; 

        //    var cipher = DESHelper.Encrypt3Des("cszfp.com", key, CipherMode.CBC);
        //    var clear = DESHelper.Decrypt3Des(cipher, key, CipherMode.CBC);
        //    Assert.AreEqual("cszfp.com", clear);
        //}

        [TestMethod]
        public void Encrytp3DesTest3()
        {
            //var des = DESHelper.Get3DesKey();

            //var cipher = DESHelper.Encrypt3Des("cszfp.com", des.Key, des.IV, CipherMode.CBC);

            //var clear = DESHelper.Decrypt3Des(cipher, des.Key, des.IV, CipherMode.CBC);

            //Assert.AreEqual("cszfp.com", clear);

        }
    }
}
