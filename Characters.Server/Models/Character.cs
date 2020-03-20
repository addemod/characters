using Addemod.Characters.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;
using NFive.SDK.Core.Models.Player;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Addemod.Characters.Server.Models {
	public class Character : IdentityModel, ICharacter {
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string LastName { get; set; }
		[Required]
		public DateTime BirthDate { get; set; }
		[Required]
		public Gender Gender { get; set; }
		[Required]
		[Range(0, 100)]
		public int Health { get; set; }
		[Required]
		[Range(0, 100)]
		public int Armor { get; set; }
		[Required]
		public Position Position { get; set; }
		[Required]
		public string Model { get; set; }
		[Required]
		public string WalkingStyle { get; set; }
		public DateTime? LastPlayed { get; set; }

		[Required]
		[ForeignKey("Apparel")]
		public Guid ApparelId { get; set; }

		public virtual Apparel Apparel { get; set; }

		[Required]
		[ForeignKey("Appearance")]
		public Guid AppearanceId { get; set; }
		public virtual Appearance Appearance { get; set; }

		[Required]

		[ForeignKey("FaceShape")]
		public Guid FaceShapeId { get; set; }
		public virtual FaceShape FaceShape { get; set; }


		[Required]

		[ForeignKey("Heritage")]
		public Guid HeritageId { get; set; }
		public virtual Heritage Heritage { get; set; }

		[Required]
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; }

		[JsonIgnore]
		public string FullName => $"{this.FirstName} {this.LastName}".Replace("  ", " ");
	}
}
