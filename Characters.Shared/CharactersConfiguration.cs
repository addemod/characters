using NFive.SDK.Core.Controllers;
using NFive.SDK.Core.Input;
using System;

namespace Addemod.Characters.Shared
{
	public class CharactersConfiguration : ControllerConfiguration {

		public int MaximumCharacters { get; set; } = -1;
		public AutosaveConfiguration Autosave { get; set; } = new AutosaveConfiguration();
		public CharacterSelectionConfiguration CharacterSelection { get; set; } = new CharacterSelectionConfiguration();

		public class AutosaveConfiguration {
			public TimeSpan CharacterInterval { get; set; } = TimeSpan.FromMinutes(5);
			public TimeSpan PositionInterval { get; set; } = TimeSpan.FromMinutes(2);
		}

		public class CharacterSelectionConfiguration {
			public InputControl ShowCharactersHotkey { get; set; } = InputControl.ReplayStartStopRecording; // Default = F1
		}
	}
}
