﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FD.Util.Crypto
{
    public class HashingCompute
    {
        /// <summary>
        /// Checks the md5sum.        
        /// </summary>
        /// <remarks>
        /// 参考淘宝sdk中AtsUtils,CheckMd5sum
        /// </remarks>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string CalMd5(byte[] bytes)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] retVal = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Checks the md5sum.        
        /// </summary>       
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string CalMd5File(string strFilePath)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = md5.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString().ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string CalMd5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        /// <summary>
        /// Cals the sha1.
        /// </summary>
        /// <see cref="http://www.teimouri.net/calculate-file-checksum-in-c/"/>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string CalSha1(byte[] bytes)
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
        
    }




  
}
