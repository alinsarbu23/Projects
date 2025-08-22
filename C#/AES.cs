using System;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace C_Sharp_Recap_28_07_2025
{
    public class AES
    {
        private byte[] publicKey;
        private byte[] secretKey;

        public void LoadConfig(string filePath)
        {
            /*  Loading the keys
             *  1. Check the file
             *  2. Create a json var and string for reading the file context and deserialize the content using a specified class for JSON
             *  3. Check the size of the numbers
             *  4. Encode using UTF8 for getting the values for the keys
             */

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found!");
            }

            string jsonText = File.ReadAllText(filePath);
            var json = JsonSerializer.Deserialize<AESConfig>(jsonText);

            if(json.SecretKey.Length != 24)
            {
                throw new Exception($"The secret key does not have the required lenght {json.SecretKey}");
                return;
            }

            if (json.PublicKey.Length != 16)
            {
                throw new Exception($"The public key does not have the required lenght {json.PublicKey}");
                return;
            }

            secretKey = Encoding.UTF8.GetBytes(json.SecretKey);
            publicKey = Encoding.UTF8.GetBytes(json.PublicKey);

        }

        public string Encrypt(string text)
        {
            /*  Encrypt 
             *  1. Set the public key and the private key using Aes.Create
             *  2. Initialzie the ICryptoTransform adding the keys as parameters for the methods
             *  3. Use MemoryStream
             *  4. Declare CryptoStream using ms, encryptor and the mode set as write  
             *  5. Include StreamWriter with cryptostream and write the text
             *  6. return the ms using toArray method in Converting.ToBase64String
             */
            using (Aes aes = Aes.Create())
            {
                aes.Key = secretKey;
                aes.IV = publicKey;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptostream))
                        {
                             streamWriter.Write(text);  
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string text)
        {
            /*
             *  1. Use a byte vector for Convert.FromBase64 for bits values
             *  2. Use aes.Create
             *  3. Use ICryptoTransform
             *  4. use MemoryStream
             *  5. Use CryptoStream with ms, encryptor and CryptoStreamMode such as Read
             *  6. Use StreamReader with CryptoStream object as a parameter
             *  7. Read to end using streamReader
             * 
             */
            byte[] data = Convert.FromBase64String(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = secretKey;
                aes.IV = publicKey;

                ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using(MemoryStream ms = new MemoryStream(data))
                {
                    using(CryptoStream crypto = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {
                        using(StreamReader streamReader = new StreamReader(crypto))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }

            }
        }

        public AES(string filePath)
        {
            LoadConfig(filePath);
        }
    }

    public class AESConfig
    {
        public string SecretKey { get; set; }
        public string PublicKey { get; set; }
    }
}
