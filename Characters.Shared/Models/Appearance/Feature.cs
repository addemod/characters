using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models.Appearance {
	public class Feature {
		public FeatureType Type { get; set; }
		public int Index { get; set; }
		public int Texture { get; set; }
		public float Opacity { get; set; }
		public FeatureColorType ColorType { get; set; }
		public int ColorId { get; set; }
		public int SecondColorId { get; set; }
	}
}
