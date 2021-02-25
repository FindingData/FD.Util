using System;
using System.Collections.Generic;
using System.IO;
using FD.Util.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FD.Util.Test.Http
{
    [TestClass]
    public class HttpTests
    {
        [TestMethod]
        public void UploadTest()
        {
            HttpHelper client = new HttpHelper("http://localhost:5150");
                   var filePath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\tmp.txt";
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                client.UploadFile("tmp.txt", fileData, "/api/tfs/upload");
            } 
        }


        [TestMethod]
        public void DeleteTest()
        {
            HttpHelper client = new HttpHelper("http://localhost:5150");
            var dic = new Dictionary<string, string>();
            dic.Add("file_name", "4d3a132dd28926019b5d446e26ccaeb5.txt");
            client.Delete(dic, "/api/tfs/upload");
        }
    }
}
