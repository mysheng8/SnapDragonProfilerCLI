using System;
using System.Diagnostics;
using System.IO;

namespace Sdp
{
	// Token: 0x0200006D RID: 109
	internal class UserGuideCommand : Command
	{
		// Token: 0x06000271 RID: 625 RVA: 0x000084E0 File Offset: 0x000066E0
		protected override void OnExecute()
		{
			string text = "doc" + Path.DirectorySeparatorChar.ToString();
			if (Directory.Exists(text))
			{
				try
				{
					global::System.Diagnostics.Process.Start(text);
					return;
				}
				catch (Exception)
				{
					ShowMessageDialogCommand.ShowErrorDialog("Unable to open Documentation folder.");
					return;
				}
			}
			ShowMessageDialogCommand.ShowErrorDialog("User Guide Folder is missing from install directory.");
		}
	}
}
