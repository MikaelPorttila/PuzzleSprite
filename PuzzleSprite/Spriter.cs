﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PuzzleSprite.Helpers;
using System.Drawing;
using PuzzleSprite.Extensions;
using System.Text.RegularExpressions;
using PuzzleSprite.Optimization.Mapping;

namespace PuzzleSprite {
	
	public class Spriter {

		private BitmapProvider _bitmapProvider;
		private BitmapProvider BitmapProvider {
			get {
				if(this._bitmapProvider == null) {
					this._bitmapProvider = new BitmapProvider();
				}

				return this._bitmapProvider;
			}
		}

		private void ProcessSprites(SpriteSheet sheet, FileInfo[] files, Action<Sprite, string, Bitmap> process) {

			if(sheet.Sprites == null) {
				return;
			}

			foreach(var sprite in sheet.Sprites) {
				var file = files.FirstOrDefault(f => f.Name == sprite.Name);
				if(file != null) {
					using(var bitmap = this.BitmapProvider.GetBitmap(file.FullName)) {
						if(bitmap != null) {
							process(sprite, file.FullName, bitmap);
						}
					}
				}
			}
		}

		public void Combine(SpriteParameters parameters) {

			if(parameters == null) {
				throw new ArgumentNullException("parameters is null");
			}

			if(!Directory.Exists(parameters.OutputPath)) {
				Directory.CreateDirectory(parameters.OutputPath);
			}

			foreach(var sheet in parameters.SpriteSheets) {

				if(string.IsNullOrEmpty(sheet.SourcePath) || !Directory.Exists(sheet.SourcePath)) {
					return;
				}

				var sourceFiles = new DirectoryInfo(sheet.SourcePath).GetFiles();
				sheet.Sprites = sheet.Sprites.DistinctBy(sprite => sprite.Name).ToList();

				List<byte[]> hashes = new List<byte[]>();

				this.ProcessSprites(sheet, sourceFiles, (sprite, path, bitmap) => {

					hashes.Add(HashHelper.GetHash(path));
					sprite.Height = bitmap.Height;
					sprite.Width = bitmap.Width;
					sprite.X = 0;
					sprite.Y = sheet.Height;
					sheet.Height += bitmap.Height;

					if(sheet.Width < sprite.Width) {
						sheet.Width = sprite.Width;
					}

				});

				MappingOptimizer.Optimize(sheet);

				sheet.Name = HashHelper.CombineHashesToString(hashes) + ".png";
				var outputPath = Path.Combine(parameters.OutputPath, sheet.Name);
				if(File.Exists(outputPath)) {
					return;
				}

				// Build image	
				sheet.Image = new Bitmap(sheet.Width, sheet.Height);
				using(Graphics graphic = Graphics.FromImage(sheet.Image)) {
					this.ProcessSprites(sheet, sourceFiles, (sprite, path, bitmap) => {
						graphic.DrawImage(bitmap, new Point(sprite.X, sprite.Y));
					});
				}

				sheet.Image.Save(outputPath);
				// TODO: opti png
			}

		}

		public string TransformCSS (string css, string sourcePath, string imageBundleOutputPath, string imageUrl) {

			if(string.IsNullOrEmpty(css)) {
				return css;
			}

			// Parse css.
			List<string> spriteRequests = new List<string>();
			var requests = CssHelper.GetAttributes(css);
			foreach(Match attr in requests) {
				spriteRequests.Add(CssHelper.GetAttributeValue(attr.Value));
			}
			SpriteSheet sheet = new SpriteSheet(sourcePath, spriteRequests.ToArray());


			// Execute image bundling
			this.Combine(new SpriteParameters {
				SpriteSheets = new List<SpriteSheet>(1) { sheet },
				OutputPath = imageBundleOutputPath
			});

			// Transform Css
			foreach(Match req in requests) {
				string attr = req.Value;
				string spriteName = CssHelper.GetAttributeValue(attr);

				var replaceCss = "";
				var sprite = sheet.Sprites.FirstOrDefault(e => e.Name == spriteName);
				if(sprite != null) {
					replaceCss = CssHelper.GetSpriteCss(
						imageUrl + sheet.Name, 
						sprite.Width, 
						sprite.Height, 
						sprite.X, 
						sprite.Y);
				}

				css = css.Replace(attr, replaceCss);
			}

			return css;
		}
		
	}
}