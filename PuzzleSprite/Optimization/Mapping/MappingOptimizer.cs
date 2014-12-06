using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite.Optimization.Mapping {
	internal class MappingOptimizer {
		internal static void Optimize(SpriteSheet sheet) {

			int width = 0;
			int height = 0;

			var ordered = sheet.Sprites.OrderByDescending(sprite => sprite.Width).ToList();

			Sprite prevSprite = null;
			foreach(var img in ordered) {

				if(prevSprite != null) {

					int startX = prevSprite.X + prevSprite.Width;
					if((startX + img.Width) > width) {

						// new row
						img.X = 0;
						img.Y = height;
						height = height + img.Height;

					} else {

						// add to row.
						img.Y = prevSprite.Y;
						img.X = startX;

						var newHeight = img.Y + img.Height;
						if((newHeight) > height) {
							height = newHeight;
						}
					}

				} else {

					img.X = 0;
					img.Y = 0;
					width = img.Width;
					height = img.Height;

				}
				prevSprite = img;
			}

			sheet.Width = width;
			sheet.Height = height;

		}

	}
}
