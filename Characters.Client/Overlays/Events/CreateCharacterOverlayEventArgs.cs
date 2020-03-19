using Addemod.Characters.Client.Models;
using NFive.SDK.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Client.Events {
	public class CreateCharacterOverlayEventArgs : OverlayEventArgs {

		public Character Character { get; }
		public CreateCharacterOverlayEventArgs(Character character, Overlay overlay) : base(overlay) {
			this.Character = character;
		}

	}
}
