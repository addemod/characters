using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Z.EntityFramework.Plus;

using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Core.Utilities;

using Addemod.Characters.Shared;
using Addemod.Characters.Server.Storage;
using Addemod.Characters.Server.Models;
using Addemod.Characters.Shared.Exceptions;
using CitizenFX.Core.Native;
using System.Data.Entity.Migrations;

namespace Addemod.Characters.Server
{
	public class NFundamentalsCharactersController : ConfigurableController<CharactersConfiguration>
	{
		private readonly List<CharacterSession> CharacterSessions = new List<CharacterSession>();
		private ICommunicationManager comms;

		public NFundamentalsCharactersController(ILogger logger, CharactersConfiguration config, ICommunicationManager comms) : base(logger, config) {
			this.comms = comms;
			// Send configuration when requested
			comms.Event(CharacterEvents.Configuration).FromClients().OnRequest(e => e.Reply(this.Configuration));

			// Character events
			comms.Event(CharacterEvents.GetAllForUser).FromClients().OnRequest(GetAllCharactersForUser);
			comms.Event(CharacterEvents.Create).FromClients().OnRequest<Character>(CreateCharacter);
			comms.Event(CharacterEvents.Delete).FromClients().OnRequest<Guid>(DeleteCharacter);
			comms.Event(CharacterEvents.Select).FromClients().OnRequest<Guid>(SelectCharacter);
			comms.Event(CharacterEvents.SaveCharacter).FromClients().On<Character>(SaveCharacter);
			comms.Event(CharacterEvents.SavePosition).FromClients().On<Guid, Position>(SaveCharacterPosition);
			comms.Event(CharacterEvents.GetById).FromClients().OnRequest<Guid>(GetCharacterById);

			// Get all active character sessions
			comms.Event(CharacterEvents.GetActiveSessions).FromServer().OnRequest(e => e.Reply(this.CharacterSessions));

			// SessionManager plugin event when a client disconnects
			comms.Event(SessionEvents.DisconnectPlayer).FromServer().On<IClient, Session>(OnPlayerDisconnected);
		}

		private void GetCharacterById(ICommunicationMessage e, Guid characterId) {
			using (var ctx = new StorageContext()) {
				e.Reply(ctx.Characters.FirstOrDefault(c => c.Id == characterId));
			}
		}

		private void GetAllCharactersForUser(ICommunicationMessage e) {
			GetAllCharactersForUser(e, e.User.Id);
		}

		private void GetAllCharactersForUser(ICommunicationMessage e, Guid UserId) {
			using (var ctx = new StorageContext()) {
				var characters = ctx.Characters.Where(c => c.UserId == UserId && c.Deleted == null).ToList();
				e.Reply(characters);
			}
		}

		private async void CreateCharacter(ICommunicationMessage e, Character character) {
			// Set some values that we don't trust from the client
			character.Id = GuidGenerator.GenerateTimeBasedGuid();
			character.UserId = e.User.Id;
			character.LastPlayed = DateTime.UtcNow;
			character.Position = new Position(0f, 0f, 71f);
			character.Apparel = new Apparel();
			character.Appearance = new Appearance();
			character.FaceShape = new FaceShape();
			character.Heritage = new Heritage();
			character.Armor = 0;
			character.Health = 100;

			// Store character in database
			using (var ctx = new StorageContext())
			using (var trans = ctx.Database.BeginTransaction()) {
				try {
					ctx.Characters.Add(character);
					await ctx.SaveChangesAsync();
					trans.Commit();

					this.Logger.Debug($"Saved new character {character.FullName} for user {e.User.Name}");

					// Reply with the character
					e.Reply(character);
				} catch (Exception ex) {
					this.Logger.Error(ex);
					trans.Rollback();
					// TODO Reply with an error so client doesn't hang
					e.Reply(null);
				}
			}
		}

		private async void DeleteCharacter(ICommunicationMessage e, Guid characterId) {
			using (var ctx = new StorageContext()) {
				// Make sure the character we try to delete are our own
				var character = ctx.Characters.First(c => c.Id == characterId && c.UserId == e.User.Id);
				character.Deleted = DateTime.UtcNow;
				await ctx.SaveChangesAsync();
				GetAllCharactersForUser(e, e.User.Id);
			}
		}

		private async void SelectCharacter(ICommunicationMessage e, Guid characterId) {
			await DeselectAll(e.User.Id);

			using (var ctx = new StorageContext())
			using (var trans = ctx.Database.BeginTransaction()) {
				try {
					var character = ctx.Characters.FirstOrDefault(c => c.Id == characterId && c.UserId == e.User.Id);
					if (character == null)
						throw new CharacterNotFoundException(characterId);

					this.comms.Event(CharacterEvents.Selecting).ToServer().Emit(character);

					var newCharSession = new CharacterSession() {
						Id = GuidGenerator.GenerateTimeBasedGuid(),
						CharacterId = character.Id,
						Character = character,
						DateCreated = DateTime.UtcNow,
						DateConnected = DateTime.UtcNow,
						SessionId = e.Session.Id
					};

					ctx.CharacterSessions.Add(newCharSession);
					await ctx.SaveChangesAsync();
					trans.Commit();

					this.Logger.Debug($"Created session for character {character.FullName} for user {e.User.Name}:");
					this.Logger.Debug(new Serializer().Serialize(newCharSession));

					newCharSession.Session = e.Session;

					e.Reply(newCharSession);

					this.CharacterSessions.Add(newCharSession);
					this.comms.Event(CharacterEvents.Selected).ToServer().Emit(newCharSession);

				} catch (Exception ex) {
					this.Logger.Error(ex);
					trans.Rollback();
					e.Reply(null);
				}
			}
		}

		private async void SaveCharacter(ICommunicationMessage e, Character character) {
			using (var ctx = new StorageContext())
			using (var trans = ctx.Database.BeginTransaction()) {
				try {
					ctx.Characters.AddOrUpdate(character);
					await ctx.SaveChangesAsync();
					trans.Commit();

					// If the player has an active session, update the character data with the one we just saved
					var activeSession = this.CharacterSessions.FirstOrDefault(s => s.Character.Id == character.Id);
					if (activeSession == null) return;
					activeSession.Character = character;

					this.Logger.Debug($"Saved existing character {character.FullName} for user {e.User.Name}");
				} catch (Exception ex) {
					this.Logger.Error(ex);
					trans.Rollback();
				}
			}
		}

		private async void SaveCharacterPosition(ICommunicationMessage e, Guid characterId, Position position) {
			using (var ctx = new StorageContext())
			using (var trans = ctx.Database.BeginTransaction()) {
				try {
					// SingleOrDefault not needed as we have try-catch if null
					var character = ctx.Characters.Single(c => c.Id == characterId);
					character.Position = position;
					await ctx.SaveChangesAsync();
					trans.Commit();

					this.Logger.Debug($"Saved position for character {character.FullName} for user {e.User.Name}");
				} catch (Exception ex) {
					this.Logger.Error(ex);
					trans.Rollback();
				}

				// If the player has an active session, update the character data with the one we just changed position for
				var activeSession = this.CharacterSessions.FirstOrDefault(s => s.Character.Id == characterId);
				if (activeSession == null) return;
				activeSession.Character.Position = position;
			}
		}

		private async void OnPlayerDisconnected(ICommunicationMessage e, IClient client, Session session) {
			await DeselectAll(session.UserId);
			API.DropPlayer(client.License, "Disconnect from character selection");
		}


		public async Task DeselectAll(Guid UserId) {
			using (var ctx = new StorageContext())
			using (var trans = ctx.Database.BeginTransaction()) {
				// Find all active character sessions for this user
				var activeSessions = ctx.CharacterSessions.Where(s => s.Character.UserId == UserId && s.DateDisconnected == null).ToList();

				foreach (var charSession in activeSessions) {
					// Tell the server that we are deselecting a character session
					this.comms.Event(CharacterEvents.Deselecting).ToServer().Emit(charSession);
					// Set date connected to null as we are not connected anymore
					charSession.DateConnected = null;
					// Date disconnected is set to "now"
					charSession.DateDisconnected = DateTime.UtcNow;
					// Update database row on save
					ctx.CharacterSessions.AddOrUpdateExtension(charSession);
				}

				await ctx.SaveChangesAsync();
				trans.Commit();

				// Remove all active sessions from session store and emit event that we deselected all
				foreach (var charSession in activeSessions) {
					this.CharacterSessions.RemoveAll(c => c.Id == charSession.Id);
					this.comms.Event(CharacterEvents.Deselected).ToServer().Emit(charSession);
				}
			}
		}
	}
}
