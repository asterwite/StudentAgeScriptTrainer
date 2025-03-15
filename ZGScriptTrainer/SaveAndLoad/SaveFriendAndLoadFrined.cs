using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZGScriptTrainer.SaveAndLoad
{
    public class SaveFriendAndLoadFrined
    {
        private const string filePath = @"BepInEx\plugins\TheSave\CanTalkFriend";
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes for AES-128
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("abcdefghijklmnop");  // 16 bytes for AES
        private static readonly string additionalText = "这么简单你都要破？";
        public static bool SaveAllFriendToFile(List<int> allFriend)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                if (allFriend.Count == 0)
                {
                    Console.WriteLine("AllFriend 列表为空，无法保存数据。");
                    return false;
                }

                string data = string.Join(",", allFriend);
                data += "\n" + additionalText; // 在数据后面添加额外文字
                byte[] encryptedData = EncryptStringToBytes_Aes(data, Key, IV);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(encryptedData, 0, encryptedData.Length);
                }

                Console.WriteLine("AllFriend 数据已保存到BepInEx\\plugins\\TheSave文件夹内。");
                Console.WriteLine("所有朋友数据加载完成，你现在可以使用朋友页面的功能了");
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"保存数据时出错: {ex.Message}");
                return false;
            }
        }
       public static List<int> ReadAllFriendFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("文件不存在，无法读取数据。");
                    return null;
                }

                byte[] encryptedData = File.ReadAllBytes(filePath);
                string decryptedData = DecryptStringFromBytes_Aes(encryptedData, Key, IV);

                // 分割数据，忽略额外文字
                string[] lines = decryptedData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 0)
                {
                    string[] numStrings = lines[0].Split(',');
                    List<int> result = new List<int>();
                    foreach (string numStr in numStrings)
                    {
                        if (int.TryParse(numStr, out int num))
                        {
                            result.Add(num);
                        }
                    }

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
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
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
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
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
