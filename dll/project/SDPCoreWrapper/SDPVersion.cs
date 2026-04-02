using System;
using System.Runtime.InteropServices;

// Token: 0x02000091 RID: 145
public class SDPVersion : IDisposable
{
	// Token: 0x0600132A RID: 4906 RVA: 0x000174F1 File Offset: 0x000156F1
	internal SDPVersion(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x0001750D File Offset: 0x0001570D
	internal static HandleRef getCPtr(SDPVersion obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00017524 File Offset: 0x00015724
	~SDPVersion()
	{
		this.Dispose();
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00017550 File Offset: 0x00015750
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SDPVersion(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x000175D0 File Offset: 0x000157D0
	public SDPVersion()
		: this(SDPCorePINVOKE.new_SDPVersion(), true)
	{
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x000175E0 File Offset: 0x000157E0
	public string GetFullVersionString()
	{
		return SDPCorePINVOKE.SDPVersion_GetFullVersionString(this.swigCPtr);
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x000175FC File Offset: 0x000157FC
	public long GetVersion()
	{
		return SDPCorePINVOKE.SDPVersion_GetVersion(this.swigCPtr);
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x00017618 File Offset: 0x00015818
	public string GetVersionString()
	{
		return SDPCorePINVOKE.SDPVersion_GetVersionString(this.swigCPtr);
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x00017634 File Offset: 0x00015834
	public string GetBuildDate()
	{
		return SDPCorePINVOKE.SDPVersion_GetBuildDate(this.swigCPtr);
	}

	// Token: 0x040001BC RID: 444
	private HandleRef swigCPtr;

	// Token: 0x040001BD RID: 445
	protected bool swigCMemOwn;
}
