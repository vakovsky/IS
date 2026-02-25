public class RSAIE
{
    // 🔑 Генерира ключове и ги записва във файлове
    public static void GenerateAndSaveKeys(string publicPath, string privatePath, int keySize = 2048)
    {
        using (var rsa = new RSACryptoServiceProvider(keySize))
        {
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);

            File.WriteAllText(publicPath, publicKey);
            File.WriteAllText(privatePath, privateKey);
        }
    }

    // 📂 Зарежда ключ от файл
    public static string LoadKey(string path)
    {
        return File.ReadAllText(path);
    }

    // 🔐 Криптира с публичен ключ
    public static string Encrypt(string plainText, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);

            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = rsa.Encrypt(data, false); // PKCS#1

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // 🔓 Декриптира с частен ключ
    public static string Decrypt(string cipherText, string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);

            byte[] encryptedBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    // ✍ Подписване с частен ключ
    public static string Sign(string message, string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);

            byte[] data = Encoding.UTF8.GetBytes(message);

            byte[] signature = rsa.SignData(
                data,
                CryptoConfig.MapNameToOID("SHA256")
            );

            return Convert.ToBase64String(signature);
        }
    }

    // ✔ Проверка на подпис с публичен ключ
    public static bool Verify(string message, string signatureBase64, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);

            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] signature = Convert.FromBase64String(signatureBase64);

            return rsa.VerifyData(
                data,
                CryptoConfig.MapNameToOID("SHA256"),
                signature
            );
        }
    }
}
