using Addemod.Characters.Shared.Models;
using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Server.Models {
	public class FaceShape : IdentityModel, IFaceShape {
		public float NoseWidth { get; set; }
		public float NosePeakHeight { get; set; }
		public float NosePeakLength { get; set; }
		public float NoseBoneHeight { get; set; }
		public float NosePeakLowering { get; set; }
		public float NoseBoneTwist { get; set; }
		public float EyeBrowHeight { get; set; }
		public float EyeBrowLength { get; set; }
		public float CheekBoneHeight { get; set; }
		public float CheekBoneWidth { get; set; }
		public float CheekWidth { get; set; }
		public float EyeOpenings { get; set; }
		public float LipThickness { get; set; }
		public float JawBoneWidth { get; set; }
		public float JawBoneLength { get; set; }
		public float ChinBoneLowering { get; set; }
		public float ChinBoneLength { get; set; }
		public float ChinBoneWidth { get; set; }
		public float ChinDimple { get; set; }
		public float NeckThickness { get; set; }
	}
}
