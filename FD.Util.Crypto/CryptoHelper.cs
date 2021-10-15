using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Crypto
{
    public static class CryptoHelper
    {

        //public static string GenerateRsaPublicKey(string containerName)
        //{
        //    // Create the CspParameters object and set the key container
        //    // name used to store the RSA key pair.
        //    var parameters = new CspParameters
        //    {
        //        KeyContainerName = containerName
        //    };

        //    // Create a new instance of RSACryptoServiceProvider that accesses
        //    // the key container MyKeyContainerName.
        //    using (var rsa = new RSACryptoServiceProvider(parameters))
        //    {
        //        return rsa.ToXmlString(false);
        //    }             
        //}

        //public static string GenerateRsaPrivateKey(string containerName)
        //{
        //    // Create the CspParameters object and set the key container
        //    // name used to store the RSA key pair.
        //    var parameters = new CspParameters
        //    {
        //        KeyContainerName = containerName
        //    };

        //    // Create a new instance of RSACryptoServiceProvider that accesses
        //    // the key container MyKeyContainerName.
        //    using (var rsa = new RSACryptoServiceProvider(parameters))
        //    {
        //        return rsa.ToXmlString(true);
        //    }
        //}

        //public static void DeleteRsaKey(string containerName)
        //{

        //    // Create the CspParameters object and set the key container
        //    // name used to store the RSA key pair.
        //    var parameters = new CspParameters
        //    {
        //        KeyContainerName = containerName
        //    };

        //    // Create a new instance of RSACryptoServiceProvider that accesses
        //    // the key container.
        //    using (var rsa = new RSACryptoServiceProvider(parameters))
        //    {
        //        // Delete the key entry in the container.
        //        rsa.PersistKeyInCsp = false;
        //        // Call Clear to release resources and delete the key from the container.
        //        rsa.Clear();
        //    };         
        //}

   
        /// <summary>
        /// convert pem public key to RSA private xml 
        /// </summary>
        /// <param name="publicPem"></param>
        /// <returns></returns>
        public static string PublicPemToXml(string publicPem)
        {
            using (var sr = new StringReader(publicPem))
            {
                var pr = new PemReader(sr);
                var publicKey = (RsaKeyParameters)pr.ReadObject();                
                using (RSA rsa = DotNetUtilities.ToRSA(publicKey))
                {
                    return rsa.ToXmlString(false);
                }
            }                                 
        }

        /// <summary>
        /// onvert pem private key to RSA private xml 
        /// </summary>
        /// <param name="privatePem"></param>
        /// <returns></returns>
        public static string PrivatePemToXml(string privatePem)
        {
            using (var sr = new StringReader(privatePem))
            {
                var pr = new PemReader(sr);                
                var keyPair = (RsaPrivateCrtKeyParameters)pr.ReadObject();      
                
                using (RSA rsa = DotNetUtilities.ToRSA(keyPair))
                {
                    return rsa.ToXmlString(true);
                }
            }
        }

        /// <summary>
        /// onvert RSA xml public key to pem public xml 
        /// </summary>
        /// <param name="privatePem"></param>
        /// <returns></returns>
        public static string PublicXmlToPem(string publicXml)
        {
            using (RSA rsa= RSA.Create())
            {               
                rsa.FromXmlString(publicXml);
                var publicKey = DotNetUtilities.GetRsaPublicKey(rsa);
                var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                var base64 = Convert.ToBase64String(publicKeyInfo.GetEncoded());
                StringBuilder b = new StringBuilder();
                b.Append("-----BEGIN PUBLIC KEY-----\n");
                for (int i = 0; i < base64.Length; i += 64)
                    b.Append($"{ base64.Substring(i, Math.Min(64, base64.Length - i)) }\n");
                b.Append("-----END PUBLIC KEY-----\n");
                return b.ToString();
                           
            }          
        }

        /// <summary>
        /// onvert RSA xml private key to pem private xml 
        /// </summary>
        /// <param name="privatePem"></param>
        /// <returns></returns>
        public static string PrivateXmlToPem(string privateXml)
        {
            using (RSA rsa = RSA.Create())
            {             
                rsa.FromXmlString(privateXml);
                var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                var base64 = Convert.ToBase64String(privateKeyInfo.GetEncoded());
                StringBuilder b = new StringBuilder();
                b.Append("-----BEGIN PRIVATE KEY-----\n");
                for (int i = 0; i < base64.Length; i += 64)
                    b.Append($"{ base64.Substring(i, Math.Min(64, base64.Length - i)) }\n");
                b.Append("-----END PRIVATE KEY-----\n");
                return b.ToString();
            }
        }
 

        //public static void ExportPrivateKey(RSACryptoServiceProvider csp, TextWriter outputStream)
        //{
        //    if (csp.PublicOnly) throw new ArgumentException("CSP does not contain a private key", "csp");
        //    var parameters = csp.ExportParameters(true);
        //    using (var stream = new MemoryStream())
        //    {
        //        var writer = new BinaryWriter(stream);
        //        writer.Write((byte)0x30); // SEQUENCE
        //        using (var innerStream = new MemoryStream())
        //        {
        //            var innerWriter = new BinaryWriter(innerStream);
        //            EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
        //            EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
        //            EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
        //            EncodeIntegerBigEndian(innerWriter, parameters.D);
        //            EncodeIntegerBigEndian(innerWriter, parameters.P);
        //            EncodeIntegerBigEndian(innerWriter, parameters.Q);
        //            EncodeIntegerBigEndian(innerWriter, parameters.DP);
        //            EncodeIntegerBigEndian(innerWriter, parameters.DQ);
        //            EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
        //            var length = (int)innerStream.Length;
        //            EncodeLength(writer, length);
        //            writer.Write(innerStream.GetBuffer(), 0, length);
        //        }

        //        var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
        //        outputStream.WriteLine("-----BEGIN RSA PRIVATE KEY-----");
        //        // Output as Base64 with lines chopped at 64 characters
        //        for (var i = 0; i < base64.Length; i += 64)
        //        {
        //            outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
        //        }
        //        outputStream.WriteLine("-----END RSA PRIVATE KEY-----");
        //    }
        //}

        //private static void EncodeLength(BinaryWriter stream, int length)
        //{
        //    if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
        //    if (length < 0x80)
        //    {
        //        // Short form
        //        stream.Write((byte)length);
        //    }
        //    else
        //    {
        //        // Long form
        //        var temp = length;
        //        var bytesRequired = 0;
        //        while (temp > 0)
        //        {
        //            temp >>= 8;
        //            bytesRequired++;
        //        }
        //        stream.Write((byte)(bytesRequired | 0x80));
        //        for (var i = bytesRequired - 1; i >= 0; i--)
        //        {
        //            stream.Write((byte)(length >> (8 * i) & 0xff));
        //        }
        //    }
        //}

        //private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        //{
        //    stream.Write((byte)0x02); // INTEGER
        //    var prefixZeros = 0;
        //    for (var i = 0; i < value.Length; i++)
        //    {
        //        if (value[i] != 0) break;
        //        prefixZeros++;
        //    }
        //    if (value.Length - prefixZeros == 0)
        //    {
        //        EncodeLength(stream, 1);
        //        stream.Write((byte)0);
        //    }
        //    else
        //    {
        //        if (forceUnsigned && value[prefixZeros] > 0x7f)
        //        {
        //            // Add a prefix zero to force unsigned if the MSB is 1
        //            EncodeLength(stream, value.Length - prefixZeros + 1);
        //            stream.Write((byte)0);
        //        }
        //        else
        //        {
        //            EncodeLength(stream, value.Length - prefixZeros);
        //        }
        //        for (var i = prefixZeros; i < value.Length; i++)
        //        {
        //            stream.Write(value[i]);
        //        }
        //    }
        //}



        /// <summary>
        /// Generates encryption key using passphrase.
        /// </summary>
        /// <remarks>link: http://stackoverflow.com/questions/667887/aes-in-asp-net-with-vb-net/668008#668008 </remarks>
        public static byte[] GenerateKey(string passphrase,int size,byte[] salt = null)
        {
            salt = salt ?? new byte[8];
            var gen = new Rfc2898DeriveBytes(passphrase,
                 salt,// use const byte[] as salt
                 10000,
                 HashAlgorithmName.SHA256
                );
            var key = gen.GetBytes(size);            
            return key;
        }

        /// <summary>
        /// Generates a random IV.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Random nonce.</returns>        
        public static byte[] GenerateIv(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] nonce = new byte[length];
                rng.GetBytes(nonce);
                return nonce;
            }
        }


        /// <summary>
        /// Parses IV from [IV-DATA] byte array.
        /// </summary>
        /// <param name="ivdata">[IV]-[DATA] byte array.</param>
        /// <param name="length"></param>
        /// <returns>IV byte array.</returns>
        public static  byte[] GetIv(byte[] ivdata,int length)
        {            
            byte[] iv = new byte[length];
            Array.Copy(sourceArray: ivdata, sourceIndex: 0,
                destinationArray: iv, destinationIndex: 0,
                length: length);
            return iv;
        }


        /// <summary>
        /// Removes IV from [IV]-[DATA] byte array.
        /// </summary>
        /// <param name="ivdata">[IV]-[DATA] byte array.</param>
        /// <param name="length"></param>
        /// <returns>[DATA] byte array.</returns>
        public static byte[] RemoveIv(byte[] ivdata,int length)
        {
            byte[] data = new byte[ivdata.Length - length];
            Array.Copy(sourceArray: ivdata, sourceIndex: length,
                destinationArray: data, destinationIndex: 0,
                length: ivdata.Length - length);
            return data;
        }

        /// <summary>
        /// Combine IV with encrypted [DATA] to get [IV]-[DATA] byte array.
        /// </summary>
        /// <param name="iv"></param>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CombineIvData(byte[] iv, byte[] data,int length)
        {
            byte[] ivdata = new byte[data.Length + iv.Length];
            iv.CopyTo(array: ivdata, index: 0);
            data.CopyTo(array: ivdata, index: length);
            return ivdata;
        }

        /// <summary>
        /// Combine byte array
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }



        /// <summary>
        /// 生成随机的0-9a-zA-Z字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateKeys()
        {
            string[] Chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
            int SeekSeek = unchecked((int)DateTime.Now.Ticks);
            Random SeekRand = new Random(SeekSeek);
            for (int i = 0; i < 100000; i++)
            {
                int r = SeekRand.Next(1, Chars.Length);
                string f = Chars[0];
                Chars[0] = Chars[r - 1];
                Chars[r - 1] = f;
            }
            return string.Join("", Chars);
        }

    }
}
