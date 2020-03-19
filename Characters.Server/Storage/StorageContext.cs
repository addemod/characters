using System.Data.Entity;
using Addemod.Characters.Server.Models;
using NFive.SDK.Server.Storage;

namespace Addemod.Characters.Server.Storage
{
	public class StorageContext : EFContext<StorageContext> {
		public DbSet<Character> Characters { get; set; }

		public DbSet<Apparel> Appearances { get; set; }

		public DbSet<CharacterSession> CharacterSessions { get; set; }
	}
}
