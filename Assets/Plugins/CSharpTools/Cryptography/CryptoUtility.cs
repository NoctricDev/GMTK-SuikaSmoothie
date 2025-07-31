using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CSharpTools.Cryptography
{
	public class CryptoUtility
	{
		private static string FormatHash(byte[] data)
		{
			StringBuilder sb = new();

			foreach (byte b in data)
			{
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}

		public static string Sha256Hash(string text)
		{
			using SHA256 sha256 = SHA256.Create();
			sha256.ComputeHash(Encoding.ASCII.GetBytes(text));

			return FormatHash(sha256.Hash);
		}

		public static string Sha256HashFromFile(string fileName)
		{
			using FileStream fs = new(fileName, FileMode.Open);
			using SHA256 sha256 = SHA256.Create();
			sha256.ComputeHash(fs);

			return FormatHash(sha256.Hash);
		}

		public static string Sha1Hash(string text)
		{
			using SHA1 sha = SHA1.Create();
			sha.ComputeHash(Encoding.ASCII.GetBytes(text));

			return FormatHash(sha.Hash);
		}

		public static string Sha1HashFromFile(string fileName)
		{
			using FileStream fs = new(fileName, FileMode.Open);
			using SHA1 sha = SHA1.Create();
			sha.ComputeHash(fs);

			return FormatHash(sha.Hash);
		}

		public static bool CompareHash(string hashA, string hashB)
		{
			return hashA.Equals(hashB, StringComparison.OrdinalIgnoreCase);
		}
	}
}