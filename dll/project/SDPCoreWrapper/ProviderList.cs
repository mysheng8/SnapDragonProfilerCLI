using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200006B RID: 107
public class ProviderList : IDisposable, IEnumerable, IList<DataProvider>, ICollection<DataProvider>, IEnumerable<DataProvider>
{
	// Token: 0x060006CB RID: 1739 RVA: 0x00011722 File Offset: 0x0000F922
	internal ProviderList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0001173E File Offset: 0x0000F93E
	internal static HandleRef getCPtr(ProviderList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00011758 File Offset: 0x0000F958
	~ProviderList()
	{
		this.Dispose();
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00011784 File Offset: 0x0000F984
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProviderList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00011804 File Offset: 0x0000FA04
	public ProviderList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			DataProvider dataProvider = (DataProvider)obj;
			this.Add(dataProvider);
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000D0 RID: 208
	public DataProvider this[int index]
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

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0001187F File Offset: 0x0000FA7F
	// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00011887 File Offset: 0x0000FA87
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

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000118A6 File Offset: 0x0000FAA6
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x000118AE File Offset: 0x0000FAAE
	public void CopyTo(DataProvider[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000118BF File Offset: 0x0000FABF
	public void CopyTo(DataProvider[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000118D0 File Offset: 0x0000FAD0
	public void CopyTo(int index, DataProvider[] array, int arrayIndex, int count)
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

	// Token: 0x060006DB RID: 1755 RVA: 0x00011980 File Offset: 0x0000FB80
	IEnumerator<DataProvider> IEnumerable<DataProvider>.GetEnumerator()
	{
		return new ProviderList.ProviderListEnumerator(this);
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00011980 File Offset: 0x0000FB80
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new ProviderList.ProviderListEnumerator(this);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00011980 File Offset: 0x0000FB80
	public ProviderList.ProviderListEnumerator GetEnumerator()
	{
		return new ProviderList.ProviderListEnumerator(this);
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00011988 File Offset: 0x0000FB88
	public void Clear()
	{
		SDPCorePINVOKE.ProviderList_Clear(this.swigCPtr);
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00011995 File Offset: 0x0000FB95
	public void Add(DataProvider x)
	{
		SDPCorePINVOKE.ProviderList_Add(this.swigCPtr, DataProvider.getCPtr(x));
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000119A8 File Offset: 0x0000FBA8
	private uint size()
	{
		return SDPCorePINVOKE.ProviderList_size(this.swigCPtr);
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000119C4 File Offset: 0x0000FBC4
	private uint capacity()
	{
		return SDPCorePINVOKE.ProviderList_capacity(this.swigCPtr);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000119DE File Offset: 0x0000FBDE
	private void reserve(uint n)
	{
		SDPCorePINVOKE.ProviderList_reserve(this.swigCPtr, n);
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x000119EC File Offset: 0x0000FBEC
	public ProviderList()
		: this(SDPCorePINVOKE.new_ProviderList__SWIG_0(), true)
	{
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x000119FA File Offset: 0x0000FBFA
	public ProviderList(ProviderList other)
		: this(SDPCorePINVOKE.new_ProviderList__SWIG_1(ProviderList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00011A1B File Offset: 0x0000FC1B
	public ProviderList(int capacity)
		: this(SDPCorePINVOKE.new_ProviderList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00011A38 File Offset: 0x0000FC38
	private DataProvider getitemcopy(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProviderList_getitemcopy(this.swigCPtr, index);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00011A78 File Offset: 0x0000FC78
	private DataProvider getitem(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProviderList_getitem(this.swigCPtr, index);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00011AB8 File Offset: 0x0000FCB8
	private void setitem(int index, DataProvider val)
	{
		SDPCorePINVOKE.ProviderList_setitem(this.swigCPtr, index, DataProvider.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00011AD9 File Offset: 0x0000FCD9
	public void AddRange(ProviderList values)
	{
		SDPCorePINVOKE.ProviderList_AddRange(this.swigCPtr, ProviderList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00011AFC File Offset: 0x0000FCFC
	public ProviderList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProviderList_GetRange(this.swigCPtr, index, count);
		ProviderList providerList = ((intPtr == IntPtr.Zero) ? null : new ProviderList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return providerList;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00011B3D File Offset: 0x0000FD3D
	public void Insert(int index, DataProvider x)
	{
		SDPCorePINVOKE.ProviderList_Insert(this.swigCPtr, index, DataProvider.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00011B5E File Offset: 0x0000FD5E
	public void InsertRange(int index, ProviderList values)
	{
		SDPCorePINVOKE.ProviderList_InsertRange(this.swigCPtr, index, ProviderList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00011B7F File Offset: 0x0000FD7F
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.ProviderList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00011B9A File Offset: 0x0000FD9A
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.ProviderList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00011BB8 File Offset: 0x0000FDB8
	public static ProviderList Repeat(DataProvider value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProviderList_Repeat(DataProvider.getCPtr(value), count);
		ProviderList providerList = ((intPtr == IntPtr.Zero) ? null : new ProviderList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return providerList;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00011BF8 File Offset: 0x0000FDF8
	public void Reverse()
	{
		SDPCorePINVOKE.ProviderList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00011C05 File Offset: 0x0000FE05
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.ProviderList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00011C21 File Offset: 0x0000FE21
	public void SetRange(int index, ProviderList values)
	{
		SDPCorePINVOKE.ProviderList_SetRange(this.swigCPtr, index, ProviderList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00011C44 File Offset: 0x0000FE44
	public bool Contains(DataProvider value)
	{
		return SDPCorePINVOKE.ProviderList_Contains(this.swigCPtr, DataProvider.getCPtr(value));
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00011C64 File Offset: 0x0000FE64
	public int IndexOf(DataProvider value)
	{
		return SDPCorePINVOKE.ProviderList_IndexOf(this.swigCPtr, DataProvider.getCPtr(value));
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00011C84 File Offset: 0x0000FE84
	public int LastIndexOf(DataProvider value)
	{
		return SDPCorePINVOKE.ProviderList_LastIndexOf(this.swigCPtr, DataProvider.getCPtr(value));
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00011CA4 File Offset: 0x0000FEA4
	public bool Remove(DataProvider value)
	{
		return SDPCorePINVOKE.ProviderList_Remove(this.swigCPtr, DataProvider.getCPtr(value));
	}

	// Token: 0x04000163 RID: 355
	private HandleRef swigCPtr;

	// Token: 0x04000164 RID: 356
	protected bool swigCMemOwn;

	// Token: 0x020000F2 RID: 242
	public sealed class ProviderListEnumerator : IEnumerator, IEnumerator<DataProvider>, IDisposable
	{
		// Token: 0x06001535 RID: 5429 RVA: 0x0001A401 File Offset: 0x00018601
		public ProviderListEnumerator(ProviderList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0001A430 File Offset: 0x00018630
		public DataProvider Current
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
				return (DataProvider)this.currentObject;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0001A48A File Offset: 0x0001868A
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0001A494 File Offset: 0x00018694
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

		// Token: 0x06001539 RID: 5433 RVA: 0x0001A4F7 File Offset: 0x000186F7
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0001A525 File Offset: 0x00018725
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x04000219 RID: 537
		private ProviderList collectionRef;

		// Token: 0x0400021A RID: 538
		private int currentIndex;

		// Token: 0x0400021B RID: 539
		private object currentObject;

		// Token: 0x0400021C RID: 540
		private int currentSize;
	}
}
