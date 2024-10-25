using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class EncryptService
    {
        private readonly byte[] Key;
        private readonly byte[] IV;

        public EncryptService(string masterPassword)
        {
            Key = SHA256.HashData(Encoding.UTF8.GetBytes(masterPassword));
            IV = new byte[16];
        }

        public string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();

            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}