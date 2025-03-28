using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepoUnitTests.Fakes
{
	[Table("Classes")]
	public class BasicClass
	{
		[Key]
		[Column("BasicClassId")]
		public Guid Id { get; set; } = Guid.NewGuid();

		public string? TestField { get; set; }

		public int Refnr { get; set; }

		public ICollection<BasicEntry> BasicEntries { get; set; } = new List<BasicEntry>();

		public DateTime Oprettet { get; set; } = DateTime.Now;
	}
}
