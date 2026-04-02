using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200003F RID: 63
public class MetricList : IDisposable, IEnumerable, IEnumerable<Metric>
{
	// Token: 0x060003C3 RID: 963 RVA: 0x0000A691 File Offset: 0x00008891
	internal MetricList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0000A6AD File Offset: 0x000088AD
	internal static HandleRef getCPtr(MetricList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0000A6C4 File Offset: 0x000088C4
	~MetricList()
	{
		this.Dispose();
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0000A6F0 File Offset: 0x000088F0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0000A770 File Offset: 0x00008970
	public MetricList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			Metric metric = (Metric)obj;
			this.Add(metric);
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060003C8 RID: 968 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060003C9 RID: 969 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000079 RID: 121
	public Metric this[int index]
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

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060003CC RID: 972 RVA: 0x0000A7EB File Offset: 0x000089EB
	// (set) Token: 0x060003CD RID: 973 RVA: 0x0000A7F3 File Offset: 0x000089F3
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

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060003CE RID: 974 RVA: 0x0000A812 File Offset: 0x00008A12
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060003CF RID: 975 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0000A81A File Offset: 0x00008A1A
	public void CopyTo(Metric[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0000A82B File Offset: 0x00008A2B
	public void CopyTo(Metric[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0000A83C File Offset: 0x00008A3C
	public void CopyTo(int index, Metric[] array, int arrayIndex, int count)
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

	// Token: 0x060003D3 RID: 979 RVA: 0x0000A8EC File Offset: 0x00008AEC
	IEnumerator<Metric> IEnumerable<Metric>.GetEnumerator()
	{
		return new MetricList.MetricListEnumerator(this);
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0000A8EC File Offset: 0x00008AEC
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new MetricList.MetricListEnumerator(this);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0000A8EC File Offset: 0x00008AEC
	public MetricList.MetricListEnumerator GetEnumerator()
	{
		return new MetricList.MetricListEnumerator(this);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0000A8F4 File Offset: 0x00008AF4
	public void Clear()
	{
		SDPCorePINVOKE.MetricList_Clear(this.swigCPtr);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0000A901 File Offset: 0x00008B01
	public void Add(Metric x)
	{
		SDPCorePINVOKE.MetricList_Add(this.swigCPtr, Metric.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0000A924 File Offset: 0x00008B24
	private uint size()
	{
		return SDPCorePINVOKE.MetricList_size(this.swigCPtr);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0000A940 File Offset: 0x00008B40
	private uint capacity()
	{
		return SDPCorePINVOKE.MetricList_capacity(this.swigCPtr);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0000A95A File Offset: 0x00008B5A
	private void reserve(uint n)
	{
		SDPCorePINVOKE.MetricList_reserve(this.swigCPtr, n);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0000A968 File Offset: 0x00008B68
	public MetricList()
		: this(SDPCorePINVOKE.new_MetricList__SWIG_0(), true)
	{
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0000A976 File Offset: 0x00008B76
	public MetricList(MetricList other)
		: this(SDPCorePINVOKE.new_MetricList__SWIG_1(MetricList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0000A997 File Offset: 0x00008B97
	public MetricList(int capacity)
		: this(SDPCorePINVOKE.new_MetricList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0000A9B4 File Offset: 0x00008BB4
	private Metric getitemcopy(int index)
	{
		Metric metric = new Metric(SDPCorePINVOKE.MetricList_getitemcopy(this.swigCPtr, index), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0000A9E4 File Offset: 0x00008BE4
	private Metric getitem(int index)
	{
		Metric metric = new Metric(SDPCorePINVOKE.MetricList_getitem(this.swigCPtr, index), false);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0000AA12 File Offset: 0x00008C12
	private void setitem(int index, Metric val)
	{
		SDPCorePINVOKE.MetricList_setitem(this.swigCPtr, index, Metric.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0000AA33 File Offset: 0x00008C33
	public void AddRange(MetricList values)
	{
		SDPCorePINVOKE.MetricList_AddRange(this.swigCPtr, MetricList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0000AA54 File Offset: 0x00008C54
	public MetricList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.MetricList_GetRange(this.swigCPtr, index, count);
		MetricList metricList = ((intPtr == IntPtr.Zero) ? null : new MetricList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricList;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0000AA95 File Offset: 0x00008C95
	public void Insert(int index, Metric x)
	{
		SDPCorePINVOKE.MetricList_Insert(this.swigCPtr, index, Metric.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0000AAB6 File Offset: 0x00008CB6
	public void InsertRange(int index, MetricList values)
	{
		SDPCorePINVOKE.MetricList_InsertRange(this.swigCPtr, index, MetricList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0000AAD7 File Offset: 0x00008CD7
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.MetricList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0000AAF2 File Offset: 0x00008CF2
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.MetricList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0000AB10 File Offset: 0x00008D10
	public static MetricList Repeat(Metric value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.MetricList_Repeat(Metric.getCPtr(value), count);
		MetricList metricList = ((intPtr == IntPtr.Zero) ? null : new MetricList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricList;
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0000AB50 File Offset: 0x00008D50
	public void Reverse()
	{
		SDPCorePINVOKE.MetricList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0000AB5D File Offset: 0x00008D5D
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.MetricList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0000AB79 File Offset: 0x00008D79
	public void SetRange(int index, MetricList values)
	{
		SDPCorePINVOKE.MetricList_SetRange(this.swigCPtr, index, MetricList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000102 RID: 258
	private HandleRef swigCPtr;

	// Token: 0x04000103 RID: 259
	protected bool swigCMemOwn;

	// Token: 0x020000E5 RID: 229
	public sealed class MetricListEnumerator : IEnumerator, IEnumerator<Metric>, IDisposable
	{
		// Token: 0x060014F6 RID: 5366 RVA: 0x00019E23 File Offset: 0x00018023
		public MetricListEnumerator(MetricList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x00019E54 File Offset: 0x00018054
		public Metric Current
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
				return (Metric)this.currentObject;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00019EAE File Offset: 0x000180AE
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00019EB8 File Offset: 0x000180B8
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

		// Token: 0x060014FA RID: 5370 RVA: 0x00019F1B File Offset: 0x0001811B
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00019F49 File Offset: 0x00018149
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x04000208 RID: 520
		private MetricList collectionRef;

		// Token: 0x04000209 RID: 521
		private int currentIndex;

		// Token: 0x0400020A RID: 522
		private object currentObject;

		// Token: 0x0400020B RID: 523
		private int currentSize;
	}
}
