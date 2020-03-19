using Addemod.Characters.Shared.Models;
using Addemod.Characters.Shared.Models.Appearance;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;

namespace Addemod.Characters.Server.Models {
	public class Appearance : IdentityModel, IAppearance {
		public int EyeColorId { get; set; }
		public int HairColorId { get; set; }
		public int HairHighlightColor { get; set; }
		public Feature Blemishes { get; set; }
		public Feature Beard { get; set; }
		public Feature Eyebrows { get; set; }
		public Feature Age { get; set; }
		public Feature Makeup { get; set; }
		public Feature Blush { get; set; }
		public Feature Complexion { get; set; }
		public Feature SunDamage { get; set; }
		public Feature Lipstick { get; set; }
		public Feature MolesAndFreckles { get; set; }
		public Feature Chest { get; set; }
		public Feature BodyBlemishes { get; set; }

		public Appearance() {
			Id = GuidGenerator.GenerateTimeBasedGuid();
			Blemishes = new Feature() {
				Type = FeatureType.Blemishes,
				ColorType = FeatureColorType.Misc
			};
			Beard = new Feature() {
				Type = FeatureType.Beard,
				ColorType = FeatureColorType.Beard
			};
			Eyebrows = new Feature() {
				Type = FeatureType.Eyebrows,
				ColorType = FeatureColorType.Eyebrows,
			};
			Age = new Feature() {
				Type = FeatureType.Age,
				ColorType = FeatureColorType.Misc,
			};
			Makeup = new Feature() {
				Type = FeatureType.Makeup,
				ColorType = FeatureColorType.Misc, // ??
			};
			Blush = new Feature() {
				Type = FeatureType.Blush,
				ColorType = FeatureColorType.Blush,
			};
			Complexion = new Feature() {
				Type = FeatureType.Complexion,
				ColorType = FeatureColorType.Misc,
			};
			SunDamage = new Feature() {
				Type = FeatureType.SunDamage,
				ColorType = FeatureColorType.Misc,
			};
			Lipstick = new Feature() {
				Type = FeatureType.Lipstick,
				ColorType = FeatureColorType.Lipstick,
			};
			MolesAndFreckles = new Feature() {
				Type = FeatureType.MolesAndFreckles,
				ColorType = FeatureColorType.Misc,
			};
			Chest = new Feature() {
				Type = FeatureType.Chest,
				ColorType = FeatureColorType.Chest,
			};
			BodyBlemishes = new Feature() {
				Type = FeatureType.BodyBlemishes,
				ColorType = FeatureColorType.Misc,
			};
		}
	}
}
