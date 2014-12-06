using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleSprite.Helpers {
	internal class CssHelper {

		internal static MatchCollection GetAttributes(string css) {
			return Regex.Matches(css, "(puzzle)\\s*:\\s*([^;]+);");
		}

		internal static string GetAttributeValue(string attribute) {
			return attribute.Split(':')[1].Trim().TrimEnd(';');
		}

		internal static string GetSpriteCss(string name, int width, int height, int x, int y) {
			return
				string.Format("background-image: url({0}); ", name) +
				string.Format("width: {0}px; ", width) +
				string.Format("height: {0}px; ", height) +
				string.Format("background-position: -{0}px -{1}px; ", x, y);
		}
	}
}
