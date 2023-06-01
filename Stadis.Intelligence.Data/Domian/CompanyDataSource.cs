using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stadis.Intelligence.Data.Domian
{
	public class CompanyDataSource : BaseEntity
	{
		[Required]
		[StringLength(50)]
		public string DataSourceName { get; set; }
		public int DataSourceId { get; set; }
		public int CompanyId { get; set; }
		public int DataSourceTypeId { get; set; }
		[StringLength(50)]
		public string SQLServerName { get; set; }
		[StringLength(50)]
		public string SQLDataBaseName { get; set; }
		[StringLength(50)]
		public string SQLUserID { get; set; }
		[StringLength(500)]
		public string SQLPassword { get; set; }
		public int SQLPort { get; set; }
	}
}
