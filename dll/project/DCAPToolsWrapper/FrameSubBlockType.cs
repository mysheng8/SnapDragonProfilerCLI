using System;

// Token: 0x02000014 RID: 20
public enum FrameSubBlockType
{
	// Token: 0x04000595 RID: 1429
	FrameSubBlockTypeUnknown,
	// Token: 0x04000596 RID: 1430
	FrameSubBlockTypeMetadata,
	// Token: 0x04000597 RID: 1431
	FrameSubBlockTypeMethodCall,
	// Token: 0x04000598 RID: 1432
	FrameSubBlockTypeFunctionCall,
	// Token: 0x04000599 RID: 1433
	FrameSubBlockTypeCompressedMethodCall,
	// Token: 0x0400059A RID: 1434
	FrameSubBlockTypeCompressedFunctionCall,
	// Token: 0x0400059B RID: 1435
	FrameSubBlockTypeLinkSource,
	// Token: 0x0400059C RID: 1436
	FrameSubBlockTypeLinkTarget
}
