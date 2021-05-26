using FD.Util.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace FD.Util.Test
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void Md5Test()
        {
            var str = "cszfp.com";
            var md5 = HashHelper.Md5(Encoding.UTF8.GetBytes(str));
            var md5Str = "5bab05138f541d8d065813d1df9651a3";
            Assert.AreEqual(md5Str, md5);
        }

        [TestMethod]
        public void Md5StringTest()
        {
            var str = "cszfp.com";
            var md5 = HashHelper.Md5(str);
            var md5Str = "5bab05138f541d8d065813d1df9651a3";
            Assert.AreEqual(md5Str, md5);
        }

        [TestMethod]
        public void Md5FileTest()
        {
            var strFile = Environment.CurrentDirectory + @"/Resources/Md5File.txt";

            var md5 = HashHelper.Md5File(strFile);
            var md5Str = "4872c5e2ef9887910cbc78524d7b661d";
            Assert.AreEqual(md5Str, md5);            
        }

        [TestMethod]
        public void Md5SaltTest()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";          
            var md5 = HashHelper.Md5WithSalt(str,Encoding.UTF8.GetBytes(salt));
            var md5Str = HashHelper.Md5WithSalt(str, Encoding.UTF8.GetBytes(salt));
            Assert.AreEqual(md5Str, md5);
        }


        [TestMethod]
        public void Sha1Test()
        {
            var str = "cszfp.com";
            var Sha1 = HashHelper.Sha1(Encoding.UTF8.GetBytes(str));
            var Sha1Str = "c1e23962f9b00bed4c1f8dadd44b6c40b474a853";
            Assert.AreEqual(Sha1Str, Sha1);
        }

        [TestMethod]
        public void Sha1StringTest()
        {
            var str = "cszfp.com";
            var Sha1 = HashHelper.Sha1(str);
            var Sha1Str = "c1e23962f9b00bed4c1f8dadd44b6c40b474a853";
            Assert.AreEqual(Sha1Str, Sha1);
        }

        [TestMethod]
        public void Sha1FileTest()
        {
            var strFile = Environment.CurrentDirectory + @"/Resources/Md5File.txt";

            var Sha1 = HashHelper.Sha1File(strFile);
            var Sha1Str = "e237e45012f4281b8f7cc9ba4519ec40292b9683";
            Assert.AreEqual(Sha1Str, Sha1);
        }

        [TestMethod]
        public void Sha1SaltTest()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";
            var Sha1 = HashHelper.Sha1WithSalt(str, Encoding.UTF8.GetBytes(salt));
            var Sha1Str = HashHelper.Sha1WithSalt(str, Encoding.UTF8.GetBytes(salt));
            Assert.AreEqual(Sha1Str, Sha1);
        }



        [TestMethod]
        public void Sha256Test()
        {
            var str = "cszfp.com";
            var Sha256 = HashHelper.Sha256(Encoding.UTF8.GetBytes(str));
            var Sha256Str = "e033efb657434268ffc7018f20feb18f9c8ffd3530fff8cded37a2be9ab36aa4";
            Assert.AreEqual(Sha256Str, Sha256);
        }

        [TestMethod]
        public void Sha256StringTest()
        {
            var str = "cszfp.com";
            var Sha256 = HashHelper.Sha256(str);
            var Sha256Str = "e033efb657434268ffc7018f20feb18f9c8ffd3530fff8cded37a2be9ab36aa4";
            Assert.AreEqual(Sha256Str, Sha256);
        }

        [TestMethod]
        public void Sha256FileTest()
        {
            var strFile = Environment.CurrentDirectory + @"/Resources/Md5File.txt";

            var Sha256 = HashHelper.Sha256File(strFile);
            var Sha256Str = "bd2382a9698532a2e22f755725317530e7de7659e570dcb389d18e27fd4a72f5";
            Assert.AreEqual(Sha256Str, Sha256);
        }

        [TestMethod]
        public void Sha256SaltTest()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";
            var Sha256 = HashHelper.Sha256WithSalt(str, Encoding.UTF8.GetBytes(salt));
            var Sha256Str = HashHelper.Sha256WithSalt(str, Encoding.UTF8.GetBytes(salt));
            Assert.AreEqual(Sha256Str, Sha256);
        }

        [TestMethod]
        public void HmacSha1Test()
        {
            var str = "cszfp.com";
            var salt = "SK1BtAloxIaQUgv9m0gjqrQJ0NJikkPPUTlRGOzp";
            var HmacSha1 = HashHelper.HmacSha1(Encoding.UTF8.GetBytes(str), Encoding.UTF8.GetBytes(salt));

            var HmacSha1Str = "fc9afbda6e559a1b378dd1b74db7e2251784ed42";
            Assert.AreEqual(HmacSha1Str, HmacSha1);

            var HmacSha1Base64 = Convert.ToBase64String(HmacSha1.HexStringToHex());

            var HmacSha1Base64Str = "/Jr72m5Vmhs3jdG3TbfiJReE7UI=";
            
            Assert.AreEqual(HmacSha1Base64Str, HmacSha1Base64);

        }

       

                [TestMethod]
        public void HmacSha1StringTest()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";

            var HmacSha256 = HashHelper.HmacSha1(str, Encoding.UTF8.GetBytes(salt));
            var HmacSha256Str = "850b40da03da5239e102c076c373b038bd6dd88668688324a4c46ab316f23a3c";
            Assert.AreEqual(HmacSha256Str, HmacSha256);
        }


        [TestMethod]
        public void HmacSha256Test()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";
            var HmacSha256 = HashHelper.HmacSha256(Encoding.UTF8.GetBytes(str), Encoding.UTF8.GetBytes(salt));

            var HmacSha256Str = "850b40da03da5239e102c076c373b038bd6dd88668688324a4c46ab316f23a3c";
            Assert.AreEqual(HmacSha256Str, HmacSha256);
        }

        [TestMethod]
        public void HmacSha256StringTest()
        {
            var str = "cszfp.com";
            var salt = "oWsc7Osv4TA=";

            var HmacSha256 = HashHelper.HmacSha256(str, Encoding.UTF8.GetBytes(salt));
            var HmacSha256Str = "850b40da03da5239e102c076c373b038bd6dd88668688324a4c46ab316f23a3c";
            Assert.AreEqual(HmacSha256Str, HmacSha256);
        }

        [TestMethod]
        public void HmacSha256FileTest()
        {
            var strFile = Environment.CurrentDirectory + @"/Resources/Md5File.txt";
            var salt = "oWsc7Osv4TA=";

            var HmacSha256 = HashHelper.HmacSha256File(strFile, Encoding.UTF8.GetBytes(salt));
            var HmacSha256Str = "20b265e9406307c0ff9d4463dbf78d67d15b94b646e7e37cb4d31e6ab6a7d24c";
            Assert.AreEqual(HmacSha256Str, HmacSha256);
        }


    }
}
