using Addemod.Characters.Server.Models;
using Addemod.Characters.Shared;
using Addemod.Characters.Server.Events;
using JetBrains.Annotations;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addemod.Characters.Server.Storage;

namespace Addemod.Characters.Server {
	[Component(Lifetime = Lifetime.Singleton)]
	[PublicAPI]
	public class CharactersManager {
		private readonly ICommunicationManager comms;

		/// <summary>
		/// Occurs when a character session is being created for the clients selected character to play.
		/// </summary>
		public event EventHandler<CharacterEventArgs> Selecting;

		/// <summary>
		/// Occurs when a character session has been created for the clients selected character to play.
		/// </summary>
		public event EventHandler<CharacterSessionEventArgs> Selected;

		/// <summary>
		/// Occurs when a character session for a client's selected character is being disconnected.
		/// </summary>
		public event EventHandler<CharacterSessionEventArgs> Deselecting;

		/// <summary>
		/// Occurs when a character session for a client's selected character is disconnected.
		/// </summary>
		public event EventHandler<CharacterSessionEventArgs> Deselected;

		/// <summary>
		/// Gets the active character sessions.
		/// </summary>
		/// <value>
		/// The active character sessions.
		/// </value>
		public async Task<List<CharacterSession>> ActiveCharacterSessions() =>
			await this.comms.Event(CharacterEvents.GetActiveSessions).ToServer().Request<List<CharacterSession>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="CharactersManager"/> class.
		/// </summary>
		/// <param name="comms"></param>
		public CharactersManager(ICommunicationManager comms) {
			this.comms = comms;
			this.comms.Event(CharacterEvents.Selecting).FromServer().On<Character>((e, c) => this.Selecting?.Invoke(this, new CharacterEventArgs(c)));
			this.comms.Event(CharacterEvents.Selected).FromServer().On<CharacterSession>((e, c) => this.Selected?.Invoke(this, new CharacterSessionEventArgs(c)));
			this.comms.Event(CharacterEvents.Deselecting).FromServer().On<CharacterSession>((e, c) => this.Deselecting?.Invoke(this, new CharacterSessionEventArgs(c)));
			this.comms.Event(CharacterEvents.Deselected).FromServer().On<CharacterSession>((e, c) => this.Deselected?.Invoke(this, new CharacterSessionEventArgs(c)));
		}

		/// <summary>
		/// Selects the specified character identifier as the active character.
		/// </summary>
		/// <param name="characterId">The character identifier.</param>
		/// <returns></returns>
		public async Task<CharacterSession> Select(Guid characterId) => await this.comms.Event(CharacterEvents.Select).ToServer().Request<CharacterSession>(characterId);

		public Character GetCharacter(Guid characterId) {
			using(var ctx = new StorageContext()) {
				return ctx.Characters.FirstOrDefault(c => c.Id == characterId);
			}
		}

		// TODO Implement get character inventories
	}
}
