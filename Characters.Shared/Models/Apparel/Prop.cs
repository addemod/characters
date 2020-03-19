using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models.Apparel {
	/**
	 * Accessories such as hats, watches etc.
	 */
	public class Prop {
		public PropType Type { get; set; }
		public int Index { get; set; }
		public int Texture { get; set; }
	}
}
