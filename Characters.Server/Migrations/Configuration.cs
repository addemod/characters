using JetBrains.Annotations;
using NFive.SDK.Server.Migrations;
using Addemod.Characters.Server.Storage;

namespace Addemod.Characters.Server.Migrations
{
	[UsedImplicitly]
	public sealed class Configuration : MigrationConfiguration<StorageContext> {
		public Configuration() {
			AutomaticMigrationDataLossAllowed = false;
		}
	}
}
