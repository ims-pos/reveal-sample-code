using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stadis.Intelligence.Data.Domian;

namespace Stadis.Intelligence.Service.Interface
{
	public interface ICompanyDataSourceService
	{
		
		Task<List<CompanyDataSource>> GetAllCompnayDataSourceByCompanyId(int companyId);
		
	}
}
