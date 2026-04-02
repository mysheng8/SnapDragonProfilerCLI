using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x02000060 RID: 96
public class ProcessList : IDisposable, IEnumerable, IEnumerable<Process>
{
	// Token: 0x0600062C RID: 1580 RVA: 0x00010334 File Offset: 0x0000E534
	internal ProcessList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00010350 File Offset: 0x0000E550
	internal static HandleRef getCPtr(ProcessList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00010368 File Offset: 0x0000E568
	~ProcessList()
	{
		this.Dispose();
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00010394 File Offset: 0x0000E594
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00010414 File Offset: 0x0000E614
	public ProcessList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			Process process = (Process)obj;
			this.Add(process);
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000631 RID: 1585 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170000AA RID: 170
	public Process this[int index]
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

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001048F File Offset: 0x0000E68F
	// (set) Token: 0x06000636 RID: 1590 RVA: 0x00010497 File Offset: 0x0000E697
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

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x000104B6 File Offset: 0x0000E6B6
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x000104BE File Offset: 0x0000E6BE
	public void CopyTo(Process[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x000104CF File Offset: 0x0000E6CF
	public void CopyTo(Process[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x000104E0 File Offset: 0x0000E6E0
	public void CopyTo(int index, Process[] array, int arrayIndex, int count)
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

	// Token: 0x0600063C RID: 1596 RVA: 0x00010590 File Offset: 0x0000E790
	IEnumerator<Process> IEnumerable<Process>.GetEnumerator()
	{
		return new ProcessList.ProcessListEnumerator(this);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00010590 File Offset: 0x0000E790
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new ProcessList.ProcessListEnumerator(this);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00010590 File Offset: 0x0000E790
	public ProcessList.ProcessListEnumerator GetEnumerator()
	{
		return new ProcessList.ProcessListEnumerator(this);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00010598 File Offset: 0x0000E798
	public void Clear()
	{
		SDPCorePINVOKE.ProcessList_Clear(this.swigCPtr);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x000105A5 File Offset: 0x0000E7A5
	public void Add(Process x)
	{
		SDPCorePINVOKE.ProcessList_Add(this.swigCPtr, Process.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x000105C8 File Offset: 0x0000E7C8
	private uint size()
	{
		return SDPCorePINVOKE.ProcessList_size(this.swigCPtr);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x000105E4 File Offset: 0x0000E7E4
	private uint capacity()
	{
		return SDPCorePINVOKE.ProcessList_capacity(this.swigCPtr);
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x000105FE File Offset: 0x0000E7FE
	private void reserve(uint n)
	{
		SDPCorePINVOKE.ProcessList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0001060C File Offset: 0x0000E80C
	public ProcessList()
		: this(SDPCorePINVOKE.new_ProcessList__SWIG_0(), true)
	{
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0001061A File Offset: 0x0000E81A
	public ProcessList(ProcessList other)
		: this(SDPCorePINVOKE.new_ProcessList__SWIG_1(ProcessList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0001063B File Offset: 0x0000E83B
	public ProcessList(int capacity)
		: this(SDPCorePINVOKE.new_ProcessList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00010658 File Offset: 0x0000E858
	private Process getitemcopy(int index)
	{
		Process process = new Process(SDPCorePINVOKE.ProcessList_getitemcopy(this.swigCPtr, index), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return process;
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00010688 File Offset: 0x0000E888
	private Process getitem(int index)
	{
		Process process = new Process(SDPCorePINVOKE.ProcessList_getitem(this.swigCPtr, index), false);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return process;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x000106B6 File Offset: 0x0000E8B6
	private void setitem(int index, Process val)
	{
		SDPCorePINVOKE.ProcessList_setitem(this.swigCPtr, index, Process.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000106D7 File Offset: 0x0000E8D7
	public void AddRange(ProcessList values)
	{
		SDPCorePINVOKE.ProcessList_AddRange(this.swigCPtr, ProcessList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x000106F8 File Offset: 0x0000E8F8
	public ProcessList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProcessList_GetRange(this.swigCPtr, index, count);
		ProcessList processList = ((intPtr == IntPtr.Zero) ? null : new ProcessList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return processList;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00010739 File Offset: 0x0000E939
	public void Insert(int index, Process x)
	{
		SDPCorePINVOKE.ProcessList_Insert(this.swigCPtr, index, Process.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0001075A File Offset: 0x0000E95A
	public void InsertRange(int index, ProcessList values)
	{
		SDPCorePINVOKE.ProcessList_InsertRange(this.swigCPtr, index, ProcessList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0001077B File Offset: 0x0000E97B
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.ProcessList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00010796 File Offset: 0x0000E996
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.ProcessList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x000107B4 File Offset: 0x0000E9B4
	public static ProcessList Repeat(Process value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ProcessList_Repeat(Process.getCPtr(value), count);
		ProcessList processList = ((intPtr == IntPtr.Zero) ? null : new ProcessList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return processList;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x000107F4 File Offset: 0x0000E9F4
	public void Reverse()
	{
		SDPCorePINVOKE.ProcessList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00010801 File Offset: 0x0000EA01
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.ProcessList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0001081D File Offset: 0x0000EA1D
	public void SetRange(int index, ProcessList values)
	{
		SDPCorePINVOKE.ProcessList_SetRange(this.swigCPtr, index, ProcessList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000148 RID: 328
	private HandleRef swigCPtr;

	// Token: 0x04000149 RID: 329
	protected bool swigCMemOwn;

	// Token: 0x020000F1 RID: 241
	public sealed class ProcessListEnumerator : IEnumerator, IEnumerator<Process>, IDisposable
	{
		// Token: 0x0600152F RID: 5423 RVA: 0x0001A2CD File Offset: 0x000184CD
		public ProcessListEnumerator(ProcessList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0001A2FC File Offset: 0x000184FC
		public Process Current
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
				return (Process)this.currentObject;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x0001A356 File Offset: 0x00018556
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0001A360 File Offset: 0x00018560
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

		// Token: 0x06001533 RID: 5427 RVA: 0x0001A3C3 File Offset: 0x000185C3
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0001A3F1 File Offset: 0x000185F1
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x04000215 RID: 533
		private ProcessList collectionRef;

		// Token: 0x04000216 RID: 534
		private int currentIndex;

		// Token: 0x04000217 RID: 535
		private object currentObject;

		// Token: 0x04000218 RID: 536
		private int currentSize;
	}
}
