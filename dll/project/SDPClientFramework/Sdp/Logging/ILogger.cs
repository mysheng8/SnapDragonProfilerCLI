using System;

namespace Sdp.Logging
{
	// Token: 0x02000301 RID: 769
	public interface ILogger
	{
		// Token: 0x06000FCD RID: 4045
		void LogDebug(string message);

		// Token: 0x06000FCE RID: 4046
		void LogInformation(string message);

		// Token: 0x06000FCF RID: 4047
		void LogWarning(string message);

		// Token: 0x06000FD0 RID: 4048
		void LogError(string message);

		// Token: 0x06000FD1 RID: 4049
		void LogException(Exception e);
	}
}
