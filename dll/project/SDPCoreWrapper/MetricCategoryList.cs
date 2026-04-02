using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200003A RID: 58
public class MetricCategoryList : IDisposable, IEnumerable, IEnumerable<MetricCategory>
{
	// Token: 0x0600034E RID: 846 RVA: 0x000094F9 File Offset: 0x000076F9
	internal MetricCategoryList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00009515 File Offset: 0x00007715
	internal static HandleRef getCPtr(MetricCategoryList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0000952C File Offset: 0x0000772C
	~MetricCategoryList()
	{
		this.Dispose();
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00009558 File Offset: 0x00007758
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricCategoryList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x000095D8 File Offset: 0x000077D8
	public MetricCategoryList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			MetricCategory metricCategory = (MetricCategory)obj;
			this.Add(metricCategory);
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000353 RID: 851 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000354 RID: 852 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000068 RID: 104
	public MetricCategory this[int index]
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

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000357 RID: 855 RVA: 0x00009653 File Offset: 0x00007853
	// (set) Token: 0x06000358 RID: 856 RVA: 0x0000965B File Offset: 0x0000785B
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

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000359 RID: 857 RVA: 0x0000967A File Offset: 0x0000787A
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600035A RID: 858 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00009682 File Offset: 0x00007882
	public void CopyTo(MetricCategory[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00009693 File Offset: 0x00007893
	public void CopyTo(MetricCategory[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000096A4 File Offset: 0x000078A4
	public void CopyTo(int index, MetricCategory[] array, int arrayIndex, int count)
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

	// Token: 0x0600035E RID: 862 RVA: 0x00009754 File Offset: 0x00007954
	IEnumerator<MetricCategory> IEnumerable<MetricCategory>.GetEnumerator()
	{
		return new MetricCategoryList.MetricCategoryListEnumerator(this);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00009754 File Offset: 0x00007954
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new MetricCategoryList.MetricCategoryListEnumerator(this);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00009754 File Offset: 0x00007954
	public MetricCategoryList.MetricCategoryListEnumerator GetEnumerator()
	{
		return new MetricCategoryList.MetricCategoryListEnumerator(this);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0000975C File Offset: 0x0000795C
	public void Clear()
	{
		SDPCorePINVOKE.MetricCategoryList_Clear(this.swigCPtr);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00009769 File Offset: 0x00007969
	public void Add(MetricCategory x)
	{
		SDPCorePINVOKE.MetricCategoryList_Add(this.swigCPtr, MetricCategory.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0000978C File Offset: 0x0000798C
	private uint size()
	{
		return SDPCorePINVOKE.MetricCategoryList_size(this.swigCPtr);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x000097A8 File Offset: 0x000079A8
	private uint capacity()
	{
		return SDPCorePINVOKE.MetricCategoryList_capacity(this.swigCPtr);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x000097C2 File Offset: 0x000079C2
	private void reserve(uint n)
	{
		SDPCorePINVOKE.MetricCategoryList_reserve(this.swigCPtr, n);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x000097D0 File Offset: 0x000079D0
	public MetricCategoryList()
		: this(SDPCorePINVOKE.new_MetricCategoryList__SWIG_0(), true)
	{
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000097DE File Offset: 0x000079DE
	public MetricCategoryList(MetricCategoryList other)
		: this(SDPCorePINVOKE.new_MetricCategoryList__SWIG_1(MetricCategoryList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x000097FF File Offset: 0x000079FF
	public MetricCategoryList(int capacity)
		: this(SDPCorePINVOKE.new_MetricCategoryList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0000981C File Offset: 0x00007A1C
	private MetricCategory getitemcopy(int index)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricCategoryList_getitemcopy(this.swigCPtr, index), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0000984C File Offset: 0x00007A4C
	private MetricCategory getitem(int index)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricCategoryList_getitem(this.swigCPtr, index), false);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0000987A File Offset: 0x00007A7A
	private void setitem(int index, MetricCategory val)
	{
		SDPCorePINVOKE.MetricCategoryList_setitem(this.swigCPtr, index, MetricCategory.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0000989B File Offset: 0x00007A9B
	public void AddRange(MetricCategoryList values)
	{
		SDPCorePINVOKE.MetricCategoryList_AddRange(this.swigCPtr, MetricCategoryList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000098BC File Offset: 0x00007ABC
	public MetricCategoryList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.MetricCategoryList_GetRange(this.swigCPtr, index, count);
		MetricCategoryList metricCategoryList = ((intPtr == IntPtr.Zero) ? null : new MetricCategoryList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategoryList;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x000098FD File Offset: 0x00007AFD
	public void Insert(int index, MetricCategory x)
	{
		SDPCorePINVOKE.MetricCategoryList_Insert(this.swigCPtr, index, MetricCategory.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0000991E File Offset: 0x00007B1E
	public void InsertRange(int index, MetricCategoryList values)
	{
		SDPCorePINVOKE.MetricCategoryList_InsertRange(this.swigCPtr, index, MetricCategoryList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0000993F File Offset: 0x00007B3F
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.MetricCategoryList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0000995A File Offset: 0x00007B5A
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.MetricCategoryList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00009978 File Offset: 0x00007B78
	public static MetricCategoryList Repeat(MetricCategory value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.MetricCategoryList_Repeat(MetricCategory.getCPtr(value), count);
		MetricCategoryList metricCategoryList = ((intPtr == IntPtr.Zero) ? null : new MetricCategoryList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategoryList;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000099B8 File Offset: 0x00007BB8
	public void Reverse()
	{
		SDPCorePINVOKE.MetricCategoryList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000099C5 File Offset: 0x00007BC5
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.MetricCategoryList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000099E1 File Offset: 0x00007BE1
	public void SetRange(int index, MetricCategoryList values)
	{
		SDPCorePINVOKE.MetricCategoryList_SetRange(this.swigCPtr, index, MetricCategoryList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x040000EC RID: 236
	private HandleRef swigCPtr;

	// Token: 0x040000ED RID: 237
	protected bool swigCMemOwn;

	// Token: 0x020000DD RID: 221
	public sealed class MetricCategoryListEnumerator : IEnumerator, IEnumerator<MetricCategory>, IDisposable
	{
		// Token: 0x060014D2 RID: 5330 RVA: 0x00019B91 File Offset: 0x00017D91
		public MetricCategoryListEnumerator(MetricCategoryList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060014D3 RID: 5331 RVA: 0x00019BC0 File Offset: 0x00017DC0
		public MetricCategory Current
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
				return (MetricCategory)this.currentObject;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x00019C1A File Offset: 0x00017E1A
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00019C24 File Offset: 0x00017E24
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

		// Token: 0x060014D6 RID: 5334 RVA: 0x00019C87 File Offset: 0x00017E87
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00019CB5 File Offset: 0x00017EB5
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x040001FF RID: 511
		private MetricCategoryList collectionRef;

		// Token: 0x04000200 RID: 512
		private int currentIndex;

		// Token: 0x04000201 RID: 513
		private object currentObject;

		// Token: 0x04000202 RID: 514
		private int currentSize;
	}
}
