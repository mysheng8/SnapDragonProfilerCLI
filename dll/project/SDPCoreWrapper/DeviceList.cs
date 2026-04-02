using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200002C RID: 44
public class DeviceList : IDisposable, IEnumerable, IList<Device>, ICollection<Device>, IEnumerable<Device>
{
	// Token: 0x06000231 RID: 561 RVA: 0x00006F01 File Offset: 0x00005101
	internal DeviceList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00006F1D File Offset: 0x0000511D
	internal static HandleRef getCPtr(DeviceList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00006F34 File Offset: 0x00005134
	~DeviceList()
	{
		this.Dispose();
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00006F60 File Offset: 0x00005160
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00006FE0 File Offset: 0x000051E0
	public DeviceList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			Device device = (Device)obj;
			this.Add(device);
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000236 RID: 566 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000237 RID: 567 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000049 RID: 73
	public Device this[int index]
	{
		get
		{
			return this.getitem(index);
		}
		set
		{
			this.setitem(index, value);
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600023A RID: 570 RVA: 0x0000705E File Offset: 0x0000525E
	// (set) Token: 0x0600023B RID: 571 RVA: 0x00007066 File Offset: 0x00005266
	public int Capacity
	{
		get
		{
			return (int)this.capacity();
		}
		set
		{
			if ((long)value < (long)((ulong)this.size()))
			{
				throw new ArgumentOutOfRangeException("Capacity");
			}
			this.reserve((uint)value);
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600023C RID: 572 RVA: 0x00007085 File Offset: 0x00005285
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600023D RID: 573 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000708D File Offset: 0x0000528D
	public void CopyTo(Device[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000709E File Offset: 0x0000529E
	public void CopyTo(Device[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x000070B0 File Offset: 0x000052B0
	public void CopyTo(int index, Device[] array, int arrayIndex, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", "Value is less than zero");
		}
		if (arrayIndex < 0)
		{
			throw new ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Value is less than zero");
		}
		if (array.Rank > 1)
		{
			throw new ArgumentException("Multi dimensional array.", "array");
		}
		if (index + count > this.Count || arrayIndex + count > array.Length)
		{
			throw new ArgumentException("Number of elements to copy is too large.");
		}
		for (int i = 0; i < count; i++)
		{
			array.SetValue(this.getitemcopy(index + i), arrayIndex + i);
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00007160 File Offset: 0x00005360
	IEnumerator<Device> IEnumerable<Device>.GetEnumerator()
	{
		return new DeviceList.DeviceListEnumerator(this);
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00007160 File Offset: 0x00005360
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new DeviceList.DeviceListEnumerator(this);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00007160 File Offset: 0x00005360
	public DeviceList.DeviceListEnumerator GetEnumerator()
	{
		return new DeviceList.DeviceListEnumerator(this);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00007168 File Offset: 0x00005368
	public void Clear()
	{
		SDPCorePINVOKE.DeviceList_Clear(this.swigCPtr);
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00007175 File Offset: 0x00005375
	public void Add(Device x)
	{
		SDPCorePINVOKE.DeviceList_Add(this.swigCPtr, Device.getCPtr(x));
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00007188 File Offset: 0x00005388
	private uint size()
	{
		return SDPCorePINVOKE.DeviceList_size(this.swigCPtr);
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000071A4 File Offset: 0x000053A4
	private uint capacity()
	{
		return SDPCorePINVOKE.DeviceList_capacity(this.swigCPtr);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x000071BE File Offset: 0x000053BE
	private void reserve(uint n)
	{
		SDPCorePINVOKE.DeviceList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x000071CC File Offset: 0x000053CC
	public DeviceList()
		: this(SDPCorePINVOKE.new_DeviceList__SWIG_0(), true)
	{
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000071DA File Offset: 0x000053DA
	public DeviceList(DeviceList other)
		: this(SDPCorePINVOKE.new_DeviceList__SWIG_1(DeviceList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x000071FB File Offset: 0x000053FB
	public DeviceList(int capacity)
		: this(SDPCorePINVOKE.new_DeviceList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00007218 File Offset: 0x00005418
	private Device getitemcopy(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceList_getitemcopy(this.swigCPtr, index);
		Device device = ((intPtr == IntPtr.Zero) ? null : new Device(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return device;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00007258 File Offset: 0x00005458
	private Device getitem(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceList_getitem(this.swigCPtr, index);
		Device device = ((intPtr == IntPtr.Zero) ? null : new Device(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return device;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x00007298 File Offset: 0x00005498
	private void setitem(int index, Device val)
	{
		SDPCorePINVOKE.DeviceList_setitem(this.swigCPtr, index, Device.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600024F RID: 591 RVA: 0x000072B9 File Offset: 0x000054B9
	public void AddRange(DeviceList values)
	{
		SDPCorePINVOKE.DeviceList_AddRange(this.swigCPtr, DeviceList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000250 RID: 592 RVA: 0x000072DC File Offset: 0x000054DC
	public DeviceList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceList_GetRange(this.swigCPtr, index, count);
		DeviceList deviceList = ((intPtr == IntPtr.Zero) ? null : new DeviceList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return deviceList;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000731D File Offset: 0x0000551D
	public void Insert(int index, Device x)
	{
		SDPCorePINVOKE.DeviceList_Insert(this.swigCPtr, index, Device.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000733E File Offset: 0x0000553E
	public void InsertRange(int index, DeviceList values)
	{
		SDPCorePINVOKE.DeviceList_InsertRange(this.swigCPtr, index, DeviceList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000735F File Offset: 0x0000555F
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.DeviceList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000737A File Offset: 0x0000557A
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.DeviceList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00007398 File Offset: 0x00005598
	public static DeviceList Repeat(Device value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceList_Repeat(Device.getCPtr(value), count);
		DeviceList deviceList = ((intPtr == IntPtr.Zero) ? null : new DeviceList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return deviceList;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000073D8 File Offset: 0x000055D8
	public void Reverse()
	{
		SDPCorePINVOKE.DeviceList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x000073E5 File Offset: 0x000055E5
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.DeviceList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00007401 File Offset: 0x00005601
	public void SetRange(int index, DeviceList values)
	{
		SDPCorePINVOKE.DeviceList_SetRange(this.swigCPtr, index, DeviceList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00007424 File Offset: 0x00005624
	public bool Contains(Device value)
	{
		return SDPCorePINVOKE.DeviceList_Contains(this.swigCPtr, Device.getCPtr(value));
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00007444 File Offset: 0x00005644
	public int IndexOf(Device value)
	{
		return SDPCorePINVOKE.DeviceList_IndexOf(this.swigCPtr, Device.getCPtr(value));
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00007464 File Offset: 0x00005664
	public int LastIndexOf(Device value)
	{
		return SDPCorePINVOKE.DeviceList_LastIndexOf(this.swigCPtr, Device.getCPtr(value));
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00007484 File Offset: 0x00005684
	public bool Remove(Device value)
	{
		return SDPCorePINVOKE.DeviceList_Remove(this.swigCPtr, Device.getCPtr(value));
	}

	// Token: 0x04000086 RID: 134
	private HandleRef swigCPtr;

	// Token: 0x04000087 RID: 135
	protected bool swigCMemOwn;

	// Token: 0x020000D7 RID: 215
	public sealed class DeviceListEnumerator : IEnumerator, IEnumerator<Device>, IDisposable
	{
		// Token: 0x060014A9 RID: 5289 RVA: 0x000195FE File Offset: 0x000177FE
		public DeviceListEnumerator(DeviceList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x0001962C File Offset: 0x0001782C
		public Device Current
		{
			get
			{
				if (this.currentIndex == -1)
				{
					throw new InvalidOperationException("Enumeration not started.");
				}
				if (this.currentIndex > this.currentSize - 1)
				{
					throw new InvalidOperationException("Enumeration finished.");
				}
				if (this.currentObject == null)
				{
					throw new InvalidOperationException("Collection modified.");
				}
				return (Device)this.currentObject;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x00019686 File Offset: 0x00017886
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00019690 File Offset: 0x00017890
		public bool MoveNext()
		{
			int count = this.collectionRef.Count;
			bool flag = this.currentIndex + 1 < count && count == this.currentSize;
			if (flag)
			{
				this.currentIndex++;
				this.currentObject = this.collectionRef[this.currentIndex];
			}
			else
			{
				this.currentObject = null;
			}
			return flag;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000196F3 File Offset: 0x000178F3
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00019721 File Offset: 0x00017921
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x040001EF RID: 495
		private DeviceList collectionRef;

		// Token: 0x040001F0 RID: 496
		private int currentIndex;

		// Token: 0x040001F1 RID: 497
		private object currentObject;

		// Token: 0x040001F2 RID: 498
		private int currentSize;
	}
}
