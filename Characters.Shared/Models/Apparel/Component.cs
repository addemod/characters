using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models.Apparel {
	/**
	 * Components such as tattoos, scars, etc.
	 */
	public class Component {
		public ComponentType Type { get; set; }
		public int Index { get; set; }
		public int Texture { get; set; }
	}
}
