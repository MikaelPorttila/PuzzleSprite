using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace PuzzleSprite.Web.Lib.Bundles {
	public class PuzzleSpriteCSSBundle : IBundleTransform {

		public void Process(BundleContext context, BundleResponse response) {

			var folder = "/Content/image/";
			var path = System.Web.HttpContext.Current.Server.MapPath("~" + folder);
			response.Content = new Spriter().TransformCSS(response.Content, path, path, folder);
		}
	}
}