using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace PuzzleSprite {
	public class ImageBundleStyleTransform : IBundleTransform {

		#region Variables
		
		private List<string> _paths;
		private readonly string _output;
		private readonly string _url;
		private readonly string _prefix;

		#endregion Variables

		#region Constructors

		public ImageBundleStyleTransform(string url, string output): this(url, output, "") { }
		public ImageBundleStyleTransform(string url, string output, string prefix) {
			this._output = output;
			this._url = url;
			this._paths = new List<string>();
			this._prefix = prefix;
		}

		#endregion Constructors

		#region Methods

		public IBundleTransform Include(params string[] paths) {

			if(paths != null){
				this._paths.AddRange(paths);
			}

			return this;
		}

		public void Process(BundleContext context, BundleResponse response) {

			List<string> paths = new List<string>();
			foreach(var path in this._paths) {
				paths.Add(context.HttpContext.Server.MapPath(path));
			}

			response.Content = new Spriter()
				.TransformCSS(
					response.Content, 
					this._url, 
					context.HttpContext.Server.MapPath(this._output),
					this._prefix,
					paths.ToArray());
		}

		#endregion Methods
	}
}
