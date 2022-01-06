using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    public class Decryption
    {
        public static string DecryptString(string s, string secretKey)
        {
            string decryptedString = null;
            byte[] encryptedBytes = Convert.FromBase64String(s);

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = Convert.FromBase64String(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform aesDecryptTransform = aesCryptoProvider.CreateDecryptor();
            using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptTransform, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cryptoStream))
                    {
                        decryptedString = sr.ReadToEnd();
                    }
                }
            }

            return decryptedString;
        }


    }
}
