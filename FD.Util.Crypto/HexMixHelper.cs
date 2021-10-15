using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util
{
    public class HexMixHelper
    {
        const string SEQ = "35o8KbepHvrjaxi0Ek947d1WBlgXSIVmwcPyT6A2uGtCUsQRLNMhJYqnOFZfDz";

        /// <summary>
        /// 将10进制转为62进制
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvertDecTo62(long num)
        {
            if (num < 62)
            {
                return SEQ[(int)num].ToString();
            }
            int y = (int)(num % 62);
            long x = (long)(num / 62);
            return ConvertDecTo62(x) + SEQ[y];
        }

        /// <summary>
        /// 将62进制转为10进制
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long Convert62ToDec(string num)
        {
            long v = 0;
            int len = num.Length;
            for (int i = len - 1; i >= 0; i--)
            {
                int t = SEQ.IndexOf(num[i]);
                double s = (len - i) - 1;
                long m = (long)(Math.Pow(62, s) * t);
                v += m;
            }
            return v;
        }



        public static string Mixup(string key)
        {
            int s = 0;
            foreach (var c in key)
            {
                s += (int)c;
            }
            int len = key.Length;
            int x = (s % len);
            char[] arr = key.ToCharArray();
            char[] newArr = new char[arr.Length];
            Array.Copy(arr, x, newArr, 0, len - x);
            Array.Copy(arr, 0, newArr, len - x, x);
            string newKey = "";
            foreach (var c in newArr)
            {
                newKey += c;
            }
            return newKey;
        }


        public static string UnMixUp(string key)
        {
            int s = 0;
            foreach (var c in key)
            {
                s += (int)c;
            }
            int len = key.Length;
            int x = (s % len);
            x = len - x;
            char[] arr = key.ToCharArray();
            char[] newArr = new char[arr.Length];
            Array.Copy(arr, x, newArr, 0, len - x);
            Array.Copy(arr, 0, newArr, len - x, x);
            string newKey = "";
            foreach (var c in newArr)
            {
                newKey += c;
            }
            return newKey;

        }
    }
}
