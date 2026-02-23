public class AES
{
    public static string Encrypt(string plainText, string key, string iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            byte[] bytes = Encoding.UTF8.GetBytes(plainText);

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();

                byte[] encryptedBytes = ms.ToArray();

                // HEX с разделител :
                return string.Join(":", encryptedBytes.Select(b => b.ToString("X2")));
            }
        }
    }

    public static string Decrypt(string cipherText, string key, string iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            // Разделяме по :
            byte[] encryptedBytes = cipherText
                .Split(':')
                .Select(x => Convert.ToByte(x, 16))
                .ToArray();

            using (MemoryStream ms = new MemoryStream(encryptedBytes))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
