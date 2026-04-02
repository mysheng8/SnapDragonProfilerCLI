using System;
using System.Collections;
using System.ComponentModel;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x02000201 RID: 513
	public class SdpPropertyDescriptor<T> : PropertyDescriptor
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00014C7F File Offset: 0x00012E7F
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x00014C86 File Offset: 0x00012E86
		public static ILogger Logger { protected get; set; } = new Sdp.Logging.Logger("PropertyGridDescriptors");

		// Token: 0x06000797 RID: 1943 RVA: 0x00014C8E File Offset: 0x00012E8E
		public SdpPropertyDescriptor(string name, Type componentType, T value, string category, string description, bool isReadOnly)
			: base(name, new Attribute[0])
		{
			this.m_name = name;
			this.m_componenType = componentType;
			this.m_value = value;
			this.m_category = category;
			this.m_description = description;
			this.m_isReadOnly = isReadOnly;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00014CCA File Offset: 0x00012ECA
		protected override void FillAttributes(IList attributeList)
		{
			base.FillAttributes(attributeList);
			if (this.m_description != null)
			{
				attributeList.Add(new DescriptionAttribute(this.m_description));
			}
			if (this.m_category != null)
			{
				attributeList.Add(new CategoryAttribute(this.m_category));
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00014D07 File Offset: 0x00012F07
		public override Type ComponentType
		{
			get
			{
				return this.m_componenType;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00014D0F File Offset: 0x00012F0F
		public override bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00014D17 File Offset: 0x00012F17
		public override Type PropertyType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00014D23 File Offset: 0x00012F23
		public override object GetValue(object component)
		{
			return this.m_value;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00014D30 File Offset: 0x00012F30
		public override void SetValue(object component, object value)
		{
			if (value is T)
			{
				PropertyGridValueSetEventArgs propertyGridValueSetEventArgs = new PropertyGridValueSetEventArgs();
				propertyGridValueSetEventArgs.component = component;
				propertyGridValueSetEventArgs.oldValue = this.m_value;
				this.m_value = (T)((object)value);
				this.OnValueChanged(this, propertyGridValueSetEventArgs);
				return;
			}
			SdpPropertyDescriptor<T>.Logger.LogError(string.Format("SetValue call ignored. {0} is type {1} instead of expected type {2}.", value.ToString(), value.GetType().ToString(), this.PropertyType.Name));
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void ResetValue(object component)
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		// Token: 0x04000748 RID: 1864
		protected string m_name;

		// Token: 0x04000749 RID: 1865
		protected string m_category;

		// Token: 0x0400074A RID: 1866
		protected T m_value;

		// Token: 0x0400074B RID: 1867
		protected Type m_componenType;

		// Token: 0x0400074C RID: 1868
		protected bool m_isReadOnly;

		// Token: 0x0400074D RID: 1869
		protected string m_description;
	}
}
