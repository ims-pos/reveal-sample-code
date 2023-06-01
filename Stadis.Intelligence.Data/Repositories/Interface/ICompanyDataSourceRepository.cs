using Stadis.Intelligence.Data.Domian;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stadis.Intelligence.Data.Repositories.Interface
{
	public  interface ICompanyDataSourceRepository
	{
		
		Task<List<CompanyDataSource>> GetCompnayDataSourceByCompanyId(int companyId);
		
	}
}
