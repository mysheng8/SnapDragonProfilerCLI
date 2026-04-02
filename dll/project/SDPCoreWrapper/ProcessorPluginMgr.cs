using System;
using System.Runtime.InteropServices;

// Token: 0x02000064 RID: 100
public class ProcessorPluginMgr : IDisposable
{
	// Token: 0x06000676 RID: 1654 RVA: 0x00010D03 File Offset: 0x0000EF03
	internal ProcessorPluginMgr(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00010D1F File Offset: 0x0000EF1F
	internal static HandleRef getCPtr(ProcessorPluginMgr obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00010D38 File Offset: 0x0000EF38
	~ProcessorPluginMgr()
	{
		this.Dispose();
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x00010D64 File Offset: 0x0000EF64
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessorPluginMgr(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00010DE4 File Offset: 0x0000EFE4
	public static ProcessorPluginMgr Get()
	{
		return new ProcessorPluginMgr(SDPCorePINVOKE.ProcessorPluginMgr_Get(), false);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00010DFE File Offset: 0x0000EFFE
	public void ShutDown()
	{
		SDPCorePINVOKE.ProcessorPluginMgr_ShutDown(this.swigCPtr);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00010E0C File Offset: 0x0000F00C
	public bool ImportCapture(int sourceCaptureID, int destinationCaptureID, string dbpath, int versionMajor, int versionMinor, int versionSubminor)
	{
		bool flag = SDPCorePINVOKE.ProcessorPluginMgr_ImportCapture(this.swigCPtr, sourceCaptureID, destinationCaptureID, dbpath, versionMajor, versionMinor, versionSubminor);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00010E3C File Offset: 0x0000F03C
	public void LoadPlugins(Client client, string path)
	{
		SDPCorePINVOKE.ProcessorPluginMgr_LoadPlugins__SWIG_0(this.swigCPtr, Client.getCPtr(client), path);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00010E5D File Offset: 0x0000F05D
	public void LoadPlugins(Client client)
	{
		SDPCorePINVOKE.ProcessorPluginMgr_LoadPlugins__SWIG_1(this.swigCPtr, Client.getCPtr(client));
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00010E70 File Offset: 0x0000F070
	public void UnloadPlugins(Client client)
	{
		SDPCorePINVOKE.ProcessorPluginMgr_UnloadPlugins(this.swigCPtr, Client.getCPtr(client));
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00010E84 File Offset: 0x0000F084
	public SDPProcessorPlugin GetPlugin(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProcessorPluginMgr_GetPlugin(this.swigCPtr, name);
		SDPProcessorPlugin sdpprocessorPlugin = ((intPtr == IntPtr.Zero) ? null : new SDPProcessorPlugin(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return sdpprocessorPlugin;
	}

	// Token: 0x04000150 RID: 336
	private HandleRef swigCPtr;

	// Token: 0x04000151 RID: 337
	protected bool swigCMemOwn;
}
