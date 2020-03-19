using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addemod.Characters.Shared.Exceptions {
	public class CharacterNotFoundException : Exception{
		public CharacterNotFoundException(Guid characterId) : base(GetMessage(characterId)) { }

		private static string GetMessage(Guid characterId) {
			return $"Character with id {characterId} was not found";
		}
	}
}
