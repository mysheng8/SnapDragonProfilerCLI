using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Sdp.Localization
{
	// Token: 0x02000303 RID: 771
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class strings
	{
		// Token: 0x06000FD8 RID: 4056 RVA: 0x0000204B File Offset: 0x0000024B
		internal strings()
		{
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00030C90 File Offset: 0x0002EE90
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (strings.resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("Sdp.Localization.strings", typeof(strings).Assembly);
					strings.resourceMan = resourceManager;
				}
				return strings.resourceMan;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00030CC9 File Offset: 0x0002EEC9
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x00030CD0 File Offset: 0x0002EED0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return strings.resourceCulture;
			}
			set
			{
				strings.resourceCulture = value;
			}
		}

		// Token: 0x04000ABF RID: 2751
		private static ResourceManager resourceMan;

		// Token: 0x04000AC0 RID: 2752
		private static CultureInfo resourceCulture;
	}
}
