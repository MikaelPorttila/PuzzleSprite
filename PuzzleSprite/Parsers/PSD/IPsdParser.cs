using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite.Parsers.PSD {
	internal interface IPSDParser {
		Bitmap PsdToBitmap(string filePath);
	}
}
