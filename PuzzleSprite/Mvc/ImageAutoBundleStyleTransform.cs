using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace PuzzleSprite {
	public class ImageAutoBundleStyleTransform : IBundleTransform {

		private readonly string[] _paths;
		private readonly string _output;
		private readonly string _imageUrlBase;


		public ImageAutoBundleStyleTransform(string imageUrlBase, string output, params string[] paths) {
			this._output = output;
			this._paths = paths;
			this._imageUrlBase = imageUrlBase;
		}

		public void Process(BundleContext context, BundleResponse response) {

			response.Content = new Spriter()
				.TransformCSS(
					response.Content, 
					this._imageUrlBase, 
					this._output, 
					this._paths);
		}
	}
}
