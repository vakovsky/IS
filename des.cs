        static async Task Main(string[] args)
        {
            string originalText = "Здравей свят!";
            string key = "12345678";   // DES изисква 8 байта
            string iv = "87654321";   // 8 байта IV

            string encrypted = DES.Encrypt(originalText, key, iv);
            string decrypted = DES.Decrypt(encrypted, key, iv);

            Console.WriteLine("Оригинал: " + originalText);
            Console.WriteLine("Криптиран: " + encrypted);
            Console.WriteLine("Декриптиран: " + decrypted);
        }

public class DES
 {
     public static string Encrypt(string plainText, string key, string iv)
     {
         using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
         {
             des.Key = Encoding.UTF8.GetBytes(key);
             des.IV = Encoding.UTF8.GetBytes(iv);

             byte[] bytes = Encoding.UTF8.GetBytes(plainText);

             using (MemoryStream ms = new MemoryStream())
             using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
             {
                 cs.Write(bytes, 0, bytes.Length);
                 cs.FlushFinalBlock();

                  return Convert.ToBase64String(ms.ToArray());
             }
         }
     }
     public static string Decrypt(string cipherText, string key, string iv)
     {
         using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
         {
             des.Key = Encoding.UTF8.GetBytes(key);
             des.IV = Encoding.UTF8.GetBytes(iv);

             byte[] bytes = Convert.FromBase64String(cipherText);

             using (MemoryStream ms = new MemoryStream())
             using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
             {
                 cs.Write(bytes, 0, bytes.Length);
                 cs.FlushFinalBlock();

                 return Encoding.UTF8.GetString(ms.ToArray());
             }
         }
     }
 }
