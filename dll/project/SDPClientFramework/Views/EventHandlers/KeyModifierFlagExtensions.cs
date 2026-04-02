using System;

namespace SDPClientFramework.Views.EventHandlers
{
	// Token: 0x02000039 RID: 57
	public static class KeyModifierFlagExtensions
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00005D95 File Offset: 0x00003F95
		public static bool HasModifer(this KeyModifierFlag flags, KeyModifierFlag modifier)
		{
			return (flags & modifier) == modifier;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005D9D File Offset: 0x00003F9D
		public static string DisplayName(this KeyModifierFlag flags)
		{
			if (flags.HasModifer(KeyModifierFlag.Shift))
			{
				return "Shift";
			}
			return "";
		}
	}
}
