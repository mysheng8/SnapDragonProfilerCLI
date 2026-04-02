using System;
using System.Collections;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000199 RID: 409
	internal class MODGatewayListEnumerator<IGatewayObj, GatewayListImpl> : IEnumerator<IGatewayObj>, IDisposable, IEnumerator where GatewayListImpl : MODGatewayList<IGatewayObj, GatewayListImpl>, IGatewayObj
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x0000B06D File Offset: 0x0000926D
		public MODGatewayListEnumerator(MODGatewayList<IGatewayObj, GatewayListImpl> gatewayList)
		{
			this.m_gatewayList = gatewayList;
			this.m_currentIndex = -1;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000B083 File Offset: 0x00009283
		IGatewayObj IEnumerator<IGatewayObj>.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000B08B File Offset: 0x0000928B
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000B098 File Offset: 0x00009298
		public bool MoveNext()
		{
			int num = this.m_currentIndex + 1;
			this.m_currentIndex = num;
			return num < this.m_gatewayList.Count;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000B0C3 File Offset: 0x000092C3
		public void Reset()
		{
			this.m_currentIndex = -1;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00008AEF File Offset: 0x00006CEF
		public void Dispose()
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0000B0CC File Offset: 0x000092CC
		private IGatewayObj Current
		{
			get
			{
				return this.m_gatewayList.GetValue(this.m_currentIndex);
			}
		}

		// Token: 0x04000616 RID: 1558
		private int m_currentIndex;

		// Token: 0x04000617 RID: 1559
		private MODGatewayList<IGatewayObj, GatewayListImpl> m_gatewayList;
	}
}
