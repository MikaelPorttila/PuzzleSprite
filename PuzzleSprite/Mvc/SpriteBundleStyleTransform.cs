using System.Web.Optimization;

namespace PuzzleSprite {
	public class SpriteBundleStyleTransform : IBundleTransform {

		private readonly string _source;
		private readonly string _output;
		private readonly string _url;

		public SpriteBundleStyleTransform(string url, string sourcePath, string outputPath) {
			
			this._source = sourcePath;
			this._output = outputPath;
			this._url = url;
		}

		public void Process(BundleContext context, BundleResponse response) {
			
			response.Content = new Spriter()
				.TransformCSS(
					css: response.Content,
					sourcePath: context.HttpContext.Server.MapPath(this._source),
					imageBundleOutputPath: context.HttpContext.Server.MapPath(this._output),
					imageUrl: this._url);
		}

	}
}
