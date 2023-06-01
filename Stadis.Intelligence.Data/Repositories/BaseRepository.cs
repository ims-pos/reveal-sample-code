using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sql;
using System.Data;

namespace Stadis.Intelligence.Data.Repositories
{
	public abstract class BaseRepository
	{
		private readonly IConfiguration _configuration;

		protected BaseRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		protected IDbConnection CreateConnection()
		{
			return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
		}
	}
}
