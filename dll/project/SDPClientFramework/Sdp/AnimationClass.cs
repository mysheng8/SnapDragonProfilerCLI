using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000271 RID: 625
	public class AnimationClass
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001E248 File Offset: 0x0001C448
		public AnimationClass(Queue<ImageViewObject> animationObjects, IScreenCaptureView view, uint duration, bool isCaptureType)
		{
			this.View = view;
			this.AnimationObjects = animationObjects;
			this.Duration = duration;
			this.IsCaptureType = isCaptureType;
		}

		// Token: 0x0400087B RID: 2171
		public Queue<ImageViewObject> AnimationObjects;

		// Token: 0x0400087C RID: 2172
		public IScreenCaptureView View;

		// Token: 0x0400087D RID: 2173
		public uint Duration;

		// Token: 0x0400087E RID: 2174
		public bool IsCaptureType;
	}
}
