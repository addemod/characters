using Addemod.Characters.Server.Models;
using System;

namespace Addemod.Characters.Server.Events {
	public class CharacterEventArgs : EventArgs {
		public Character Character { get; }

		public CharacterEventArgs(Character character) {
			this.Character = character;
		}
	}
}
