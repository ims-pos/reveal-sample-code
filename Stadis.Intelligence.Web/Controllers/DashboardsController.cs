
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reveal.Sdk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stadis.Intelligence.Data.Domian;
using Stadis.Intelligence.Service;
using System.Reflection;
using System.Collections.ObjectModel;
using Stadis.Intelligence.Service.Interface;
using Microsoft.AspNetCore.Http;
using Stadis.Intelligence.Web.SDK;
using Microsoft.JSInterop;
using Stadis.Intelligence.Web.Models;
using System.Text;

namespace Stadis.Intelligence.Web.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {

        
        private readonly ICompanyDataSourceService _companyDataSourceService;

        public string[] _availableDashboards = null;
        public DashboardsController(ICompanyDataSourceService companyDataSourceService)
        {
            
            _companyDataSourceService = companyDataSourceService;
             
            var dashboardsPrefix = @"Dashboards\";
            _availableDashboards = Directory.GetFiles(dashboardsPrefix, "*.rdash");
            

        }


        
        [AllowAnonymous]
        public async Task<IActionResult> DashboardCreate()
        {
            var id = "1";
            List<Dashboards> dashboardList = new List<Dashboards>();
            
            var companyDataSource = await _companyDataSourceService.GetAllCompnayDataSourceByCompanyId(Convert.ToInt32(id));
            ViewData["CompanyDataSource"] = companyDataSource;
            
            return View();
        }



        }
}
