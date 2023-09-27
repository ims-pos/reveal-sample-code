using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Reveal.Sdk;
using Reveal.Sdk.Data;
using Reveal.Sdk.Data.Microsoft.SqlServer;
using Reveal.Sdk.Data.PostgreSQL;
using Reveal.Sdk.Data.Google.Drive;
using Reveal.Sdk.Data.Rest;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Stadis.Intelligence.Data.Domian;
using Stadis.Intelligence.Web.External.Logger;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using NLog;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Stadis.Intelligence.Web.Models;
using Stadis.Intelligence.Service.Interface;
using Stadis.Intelligence.Data.Repositories.Interface;

namespace Stadis.Intelligence.Web.SDK
{

    public class SampleDataSourceProvider : IRVDataSourceProvider
    {
        private readonly IConfiguration _configuration;
        public SampleDataSourceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<RVDataSourceItem> ChangeDataSourceItemAsync(IRVUserContext userContext, string dashboardId, RVDataSourceItem dataSourceItem)
        {
            return Task.FromResult<RVDataSourceItem>(null);
        }

        public Task<RVDashboardDataSource> ChangeDataSourceAsync(IRVUserContext userContext, RVDashboardDataSource dataSourceItem)
        {
            return Task.FromResult<RVDashboardDataSource>(null);
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<RVDataSourceItem> ChangeVisualizationDataSourceItemAsync(string userId, string dashboardId, RVVisualization visualization, RVDataSourceItem dataSourceItem)
        {
            string[] idValues = userId.Split(',');              ///Differentiate CompanyId and UserId
            userId = idValues[0];
            Dashboards companyDashboard = new Dashboards();
            if (dashboardId != "")
            {
                companyDashboard.IsDeleted = false;
                var query = "SELECT * FROM Dashboard WHERE DashboardName = @DashboardName AND IsDeleted = @IsDeleted";
                var parameters = new DynamicParameters();
                parameters.Add("DashboardName", dashboardId, DbType.String);
                parameters.Add("IsDeleted", companyDashboard.IsDeleted, DbType.String);
                using (var connection = CreateConnection())
                {
                    companyDashboard = connection.Query<Dashboards>(query, parameters).FirstOrDefault();
                }
                CompanyDataSource companyDataSource = new CompanyDataSource();
                if (companyDashboard.DataSourceId != null)
                {
                    companyDataSource.IsDeleted = false;
                    var query1 = "SELECT * FROM CompanyDataSource WHERE Id = @Id AND IsDeleted = @IsDeleted";
                    var parameters1 = new DynamicParameters();
                    parameters1.Add("Id", companyDashboard.DataSourceId, DbType.Int32);
                    parameters1.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
                    using (var connection = CreateConnection())
                    {
                        companyDataSource = connection.Query<CompanyDataSource>(query1, parameters1).FirstOrDefault();
                    }

                    var sqlServerDsi = dataSourceItem as RVSqlServerDataSourceItem;
                    if (sqlServerDsi != null)
                    {
                        sqlServerDsi.Database = companyDataSource.SQLDataBaseName;
                        sqlServerDsi.Id = companyDataSource.SQLDataBaseName + "." + sqlServerDsi.Schema + "." + sqlServerDsi.Title;
                        // Change SQL Server host and database  
                        var sqlServerDS = (RVSqlServerDataSource)sqlServerDsi.DataSource;
                        sqlServerDS.Host = companyDataSource.SQLServerName;
                        sqlServerDS.Database = companyDataSource.SQLDataBaseName;
                        sqlServerDS.Id = (companyDataSource.Id).ToString();
                        sqlServerDS.Port = companyDataSource.SQLPort;
                        // Change SQL Server table/view
                        //sqlServerDsi.Table = visualization.Title;
                        return Task.FromResult((RVDataSourceItem)sqlServerDsi);
                    }
                }
            }
            return Task.FromResult(dataSourceItem);
        }
    }

    public class EmbedAuthenticationProvider : IRVAuthenticationProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public EmbedAuthenticationProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IRVDataSourceCredential> ResolveCredentialsAsync(IRVUserContext userContext, RVDashboardDataSource dataSource)
        {
            // string[] idValues = userId.Split(',');               ///Differentiate CompanyId and UserId
            string companyId = _httpContextAccessor.HttpContext.Session.GetString("CompanyId");
            string strId = "1";
            int id1 = 0;
            var id = true;
            CompanyDataSource companyDataSource = new CompanyDataSource();
            if (id)
            {
                companyDataSource.IsDeleted = false;
                var query = "SELECT * FROM CompanyDataSource WHERE Id = @Id AND IsDeleted = @IsDeleted";
                var parameters = new DynamicParameters();
                parameters.Add("Id", 5, DbType.Int32);
                parameters.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
                using (var connection = CreateConnection())
                {
                    companyDataSource = connection.Query<CompanyDataSource>(query, parameters).FirstOrDefault();
                }
            }
            else
            {
                companyDataSource.IsDeleted = false;
                var query = "SELECT * FROM CompanyDataSource WHERE CompanyId = @CompanyId AND IsDeleted = @IsDeleted";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", Convert.ToInt32(companyId), DbType.Int32);
                parameters.Add("IsDeleted", companyDataSource.IsDeleted, DbType.String);
                using (var connection = CreateConnection())
                {
                    companyDataSource = connection.Query<CompanyDataSource>(query, parameters).FirstOrDefault();
                }
            }
            //var connectionString = _httpContextAccessor.HttpContext.Session.GetString("ConnDetails");
            IRVDataSourceCredential userCredential = null;


                // The "domain" parameter is not always needed and this depends on your SQL Server configuration. 
                userCredential = new RVUsernamePasswordDataSourceCredential(companyDataSource.SQLUserID, companyDataSource.SQLPassword);
            

            return Task.FromResult<IRVDataSourceCredential>(userCredential);
        }
    }

    public class MyDashboardProvider : IRVDashboardProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public MyDashboardProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<Dashboard> GetDashboardAsync(IRVUserContext userContext, string dashboardId)
        {
            var dashboardFileName = dashboardId + ".rdash";
            var resourceName = @"Dashboards\" + dashboardFileName;
            using (var fileStream = new FileStream(resourceName, FileMode.Open))
            {
                var dash = new Dashboard(fileStream);
                return Task.FromResult(dash);
            }
        }
        protected IDbConnection CreateConnection()
        {
            Logger.Info(_configuration.GetConnectionString("DefaultConnection"));
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        public Task SaveDashboardAsync(IRVUserContext userContext, string dashboardId, Dashboard dashboard)
        {
            try
            {

                //string[] idValues = userContext.UserId.Split(',');
                int roleId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("Role"));
                string companyId = _httpContextAccessor.HttpContext.Session.GetString("CompanyId");
                int loginUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));         ///Differentiate CompanyId and UserId and FolderId
                int folderId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("FolderId"));         ///Differentiate CompanyId and UserId and FolderId
                ////////////Get DashboardName on Edit time///////////
                //  var jsonDashboard = dashboard.SerializeAsJsonAsync();
                var jsonDashboard = dashboard.ToJsonStringAsync();

                var data1 = JObject.Parse(jsonDashboard.Result);
                DashboardResult dashboardResult = System.Text.Json.JsonSerializer.Deserialize<DashboardResult>(jsonDashboard.Result.ToString());
                var newDasbaordName = data1;
                ////////////Get DashboardName on Edit time///////////

                var random = new Random();
                var randomId = random.Next(1000, 9999);
                bool newCreateFile = false;
                var newpath = @"Dashboards/";
                var existingFile = Path.Combine(newpath, dashboardId + ".rdash");
                var fileName = "";
                var pathMerg = "";
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                var exits = !File.Exists(existingFile);
                //var dashData = dashboard.SerializeAsync();
                var dashData = dashboard.ToStreamAsync();

                if (exits)
                {
                    /* if (dashboardResult.Title == null)
                     {*/
                    Dashboards dashboardData = new Dashboards();
                    try
                    {
                        var query = "SELECT * FROM Dashboard WHERE DashboardName = @DashboardName OR DisplayDashboardName = @DisplayDashboardName AND CompanyId = @CompanyId AND IsDeleted = @IsDeleted";
                        dashboardData.IsDeleted = false;
                        var parameters = new DynamicParameters();
                        parameters.Add("DashboardName", dashboardId, DbType.String);
                        parameters.Add("DisplayDashboardName", dashboardId, DbType.String);
                        parameters.Add("CompanyId", companyId, DbType.String);
                        parameters.Add("IsDeleted", dashboardData.IsDeleted, DbType.String);

                        using (var connection = CreateConnection())
                        {
                            var resultData = connection.QueryFirstOrDefaultAsync<Dashboards>(query, parameters);
                            dashboardData = resultData.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }

                    if (dashboardData == null)
                    {
                        //For New Record
                        newCreateFile = true;
                        fileName = dashboardId + "-" + companyId + "-" + randomId + ".rdash";
                        pathMerg = Path.Combine(newpath, fileName);

                        byte[] bytes = new byte[dashData.Result.Length];
                        MemoryStream myStream = new MemoryStream(bytes);
                        using (FileStream fileStream = new FileStream(pathMerg, FileMode.Create))
                        {
                            dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                            myStream.Seek(0, SeekOrigin.Begin);
                            myStream.WriteTo(fileStream);
                            fileStream.Close();
                            myStream.Close();
                        }

                        /*using (FileStream fileStream = new FileStream(pathMerg, FileMode.Create))
                        {
                            byte[] bytes = new byte[dashData.Result.Length];
                            dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                            dashData.Result.Close();
                        }*/
                    }
                    else
                    {
                        // Keep editing working as it is 
                        fileName = dashboardData.DashboardName + ".rdash";
                        pathMerg = Path.Combine(newpath, fileName);

                        byte[] bytes = new byte[dashData.Result.Length];
                        MemoryStream myStream = new MemoryStream(bytes);
                        using (FileStream fileStream = new FileStream(pathMerg, FileMode.Create))
                        {
                            dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                            myStream.Seek(0, SeekOrigin.Begin);
                            myStream.WriteTo(fileStream);
                            fileStream.Close();
                            myStream.Close();
                        }
                        /*using (FileStream fileStream = new FileStream(pathMerg, FileMode.Open))
                        {
                            byte[] bytes = new byte[dashData.Result.Length];
                            dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                            dashData.Result.Close();
                        }*/
                    }
                    /* }*/
                }
                else
                {
                    // Keep editing working as it is 
                    fileName = dashboardId + ".rdash";
                    pathMerg = Path.Combine(newpath, fileName);

                    byte[] bytes = new byte[dashData.Result.Length];
                    MemoryStream myStream = new MemoryStream(bytes);
                    using (FileStream fileStream = new FileStream(pathMerg, FileMode.Create))
                    {
                        dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                        myStream.Seek(0, SeekOrigin.Begin);
                        myStream.WriteTo(fileStream);
                        fileStream.Close();
                        myStream.Close();
                    }
                    /*using (FileStream fileStream = new FileStream(pathMerg, FileMode.Open))
                    {
                        byte[] bytes = new byte[dashData.Result.Length];
                        dashData.Result.Read(bytes, 0, (int)dashData.Result.Length);
                        fileStream.Write(bytes, 0, bytes.Length);
                        dashData.Result.Close();
                    }*/
                }
                Logger.Info("File created");
                //Save in DB...

                Dashboards dashboards = new Dashboards();
                // int companyId = Convert.ToInt32(companyId);
                dashboards.CreatedBy = Convert.ToInt32(companyId);
                dashboards.CreatedOn = DateTime.UtcNow;
                dashboards.ModifiedBy = Convert.ToInt32(companyId);
                dashboards.ModifiedOn = DateTime.UtcNow;
                dashboards.CompanyId = Convert.ToInt32(companyId);
                dashboards.DashboardId = randomId.ToString();
                dashboards.DashboardName = dashboardId + "-" + companyId + "-" + randomId;
                dashboards.DisplayDashboardName = dashboardId;
                dashboards.DashboardPath = pathMerg;
                dashboards.IsActive = true;

                string details;         ///For Edit or Create Dashboard
                string actionType;         ///For Edit or Create Dashboard
                int currentDashboardId;
                string dashboardName;

                if (newCreateFile)
                {
                    details = "Viewed Dashboard Created Dashboard Name : " + dashboards.DashboardName;
                    actionType = "AddDashboard";
                    currentDashboardId = Convert.ToInt32(dashboards.DashboardId);
                    dashboardName = dashboards.DashboardName;
                    Logger.Info("Step one complete");
                    try
                    {
                        var query = "INSERT INTO Dashboard(IsDeleted,CreatedBy,CreatedOn,ModifiedOn,DashboardId,DisplayDashboardName,DashboardName,DashboardPath,IsActive,CompanyId) VALUES (@IsDeleted,@CreatedBy,@CreatedOn,@ModifiedOn,@DashboardId,@DisplayDashboardName,@DashboardName,@DashboardPath,@IsActive,@CompanyId)";
                        dashboards.IsDeleted = false;
                        var parameters = new DynamicParameters();
                        parameters.Add("CreatedBy", 1, DbType.Int32);
                        parameters.Add("IsDeleted", dashboards.IsDeleted, DbType.String);
                        parameters.Add("CreatedOn", DateTime.UtcNow, DbType.DateTimeOffset);
                        parameters.Add("ModifiedOn", DateTime.UtcNow, DbType.DateTimeOffset);
                        parameters.Add("DashboardId", dashboards.DashboardId, DbType.String);
                        parameters.Add("DisplayDashboardName", dashboards.DisplayDashboardName, DbType.String);
                        parameters.Add("DashboardName", dashboards.DashboardName, DbType.String);
                        parameters.Add("DashboardPath", dashboards.DashboardPath, DbType.String);
                        parameters.Add("IsActive", dashboards.IsActive, DbType.String);
                        parameters.Add("CompanyId", dashboards.CompanyId, DbType.String);

                        //var query2 = "INSERT INTO Dashboard(IsDeleted,CreatedBy,CreatedOn,DashboardId,DashboardName,DashboardPath,IsActive,CompanyId) VALUES ('False',1,'2021-08-25 06:40:40.0000000 +05:30','5645','ABC','Dashboards/ABC.rdash','True',1)";
                        using (var connection = CreateConnection())
                        {
                            var resultData = connection.ExecuteAsync(query, parameters).Wait(100000);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }

                    
                    

                                    }
                else
                {
                    var getDashboardId = dashboardId.Split("-").ToList();
                    details = "Viewed Dashboard Updated Dashboard Name : " + dashboardId;
                    actionType = "Edit Dashboard";
                    if (getDashboardId.Count() > 0)
                    {
                        currentDashboardId = Convert.ToInt32(getDashboardId.Last());
                    }
                    else
                    {
                        currentDashboardId = 0;
                    }
                    dashboardName = dashboardId;
                    Logger.Info("Step one complete");
                    try
                    {
                        string query;
                        if (dashboardResult.Title != null)
                        {
                            query = "UPDATE Dashboard SET ModifiedOn = @ModifiedOn,ModifiedBy = @ModifiedBy,DisplayDashboardName = @DisplayDashboardName WHERE DashboardName = @DashboardName";
                        }
                        else
                        {
                            query = "UPDATE Dashboard SET ModifiedOn = @ModifiedOn,ModifiedBy = @ModifiedBy WHERE DashboardName = @DashboardName";
                        }

                        dashboards.IsDeleted = false;
                        var parameters = new DynamicParameters();
                        parameters.Add("ModifiedBy", 1, DbType.Int32);
                        parameters.Add("ModifiedOn", DateTime.UtcNow, DbType.DateTimeOffset);
                        if (dashboardResult.Title != null)
                        {
                            parameters.Add("DisplayDashboardName", dashboardResult.Title, DbType.String);
                        }
                        parameters.Add("DashboardName", dashboardId, DbType.String);

                        using (var connection = CreateConnection())
                        {
                            var resultData = connection.ExecuteScalarAsync(query, parameters).Wait(100000);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }

                /////////////////// Add Entry on Dashboard Version Table ///////////////
                var oldFileName = fileName.Replace(".rdash", "");
                int count = 0;

                try
                {
                    var dashboardVersionCount = "SELECT count(*) FROM DashboardVersion WHERE DashboardId = @DashboardId AND IsDeleted = @IsDeleted";
                    var parameters = new DynamicParameters();
                    parameters.Add("DashboardId", oldFileName, DbType.String);
                    parameters.Add("IsDeleted", false, DbType.String);
                    using (var connection = CreateConnection())
                    {
                        count = connection.ExecuteScalar<int>(dashboardVersionCount, parameters);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

                int vesrionNumber = count + 1;
                var newFileName = oldFileName + "-" + vesrionNumber.ToString() + ".rdash";
                try
                {
                    /////////Create new File with Verion Number ///////////                 
                    var merg = Path.Combine(newpath, newFileName);
                    try
                    {
                        System.IO.File.Copy(pathMerg, merg);
                    }
                    catch (IOException copyError)
                    {
                        Console.WriteLine(copyError.Message);
                    }

                    var query = "INSERT INTO DashboardVersion(IsDeleted,CreatedBy,CreatedOn,ModifiedOn,DashboardId,VersionNumber) VALUES (@IsDeleted,@CreatedBy,@CreatedOn,@ModifiedOn,@DashboardId,@VersionNumber)";
                    dashboards.IsDeleted = false;
                    var parameters = new DynamicParameters();
                    parameters.Add("CreatedBy", 1, DbType.Int32);
                    parameters.Add("IsDeleted", dashboards.IsDeleted, DbType.String);
                    parameters.Add("CreatedOn", DateTime.UtcNow, DbType.DateTimeOffset);
                    parameters.Add("ModifiedOn", DateTime.UtcNow, DbType.DateTimeOffset);
                    parameters.Add("DashboardId", oldFileName, DbType.String);
                    parameters.Add("VersionNumber", vesrionNumber, DbType.String);

                    using (var connection = CreateConnection())
                    {
                        var resultData = connection.ExecuteAsync(query, parameters).Wait(100000);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

                

                return Task.FromResult(dashboard);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }

        
    }
}
