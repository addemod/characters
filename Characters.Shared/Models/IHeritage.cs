using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models {
	public interface IHeritage : IIdentityModel{
		int Parent1 { get; set; }
		int Parent2 { get; set; }
		float Resemblance { get; set; }
		float SkinTone { get; set; }
	}
}
