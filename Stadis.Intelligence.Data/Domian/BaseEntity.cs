using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stadis.Intelligence.Data
{
	public abstract class BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public int CreatedBy { get; set; }
		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset ModifiedOn { get; set; }
		public int ModifiedBy { get; set; }
	}
}
