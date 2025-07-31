using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace CSharpTools.Cryptography
{
	public static class GuidUtility
	{
		private const char PaddingChar = '=';
		private const char HyphenChar = '-';
		private const char UnderscoreChar = '_';
		private const char SlashChar = '/';
		private const char PlusChar = '+';
		private const byte SlashByte = (byte)'/';
		private const byte PlusByte = (byte)'+';

		/// <summary>
		///		Converts a Guid to a string that can be used in a URL
		/// </summary>
		/// <param name="id">The Guid to convert</param>
		/// <returns>URL friendly string</returns>
		public static string GuidToString(Guid id)
		{
			Span<byte> bytesId = stackalloc byte[16];
			Span<byte> base64Bytes = stackalloc byte[24];

			MemoryMarshal.TryWrite(bytesId, ref id);
			Base64.EncodeToUtf8(bytesId, base64Bytes, out _, out _);

			Span<char> finalChars = stackalloc char[22];

			for (int i = 0; i < 22; i++)
			{
				finalChars[i] = base64Bytes[i] switch
				{
					SlashByte => HyphenChar,
					PlusByte => UnderscoreChar,
					_ => (char)base64Bytes[i]
				};
			}

			return new string(finalChars);
		}

		/// <summary>
		///		Converts a Base64 encoded string from a URL to a Guid
		/// </summary>
		/// <param name="id">The Base64 String to convert</param>
		/// <returns>The generated Guid</returns>
		public static Guid StringToGuid(ReadOnlySpan<char> id)
		{
			Span<char> base64Chars = stackalloc char[24];

			for (int i = 0; i < 22; i++)
			{
				base64Chars[i] = id[i] switch
				{
					HyphenChar => SlashChar,
					UnderscoreChar => PlusChar,
					_ => id[i]
				};
			}

			base64Chars[22] = PaddingChar;
			base64Chars[23] = PaddingChar;

			Span<byte> bytesId = stackalloc byte[16];
			Convert.TryFromBase64Chars(base64Chars, bytesId, out _);

			return new Guid(bytesId);
		}
	}
}
