using System;
using System.Collections.Generic;

namespace Sdp.Helpers
{
	// Token: 0x02000313 RID: 787
	public class ParametersFormatHelper
	{
		// Token: 0x06001042 RID: 4162 RVA: 0x00033110 File Offset: 0x00031310
		public static string PrintAPIParameter(string name, object value, ParameterType type, List<ParametersFormatHelper.APIParameters> derefParams, bool applyFormatting = false, bool applyLimitOnDerefParamLen = false, bool printDerefParamNames = true)
		{
			int num = 0;
			string text = ParametersFormatHelper.GetParameterString(name, value, type, applyFormatting, false, ref num);
			int num2 = 0;
			if (derefParams.IsNotNullOrEmpty<ParametersFormatHelper.APIParameters>())
			{
				text += ParametersFormatHelper.PrintAPIDerefParameters(derefParams, applyFormatting, applyLimitOnDerefParamLen, printDerefParamNames, ref num2);
			}
			return text;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00033150 File Offset: 0x00031350
		public static string PrintAPIParameters(List<ParametersFormatHelper.APIParameters> args, bool applyFormatting = true)
		{
			string text = "";
			if (args.IsNotNullOrEmpty<ParametersFormatHelper.APIParameters>())
			{
				List<string> list = new List<string>();
				bool flag = true;
				bool flag2 = false;
				foreach (ParametersFormatHelper.APIParameters apiparameters in args)
				{
					string text2 = ParametersFormatHelper.PrintAPIParameter(apiparameters.name, apiparameters.value, apiparameters.type, apiparameters.derefParams, applyFormatting, flag, flag2);
					list.Add(text2);
				}
				text = ParametersFormatHelper.APIPARAMS_BEGIN + string.Join(ParametersFormatHelper.PARAMS_SEPARATOR, list) + ParametersFormatHelper.APIPARAMS_END;
			}
			return text;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x000331FC File Offset: 0x000313FC
		private static string ParamNONE(string t, object o)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("{0}", o);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = {1}", t, o);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003321E File Offset: 0x0003141E
		private static string ParamNUM(string t, object n)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("<span foreground='yellow'>{0}</span>", n);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = <span foreground='yellow'>{1}</span>", t, n);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00033240 File Offset: 0x00031440
		private static string ParamPTR(string t, object p)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("<span foreground='orange'>0x{0:X}</span>", p);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = <span foreground='orange'>0x{1:X}</span>", t, p);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00033262 File Offset: 0x00031462
		private static string ParamHEX(string t, object h)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("<span foreground='#9CDCFE'>0x{0:X}</span>", h);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = <span foreground='#9CDCFE'>0x{1:X}</span>", t, h);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00033284 File Offset: 0x00031484
		private static string ParamENUM(string t, object e)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("<span foreground='#6CBE4E'>{0}</span>", e);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = <span foreground='#6CBE4E'>{1}</span>", t, e);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000332A6 File Offset: 0x000314A6
		private static string ParamSTR(string t, object str)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("<span foreground='#A391DC'>\"{0}\"</span>", str);
			}
			return string.Format("<span foreground='#BBBBBB'><i>{0}</i></span> = <span foreground='#A391DC'>\"{1}\"</span>", t, str);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x000332C8 File Offset: 0x000314C8
		private static string UParamNONE(string t, object o)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("{0}", o);
			}
			return string.Format("{0} = {1}", t, o);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000332C8 File Offset: 0x000314C8
		private static string UParamNUM(string t, object n)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("{0}", n);
			}
			return string.Format("{0} = {1}", t, n);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000332EA File Offset: 0x000314EA
		private static string UParamPTR(string t, object p)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("0x{0:X}", p);
			}
			return string.Format("{0} = 0x{1:X}", t, p);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000332EA File Offset: 0x000314EA
		private static string UParamHEX(string t, object h)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("0x{0:X}", h);
			}
			return string.Format("{0} = 0x{1:X}", t, h);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x000332C8 File Offset: 0x000314C8
		private static string UParamENUM(string t, object e)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("{0}", e);
			}
			return string.Format("{0} = {1}", t, e);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0003330C File Offset: 0x0003150C
		private static string UParamSTR(string t, object str)
		{
			if (string.IsNullOrEmpty(t))
			{
				return string.Format("\"{0}\"", str);
			}
			return string.Format("{0} = \"{1}\"", t, str);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00033330 File Offset: 0x00031530
		private static string GetParameterString(string name, object value, ParameterType type, bool applyFormat, bool applyLimit, ref int unformattedStrLen)
		{
			string text = "";
			switch (type)
			{
			case ParameterType.NONE:
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamNONE(name, value);
					unformattedStrLen += (applyLimit ? ParametersFormatHelper.UParamNONE(name, value).Length : 0);
				}
				else
				{
					text = ParametersFormatHelper.UParamNONE(name, value);
					unformattedStrLen += (applyLimit ? text.Length : 0);
				}
				break;
			case ParameterType.NUM:
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamNUM(name, value);
					unformattedStrLen += (applyLimit ? ParametersFormatHelper.UParamNUM(name, value).Length : 0);
				}
				else
				{
					text = ParametersFormatHelper.UParamNUM(name, value);
					unformattedStrLen += (applyLimit ? text.Length : 0);
				}
				break;
			case ParameterType.PTR:
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamPTR(name, value);
					unformattedStrLen += (applyLimit ? ParametersFormatHelper.UParamPTR(name, value).Length : 0);
				}
				else
				{
					text = ParametersFormatHelper.UParamPTR(name, value);
					unformattedStrLen += (applyLimit ? text.Length : 0);
				}
				break;
			case ParameterType.HEX:
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamHEX(name, value);
					unformattedStrLen += (applyLimit ? ParametersFormatHelper.UParamHEX(name, value).Length : 0);
				}
				else
				{
					text = ParametersFormatHelper.UParamHEX(name, value);
					unformattedStrLen += (applyLimit ? text.Length : 0);
				}
				break;
			case ParameterType.ENUM:
				if (applyLimit)
				{
					int num = ParametersFormatHelper.MAXLEN - ParametersFormatHelper.ENUM_STR_ELLIPSIS.Length;
					if (value.ToString().Length > num)
					{
						value = value.ToString().Substring(0, num) + ParametersFormatHelper.ENUM_STR_ELLIPSIS;
					}
				}
				unformattedStrLen += (applyLimit ? value.ToString().Length : 0);
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamENUM(name, value);
				}
				else
				{
					text = ParametersFormatHelper.UParamENUM(name, value);
				}
				break;
			case ParameterType.STR:
				if (applyLimit)
				{
					int num2 = ParametersFormatHelper.MAXLEN - ParametersFormatHelper.ENUM_STR_ELLIPSIS.Length;
					if (value.ToString().Length > num2)
					{
						value = value.ToString().Substring(0, num2) + ParametersFormatHelper.ENUM_STR_ELLIPSIS;
					}
				}
				unformattedStrLen += (applyLimit ? value.ToString().Length : 0);
				if (applyFormat)
				{
					text = ParametersFormatHelper.ParamSTR(name, value);
				}
				else
				{
					text = ParametersFormatHelper.UParamSTR(name, value);
				}
				break;
			}
			return text;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003356C File Offset: 0x0003176C
		private static string PrintAPIDerefParameters(List<ParametersFormatHelper.APIParameters> args, bool applyFormatting, bool applyLimitOnParamLen, bool printParamNames, ref int unformattedOutputLen)
		{
			string text = "";
			if (args.IsNotNullOrEmpty<ParametersFormatHelper.APIParameters>())
			{
				List<string> list = new List<string>();
				bool flag = false;
				foreach (ParametersFormatHelper.APIParameters apiparameters in args)
				{
					string text2 = (printParamNames ? apiparameters.name : "");
					string text3 = ParametersFormatHelper.GetParameterString(text2, apiparameters.value, apiparameters.type, applyFormatting, applyLimitOnParamLen, ref unformattedOutputLen);
					if (apiparameters.derefParams.IsNotNullOrEmpty<ParametersFormatHelper.APIParameters>())
					{
						text3 += ParametersFormatHelper.PrintAPIDerefParameters(apiparameters.derefParams, applyFormatting, applyLimitOnParamLen, flag, ref unformattedOutputLen);
					}
					if (applyLimitOnParamLen && unformattedOutputLen > ParametersFormatHelper.MAXLEN)
					{
						list.Add(ParametersFormatHelper.PARAMS_ELLIPSIS);
						break;
					}
					list.Add(text3);
					if (applyLimitOnParamLen)
					{
						unformattedOutputLen += ParametersFormatHelper.PARAMS_SEPARATOR.Length;
					}
				}
				text = ParametersFormatHelper.DEREFPARAMS_BEGIN + string.Join(ParametersFormatHelper.PARAMS_SEPARATOR, list) + ParametersFormatHelper.DEREFPARAMS_END;
				if (applyLimitOnParamLen)
				{
					unformattedOutputLen += ParametersFormatHelper.DEREFPARAMS_BEGIN.Length + ParametersFormatHelper.DEREFPARAMS_END.Length - ParametersFormatHelper.PARAMS_SEPARATOR.Length;
				}
			}
			return text;
		}

		// Token: 0x04000AE8 RID: 2792
		private static string APIPARAMS_BEGIN = "( ";

		// Token: 0x04000AE9 RID: 2793
		private static string APIPARAMS_END = " )";

		// Token: 0x04000AEA RID: 2794
		private static string DEREFPARAMS_BEGIN = " [ ";

		// Token: 0x04000AEB RID: 2795
		private static string DEREFPARAMS_END = " ]";

		// Token: 0x04000AEC RID: 2796
		private static string PARAMS_SEPARATOR = ", ";

		// Token: 0x04000AED RID: 2797
		private static string PARAMS_ELLIPSIS = "...";

		// Token: 0x04000AEE RID: 2798
		private static string ENUM_STR_ELLIPSIS = " ...";

		// Token: 0x04000AEF RID: 2799
		private static int MAXLEN = 44;

		// Token: 0x0200040B RID: 1035
		public struct APIParameters
		{
			// Token: 0x06001319 RID: 4889 RVA: 0x0003B901 File Offset: 0x00039B01
			public APIParameters(string _name, ParameterType _type, object _value, List<ParametersFormatHelper.APIParameters> _derefParams = null)
			{
				this.name = _name;
				this.type = _type;
				this.value = _value;
				this.derefParams = _derefParams;
			}

			// Token: 0x04000E15 RID: 3605
			public string name;

			// Token: 0x04000E16 RID: 3606
			public ParameterType type;

			// Token: 0x04000E17 RID: 3607
			public object value;

			// Token: 0x04000E18 RID: 3608
			public List<ParametersFormatHelper.APIParameters> derefParams;
		}
	}
}
