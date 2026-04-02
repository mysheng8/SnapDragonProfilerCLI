using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Sdp
{
	// Token: 0x02000264 RID: 612
	public class OptionEnum
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001D2DC File Offset: 0x0001B4DC
		private ModuleBuilder MB
		{
			get
			{
				if (OptionEnum.m_mb == null)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					AssemblyName assemblyName = new AssemblyName("TempAssembly");
					AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
					OptionEnum.m_mb = assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");
				}
				return OptionEnum.m_mb;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000204B File Offset: 0x0000024B
		private OptionEnum()
		{
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001D338 File Offset: 0x0001B538
		public OptionEnum(string optionsValueString)
		{
			char[] array = new char[] { ',' };
			string[] array2 = optionsValueString.Split(array);
			int num = int.Parse(array2[0]);
			Type type = OptionEnum.GetType(array2);
			if (type != null)
			{
				this.m_type = type;
			}
			else
			{
				EnumBuilder enumBuilder = this.MB.DefineEnum("OptionEnum" + OptionEnum.m_optionEnumCount++.ToString(), TypeAttributes.Public, typeof(int));
				if (array2.Length > 1)
				{
					for (int i = 1; i < array2.Length; i++)
					{
						enumBuilder.DefineLiteral(array2[i], i - 1);
					}
				}
				this.m_type = enumBuilder.CreateType();
				OptionEnum.m_enumTypes.Add(this.m_type);
			}
			this.m_value = Enum.GetValues(this.m_type).GetValue(num);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001D41F File Offset: 0x0001B61F
		public static OptionEnum Parse(string optionValueString)
		{
			return new OptionEnum(optionValueString);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001D428 File Offset: 0x0001B628
		private static Type GetType(string[] values)
		{
			foreach (Type type in OptionEnum.m_enumTypes)
			{
				string[] names = Enum.GetNames(type);
				if (names.Length == values.Length - 1)
				{
					bool flag = true;
					for (int i = 1; i < values.Length; i++)
					{
						if (names[i - 1] != values[i])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0001D4BC File Offset: 0x0001B6BC
		[Dynamic]
		public dynamic EnumValue
		{
			[return: Dynamic]
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		public Type EnumType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x04000863 RID: 2147
		private static int m_optionEnumCount = 0;

		// Token: 0x04000864 RID: 2148
		private static ModuleBuilder m_mb = null;

		// Token: 0x04000865 RID: 2149
		private static List<Type> m_enumTypes = new List<Type>();

		// Token: 0x04000866 RID: 2150
		private Type m_type;

		// Token: 0x04000867 RID: 2151
		[Dynamic]
		private dynamic m_value;
	}
}
