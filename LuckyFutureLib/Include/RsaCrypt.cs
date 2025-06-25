using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.IO;

namespace LuckyFutureLib.Include
{
    public static class RsaCrypt
    {

        public static string publicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAowjXqBAK/VxSr3DNSa+w
OgKdMFdAqx1PaYk+7uxLo1gyJz/cgO93buE1P2nrovwY0E1GVM1bEmRLqX9ypGAq
n6LmglshIsZUvvpEKR5SXG2W/nxDO5Y4nLcyp5q+hdsM7uKrENt8fc2IqNcu78i+
RbvaXDgVJmAtGWf6j0JyczeSYb5wHvBmwJ0Og9cURwMTcuHtzipkiUNBmqAmlk+Y
rEOJzqMJY97pAJUJjKK/UtS/v0cbLTynmjZctEl5StyaOy9b92nbsqq5F4gk3bz/
aA+ZFTGxRbaa+L6pqs/txkVISL0uoSH+OwSZ4Hr2+Z2gR462g+j0VLFOWAiY60eL
1QIDAQAB
-----END PUBLIC KEY-----";

        public static string Decrypt(string cipherText, bool isPublic = true)
        {
            if (cipherText.Length < 1)
                throw new ArgumentNullException(nameof(cipherText));

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            StringReader sr = new StringReader(publicKey);
            // PKCS1 v1.5 paddings
            Pkcs1Encoding eng = new Pkcs1Encoding(new RsaEngine());

            PemReader pr = new PemReader(sr);
            if (isPublic)
            {
                RsaKeyParameters keys = (RsaKeyParameters)pr.ReadObject();
                eng.Init(false, keys);

            }
            else
            {
                AsymmetricCipherKeyPair keys = (AsymmetricCipherKeyPair)pr.ReadObject();
                eng.Init(false, keys.Private);
            }

            int length = cipherTextBytes.Length;
            int blockSize = eng.GetInputBlockSize();
            List<byte> plainTextBytes = new List<byte>();
            for (int chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                plainTextBytes.AddRange(eng.ProcessBlock(
                    cipherTextBytes, chunkPosition, chunkSize
                ));
            }
            return Encoding.UTF8.GetString(plainTextBytes.ToArray());
        }

        public static string Encrypt(string plainText)
        {
            if (plainText.Length < 1)
                throw new ArgumentNullException(nameof(plainText));

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            StringReader sr = new StringReader(publicKey);

            PemReader pr = new PemReader(sr);

            RsaKeyParameters keys = (RsaKeyParameters)pr.ReadObject();

            // PKCS1 v1.5 paddings
            Pkcs1Encoding eng = new Pkcs1Encoding(new RsaEngine());

            eng.Init(true, keys);

            int length = plainTextBytes.Length;
            int blockSize = eng.GetInputBlockSize();
            List<byte> cipherTextBytes = new List<byte>();
            for (int chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                cipherTextBytes.AddRange(eng.ProcessBlock(
                    plainTextBytes, chunkPosition, chunkSize
                ));
            }
            return Convert.ToBase64String(cipherTextBytes.ToArray());
        }

        public static string GetKeyString(RSAParameters publicKey)
        {

            var stringWriter = new StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }
    }
}
