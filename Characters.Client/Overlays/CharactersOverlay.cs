using Addemod.Characters.Client.Events;
using Addemod.Characters.Client.Models;
using NFive.SDK.Client.Interface;
using System;
using System.Collections.Generic;

namespace Addemod.Characters.Client.Overlays
{
	public class CharactersOverlay : Overlay
	{
		public event EventHandler<CreateCharacterOverlayEventArgs> CreateCharacterEvent;
		public event EventHandler<CharacterIdOverlayEventArgs> SelectCharacterEvent;
		public event EventHandler<CharacterIdOverlayEventArgs> DeleteCharacterEvent;
		public event EventHandler<OverlayEventArgs> DisconnectEvent;

		public List<Character> Characters { get; set; }

		public CharactersOverlay(List<Character> characters, IOverlayManager manager) : base(manager) {
			this.Characters = characters;

			On("disconnect", () => this.DisconnectEvent?.Invoke(this, new OverlayEventArgs(this)));
			On<Character>("createCharacter", (character) => this.CreateCharacterEvent?.Invoke(this, new CreateCharacterOverlayEventArgs(character, this)));
			On<Guid>("selectCharacter", (characterId) => this.SelectCharacterEvent?.Invoke(this, new CharacterIdOverlayEventArgs(characterId, this)));
			On<Guid>("deleteCharacter", (characterId) => this.DeleteCharacterEvent?.Invoke(this, new CharacterIdOverlayEventArgs(characterId, this)));
		}

		protected override dynamic Ready() => this.Characters.ToArray();

		public void SyncCharacters() {
			Emit("sync", this.Characters.ToArray());
		}
	}
}
