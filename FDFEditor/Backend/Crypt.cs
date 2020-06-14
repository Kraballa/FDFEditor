using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace FDFEditor.Backend
{
    public static class Crypt
    {

        public enum Game
        {
            FDF1,
            FDF2
        }

        private static byte[][] FDFSteamKeys = new byte[4][]
        {
            new byte[16]{202,144,216,211,188,214,244,180,174,194,188,101,40,22,189,183},
            new byte[16]{61,88,123,71,96,236,242,152,50,42,213,5,171,156,175,190},
            new byte[16]{77,60,63,80,18,58,69,119,137,140,117,143,126,158,17,135},
            new byte[16]{242,165,41,132,231,150,148,35,220,80,204,233,36,174,251,77}
        };
        private static byte[][] FDFOldKeys = new byte[4][]
        {
            new byte[16]{79,14,42,91,9,12,143,221,62,193,178,163,byte.MaxValue,162,5,7},
            new byte[16]{28,91,61,0,5,4,127,187,204,45,195,212,170,241,242,248},
            new byte[16]{204,219,153,8,byte.MaxValue,250,154,184,199,109,227,171,202,253,254,250},
            new byte[16]{79,145,221,238,198,51,249,164,187,17,252,13,241,184,23,0}
        };

        public static Stream CryptStreamFromFile(string path, bool decrypt = true, bool fdf1 = false, int type = 2)
        {
            byte[] content = File.ReadAllBytes(path);
            return CryptStreamFromBuffer(content, decrypt, fdf1, type);
        }

        public static Stream CryptStreamFromBuffer(byte[] buffer, bool decrypt = true, bool fdf1 = false, int type = 2)
        {
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.Key = fdf1 ? FDFOldKeys[type] : FDFSteamKeys[type];
            cryptoServiceProvider.Mode = CipherMode.ECB;
            ICryptoTransform decryptor = decrypt ? cryptoServiceProvider.CreateDecryptor() : cryptoServiceProvider.CreateEncryptor();
            return (Stream)new MemoryStream(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static void CryptContentToFile(string path, string content, bool decrypt = false, bool fdf1 = false, int type = 2)
        {
            File.WriteAllText(path, content);
            CryptFileInPlace(path, decrypt, fdf1, type);
        }

        public static void CryptAndCopyFile(string from, string to, bool decrypt = false, bool fdf1 = false, int type = 2)
        {
            byte[] content = File.ReadAllBytes(from);
            Stream s = Crypt.CryptStreamFromBuffer(content, decrypt, fdf1, type);
            FileStream fs = File.Create(to);
            s.CopyTo(fs);
            fs.Close();
        }

        public static void CryptFileInPlace(string FileName, bool decrypt = false, bool fdf1 = false, int type = 2)
        {
            byte[] inputBuffer = File.ReadAllBytes(FileName);
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.Key = fdf1 ? FDFOldKeys[type] : FDFSteamKeys[type];
            cryptoServiceProvider.Mode = CipherMode.ECB;
            byte[] buffer = cryptoServiceProvider.CreateEncryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            FileStream fileStream = File.Create(FileName);
            fileStream.Write(buffer, 0, buffer.GetLength(0));
            fileStream.Close();
        }
    }
}
