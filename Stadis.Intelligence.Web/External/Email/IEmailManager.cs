using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stadis.Intelligence.Web.External.Email
{
	public interface IEmailManager
	{
		void Send(EmailMessage emailMessage);
		List<EmailMessage> ReceiveEmail(int maxCount = 10);
	}
}
