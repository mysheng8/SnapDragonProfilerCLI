using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200009A RID: 154
public class StringList : IDisposable, IEnumerable, IList<string>, ICollection<string>, IEnumerable<string>
{
	// Token: 0x0600137F RID: 4991 RVA: 0x0001813A File Offset: 0x0001633A
	internal StringList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00018156 File Offset: 0x00016356
	internal static HandleRef getCPtr(StringList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x00018170 File Offset: 0x00016370
	~StringList()
	{
		this.Dispose();
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0001819C File Offset: 0x0001639C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_StringList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0001821C File Offset: 0x0001641C
	public StringList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			string text = (string)obj;
			this.Add(text);
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001384 RID: 4996 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001385 RID: 4997 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002F7 RID: 759
	public string this[int index]
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

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06001388 RID: 5000 RVA: 0x00018297 File Offset: 0x00016497
	// (set) Token: 0x06001389 RID: 5001 RVA: 0x0001829F File Offset: 0x0001649F
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

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x0600138A RID: 5002 RVA: 0x000182BE File Offset: 0x000164BE
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x0600138B RID: 5003 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x000182C6 File Offset: 0x000164C6
	public void CopyTo(string[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x000182D7 File Offset: 0x000164D7
	public void CopyTo(string[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000182E8 File Offset: 0x000164E8
	public void CopyTo(int index, string[] array, int arrayIndex, int count)
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

	// Token: 0x0600138F RID: 5007 RVA: 0x00018398 File Offset: 0x00016598
	IEnumerator<string> IEnumerable<string>.GetEnumerator()
	{
		return new StringList.StringListEnumerator(this);
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00018398 File Offset: 0x00016598
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new StringList.StringListEnumerator(this);
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x00018398 File Offset: 0x00016598
	public StringList.StringListEnumerator GetEnumerator()
	{
		return new StringList.StringListEnumerator(this);
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000183A0 File Offset: 0x000165A0
	public void Clear()
	{
		SDPCorePINVOKE.StringList_Clear(this.swigCPtr);
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x000183AD File Offset: 0x000165AD
	public void Add(string x)
	{
		SDPCorePINVOKE.StringList_Add(this.swigCPtr, x);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x000183C8 File Offset: 0x000165C8
	private uint size()
	{
		return SDPCorePINVOKE.StringList_size(this.swigCPtr);
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x000183E4 File Offset: 0x000165E4
	private uint capacity()
	{
		return SDPCorePINVOKE.StringList_capacity(this.swigCPtr);
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x000183FE File Offset: 0x000165FE
	private void reserve(uint n)
	{
		SDPCorePINVOKE.StringList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0001840C File Offset: 0x0001660C
	public StringList()
		: this(SDPCorePINVOKE.new_StringList__SWIG_0(), true)
	{
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0001841A File Offset: 0x0001661A
	public StringList(StringList other)
		: this(SDPCorePINVOKE.new_StringList__SWIG_1(StringList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x0001843B File Offset: 0x0001663B
	public StringList(int capacity)
		: this(SDPCorePINVOKE.new_StringList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x00018458 File Offset: 0x00016658
	private string getitemcopy(int index)
	{
		string text = SDPCorePINVOKE.StringList_getitemcopy(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x00018480 File Offset: 0x00016680
	private string getitem(int index)
	{
		string text = SDPCorePINVOKE.StringList_getitem(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x000184A8 File Offset: 0x000166A8
	private void setitem(int index, string val)
	{
		SDPCorePINVOKE.StringList_setitem(this.swigCPtr, index, val);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x000184C4 File Offset: 0x000166C4
	public void AddRange(StringList values)
	{
		SDPCorePINVOKE.StringList_AddRange(this.swigCPtr, StringList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x000184E4 File Offset: 0x000166E4
	public StringList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.StringList_GetRange(this.swigCPtr, index, count);
		StringList stringList = ((intPtr == IntPtr.Zero) ? null : new StringList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return stringList;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00018525 File Offset: 0x00016725
	public void Insert(int index, string x)
	{
		SDPCorePINVOKE.StringList_Insert(this.swigCPtr, index, x);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x00018541 File Offset: 0x00016741
	public void InsertRange(int index, StringList values)
	{
		SDPCorePINVOKE.StringList_InsertRange(this.swigCPtr, index, StringList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x00018562 File Offset: 0x00016762
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.StringList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0001857D File Offset: 0x0001677D
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.StringList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0001859C File Offset: 0x0001679C
	public static StringList Repeat(string value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.StringList_Repeat(value, count);
		StringList stringList = ((intPtr == IntPtr.Zero) ? null : new StringList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return stringList;
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000185D7 File Offset: 0x000167D7
	public void Reverse()
	{
		SDPCorePINVOKE.StringList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000185E4 File Offset: 0x000167E4
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.StringList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x00018600 File Offset: 0x00016800
	public void SetRange(int index, StringList values)
	{
		SDPCorePINVOKE.StringList_SetRange(this.swigCPtr, index, StringList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x00018624 File Offset: 0x00016824
	public bool Contains(string value)
	{
		bool flag = SDPCorePINVOKE.StringList_Contains(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x0001864C File Offset: 0x0001684C
	public int IndexOf(string value)
	{
		int num = SDPCorePINVOKE.StringList_IndexOf(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060013A9 RID: 5033 RVA: 0x00018674 File Offset: 0x00016874
	public int LastIndexOf(string value)
	{
		int num = SDPCorePINVOKE.StringList_LastIndexOf(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x0001869C File Offset: 0x0001689C
	public bool Remove(string value)
	{
		bool flag = SDPCorePINVOKE.StringList_Remove(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x040001CA RID: 458
	private HandleRef swigCPtr;

	// Token: 0x040001CB RID: 459
	protected bool swigCMemOwn;

	// Token: 0x020000F6 RID: 246
	public sealed class StringListEnumerator : IEnumerator, IEnumerator<string>, IDisposable
	{
		// Token: 0x06001555 RID: 5461 RVA: 0x0001A8F0 File Offset: 0x00018AF0
		public StringListEnumerator(StringList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0001A920 File Offset: 0x00018B20
		public string Current
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
				return (string)this.currentObject;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x0001A97A File Offset: 0x00018B7A
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0001A984 File Offset: 0x00018B84
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

		// Token: 0x06001559 RID: 5465 RVA: 0x0001A9E7 File Offset: 0x00018BE7
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0001AA15 File Offset: 0x00018C15
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x0400022E RID: 558
		private StringList collectionRef;

		// Token: 0x0400022F RID: 559
		private int currentIndex;

		// Token: 0x04000230 RID: 560
		private object currentObject;

		// Token: 0x04000231 RID: 561
		private int currentSize;
	}
}
