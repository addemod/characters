using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models {
	public interface ICharacter : IIdentityModel {
		string FirstName { get; set; }
		string LastName { get; set; }
		DateTime BirthDate { get; set; }
		Gender Gender { get; set; }
		int Health { get; set; }
		int Armor { get; set; }
		Position Position { get; set; }
		string Model { get; set; }
		string WalkingStyle { get; set; }
		DateTime? LastPlayed { get; set; }
	}
}
