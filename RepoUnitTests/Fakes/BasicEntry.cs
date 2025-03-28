using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepoUnitTests.Fakes
{
	[Table("Entries")]
	public class BasicEntry
	{
		[Key]
		[Column("BasicEntryId")]
		public Guid Id { get; set; }

		[ForeignKey("BasicClass")]
		public Guid BasicClassId { get; set; }

		public string? ValueToLoad { get; set; }



	}
}
