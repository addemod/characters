using Addemod.Characters.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Client.Models {
	public class CharacterSession : ICharacterSession {
		public Guid Id { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
		public DateTime? DateConnected { get; set; }
		public DateTime? DateDisconnected { get; set; }
		public Guid CharacterId { get; set; }
		public virtual Character Character { get; set; }

		[JsonIgnore]
		public bool IsConnected => this.DateConnected.HasValue && !this.DateDisconnected.HasValue;
	}
}
