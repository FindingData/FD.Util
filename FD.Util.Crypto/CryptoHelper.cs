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

        public static string ToBase64PemFromXML(string xml)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xml);
                
                AsymmetricCipherKeyPair keyPair =  DotNetUtilities.GetRsaKeyPair(rsa); // try get private and public key pair
                if (keyPair != null) // if XML RSA key contains private key
                {
                    PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                    return FormatPem(Convert.ToBase64String(privateKeyInfo.GetEncoded()), "PRIVATE KEY");

                }

                RsaKeyParameters publicKey = Org.BouncyCastle.Security.DotNetUtilities.GetRsaPublicKey(rsa); // try get public key
                if (publicKey != null) // if XML RSA key contains public key
                {
                    SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                    return FormatPem(Convert.ToBase64String(publicKeyInfo.GetEncoded()), "PUBLIC KEY");
                }
            }
            return "";
        }


        private static string FormatPem(string pem, string keyType)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-----BEGIN {0}-----\n", keyType);

            int line = 1, width = 64;

            while ((line - 1) * width < pem.Length)
            {
                int startIndex = (line - 1) * width;
                int len = line * width > pem.Length
                              ? pem.Length - startIndex
                              : width;
                sb.AppendFormat("{0}\n", pem.Substring(startIndex, len));
                line++;
            }

            sb.AppendFormat("-----END {0}-----\n", keyType);
            return sb.ToString();
        }

        public static string ToBase64XmlFromPem(string pem)
        {
            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();// cspParams);
            csp.ImportParameters(rsaParams);
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(csp.ToXmlString(true)));
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

    }
}
