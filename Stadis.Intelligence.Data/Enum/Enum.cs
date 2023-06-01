using System;
using System.Collections.Generic;
using System.Text;

namespace Stadis.Intelligence.Data.Enum
{
	public class Enum
	{
		public enum UserType
		{
			IMSUser = 1,
			CompanyUser = 2,
			SiteUser = 3,
			SiteUserAdmin = 4
		}
		public enum SourceType
		{
			Folder = 1,
			Dashboard = 2
		}
		public enum PermissionType
		{			
			Modify = 1,
			View = 2,
			Owner = 3
		}


		public enum RoleType
		{
			IMSAdmin = 1,
			IMSViewer = 2,
			CompanyAdmin = 3,
			CompanyViewer = 4,
			SiteAdmin = 7,
			SiteEditor = 8,
			SiteViewer = 9
		}
		public enum CompanyDataSourceType
		{
			SqlDatabase = 1,
			ExcelCsv = 2,
			GoogleDrive = 3,
			PostgreSQL = 4		
		}

		public enum RoleTypeTypeEnum
		{
			IMSAdmin = 1,
			IMSViewer = 2,
			CompanyAdmin = 3,
			CompanyViewer = 4,
			SiteAdmin = 5,
			SiteEditor = 6,
			SiteViewer = 7
		}
	}
}
