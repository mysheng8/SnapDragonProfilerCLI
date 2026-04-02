using System;

// Token: 0x02000017 RID: 23
public class libDCAP
{
	// Token: 0x0600055B RID: 1371 RVA: 0x000187A0 File Offset: 0x000169A0
	public static ulong GetFileSize(string fileName)
	{
		ulong fileSize = libDCAPPINVOKE.GetFileSize(fileName);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return fileSize;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000187B5 File Offset: 0x000169B5
	public static bool IsFileFinalized(string fileName, ulong size)
	{
		bool flag = libDCAPPINVOKE.IsFileFinalized__SWIG_0(fileName, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000187CB File Offset: 0x000169CB
	public static bool IsFileFinalized(string fileName)
	{
		bool flag = libDCAPPINVOKE.IsFileFinalized__SWIG_1(fileName);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000187E0 File Offset: 0x000169E0
	public static bool IsFileFinalized(DataReader pFileReader, ulong fileSize)
	{
		return libDCAPPINVOKE.IsFileFinalized__SWIG_2(DataReader.getCPtr(pFileReader), fileSize);
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x000187EE File Offset: 0x000169EE
	public static bool IsFinalFrameTrailerNeeded(string fileName, ulong fileSize)
	{
		bool flag = libDCAPPINVOKE.IsFinalFrameTrailerNeeded__SWIG_0(fileName, fileSize);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00018804 File Offset: 0x00016A04
	public static bool IsFinalFrameTrailerNeeded(DataReader pFileReader, ulong fileSize)
	{
		return libDCAPPINVOKE.IsFinalFrameTrailerNeeded__SWIG_1(DataReader.getCPtr(pFileReader), fileSize);
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00018812 File Offset: 0x00016A12
	public static void GenerateFrameIndex(string fileName, SWIGTYPE_p_std__vectorT_Data__FrameEntry_t index)
	{
		libDCAPPINVOKE.GenerateFrameIndex__SWIG_0(fileName, SWIGTYPE_p_std__vectorT_Data__FrameEntry_t.getCPtr(index));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001882D File Offset: 0x00016A2D
	public static void GenerateFrameIndex(DataReader pFileReader, SWIGTYPE_p_std__vectorT_Data__FrameEntry_t index)
	{
		libDCAPPINVOKE.GenerateFrameIndex__SWIG_1(DataReader.getCPtr(pFileReader), SWIGTYPE_p_std__vectorT_Data__FrameEntry_t.getCPtr(index));
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00018840 File Offset: 0x00016A40
	public static void FinalizeFile(string fileName, ulong size, uint frameCount, ulong lastFramePos, bool needsTrailer)
	{
		libDCAPPINVOKE.FinalizeFile__SWIG_0(fileName, size, frameCount, lastFramePos, needsTrailer);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001885A File Offset: 0x00016A5A
	public static void FinalizeFile(DataWriter pFileWriter, ulong fileSize, uint frameCount, ulong lastFramePos, bool needsTrailer)
	{
		libDCAPPINVOKE.FinalizeFile__SWIG_1(DataWriter.getCPtr(pFileWriter), fileSize, frameCount, lastFramePos, needsTrailer);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0001886C File Offset: 0x00016A6C
	public static void EnsureFinalization(string fileName)
	{
		libDCAPPINVOKE.EnsureFinalization(fileName);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00018881 File Offset: 0x00016A81
	public static bool EnsureDCAPFinalized(string DCAPFileName)
	{
		bool flag = libDCAPPINVOKE.EnsureDCAPFinalized(DCAPFileName);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000567 RID: 1383 RVA: 0x00018896 File Offset: 0x00016A96
	public static uint OmitsTextures
	{
		get
		{
			return libDCAPPINVOKE.OmitsTextures_get();
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001889D File Offset: 0x00016A9D
	public static uint OmitsData
	{
		get
		{
			return libDCAPPINVOKE.OmitsData_get();
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000569 RID: 1385 RVA: 0x000188A4 File Offset: 0x00016AA4
	public static uint HaveThreadIdMask
	{
		get
		{
			return libDCAPPINVOKE.HaveThreadIdMask_get();
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x000188AB File Offset: 0x00016AAB
	public static uint HaveTimestampMask
	{
		get
		{
			return libDCAPPINVOKE.HaveTimestampMask_get();
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x000188B2 File Offset: 0x00016AB2
	public static uint DisableFrameTrailerMask
	{
		get
		{
			return libDCAPPINVOKE.DisableFrameTrailerMask_get();
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x000188B9 File Offset: 0x00016AB9
	public static uint BlockSize64BitMask
	{
		get
		{
			return libDCAPPINVOKE.BlockSize64BitMask_get();
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x000188C0 File Offset: 0x00016AC0
	public static uint BlockType32BitMask
	{
		get
		{
			return libDCAPPINVOKE.BlockType32BitMask_get();
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x000188C7 File Offset: 0x00016AC7
	public static uint ObjectId64BitMask
	{
		get
		{
			return libDCAPPINVOKE.ObjectId64BitMask_get();
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600056F RID: 1391 RVA: 0x000188CE File Offset: 0x00016ACE
	public static uint TrimmedFileMask
	{
		get
		{
			return libDCAPPINVOKE.TrimmedFileMask_get();
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000570 RID: 1392 RVA: 0x000188D5 File Offset: 0x00016AD5
	public static uint CompressedBlockDataMask
	{
		get
		{
			return libDCAPPINVOKE.CompressedBlockDataMask_get();
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x000188DC File Offset: 0x00016ADC
	public static uint frameSize64BitMask
	{
		get
		{
			return libDCAPPINVOKE.frameSize64BitMask_get();
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000572 RID: 1394 RVA: 0x000188E3 File Offset: 0x00016AE3
	public static uint StateIntervalMask
	{
		get
		{
			return libDCAPPINVOKE.StateIntervalMask_get();
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000573 RID: 1395 RVA: 0x000188EA File Offset: 0x00016AEA
	public static uint VersionMajorMask
	{
		get
		{
			return libDCAPPINVOKE.VersionMajorMask_get();
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000574 RID: 1396 RVA: 0x000188F1 File Offset: 0x00016AF1
	public static uint VersionMinorMask
	{
		get
		{
			return libDCAPPINVOKE.VersionMinorMask_get();
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x000188F8 File Offset: 0x00016AF8
	public static uint VersionContentRevMask
	{
		get
		{
			return libDCAPPINVOKE.VersionContentRevMask_get();
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000188FF File Offset: 0x00016AFF
	public static SWIGTYPE_p_uint16_t GetFileVersionMajor(uint fileVersion)
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.GetFileVersionMajor(fileVersion), true);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001890D File Offset: 0x00016B0D
	public static SWIGTYPE_p_uint16_t GetFileVersionMinor(uint fileVersion)
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.GetFileVersionMinor(fileVersion), true);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0001891B File Offset: 0x00016B1B
	public static bool IsTextureDataOmitted(uint options)
	{
		return libDCAPPINVOKE.IsTextureDataOmitted(options);
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00018923 File Offset: 0x00016B23
	public static bool IsAllDataOmitted(uint options)
	{
		return libDCAPPINVOKE.IsAllDataOmitted(options);
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001892B File Offset: 0x00016B2B
	public static bool IsThreadIdPresent(uint options)
	{
		return libDCAPPINVOKE.IsThreadIdPresent(options);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00018933 File Offset: 0x00016B33
	public static bool IsTimestampPresent(uint options)
	{
		return libDCAPPINVOKE.IsTimestampPresent(options);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001893B File Offset: 0x00016B3B
	public static bool IsFrameTrailerDisabled(uint options)
	{
		return libDCAPPINVOKE.IsFrameTrailerDisabled(options);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00018943 File Offset: 0x00016B43
	public static bool IsBlockType32Bit(uint options)
	{
		return libDCAPPINVOKE.IsBlockType32Bit(options);
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001894B File Offset: 0x00016B4B
	public static bool IsObjectId64Bit(uint options)
	{
		return libDCAPPINVOKE.IsObjectId64Bit(options);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00018953 File Offset: 0x00016B53
	public static bool IsTrimmedFile(uint options)
	{
		return libDCAPPINVOKE.IsTrimmedFile(options);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001895B File Offset: 0x00016B5B
	public static byte GetStateInterval(uint options)
	{
		return libDCAPPINVOKE.GetStateInterval(options);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00018963 File Offset: 0x00016B63
	public static CompressionAlgorithm GetCompressionAlgorithm(uint options)
	{
		return (CompressionAlgorithm)libDCAPPINVOKE.GetCompressionAlgorithm(options);
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001896B File Offset: 0x00016B6B
	public static bool Is64BitFrameSize(uint options)
	{
		return libDCAPPINVOKE.Is64BitFrameSize(options);
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00018973 File Offset: 0x00016B73
	public static uint SetOmitsTextures(uint options, bool omitted)
	{
		return libDCAPPINVOKE.SetOmitsTextures(options, omitted);
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0001897C File Offset: 0x00016B7C
	public static uint SetOmitsData(uint options, bool omitted)
	{
		return libDCAPPINVOKE.SetOmitsData(options, omitted);
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00018985 File Offset: 0x00016B85
	public static uint SetThreadIdPresent(uint options, bool present)
	{
		return libDCAPPINVOKE.SetThreadIdPresent(options, present);
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001898E File Offset: 0x00016B8E
	public static uint SetTimestampPresent(uint options, bool present)
	{
		return libDCAPPINVOKE.SetTimestampPresent(options, present);
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00018997 File Offset: 0x00016B97
	public static uint SetFrameTrailerDisabled(uint options, bool disabled)
	{
		return libDCAPPINVOKE.SetFrameTrailerDisabled(options, disabled);
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x000189A0 File Offset: 0x00016BA0
	public static uint SetBlockDataCompressed(uint options, CompressionAlgorithm algorithm)
	{
		return libDCAPPINVOKE.SetBlockDataCompressed(options, (int)algorithm);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x000189A9 File Offset: 0x00016BA9
	public static uint SetBlockType32Bit(uint options, bool blockType32)
	{
		return libDCAPPINVOKE.SetBlockType32Bit(options, blockType32);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x000189B2 File Offset: 0x00016BB2
	public static uint SetObjectId64Bit(uint options, bool objectId64)
	{
		return libDCAPPINVOKE.SetObjectId64Bit(options, objectId64);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000189BB File Offset: 0x00016BBB
	public static uint SetTrimmedFile(uint options, bool objectId64)
	{
		return libDCAPPINVOKE.SetTrimmedFile(options, objectId64);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x000189C4 File Offset: 0x00016BC4
	public static uint SetStateInterval(uint options, byte interval)
	{
		return libDCAPPINVOKE.SetStateInterval(options, interval);
	}

	// Token: 0x0400099D RID: 2461
	public static readonly int DCAP_HAVE_GLES = libDCAPPINVOKE.DCAP_HAVE_GLES_get();
}
