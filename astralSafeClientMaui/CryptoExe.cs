using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;

namespace astralSafeClientMaui
{
    internal class CryptoExe
    {
        public byte[] Executable { get; set; }

        public string Path { get; set; }

        public byte[] Key { get; set; }

        private Aes Aes { get; }

        [RequiresAssemblyFiles("Calls astralSafeClientMaui.CryptoExe.getExePath()")]
        public CryptoExe()
        {
            Executable = null;

            Path = GetExePath();

            Key = null;

            Aes = Aes.Create();

            Aes.GenerateIV();
        }

        [RequiresAssemblyFiles("Calls System.Reflection.Assembly.Location")]
        private static string GetExePath()
        {
            string tmp = System.IO.Path.GetDirectoryName(path: Assembly.GetEntryAssembly().Location);

            tmp = Directory.GetParent(path: tmp).FullName;

            tmp = Directory.GetParent(path: tmp).FullName;

            tmp = Directory.GetParent(path: tmp).FullName;

            tmp = Directory.GetParent(path: tmp).FullName;

            tmp = Directory.GetParent(path: tmp).FullName;

            return tmp + "\\Resources\\Raw\\main.exe";
        }

        public static byte[] StringToByteArray(string key)
        {
            byte[] bytes = new byte[key.Length / 2];

            for (int i = 0; i < key.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value: key.Substring(i, 2), fromBase: 16);
            }

            return bytes;
        }

        /// <summary>
        /// Encrypts a byte array using AES 128
        /// </summary>
        public void Crypt()
        {
            Executable = File.ReadAllBytes(path: Path);

            using MemoryStream ms = new();

            Aes.Mode = CipherMode.CBC;
            Aes.Padding = PaddingMode.PKCS7;
            Aes.KeySize = 128;
            Aes.BlockSize = 128;

            //We use the random generated iv created by AesManaged
            byte[] iv = Aes.IV;

            using (CryptoStream cs = new(stream: ms, transform: Aes.CreateEncryptor(Key, iv), mode: CryptoStreamMode.Write))
            {
                cs.Write(buffer: Executable, offset: 0, count: Executable.Length);
            }
            byte[] encryptedContent = ms.ToArray();

            //Create new byte array that should contain both unencrypted iv and encrypted data
            byte[] result = new byte[iv.Length + encryptedContent.Length];

            //copy our 2 array into one
            Buffer.BlockCopy(src: iv, srcOffset: 0, dst: result, dstOffset: 0, count: iv.Length);
            Buffer.BlockCopy(src: encryptedContent, srcOffset: 0, dst: result, dstOffset: iv.Length, count: encryptedContent.Length);

            File.WriteAllBytes(path: Path, bytes: result);
        }

        /// <summary>
        /// Decrypt a byte array using AES 128
        /// </summary>
        public void Decrypt()
        {
            Executable = File.ReadAllBytes(path: Path);

            byte[] iv = new byte[16]; //initial vector is 16 bytes
            byte[] encryptedContent = new byte[Executable.Length - 16]; //the rest should be encryptedcontent

            //Copy data to byte array
            Buffer.BlockCopy(src: Executable, srcOffset: 0, dst: iv, dstOffset: 0, count: iv.Length);
            Buffer.BlockCopy(src: Executable, srcOffset: iv.Length, dst: encryptedContent, dstOffset: 0, count: encryptedContent.Length);

            using MemoryStream ms = new();

            Aes.Mode = CipherMode.CBC;
            Aes.Padding = PaddingMode.PKCS7;
            Aes.KeySize = 128;
            Aes.BlockSize = 128;

            using (CryptoStream cs = new(stream: ms, transform: Aes.CreateDecryptor(Key, iv), mode: CryptoStreamMode.Write))
            {
                cs.Write(buffer: encryptedContent, offset: 0, count: encryptedContent.Length);
            }

            File.WriteAllBytes(path: Path, bytes: ms.ToArray());
        }

    }
}