using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stadis.Intelligence.Data.Domian
{
	public class Dashboards:BaseEntity
	{

		public string DashboardId { get; set; }
		public string DashboardName { get; set; }
		public string DashboardPath { get; set; }
		public string DisplayDashboardName { get; set; }
		public bool IsActive { get; set; } = true;
		public int CompanyId { get; set; }
		public int? DataSourceId { get; set; }
		
		[NotMapped]
		public string CompanyName { get; set; }
		[NotMapped]
		public DateTimeOffset CreatedDate { get; set; }
		[NotMapped]
		public DateTimeOffset ModifieDate { get; set; }
		[NotMapped]
		public int DashboardPermission { get; set; }

		[NotMapped]
		public int SourceTypeId { get; set; }
		[NotMapped]
		public string FolderName { get; set; }
		[NotMapped]
		public int ParentFolderId { get; set; }
		[NotMapped]
		public int FolderId { get; set; }
	}
}
