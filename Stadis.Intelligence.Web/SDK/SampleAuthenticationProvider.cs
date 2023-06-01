using Microsoft.Data.SqlClient;
using Reveal.Sdk;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stadis.Intelligence.Data.Domian;
using Dapper;
using System.Linq;
using System;

namespace Stadis.Intelligence.Web.SDK
{
	public class SampleAuthenticationProvider
	{
		//internal class SampleAuthenticationProvider : RVBaseAuthenticationProvider
		//{

		//	private readonly IConfiguration _configuration;
		//	public SampleAuthenticationProvider() { }
		//	public SampleAuthenticationProvider(IConfiguration IConfiguration) {
		//		_configuration = IConfiguration;
		//	}

		//	protected IDbConnection CreateConnection()
		//	{
		//		return new SqlConnection("Server=CS-REMOTE1; Database=StadisDashboardDB; Trusted_Connection=True; MultipleActiveResultSets=true");
		//	}
		//	protected override Task<IRVDataSourceCredential> ResolveCredentialsAsync(RVUserContext userContext, RVDashboardDataSource dataSource)
		//	{
		//		string strId = dataSource.Id.ToString();
		//		int id1 = 0;
		//		var id = Int32.TryParse(strId, out id1);
		//		CompanyDataSource companyDataSource = new CompanyDataSource();
		//		if (id)
		//		{
		//			companyDataSource.IsDeleted = false;
		//			var query = "SELECT * FROM CompanyDataSource WHERE Id = @Id AND IsDeleted = @IsDeleted";
		//			var parameters = new DynamicParameters();
		//			parameters.Add("Id", dataSource.Id, DbType.Int32);
		//			parameters.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
		//			using (var connection = CreateConnection())
		//			{
		//				companyDataSource = connection.Query<CompanyDataSource>(query, parameters).FirstOrDefault();

		//			}
		//		}
		//		else
		//		{
		//			companyDataSource.IsDeleted = false;
		//			var query = "SELECT * FROM CompanyDataSource WHERE CompanyId = @CompanyId AND IsDeleted = @IsDeleted";
		//			var parameters = new DynamicParameters();
		//			parameters.Add("CompanyId", 5, DbType.Int32);
		//			parameters.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
		//			using (var connection = CreateConnection())
		//			{
		//				companyDataSource = connection.Query<CompanyDataSource>(query, parameters).FirstOrDefault();
		//			}
		//		}
		//		IRVDataSourceCredential userCredential = null;
		//           if (dataSource is RVRESTDataSource)
		//           {
		//               //get the session sampleSessionHeader value
		//               string sessionHeader = (string)userContext.Properties.GetValueOrDefault("samplesessionheader");

		//               //pass a fixed cookie just for testing purposes and the sessionSesionHeader we stored in the SampleUserContextProvider
		//               string cookies = $"testCookie1=testValue";

		//               return Task.FromResult<IRVDataSourceCredential>(new RVHeadersDataSourceCredentials(
		//                   new Dictionary<string, string>()
		//                   {
		//                       { "userId", userContext.UserId },
		//                       { "cookie", cookies},
		//                       { "sampleSessionHeader", sessionHeader}
		//                   }));
		//           }
		//		if (dataSource is RVPostgresDataSource)
		//		{
		//			userCredential = new RVUsernamePasswordDataSourceCredential("postgresuser", "password");
		//		}
		//		else if (dataSource is RVSqlServerDataSource)
		//		{
		//			// The "domain" parameter is not always needed and this depends on your SQL Server configuration. 
		//			userCredential = new RVUsernamePasswordDataSourceCredential(companyDataSource.SQLUserID, companyDataSource.SQLPassword);
		//		}
		//		else if (dataSource is RVGoogleDriveDataSource)
		//		{
		//			userCredential = new RVBearerTokenDataSourceCredential("fhJhbUci0mJSUzi1nIiSint....", "user@company.com");
		//		}
		//		else if (dataSource is RVRESTDataSource)
		//		{
		//			userCredential = new RVUsernamePasswordDataSourceCredential(); // Anonymous
		//		}
		//		return Task.FromResult<IRVDataSourceCredential>(userCredential);
		//	}
		//}
	}
}
