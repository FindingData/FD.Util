using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
            var publicKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var privateKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent><P>6aG9bMiBbgflnj9Rleng7vBQQPuWuuR7ETMw9XQnL1WLxEeZs8zn36Ph+ziSg6TDvOhBHlLC49lmHdvha0vJpw==</P><Q>92VsZInwwVfUVL/oyS87Ya4PR0FBRI9pZJyR6C4TTf7vTMCBC2XDV6uRC+m9RU/JypEuOsp8+WLgBVHlrCjiFw==</Q><DP>HvjCE9nAzsVdO01Jk4Ydu49AFF1F7iC779vJccCkMTI2BR840Q0o8AzZuGQXiDwfdruTZmGyVGJNl0e+6mpxoQ==</DP><DQ>R5wqBegPskdUBLwQC7wKOjoB3iQ7WjcQ0LipW0WK/PagGd1W/Q+VvZjBwWsFCD0SMfpYIVhfWGiQY7nS+0RSPQ==</DQ><InverseQ>MC5PTTSaiCRRGerW9CpWq6k+b1pBT5q3QO0TonmqPVoJ5dyprVgeHmLUPkmefKcqLQh8+5Bdw15fjJfI9g8iBg==</InverseQ><D>MOcemxA119j7aAga1ftpVFRvMpfd++xSMY6bA+aypm7phZuzQHYivqDinnAcSmxC8hJ8KkfOgzAtd2u6EeEWrmshZ04ZZ+doDc+aejLBcm+CtvqzW10loMIdbsoAYxGw+TV3P8sddRU7xQjbR6nLmcudzAFQiV3yoOe6Ynp7Wo0=</D></RSAKeyValue>";
             
            var cipher = RSAHelper.Encrypt("cszfp.com", publicKey);
            var clear = RSAHelper.Decrypt(cipher, privateKey);
            
            Assert.AreEqual("cszfp.com", clear);
        }




        [TestMethod]
        public void PublicXmlToPemTest()
        {
            var xmlKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var toPemKey = CryptoHelper.PublicXmlToPem(xmlKey).Trim();

            var pemKey = "-----BEGIN PUBLIC KEY-----\n" +
                        "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDhx51/rlslwtl6f+qc1SfVZfDM\n" +
                        "IkanzHWecpoGIGZiY3EIHpogPBOipYsWicv/gUcwMi0i3Dc9WQxYreEwn7lNmkHf\n" +
                        "ofnqJbToRhG1triu+IpvkoX+L59tESoTisAPEFkhv93WE/b1yIDsP8k/oJ3oWt4d\n" +
                        "ZrjYdodd96qwQeyMAQIDAQAB\n" +
                        "-----END PUBLIC KEY-----\n".Trim();          

            Assert.AreEqual<string>(toPemKey, pemKey);
        }


        [TestMethod]
        public void PrivateXmlToPemTest()
        {
            var xmlKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent><P>6aG9bMiBbgflnj9Rleng7vBQQPuWuuR7ETMw9XQnL1WLxEeZs8zn36Ph+ziSg6TDvOhBHlLC49lmHdvha0vJpw==</P><Q>92VsZInwwVfUVL/oyS87Ya4PR0FBRI9pZJyR6C4TTf7vTMCBC2XDV6uRC+m9RU/JypEuOsp8+WLgBVHlrCjiFw==</Q><DP>HvjCE9nAzsVdO01Jk4Ydu49AFF1F7iC779vJccCkMTI2BR840Q0o8AzZuGQXiDwfdruTZmGyVGJNl0e+6mpxoQ==</DP><DQ>R5wqBegPskdUBLwQC7wKOjoB3iQ7WjcQ0LipW0WK/PagGd1W/Q+VvZjBwWsFCD0SMfpYIVhfWGiQY7nS+0RSPQ==</DQ><InverseQ>MC5PTTSaiCRRGerW9CpWq6k+b1pBT5q3QO0TonmqPVoJ5dyprVgeHmLUPkmefKcqLQh8+5Bdw15fjJfI9g8iBg==</InverseQ><D>MOcemxA119j7aAga1ftpVFRvMpfd++xSMY6bA+aypm7phZuzQHYivqDinnAcSmxC8hJ8KkfOgzAtd2u6EeEWrmshZ04ZZ+doDc+aejLBcm+CtvqzW10loMIdbsoAYxGw+TV3P8sddRU7xQjbR6nLmcudzAFQiV3yoOe6Ynp7Wo0=</D></RSAKeyValue>";
            var toPemKey = CryptoHelper.PrivateXmlToPem(xmlKey).Trim();
            var pemKey = "-----BEGIN PRIVATE KEY-----\n"+
           "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAOHHnX+uWyXC2Xp/\n"+
           "6pzVJ9Vl8MwiRqfMdZ5ymgYgZmJjcQgemiA8E6KlixaJy/+BRzAyLSLcNz1ZDFit\n"+
           "4TCfuU2aQd+h+eoltOhGEbW2uK74im+Shf4vn20RKhOKwA8QWSG/3dYT9vXIgOw/\n"+
           "yT+gneha3h1muNh2h133qrBB7IwBAgMBAAECgYAw5x6bEDXX2PtoCBrV+2lUVG8y\n"+
           "l9377FIxjpsD5rKmbumFm7NAdiK+oOKecBxKbELyEnwqR86DMC13a7oR4RauayFn\n"+
           "Thln52gNz5p6MsFyb4K2+rNbXSWgwh1uygBjEbD5NXc/yx11FTvFCNtHqcuZy53M\n"+
           "AVCJXfKg57pientajQJBAOmhvWzIgW4H5Z4/UZXp4O7wUED7lrrkexEzMPV0Jy9V\n"+
           "i8RHmbPM59+j4fs4koOkw7zoQR5SwuPZZh3b4WtLyacCQQD3ZWxkifDBV9RUv+jJ\n"+
           "Lzthrg9HQUFEj2lknJHoLhNN/u9MwIELZcNXq5EL6b1FT8nKkS46ynz5YuAFUeWs\n"+
           "KOIXAkAe+MIT2cDOxV07TUmThh27j0AUXUXuILvv28lxwKQxMjYFHzjRDSjwDNm4\n"+
           "ZBeIPB92u5NmYbJUYk2XR77qanGhAkBHnCoF6A+yR1QEvBALvAo6OgHeJDtaNxDQ\n"+
           "uKlbRYr89qAZ3Vb9D5W9mMHBawUIPRIx+lghWF9YaJBjudL7RFI9AkAwLk9NNJqI\n"+
           "JFEZ6tb0KlarqT5vWkFPmrdA7ROieao9Wgnl3KmtWB4eYtQ+SZ58pyotCHz7kF3D\n" +
           "Xl+Ml8j2DyIG\n"+
           "-----END PRIVATE KEY-----\n".Trim();            
            Assert.AreEqual<string>(toPemKey, pemKey);
        }

        [TestMethod]
        public void PublicPemToXml()
        {
            var pemKey = "-----BEGIN PUBLIC KEY-----\n" +
                        "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDhx51/rlslwtl6f+qc1SfVZfDM\n" +
                        "IkanzHWecpoGIGZiY3EIHpogPBOipYsWicv/gUcwMi0i3Dc9WQxYreEwn7lNmkHf\n" +
                        "ofnqJbToRhG1triu+IpvkoX+L59tESoTisAPEFkhv93WE/b1yIDsP8k/oJ3oWt4d\n" +
                        "ZrjYdodd96qwQeyMAQIDAQAB\n" +
                        "-----END PUBLIC KEY-----\n".Trim();
            var toXmlKey = CryptoHelper.PublicPemToXml(pemKey).Trim();
            var xmlKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            Assert.AreEqual<string>(toXmlKey, xmlKey);
        }

        [TestMethod]
        public void PrivatePemToXml()
        {
            var pemKey = "-----BEGIN PRIVATE KEY-----\n" +
           "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAOHHnX+uWyXC2Xp/\n" +
           "6pzVJ9Vl8MwiRqfMdZ5ymgYgZmJjcQgemiA8E6KlixaJy/+BRzAyLSLcNz1ZDFit\n" +
           "4TCfuU2aQd+h+eoltOhGEbW2uK74im+Shf4vn20RKhOKwA8QWSG/3dYT9vXIgOw/\n" +
           "yT+gneha3h1muNh2h133qrBB7IwBAgMBAAECgYAw5x6bEDXX2PtoCBrV+2lUVG8y\n" +
           "l9377FIxjpsD5rKmbumFm7NAdiK+oOKecBxKbELyEnwqR86DMC13a7oR4RauayFn\n" +
           "Thln52gNz5p6MsFyb4K2+rNbXSWgwh1uygBjEbD5NXc/yx11FTvFCNtHqcuZy53M\n" +
           "AVCJXfKg57pientajQJBAOmhvWzIgW4H5Z4/UZXp4O7wUED7lrrkexEzMPV0Jy9V\n" +
           "i8RHmbPM59+j4fs4koOkw7zoQR5SwuPZZh3b4WtLyacCQQD3ZWxkifDBV9RUv+jJ\n" +
           "Lzthrg9HQUFEj2lknJHoLhNN/u9MwIELZcNXq5EL6b1FT8nKkS46ynz5YuAFUeWs\n" +
           "KOIXAkAe+MIT2cDOxV07TUmThh27j0AUXUXuILvv28lxwKQxMjYFHzjRDSjwDNm4\n" +
           "ZBeIPB92u5NmYbJUYk2XR77qanGhAkBHnCoF6A+yR1QEvBALvAo6OgHeJDtaNxDQ\n" +
           "uKlbRYr89qAZ3Vb9D5W9mMHBawUIPRIx+lghWF9YaJBjudL7RFI9AkAwLk9NNJqI\n" +
           "JFEZ6tb0KlarqT5vWkFPmrdA7ROieao9Wgnl3KmtWB4eYtQ+SZ58pyotCHz7kF3D\n" +
           "Xl+Ml8j2DyIG\n" +
           "-----END PRIVATE KEY-----\n".Trim();
            var toXmlKey = CryptoHelper.PrivatePemToXml(pemKey).Trim();
            var xmlKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent><P>6aG9bMiBbgflnj9Rleng7vBQQPuWuuR7ETMw9XQnL1WLxEeZs8zn36Ph+ziSg6TDvOhBHlLC49lmHdvha0vJpw==</P><Q>92VsZInwwVfUVL/oyS87Ya4PR0FBRI9pZJyR6C4TTf7vTMCBC2XDV6uRC+m9RU/JypEuOsp8+WLgBVHlrCjiFw==</Q><DP>HvjCE9nAzsVdO01Jk4Ydu49AFF1F7iC779vJccCkMTI2BR840Q0o8AzZuGQXiDwfdruTZmGyVGJNl0e+6mpxoQ==</DP><DQ>R5wqBegPskdUBLwQC7wKOjoB3iQ7WjcQ0LipW0WK/PagGd1W/Q+VvZjBwWsFCD0SMfpYIVhfWGiQY7nS+0RSPQ==</DQ><InverseQ>MC5PTTSaiCRRGerW9CpWq6k+b1pBT5q3QO0TonmqPVoJ5dyprVgeHmLUPkmefKcqLQh8+5Bdw15fjJfI9g8iBg==</InverseQ><D>MOcemxA119j7aAga1ftpVFRvMpfd++xSMY6bA+aypm7phZuzQHYivqDinnAcSmxC8hJ8KkfOgzAtd2u6EeEWrmshZ04ZZ+doDc+aejLBcm+CtvqzW10loMIdbsoAYxGw+TV3P8sddRU7xQjbR6nLmcudzAFQiV3yoOe6Ynp7Wo0=</D></RSAKeyValue>";
            Assert.AreEqual<string>(toXmlKey, xmlKey);
        }



       

        [TestMethod]
        public void RSACngTest()
        {
            byte[] keyData;
            var rsaKeyParameters = new CngKeyCreationParameters
            {
                ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                KeyUsage = CngKeyUsages.AllUsages,
                Parameters =
                {
                    new CngProperty("Length", BitConverter.GetBytes(2048), CngPropertyOptions.None)
                }
            };
            using (var key = CngKey.Create(CngAlgorithm.Rsa, null, rsaKeyParameters))
            {
                keyData = key.Export(CngKeyBlobFormat.Pkcs8PrivateBlob);
            }

            Console.WriteLine("-----BEGIN PRIVATE KEY-----");
            Console.WriteLine(Convert.ToBase64String(keyData, Base64FormattingOptions.InsertLineBreaks));
            Console.WriteLine("-----END PRIVATE KEY-----");

            string original = "Hello world!";
            Console.WriteLine(original);

            byte[] encrypted;

            using (var key = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (var rsa = new RSACng(key))
            {                
                encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(original), RSAEncryptionPadding.Pkcs1);
            }

            Console.WriteLine(Convert.ToBase64String(encrypted));

            string decrypted;

            using (var key = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (var rsa = new RSACng(key))
            {
                decrypted = Encoding.UTF8.GetString(rsa.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1));
            }

        }
    }
}
