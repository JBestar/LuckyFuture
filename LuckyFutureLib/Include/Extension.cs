using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyFutureLib.Include
{
	public static class Extension
	{	public static void DoubleBuffered(this DataGridView dgv, bool setting)
		{
			dgv.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgv, setting, null);
		}
        public static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
        public static string ByteArrayToString(byte[] bytes, int len)
        {
            StringBuilder hex = new StringBuilder();
            if (len > bytes.Length)
                return "[ error ] bytes";
            int ln = 0;
            byte b = 0;
            for (int i = 0; i < len; i++)
            {
                b = bytes[i];
                hex.AppendFormat("{0:x2} ", b);
                if (ln++ >= 15)
                {
                    ln = 0;
                    hex.Append("\n");
                }
            }
            return hex.ToString();
        }
    }
}
