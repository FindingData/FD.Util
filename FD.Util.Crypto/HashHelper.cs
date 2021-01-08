using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FD.Util.Crypto
{
    public class HashHelper
    {
        #region MD5
        /// <summary>
        /// MD5字符串+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Md5WithSalt(string plain, byte[] salt)
        {
            return Md5(Encoding.UTF8.GetBytes(plain).Concat(salt).ToArray());
        }

        /// <summary>
        /// MD5字符串+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Md5WithSalt(byte[] plain, byte[] salt)
        {
            return Md5(plain.Concat(salt).ToArray());
        }


        /// <summary>
        /// MD5字符串
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <returns></returns>
        public static string Md5(string plain)
        {
            return Md5(Encoding.UTF8.GetBytes(plain));
        }

        /// <summary>
        /// MD5字节
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Md5(byte[] bytes)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] retVal = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// MD5文件
        /// </summary>       
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string Md5File(string filePath)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = md5.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        #endregion

        #region Sha1
        /// <summary>
        /// Sha1+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Sha1WithSalt(string plain, byte[] salt)
        {
            return Sha1(Encoding.UTF8.GetBytes(plain).Concat(salt).ToArray());
        }

        /// <summary>
        /// Sha1+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Sha1WithSalt(byte[] plain, byte[] salt)
        {
            return Sha1(plain.Concat(salt).ToArray());
        }

        /// <summary>
        /// SHA-1
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string Sha1(byte[] bytes)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] retVal = sha1.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// SHA-1
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string Sha1(string plain)
        {
            return Sha1(Encoding.UTF8.GetBytes(plain));
        }


        /// <summary>
        /// Sha1 文件
        /// </summary>       
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string Sha1File(string filePath)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = sha1.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString().ToLowerInvariant();
                }
            }
        }
        #endregion


        #region Sha256
        /// <summary>
        /// Sha256+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Sha256WithSalt(string plain, byte[] salt)
        {
            return Sha1(Encoding.UTF8.GetBytes(plain).Concat(salt).ToArray());
        }

        /// <summary>
        /// Sha256+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Sha256WithSalt(byte[] plain, byte[] salt)
        {
            return Sha256(plain.Concat(salt).ToArray());
        }

        /// <summary>
        /// Sha256
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string Sha256(byte[] bytes)
        {
            using (SHA256CryptoServiceProvider sha1 = new SHA256CryptoServiceProvider())
            {
                byte[] retVal = sha1.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Sha256
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string Sha256(string plain)
        {
            return Sha256(Encoding.UTF8.GetBytes(plain));
        }


        /// <summary>
        /// Sha256 文件
        /// </summary>       
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string Sha256File(string filePath)
        {
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = sha256.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString().ToLowerInvariant();
                }
            }
        }
        #endregion

        #region HMAC sha256   
        /// <summary>
        /// HmacSha256
        /// </summary>
        /// <param name="plain">The plain string</param>
        /// <returns></returns>
        public static string HmacSha256(string plain,byte[] key)
        {
            return HmacSha256(Encoding.UTF8.GetBytes(plain),key);
        }


        /// <summary>
        /// HmacSha256
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string HmacSha256(byte[] bytes,byte[] key)
        {
            using (HMACSHA256 hmacSha256 = new HMACSHA256(key))
            {
                byte[] retVal = hmacSha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToLowerInvariant();
            }
        }


        /// <summary>
        /// HmacSha256 文件
        /// </summary>       
        /// <param name="filePath">文件路径</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string HmacSha256File(string filePath,byte[] key)
        {
            using (HMACSHA256 hmacSha256 = new HMACSHA256(key))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = hmacSha256.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString().ToLowerInvariant();
                }
            }
        }

        #endregion
    }
}
