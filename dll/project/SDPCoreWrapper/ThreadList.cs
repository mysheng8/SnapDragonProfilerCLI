using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x020000BA RID: 186
public class ThreadList : IDisposable, IEnumerable, IList<SWIGTYPE_p_SDP__Thread>, ICollection<SWIGTYPE_p_SDP__Thread>, IEnumerable<SWIGTYPE_p_SDP__Thread>
{
	// Token: 0x06001408 RID: 5128 RVA: 0x00018F1F File Offset: 0x0001711F
	internal ThreadList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x00018F3B File Offset: 0x0001713B
	internal static HandleRef getCPtr(ThreadList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x00018F54 File Offset: 0x00017154
	~ThreadList()
	{
		this.Dispose();
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x00018F80 File Offset: 0x00017180
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ThreadList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x00019000 File Offset: 0x00017200
	public ThreadList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			SWIGTYPE_p_SDP__Thread swigtype_p_SDP__Thread = (SWIGTYPE_p_SDP__Thread)obj;
			this.Add(swigtype_p_SDP__Thread);
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x0600140D RID: 5133 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x0600140E RID: 5134 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002FD RID: 765
	public SWIGTYPE_p_SDP__Thread this[int index]
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

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x0001907B File Offset: 0x0001727B
	// (set) Token: 0x06001412 RID: 5138 RVA: 0x00019083 File Offset: 0x00017283
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

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x000190A2 File Offset: 0x000172A2
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06001414 RID: 5140 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x000190AA File Offset: 0x000172AA
	public void CopyTo(SWIGTYPE_p_SDP__Thread[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000190BB File Offset: 0x000172BB
	public void CopyTo(SWIGTYPE_p_SDP__Thread[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000190CC File Offset: 0x000172CC
	public void CopyTo(int index, SWIGTYPE_p_SDP__Thread[] array, int arrayIndex, int count)
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

	// Token: 0x06001418 RID: 5144 RVA: 0x0001917C File Offset: 0x0001737C
	IEnumerator<SWIGTYPE_p_SDP__Thread> IEnumerable<SWIGTYPE_p_SDP__Thread>.GetEnumerator()
	{
		return new ThreadList.ThreadListEnumerator(this);
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0001917C File Offset: 0x0001737C
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new ThreadList.ThreadListEnumerator(this);
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x0001917C File Offset: 0x0001737C
	public ThreadList.ThreadListEnumerator GetEnumerator()
	{
		return new ThreadList.ThreadListEnumerator(this);
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x00019184 File Offset: 0x00017384
	public void Clear()
	{
		SDPCorePINVOKE.ThreadList_Clear(this.swigCPtr);
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x00019191 File Offset: 0x00017391
	public void Add(SWIGTYPE_p_SDP__Thread x)
	{
		SDPCorePINVOKE.ThreadList_Add(this.swigCPtr, SWIGTYPE_p_SDP__Thread.getCPtr(x));
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000191A4 File Offset: 0x000173A4
	private uint size()
	{
		return SDPCorePINVOKE.ThreadList_size(this.swigCPtr);
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000191C0 File Offset: 0x000173C0
	private uint capacity()
	{
		return SDPCorePINVOKE.ThreadList_capacity(this.swigCPtr);
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000191DA File Offset: 0x000173DA
	private void reserve(uint n)
	{
		SDPCorePINVOKE.ThreadList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x000191E8 File Offset: 0x000173E8
	public ThreadList()
		: this(SDPCorePINVOKE.new_ThreadList__SWIG_0(), true)
	{
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000191F6 File Offset: 0x000173F6
	public ThreadList(ThreadList other)
		: this(SDPCorePINVOKE.new_ThreadList__SWIG_1(ThreadList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x00019217 File Offset: 0x00017417
	public ThreadList(int capacity)
		: this(SDPCorePINVOKE.new_ThreadList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00019234 File Offset: 0x00017434
	private SWIGTYPE_p_SDP__Thread getitemcopy(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.ThreadList_getitemcopy(this.swigCPtr, index);
		SWIGTYPE_p_SDP__Thread swigtype_p_SDP__Thread = ((intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__Thread(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return swigtype_p_SDP__Thread;
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x00019274 File Offset: 0x00017474
	private SWIGTYPE_p_SDP__Thread getitem(int index)
	{
		IntPtr intPtr = SDPCorePINVOKE.ThreadList_getitem(this.swigCPtr, index);
		SWIGTYPE_p_SDP__Thread swigtype_p_SDP__Thread = ((intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__Thread(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return swigtype_p_SDP__Thread;
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000192B4 File Offset: 0x000174B4
	private void setitem(int index, SWIGTYPE_p_SDP__Thread val)
	{
		SDPCorePINVOKE.ThreadList_setitem(this.swigCPtr, index, SWIGTYPE_p_SDP__Thread.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000192D5 File Offset: 0x000174D5
	public void AddRange(ThreadList values)
	{
		SDPCorePINVOKE.ThreadList_AddRange(this.swigCPtr, ThreadList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000192F8 File Offset: 0x000174F8
	public ThreadList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ThreadList_GetRange(this.swigCPtr, index, count);
		ThreadList threadList = ((intPtr == IntPtr.Zero) ? null : new ThreadList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return threadList;
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x00019339 File Offset: 0x00017539
	public void Insert(int index, SWIGTYPE_p_SDP__Thread x)
	{
		SDPCorePINVOKE.ThreadList_Insert(this.swigCPtr, index, SWIGTYPE_p_SDP__Thread.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0001935A File Offset: 0x0001755A
	public void InsertRange(int index, ThreadList values)
	{
		SDPCorePINVOKE.ThreadList_InsertRange(this.swigCPtr, index, ThreadList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0001937B File Offset: 0x0001757B
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.ThreadList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x00019396 File Offset: 0x00017596
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.ThreadList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000193B4 File Offset: 0x000175B4
	public static ThreadList Repeat(SWIGTYPE_p_SDP__Thread value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ThreadList_Repeat(SWIGTYPE_p_SDP__Thread.getCPtr(value), count);
		ThreadList threadList = ((intPtr == IntPtr.Zero) ? null : new ThreadList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return threadList;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000193F4 File Offset: 0x000175F4
	public void Reverse()
	{
		SDPCorePINVOKE.ThreadList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x00019401 File Offset: 0x00017601
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.ThreadList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x0001941D File Offset: 0x0001761D
	public void SetRange(int index, ThreadList values)
	{
		SDPCorePINVOKE.ThreadList_SetRange(this.swigCPtr, index, ThreadList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x00019440 File Offset: 0x00017640
	public bool Contains(SWIGTYPE_p_SDP__Thread value)
	{
		return SDPCorePINVOKE.ThreadList_Contains(this.swigCPtr, SWIGTYPE_p_SDP__Thread.getCPtr(value));
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00019460 File Offset: 0x00017660
	public int IndexOf(SWIGTYPE_p_SDP__Thread value)
	{
		return SDPCorePINVOKE.ThreadList_IndexOf(this.swigCPtr, SWIGTYPE_p_SDP__Thread.getCPtr(value));
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x00019480 File Offset: 0x00017680
	public int LastIndexOf(SWIGTYPE_p_SDP__Thread value)
	{
		return SDPCorePINVOKE.ThreadList_LastIndexOf(this.swigCPtr, SWIGTYPE_p_SDP__Thread.getCPtr(value));
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x000194A0 File Offset: 0x000176A0
	public bool Remove(SWIGTYPE_p_SDP__Thread value)
	{
		return SDPCorePINVOKE.ThreadList_Remove(this.swigCPtr, SWIGTYPE_p_SDP__Thread.getCPtr(value));
	}

	// Token: 0x040001EB RID: 491
	private HandleRef swigCPtr;

	// Token: 0x040001EC RID: 492
	protected bool swigCMemOwn;

	// Token: 0x020000F7 RID: 247
	public sealed class ThreadListEnumerator : IEnumerator, IEnumerator<SWIGTYPE_p_SDP__Thread>, IDisposable
	{
		// Token: 0x0600155B RID: 5467 RVA: 0x0001AA25 File Offset: 0x00018C25
		public ThreadListEnumerator(ThreadList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0001AA54 File Offset: 0x00018C54
		public SWIGTYPE_p_SDP__Thread Current
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
				return (SWIGTYPE_p_SDP__Thread)this.currentObject;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x0001AAAE File Offset: 0x00018CAE
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0001AAB8 File Offset: 0x00018CB8
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

		// Token: 0x0600155F RID: 5471 RVA: 0x0001AB1B File Offset: 0x00018D1B
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0001AB49 File Offset: 0x00018D49
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x04000232 RID: 562
		private ThreadList collectionRef;

		// Token: 0x04000233 RID: 563
		private int currentIndex;

		// Token: 0x04000234 RID: 564
		private object currentObject;

		// Token: 0x04000235 RID: 565
		private int currentSize;
	}
}
