using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stadis.Intelligence.Web.External.Email
{
	public interface IEmailConfiguration
	{
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        string FromDisplayName { get; set; }
        string WebClientURL { get; set; }
        string WebAPIURL { get; set; }
        string PopServer { get; }
        int PopPort { get; }
        string PopUsername { get; }
        string PopPassword { get; }
    }
}
