using Microsoft.AspNetCore.Http;
using Reveal.Sdk;

using System.Collections.Generic;
//using Stadis.Intelligence.Web.External.Logger;

namespace Stadis.Intelligence.Web.SDK
{
	public class SampleUserContextProvider: IRVUserContextProvider
	{
        public IRVUserContext GetUserContext(HttpContext aspnetContext)
        {
            string user = aspnetContext.Session.GetString("UserId");
            string compId = aspnetContext.Session.GetString("CompanyId");
            string folderId = aspnetContext.Session.GetString("FolderId");
            string userId = compId + ',' + user + ',' + folderId;
            return new RVUserContext(
               userId,
               new Dictionary<string, object>());
        }

        //protected override RVUserContext GetUserContext(HttpContext aspnetContext)
        //{
        //    string user = aspnetContext.Session.GetString("UserId");
        //    string compId = aspnetContext.Session.GetString("CompanyId");
        //    string folderId = aspnetContext.Session.GetString("FolderId");
        //    string userId = compId + ',' + user + ',' + folderId;
        //    return new RVUserContext(
        //       userId,
        //       new Dictionary<string, object>());
        //}

      /*  protected override RVUserContext IRVUserContextProvider.GetUserContext(HttpContext aspnetContext)
        {
            string user = aspnetContext.Session.GetString("UserId");
            string compId = aspnetContext.Session.GetString("CompanyId");
            string folderId = aspnetContext.Session.GetString("FolderId");
            string userId = compId + ',' + user + ',' + folderId;
            return new RVUserContext(
               userId,
               new Dictionary<string, object>());
        }*/
    }
}
