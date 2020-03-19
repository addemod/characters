using Addemod.Characters.Server.Models;
using System;

namespace Addemod.Characters.Server.Events {
	public class CharacterSessionEventArgs : EventArgs {
		public CharacterSession CharacterSession { get; }

		public CharacterSessionEventArgs(CharacterSession characterSession) {
			this.CharacterSession = characterSession;
		}
	}
}
