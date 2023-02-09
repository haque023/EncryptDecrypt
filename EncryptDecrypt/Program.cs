using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Main
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            var e = EncryptStream("Hello World");
            var eStr = Convert.ToBase64String(e.ToArray());
            Console.WriteLine(eStr);
            var d = DecrypStream(eStr);
            Console.Write(d);

        }
        private static Aes GetEncryptionAlgorithm()
        {

            string key = "fe55519df6535b1e";
            string iv = "fe55519df6535b1e";
            var secret_key = key;
            var initialization_vector = iv;
            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(secret_key);
            aes.IV = Encoding.UTF8.GetBytes(initialization_vector);
            return aes;
        }
        private static MemoryStream EncryptStream(string s)
        {
            var responseStream = new MemoryStream();
            var writer = new StreamWriter(responseStream);
            writer.Write(s);
            writer.Flush();
            responseStream.Position = 0;

            Aes aes = GetEncryptionAlgorithm();
            using MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream);
                streamWriter.Write(s);

            }
            return memoryStream;
        }

        private static string DecrypStream(string s)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(s);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

    }

}