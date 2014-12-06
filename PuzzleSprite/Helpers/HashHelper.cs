using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite.Helpers {
	internal class HashHelper {

		internal static byte[] GetHash(string fileName) {
			using(var md5 = MD5.Create()) {
				using(var stream = File.OpenRead(fileName)) {
					return md5.ComputeHash(stream);
				}
			}
		}

		internal static byte[] CombineHashes(List<byte[]> hashes) {
			var output = new byte[hashes.Sum(arr => arr.Length)];
			int writeIdx = 0;
			foreach(var byteArr in hashes) {
				byteArr.CopyTo(output, writeIdx);
				writeIdx += byteArr.Length;
			}


			using(var md5 = MD5.Create()) {
				return md5.ComputeHash(output);
			}
		}

		internal static string ByteArrayToString(byte[] byteArr) {

			return BitConverter.ToString(byteArr).Replace("-", "");
		}

		internal static string CombineHashesToString(List<byte[]> hashes) {
			return HashHelper.ByteArrayToString(HashHelper.CombineHashes(hashes));
		}

	}
}
