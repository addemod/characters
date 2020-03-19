using Addemod.Characters.Shared.Models;
using Addemod.Characters.Shared.Models.Apparel;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;

namespace Addemod.Characters.Server.Models {
	public class Apparel : IdentityModel, IApparel {
		public Component Face { get; set; }
		public Component Head { get; set; }
		public Component Hair { get; set; }
		public Component Torso { get; set; }
		public Component Legs { get; set; }
		public Component Hands { get; set; }
		public Component Shoes { get; set; }
		public Component Special1 { get; set; }
		public Component Special2 { get; set; }
		public Component Special3 { get; set; }
		public Component Textures { get; set; }
		public Component Torso2 { get; set; }
		public Prop Hat { get; set; }
		public Prop Glasses { get; set; }
		public Prop EarPiece { get; set; }
		public Prop Unknown3 { get; set; }
		public Prop Unknown4 { get; set; }
		public Prop Unknown5 { get; set; }
		public Prop Watch { get; set; }
		public Prop Wristband { get; set; }
		public Prop Unknown8 { get; set; }
		public Prop Unknown9 { get; set; }

		public Apparel() {
			Id = GuidGenerator.GenerateTimeBasedGuid();
			Face = new Component();
			Head = new Component();
			Hair = new Component();
			Torso = new Component();
			Torso2 = new Component();
			Legs = new Component();
			Hands = new Component();
			Shoes = new Component();
			Special1 = new Component();
			Special2 = new Component();
			Special3 = new Component();
			Textures = new Component();

			Hat = new Prop();
			Glasses = new Prop();
			EarPiece = new Prop();
			Unknown3 = new Prop();
			Unknown4 = new Prop();
			Unknown5 = new Prop();
			Watch = new Prop();
			Wristband = new Prop();
			Unknown8 = new Prop();
			Unknown9 = new Prop();
		}
	}
}
