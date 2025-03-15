using Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZGScriptTrainer.SaveAndLoad
{
    class SaveAndLoadNeed
    {
        private  readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes for AES-128
        private  readonly byte[] IV = Encoding.UTF8.GetBytes("abcdefghijklmnop");  // 16 bytes for AES
        private  readonly string additionalText = "这么简单你都要破？";

        public  bool SaveAllFriendToFile<T>(Dictionary<int, T> allFriend,string SavePath,string SaveName )
        {
            string directory = Path.GetDirectoryName(SavePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                if (allFriend.Count == 0)
                {
                    Console.WriteLine(SaveName+" 字典为空，无法保存数据。");
                    return false;
                }

                string data = JsonConvert.SerializeObject(allFriend);
                data += "\n" + additionalText; // 在数据后面添加额外文字
                byte[] encryptedData = EncryptStringToBytes_Aes(data, Key, IV);

                using (FileStream fs = new FileStream(SavePath, FileMode.Create))
                {
                    fs.Write(encryptedData, 0, encryptedData.Length);
                }

                Console.WriteLine(SaveName + " 数据已保存到 BepInEx\\plugins\\TheSave文件夹内。");
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"保存数据时出错: {ex.Message}");
                return false;
            }
        }

        public  Dictionary<int, T> ReadAllFriendFromFile<T>(string SavePath)
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    Console.WriteLine("文件不存在，无法读取数据。");
                    return null;
                }

                byte[] encryptedData = File.ReadAllBytes(SavePath);
                string decryptedData = DecryptStringFromBytes_Aes(encryptedData, Key, IV);

                // 分割数据，忽略额外文字
                string[] lines = decryptedData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 0)
                {
                    string jsonData = lines[0];
                    Dictionary<int, T> result = JsonConvert.DeserializeObject<Dictionary<int, T>>(jsonData);

                    //Console.WriteLine("AllFriend 数据已从文件中读取并解密。");
                    return result;
                }
                else
                {
                    Console.WriteLine("未找到有效数据。");
                    return null;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"读取并解密数据时出错: {ex.Message}");
                return null;
            }
        }

        private  byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private  string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
