using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Crypto
{
    public static class CryptoHelper
    {

        public static string GenerateRsaPublicKey(string containerName)
        {
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            using (var rsa = new RSACryptoServiceProvider(parameters))
            {
                return rsa.ToXmlString(false);
            }             
        }

        public static string GenerateRsaPrivateKey(string containerName)
        {
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            using (var rsa = new RSACryptoServiceProvider(parameters))
            {
                return rsa.ToXmlString(true);
            }
        }

        public static void DeleteRsaKey(string containerName)
        {
            
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            using (var rsa = new RSACryptoServiceProvider(parameters))
            {
                // Delete the key entry in the container.
                rsa.PersistKeyInCsp = false;
                // Call Clear to release resources and delete the key from the container.
                rsa.Clear();
            };         
        }



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
