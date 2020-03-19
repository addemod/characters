using Addemod.Characters.Client.Models;
using NFive.SDK.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Client.Events {
	public class CharacterIdOverlayEventArgs : OverlayEventArgs {

		public Guid CharacterId { get; }
		public CharacterIdOverlayEventArgs(Guid characterId, Overlay overlay) : base(overlay) {
			this.CharacterId = characterId;
		}

	}
}
