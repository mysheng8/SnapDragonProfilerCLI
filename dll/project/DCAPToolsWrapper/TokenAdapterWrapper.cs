using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000038 RID: 56
	public class TokenAdapterWrapper
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0001A10C File Offset: 0x0001830C
		public bool Open(string DCAPFilename)
		{
			if (DCAPFilename == null)
			{
				return false;
			}
			if (!libDCAP.EnsureDCAPFinalized(DCAPFilename))
			{
				this.m_HasTokens = false;
				return false;
			}
			if (new StdioReader(DCAPFilename).Open() != DCAPStatus.DCAPSuccess)
			{
				this.m_HasTokens = false;
				return false;
			}
			this.m_EGLDecoder = new EGLDecoder();
			this.m_GLDecoder = new GLDecoder();
			DataReader dataReader = new StdioReader(DCAPFilename);
			if (dataReader.Open() != DCAPStatus.DCAPSuccess)
			{
				this.m_HasTokens = false;
				return false;
			}
			this.m_CaptureReader = new CaptureFileReader();
			this.m_CaptureReader.Initialize(dataReader);
			this.m_CaptureReader.AddDecoder(this.m_EGLDecoder);
			this.m_CaptureReader.AddDecoder(this.m_GLDecoder);
			this.m_HasTokens = true;
			return true;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001A1B4 File Offset: 0x000183B4
		public void AddEglAdapter(EGLAdapter adapter)
		{
			this.m_EGLDecoder.AddAdapter(adapter);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001A1C2 File Offset: 0x000183C2
		public void RemoveEglAdapter(EGLAdapter adapter)
		{
			this.m_EGLDecoder.RemoveAdapter(adapter);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001A1D0 File Offset: 0x000183D0
		public void AddGlAdapter(GLAdapter adapter)
		{
			this.m_GLDecoder.AddAdapter(adapter);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001A1DE File Offset: 0x000183DE
		public void RemoveGlAdapter(GLAdapter adapter)
		{
			this.m_GLDecoder.RemoveAdapter(adapter);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001A1EC File Offset: 0x000183EC
		public void AddMetaHandler(MetaHandler handler)
		{
			this.m_CaptureReader.AddMetaHandler(handler);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001A1FA File Offset: 0x000183FA
		public void RemoveMetaHandler(MetaHandler handler)
		{
			this.m_CaptureReader.RemoveMetaHandler(handler);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001A208 File Offset: 0x00018408
		public bool NextTokenBlock()
		{
			bool flag = this.m_CaptureReader.ProcessNextBlock();
			this.m_HasTokens = flag;
			return flag;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001A229 File Offset: 0x00018429
		public bool HasTokensRemaining()
		{
			return this.m_HasTokens;
		}

		// Token: 0x04000B9E RID: 2974
		private CaptureFileReader m_CaptureReader;

		// Token: 0x04000B9F RID: 2975
		private EGLDecoder m_EGLDecoder;

		// Token: 0x04000BA0 RID: 2976
		private GLDecoder m_GLDecoder;

		// Token: 0x04000BA1 RID: 2977
		private bool m_HasTokens;
	}
}
