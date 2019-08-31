using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilingTest.until
{
   public class ByteStringConvert
    {
        // 将 byte[] 转换为字符串
        public static string BytesToString(byte[] bt)
        {
            string str = "";
            foreach (var a in bt)
            {
                if (str != "")
                {
                    str += "-";
                }
                str += a.ToString();
            }
            return str;
        }

        // 将字符串转换为 byte[]
        public static byte[] StringToBytes(string str)
        {
            string[] strArray = str.Split('-');
            int len = strArray.Length;
            byte[] bt = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bt[i] = Convert.ToByte(strArray[i]);
            }
            return bt;
        }
    }
}
