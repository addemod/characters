using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Models {
	public interface ICharacterSession {

		Guid Id { get; set; }
		DateTime DateCreated { get; set; }
		DateTime? DateConnected { get; set; }
		DateTime? DateDisconnected { get; set; }
		Guid CharacterId { get; set; }
	}
}
