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
            var publicKey = CryptoHelper.GenerateRsaPublicKey("fd@123.com");
            var privateKey = CryptoHelper.GenerateRsaPrivateKey("fd@123.com");

            var cipher = RSAHelper.Encrypt("cszfp.com", publicKey);
            var clear = RSAHelper.Decrypt(cipher, privateKey);
            
            Assert.AreEqual("cszfp.com", clear);
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


        [TestCleanup]
        public void ClearTest()
        {
            CryptoHelper.DeleteRsaKey("fd@123.com");
        }

        
    }
}
