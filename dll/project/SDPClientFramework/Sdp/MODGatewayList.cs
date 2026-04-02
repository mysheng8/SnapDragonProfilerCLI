using System;
using System.Collections;
using System.Collections.Generic;
using Sdp.Functional;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x02000198 RID: 408
	public abstract class MODGatewayList<IGatewayObj, GatewayListImpl> : IEnumerable<IGatewayObj>, IEnumerable where GatewayListImpl : MODGatewayList<IGatewayObj, GatewayListImpl>, IGatewayObj
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x0000AD8A File Offset: 0x00008F8A
		public MODGatewayList(StringList searchString, string modelName, string tableName)
		{
			this.m_MODList = this.GetDataList(searchString, modelName, tableName);
			this.m_index = 0;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		public int Count
		{
			get
			{
				if (this.m_MODList != null)
				{
					return this.m_MODList.Count;
				}
				return 0;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public IGatewayObj GetValue(int index)
		{
			this.m_index = index;
			if (this.IsValid())
			{
				return (IGatewayObj)((object)(this as GatewayListImpl));
			}
			return default(IGatewayObj);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000ADFB File Offset: 0x00008FFB
		public bool IsValid()
		{
			return this.m_MODList != null && this.m_MODList.Count > this.m_index;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000AE1A File Offset: 0x0000901A
		public IEnumerator<IGatewayObj> GetEnumerator()
		{
			return new MODGatewayListEnumerator<IGatewayObj, GatewayListImpl>(this);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000AE1A File Offset: 0x0000901A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MODGatewayListEnumerator<IGatewayObj, GatewayListImpl>(this);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000AE24 File Offset: 0x00009024
		[Obsolete("GetUintValue is deprecated please use TryGetUIntValue instead")]
		protected uint GetUIntValue(string columnName)
		{
			if (!this.IsValid())
			{
				return uint.MaxValue;
			}
			string value = this.m_MODList[this.m_index].GetValue(columnName);
			if (string.IsNullOrEmpty(value))
			{
				return uint.MaxValue;
			}
			return UintConverter.Convert(value);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000AE64 File Offset: 0x00009064
		protected Result<uint, string> TryGetUIntValue(string columnName)
		{
			if (!this.IsValid())
			{
				return new Result<uint, string>.Error("Unable to retrieve uint value for column: " + columnName);
			}
			string value = this.m_MODList[this.m_index].GetValue(columnName);
			if (string.IsNullOrEmpty(value))
			{
				return new Result<uint, string>.Error("Unable to retrieve uint value for column: " + columnName);
			}
			return UintConverter.TryConvert(value);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000AEC4 File Offset: 0x000090C4
		protected ulong GetULongValue(string columnName)
		{
			if (!this.IsValid())
			{
				return ulong.MaxValue;
			}
			string value = this.m_MODList[this.m_index].GetValue(columnName);
			if (string.IsNullOrEmpty(value))
			{
				return ulong.MaxValue;
			}
			return Uint64Converter.Convert(value);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000AF08 File Offset: 0x00009108
		protected Result<ulong, string> TryGetULongValue(string columnName)
		{
			if (!this.IsValid())
			{
				return new Result<ulong, string>.Error("Unable to retrieve ulong value for column: " + columnName);
			}
			string value = this.m_MODList[this.m_index].GetValue(columnName);
			if (string.IsNullOrEmpty(value))
			{
				return new Result<ulong, string>.Error("Unable to retrieve ulong value for column: " + columnName);
			}
			return new Result<ulong, string>.Success(Uint64Converter.Convert(value));
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000AF6C File Offset: 0x0000916C
		[Obsolete("GetStringValue is deprecated please use TryGetStringValue instead")]
		protected string GetStringValue(string columnName)
		{
			if (!this.IsValid())
			{
				return null;
			}
			return this.m_MODList[this.m_index].GetValue(columnName);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000AF9C File Offset: 0x0000919C
		protected Result<string, string> TryGetStringValue(string columnName)
		{
			if (!this.IsValid())
			{
				return new Result<string, string>.Error("Unable to retrieve string value for column: " + columnName);
			}
			string value = this.m_MODList[this.m_index].GetValue(columnName);
			if (string.IsNullOrEmpty(value))
			{
				return new Result<string, string>.Error("Unable to retrieve string value for column: " + columnName);
			}
			return new Result<string, string>.Success(value);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000AFFC File Offset: 0x000091FC
		protected BinaryDataPair GetBDPValue(string columnName)
		{
			if (!this.IsValid())
			{
				return null;
			}
			return this.m_MODList[this.m_index].GetValuePtrBinaryDataPair(columnName);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000B02C File Offset: 0x0000922C
		private ModelObjectDataList GetDataList(StringList searchString, string ModelName, string TableName)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel(ModelName);
			if (model == null)
			{
				return null;
			}
			ModelObject modelObject = dataModel.GetModelObject(model, TableName);
			if (modelObject == null)
			{
				return null;
			}
			ModelObjectDataList data = modelObject.GetData(searchString);
			if (data == null)
			{
				return null;
			}
			return data;
		}

		// Token: 0x04000614 RID: 1556
		private int m_index;

		// Token: 0x04000615 RID: 1557
		private ModelObjectDataList m_MODList;
	}
}
