using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestService.Models
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
