﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleSprite.Helpers {
	internal class CssHelper {


		internal static List<string> ParseAttributes(string css) {
			List<string> spriteAttributes = new List<string>();

			var selectors = css.Split(new char[1]{'}'}, StringSplitOptions.RemoveEmptyEntries);
			foreach(var selector in selectors) {
				
				var attributes = selector
					.Substring(selector.IndexOf('{') + 1)
					.Split(new char[1]{';'}, StringSplitOptions.RemoveEmptyEntries);
				
				foreach(var attr in attributes) {

					if(attr.Split(':')[0].Trim() == "puzzle") {
						spriteAttributes.Add(attr);
					}

				}
			}

			return spriteAttributes;
		}

		internal static string GetAttributeValue(string attribute) {
			return attribute.Split(':')[1].Trim().TrimEnd(';');
		}

		internal static string GetSpriteCssWithClass(string className, string imageUrl, Sprite sprite) {
			return "." + className + "{" + GetSpriteCss(imageUrl, sprite.Width, sprite.Height, sprite.X, sprite.Y) + "} ";
		}
		internal static string GetSpriteCss(string name, int width, int height, int x, int y) {
			return
				string.Format("background-image: url(\"{0}\"); ", name) +
				string.Format("width: {0}px; ", width) +
				string.Format("height: {0}px; ", height) +
				string.Format("background-position: -{0}px -{1}px; ", x, y);
		}
	}
}
