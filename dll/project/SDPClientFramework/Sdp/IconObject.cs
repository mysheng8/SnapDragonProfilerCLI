using System;

namespace Sdp
{
	// Token: 0x0200020B RID: 523
	public class IconObject
	{
		// Token: 0x060007BF RID: 1983 RVA: 0x0001542F File Offset: 0x0001362F
		public IconObject(uint numBytes, uint width, uint height, byte[] bytes)
		{
			this.m_numBytes = numBytes;
			this.m_width = width;
			this.m_height = height;
			this.m_bytes = bytes;
			this.m_rowStride = IconObject.BytesPerPixel * this.m_width;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00015466 File Offset: 0x00013666
		public uint Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0001546E File Offset: 0x0001366E
		public uint Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00015476 File Offset: 0x00013676
		public uint NumBytes
		{
			get
			{
				return this.m_numBytes;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0001547E File Offset: 0x0001367E
		public uint RowStride
		{
			get
			{
				return this.m_rowStride;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00015486 File Offset: 0x00013686
		public byte[] Bytes
		{
			get
			{
				return this.m_bytes;
			}
		}

		// Token: 0x04000761 RID: 1889
		public static uint BytesPerPixel = 4U;

		// Token: 0x04000762 RID: 1890
		public static uint BitsPerChannel = 8U;

		// Token: 0x04000763 RID: 1891
		public static bool HasAlpha = true;

		// Token: 0x04000764 RID: 1892
		private uint m_width;

		// Token: 0x04000765 RID: 1893
		private uint m_height;

		// Token: 0x04000766 RID: 1894
		private uint m_numBytes;

		// Token: 0x04000767 RID: 1895
		private byte[] m_bytes;

		// Token: 0x04000768 RID: 1896
		private uint m_rowStride;
	}
}
