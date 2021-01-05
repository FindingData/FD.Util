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
        public void EncryptTest4()
        {
            var xmlKey = @"<RSAKeyValue><Modulus>4cedf65bJcLZen/qnNUn1WXwzCJGp8x1nnKaBiBmYmNxCB6aIDwToqWLFonL/4FHMDItItw3PVkMWK3hMJ+5TZpB36H56iW06EYRtba4rviKb5KF/i+fbREqE4rADxBZIb/d1hP29ciA7D/JP6Cd6FreHWa42HaHXfeqsEHsjAE=</Modulus><Exponent>AQAB</Exponent><P>6aG9bMiBbgflnj9Rleng7vBQQPuWuuR7ETMw9XQnL1WLxEeZs8zn36Ph+ziSg6TDvOhBHlLC49lmHdvha0vJpw==</P><Q>92VsZInwwVfUVL/oyS87Ya4PR0FBRI9pZJyR6C4TTf7vTMCBC2XDV6uRC+m9RU/JypEuOsp8+WLgBVHlrCjiFw==</Q><DP>HvjCE9nAzsVdO01Jk4Ydu49AFF1F7iC779vJccCkMTI2BR840Q0o8AzZuGQXiDwfdruTZmGyVGJNl0e+6mpxoQ==</DP><DQ>R5wqBegPskdUBLwQC7wKOjoB3iQ7WjcQ0LipW0WK/PagGd1W/Q+VvZjBwWsFCD0SMfpYIVhfWGiQY7nS+0RSPQ==</DQ><InverseQ>MC5PTTSaiCRRGerW9CpWq6k+b1pBT5q3QO0TonmqPVoJ5dyprVgeHmLUPkmefKcqLQh8+5Bdw15fjJfI9g8iBg==</InverseQ><D>MOcemxA119j7aAga1ftpVFRvMpfd++xSMY6bA+aypm7phZuzQHYivqDinnAcSmxC8hJ8KkfOgzAtd2u6EeEWrmshZ04ZZ+doDc+aejLBcm+CtvqzW10loMIdbsoAYxGw+TV3P8sddRU7xQjbR6nLmcudzAFQiV3yoOe6Ynp7Wo0=</D></RSAKeyValue>";
            var toPemKey = CryptoHelper.ToBase64XmlFromPem(xmlKey);
            var pemKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAOHHnX+uWyXC2Xp/
6pzVJ9Vl8MwiRqfMdZ5ymgYgZmJjcQgemiA8E6KlixaJy/+BRzAyLSLcNz1ZDFit
4TCfuU2aQd+h+eoltOhGEbW2uK74im+Shf4vn20RKhOKwA8QWSG/3dYT9vXIgOw/
yT+gneha3h1muNh2h133qrBB7IwBAgMBAAECgYAw5x6bEDXX2PtoCBrV+2lUVG8y
l9377FIxjpsD5rKmbumFm7NAdiK+oOKecBxKbELyEnwqR86DMC13a7oR4RauayFn
Thln52gNz5p6MsFyb4K2+rNbXSWgwh1uygBjEbD5NXc/yx11FTvFCNtHqcuZy53M
AVCJXfKg57pientajQJBAOmhvWzIgW4H5Z4/UZXp4O7wUED7lrrkexEzMPV0Jy9V
i8RHmbPM59+j4fs4koOkw7zoQR5SwuPZZh3b4WtLyacCQQD3ZWxkifDBV9RUv+jJ
Lzthrg9HQUFEj2lknJHoLhNN/u9MwIELZcNXq5EL6b1FT8nKkS46ynz5YuAFUeWs
KOIXAkAe+MIT2cDOxV07TUmThh27j0AUXUXuILvv28lxwKQxMjYFHzjRDSjwDNm4
ZBeIPB92u5NmYbJUYk2XR77qanGhAkBHnCoF6A+yR1QEvBALvAo6OgHeJDtaNxDQ
uKlbRYr89qAZ3Vb9D5W9mMHBawUIPRIx+lghWF9YaJBjudL7RFI9AkAwLk9NNJqI
JFEZ6tb0KlarqT5vWkFPmrdA7ROieao9Wgnl3KmtWB4eYtQ+SZ58pyotCHz7kF3D
Xl+Ml8j2DyIG
-----END RSA PRIVATE KEY-----";
            Assert.AreEqual(toPemKey, pemKey);
        }


        [TestMethod]
        public void EncryptTest2()
        {
            var pckKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCPCCpYxo4A96so+tuggeqcLpnfnDvlOKvDDc4dyQzNK9JXbzFQFdhJRJxs7/AG/rJk
lnGrMpddjCWMw4csonl1swCIsm/DdObhpBMXWe6T7YhMfQh4vc4Vj+7DuhxotoEHFiLXPkgcVYIiyGAgtz+zwok32MUaXYU1H6ZE5XPxBQIDAQAB";

            var xmlKey = @"PFJTQUtleVZhbHVlPjxNb2R1bHVzPnVTeWJoWXpWYndUa3VIcmhzYUd0UTVNYmxJS0dtVVR1dnJuU0h6Wkc4Yko1Wnp4cWRqK1NOWEt5UU
dpT1psNEI2cUsyem5TK21xOE85dlNWM0xoS2t3MFBORTRDSjdlL3piNVdYcHhhMFh1UXZWQk51Nm9iWHMrQUhrVlpnUmMyNmlIZktFOFVHUUFEN0lyNFp1N2dMQVNFdkxxV0F
BckdRUEErSVp3czJ6OD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+";


            var privateKey = @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAI8IKljGjgD3qyj626CB6pwumd+cO+U4q8MNzh3JDM0r0ldvMVAV2ElEnGzv8Ab+smSWcasyl12MJYzDhyyieXWzAIiyb8N05uGkExdZ7pPtiEx9CHi9zhWP7sO6HGi2gQcWItc+SBxVgiLIYCC3P7PCiTfYxRpdhTUfpkTlc/EFAgMBAAECgYA9aYkdIuuX2L9h0gv4568/Lfcdfqxs6B8/ma7VNRzM/zLKHdDzjN2X3+C2GXNa3YLlE39svUUmgmUhAlcIwB4b2dMkW344hWdyxlq++U7PB9PhjdQLOaLJ9IVYFdeeXWapVsu5MFYtDBGekdm059oiGU6jp+wKAQbcpoEBqDOhIQJBAMDJ9mb4EXESdFKH8VKIscNWVqgORWSQLZxh+tGbkUFPR71D9OEa3H9zdudE62s5hUsupl9Cu2nouGrSiiWmiCkCQQC97cUi0qm/oj4VyQAzQDdG7S2heEj3tuQSq6Eq9J3zeqr20wC2nOSMNIxjT1lrOEPHMpDnzaoByWVzBs1vGm19AkBbI58O0ps8PyMqmQWmpOSUmc5hqE152wcU9OTkDo0+uMILYYL0oAQ5ZFaDwnsgt5KiFi2kvyFmUhRMu7d/URjxAkEAk+1xMaIqnBtdh9I52bEWXKO2eqEZE/baxL/wk2ha7ZyJNB073P9t8tWq0l6nIp98CtYTjrfcxe96mJCfyP0ocQJBAJD9eQuoHiOoEO8s4IXPLC0EmZuA3EXQ1c6GTMrVXdcOdpo6rY1ozSRNN7l+tbDjDHDR79uAuvaH+sib50jKDCY=";


            

            var privateKey2 = @"PFJTQUtleVZhbHVlPjxNb2R1bHVzPnVTeWJoWXpWYndUa3VIcmhzYUd0UTVNYmxJS0dtVVR1dnJuU0h6Wkc4Yko1Wnp4cWRqK1NOWEt5UUdpT1psNEI2cUsyem5TK21xOE85dlNWM0xoS2t3MFBORTRDSjdlL3piNVdYcHhhMFh1UXZWQk51Nm9iWHMrQUhrVlpnUmMyNmlIZktFOFVHUUFEN0lyNFp1N2dMQVNFdkxxV0FBckdRUEErSVp3czJ6OD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjRpV1lMSzhtbFFXNUxMRHUvKzhsR0djNGc4cGRxWFA4S0pzTEFJMVloYm5jOE5pNCt4d3R1UFBRS3hQMm11V0YxTlVqOU1oVTBXanMzbjJabmVSTWp3PT08L1A+PFE+MFo1aWxBVFdJdTRVMWZKY3dzRkdYYmwxTjljMU5WbVpEc054SU9uQ1dwNm1HWHJTMDVaWXRoOHBpTTh1RkhZS3VlVkNOZWNaUXlFUFFFVzV2RG8rVVE9PTwvUT48RFA+YmRvck9KR0FZV1ZkVlJ0QmphdENUcjlkVVkrTXZkS3NpNkQ4MEREWTdtU2hzWkRsRW4zV3JBQXJmN0Y3MmpSaU5VZXY1cWtsb20rZ21GZFV0c0QrZXc9PTwvRFA+PERRPnBmSllva3dONW90SzdYRTRwR24wTmdDN1hxQytHK1U1dWFsYUp5OUlVUXRsL2FmeHZkWTVscnltMmdzQ3RPb2FaYjZzb3hXNE54KzEvakQwOEtHL2NRPT08L0RRPjxJbnZlcnNlUT5XU0trU0J4TGtJaXE1K0Jta0orN3lpcUZkN0orRlZCbC9RYmpiczdkZG4vdFFwLzJTWnNiSVFBQlQxbVNnMWQvSjFYQWNDd0E3c0pkcHNBOXJCbnlBQT09PC9JbnZlcnNlUT48RD5YenhyK0RISWk4S3VoNXJiZk9vMEhHOEtYa1VMU01URkxWNlFqUER6Wjlkc3pRV3JrNGw2ZnZhd2FxUGJxUlpzVWVyQnJra294cEdHVnNqUVVxYU40MWRUY3hRVHdkV3hhNk56cEJ4Q29Sbm0ybnlTVnFESkx3ZUpOTVRUNmRJS2JHY1ZyTFh5ZU5rR3N6NmZkbVVJcEpQZUxzcnZ3TjR0ZlE5bjQ3RGhmOEU9PC9EPjwvUlNBS2V5VmFsdWU+";


            var publicKey3 = @"<RSAKeyValue>
  <Modulus>o9AYbnx0a9YHjhnaE8E8w1plNsmyqg2tW41sakkoELxRz5PlDAsmc31caXUveSzBY8obp5Jt0hF9EIqY5wzLZQ==</Modulus>
  <Exponent>AQAB</Exponent>
</RSAKeyValue>";



            var s1 = Encoding.ASCII.GetString(Convert.FromBase64String(pckKey));

            var s2 = Encoding.UTF8.GetString(Convert.FromBase64String(xmlKey));

            var s3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey3));

            var s4 = Encoding.ASCII.GetString(Convert.FromBase64String(s3));

            //var key = CngKey.Create(CngAlgorithm.Rsa, null);
            //var data = key.Export(CngKeyBlobFormat.Pkcs8PrivateBlob);
            //var str = Convert.ToBase64String(data);

            //var rsa = new RSACng(key);
            //var encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes("cszfp.com"), RSAEncryptionPadding.Pkcs1);


            var s = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
      @"Microsoft\Crypto\RSA\MachineKeys");



        }


        [TestMethod]
        public void Test3()
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
