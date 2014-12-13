using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PuzzleSprite.Helpers {
	internal class HashHelper {

		internal static byte[] GetHash(string fileName) {

			using(var md5 = MD5.Create()) {
				using(var stream = File.OpenRead(fileName)) {
					return md5.ComputeHash(stream);
				}
			}
		}

		internal static byte[] Combine(List<byte[]> hashes) {

			byte[] output = new byte[hashes.Sum(arr => arr.Length)];
			int pos = 0;
			foreach(var byteArr in hashes) {
				byteArr.CopyTo(output, pos);
				pos += byteArr.Length;
			}

			using(var md5 = MD5.Create()) {
				return md5.ComputeHash(output);
			}
		}

		internal static string ByteArrayToString(byte[] byteArr) {
			return BitConverter.ToString(byteArr).Replace("-", "");
		}

		internal static string CombineHashesToString(List<byte[]> hashes) {
			return ByteArrayToString(Combine(hashes));
		}

	}
}
