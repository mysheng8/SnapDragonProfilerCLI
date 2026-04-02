using System;

// Token: 0x02000012 RID: 18
public enum FrameDelim
{
	// Token: 0x04000582 RID: 1410
	FrameDelimPresent = 1,
	// Token: 0x04000583 RID: 1411
	FrameDelimDispatch,
	// Token: 0x04000584 RID: 1412
	FrameDelimClear = 4,
	// Token: 0x04000585 RID: 1413
	FrameDelimDiscard = 8,
	// Token: 0x04000586 RID: 1414
	FrameDelimFlush = 16,
	// Token: 0x04000587 RID: 1415
	FrameDelimFinish = 32,
	// Token: 0x04000588 RID: 1416
	FrameDelimMakeCurrent = 64,
	// Token: 0x04000589 RID: 1417
	FrameDelimLockSurface = 128,
	// Token: 0x0400058A RID: 1418
	FrameDelimUnlockSurface = 256,
	// Token: 0x0400058B RID: 1419
	FrameDelimCopyBuffers = 512,
	// Token: 0x0400058C RID: 1420
	FrameDelimBindApi = 1024,
	// Token: 0x0400058D RID: 1421
	FrameDelimWaitGl = 2048,
	// Token: 0x0400058E RID: 1422
	FrameDelimWaitNative = 4096,
	// Token: 0x0400058F RID: 1423
	FrameDelimWaitClient = 8192,
	// Token: 0x04000590 RID: 1424
	FrameDelimWaitSync = 16384,
	// Token: 0x04000591 RID: 1425
	FrameDelimWaitVBlank = 32768
}
