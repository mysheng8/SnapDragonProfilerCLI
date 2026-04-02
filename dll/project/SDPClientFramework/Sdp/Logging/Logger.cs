using System;

namespace Sdp.Logging
{
	// Token: 0x02000302 RID: 770
	public class Logger : ILogger
	{
		// Token: 0x06000FD2 RID: 4050 RVA: 0x00030BF7 File Offset: 0x0002EDF7
		public Logger(string tag)
		{
			this.m_tag = tag;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00030C06 File Offset: 0x0002EE06
		public void LogDebug(string message)
		{
			Logger.Get().Write(LogLevel.LOG_DEBUG, this.m_tag, message);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00030C1A File Offset: 0x0002EE1A
		public void LogInformation(string message)
		{
			Logger.Get().Write(LogLevel.LOG_INFO, this.m_tag, message);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00030C2E File Offset: 0x0002EE2E
		public void LogWarning(string message)
		{
			Logger.Get().Write(LogLevel.LOG_WARN, this.m_tag, message);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00030C42 File Offset: 0x0002EE42
		public void LogError(string message)
		{
			Logger.Get().Write(LogLevel.LOG_ERROR, this.m_tag, message);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00030C58 File Offset: 0x0002EE58
		public void LogException(Exception e)
		{
			string text = e.Message + "\n" + e.StackTrace;
			Logger.Get().Write(LogLevel.LOG_ERROR, this.m_tag, text);
		}

		// Token: 0x04000ABE RID: 2750
		private readonly string m_tag;
	}
}
