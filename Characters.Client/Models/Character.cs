using Addemod.Characters.Shared.Models;
using Addemod.Characters.Shared.Models.Apparel;
using Addemod.Characters.Shared.Models.Appearance;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;
using NFive.SDK.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prop = Addemod.Characters.Shared.Models.Apparel.Prop;

namespace Addemod.Characters.Client.Models {
	public class Character : IdentityModel, ICharacter {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public Shared.Models.Gender Gender { get; set; }
		public int Health { get; set; }
		public int Armor { get; set; }
		public Position Position { get; set; }
		public string Model { get; set; }
		public string WalkingStyle { get; set; }
		public DateTime? LastPlayed { get; set; }
		public Guid ApparelId { get; set; }
		public Apparel Apparel { get; set; }
		public Guid AppearanceId { get; set; }
		public Appearance Appearance { get; set; }
		public Guid FaceShapeId { get; set; }
		public Heritage Heritage { get; set; }
		public Guid HeritageId { get; set; }
		public FaceShape FaceShape { get; set; }
		public Guid UserId { get; set; }


		[JsonIgnore] public string FullName => $"{FirstName} {LastName}".Replace("  ", " ");

		[JsonIgnore]
		public PedHash ModelHash {
			get
			{
				uint modelUInt = Convert.ToUInt32(Model);
				var pedHash = (PedHash)modelUInt;
				return pedHash;
			}
		}

		public void RenderCustom(ILogger logger) {
			// Only for free mode models
			//if (ModelHash != PedHash.FreemodeMale01 && ModelHash != PedHash.FreemodeFemale01) return;

			var fiveMPlayer = Game.Player.Character.Handle;

			// Set face from heritage
			API.SetPedHeadBlendData(fiveMPlayer, Heritage.Parent1, Heritage.Parent2, 0, Heritage.Parent1, Heritage.Parent2, 0, Heritage.Resemblance, Heritage.SkinTone, 0f, true);

			// Face appearance
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Blemishes.Type, Appearance.Blemishes.Index, Appearance.Blemishes.Opacity); // Blemishes
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Beard.Type, Appearance.Beard.Index, Appearance.Beard.Opacity); // Beard
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Eyebrows.Type, Appearance.Eyebrows.Index, Appearance.Eyebrows.Opacity); // Eyebrows
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Age.Type, Appearance.Age.Index, Appearance.Age.Opacity); // Age
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Makeup.Type, Appearance.Makeup.Index, Appearance.Makeup.Opacity); // Makeup
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Blush.Type, Appearance.Blush.Index, Appearance.Blush.Opacity); // Blush
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Complexion.Type, Appearance.Complexion.Index, Appearance.Complexion.Opacity); // Complexion
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.SunDamage.Type, Appearance.SunDamage.Index, Appearance.SunDamage.Opacity); // SunDamage
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Lipstick.Type, Appearance.Lipstick.Index, Appearance.Lipstick.Opacity); // Lipstick
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.MolesAndFreckles.Type, Appearance.MolesAndFreckles.Index, Appearance.MolesAndFreckles.Opacity); // Moles and freckles
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.Chest.Type, Appearance.Chest.Index, Appearance.Chest.Opacity); // Chest
			API.SetPedHeadOverlay(fiveMPlayer, (int)Appearance.BodyBlemishes.Type, Appearance.BodyBlemishes.Index, Appearance.BodyBlemishes.Opacity); // Body blemishes

			// Colors
			API.SetPedHairColor(fiveMPlayer, Appearance.HairColorId, Appearance.HairHighlightColor); // Hair color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Beard.Type, (int)Appearance.Beard.ColorType, Appearance.Beard.ColorId, Appearance.Beard.SecondColorId); // Beard color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Eyebrows.Type, (int)Appearance.Eyebrows.ColorType, Appearance.Eyebrows.ColorId, Appearance.Eyebrows.SecondColorId); // Eyebrows color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Makeup.Type, (int)Appearance.Makeup.ColorType, Appearance.Makeup.ColorId, Appearance.Makeup.SecondColorId); // Makeup color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Lipstick.Type, (int)Appearance.Lipstick.ColorType, Appearance.Lipstick.ColorId, Appearance.Lipstick.SecondColorId); // Lipstick color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Blush.Type, (int)Appearance.Blush.ColorType, Appearance.Blush.ColorId, Appearance.Blush.SecondColorId); // Blush color
			API.SetPedHeadOverlayColor(fiveMPlayer, (int)Appearance.Chest.Type, (int)Appearance.Chest.ColorType, Appearance.Chest.ColorId, Appearance.Chest.SecondColorId); // Chest color
			API.SetPedEyeColor(fiveMPlayer, Appearance.EyeColorId); // Eye color

			// Face features
			API.SetPedFaceFeature(fiveMPlayer, 0, FaceShape.NoseWidth);
			API.SetPedFaceFeature(fiveMPlayer, 1, FaceShape.NosePeakHeight);
			API.SetPedFaceFeature(fiveMPlayer, 2, FaceShape.NosePeakLength);
			API.SetPedFaceFeature(fiveMPlayer, 3, FaceShape.NoseBoneHeight);
			API.SetPedFaceFeature(fiveMPlayer, 4, FaceShape.NosePeakLowering);
			API.SetPedFaceFeature(fiveMPlayer, 5, FaceShape.NoseBoneTwist);
			API.SetPedFaceFeature(fiveMPlayer, 6, FaceShape.EyeBrowHeight);
			API.SetPedFaceFeature(fiveMPlayer, 7, FaceShape.EyeBrowLength);
			API.SetPedFaceFeature(fiveMPlayer, 8, FaceShape.CheekBoneHeight);
			API.SetPedFaceFeature(fiveMPlayer, 9, FaceShape.CheekBoneWidth);
			API.SetPedFaceFeature(fiveMPlayer, 10, FaceShape.CheekWidth);
			API.SetPedFaceFeature(fiveMPlayer, 11, FaceShape.EyeOpenings);
			API.SetPedFaceFeature(fiveMPlayer, 12, FaceShape.LipThickness);
			API.SetPedFaceFeature(fiveMPlayer, 13, FaceShape.JawBoneWidth);
			API.SetPedFaceFeature(fiveMPlayer, 14, FaceShape.JawBoneLength);
			API.SetPedFaceFeature(fiveMPlayer, 15, FaceShape.ChinBoneLowering);
			API.SetPedFaceFeature(fiveMPlayer, 16, FaceShape.ChinBoneLength);
			API.SetPedFaceFeature(fiveMPlayer, 17, FaceShape.ChinBoneWidth);
			API.SetPedFaceFeature(fiveMPlayer, 18, FaceShape.ChinDimple);
			API.SetPedFaceFeature(fiveMPlayer, 19, FaceShape.NeckThickness);
		}

		public async Task Render(ILogger logger) {
			var fiveMCharacter = Game.Player.Character;
			var fiveMCharacterStyle = fiveMCharacter.Style;

			// According to other plugins, this must be called (not tested without, just leaving as is)
			fiveMCharacter.Style.SetDefaultClothes();

			fiveMCharacter.Position = Position.ToVector3().ToCitVector3();
			fiveMCharacter.Armor = Armor;

			API.RequestClipSet(WalkingStyle);
			await BaseScript.Delay(100); // Required to load
			fiveMCharacter.MovementAnimationSet = WalkingStyle;


			fiveMCharacterStyle[PedComponents.Face].SetVariation(Apparel.Face.Index, Apparel.Face.Texture);
			fiveMCharacterStyle[PedComponents.Head].SetVariation(Apparel.Head.Index, Apparel.Head.Texture);

			// Temporary network visibility fix workaround
			fiveMCharacterStyle[PedComponents.Hair].SetVariation(1, 1);

			fiveMCharacterStyle[PedComponents.Hair].SetVariation(Apparel.Hair.Index, Apparel.Hair.Texture);

			fiveMCharacterStyle[PedComponents.Torso].SetVariation(Apparel.Torso.Index, Apparel.Torso.Texture);
			fiveMCharacterStyle[PedComponents.Legs].SetVariation(Apparel.Legs.Index, Apparel.Legs.Texture);
			fiveMCharacterStyle[PedComponents.Hands].SetVariation(Apparel.Hands.Index, Apparel.Hands.Texture);
			fiveMCharacterStyle[PedComponents.Shoes].SetVariation(Apparel.Shoes.Index, Apparel.Shoes.Texture);
			fiveMCharacterStyle[PedComponents.Special1].SetVariation(Apparel.Special1.Index, Apparel.Special1.Texture);
			fiveMCharacterStyle[PedComponents.Special2].SetVariation(Apparel.Special2.Index, Apparel.Special2.Texture);
			fiveMCharacterStyle[PedComponents.Special3].SetVariation(Apparel.Special3.Index, Apparel.Special3.Texture);
			fiveMCharacterStyle[PedComponents.Textures].SetVariation(Apparel.Textures.Index, Apparel.Textures.Texture);
			fiveMCharacterStyle[PedComponents.Torso2].SetVariation(Apparel.Torso2.Index, Apparel.Torso2.Texture);

			fiveMCharacterStyle[PedProps.Hats].SetVariation(Apparel.Hat.Index, Apparel.Hat.Texture);
			fiveMCharacterStyle[PedProps.Glasses].SetVariation(Apparel.Glasses.Index, Apparel.Glasses.Texture);
			fiveMCharacterStyle[PedProps.EarPieces].SetVariation(Apparel.EarPiece.Index, Apparel.EarPiece.Texture);
			fiveMCharacterStyle[PedProps.Unknown3].SetVariation(Apparel.Unknown3.Index, Apparel.Unknown3.Texture);
			fiveMCharacterStyle[PedProps.Unknown4].SetVariation(Apparel.Unknown4.Index, Apparel.Unknown4.Texture);
			fiveMCharacterStyle[PedProps.Unknown5].SetVariation(Apparel.Unknown5.Index, Apparel.Unknown5.Texture);
			fiveMCharacterStyle[PedProps.Watches].SetVariation(Apparel.Watch.Index, Apparel.Watch.Texture);
			fiveMCharacterStyle[PedProps.Wristbands].SetVariation(Apparel.Wristband.Index, Apparel.Wristband.Texture);
			fiveMCharacterStyle[PedProps.Unknown8].SetVariation(Apparel.Unknown8.Index, Apparel.Unknown8.Texture);
			fiveMCharacterStyle[PedProps.Unknown9].SetVariation(Apparel.Unknown9.Index, Apparel.Unknown9.Texture);
		}

		public static Apparel GetApparelFromPed(Ped ped, Guid? id = null) {
			return new Apparel() {
				Id = id ?? GuidGenerator.GenerateTimeBasedGuid(),
				Face = new Component {
					Type = ComponentType.Face,
					Index = ped.Style[PedComponents.Face].Index,
					Texture = ped.Style[PedComponents.Face].TextureIndex
				},
				Head = new Component {
					Type = ComponentType.Head,
					Index = ped.Style[PedComponents.Head].Index,
					Texture = ped.Style[PedComponents.Head].TextureIndex
				},
				Hair = new Component {
					Type = ComponentType.Hair,
					Index = ped.Style[PedComponents.Hair].Index,
					Texture = ped.Style[PedComponents.Hair].TextureIndex
				},
				Torso = new Component {
					Type = ComponentType.Torso,
					Index = ped.Style[PedComponents.Torso].Index,
					Texture = ped.Style[PedComponents.Torso].TextureIndex
				},
				Legs = new Component {
					Type = ComponentType.Legs,
					Index = ped.Style[PedComponents.Legs].Index,
					Texture = ped.Style[PedComponents.Legs].TextureIndex
				},
				Hands = new Component {
					Type = ComponentType.Hands,
					Index = ped.Style[PedComponents.Hands].Index,
					Texture = ped.Style[PedComponents.Hands].TextureIndex
				},
				Shoes = new Component {
					Type = ComponentType.Shoes,
					Index = ped.Style[PedComponents.Shoes].Index,
					Texture = ped.Style[PedComponents.Shoes].TextureIndex
				},
				Special1 = new Component {
					Type = ComponentType.Special1,
					Index = ped.Style[PedComponents.Special1].Index,
					Texture = ped.Style[PedComponents.Special1].TextureIndex
				},
				Special2 = new Component {
					Type = ComponentType.Special2,
					Index = ped.Style[PedComponents.Special2].Index,
					Texture = ped.Style[PedComponents.Special2].TextureIndex
				},
				Special3 = new Component {
					Type = ComponentType.Special3,
					Index = ped.Style[PedComponents.Special3].Index,
					Texture = ped.Style[PedComponents.Special3].TextureIndex
				},
				Textures = new Component {
					Type = ComponentType.Textures,
					Index = ped.Style[PedComponents.Textures].Index,
					Texture = ped.Style[PedComponents.Textures].TextureIndex
				},
				Torso2 = new Component {
					Type = ComponentType.Torso2,
					Index = ped.Style[PedComponents.Torso2].Index,
					Texture = ped.Style[PedComponents.Torso2].TextureIndex
				},
				Hat = new Prop {
					Type = PropType.Hat,
					Index = ped.Style[PedProps.Hats].Index,
					Texture = ped.Style[PedProps.Hats].TextureIndex
				},
				Glasses = new Prop {
					Type = PropType.Glasses,
					Index = ped.Style[PedProps.Glasses].Index,
					Texture = ped.Style[PedProps.Glasses].TextureIndex
				},
				EarPiece = new Prop {
					Type = PropType.EarPiece,
					Index = ped.Style[PedProps.EarPieces].Index,
					Texture = ped.Style[PedProps.EarPieces].TextureIndex
				},
				Unknown3 = new Prop {
					Type = PropType.Unknown3,
					Index = ped.Style[PedProps.Unknown3].Index,
					Texture = ped.Style[PedProps.Unknown3].TextureIndex
				},
				Unknown4 = new Prop {
					Type = PropType.Unknown4,
					Index = ped.Style[PedProps.Unknown4].Index,
					Texture = ped.Style[PedProps.Unknown4].TextureIndex
				},
				Unknown5 = new Prop {
					Type = PropType.Unknown5,
					Index = ped.Style[PedProps.Unknown5].Index,
					Texture = ped.Style[PedProps.Unknown5].TextureIndex
				},
				Watch = new Prop {
					Type = PropType.Watch,
					Index = ped.Style[PedProps.Watches].Index,
					Texture = ped.Style[PedProps.Watches].TextureIndex
				},
				Wristband = new Prop {
					Type = PropType.Wristband,
					Index = ped.Style[PedProps.Wristbands].Index,
					Texture = ped.Style[PedProps.Wristbands].TextureIndex
				},
				Unknown8 = new Prop {
					Type = PropType.Unknown8,
					Index = ped.Style[PedProps.Unknown8].Index,
					Texture = ped.Style[PedProps.Unknown8].TextureIndex
				},
				Unknown9 = new Prop {
					Type = PropType.Unknown9,
					Index = ped.Style[PedProps.Unknown9].Index,
					Texture = ped.Style[PedProps.Unknown9].TextureIndex
				}
			};
		}

		public static Heritage GetHeritageFromPed(Ped ped, Guid? id = null) {
			var headBlendData = ped.GetHeadBlendData();
			return new Heritage {
				Id = id ?? GuidGenerator.GenerateTimeBasedGuid(),
				Parent1 = headBlendData.FirstFaceShape,
				Parent2 = headBlendData.SecondFaceShape,
				Resemblance = headBlendData.ParentFaceShapePercent,
				SkinTone = headBlendData.ParentSkinTonePercent
			};
		}

		public static Appearance GetAppearanceFromPed(Ped ped, Guid? id = null) => new Appearance {
			Id = id ?? GuidGenerator.GenerateTimeBasedGuid(),

			EyeColorId = API.GetPedEyeColor(ped.Handle),
			HairColorId = API.GetPedHairColor(ped.Handle),
			HairHighlightColor = API.GetPedHairHighlightColor(ped.Handle),

			Blemishes = GetFeatureFromPed(ped, FeatureType.Blemishes),
			Beard = GetFeatureFromPed(ped, FeatureType.Beard),
			Eyebrows = GetFeatureFromPed(ped, FeatureType.Eyebrows),
			Age = GetFeatureFromPed(ped, FeatureType.Age),
			Makeup = GetFeatureFromPed(ped, FeatureType.Makeup),
			Blush = GetFeatureFromPed(ped, FeatureType.Blush),
			Complexion = GetFeatureFromPed(ped, FeatureType.Complexion),
			SunDamage = GetFeatureFromPed(ped, FeatureType.SunDamage),
			Lipstick = GetFeatureFromPed(ped, FeatureType.Lipstick),
			MolesAndFreckles = GetFeatureFromPed(ped, FeatureType.MolesAndFreckles),
			Chest = GetFeatureFromPed(ped, FeatureType.Chest),
			BodyBlemishes = GetFeatureFromPed(ped, FeatureType.BodyBlemishes)
		};

		public static FaceShape GetFaceShapeFromPed(Ped ped, Guid? id = null) => new FaceShape {
			Id = id ?? GuidGenerator.GenerateTimeBasedGuid(),

			NoseWidth = API.GetPedFaceFeature(ped.Handle, 0),
			NosePeakHeight = API.GetPedFaceFeature(ped.Handle, 1),
			NosePeakLength = API.GetPedFaceFeature(ped.Handle, 2),
			NoseBoneHeight = API.GetPedFaceFeature(ped.Handle, 3),
			NosePeakLowering = API.GetPedFaceFeature(ped.Handle, 4),
			NoseBoneTwist = API.GetPedFaceFeature(ped.Handle, 5),
			EyeBrowHeight = API.GetPedFaceFeature(ped.Handle, 6),
			EyeBrowLength = API.GetPedFaceFeature(ped.Handle, 7),
			CheekBoneHeight = API.GetPedFaceFeature(ped.Handle, 8),
			CheekBoneWidth = API.GetPedFaceFeature(ped.Handle, 9),
			CheekWidth = API.GetPedFaceFeature(ped.Handle, 10),
			EyeOpenings = API.GetPedFaceFeature(ped.Handle, 11),
			LipThickness = API.GetPedFaceFeature(ped.Handle, 12),
			JawBoneWidth = API.GetPedFaceFeature(ped.Handle, 13),
			JawBoneLength = API.GetPedFaceFeature(ped.Handle, 14),
			ChinBoneLowering = API.GetPedFaceFeature(ped.Handle, 15),
			ChinBoneLength = API.GetPedFaceFeature(ped.Handle, 16),
			ChinBoneWidth = API.GetPedFaceFeature(ped.Handle, 17),
			ChinDimple = API.GetPedFaceFeature(ped.Handle, 18),
			NeckThickness = API.GetPedFaceFeature(ped.Handle, 19)
		};

		protected static Feature GetFeatureFromPed(Ped ped, FeatureType featureType) {
			int index = 0;
			int colorType = 0;
			int colorId = 0;
			int secondColorId = 0;
			float opacity = 0;

			API.GetPedHeadOverlayData(ped.Handle, (int)featureType, ref index, ref colorType, ref colorId, ref secondColorId, ref opacity);

			return new Feature {
				Index = index,
				ColorId = colorId,
				ColorType = (FeatureColorType)colorType,
				SecondColorId = secondColorId,
				Opacity = opacity
			};
		}
	}
}
