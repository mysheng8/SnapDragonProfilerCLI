using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001DE RID: 478
	public class PropertiesWidgetController
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x0000FE1A File Offset: 0x0000E01A
		public PropertiesWidgetController(IPropertiesWidgetView view)
		{
			this.m_view = view;
			this.m_view.SelectedCategoryChanged += this.m_view_SelectedCategoryChanged;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0000FE4C File Offset: 0x0000E04C
		public bool AddProperty(PropertiesItem item)
		{
			if (item == null)
			{
				return false;
			}
			item.Category = this.TryAddCategory(item.Category);
			this.m_properties[item.Category].Add(item);
			if (string.Compare(item.Category, this.m_view.SelectedCategory) == 0)
			{
				this.InvalidateProperties();
			}
			return true;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0000FEA8 File Offset: 0x0000E0A8
		private void InvalidateProperties()
		{
			this.m_view.ClearProperties();
			if (string.IsNullOrEmpty(this.m_view.SelectedCategory))
			{
				return;
			}
			if (this.m_properties.ContainsKey(this.m_view.SelectedCategory))
			{
				foreach (PropertiesItem propertiesItem in this.m_properties[this.m_view.SelectedCategory])
				{
					this.m_view.AddProperty(propertiesItem);
				}
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0000FF48 File Offset: 0x0000E148
		private string TryAddCategory(string category)
		{
			if (string.IsNullOrEmpty(category))
			{
				category = "Uncategorized";
			}
			if (!this.m_properties.ContainsKey(category))
			{
				this.m_properties.Add(category, new List<PropertiesItem>());
				this.m_view.AddCategory(category);
			}
			return category;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0000FF85 File Offset: 0x0000E185
		private void m_view_SelectedCategoryChanged(object o, EventArgs e)
		{
			this.InvalidateProperties();
		}

		// Token: 0x040006F3 RID: 1779
		private readonly Dictionary<string, List<PropertiesItem>> m_properties = new Dictionary<string, List<PropertiesItem>>();

		// Token: 0x040006F4 RID: 1780
		private IPropertiesWidgetView m_view;
	}
}
