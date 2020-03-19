using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models.Appearance {
	public enum FeatureColorType {
		/**
		 * 1: Eyebrows, beard, chest hair
		 * 2: Blush, lipstick
		 * 0: Misc
		 */
		 Misc = 0,
		 Eyebrows = 1,
		 Beard = 1,
		 Chest = 1,
		 Blush = 2,
		 Lipstick = 2
	}
}
