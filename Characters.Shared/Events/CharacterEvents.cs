namespace Addemod.Characters.Shared
{
	public static class CharacterEvents {
		public const string Configuration = "addemod:character:configuration";
		public const string Disconnecting = "addemod:character:disconnecting";
		public const string Create = "addemod:character:create";
		public const string Delete = "addemod:character:delete";
		public const string GetAllForUser = "addemod:character:getallforuser";
		public const string SaveCharacter = "addemod:character:savecharacter";
		public const string SavePosition = "addemod:character:saveposition";
		public const string Select = "addemod:character:select";
		public const string Deselect = "addemod:character:deselect";
		public const string DeselectAll = "addemod:character:deselectall";
		public const string GetActiveSessions = "addemod:character:getactivesessions";

		public const string Selecting = "addemod:character:selecting";
		public const string Deselecting = "addemod:character:deselecting";
		public const string DeselectingAll = "addemod:character:deselectingall";

		public const string Selected = "addemod:character:selected";
		public const string Deselected = "addemod:character:deselected";
		public const string DeselectedAll = "addemod:character:deselectedall";

		/// <summary>
		/// Called when the player has spawned and is ready
		/// </summary>
		public const string Spawned = "addemod:character:spawned";

		public const string GetById = "addemod:character:getbyid";
	}
}
