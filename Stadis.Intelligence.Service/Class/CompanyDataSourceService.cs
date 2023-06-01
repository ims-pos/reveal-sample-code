using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stadis.Intelligence.Data.Domian;
using Stadis.Intelligence.Service.Interface;
using Stadis.Intelligence.Data.Repositories.Interface;

namespace Stadis.Intelligence.Service.Class
{
	public class CompanyDataSourceService : ICompanyDataSourceService
	{
		private readonly ICompanyDataSourceRepository _companyDataSourceRepository;

		public CompanyDataSourceService(ICompanyDataSourceRepository companyDataSourceRepository)
		{
			_companyDataSourceRepository = companyDataSourceRepository;
		}

		public async Task<List<CompanyDataSource>> GetAllCompnayDataSourceByCompanyId(int companyId)
		{
			return await _companyDataSourceRepository.GetCompnayDataSourceByCompanyId(companyId);
		}
		
	}
}
