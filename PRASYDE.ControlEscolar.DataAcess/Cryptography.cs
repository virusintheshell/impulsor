

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Text;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.IO;


    public static class Cryptography
    {
        //public static  byte[] IV { get; set; }
        //public static byte[] Key { get; set; }

        //public static string Encriptar(string cadenaOrigen)
        //{
        //    UTF8Encoding encoding = new UTF8Encoding();
        //    byte[] message = encoding.GetBytes(cadenaOrigen);

        //    TripleDESCryptoServiceProvider criptoProvider = new TripleDESCryptoServiceProvider();

        //    ICryptoTransform criptoTransform = criptoProvider.CreateEncryptor(Key, IV);
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, criptoTransform, CryptoStreamMode.Write);

        //    cryptoStream.FlushFinalBlock();
        //    byte[] encriptado = memoryStream.ToArray();
        //    string cadenaEncriptada = encoding.GetString(encriptado);

        //    return cadenaEncriptada;
        //}

        //public static string DesEncriptar(byte[] message)
        //{
        //    TripleDES cryptoProvider = new TripleDESCryptoServiceProvider();

        //    var IV = cryptoProvider.IV;
        //    var Key = cryptoProvider.Key;

        //    ICryptoTransform cryptoTransform = cryptoProvider.CreateDecryptor(Key, IV);

        //    MemoryStream memoryStream = new MemoryStream(message);
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        //    StreamReader sr = new StreamReader(cryptoStream, true);

        //    string textoLimpio = sr.ReadToEnd();
        //    return textoLimpio;
        //}


        //        public static string Encrypt(string plainText, string password)
        //        {
        //            if (plainText == null) { return null; }
        //            if (password == null) { password = String.Empty; }

        //            // Get the bytes of the string
        //            var bytesToBeEncrypted = Convert.FromBase64String(plainText.Replace(" ", "")); //((Encoding.UTF8.GetBytes(plainText);
        //            var passwordBytes = Encoding.UTF8.GetBytes(password);

        //            // Hash the password with SHA256
        //            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //            var bytesEncrypted = Cryptography.Encrypt(bytesToBeEncrypted, passwordBytes);

        //            return Convert.ToBase64String(bytesEncrypted);
        //        }

        //        public static string Decrypt(string encryptedText, string password)
        //        {
        //            if (encryptedText == null) { return null; }
        //            if (password == null) { password = String.Empty; }

        //            // Get the bytes of the string
        //            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
        //            var passwordBytes = Encoding.UTF8.GetBytes(password);

        //            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //            var bytesDecrypted = Cryptography.Decrypt(bytesToBeDecrypted, passwordBytes);

        //            return Encoding.UTF8.GetString(bytesDecrypted);
        //        }

        //        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        //        {
        //            byte[] encryptedBytes = null;

        //            // Set your salt here, change it to meet your flavor:
        //            // The salt bytes must be at least 8 bytes.
        //            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (RijndaelManaged AES = new RijndaelManaged())
        //                {
        //                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

        //                    AES.KeySize = 256;
        //                    AES.BlockSize = 128;
        //                    AES.Key = key.GetBytes(AES.KeySize / 8);
        //                    AES.IV = key.GetBytes(AES.BlockSize / 8);

        //                    AES.Mode = CipherMode.CBC;

        //                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
        //                    {
        //                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        //                        cs.Close();
        //                    }

        //                    encryptedBytes = ms.ToArray();
        //                }
        //            }

        //            return encryptedBytes;
        //        }

        //        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        //        {
        //            byte[] decryptedBytes = null;

        //            // Set your salt here, change it to meet your flavor:
        //            // The salt bytes must be at least 8 bytes.
        //            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (RijndaelManaged AES = new RijndaelManaged())
        //                {
        //                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

        //                    AES.KeySize = 256;
        //                    AES.BlockSize = 128;
        //                    AES.Key = key.GetBytes(AES.KeySize / 8);
        //                    AES.IV = key.GetBytes(AES.BlockSize / 8);
        //                    AES.Mode = CipherMode.CBC;

        //                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
        //                    {
        ////                        cs.Write(Convert.FromBase64String(Encoding.UTF8.GetString(bytesToBeDecrypted)), 0, Convert.FromBase64String(Encoding.UTF8.GetString(bytesToBeDecrypted)).Length);
        //                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
        //                        cs.Close();
        //                    }

        //                    decryptedBytes = ms.ToArray();
        //                }
        //            }

        //            return decryptedBytes;
        //        }



        public static readonly string Key = ConfigurationManager.AppSettings["Encryption_Key"];
        public static readonly Encoding Encoder = Encoding.UTF8;

        public static string TripleDesEncrypt(string plainText)
        {
            var des = CreateDes(Key);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText); //   //
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public static string TripleDesDecrypt(string cypherText)
        {
            try
            {
                var des = CreateDes(Key);
                var ct = des.CreateDecryptor();
                var input = Convert.FromBase64String(cypherText);

                var output = ct.TransformFinalBlock(input, 0, input.Length-1);
                return Encoding.UTF8.GetString(output);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static TripleDES CreateDes(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
        TripleDES des = new TripleDESCryptoServiceProvider();
        var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
        des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }
}
}
