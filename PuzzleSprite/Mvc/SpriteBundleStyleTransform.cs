using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace PuzzleSprite.Mvc {
	public class SpriteBundleStyleTransform : IBundleTransform {

		private readonly string _sourceFolder;
		private readonly string _outputFolder;
		private readonly string _imageUrl;

		public SpriteBundleStyleTransform(string imageUrlBase, string sourceFolder, string outputFolder) {
			
			this._sourceFolder = sourceFolder;
			this._outputFolder = outputFolder;
			this._imageUrl = imageUrlBase;
		}

		public void Process(BundleContext context, BundleResponse response) {

			response.Content = new Spriter()
				.TransformCSS(
					css: response.Content,
					sourcePath: this._sourceFolder,
					imageBundleOutputPath: this._outputFolder,
					imageUrl: this._imageUrl);
		}

	}
}
