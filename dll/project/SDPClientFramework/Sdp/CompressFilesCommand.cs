using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Sdp
{
	// Token: 0x0200005F RID: 95
	internal class CompressFilesCommand : Command
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600022E RID: 558 RVA: 0x000078AC File Offset: 0x00005AAC
		// (remove) Token: 0x0600022F RID: 559 RVA: 0x000078E4 File Offset: 0x00005AE4
		public event EventHandler ProgressChanged;

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00007919 File Offset: 0x00005B19
		public double Progress
		{
			get
			{
				return this.m_progress;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00007921 File Offset: 0x00005B21
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00007929 File Offset: 0x00005B29
		public string Output { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00007932 File Offset: 0x00005B32
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000793A File Offset: 0x00005B3A
		public List<string> Files { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00007943 File Offset: 0x00005B43
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000794B File Offset: 0x00005B4B
		public Dictionary<string, byte[]> InMemoryData { get; set; }

		// Token: 0x06000237 RID: 567 RVA: 0x00007954 File Offset: 0x00005B54
		protected override void OnExecute()
		{
			if (string.IsNullOrEmpty(this.Output))
			{
				return;
			}
			long num = 0L;
			long num2 = 0L;
			if (this.Files != null)
			{
				foreach (string text in this.Files)
				{
					try
					{
						FileInfo fileInfo = new FileInfo(text);
						num += fileInfo.Length;
					}
					catch (Exception)
					{
						throw new Exception(string.Format("Error calculating file size for file {0}", text));
					}
				}
			}
			if (this.InMemoryData != null)
			{
				foreach (KeyValuePair<string, byte[]> keyValuePair in this.InMemoryData)
				{
					num += (long)keyValuePair.Value.Length;
				}
			}
			if (num > 0L)
			{
				using (ZipArchive zipArchive = ZipFile.Open(this.Output, ZipArchiveMode.Create))
				{
					if (this.Files != null)
					{
						foreach (string text2 in this.Files)
						{
							try
							{
								using (FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
								{
									byte[] array = new byte[10240];
									ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(Path.GetFileName(text2));
									using (BinaryWriter binaryWriter = new BinaryWriter(zipArchiveEntry.Open()))
									{
										int num3;
										while ((num3 = fileStream.Read(array, 0, array.Length)) > 0)
										{
											binaryWriter.Write(array, 0, num3);
											if (this.ProgressChanged != null)
											{
												num2 += (long)num3;
												this.m_progress = (double)num2 / (double)num;
												this.ProgressChanged(this, EventArgs.Empty);
											}
										}
									}
								}
							}
							catch (IOException)
							{
								throw new Exception(string.Format("Error zipping file: {0}", text2));
							}
						}
					}
					if (this.InMemoryData != null)
					{
						foreach (KeyValuePair<string, byte[]> keyValuePair2 in this.InMemoryData)
						{
							string key = keyValuePair2.Key;
							byte[] value = keyValuePair2.Value;
							ZipArchiveEntry zipArchiveEntry2 = zipArchive.CreateEntry(key);
							try
							{
								using (BinaryWriter binaryWriter2 = new BinaryWriter(zipArchiveEntry2.Open()))
								{
									binaryWriter2.Write(value);
									if (this.ProgressChanged != null)
									{
										num2 += (long)value.Length;
										this.m_progress = (double)num2 / (double)num;
										this.ProgressChanged(this, EventArgs.Empty);
									}
								}
							}
							catch (IOException)
							{
								throw new Exception("Error zipping description");
							}
						}
					}
				}
			}
		}

		// Token: 0x0400017F RID: 383
		private double m_progress;
	}
}
