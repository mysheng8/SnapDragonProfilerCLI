using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200019B RID: 411
	public class ByteBufferGateway
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x0000B0DF File Offset: 0x000092DF
		public ByteBufferGateway(string modelName, string tableName)
		{
			this.m_modelName = modelName;
			this.m_tableName = tableName;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000B0F8 File Offset: 0x000092F8
		public IEnumerable<IByteBuffer> GetByteBuffers(int captureID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString()
			};
			return this.GetByteBuffers(stringList);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000B12C File Offset: 0x0000932C
		public IEnumerable<IByteBuffer> GetByteBuffers(int captureID, ulong resourceID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString(),
				"resourceID",
				resourceID.ToString()
			};
			return this.GetByteBuffers(stringList);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000B178 File Offset: 0x00009378
		public IEnumerable<IByteBuffer> GetByteBuffers(StringList stringList)
		{
			ByteBufferGateway.ByteBufferListImpl byteBufferListImpl = new ByteBufferGateway.ByteBufferListImpl(stringList, this.m_modelName, this.m_tableName);
			if (!byteBufferListImpl.IsValid())
			{
				return null;
			}
			return byteBufferListImpl;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000B1A3 File Offset: 0x000093A3
		public IByteBuffer GetByteBuffer(int captureID, ulong resourceID)
		{
			return this.GetByteBuffer(captureID, resourceID, uint.MaxValue);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000B1B0 File Offset: 0x000093B0
		public IByteBuffer GetByteBuffer(int captureID, ulong resourceID, uint sequenceID)
		{
			StringList stringList = new StringList
			{
				"captureID",
				captureID.ToString(),
				"resourceID",
				resourceID.ToString(),
				"sequenceID",
				sequenceID.ToString()
			};
			ByteBufferGateway.ByteBufferListImpl byteBufferListImpl = new ByteBufferGateway.ByteBufferListImpl(stringList, this.m_modelName, this.m_tableName);
			if (byteBufferListImpl.IsValid())
			{
				return byteBufferListImpl.GetValue(0);
			}
			return null;
		}

		// Token: 0x04000618 RID: 1560
		private string m_modelName;

		// Token: 0x04000619 RID: 1561
		private string m_tableName;

		// Token: 0x02000370 RID: 880
		private class ByteBufferListImpl : MODGatewayList<IByteBuffer, ByteBufferGateway.ByteBufferListImpl>, IByteBuffer
		{
			// Token: 0x060011A5 RID: 4517 RVA: 0x000368A2 File Offset: 0x00034AA2
			public ByteBufferListImpl(StringList searchString, string modelName, string tableName)
				: base(searchString, modelName, tableName)
			{
			}

			// Token: 0x170002F6 RID: 758
			// (get) Token: 0x060011A6 RID: 4518 RVA: 0x000368AD File Offset: 0x00034AAD
			public uint CaptureID
			{
				get
				{
					return base.GetUIntValue("captureID");
				}
			}

			// Token: 0x170002F7 RID: 759
			// (get) Token: 0x060011A7 RID: 4519 RVA: 0x000368BA File Offset: 0x00034ABA
			public ulong ResourceID
			{
				get
				{
					return base.GetULongValue("resourceID");
				}
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x060011A8 RID: 4520 RVA: 0x000368C7 File Offset: 0x00034AC7
			public uint SequenceID
			{
				get
				{
					return base.GetUIntValue("sequenceID");
				}
			}

			// Token: 0x170002F9 RID: 761
			// (get) Token: 0x060011A9 RID: 4521 RVA: 0x000368D4 File Offset: 0x00034AD4
			public uint Offset
			{
				get
				{
					return base.GetUIntValue("offset");
				}
			}

			// Token: 0x170002FA RID: 762
			// (get) Token: 0x060011AA RID: 4522 RVA: 0x000368E1 File Offset: 0x00034AE1
			public BinaryDataPair BDP
			{
				get
				{
					return base.GetBDPValue("data");
				}
			}
		}

		// Token: 0x02000371 RID: 881
		private static class ColumnNames
		{
			// Token: 0x04000C0D RID: 3085
			internal const string CaptureID = "captureID";

			// Token: 0x04000C0E RID: 3086
			internal const string ResourceID = "resourceID";

			// Token: 0x04000C0F RID: 3087
			internal const string Data = "data";

			// Token: 0x04000C10 RID: 3088
			internal const string SequenceID = "sequenceID";

			// Token: 0x04000C11 RID: 3089
			internal const string Offset = "offset";
		}
	}
}
