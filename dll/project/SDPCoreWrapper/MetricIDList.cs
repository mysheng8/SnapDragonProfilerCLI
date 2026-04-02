using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200003E RID: 62
public class MetricIDList : IDisposable, IEnumerable, ICollection<uint>, IEnumerable<uint>
{
	// Token: 0x060003AA RID: 938 RVA: 0x0000A314 File Offset: 0x00008514
	internal MetricIDList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0000A330 File Offset: 0x00008530
	internal static HandleRef getCPtr(MetricIDList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0000A348 File Offset: 0x00008548
	~MetricIDList()
	{
		this.Dispose();
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0000A374 File Offset: 0x00008574
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricIDList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0000A3F4 File Offset: 0x000085F4
	public MetricIDList(ICollection c)
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

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060003AF RID: 943 RVA: 0x0000A45C File Offset: 0x0000865C
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0000A464 File Offset: 0x00008664
	public void Clear()
	{
		this.clear();
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060003B1 RID: 945 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0000A46C File Offset: 0x0000866C
	public void CopyTo(uint[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0000A47D File Offset: 0x0000867D
	public void CopyTo(uint[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0000A490 File Offset: 0x00008690
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
		int num = 0;
		int num2 = 0;
		foreach (uint num3 in this)
		{
			if (num2 >= index)
			{
				array.SetValue(num3, arrayIndex + num);
				num++;
			}
			if (num >= count)
			{
				break;
			}
			num2++;
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0000A57C File Offset: 0x0000877C
	IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
	{
		return new MetricIDList.MetricIDListEnumerator(this);
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0000A57C File Offset: 0x0000877C
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new MetricIDList.MetricIDListEnumerator(this);
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0000A57C File Offset: 0x0000877C
	public MetricIDList.MetricIDListEnumerator GetEnumerator()
	{
		return new MetricIDList.MetricIDListEnumerator(this);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0000A584 File Offset: 0x00008784
	public MetricIDList()
		: this(SDPCorePINVOKE.new_MetricIDList__SWIG_0(), true)
	{
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0000A592 File Offset: 0x00008792
	public MetricIDList(MetricIDList other)
		: this(SDPCorePINVOKE.new_MetricIDList__SWIG_1(MetricIDList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0000A5B4 File Offset: 0x000087B4
	private uint size()
	{
		return SDPCorePINVOKE.MetricIDList_size(this.swigCPtr);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0000A5D0 File Offset: 0x000087D0
	public bool empty()
	{
		return SDPCorePINVOKE.MetricIDList_empty(this.swigCPtr);
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0000A5EA File Offset: 0x000087EA
	public void clear()
	{
		SDPCorePINVOKE.MetricIDList_clear(this.swigCPtr);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0000A5F8 File Offset: 0x000087F8
	public bool Contains(uint val)
	{
		return SDPCorePINVOKE.MetricIDList_Contains(this.swigCPtr, val);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0000A613 File Offset: 0x00008813
	public void Add(uint val)
	{
		SDPCorePINVOKE.MetricIDList_Add(this.swigCPtr, val);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0000A630 File Offset: 0x00008830
	public bool Remove(uint val)
	{
		return SDPCorePINVOKE.MetricIDList_Remove(this.swigCPtr, val);
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0000A64C File Offset: 0x0000884C
	private IntPtr create_iterator_begin()
	{
		return SDPCorePINVOKE.MetricIDList_create_iterator_begin(this.swigCPtr);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0000A668 File Offset: 0x00008868
	private uint get_next(IntPtr swigiterator)
	{
		return SDPCorePINVOKE.MetricIDList_get_next(this.swigCPtr, swigiterator);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0000A683 File Offset: 0x00008883
	private void destroy_iterator(IntPtr swigiterator)
	{
		SDPCorePINVOKE.MetricIDList_destroy_iterator(this.swigCPtr, swigiterator);
	}

	// Token: 0x04000100 RID: 256
	private HandleRef swigCPtr;

	// Token: 0x04000101 RID: 257
	protected bool swigCMemOwn;

	// Token: 0x020000E4 RID: 228
	public sealed class MetricIDListEnumerator : IEnumerator, IEnumerator<uint>, IDisposable
	{
		// Token: 0x060014F0 RID: 5360 RVA: 0x00019CC5 File Offset: 0x00017EC5
		public MetricIDListEnumerator(MetricIDList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
			this.iterator = this.collectionRef.create_iterator_begin();
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x00019D04 File Offset: 0x00017F04
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

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00019D5E File Offset: 0x00017F5E
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00019D6C File Offset: 0x00017F6C
		public bool MoveNext()
		{
			int count = this.collectionRef.Count;
			bool flag = this.currentIndex + 1 < count && count == this.currentSize;
			if (flag)
			{
				this.currentIndex++;
				this.currentObject = this.collectionRef.get_next(this.iterator);
			}
			else
			{
				this.currentObject = null;
			}
			return flag;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00019DD4 File Offset: 0x00017FD4
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00019E02 File Offset: 0x00018002
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			this.collectionRef.destroy_iterator(this.iterator);
		}

		// Token: 0x04000203 RID: 515
		private MetricIDList collectionRef;

		// Token: 0x04000204 RID: 516
		private int currentIndex;

		// Token: 0x04000205 RID: 517
		private object currentObject;

		// Token: 0x04000206 RID: 518
		private int currentSize;

		// Token: 0x04000207 RID: 519
		private IntPtr iterator;
	}
}
