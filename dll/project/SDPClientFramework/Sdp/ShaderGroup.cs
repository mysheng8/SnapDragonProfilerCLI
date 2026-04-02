using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000135 RID: 309
	public class ShaderGroup
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0000A4CD File Offset: 0x000086CD
		public ShaderGroup()
		{
			this.Separable = false;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000A4E7 File Offset: 0x000086E7
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000A4EF File Offset: 0x000086EF
		public int CaptureID { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000A4F8 File Offset: 0x000086F8
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000A500 File Offset: 0x00008700
		public uint ContextID { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000A509 File Offset: 0x00008709
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000A511 File Offset: 0x00008711
		public ulong ResourceID { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000A51A File Offset: 0x0000871A
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000A522 File Offset: 0x00008722
		public string ResourceType { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000A52B File Offset: 0x0000872B
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000A533 File Offset: 0x00008733
		public bool Separable { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000A53C File Offset: 0x0000873C
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000A544 File Offset: 0x00008744
		public bool IsBinary { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000A54D File Offset: 0x0000874D
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000A555 File Offset: 0x00008755
		public bool IsSourceChangeUpdate { get; set; }

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000A560 File Offset: 0x00008760
		public void AddShader(ShaderStage shaderStage, ShaderObject shader)
		{
			if (!this.ShadersList.ContainsKey(shaderStage))
			{
				this.ShadersList.Add(shaderStage, new List<ShaderObject>());
			}
			if (this.ShadersList[shaderStage].Count != 0 && !ShaderGroup.CanShaderStageContainMultipleShaders(shaderStage))
			{
				return;
			}
			this.ShadersList[shaderStage].Add(shader);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000A5BA File Offset: 0x000087BA
		public int Count()
		{
			return this.ShadersList.Count;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000A5C7 File Offset: 0x000087C7
		public bool ContainShaderStage(ShaderStage shaderStage)
		{
			return this.ShadersList.ContainsKey(shaderStage);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000A5D5 File Offset: 0x000087D5
		public ShaderObject GetFirstShader(ShaderStage shaderStage)
		{
			return this.ShadersList[shaderStage][0];
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000A5E9 File Offset: 0x000087E9
		public List<ShaderObject> GetShaders(ShaderStage shaderStage)
		{
			return this.ShadersList[shaderStage];
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000A5F7 File Offset: 0x000087F7
		public Dictionary<ShaderStage, List<ShaderObject>>.KeyCollection GetShaderStages()
		{
			return this.ShadersList.Keys;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000A604 File Offset: 0x00008804
		public static bool CanShaderStageContainMultipleShaders(ShaderStage stage)
		{
			return stage == ShaderStage.RayGen || stage == ShaderStage.AnyHit || stage == ShaderStage.ClosestHit || stage == ShaderStage.Miss || stage == ShaderStage.Intersection || stage == ShaderStage.Callable;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000A624 File Offset: 0x00008824
		public bool CanAddShaderType(ShaderStage stage)
		{
			return ShaderGroup.CanShaderStageContainMultipleShaders(stage) || !this.ShadersList.ContainsKey(stage);
		}

		// Token: 0x04000466 RID: 1126
		private readonly Dictionary<ShaderStage, List<ShaderObject>> ShadersList = new Dictionary<ShaderStage, List<ShaderObject>>();
	}
}
