using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Stadis.Intelligence.Data.Repositories.Interface;
using System.Threading.Tasks;
using Stadis.Intelligence.Data.Domian;
using Dapper;
using System.Linq;
using System.Data;

namespace Stadis.Intelligence.Data.Repositories.Class
{
	public  class CompanyDataSourceRepository : BaseRepository, ICompanyDataSourceRepository
	{
		public CompanyDataSourceRepository(IConfiguration configuration) : base(configuration) { }
		
        public async Task<List<CompanyDataSource>> GetCompnayDataSourceByCompanyId(int companyId)
        {
            try
            {
                CompanyDataSource companyDataSource = new CompanyDataSource();
                companyDataSource.IsDeleted = false;
                var query = "SELECT * FROM CompanyDataSource WHERE CompanyId = @CompanyId AND DataSourceTypeId = @DataSourceTypeId AND IsDeleted = @IsDeleted";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", companyId, DbType.Int32);
                parameters.Add("DataSourceTypeId", Convert.ToInt32(Stadis.Intelligence.Data.Enum.Enum.CompanyDataSourceType.SqlDatabase), DbType.Int32);
                parameters.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
                using (var connection = CreateConnection())
                {
                    return (await connection.QueryAsync<CompanyDataSource>(query, parameters)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


    }
}
