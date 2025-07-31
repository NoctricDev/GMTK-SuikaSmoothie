using System;
using System.IO;
using System.Security.Cryptography;

namespace CSharpTools.Cryptography
{
	public static class EncryptionHelper
	{
		public static string Encrypt(string plainText, byte[] key)
		{
			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.GenerateIV();
			ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

			using MemoryStream stream = new();
			stream.Write(aes.IV, 0, aes.IV.Length);

			using CryptoStream cs = new(stream, encryptor, CryptoStreamMode.Write);
			using StreamWriter sw = new(cs);
			sw.Write(plainText);
			sw.Close();

			return Convert.ToBase64String(stream.ToArray());
		}

		public static string Encrypt(string plainText, string key)
		{
			byte[] keyBytes = StringKeyToByte(key);
			return Encrypt(plainText, keyBytes);
		}

		public static string Decrypt(string cipherText, byte[] key)
		{
			byte[] fullCipher = Convert.FromBase64String(cipherText);
			byte[] iv = new byte[16];
			byte[] cipher = new byte[fullCipher.Length - 16];

			Array.Copy(fullCipher, iv, iv.Length);
			Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;

			ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

			using MemoryStream stream = new(cipher);
			using CryptoStream cs = new(stream, decryptor, CryptoStreamMode.Read);
			using StreamReader sr = new(cs);

			return sr.ReadToEnd();
		}

		public static string Decrypt(string cipherText, string key)
		{
			byte[] keyBytes = StringKeyToByte(key);
			return Decrypt(cipherText, keyBytes);
		}

		public static byte[] GenerateEncryptionKey()
		{
			byte[] key = new byte[32];
			RandomNumberGenerator.Fill(key);

			return key;
		}

		public static string GenerateEncryptionKeyAsString()
		{
			return ByteKeyToString(GenerateEncryptionKey());
		}

		public static string ByteKeyToString(byte[] key)
		{
			return BitConverter.ToString(key).Replace("-", "").ToLower();
		}

		public static byte[] StringKeyToByte(string key)
		{
			int length = key.Length;
			byte[] bytes = new byte[length / 2];

			for (int i = 0; i < length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(key.Substring(i, 2), 16);
			}

			return bytes;
		}
	}
}
