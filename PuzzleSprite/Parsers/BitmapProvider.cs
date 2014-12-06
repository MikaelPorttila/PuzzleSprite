using PuzzleSprite.Parsers.PSD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite {
	public class BitmapProvider {

		private IPSDParser _psdParser;
		public BitmapProvider() {
			this._psdParser = new PSDParser();
		}

		public Bitmap GetBitmap(string filePath) {

			Image img = null;
			switch(Path.GetExtension(filePath).ToLower()) {
				case ".psd":
					img = this._psdParser.PsdToBitmap(filePath);
					break;
				default:
					img = Bitmap.FromFile(filePath);
					break;
			}

			if(img != null) {
				return img as Bitmap;
			}

			return null;
		} 

	}
}
