using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FD.Util.Crypto
{
    /// <summary>
    /// 非对称RSA加密类 可以参考
    /// http://www.cnblogs.com/hhh/archive/2011/06/03/2070692.html
    /// http://blog.csdn.net/zhilunchen/article/details/2943158
    /// 若是私匙加密 则需公钥解密
    /// 反正公钥加密 私匙来解密
    /// 需要BigInteger类来辅助
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>
        /// RSA的容器 可以解密的源字符串长度为 DWKEYSIZE/8-11 
        /// </summary>
        public const int DWKEYSIZE = 1024;

        /// <summary>
        /// RSA加密的密匙结构  公钥和私匙
        /// </summary>
        public struct RSAKey
        {
            /// <summary>
            /// 公钥 <RSAKeyValue><Modulus>uSybhYzVbwTkuHrhsaGtQ5MblIKGmUTuvrnSHzZG8bJ5Zzxqdj+SNXKyQGiOZl4B6qK2znS+mq8O9vSV3LhKkw0PNE4CJ7e/zb5WXpxa0XuQvVBNu6obXs+AHkVZgRc26iHfKE8UGQAD7Ir4Zu7gLASEvLqWAArGQPA+IZws2z8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>
            /// </summary>
            public string PublicKey { get; set; }
            /// <summary>
            /// 私钥 <RSAKeyValue><Modulus>uSybhYzVbwTkuHrhsaGtQ5MblIKGmUTuvrnSHzZG8bJ5Zzxqdj+SNXKyQGiOZl4B6qK2znS+mq8O9vSV3LhKkw0PNE4CJ7e/zb5WXpxa0XuQvVBNu6obXs+AHkVZgRc26iHfKE8UGQAD7Ir4Zu7gLASEvLqWAArGQPA+IZws2z8=</Modulus><Exponent>AQAB</Exponent><P>4iWYLK8mlQW5LLDu/+8lGGc4g8pdqXP8KJsLAI1Yhbnc8Ni4+xwtuPPQKxP2muWF1NUj9MhU0Wjs3n2ZneRMjw==</P><Q>0Z5ilATWIu4U1fJcwsFGXbl1N9c1NVmZDsNxIOnCWp6mGXrS05ZYth8piM8uFHYKueVCNecZQyEPQEW5vDo+UQ==</Q><DP>bdorOJGAYWVdVRtBjatCTr9dUY+MvdKsi6D80DDY7mShsZDlEn3WrAArf7F72jRiNUev5qklom+gmFdUtsD+ew==</DP><DQ>pfJYokwN5otK7XE4pGn0NgC7XqC+G+U5ualaJy9IUQtl/afxvdY5lrym2gsCtOoaZb6soxW4Nx+1/jD08KG/cQ==</DQ><InverseQ>WSKkSBxLkIiq5+BmkJ+7yiqFd7J+FVBl/Qbjbs7ddn/tQp/2SZsbIQABT1mSg1d/J1XAcCwA7sJdpsA9rBnyAA==</InverseQ><D>Xzxr+DHIi8Kuh5rbfOo0HG8KXkULSMTFLV6QjPDzZ9dszQWrk4l6fvawaqPbqRZsUerBrkkoxpGGVsjQUqaN41dTcxQTwdWxa6NzpBxCoRnm2nySVqDJLweJNMTT6dIKbGcVrLXyeNkGsz6fdmUIpJPeLsrvwN4tfQ9n47Dhf8E=</D></RSAKeyValue>
            /// </summary>
            public string PrivateKey { get; set; }
            public string PublicKeyExponent { get; set; }
            public string PublicKeyModulus { get; set; }
        }

        #region 得到RSA的解谜的密匙对
        /// <summary>
        /// 得到RSA的解谜的密匙对
        /// </summary>
        /// <returns></returns>
        public static RSAKey GetRASKey()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            //声明一个指定大小的RSA容器
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(DWKEYSIZE);
            //取得RSA容易里的各种参数
            RSAParameters p = rsaProvider.ExportParameters(true);

            return new RSAKey()
            {
                PrivateKey = ToBase64Key(rsaProvider.ToXmlString(true)),
                PublicKey = ToBase64Key(rsaProvider.ToXmlString(false)),
                PublicKeyExponent = BytesToHexString(p.Exponent),
                PublicKeyModulus = BytesToHexString(p.Modulus)
            };
        }
        #endregion

        #region 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
        /// <summary>
        /// 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool CheckSourceValidate(string source)
        {
            return (DWKEYSIZE / 8 - 11) >= source.Length;
        }
        #endregion

        #region 编码和还原密匙
        private static string ToBase64Key(string key)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(key);
            return Convert.ToBase64String(b);
        }

        private static string FromBase64Key(string key)
        {
            //从base64字符串 解析成原来的字节数组
            byte[] b = Convert.FromBase64String(key);
            return System.Text.Encoding.ASCII.GetString(b);
        }
        #endregion

        #region 字符串加密解密 公开方法
        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="source">源字符串 明文</param>
        /// <param name="publicKey">公匙或私匙</param>
        /// <returns>加密遇到错误将会返回原字符串</returns>
        public static string EncryptString(string source, string key)
        {
            string encryptString = string.Empty;
            try
            {
                if (!CheckSourceValidate(source))
                {
                    throw new Exception("source string too long");
                }
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                rsaProvider.FromXmlString(FromBase64Key(key));
                byte[] data = rsaProvider.Encrypt(System.Text.Encoding.ASCII.GetBytes(source), false);
                encryptString = BytesToHexString(data);
            }
            catch { }
            return encryptString;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="encryptString">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>遇到解密失败将会返回原字符串</returns>
        public static string DecryptString(string encryptString, string privateKey)
        {
            string source = string.Empty;
            try
            {
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                rsaProvider.FromXmlString(FromBase64Key(privateKey));
                byte[] data = rsaProvider.Decrypt(HexStringToBytes(encryptString), false);
                source = System.Text.Encoding.ASCII.GetString(data);
            }
            catch { }
            return source;
        }
        #endregion

        #region 配套JavaScript RSA使用
        private static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        private static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }
        #endregion


        /////Source: https://stackoverflow.com/questions/17128038/c-sharp-rsa-encryption-decryption-with-transmission
        //public static void GenerateKeys()
        //{
        //    //CSP with a new 2048 bit rsa key pair
        //    var csp = new RSACryptoServiceProvider(2048);

        //    //Private key
        //    var privKey = csp.ExportParameters(true);

        //    //Public key
        //    var pubKey = csp.ExportParameters(false);

        //    string privKeyString = string.Empty;

        //    var sw = new System.IO.StringWriter();
        //    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
        //    xs.Serialize(sw, privKey);
        //    privKeyString = sw.ToString();

        //    //This will give public key in the following format which is required by the JS library
        //    //-----BEGIN PUBLIC KEY-----
        //    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //    //-----END PUBLIC KEY-----
        //    string publicKeyBase64 = ConvertPublicKeyForJS(pubKey);

        //    //Will be saved in sesssion for later use during decryption
        //    string privateKeyBase64 = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(privKeyString));

        //    //Save in session/temp location
        //    //Return publicKeyBase64 to JS
        //}

        /// <summary>
        /// Source: https://stackoverflow.com/questions/28406888/c-sharp-rsa-public-key-output-not-correct/28407693#28407693
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string ConvertPublicKeyForJS(RSAParameters publicKey)
        {
            string output = string.Empty;

            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, publicKey.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, publicKey.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("-----BEGIN PUBLIC KEY-----");
                sb.AppendLine(base64);
                sb.AppendLine("-----END PUBLIC KEY-----");

                output = sb.ToString();
            }

            return output;
        }


        public static string ExportPublicKey(RSACryptoServiceProvider csp)
        {          
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.D
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.P
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.Q
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DP
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DQ
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.InverseQ
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                char[] base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                StringBuilder res = new StringBuilder();
                res.AppendLine("-----BEGIN RSA PUBLIC KEY-----");
                for (int i = 0; i < base64.Length; i += 64)
                {
                    int l = Math.Min(64, base64.Length - i);
                    for (int j = 0; j < l; j++) res.Append(base64[i + j]);
                    res.AppendLine();
                }
                res.AppendLine("-----END RSA PUBLIC KEY-----");
                return res.ToString();
            }
        }      


        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
 
    }
}