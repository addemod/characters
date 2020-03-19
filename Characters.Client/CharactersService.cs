using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using Addemod.Characters.Client.Overlays;
using Addemod.Characters.Shared;
using Addemod.Characters.Client.Models;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using NFive.SDK.Client.Input;
using NFive.SDK.Core.Input;
using NFive.SDK.Core.Utilities;
using CitizenFX.Core.Native;
using Addemod.Characters.Client.Events;
using NFive.SDK.Client.Extensions;
using CitizenFX.Core.UI;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Models;

namespace Addemod.Characters.Client
{
	[PublicAPI]
	public class CharactersService : Service {
		private CharactersConfiguration config;
		private CharactersOverlay overlay;

		private bool started = false;
		private bool isCurrentlyPlaying = false;
		private CharacterSession characterSession;
		private Character currentCharacter;
		private Hotkey showCharacterSelectionHotkey;

		public CharactersService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started() {
			// Request server configuration
			this.config = await this.Comms.Event(CharacterEvents.Configuration).ToServer().Request<CharactersConfiguration>();

			// Get hotkey to open character selection screen
			this.showCharacterSelectionHotkey = new Hotkey(this.config.CharacterSelection.ShowCharactersHotkey);
		}

		public override async Task HoldFocus() {
			// Hide HUD
			Screen.Hud.IsVisible = false;

			// Disable the loading screen from automatically being dismissed
			API.SetManualShutdownLoadingScreenNui(true);

			// Freeze
			Game.Player.Freeze();

			// Switch out the player if it isn't already in a switch state
			if (!API.IsPlayerSwitchInProgress()) API.SwitchOutPlayer(API.PlayerPedId(), 0, 1);

			// Remove most clouds
			API.SetCloudHatOpacity(0.01f);

			// Wait for switch
			while (API.GetPlayerSwitchState() != 5) await Delay(10);

			// Hide loading screen
			API.ShutdownLoadingScreen();

			// Fade out
			Screen.Fading.FadeOut(0);
			while (Screen.Fading.IsFadingOut) await Delay(10);

			// Position character, required for switching
			Game.Player.Character.Position = CitizenFX.Core.Vector3.Zero;

			// Get characters
			var characters = await this.Comms.Event(CharacterEvents.GetAllForUser).ToServer().Request<List<Character>>();

			// Show overlay
			this.overlay = new CharactersOverlay(characters, this.OverlayManager);
			this.overlay.CreateCharacterEvent += OnCreateCharacter;
			this.overlay.DisconnectEvent += OnDisconnect;
			this.overlay.SelectCharacterEvent += OnSelectCharacter;
			this.overlay.DeleteCharacterEvent += OnDeleteCharacter;

			// Focus overlay
			this.overlay.Focus(true);

			// Shut down the NUI loading screen
			API.ShutdownLoadingScreenNui();

			// Fade in
			Screen.Fading.FadeIn(500);
			while (Screen.Fading.IsFadingIn) await Delay(10);

			// Wait for user before releasing focus
			while (!this.started) await Delay(20);
		}

		public async Task OnHotkey() {
			if (!this.showCharacterSelectionHotkey.IsJustPressed()) return;

			await ShowCharacterSelectionScreen();

			this.Ticks.Off(OnHotkey);
		}

		private async Task ShowCharacterSelectionScreen() {
			// Set as not playing
			this.isCurrentlyPlaying = false;

			// Hide HUD
			Screen.Hud.IsVisible = false;

			// Remove most clouds
			API.SetCloudHatOpacity(0.01f);

			// Switch out the player if it isn't already in a switch state
			if (!API.IsPlayerSwitchInProgress()) API.SwitchOutPlayer(API.PlayerPedId(), 0, 1);

			// Wait for switch
			while (API.GetPlayerSwitchState() != 5) await Delay(10);

			// Freeze
			Game.Player.Freeze();

			// Fade out
			Screen.Fading.FadeOut(1000);
			while (Screen.Fading.IsFadingOut) await Delay(10);

			// Position character, required for switching
			Game.Player.Character.Position = CitizenFX.Core.Vector3.Zero;

			// Get characters
			var characters = await this.Comms.Event(CharacterEvents.GetAllForUser).ToServer().Request<List<Character>>();

			// Show overlay
			this.overlay = new CharactersOverlay(characters, this.OverlayManager);
			this.overlay.CreateCharacterEvent += OnCreateCharacter;
			this.overlay.DisconnectEvent += OnDisconnect;
			this.overlay.SelectCharacterEvent += OnSelectCharacter;
			this.overlay.DeleteCharacterEvent += OnDeleteCharacter;

			// Focus overlay
			this.overlay.Focus(true);

			// Fade in
			Screen.Fading.FadeIn(500);
			while (Screen.Fading.IsFadingIn) await Delay(10);
		}

		private async void OnCreateCharacter(object sender, CreateCharacterOverlayEventArgs e) {
			// Set default walking style and model
			e.Character.WalkingStyle = "move_m@drunk@verydrunk";
			e.Character.Model = e.Character.Gender == 0 ? "mp_m_freemode_01" : "mp_f_freemode_01";
			/*e.Character.Apparel = Character.GetApparelFromPed(Game.PlayerPed);
			e.Character.Appearance = Character.GetAppearanceFromPed(Game.PlayerPed);
			e.Character.FaceShape = Character.GetFaceShapeFromPed(Game.PlayerPed);
			e.Character.Heritage = Character.GetHeritageFromPed(Game.PlayerPed);*/

			// Send new character to server, retrieve the created character
			var character = await this.Comms.Event(CharacterEvents.Create).ToServer().Request<Character>(e.Character);

			// Create and store the character session
			this.characterSession = await this.Comms.Event(CharacterEvents.Select).ToServer().Request<CharacterSession>(character.Id);
			await PlaySelectedCharacter(e.Overlay, character);
		}

		private void OnDisconnect(object sender, OverlayEventArgs e) {
			this.Comms.Event(SessionEvents.DisconnectPlayer).ToServer().Emit("Thanks for playing");
		}

		private async void OnSelectCharacter(object sender, CharacterIdOverlayEventArgs e) {
			this.characterSession = await this.Comms.Event(CharacterEvents.Select).ToServer().Request<CharacterSession>(e.CharacterId);
			await PlaySelectedCharacter(e.Overlay, this.characterSession.Character);
		}

		private async void OnDeleteCharacter(object sender, CharacterIdOverlayEventArgs e) {
			this.Logger.Debug($"Delete character with ID {e.CharacterId}");

			this.overlay.Characters = await this.Comms.Event(CharacterEvents.Delete).ToServer().Request<List<Character>>(e.CharacterId);

			this.overlay.SyncCharacters();
		}

		private async Task PlaySelectedCharacter(Overlay overlay, Character character) {
			// Destroy overlay
			overlay.Dispose();

			// Un-focus overlay
			this.overlay.Blur();
			// Load and render character model
			var model = new Model(character.Model);

			//API.RequestModel((uint)model.Hash);
			//while (!API.HasModelLoaded((uint)model.Hash)) await Delay(10);
			var retries = 0;
			while (!await Game.Player.ChangeModel(model)) {
				if (retries > 3) {
					Logger.Error(new Exception("Could not change player model to " + character.Model));
					await ShowCharacterSelectionScreen();
					return;
				}

				await Delay(10);
				retries++;
			}
			character.RenderCustom(this.Logger);
			await character.Render(this.Logger);

			// Unfreeze
			Game.Player.Unfreeze();

			// Show HUD
			Screen.Hud.IsVisible = true;

			// Switch in
			API.SwitchInPlayer(API.PlayerPedId());

			// Set character as active character
			this.currentCharacter = character;

			// Set as currently playing
			this.isCurrentlyPlaying = true;

			// Set player health (Rare #OnSpawnDeath Fix)
			this.currentCharacter.Health = character.Health;

			// Attach tick handlers after character selection
			// to reduce character select click lag
			this.Ticks.On(OnHotkey);
			this.Ticks.On(OnSaveCharacter);
			this.Ticks.On(OnSavePosition);

			// Release focus hold
			this.started = true;
		}
		private async Task OnSaveCharacter() {
			SaveCharacter();

			await Delay(this.config.Autosave.CharacterInterval);
		}

		private async Task OnSavePosition() {
			SavePosition();

			await Delay(this.config.Autosave.PositionInterval);
		}

		private Position GetCurrentPosition() {
			return Game.Player.Character.Position.ToVector3().ToPosition();
		}
		
		private async void OnMugshotHotkeyTick() {
			if (!Input.IsControlJustReleased(InputControl.ReplayClipDelete)) return; // delete
			var pedId = API.PlayerPedId();
			API.UnregisterPedheadshot(pedId);
			var registerPedHeadshotHandle = API.RegisterPedheadshotTransparent(pedId);
			while (!API.IsPedheadshotReady(registerPedHeadshotHandle) || !API.IsPedheadshotValid(registerPedHeadshotHandle)) await Delay(1000);
			string pedHeadshotTexture = API.GetPedheadshotTxdString(registerPedHeadshotHandle);
			Logger.Debug(pedHeadshotTexture);
		}

		/* API */
		public Character GetCurrentCharacter() {
			return this.currentCharacter;
		}

		public CharacterSession GetCharacterSession() {
			return this.characterSession;
		}

		public void SavePosition() {
			if (!this.isCurrentlyPlaying) return;

			this.Comms.Event(CharacterEvents.SavePosition).ToServer().Emit(this.currentCharacter.Id, GetCurrentPosition());
		}

		public void SaveCharacter() {
			if (!this.isCurrentlyPlaying) return;

			// Update the position as well
			this.currentCharacter.Position = GetCurrentPosition();
			this.Comms.Event(CharacterEvents.SaveCharacter).ToServer().Emit(this.currentCharacter);
		}

		/// <summary>
		/// Updates the character's appearance data with how current Ped's apperance
		/// </summary>
		public void SyncCharacterAppearance() {
			this.currentCharacter.Apparel = Character.GetApparelFromPed(Game.PlayerPed);
			this.currentCharacter.Appearance = Character.GetAppearanceFromPed(Game.PlayerPed);
			this.currentCharacter.FaceShape = Character.GetFaceShapeFromPed(Game.PlayerPed);
			this.currentCharacter.Heritage = Character.GetHeritageFromPed(Game.PlayerPed);
		}
	}
}
