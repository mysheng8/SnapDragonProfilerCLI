using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x02000056 RID: 86
public class OptionList : IDisposable, IEnumerable, IList<Option>, ICollection<Option>, IEnumerable<Option>
{
	// Token: 0x0600057B RID: 1403 RVA: 0x0000E809 File Offset: 0x0000CA09
	internal OptionList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0000E825 File Offset: 0x0000CA25
	internal static HandleRef getCPtr(OptionList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0000E83C File Offset: 0x0000CA3C
	~OptionList()
	{
		this.Dispose();
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0000E868 File Offset: 0x0000CA68
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
	public OptionList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			Option option = (Option)obj;
			this.Add(option);
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000580 RID: 1408 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000581 RID: 1409 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000099 RID: 153
	public Option this[int index]
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

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000E963 File Offset: 0x0000CB63
	// (set) Token: 0x06000585 RID: 1413 RVA: 0x0000E96B File Offset: 0x0000CB6B
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

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x0000E98A File Offset: 0x0000CB8A
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000587 RID: 1415 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0000E992 File Offset: 0x0000CB92
	public void CopyTo(Option[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0000E9A3 File Offset: 0x0000CBA3
	public void CopyTo(Option[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0000E9B4 File Offset: 0x0000CBB4
	public void CopyTo(int index, Option[] array, int arrayIndex, int count)
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

	// Token: 0x0600058B RID: 1419 RVA: 0x0000EA64 File Offset: 0x0000CC64
	IEnumerator<Option> IEnumerable<Option>.GetEnumerator()
	{
		return new OptionList.OptionListEnumerator(this);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0000EA64 File Offset: 0x0000CC64
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new OptionList.OptionListEnumerator(this);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0000EA64 File Offset: 0x0000CC64
	public OptionList.OptionListEnumerator GetEnumerator()
	{
		return new OptionList.OptionListEnumerator(this);
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0000EA6C File Offset: 0x0000CC6C
	public void Clear()
	{
		SDPCorePINVOKE.OptionList_Clear(this.swigCPtr);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0000EA79 File Offset: 0x0000CC79
	public void Add(Option x)
	{
		SDPCorePINVOKE.OptionList_Add(this.swigCPtr, Option.getCPtr(x));
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0000EA8C File Offset: 0x0000CC8C
	private uint size()
	{
		return SDPCorePINVOKE.OptionList_size(this.swigCPtr);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
	private uint capacity()
	{
		return SDPCorePINVOKE.OptionList_capacity(this.swigCPtr);
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0000EAC2 File Offset: 0x0000CCC2
	private void reserve(uint n)
	{
		SDPCorePINVOKE.OptionList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
	public OptionList()
		: this(SDPCorePINVOKE.new_OptionList__SWIG_0(), true)
	{
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0000EADE File Offset: 0x0000CCDE
	public OptionList(OptionList other)
		: this(SDPCorePINVOKE.new_OptionList__SWIG_1(OptionList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0000EAFF File Offset: 0x0000CCFF
	public OptionList(int capacity)
		: this(SDPCorePINVOKE.new_OptionList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0000EB1C File Offset: 0x0000CD1C
	private Option getitemcopy(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionList_getitemcopy(this.swigCPtr, index);
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0000EB5C File Offset: 0x0000CD5C
	private Option getitem(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionList_getitem(this.swigCPtr, index);
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0000EB9C File Offset: 0x0000CD9C
	private void setitem(int index, Option val)
	{
		SDPCorePINVOKE.OptionList_setitem(this.swigCPtr, index, Option.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0000EBBD File Offset: 0x0000CDBD
	public void AddRange(OptionList values)
	{
		SDPCorePINVOKE.OptionList_AddRange(this.swigCPtr, OptionList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0000EBE0 File Offset: 0x0000CDE0
	public OptionList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionList_GetRange(this.swigCPtr, index, count);
		OptionList optionList = ((intPtr == IntPtr.Zero) ? null : new OptionList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionList;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0000EC21 File Offset: 0x0000CE21
	public void Insert(int index, Option x)
	{
		SDPCorePINVOKE.OptionList_Insert(this.swigCPtr, index, Option.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0000EC42 File Offset: 0x0000CE42
	public void InsertRange(int index, OptionList values)
	{
		SDPCorePINVOKE.OptionList_InsertRange(this.swigCPtr, index, OptionList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0000EC63 File Offset: 0x0000CE63
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.OptionList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0000EC7E File Offset: 0x0000CE7E
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.OptionList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0000EC9C File Offset: 0x0000CE9C
	public static OptionList Repeat(Option value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionList_Repeat(Option.getCPtr(value), count);
		OptionList optionList = ((intPtr == IntPtr.Zero) ? null : new OptionList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionList;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0000ECDC File Offset: 0x0000CEDC
	public void Reverse()
	{
		SDPCorePINVOKE.OptionList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0000ECE9 File Offset: 0x0000CEE9
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.OptionList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0000ED05 File Offset: 0x0000CF05
	public void SetRange(int index, OptionList values)
	{
		SDPCorePINVOKE.OptionList_SetRange(this.swigCPtr, index, OptionList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0000ED28 File Offset: 0x0000CF28
	public bool Contains(Option value)
	{
		return SDPCorePINVOKE.OptionList_Contains(this.swigCPtr, Option.getCPtr(value));
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0000ED48 File Offset: 0x0000CF48
	public int IndexOf(Option value)
	{
		return SDPCorePINVOKE.OptionList_IndexOf(this.swigCPtr, Option.getCPtr(value));
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0000ED68 File Offset: 0x0000CF68
	public int LastIndexOf(Option value)
	{
		return SDPCorePINVOKE.OptionList_LastIndexOf(this.swigCPtr, Option.getCPtr(value));
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0000ED88 File Offset: 0x0000CF88
	public bool Remove(Option value)
	{
		return SDPCorePINVOKE.OptionList_Remove(this.swigCPtr, Option.getCPtr(value));
	}

	// Token: 0x04000139 RID: 313
	private HandleRef swigCPtr;

	// Token: 0x0400013A RID: 314
	protected bool swigCMemOwn;

	// Token: 0x020000F0 RID: 240
	public sealed class OptionListEnumerator : IEnumerator, IEnumerator<Option>, IDisposable
	{
		// Token: 0x06001529 RID: 5417 RVA: 0x0001A199 File Offset: 0x00018399
		public OptionListEnumerator(OptionList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0001A1C8 File Offset: 0x000183C8
		public Option Current
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
				return (Option)this.currentObject;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0001A222 File Offset: 0x00018422
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0001A22C File Offset: 0x0001842C
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

		// Token: 0x0600152D RID: 5421 RVA: 0x0001A28F File Offset: 0x0001848F
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0001A2BD File Offset: 0x000184BD
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x04000211 RID: 529
		private OptionList collectionRef;

		// Token: 0x04000212 RID: 530
		private int currentIndex;

		// Token: 0x04000213 RID: 531
		private object currentObject;

		// Token: 0x04000214 RID: 532
		private int currentSize;
	}
}
