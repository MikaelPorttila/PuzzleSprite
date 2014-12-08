﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using System.Web;

namespace PuzzleSprite {
	public class ImageAutoBundleStyleTransform : IBundleTransform {

		private readonly string[] _paths;
		private readonly string _output;
		private readonly string _url;


		public ImageAutoBundleStyleTransform(string url, string output, params string[] paths) {
			this._output = output;
			this._paths = paths;
			this._url = url;
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
					paths.ToArray());
		}
	}
}
