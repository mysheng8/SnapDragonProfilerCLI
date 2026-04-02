using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x02000033 RID: 51
public class IDList : IDisposable, IEnumerable, IList<uint>, ICollection<uint>, IEnumerable<uint>
{
	// Token: 0x060002D9 RID: 729 RVA: 0x000085B2 File Offset: 0x000067B2
	internal IDList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000085CE File Offset: 0x000067CE
	internal static HandleRef getCPtr(IDList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000085E8 File Offset: 0x000067E8
	~IDList()
	{
		this.Dispose();
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00008614 File Offset: 0x00006814
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_IDList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00008694 File Offset: 0x00006894
	public IDList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			uint num = (uint)obj;
			this.Add(num);
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060002DE RID: 734 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060002DF RID: 735 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000062 RID: 98
	public uint this[int index]
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

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000870F File Offset: 0x0000690F
	// (set) Token: 0x060002E3 RID: 739 RVA: 0x00008717 File Offset: 0x00006917
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

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x00008736 File Offset: 0x00006936
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0000873E File Offset: 0x0000693E
	public void CopyTo(uint[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0000874F File Offset: 0x0000694F
	public void CopyTo(uint[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00008760 File Offset: 0x00006960
	public void CopyTo(int index, uint[] array, int arrayIndex, int count)
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

	// Token: 0x060002E9 RID: 745 RVA: 0x00008815 File Offset: 0x00006A15
	IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
	{
		return new IDList.IDListEnumerator(this);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00008815 File Offset: 0x00006A15
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new IDList.IDListEnumerator(this);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00008815 File Offset: 0x00006A15
	public IDList.IDListEnumerator GetEnumerator()
	{
		return new IDList.IDListEnumerator(this);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000881D File Offset: 0x00006A1D
	public void Clear()
	{
		SDPCorePINVOKE.IDList_Clear(this.swigCPtr);
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000882A File Offset: 0x00006A2A
	public void Add(uint x)
	{
		SDPCorePINVOKE.IDList_Add(this.swigCPtr, x);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00008838 File Offset: 0x00006A38
	private uint size()
	{
		return SDPCorePINVOKE.IDList_size(this.swigCPtr);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00008854 File Offset: 0x00006A54
	private uint capacity()
	{
		return SDPCorePINVOKE.IDList_capacity(this.swigCPtr);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000886E File Offset: 0x00006A6E
	private void reserve(uint n)
	{
		SDPCorePINVOKE.IDList_reserve(this.swigCPtr, n);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000887C File Offset: 0x00006A7C
	public IDList()
		: this(SDPCorePINVOKE.new_IDList__SWIG_0(), true)
	{
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000888A File Offset: 0x00006A8A
	public IDList(IDList other)
		: this(SDPCorePINVOKE.new_IDList__SWIG_1(IDList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000088AB File Offset: 0x00006AAB
	public IDList(int capacity)
		: this(SDPCorePINVOKE.new_IDList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000088C8 File Offset: 0x00006AC8
	private uint getitemcopy(int index)
	{
		uint num = SDPCorePINVOKE.IDList_getitemcopy(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000088F0 File Offset: 0x00006AF0
	private uint getitem(int index)
	{
		uint num = SDPCorePINVOKE.IDList_getitem(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00008918 File Offset: 0x00006B18
	private void setitem(int index, uint val)
	{
		SDPCorePINVOKE.IDList_setitem(this.swigCPtr, index, val);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00008934 File Offset: 0x00006B34
	public void AddRange(IDList values)
	{
		SDPCorePINVOKE.IDList_AddRange(this.swigCPtr, IDList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00008954 File Offset: 0x00006B54
	public IDList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.IDList_GetRange(this.swigCPtr, index, count);
		IDList idlist = ((intPtr == IntPtr.Zero) ? null : new IDList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return idlist;
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00008995 File Offset: 0x00006B95
	public void Insert(int index, uint x)
	{
		SDPCorePINVOKE.IDList_Insert(this.swigCPtr, index, x);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000089B1 File Offset: 0x00006BB1
	public void InsertRange(int index, IDList values)
	{
		SDPCorePINVOKE.IDList_InsertRange(this.swigCPtr, index, IDList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002FB RID: 763 RVA: 0x000089D2 File Offset: 0x00006BD2
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.IDList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000089ED File Offset: 0x00006BED
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.IDList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00008A0C File Offset: 0x00006C0C
	public static IDList Repeat(uint value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.IDList_Repeat(value, count);
		IDList idlist = ((intPtr == IntPtr.Zero) ? null : new IDList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return idlist;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00008A47 File Offset: 0x00006C47
	public void Reverse()
	{
		SDPCorePINVOKE.IDList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00008A54 File Offset: 0x00006C54
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.IDList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00008A70 File Offset: 0x00006C70
	public void SetRange(int index, IDList values)
	{
		SDPCorePINVOKE.IDList_SetRange(this.swigCPtr, index, IDList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00008A94 File Offset: 0x00006C94
	public bool Contains(uint value)
	{
		return SDPCorePINVOKE.IDList_Contains(this.swigCPtr, value);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00008AB0 File Offset: 0x00006CB0
	public int IndexOf(uint value)
	{
		return SDPCorePINVOKE.IDList_IndexOf(this.swigCPtr, value);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00008ACC File Offset: 0x00006CCC
	public int LastIndexOf(uint value)
	{
		return SDPCorePINVOKE.IDList_LastIndexOf(this.swigCPtr, value);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00008AE8 File Offset: 0x00006CE8
	public bool Remove(uint value)
	{
		return SDPCorePINVOKE.IDList_Remove(this.swigCPtr, value);
	}

	// Token: 0x040000A2 RID: 162
	private HandleRef swigCPtr;

	// Token: 0x040000A3 RID: 163
	protected bool swigCMemOwn;

	// Token: 0x020000D8 RID: 216
	public sealed class IDListEnumerator : IEnumerator, IEnumerator<uint>, IDisposable
	{
		// Token: 0x060014AF RID: 5295 RVA: 0x00019731 File Offset: 0x00017931
		public IDListEnumerator(IDList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x00019760 File Offset: 0x00017960
		public uint Current
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
				return (uint)this.currentObject;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x000197BA File Offset: 0x000179BA
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000197C8 File Offset: 0x000179C8
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

		// Token: 0x060014B3 RID: 5299 RVA: 0x00019830 File Offset: 0x00017A30
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0001985E File Offset: 0x00017A5E
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x040001F3 RID: 499
		private IDList collectionRef;

		// Token: 0x040001F4 RID: 500
		private int currentIndex;

		// Token: 0x040001F5 RID: 501
		private object currentObject;

		// Token: 0x040001F6 RID: 502
		private int currentSize;
	}
}
