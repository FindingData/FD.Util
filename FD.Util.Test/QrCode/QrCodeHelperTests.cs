using Microsoft.VisualStudio.TestTools.UnitTesting;
using FD.Util.QrCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FD.Util.QrCode.Tests
{
    [TestClass()]
    public class QrCodeHelperTests
    {
        [TestMethod()]
        public void GenerateQrCodeTest()
        {
            QrCodeHelper qrHelper = new QrCodeHelper();
            var textImg = qrHelper.ConvertStringToImage("www.baidu.com");
            var path1 = Path.Combine(System.Environment.CurrentDirectory, "text1.png");
            textImg.Save(path1);
            var qrImg =  qrHelper.GenerateQrCodeImg("https://www.baidu.com/", "2402");
            //var qrImg = qrHelper.GenerateQRCode("https://www.baidu.com/");
            var path = Path.Combine(System.Environment.CurrentDirectory,"code1.png");
            qrImg.Save(path);            
            //qrImg.Save("");

        }
    }
}