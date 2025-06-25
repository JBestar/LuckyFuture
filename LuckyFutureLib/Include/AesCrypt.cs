using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFutureLib.Include
{
    public static class AesCrypt
    {
        public static string Decrypt(string data, string privateKey)
        {
            if (data.Length < 1)
                throw new ArgumentNullException(nameof(data));
            if (privateKey.Length < 1)
                throw new ArgumentNullException(nameof(privateKey));

            string[] info = data.Split(':');
            if (info.Length != 2)
                return "";

            string publicKey = info[0];
            string cipherText = info[1];

            if (publicKey.Length < 1)
                throw new ArgumentNullException(nameof(publicKey));

            var aesAlg = Aes.Create();
            //aesAlg.Padding = PaddingMode.PKCS7;
            //aesAlg.BlockSize = 128;
            //aesAlg.KeySize = 192;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Key = ConvertBytes(privateKey);
            aesAlg.IV = Convert.FromBase64String(publicKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            var srDecrypt = new StreamReader(csDecrypt);
            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }

        public static string Encrypt(string plainText, string privateKey)
        {
            if (plainText.Length < 1)
                throw new ArgumentNullException(nameof(plainText));
            if (privateKey.Length < 1)
                throw new ArgumentNullException(nameof(privateKey));

            byte[] encrypted;

            string publicKey = Convert.ToBase64String(GenerateRandomPublicKey());
            using (var aesAlg = Aes.Create())
            {
                //aesAlg.BlockSize = 128;
                //aesAlg.KeySize = 192;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = ConvertBytes(privateKey);
                aesAlg.IV = Convert.FromBase64String(publicKey);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return publicKey + ":" + Convert.ToBase64String(encrypted);
        }
        public static byte[] GenerateRandomPublicKey()
        {
            //var iv = new byte[16]; // AES > IV > 128 bit
            //iv = RandomNumberGenerator.GetBytes(iv.Length);
            //return iv;
            var iv = RandomString(16);
            return ConvertBytes(iv);
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] ConvertBytes(string inputString)
        {
            return Encoding.ASCII.GetBytes(inputString);
            //return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string DecimalToArbitraryBase(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "ZaBcDeFghIJkLmnOPqrStUvWxY";

            if (radix < 2 || radix > Digits.Length)
                return "Z";
            //throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber <= 0)
                return "Z";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }

}
