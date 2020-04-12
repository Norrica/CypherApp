using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static Encoding u8 = Encoding.Default;
        static void Main(string[] args)
        {

            string key = "гшмарвфыапрыцйу";
            var bkey = key.GetBytes(u8);
            List<byte[]> tenKeys = GenerateTen(bkey);
            Console.WriteLine(bkey.PrintBytes());
            foreach (var oneKey in tenKeys)
            {
                Console.WriteLine(oneKey.PrintBytes());
            }
            Console.WriteLine();
            Console.WriteLine(bkey.PrintBytes());
            for (int i = 0; i < tenKeys.Count; i++)
            {
                Console.WriteLine(GetOne(tenKeys,i).PrintBytes());
            }
        }

        private static List<byte[]> GenerateTen(byte[] bkey)
        {
            List<byte[]> tenKeys = new List<byte[]>();
            for (int i = 0; i < bkey.Length && i < 10; i++)
            {
                List<byte> buf = new List<byte>();
                buf.Add(bkey[i]);
                foreach (var k in bkey)
                {
                    buf.Add((byte)(k ^ bkey[i]));
                }
                tenKeys.Add(buf.ToArray());
            }
            return tenKeys;
        }
        private static byte[] GetOne(List<byte[]> tenKeys, int index)
        {
            var oneOfTenKeys = tenKeys[index];
            byte first = oneOfTenKeys.First();
            var trimmedKeys = new byte[oneOfTenKeys.Length - 1];
            Array.Copy(oneOfTenKeys, 1, trimmedKeys, 0, oneOfTenKeys.Length - 1);
            List<byte> buf = new List<byte>();
            foreach (var k in trimmedKeys)
            {
                buf.Add((byte)(k ^ first));
            }
            return buf.ToArray();
        }
    }
    public static class ByteEncoder
    {
        public static byte[] GetBytes(this string chars, Encoding enc)
        {
            byte[] bytes = enc.GetBytes(chars);
            return bytes;
        }
        public static string GetString(this byte[] bytes, Encoding enc)
        {
            string res = enc.GetString(bytes);
            return res;
        }
        public static string PrintBytes(this byte[] bytes)
        {

            StringBuilder res = new StringBuilder();
            if ((bytes == null) || (bytes.Length == 0))
            {
                Console.WriteLine("<none>");
            }
            else
            {
                foreach (var b in bytes)
                    res.Append(b.ToString("X2"));

            }
            return res.ToString();
        }
        public static byte[] GetBytesFromPrintableString(this string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            if (str.Length % 2 == 0)
            {
                for (int i = 0; i < str.Length; i += 2)
                {
                    if (hexindex.ContainsKey(str.Substring(i, 2)))
                    {
                        hexres.Add(hexindex[str.Substring(i, 2)]);
                    }
                }
            }
            return hexres.ToArray();
        }
        public static byte[] Shift(this byte[] myArray, int shift)
        {
            byte[] tArray = new byte[myArray.Length];
            if (shift >= 0)
            {
                for (int j = 0; j < shift; j++)
                {
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        var buf = myArray[0];
                        if (i < myArray.Length - 1)
                            myArray[i] = myArray[i + 1];
                        else
                            myArray[i] = buf;
                    }
                }
                return myArray;
            }
            else if (shift < 0)
            {
                for (int j = 0; j > shift; j--)
                {
                    for (int i = myArray.Length; i > 0; i--)
                    {
                        if (i < myArray.Length - 1)
                            tArray[i] = myArray[i - 1];
                        else
                            tArray[0] = myArray[i];
                    }
                }
                return tArray;
            }
            else
            {
                return myArray;
            }
        }
    }

}
