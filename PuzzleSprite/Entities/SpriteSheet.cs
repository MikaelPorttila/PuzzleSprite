using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite {
	public class SpriteSheet {

		#region Constructors

		public SpriteSheet(string sourcePath):this(sourcePath, filenames: null) { }
		public SpriteSheet(string sourcePath, params string[] filenames) {
			this.SourcePath = sourcePath;
			this.Sprites = new List<Sprite>();
			if(filenames != null) {
				this.AddMaps(filenames);
			}

			this.Height = 0;
			this.Width = 0;

		}

		#endregion Constructors

		#region Properties

		public List<Sprite> Sprites { get; set; }

		public string SourcePath { get; set; }

		internal int Height { get; set; }

		internal int Width { get; set; }

		public string Name { get; set; }

		internal Image Image { get; set; }

		#endregion Properties

		#region Methods

		public void AddMap(string name) {
			this.Sprites.Add(new Sprite(name));
		}
		public void AddMaps(params string[] maps) {
			if(maps != null) {
				for(int i = 0; i < maps.Length; i++) {
					this.AddMap(maps[i]);
				}
			}
		}

		#endregion Methods

	}
}
