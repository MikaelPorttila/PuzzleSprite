using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace PuzzleSprite {
	public class SpriteBundleStyleTransform : IBundleTransform {

		private readonly string _sourcePath;
		private readonly string _outputPath;
		private readonly string _imageUrl;

		public SpriteBundleStyleTransform(string imageUrlBase, string sourcePath, string outputPath) {
			
			this._sourcePath = sourcePath;
			this._outputPath = outputPath;
			this._imageUrl = imageUrlBase;
		}

		public void Process(BundleContext context, BundleResponse response) {

			response.Content = new Spriter()
				.TransformCSS(
					css: response.Content,
					sourcePath: this._sourcePath,
					imageBundleOutputPath: this._outputPath,
					imageUrl: this._imageUrl);
		}

	}
}
