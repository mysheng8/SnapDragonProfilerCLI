using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x02000048 RID: 72
public class ModelObjectDataList : IDisposable, IEnumerable, IEnumerable<ModelObjectData>
{
	// Token: 0x06000485 RID: 1157 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
	internal ModelObjectDataList(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
	internal static HandleRef getCPtr(ModelObjectDataList obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
	~ModelObjectDataList()
	{
		this.Dispose();
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0000C214 File Offset: 0x0000A414
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ModelObjectDataList(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0000C294 File Offset: 0x0000A494
	public ModelObjectDataList(ICollection c)
		: this()
	{
		if (c == null)
		{
			throw new ArgumentNullException("c");
		}
		foreach (object obj in c)
		{
			ModelObjectData modelObjectData = (ModelObjectData)obj;
			this.Add(modelObjectData);
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000093 RID: 147
	public ModelObjectData this[int index]
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

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000C30F File Offset: 0x0000A50F
	// (set) Token: 0x0600048F RID: 1167 RVA: 0x0000C317 File Offset: 0x0000A517
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

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000C336 File Offset: 0x0000A536
	public int Count
	{
		get
		{
			return (int)this.size();
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x00007048 File Offset: 0x00005248
	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0000C33E File Offset: 0x0000A53E
	public void CopyTo(ModelObjectData[] array)
	{
		this.CopyTo(0, array, 0, this.Count);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0000C34F File Offset: 0x0000A54F
	public void CopyTo(ModelObjectData[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.Count);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0000C360 File Offset: 0x0000A560
	public void CopyTo(int index, ModelObjectData[] array, int arrayIndex, int count)
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

	// Token: 0x06000495 RID: 1173 RVA: 0x0000C410 File Offset: 0x0000A610
	IEnumerator<ModelObjectData> IEnumerable<ModelObjectData>.GetEnumerator()
	{
		return new ModelObjectDataList.ModelObjectDataListEnumerator(this);
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0000C410 File Offset: 0x0000A610
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new ModelObjectDataList.ModelObjectDataListEnumerator(this);
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0000C410 File Offset: 0x0000A610
	public ModelObjectDataList.ModelObjectDataListEnumerator GetEnumerator()
	{
		return new ModelObjectDataList.ModelObjectDataListEnumerator(this);
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0000C418 File Offset: 0x0000A618
	public void Clear()
	{
		SDPCorePINVOKE.ModelObjectDataList_Clear(this.swigCPtr);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0000C425 File Offset: 0x0000A625
	public void Add(ModelObjectData x)
	{
		SDPCorePINVOKE.ModelObjectDataList_Add(this.swigCPtr, ModelObjectData.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0000C448 File Offset: 0x0000A648
	private uint size()
	{
		return SDPCorePINVOKE.ModelObjectDataList_size(this.swigCPtr);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0000C464 File Offset: 0x0000A664
	private uint capacity()
	{
		return SDPCorePINVOKE.ModelObjectDataList_capacity(this.swigCPtr);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0000C47E File Offset: 0x0000A67E
	private void reserve(uint n)
	{
		SDPCorePINVOKE.ModelObjectDataList_reserve(this.swigCPtr, n);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0000C48C File Offset: 0x0000A68C
	public ModelObjectDataList()
		: this(SDPCorePINVOKE.new_ModelObjectDataList__SWIG_0(), true)
	{
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0000C49A File Offset: 0x0000A69A
	public ModelObjectDataList(ModelObjectDataList other)
		: this(SDPCorePINVOKE.new_ModelObjectDataList__SWIG_1(ModelObjectDataList.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0000C4BB File Offset: 0x0000A6BB
	public ModelObjectDataList(int capacity)
		: this(SDPCorePINVOKE.new_ModelObjectDataList__SWIG_2(capacity), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0000C4D8 File Offset: 0x0000A6D8
	private ModelObjectData getitemcopy(int index)
	{
		ModelObjectData modelObjectData = new ModelObjectData(SDPCorePINVOKE.ModelObjectDataList_getitemcopy(this.swigCPtr, index), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectData;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0000C508 File Offset: 0x0000A708
	private ModelObjectData getitem(int index)
	{
		ModelObjectData modelObjectData = new ModelObjectData(SDPCorePINVOKE.ModelObjectDataList_getitem(this.swigCPtr, index), false);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectData;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0000C536 File Offset: 0x0000A736
	private void setitem(int index, ModelObjectData val)
	{
		SDPCorePINVOKE.ModelObjectDataList_setitem(this.swigCPtr, index, ModelObjectData.getCPtr(val));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0000C557 File Offset: 0x0000A757
	public void AddRange(ModelObjectDataList values)
	{
		SDPCorePINVOKE.ModelObjectDataList_AddRange(this.swigCPtr, ModelObjectDataList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0000C578 File Offset: 0x0000A778
	public ModelObjectDataList GetRange(int index, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ModelObjectDataList_GetRange(this.swigCPtr, index, count);
		ModelObjectDataList modelObjectDataList = ((intPtr == IntPtr.Zero) ? null : new ModelObjectDataList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0000C5B9 File Offset: 0x0000A7B9
	public void Insert(int index, ModelObjectData x)
	{
		SDPCorePINVOKE.ModelObjectDataList_Insert(this.swigCPtr, index, ModelObjectData.getCPtr(x));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0000C5DA File Offset: 0x0000A7DA
	public void InsertRange(int index, ModelObjectDataList values)
	{
		SDPCorePINVOKE.ModelObjectDataList_InsertRange(this.swigCPtr, index, ModelObjectDataList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0000C5FB File Offset: 0x0000A7FB
	public void RemoveAt(int index)
	{
		SDPCorePINVOKE.ModelObjectDataList_RemoveAt(this.swigCPtr, index);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0000C616 File Offset: 0x0000A816
	public void RemoveRange(int index, int count)
	{
		SDPCorePINVOKE.ModelObjectDataList_RemoveRange(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0000C634 File Offset: 0x0000A834
	public static ModelObjectDataList Repeat(ModelObjectData value, int count)
	{
		IntPtr intPtr = SDPCorePINVOKE.ModelObjectDataList_Repeat(ModelObjectData.getCPtr(value), count);
		ModelObjectDataList modelObjectDataList = ((intPtr == IntPtr.Zero) ? null : new ModelObjectDataList(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0000C674 File Offset: 0x0000A874
	public void Reverse()
	{
		SDPCorePINVOKE.ModelObjectDataList_Reverse__SWIG_0(this.swigCPtr);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0000C681 File Offset: 0x0000A881
	public void Reverse(int index, int count)
	{
		SDPCorePINVOKE.ModelObjectDataList_Reverse__SWIG_1(this.swigCPtr, index, count);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0000C69D File Offset: 0x0000A89D
	public void SetRange(int index, ModelObjectDataList values)
	{
		SDPCorePINVOKE.ModelObjectDataList_SetRange(this.swigCPtr, index, ModelObjectDataList.getCPtr(values));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000126 RID: 294
	private HandleRef swigCPtr;

	// Token: 0x04000127 RID: 295
	protected bool swigCMemOwn;

	// Token: 0x020000EF RID: 239
	public sealed class ModelObjectDataListEnumerator : IEnumerator, IEnumerator<ModelObjectData>, IDisposable
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x0001A064 File Offset: 0x00018264
		public ModelObjectDataListEnumerator(ModelObjectDataList collection)
		{
			this.collectionRef = collection;
			this.currentIndex = -1;
			this.currentObject = null;
			this.currentSize = this.collectionRef.Count;
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0001A094 File Offset: 0x00018294
		public ModelObjectData Current
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
				return (ModelObjectData)this.currentObject;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0001A0EE File Offset: 0x000182EE
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0001A0F8 File Offset: 0x000182F8
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

		// Token: 0x06001527 RID: 5415 RVA: 0x0001A15B File Offset: 0x0001835B
		public void Reset()
		{
			this.currentIndex = -1;
			this.currentObject = null;
			if (this.collectionRef.Count != this.currentSize)
			{
				throw new InvalidOperationException("Collection modified.");
			}
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0001A189 File Offset: 0x00018389
		public void Dispose()
		{
			this.currentIndex = -1;
			this.currentObject = null;
		}

		// Token: 0x0400020D RID: 525
		private ModelObjectDataList collectionRef;

		// Token: 0x0400020E RID: 526
		private int currentIndex;

		// Token: 0x0400020F RID: 527
		private object currentObject;

		// Token: 0x04000210 RID: 528
		private int currentSize;
	}
}
