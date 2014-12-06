using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSprite {
	public class Sprite {

		#region Constructor(s)

		public Sprite(string name) {
			this.Name = name;
		}

		#endregion Constructor(s)

		#region Properties

		public string Name { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }

		#endregion Properties
	
	}
}
