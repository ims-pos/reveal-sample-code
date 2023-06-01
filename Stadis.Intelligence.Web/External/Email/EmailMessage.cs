using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stadis.Intelligence.Web.External.Email
{
	public class EmailMessage
	{
        public EmailMessage() {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }
        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }

        [NotMapped]
        public int SessionTimeout { get; set; } = 60;

        [NotMapped]
        [StringLength(50)]
        public string SmtpServer { get; set; }

        [NotMapped]
        [StringLength(50)]
        public string SmtpUsername { get; set; }

        [NotMapped]
        [StringLength(100)]
        public string SmtpPassword { get; set; }

        [NotMapped]
        [StringLength(100)]
        public string FromDisplayName { get; set; }

        [NotMapped]
        public int SmtpPort { get; set; }
    }
}
