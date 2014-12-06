using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite.Parsers.PSD {
	internal class PSDParser: IPSDParser {

		public Bitmap PsdToBitmap(string filePath) {
			SimplePsd.CPSD psd = new SimplePsd.CPSD();
			psd.Load(filePath);
			return Bitmap.FromHbitmap(psd.GetHBitmap());
		}
	}
}
