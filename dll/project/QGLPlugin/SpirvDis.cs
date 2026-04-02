using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000006 RID: 6
	public class SpirvDis
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002930 File Offset: 0x00000B30
		public unsafe static string GetSpirvDis(byte* spirvData, uint dataSize)
		{
			string spirVDisPath = SpirvDis.GetSpirVDisPath();
			string text = "";
			if (string.IsNullOrEmpty(spirVDisPath))
			{
				return "Error unable to find spirv-dis.exe";
			}
			string tempFileName = Path.GetTempFileName();
			try
			{
				FileStream fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write);
				UnmanagedMemoryStream unmanagedMemoryStream = new UnmanagedMemoryStream(spirvData, (long)((ulong)dataSize));
				unmanagedMemoryStream.CopyTo(fileStream);
				unmanagedMemoryStream.Close();
				fileStream.Close();
			}
			catch (IOException ex)
			{
				Console.WriteLine("Unable to write to tempFile: {0}, {1}", tempFileName, ex.ToString());
				return "Error running spirv-dis.exe";
			}
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = spirVDisPath;
				processStartInfo.Arguments = tempFileName;
				processStartInfo.UseShellExecute = false;
				processStartInfo.CreateNoWindow = true;
				processStartInfo.RedirectStandardOutput = true;
				processStartInfo.RedirectStandardError = true;
				Process process = new Process();
				process.StartInfo = processStartInfo;
				if (process.Start())
				{
					text = process.StandardOutput.ReadToEnd();
					string text2 = ";\\s*Schema: \\d+\\s*\\r\\n$";
					bool flag = Regex.IsMatch(text, text2);
					if (flag && !SpirvDis.isVersionDialogShown)
					{
						string text3 = "Failed to extract the source code for the selected shader.\n";
						string text4 = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.VulkanSDKPath);
						if (!string.IsNullOrEmpty(text4))
						{
							text3 = text3 + "Vulkan SDK used:\n\t " + text4 + "\n";
						}
						text3 += "Please update to the latest <a href=\"https://vulkan.lunarg.com\">Vulkan SDK</a> ";
						new ShowMessageDialogCommand
						{
							Message = text3,
							IconType = IconType.Warning
						}.Execute();
						SpirvDis.isVersionDialogShown = true;
					}
				}
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Error running spir-dis.exe executable: {0}", ex2.ToString());
				text = "spir - dis.exe";
			}
			finally
			{
				try
				{
					File.Delete(tempFileName);
				}
				catch (IOException ex3)
				{
					Console.WriteLine("Unable to delete tempFile: {0}, {1}", tempFileName, ex3.ToString());
				}
			}
			return text;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B3C File Offset: 0x00000D3C
		private static string GetSpirVDisPath()
		{
			string text = SdpApp.ModelManager.SettingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.VulkanSDKPath);
			if (!string.IsNullOrEmpty(text))
			{
				if (!Directory.Exists(text))
				{
					new ShowMessageDialogCommand
					{
						Message = string.Format("Unable to find Vulkan SDK path set at: {0}", text),
						IconType = IconType.Warning
					}.Execute();
				}
				string text2 = Path.Combine(text, "Bin", "spirv-dis.exe");
				if (File.Exists(text2))
				{
					return text2;
				}
				new ShowMessageDialogCommand
				{
					Message = string.Format("Unable to find spirv-dis in {0}", text2),
					IconType = IconType.Warning
				}.Execute();
				return "";
			}
			else
			{
				if (SpirvDis.isDialogShown)
				{
					return "";
				}
				new ShowMessageDialogCommand
				{
					Message = string.Format("To view shader disassembly, set the Vulkan SDK path in Settings -> Capture -> Vulkan SDK path.", Array.Empty<object>()),
					IconType = IconType.Warning
				}.Execute();
				SpirvDis.isDialogShown = true;
				return "";
			}
		}

		// Token: 0x040000E3 RID: 227
		private static bool isDialogShown;

		// Token: 0x040000E4 RID: 228
		private static bool isVersionDialogShown;
	}
}
