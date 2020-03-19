using Addemod.Characters.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models.Player;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Addemod.Characters.Server.Models {
	public class CharacterSession : ICharacterSession {
		[Key]
		[Required]
		public Guid Id { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateConnected { get; set; }
		public DateTime? DateDisconnected { get; set; }

		[Required]
		public Guid CharacterId { get; set; }

		[ForeignKey("CharacterId")]
		public virtual Character Character { get; set; }

		[Required]
		public Guid SessionId { get; set; }

		// Foreign key to NFive session
		[ForeignKey("SessionId")]
		public virtual Session Session { get; set; }

		[JsonIgnore]
		public bool IsConnected => DateConnected.HasValue && !DateDisconnected.HasValue;
	}
}
