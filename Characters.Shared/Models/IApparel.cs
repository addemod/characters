using Addemod.Characters.Shared.Models.Apparel;
using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models {
	public interface IApparel : IIdentityModel {
		Component Face { get; set; }
		Component Head { get; set; }
		Component Hair { get; set; }
		Component Torso { get; set; }
		Component Legs { get; set; }
		Component Hands { get; set; }
		Component Shoes { get; set; }
		Component Special1 { get; set; }
		Component Special2 { get; set; }
		Component Special3 { get; set; }
		Component Textures { get; set; }
		Component Torso2 { get; set; }

		Prop Hat { get; set; }
		Prop Glasses { get; set; }
		Prop EarPiece { get; set; }
		Prop Unknown3 { get; set; }
		Prop Unknown4 { get; set; }
		Prop Unknown5 { get; set; }
		Prop Watch { get; set; }
		Prop Wristband { get; set; }
		Prop Unknown8 { get; set; }
		Prop Unknown9 { get; set; }
	}
}
