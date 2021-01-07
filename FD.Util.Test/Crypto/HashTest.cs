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

    }
}
