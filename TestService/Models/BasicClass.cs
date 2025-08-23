﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestService.Models
{
	[Table("Classes")]
	public class BasicClass
	{
		[Key]
		[Column("BasicClassId")]
		public Guid Id { get; set; }

		public string? TestField { get; set; }

		public int Refnr { get; set; }

		public ICollection<BasicEntry> BasicEntries { get; set; } = new List<BasicEntry>();

		public DateTime Oprettet { get; set; }

		public BasicClass()
		{
			Id = Guid.NewGuid();
			Oprettet = DateTime.Now;
		}
	}
}
