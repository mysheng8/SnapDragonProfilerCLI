using System;
using System.Runtime.InteropServices;

// Token: 0x02000090 RID: 144
public class SDPProcessorPlugin : SDPPlugin
{
	// Token: 0x06001323 RID: 4899 RVA: 0x0001738E File Offset: 0x0001558E
	internal SDPProcessorPlugin(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.SDPProcessorPlugin_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x000173AA File Offset: 0x000155AA
	internal static HandleRef getCPtr(SDPProcessorPlugin obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x000173C4 File Offset: 0x000155C4
	~SDPProcessorPlugin()
	{
		this.Dispose();
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x000173F0 File Offset: 0x000155F0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SDPProcessorPlugin(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x00017474 File Offset: 0x00015674
	public virtual bool ImportCapture(uint sourceCaptureID, uint destinationCaptureID, string dbname, int versionMajor, int versionMinor, int versionSubminor)
	{
		bool flag = SDPCorePINVOKE.SDPProcessorPlugin_ImportCapture(this.swigCPtr, sourceCaptureID, destinationCaptureID, dbname, versionMajor, versionMinor, versionSubminor);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x000174A4 File Offset: 0x000156A4
	public virtual void ProcessData(uint bufferCategory, uint bufferID, uint captureID, IntPtr data, uint dataSize, Void_Double_Fn progressCallback)
	{
		SDPCorePINVOKE.SDPProcessorPlugin_ProcessData(this.swigCPtr, bufferCategory, bufferID, captureID, data, dataSize, progressCallback);
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x000174BC File Offset: 0x000156BC
	public virtual BinaryDataPair GetLocalBuffer(uint bufferCategory, uint bufferID, uint captureID)
	{
		IntPtr intPtr = SDPCorePINVOKE.SDPProcessorPlugin_GetLocalBuffer(this.swigCPtr, bufferCategory, bufferID, captureID);
		return (intPtr == IntPtr.Zero) ? null : new BinaryDataPair(intPtr, true);
	}

	// Token: 0x040001BB RID: 443
	private HandleRef swigCPtr;
}
