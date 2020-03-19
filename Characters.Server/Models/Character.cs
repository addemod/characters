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
		public Guid ApparelId { get; set; }

		[ForeignKey("ApparelId")]
		public virtual Apparel Apparel { get; set; }

		[Required]
		public Guid AppearanceId { get; set; }

		[ForeignKey("AppearanceId")]
		public virtual Appearance Appearance { get; set; }

		[Required]
		public Guid FaceShapeId { get; set; }

		[ForeignKey("FaceShapeId")]
		public virtual FaceShape FaceShape { get; set; }


		[Required]
		public Guid HeritageId { get; set; }

		[ForeignKey("HeritageId")]
		public virtual Heritage Heritage { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[JsonIgnore]
		public string FullName => $"{this.FirstName} {this.LastName}".Replace("  ", " ");
	}
}
