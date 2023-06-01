using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stadis.Intelligence.Web.External.Logger
{
	public interface ILoggerManager
	{
		void LogInfo(string message);
		void LogWarn(string message);
		void LogDebug(string message);
		void LogError(string message);
	}
}
