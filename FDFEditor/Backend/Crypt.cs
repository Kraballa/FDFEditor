using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FDFEditor.Backend
{
    public static class Crypt
    {

        private static byte[][] Keys = new byte[4][]
        {
          new byte[16]
          {
            (byte) 202,
            (byte) 144,
             216,
            (byte) 211,
            (byte) 188,
            (byte) 214,
            (byte) 244,
            (byte) 180,
            (byte) 174,
            (byte) 194,
            (byte) 188,
            (byte) 101,
            (byte) 40,
            (byte) 22,
            (byte) 189,
            (byte) 183
          },
          new byte[16]
          {
            (byte) 61,
            (byte) 88,
            (byte) 123,
            (byte) 71,
            (byte) 96,
            (byte) 236,
            (byte) 242,
            (byte) 152,
            (byte) 50,
            (byte) 42,
            (byte) 213,
            (byte) 5,
            (byte) 171,
            (byte) 156,
            (byte) 175,
            (byte) 190
          },
          new byte[16]
          {
            (byte) 77,
            (byte) 60,
            (byte) 63,
            (byte) 80,
            (byte) 18,
            (byte) 58,
            (byte) 69,
            (byte) 119,
            (byte) 137,
            (byte) 140,
            (byte) 117,
            (byte) 143,
            (byte) 126,
            (byte) 158,
            (byte) 17,
            (byte) 135
          },
          new byte[16]
          {
            (byte) 242,
            (byte) 165,
            (byte) 41,
            (byte) 132,
            (byte) 231,
            (byte) 150,
            (byte) 148,
            (byte) 35,
            (byte) 220,
            (byte) 80,
            (byte) 204,
            (byte) 233,
            (byte) 36,
            (byte) 174,
            (byte) 251,
            (byte) 77
          }
        };

        public static Stream Decrypt(string FileName, int type = 2)
        {
            byte[] numArray = File.ReadAllBytes(FileName);
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.Key = Crypt.Keys[type];
            cryptoServiceProvider.Mode = CipherMode.ECB;
            ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor();
            return (Stream)new MemoryStream(decryptor.TransformFinalBlock(numArray, 0, numArray.Length));
        }

        public static void DecryptAndMove(string from, string to, int type = 2)
        {
            byte[] inputBuffer = File.ReadAllBytes(from);
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.Key = Crypt.Keys[type];
            cryptoServiceProvider.Mode = CipherMode.ECB;
            byte[] buffer = cryptoServiceProvider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            FileStream fileStream = File.Create(to);
            fileStream.Write(buffer, 0, buffer.GetLength(0));
            fileStream.Close();
        }

        public static void EncryptAndMove(string from, string to, int type = 2)
        {
            byte[] inputBuffer = File.ReadAllBytes(from);
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.Key = Crypt.Keys[type];
            cryptoServiceProvider.Mode = CipherMode.ECB;
            byte[] buffer = cryptoServiceProvider.CreateEncryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            FileStream fileStream = File.Create(to);
            fileStream.Write(buffer, 0, buffer.GetLength(0));
            fileStream.Close();
        }

        private static string Cuts(string word, string num, int array)
        {
            char[] charArray = num.ToCharArray();
            return word.Split(charArray)[array - 1];
        }
    }
}
