using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000034 RID: 52
public class Logger : IDisposable
{
	// Token: 0x06000305 RID: 773 RVA: 0x00008B03 File Offset: 0x00006D03
	internal Logger(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00008B1F File Offset: 0x00006D1F
	internal static HandleRef getCPtr(Logger obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00008B38 File Offset: 0x00006D38
	~Logger()
	{
		this.Dispose();
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00008B64 File Offset: 0x00006D64
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Logger(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00008BE4 File Offset: 0x00006DE4
	public static Logger Get()
	{
		return new Logger(SDPCorePINVOKE.Logger_Get(), false);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00008BFE File Offset: 0x00006DFE
	public Logger()
		: this(SDPCorePINVOKE.new_Logger(), true)
	{
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00008C0C File Offset: 0x00006E0C
	public void SetDefaultTag(string tag)
	{
		SDPCorePINVOKE.Logger_SetDefaultTag(this.swigCPtr, tag);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00008C28 File Offset: 0x00006E28
	public string GetDefaultTag()
	{
		return SDPCorePINVOKE.Logger_GetDefaultTag(this.swigCPtr);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00008C42 File Offset: 0x00006E42
	public void AddSink(Logger.LogSink sink)
	{
		SDPCorePINVOKE.Logger_AddSink(this.swigCPtr, Logger.LogSink.getCPtr(sink));
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00008C55 File Offset: 0x00006E55
	public void RemoveSink(Logger.LogSink sink)
	{
		SDPCorePINVOKE.Logger_RemoveSink(this.swigCPtr, Logger.LogSink.getCPtr(sink));
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00008C68 File Offset: 0x00006E68
	public void RemoveAllSinks()
	{
		SDPCorePINVOKE.Logger_RemoveAllSinks(this.swigCPtr);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00008C78 File Offset: 0x00006E78
	public SWIGTYPE_p_std__vectorT_std__shared_ptrT_CoreUtils__Logger__LogSink_t_t GetSinks()
	{
		return new SWIGTYPE_p_std__vectorT_std__shared_ptrT_CoreUtils__Logger__LogSink_t_t(SDPCorePINVOKE.Logger_GetSinks(this.swigCPtr), true);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00008C98 File Offset: 0x00006E98
	public void Write(LogLevel msgLevel, string tag, string msg)
	{
		SDPCorePINVOKE.Logger_Write__SWIG_0(this.swigCPtr, (int)msgLevel, tag, msg);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00008CA8 File Offset: 0x00006EA8
	public void WriteFormatted(LogLevel msgLevel, string tag, string format)
	{
		SDPCorePINVOKE.Logger_WriteFormatted(this.swigCPtr, (int)msgLevel, tag, format);
	}

	// Token: 0x040000A4 RID: 164
	private HandleRef swigCPtr;

	// Token: 0x040000A5 RID: 165
	protected bool swigCMemOwn;

	// Token: 0x020000D9 RID: 217
	public class LogSink : IDisposable
	{
		// Token: 0x060014B5 RID: 5301 RVA: 0x0001986E File Offset: 0x00017A6E
		internal LogSink(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwnBase = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0001988A File Offset: 0x00017A8A
		internal static HandleRef getCPtr(Logger.LogSink obj)
		{
			if (obj != null)
			{
				return obj.swigCPtr;
			}
			return new HandleRef(null, IntPtr.Zero);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x000198A4 File Offset: 0x00017AA4
		~LogSink()
		{
			this.Dispose();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x000198D0 File Offset: 0x00017AD0
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwnBase)
					{
						this.swigCMemOwnBase = false;
						SDPCorePINVOKE.delete_Logger_LogSink(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00019950 File Offset: 0x00017B50
		public LogSink(LogLevel level)
			: this(SDPCorePINVOKE.new_Logger_LogSink((int)level), true)
		{
			this.SwigDirectorConnect();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00019968 File Offset: 0x00017B68
		public virtual string GetName()
		{
			string text = SDPCorePINVOKE.Logger_LogSink_GetName(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0001998F File Offset: 0x00017B8F
		public virtual void Init()
		{
			if (this.SwigDerivedClassHasMethod("Init", Logger.LogSink.swigMethodTypes1))
			{
				SDPCorePINVOKE.Logger_LogSink_InitSwigExplicitLogSink(this.swigCPtr);
			}
			else
			{
				SDPCorePINVOKE.Logger_LogSink_Init(this.swigCPtr);
			}
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000199C8 File Offset: 0x00017BC8
		public virtual void Write(LogLevel level, string tag, string output)
		{
			SDPCorePINVOKE.Logger_LogSink_Write(this.swigCPtr, (int)level, tag, output);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x000199E5 File Offset: 0x00017BE5
		public void SetLevel(LogLevel level)
		{
			SDPCorePINVOKE.Logger_LogSink_SetLevel(this.swigCPtr, (int)level);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00019A00 File Offset: 0x00017C00
		public LogLevel GetLevel()
		{
			LogLevel logLevel = (LogLevel)SDPCorePINVOKE.Logger_LogSink_GetLevel(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return logLevel;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00019A28 File Offset: 0x00017C28
		public bool CanLog(LogLevel msgLevel)
		{
			bool flag = SDPCorePINVOKE.Logger_LogSink_CanLog(this.swigCPtr, (int)msgLevel);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return flag;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00019A50 File Offset: 0x00017C50
		private void SwigDirectorConnect()
		{
			if (this.SwigDerivedClassHasMethod("GetName", Logger.LogSink.swigMethodTypes0))
			{
				this.swigDelegate0 = new Logger.LogSink.SwigDelegateLogSink_0(this.SwigDirectorGetName);
			}
			if (this.SwigDerivedClassHasMethod("Init", Logger.LogSink.swigMethodTypes1))
			{
				this.swigDelegate1 = new Logger.LogSink.SwigDelegateLogSink_1(this.SwigDirectorInit);
			}
			if (this.SwigDerivedClassHasMethod("Write", Logger.LogSink.swigMethodTypes2))
			{
				this.swigDelegate2 = new Logger.LogSink.SwigDelegateLogSink_2(this.SwigDirectorWrite);
			}
			SDPCorePINVOKE.Logger_LogSink_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00019AE8 File Offset: 0x00017CE8
		private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
		{
			MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
			return method.DeclaringType.IsSubclassOf(typeof(Logger.LogSink));
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00019B1E File Offset: 0x00017D1E
		private string SwigDirectorGetName()
		{
			return this.GetName();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00019B26 File Offset: 0x00017D26
		private void SwigDirectorInit()
		{
			this.Init();
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00019B2E File Offset: 0x00017D2E
		private void SwigDirectorWrite(int level, string tag, string output)
		{
			this.Write((LogLevel)level, tag, output);
		}

		// Token: 0x040001F7 RID: 503
		private HandleRef swigCPtr;

		// Token: 0x040001F8 RID: 504
		private bool swigCMemOwnBase;

		// Token: 0x040001F9 RID: 505
		private Logger.LogSink.SwigDelegateLogSink_0 swigDelegate0;

		// Token: 0x040001FA RID: 506
		private Logger.LogSink.SwigDelegateLogSink_1 swigDelegate1;

		// Token: 0x040001FB RID: 507
		private Logger.LogSink.SwigDelegateLogSink_2 swigDelegate2;

		// Token: 0x040001FC RID: 508
		private static Type[] swigMethodTypes0 = new Type[0];

		// Token: 0x040001FD RID: 509
		private static Type[] swigMethodTypes1 = new Type[0];

		// Token: 0x040001FE RID: 510
		private static Type[] swigMethodTypes2 = new Type[]
		{
			typeof(LogLevel),
			typeof(string),
			typeof(string)
		};

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x06001562 RID: 5474
		public delegate string SwigDelegateLogSink_0();

		// Token: 0x020000F9 RID: 249
		// (Invoke) Token: 0x06001566 RID: 5478
		public delegate void SwigDelegateLogSink_1();

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x0600156A RID: 5482
		public delegate void SwigDelegateLogSink_2(int level, string tag, string output);
	}
}
