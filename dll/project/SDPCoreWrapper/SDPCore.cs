using System;

// Token: 0x0200008B RID: 139
public class SDPCore
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x00014BC4 File Offset: 0x00012DC4
	public static string CLIENT_IP_FILE_NAME
	{
		get
		{
			return SDPCorePINVOKE.CLIENT_IP_FILE_NAME_get();
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600084D RID: 2125 RVA: 0x00014BD8 File Offset: 0x00012DD8
	public static uint PORT_NUMBER
	{
		get
		{
			return SDPCorePINVOKE.PORT_NUMBER_get();
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x00014BEC File Offset: 0x00012DEC
	public static string LOCALHOST_IP
	{
		get
		{
			return SDPCorePINVOKE.LOCALHOST_IP_get();
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x00014C00 File Offset: 0x00012E00
	public static uint SDP_MAX_STRING_LENGTH
	{
		get
		{
			return SDPCorePINVOKE.SDP_MAX_STRING_LENGTH_get();
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x00014C14 File Offset: 0x00012E14
	public static uint SDP_ICON_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.SDP_ICON_WIDTH_get();
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x00014C28 File Offset: 0x00012E28
	public static uint SDP_ICON_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.SDP_ICON_HEIGHT_get();
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x00014C3C File Offset: 0x00012E3C
	public static uint SDP_ICON_SIZE
	{
		get
		{
			return SDPCorePINVOKE.SDP_ICON_SIZE_get();
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000853 RID: 2131 RVA: 0x00014C50 File Offset: 0x00012E50
	public static uint SDP_DEFAULT_CAPTURE
	{
		get
		{
			return SDPCorePINVOKE.SDP_DEFAULT_CAPTURE_get();
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x00014C64 File Offset: 0x00012E64
	public static uint SDP_CAPTURE_UNKNOWN
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURE_UNKNOWN_get();
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x00014C78 File Offset: 0x00012E78
	public static uint SDP_ANY_PID
	{
		get
		{
			return SDPCorePINVOKE.SDP_ANY_PID_get();
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00014C8C File Offset: 0x00012E8C
	public static uint SDP_GLOBAL_PID
	{
		get
		{
			return SDPCorePINVOKE.SDP_GLOBAL_PID_get();
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x00014CA0 File Offset: 0x00012EA0
	public static uint SDP_UNUSED
	{
		get
		{
			return SDPCorePINVOKE.SDP_UNUSED_get();
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x00014CB4 File Offset: 0x00012EB4
	public static string LRZStateMetricName
	{
		get
		{
			return SDPCorePINVOKE.LRZStateMetricName_get();
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x00014CC8 File Offset: 0x00012EC8
	public static uint SDP_USER_DATA_CAPTURE_SUPPORT
	{
		get
		{
			return SDPCorePINVOKE.SDP_USER_DATA_CAPTURE_SUPPORT_get();
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600085A RID: 2138 RVA: 0x00014CDC File Offset: 0x00012EDC
	public static uint SDP_CAPTURETYPE_NONE
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_NONE_get();
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x00014CF0 File Offset: 0x00012EF0
	public static uint SDP_CAPTURETYPE_REALTIME
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_REALTIME_get();
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600085C RID: 2140 RVA: 0x00014D04 File Offset: 0x00012F04
	public static uint SDP_CAPTURETYPE_TRACE
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_TRACE_get();
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600085D RID: 2141 RVA: 0x00014D18 File Offset: 0x00012F18
	public static uint SDP_CAPTURETYPE_SNAPSHOT
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_SNAPSHOT_get();
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x00014D2C File Offset: 0x00012F2C
	public static uint SDP_CAPTURETYPE_SAMPLE
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_SAMPLE_get();
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x0600085F RID: 2143 RVA: 0x00014D40 File Offset: 0x00012F40
	public static uint SDP_CAPTURETYPE_MAX_BIT
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_MAX_BIT_get();
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000860 RID: 2144 RVA: 0x00014D54 File Offset: 0x00012F54
	public static uint SDP_CAPTURETYPE_ALL
	{
		get
		{
			return SDPCorePINVOKE.SDP_CAPTURETYPE_ALL_get();
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000861 RID: 2145 RVA: 0x00014D68 File Offset: 0x00012F68
	public static uint SDP_DATAPROVIDER_ALL
	{
		get
		{
			return SDPCorePINVOKE.SDP_DATAPROVIDER_ALL_get();
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000862 RID: 2146 RVA: 0x00014D7C File Offset: 0x00012F7C
	public static uint SDP_OPTION_ATTR_NONE
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_NONE_get();
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000863 RID: 2147 RVA: 0x00014D90 File Offset: 0x00012F90
	public static uint SDP_OPTION_ATTR_HIDDEN
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_HIDDEN_get();
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000864 RID: 2148 RVA: 0x00014DA4 File Offset: 0x00012FA4
	public static uint SDP_OPTION_ATTR_READONLY
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_READONLY_get();
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000865 RID: 2149 RVA: 0x00014DB8 File Offset: 0x00012FB8
	public static uint SDP_OPTION_ATTR_PROC_INFO
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_PROC_INFO_get();
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000866 RID: 2150 RVA: 0x00014DCC File Offset: 0x00012FCC
	public static uint SDP_OPTION_ATTR_CONTEXT_STATE
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_CONTEXT_STATE_get();
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000867 RID: 2151 RVA: 0x00014DE0 File Offset: 0x00012FE0
	public static uint SDP_OPTION_ATTR_LAUNCH_APP
	{
		get
		{
			return SDPCorePINVOKE.SDP_OPTION_ATTR_LAUNCH_APP_get();
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000868 RID: 2152 RVA: 0x00014DF4 File Offset: 0x00012FF4
	public static uint BUFFER_TYPE_UNKNOWN
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_UNKNOWN_get();
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x00014E08 File Offset: 0x00013008
	public static uint BUFFER_TYPE_PERFETTO_PROTOBUF
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_PERFETTO_PROTOBUF_get();
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600086A RID: 2154 RVA: 0x00014E1C File Offset: 0x0001301C
	public static uint BUFFER_TYPE_SYSTRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_SYSTRACE_DATA_get();
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x00014E30 File Offset: 0x00013030
	public static uint BUFFER_TYPE_GLES_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_DATA_get();
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x0600086C RID: 2156 RVA: 0x00014E44 File Offset: 0x00013044
	public static uint BUFFER_TYPE_GLES_STRIPPED_DCAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_STRIPPED_DCAP_get();
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x00014E58 File Offset: 0x00013058
	public static uint BUFFER_TYPE_GLES_FULL_DCAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_FULL_DCAP_get();
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600086E RID: 2158 RVA: 0x00014E6C File Offset: 0x0001306C
	public static uint BUFFER_TYPE_GLES_SHADER_STAT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_SHADER_STAT_get();
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600086F RID: 2159 RVA: 0x00014E80 File Offset: 0x00013080
	public static uint BUFFER_TYPE_GLES_BUFFER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_BUFFER_DATA_get();
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000870 RID: 2160 RVA: 0x00014E94 File Offset: 0x00013094
	public static uint BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT_get();
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x00014EA8 File Offset: 0x000130A8
	public static uint BUFFER_TYPE_CL_KA_KERNEL_STAT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CL_KA_KERNEL_STAT_get();
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x00014EBC File Offset: 0x000130BC
	public static uint BUFFER_TYPE_QGL_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_DATA_get();
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x00014ED0 File Offset: 0x000130D0
	public static uint BUFFER_TYPE_QGL_STRIPPED_DCAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_STRIPPED_DCAP_get();
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000874 RID: 2164 RVA: 0x00014EE4 File Offset: 0x000130E4
	public static uint BUFFER_TYPE_QGL_FULL_DCAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_FULL_DCAP_get();
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x00014EF8 File Offset: 0x000130F8
	public static uint BUFFER_TYPE_QGL_SHADER_STAT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_SHADER_STAT_get();
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000876 RID: 2166 RVA: 0x00014F0C File Offset: 0x0001310C
	public static uint BUFFER_TYPE_QGL_BUFFER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_BUFFER_DATA_get();
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000877 RID: 2167 RVA: 0x00014F20 File Offset: 0x00013120
	public static uint BUFFER_TYPE_QGL_CAPTURE_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_CAPTURE_SCREENSHOT_get();
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x00014F34 File Offset: 0x00013134
	public static uint BUFFER_TYPE_QGL_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_TRACE_DATA_get();
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x00014F48 File Offset: 0x00013148
	public static uint BUFFER_TYPE_ADSP_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_ADSP_DATA_get();
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x00014F5C File Offset: 0x0001315C
	public static uint BUFFER_TYPE_ADSP_METRIC_EVENT_MAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_ADSP_METRIC_EVENT_MAP_get();
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x0600087B RID: 2171 RVA: 0x00014F70 File Offset: 0x00013170
	public static uint BUFFER_TYPE_SDSP_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_SDSP_DATA_get();
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x00014F84 File Offset: 0x00013184
	public static uint BUFFER_TYPE_SDSP_METRIC_EVENT_MAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_SDSP_METRIC_EVENT_MAP_get();
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x0600087D RID: 2173 RVA: 0x00014F98 File Offset: 0x00013198
	public static uint BUFFER_TYPE_CDSP_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CDSP_DATA_get();
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x00014FAC File Offset: 0x000131AC
	public static uint BUFFER_TYPE_CDSP_METRIC_EVENT_MAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CDSP_METRIC_EVENT_MAP_get();
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x0600087F RID: 2175 RVA: 0x00014FC0 File Offset: 0x000131C0
	public static uint BUFFER_TYPE_VULKAN_REPLAY_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_REPLAY_DATA_get();
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000880 RID: 2176 RVA: 0x00014FD4 File Offset: 0x000131D4
	public static uint BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA_get();
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000881 RID: 2177 RVA: 0x00014FE8 File Offset: 0x000131E8
	public static uint BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA_get();
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x00014FFC File Offset: 0x000131FC
	public static uint BUFFER_TYPE_CL_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CL_TRACE_DATA_get();
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000883 RID: 2179 RVA: 0x00015010 File Offset: 0x00013210
	public static uint BUFFER_TYPE_CL_ROOFLINE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CL_ROOFLINE_DATA_get();
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x00015024 File Offset: 0x00013224
	public static uint BUFFER_TYPE_NPU_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_NPU_DATA_get();
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x00015038 File Offset: 0x00013238
	public static uint BUFFER_TYPE_NPU_METRIC_EVENT_MAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_NPU_METRIC_EVENT_MAP_get();
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001504C File Offset: 0x0001324C
	public static uint BUFFER_TYPE_QGL_CMDBUF_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_CMDBUF_DATA_get();
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000887 RID: 2183 RVA: 0x00015060 File Offset: 0x00013260
	public static uint BUFFER_TYPE_QGL_DATA_BOTH
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_QGL_DATA_BOTH_get();
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000888 RID: 2184 RVA: 0x00015074 File Offset: 0x00013274
	public static uint BUFFER_TYPE_GGPM_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GGPM_TRACE_DATA_get();
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x00015088 File Offset: 0x00013288
	public static uint BUFFER_TYPE_ERROR_MSG
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_ERROR_MSG_get();
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001509C File Offset: 0x0001329C
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_BIT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_BIT_get();
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x000150B0 File Offset: 0x000132B0
	public static uint BUFFER_TYPE_ADRENOINSIGHTBA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_ADRENOINSIGHTBA_get();
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x0600088C RID: 2188 RVA: 0x000150C4 File Offset: 0x000132C4
	public static uint BUFFER_TYPE_ADRENOINSIGHTDCAP
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_ADRENOINSIGHTDCAP_get();
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600088D RID: 2189 RVA: 0x000150D8 File Offset: 0x000132D8
	public static uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA_get();
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600088E RID: 2190 RVA: 0x000150EC File Offset: 0x000132EC
	public static uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_FILE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_FILE_DATA_get();
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x00015100 File Offset: 0x00013300
	public static uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA_get();
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000890 RID: 2192 RVA: 0x00015114 File Offset: 0x00013314
	public static uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_FILE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get();
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00015128 File Offset: 0x00013328
	public static uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF_get();
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000892 RID: 2194 RVA: 0x0001513C File Offset: 0x0001333C
	public static uint BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA_get();
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000893 RID: 2195 RVA: 0x00015150 File Offset: 0x00013350
	public static uint BUFFER_TYPE_VULKAN_PUSH_CONSTANT_REFLECTION
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_PUSH_CONSTANT_REFLECTION_get();
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000894 RID: 2196 RVA: 0x00015164 File Offset: 0x00013364
	public static uint BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA_get();
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000895 RID: 2197 RVA: 0x00015178 File Offset: 0x00013378
	public static uint BUFFER_TYPE_VULKAN_UNIFORM_BUFFER_REFLECTION
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_UNIFORM_BUFFER_REFLECTION_get();
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000896 RID: 2198 RVA: 0x0001518C File Offset: 0x0001338C
	public static uint BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING_get();
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000897 RID: 2199 RVA: 0x000151A0 File Offset: 0x000133A0
	public static uint BUFFER_TYPE_MARKER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_MARKER_DATA_get();
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000898 RID: 2200 RVA: 0x000151B4 File Offset: 0x000133B4
	public static uint BUFFER_TYPE_CPU_PERF_GLOBAL_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CPU_PERF_GLOBAL_DATA_get();
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x000151C8 File Offset: 0x000133C8
	public static uint BUFFER_TYPE_CPU_PERF_PROCESS_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_CPU_PERF_PROCESS_DATA_get();
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x0600089A RID: 2202 RVA: 0x000151DC File Offset: 0x000133DC
	public static uint BUFFER_TYPE_GLES_SHADER_STAT_CSV
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_SHADER_STAT_CSV_get();
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x0600089B RID: 2203 RVA: 0x000151F0 File Offset: 0x000133F0
	public static uint BUFFER_TYPE_GLES_SHADER_DISASM
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_SHADER_DISASM_get();
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x00015204 File Offset: 0x00013404
	public static uint BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA_get();
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x0600089D RID: 2205 RVA: 0x00015218 File Offset: 0x00013418
	public static uint BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA_get();
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600089E RID: 2206 RVA: 0x0001522C File Offset: 0x0001342C
	public static uint BUFFER_TYPE_DX11_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX11_TRACE_DATA_get();
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x00015240 File Offset: 0x00013440
	public static uint BUFFER_TYPE_DX12_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_TRACE_DATA_get();
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00015254 File Offset: 0x00013454
	public static uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_GFXRECONSTRUCT_DATA_get();
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00015268 File Offset: 0x00013468
	public static uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_FILE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_GFXRECONSTRUCT_FILE_DATA_get();
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001527C File Offset: 0x0001347C
	public static uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_OPTIMIZE_FILE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_GFXRECONSTRUCT_OPTIMIZE_FILE_DATA_get();
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00015290 File Offset: 0x00013490
	public static uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_DATA_get();
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000152A4 File Offset: 0x000134A4
	public static uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_FILE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get();
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x000152B8 File Offset: 0x000134B8
	public static uint BUFFER_TYPE_WINCPU_TRACE_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_WINCPU_TRACE_DATA_get();
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060008A6 RID: 2214 RVA: 0x000152CC File Offset: 0x000134CC
	public static uint BUFFER_TYPE_DX12_SNAPSHOT_ATTACHMENTS_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_SNAPSHOT_ATTACHMENTS_DATA_get();
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060008A7 RID: 2215 RVA: 0x000152E0 File Offset: 0x000134E0
	public static uint BUFFER_TYPE_DX12_SNAPSHOT_METRICS_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_SNAPSHOT_METRICS_DATA_get();
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060008A8 RID: 2216 RVA: 0x000152F4 File Offset: 0x000134F4
	public static uint BUFFER_TYPE_DX12_SNAPSHOT_SHADER_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_SNAPSHOT_SHADER_DATA_get();
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00015308 File Offset: 0x00013508
	public static uint BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_API_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_API_DATA_get();
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060008AA RID: 2218 RVA: 0x0001531C File Offset: 0x0001351C
	public static uint BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_METRICS_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_METRICS_DATA_get();
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060008AB RID: 2219 RVA: 0x00015330 File Offset: 0x00013530
	public static uint BUFFER_TYPE_SERVICE_FILE
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_SERVICE_FILE_get();
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060008AC RID: 2220 RVA: 0x00015344 File Offset: 0x00013544
	public static uint BUFFER_TYPE_DEVICE_FILE
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_DEVICE_FILE_get();
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x00015358 File Offset: 0x00013558
	public static uint BUFFER_TYPE_READ_API_FROM_GFXR
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_READ_API_FROM_GFXR_get();
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060008AE RID: 2222 RVA: 0x0001536C File Offset: 0x0001356C
	public static uint BUFFER_TYPE_READ_VULKAN_FROM_GFXR
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_READ_VULKAN_FROM_GFXR_get();
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x00015380 File Offset: 0x00013580
	public static uint BUFFER_TYPE_READ_DX12_FROM_GFXR
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_READ_DX12_FROM_GFXR_get();
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00015394 File Offset: 0x00013594
	public static uint BUFFER_TYPE_SNAPSHOT_FROM_GFXR
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_SNAPSHOT_FROM_GFXR_get();
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x000153A8 File Offset: 0x000135A8
	public static uint BUFFER_TYPE_EXPORT_GFXRECONSTRUCT_DATA
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_EXPORT_GFXRECONSTRUCT_DATA_get();
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000153BC File Offset: 0x000135BC
	public static bool IsBufferCategoryApiTraceData(uint category)
	{
		return SDPCorePINVOKE.IsBufferCategoryApiTraceData(category);
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x000153D4 File Offset: 0x000135D4
	public static uint BUFFER_TYPE_GLES_PLAYBACK_FULLSIZE_BIT
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_FULLSIZE_BIT_get();
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000153E8 File Offset: 0x000135E8
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_THUMB
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_THUMB_get();
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000153FC File Offset: 0x000135FC
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_THUMB
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_THUMB_get();
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00015410 File Offset: 0x00013610
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_THUMB
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_THUMB_get();
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00015424 File Offset: 0x00013624
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_THUMB
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_THUMB_get();
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00015438 File Offset: 0x00013638
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_FULL
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_FULL_get();
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0001544C File Offset: 0x0001364C
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_FULL
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_FULL_get();
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060008BA RID: 2234 RVA: 0x00015460 File Offset: 0x00013660
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_FULL
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_FULL_get();
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060008BB RID: 2235 RVA: 0x00015474 File Offset: 0x00013674
	public static uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_FULL
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_FULL_get();
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060008BC RID: 2236 RVA: 0x00015488 File Offset: 0x00013688
	public static uint BUFFER_ID_GFXRECONSTRUCT_ERROR
	{
		get
		{
			return SDPCorePINVOKE.BUFFER_ID_GFXRECONSTRUCT_ERROR_get();
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001549C File Offset: 0x0001369C
	public static string PLUGINS_DIR
	{
		get
		{
			return SDPCorePINVOKE.PLUGINS_DIR_get();
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x000154B0 File Offset: 0x000136B0
	public static string DATA_PLUGIN_DIR
	{
		get
		{
			return SDPCorePINVOKE.DATA_PLUGIN_DIR_get();
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060008BF RID: 2239 RVA: 0x000154C4 File Offset: 0x000136C4
	public static uint SDPNET_MONITOR_REQ_HASH_FILE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REQ_HASH_FILE_get();
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x000154D8 File Offset: 0x000136D8
	public static uint SDPNET_MONITOR_REP_HASH_FILE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REP_HASH_FILE_get();
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x000154EC File Offset: 0x000136EC
	public static uint SDPNET_MONITOR_START_SERVICE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_START_SERVICE_get();
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00015500 File Offset: 0x00013700
	public static uint SDPNET_MONITOR_STOP_SERVICE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_STOP_SERVICE_get();
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00015514 File Offset: 0x00013714
	public static uint SDPNET_MONITOR_UPDATE_SERVICE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_UPDATE_SERVICE_get();
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00015528 File Offset: 0x00013728
	public static uint SDPNET_MONITOR_REQ_UPDATE_SERVICE_COMPLETE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REQ_UPDATE_SERVICE_COMPLETE_get();
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001553C File Offset: 0x0001373C
	public static uint SDPNET_MONITOR_REP_UPDATE_SERVICE_COMPLETE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REP_UPDATE_SERVICE_COMPLETE_get();
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00015550 File Offset: 0x00013750
	public static uint SDPNET_MONITOR_REGISTER_CLIENT
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REGISTER_CLIENT_get();
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00015564 File Offset: 0x00013764
	public static uint SDPNET_MONITOR_UNINSTALL_SERVICE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_UNINSTALL_SERVICE_get();
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00015578 File Offset: 0x00013778
	public static uint SDPNET_MONITOR_REQ_CHECK_VERSION
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REQ_CHECK_VERSION_get();
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001558C File Offset: 0x0001378C
	public static uint SDPNET_MONITOR_REP_CHECK_VERSION
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_REP_CHECK_VERSION_get();
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x060008CA RID: 2250 RVA: 0x000155A0 File Offset: 0x000137A0
	public static uint SDP_MONITOR_VERSION
	{
		get
		{
			return SDPCorePINVOKE.SDP_MONITOR_VERSION_get();
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x000155B4 File Offset: 0x000137B4
	public static uint SDPNET_BROADCAST
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_BROADCAST_get();
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x000155C8 File Offset: 0x000137C8
	public static uint SDPNET_SERVICE
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_SERVICE_get();
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x000155DC File Offset: 0x000137DC
	public static uint SDPNET_CLIENT
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_CLIENT_get();
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x000155F0 File Offset: 0x000137F0
	public static uint SDPNET_MONITOR_SERVER
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_SERVER_get();
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x00015604 File Offset: 0x00013804
	public static uint SDPNET_ADB_PORT
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_ADB_PORT_get();
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00015618 File Offset: 0x00013818
	public static uint SDPNET_MONITOR_PORT
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_MONITOR_PORT_get();
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001562C File Offset: 0x0001382C
	public static uint SDPNET_DEFAULT_COMMAND_PORT
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_DEFAULT_COMMAND_PORT_get();
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00015640 File Offset: 0x00013840
	public static uint SDPNET_FILE_XFER_PORT_OFFSET
	{
		get
		{
			return SDPCorePINVOKE.SDPNET_FILE_XFER_PORT_OFFSET_get();
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00015654 File Offset: 0x00013854
	public static uint SDP_AUTOGEN_METRIC_ID
	{
		get
		{
			return SDPCorePINVOKE.SDP_AUTOGEN_METRIC_ID_get();
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00015668 File Offset: 0x00013868
	public static uint kInvalidMetricID
	{
		get
		{
			return SDPCorePINVOKE.kInvalidMetricID_get();
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001567C File Offset: 0x0001387C
	public static string OPT_REQUEST
	{
		get
		{
			return SDPCorePINVOKE.OPT_REQUEST_get();
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00015690 File Offset: 0x00013890
	public static string OPT_CAPTURE_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_CAPTURE_ID_get();
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x000156A4 File Offset: 0x000138A4
	public static string OPT_DRAW_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_DRAW_ID_get();
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000156B8 File Offset: 0x000138B8
	public static string OPT_DRAW_MODE
	{
		get
		{
			return SDPCorePINVOKE.OPT_DRAW_MODE_get();
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060008D9 RID: 2265 RVA: 0x000156CC File Offset: 0x000138CC
	public static string OPT_SELECT_TEXTURE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_TEXTURE_get();
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060008DA RID: 2266 RVA: 0x000156E0 File Offset: 0x000138E0
	public static string OPT_SELECT_PROGRAM
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_PROGRAM_get();
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060008DB RID: 2267 RVA: 0x000156F4 File Offset: 0x000138F4
	public static string OPT_SELECT_DRAW_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_DRAW_ID_get();
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060008DC RID: 2268 RVA: 0x00015708 File Offset: 0x00013908
	public static string OPT_ATTACHMENT
	{
		get
		{
			return SDPCorePINVOKE.OPT_ATTACHMENT_get();
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001571C File Offset: 0x0001391C
	public static string OPT_TARGET_SELECTION
	{
		get
		{
			return SDPCorePINVOKE.OPT_TARGET_SELECTION_get();
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x00015730 File Offset: 0x00013930
	public static string OPT_THUMBNAIL_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_THUMBNAIL_WIDTH_get();
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060008DF RID: 2271 RVA: 0x00015744 File Offset: 0x00013944
	public static string OPT_THUMBNAIL_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.OPT_THUMBNAIL_HEIGHT_get();
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00015758 File Offset: 0x00013958
	public static uint DEFAULT_THUMBNAIL_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.DEFAULT_THUMBNAIL_WIDTH_get();
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001576C File Offset: 0x0001396C
	public static uint DEFAULT_THUMBNAIL_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.DEFAULT_THUMBNAIL_HEIGHT_get();
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00015780 File Offset: 0x00013980
	public static string OPT_SELECT_LOCATION_X
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_LOCATION_X_get();
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00015794 File Offset: 0x00013994
	public static string OPT_SELECT_LOCATION_Y
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_LOCATION_Y_get();
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x000157A8 File Offset: 0x000139A8
	public static string OPT_SELECT_LOCATION_VALID
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECT_LOCATION_VALID_get();
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x000157BC File Offset: 0x000139BC
	public static string OPT_HISTORY_LOCATION_VALID
	{
		get
		{
			return SDPCorePINVOKE.OPT_HISTORY_LOCATION_VALID_get();
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000157D0 File Offset: 0x000139D0
	public static string OPT_BUFFER_LIST
	{
		get
		{
			return SDPCorePINVOKE.OPT_BUFFER_LIST_get();
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x000157E4 File Offset: 0x000139E4
	public static string OPT_SELECTED_CONTEXT
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECTED_CONTEXT_get();
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000157F8 File Offset: 0x000139F8
	public static string OPT_RESPONSE
	{
		get
		{
			return SDPCorePINVOKE.OPT_RESPONSE_get();
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001580C File Offset: 0x00013A0C
	public static string OPT_WORK_STARTED
	{
		get
		{
			return SDPCorePINVOKE.OPT_WORK_STARTED_get();
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x00015820 File Offset: 0x00013A20
	public static string OPT_FAILURE
	{
		get
		{
			return SDPCorePINVOKE.OPT_FAILURE_get();
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060008EB RID: 2283 RVA: 0x00015834 File Offset: 0x00013A34
	public static string OPT_FORMAT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORMAT_get();
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x00015848 File Offset: 0x00013A48
	public static string OPT_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_WIDTH_get();
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x0001585C File Offset: 0x00013A5C
	public static string OPT_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.OPT_HEIGHT_get();
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x00015870 File Offset: 0x00013A70
	public static string OPT_TARGET_SELECTED
	{
		get
		{
			return SDPCorePINVOKE.OPT_TARGET_SELECTED_get();
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x00015884 File Offset: 0x00013A84
	public static string OPT_DRAW_ID_SELECTED
	{
		get
		{
			return SDPCorePINVOKE.OPT_DRAW_ID_SELECTED_get();
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00015898 File Offset: 0x00013A98
	public static string OPT_PIXEL_HISTORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_PIXEL_HISTORY_get();
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060008F1 RID: 2289 RVA: 0x000158AC File Offset: 0x00013AAC
	public static string OPT_GL_CONTEXT_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_CONTEXT_STATES_get();
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x000158C0 File Offset: 0x00013AC0
	public static string OPT_SELECTED_CONTEXTS
	{
		get
		{
			return SDPCorePINVOKE.OPT_SELECTED_CONTEXTS_get();
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000158D4 File Offset: 0x00013AD4
	public static string OPT_CONTEXT_OVERRIDE_MAKE_GLOBAL
	{
		get
		{
			return SDPCorePINVOKE.OPT_CONTEXT_OVERRIDE_MAKE_GLOBAL_get();
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x000158E8 File Offset: 0x00013AE8
	public static string OPT_CONTEXT_OVERRIDE_REMOVE
	{
		get
		{
			return SDPCorePINVOKE.OPT_CONTEXT_OVERRIDE_REMOVE_get();
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x000158FC File Offset: 0x00013AFC
	public static string OPT_CONTEXT_OVERRIDE_REMOVE_ALL
	{
		get
		{
			return SDPCorePINVOKE.OPT_CONTEXT_OVERRIDE_REMOVE_ALL_get();
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00015910 File Offset: 0x00013B10
	public static string OPT_UPDATING_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_UPDATING_OPTIONS_get();
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00015924 File Offset: 0x00013B24
	public static string OPT_GENERAL_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_GENERAL_STATES_get();
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00015938 File Offset: 0x00013B38
	public static string OPT_DITHER_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_DITHER_ENABLE_get();
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0001594C File Offset: 0x00013B4C
	public static string OPT_BLEND_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_STATES_get();
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x00015960 File Offset: 0x00013B60
	public static string OPT_BLEND_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_ENABLE_get();
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060008FB RID: 2299 RVA: 0x00015974 File Offset: 0x00013B74
	public static string OPT_BLEND_EQUATION_RGB
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_EQUATION_RGB_get();
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x00015988 File Offset: 0x00013B88
	public static string OPT_BLEND_EQUATION_ALPHA
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_EQUATION_ALPHA_get();
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060008FD RID: 2301 RVA: 0x0001599C File Offset: 0x00013B9C
	public static string OPT_BLEND_FUNC_SRC_RGB
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_FUNC_SRC_RGB_get();
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x000159B0 File Offset: 0x00013BB0
	public static string OPT_BLEND_FUNC_SRC_ALPHA
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_FUNC_SRC_ALPHA_get();
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060008FF RID: 2303 RVA: 0x000159C4 File Offset: 0x00013BC4
	public static string OPT_BLEND_FUNC_DEST_RGB
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_FUNC_DEST_RGB_get();
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x000159D8 File Offset: 0x00013BD8
	public static string OPT_BLEND_FUNC_DEST_ALPHA
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_FUNC_DEST_ALPHA_get();
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x000159EC File Offset: 0x00013BEC
	public static string OPT_BLEND_COLOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_BLEND_COLOR_get();
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x00015A00 File Offset: 0x00013C00
	public static string OPT_CULL_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_CULL_STATES_get();
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x00015A14 File Offset: 0x00013C14
	public static string OPT_CULL_FACE_MODE
	{
		get
		{
			return SDPCorePINVOKE.OPT_CULL_FACE_MODE_get();
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x00015A28 File Offset: 0x00013C28
	public static string OPT_CULL_FACE_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_CULL_FACE_ENABLE_get();
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x00015A3C File Offset: 0x00013C3C
	public static string OPT_DEPTH_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_STATES_get();
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x00015A50 File Offset: 0x00013C50
	public static string OPT_DEPTH_FUNC
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_FUNC_get();
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00015A64 File Offset: 0x00013C64
	public static string OPT_DEPTH_MASK
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_MASK_get();
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x00015A78 File Offset: 0x00013C78
	public static string OPT_DEPTH_TEST_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_TEST_ENABLE_get();
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000909 RID: 2313 RVA: 0x00015A8C File Offset: 0x00013C8C
	public static string OPT_CLEAR_DEPTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_CLEAR_DEPTH_get();
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600090A RID: 2314 RVA: 0x00015AA0 File Offset: 0x00013CA0
	public static string OPT_DEPTH_RANGE_NEAR
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_RANGE_NEAR_get();
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600090B RID: 2315 RVA: 0x00015AB4 File Offset: 0x00013CB4
	public static string OPT_DEPTH_RANGE_FAR
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEPTH_RANGE_FAR_get();
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600090C RID: 2316 RVA: 0x00015AC8 File Offset: 0x00013CC8
	public static string OPT_COLOR_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_COLOR_STATES_get();
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600090D RID: 2317 RVA: 0x00015ADC File Offset: 0x00013CDC
	public static string OPT_CLEAR_COLOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_CLEAR_COLOR_get();
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600090E RID: 2318 RVA: 0x00015AF0 File Offset: 0x00013CF0
	public static string OPT_COLOR_MASK_RED
	{
		get
		{
			return SDPCorePINVOKE.OPT_COLOR_MASK_RED_get();
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x0600090F RID: 2319 RVA: 0x00015B04 File Offset: 0x00013D04
	public static string OPT_COLOR_MASK_GREEN
	{
		get
		{
			return SDPCorePINVOKE.OPT_COLOR_MASK_GREEN_get();
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000910 RID: 2320 RVA: 0x00015B18 File Offset: 0x00013D18
	public static string OPT_COLOR_MASK_BLUE
	{
		get
		{
			return SDPCorePINVOKE.OPT_COLOR_MASK_BLUE_get();
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x00015B2C File Offset: 0x00013D2C
	public static string OPT_COLOR_MASK_ALPHA
	{
		get
		{
			return SDPCorePINVOKE.OPT_COLOR_MASK_ALPHA_get();
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000912 RID: 2322 RVA: 0x00015B40 File Offset: 0x00013D40
	public static string OPT_STENCIL_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_STATES_get();
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x00015B54 File Offset: 0x00013D54
	public static string OPT_CLEAR_STENCIL
	{
		get
		{
			return SDPCorePINVOKE.OPT_CLEAR_STENCIL_get();
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000914 RID: 2324 RVA: 0x00015B68 File Offset: 0x00013D68
	public static string OPT_STENCIL_FUNC_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_FUNC_FRONT_get();
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00015B7C File Offset: 0x00013D7C
	public static string OPT_STENCIL_FAIL_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_FAIL_FRONT_get();
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000916 RID: 2326 RVA: 0x00015B90 File Offset: 0x00013D90
	public static string OPT_STENCIL_DEPTH_FAIL_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_DEPTH_FAIL_FRONT_get();
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x00015BA4 File Offset: 0x00013DA4
	public static string OPT_STENCIL_DEPTH_PASS_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_DEPTH_PASS_FRONT_get();
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x00015BB8 File Offset: 0x00013DB8
	public static string OPT_STENCIL_FUNC_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_FUNC_BACK_get();
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x00015BCC File Offset: 0x00013DCC
	public static string OPT_STENCIL_FAIL_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_FAIL_BACK_get();
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x00015BE0 File Offset: 0x00013DE0
	public static string OPT_STENCIL_DEPTH_FAIL_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_DEPTH_FAIL_BACK_get();
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x00015BF4 File Offset: 0x00013DF4
	public static string OPT_STENCIL_DEPTH_PASS_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_DEPTH_PASS_BACK_get();
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600091C RID: 2332 RVA: 0x00015C08 File Offset: 0x00013E08
	public static string OPT_STENCIL_TEST_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_TEST_ENABLE_get();
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x00015C1C File Offset: 0x00013E1C
	public static string OPT_STENCIL_REFERENCE_VALUE_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_REFERENCE_VALUE_FRONT_get();
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x00015C30 File Offset: 0x00013E30
	public static string OPT_STENCIL_VALUE_MASK_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_VALUE_MASK_FRONT_get();
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x00015C44 File Offset: 0x00013E44
	public static string OPT_STENCIL_WRITE_MASK_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_WRITE_MASK_FRONT_get();
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000920 RID: 2336 RVA: 0x00015C58 File Offset: 0x00013E58
	public static string OPT_STENCIL_REFERENCE_VALUE_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_REFERENCE_VALUE_BACK_get();
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x00015C6C File Offset: 0x00013E6C
	public static string OPT_STENCIL_VALUE_MASK_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_VALUE_MASK_BACK_get();
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00015C80 File Offset: 0x00013E80
	public static string OPT_STENCIL_WRITE_MASK_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_STENCIL_WRITE_MASK_BACK_get();
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00015C94 File Offset: 0x00013E94
	public static string OPT_LINE_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_LINE_STATES_get();
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x00015CA8 File Offset: 0x00013EA8
	public static string OPT_LINE_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_LINE_WIDTH_get();
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x00015CBC File Offset: 0x00013EBC
	public static string OPT_POLYGON_OFFSET_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_POLYGON_OFFSET_STATES_get();
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x00015CD0 File Offset: 0x00013ED0
	public static string OPT_POLYGON_OFFSET_FILL_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_POLYGON_OFFSET_FILL_ENABLE_get();
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x00015CE4 File Offset: 0x00013EE4
	public static string OPT_POLYGONOFFSET_FACTOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_POLYGONOFFSET_FACTOR_get();
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x00015CF8 File Offset: 0x00013EF8
	public static string OPT_POLYGONOFFSET_UNITS
	{
		get
		{
			return SDPCorePINVOKE.OPT_POLYGONOFFSET_UNITS_get();
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x00015D0C File Offset: 0x00013F0C
	public static string OPT_SAMPLE_COVERAGE_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLE_COVERAGE_STATES_get();
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x00015D20 File Offset: 0x00013F20
	public static string OPT_SAMPLECOVERAGE_VALUE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLECOVERAGE_VALUE_get();
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x00015D34 File Offset: 0x00013F34
	public static string OPT_SAMPLECOVERAGE_INVERT
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLECOVERAGE_INVERT_get();
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x0600092C RID: 2348 RVA: 0x00015D48 File Offset: 0x00013F48
	public static string OPT_SAMPLE_ALPHA_TO_COVERAGE_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLE_ALPHA_TO_COVERAGE_ENABLE_get();
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x00015D5C File Offset: 0x00013F5C
	public static string OPT_SAMPLE_COVERAGE_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLE_COVERAGE_ENABLE_get();
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x00015D70 File Offset: 0x00013F70
	public static string OPT_SAMPLE_MASK_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SAMPLE_MASK_ENABLE_get();
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00015D84 File Offset: 0x00013F84
	public static string OPT_SCISSOR_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_STATES_get();
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x00015D98 File Offset: 0x00013F98
	public static string OPT_SCISSOR_TEST_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_TEST_ENABLE_get();
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x00015DAC File Offset: 0x00013FAC
	public static string OPT_SCISSOR_X
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_X_get();
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000932 RID: 2354 RVA: 0x00015DC0 File Offset: 0x00013FC0
	public static string OPT_SCISSOR_Y
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_Y_get();
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000933 RID: 2355 RVA: 0x00015DD4 File Offset: 0x00013FD4
	public static string OPT_SCISSOR_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_WIDTH_get();
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x00015DE8 File Offset: 0x00013FE8
	public static string OPT_SCISSOR_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.OPT_SCISSOR_HEIGHT_get();
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x00015DFC File Offset: 0x00013FFC
	public static string OPT_PIXEL_PACKING
	{
		get
		{
			return SDPCorePINVOKE.OPT_PIXEL_PACKING_get();
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x00015E10 File Offset: 0x00014010
	public static string OPT_PIXEL_STORE_PACKALIGNMENT
	{
		get
		{
			return SDPCorePINVOKE.OPT_PIXEL_STORE_PACKALIGNMENT_get();
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00015E24 File Offset: 0x00014024
	public static string OPT_PIXEL_STORE_PACKUNALIGNMENT
	{
		get
		{
			return SDPCorePINVOKE.OPT_PIXEL_STORE_PACKUNALIGNMENT_get();
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x00015E38 File Offset: 0x00014038
	public static string OPT_ACTIVE_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_ACTIVE_STATES_get();
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x00015E4C File Offset: 0x0001404C
	public static string OPT_ACTIVE_PROGRAM
	{
		get
		{
			return SDPCorePINVOKE.OPT_ACTIVE_PROGRAM_get();
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x00015E60 File Offset: 0x00014060
	public static string OPT_ACTIVE_PIPELINE
	{
		get
		{
			return SDPCorePINVOKE.OPT_ACTIVE_PIPELINE_get();
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x00015E74 File Offset: 0x00014074
	public static string OPT_ACTIVE_TEXTURE_UNIT
	{
		get
		{
			return SDPCorePINVOKE.OPT_ACTIVE_TEXTURE_UNIT_get();
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x0600093C RID: 2364 RVA: 0x00015E88 File Offset: 0x00014088
	public static string OPT_VIEWPORT_STATES
	{
		get
		{
			return SDPCorePINVOKE.OPT_VIEWPORT_STATES_get();
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x00015E9C File Offset: 0x0001409C
	public static string OPT_VIEWPORT_X
	{
		get
		{
			return SDPCorePINVOKE.OPT_VIEWPORT_X_get();
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x0600093E RID: 2366 RVA: 0x00015EB0 File Offset: 0x000140B0
	public static string OPT_VIEWPORT_Y
	{
		get
		{
			return SDPCorePINVOKE.OPT_VIEWPORT_Y_get();
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x00015EC4 File Offset: 0x000140C4
	public static string OPT_VIEWPORT_WIDTH
	{
		get
		{
			return SDPCorePINVOKE.OPT_VIEWPORT_WIDTH_get();
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x00015ED8 File Offset: 0x000140D8
	public static string OPT_VIEWPORT_HEIGHT
	{
		get
		{
			return SDPCorePINVOKE.OPT_VIEWPORT_HEIGHT_get();
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000941 RID: 2369 RVA: 0x00015EEC File Offset: 0x000140EC
	public static string OPT_PROGRAM_OVERRIDE
	{
		get
		{
			return SDPCorePINVOKE.OPT_PROGRAM_OVERRIDE_get();
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x00015F00 File Offset: 0x00014100
	public static string OPT_OPTION_STRUCT_DEF
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPTION_STRUCT_DEF_get();
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000943 RID: 2371 RVA: 0x00015F14 File Offset: 0x00014114
	public static string OPT_CONTEXT_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_CONTEXT_ID_get();
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000944 RID: 2372 RVA: 0x00015F28 File Offset: 0x00014128
	public static string OPT_PROGRAM_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_PROGRAM_ID_get();
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x00015F3C File Offset: 0x0001413C
	public static string OPT_ENABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_ENABLE_get();
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x00015F50 File Offset: 0x00014150
	public static string OPT_PROGRAM_SEPARABLE
	{
		get
		{
			return SDPCorePINVOKE.OPT_PROGRAM_SEPARABLE_get();
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000947 RID: 2375 RVA: 0x00015F64 File Offset: 0x00014164
	public static string OPT_VERTEX_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_VERTEX_SHADER_get();
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000948 RID: 2376 RVA: 0x00015F78 File Offset: 0x00014178
	public static string OPT_FRAGMENT_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_FRAGMENT_SHADER_get();
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x00015F8C File Offset: 0x0001418C
	public static string OPT_COMPUTE_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_COMPUTE_SHADER_get();
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x00015FA0 File Offset: 0x000141A0
	public static string OPT_GEOMETRY_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_GEOMETRY_SHADER_get();
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x0600094B RID: 2379 RVA: 0x00015FB4 File Offset: 0x000141B4
	public static string OPT_TESSCTRL_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_TESSCTRL_SHADER_get();
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x00015FC8 File Offset: 0x000141C8
	public static string OPT_TESSEVAL_SHADER
	{
		get
		{
			return SDPCorePINVOKE.OPT_TESSEVAL_SHADER_get();
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x0600094D RID: 2381 RVA: 0x00015FDC File Offset: 0x000141DC
	public static string OPT_DRAWCALL_SKIP_LIST
	{
		get
		{
			return SDPCorePINVOKE.OPT_DRAWCALL_SKIP_LIST_get();
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x0600094E RID: 2382 RVA: 0x00015FF0 File Offset: 0x000141F0
	public static string OPT_GL_DEBUG_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_DEBUG_OPTIONS_get();
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x0600094F RID: 2383 RVA: 0x00016004 File Offset: 0x00014204
	public static string OPT_DEBUG_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_CATEGORY_get();
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x00016018 File Offset: 0x00014218
	public static string OPT_DEBUG_ENABLE_CONTEXT_SELECTION_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_CONTEXT_SELECTION_OUTPUT_get();
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000951 RID: 2385 RVA: 0x0001602C File Offset: 0x0001422C
	public static string OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_get();
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00016040 File Offset: 0x00014240
	public static string OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_DEBUG_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_DEBUG_OUTPUT_get();
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000953 RID: 2387 RVA: 0x00016054 File Offset: 0x00014254
	public static string OPT_DEBUG_ENABLE_OVERRIDE_TOKENS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_OVERRIDE_TOKENS_get();
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x00016068 File Offset: 0x00014268
	public static string OPT_DEBUG_ENABLE_REPORTING_TOKENS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_REPORTING_TOKENS_get();
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x0001607C File Offset: 0x0001427C
	public static string OPT_DEBUG_ENABLE_REPORTING_TOKEN_DEBUG_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_REPORTING_TOKEN_DEBUG_OUTPUT_get();
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x00016090 File Offset: 0x00014290
	public static string OPT_DEBUG_ENABLE_SCREENSHOT_TOKEN_DEBUG_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_SCREENSHOT_TOKEN_DEBUG_OUTPUT_get();
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000957 RID: 2391 RVA: 0x000160A4 File Offset: 0x000142A4
	public static string OPT_DEBUG_ENABLE_TOKEN_DEBUG_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_TOKEN_DEBUG_OUTPUT_get();
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x000160B8 File Offset: 0x000142B8
	public static string OPT_DEBUG_ENABLE_THUMBNAIL_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_THUMBNAIL_SCREENSHOT_get();
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x000160CC File Offset: 0x000142CC
	public static string OPT_DEBUG_THUMBNAIL_SHRINK_FACTOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_THUMBNAIL_SHRINK_FACTOR_get();
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x0600095A RID: 2394 RVA: 0x000160E0 File Offset: 0x000142E0
	public static string OPT_DEBUG_PIXEL_HISTORY_RADIUS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_PIXEL_HISTORY_RADIUS_get();
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x000160F4 File Offset: 0x000142F4
	public static string OPT_DEBUG_ENABLE_HIGHLIGHT_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_HIGHLIGHT_SCREENSHOT_get();
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x0600095C RID: 2396 RVA: 0x00016108 File Offset: 0x00014308
	public static string OPT_DEBUG_ENABLE_DEPTHSTENCIL_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_DEPTHSTENCIL_SCREENSHOT_get();
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x0001611C File Offset: 0x0001431C
	public static string OPT_DEBUG_ENABLE_FULL_SCREENSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_FULL_SCREENSHOT_get();
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x0600095E RID: 2398 RVA: 0x00016130 File Offset: 0x00014330
	public static string OPT_DEBUG_ENABLE_FULL_DCAP
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_FULL_DCAP_get();
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x00016144 File Offset: 0x00014344
	public static string OPT_DEBUG_TEST_PIXEL_HISTORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_TEST_PIXEL_HISTORY_get();
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000960 RID: 2400 RVA: 0x00016158 File Offset: 0x00014358
	public static string OPT_DEBUG_ENABLE_OPTION_CHANGE_DEBUG_OUTPUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_OPTION_CHANGE_DEBUG_OUTPUT_get();
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x0001616C File Offset: 0x0001436C
	public static string OPT_DEBUG_ENABLE_BUFFER_ON_DEMAND
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_BUFFER_ON_DEMAND_get();
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000962 RID: 2402 RVA: 0x00016180 File Offset: 0x00014380
	public static string OPT_DEBUG_ENABLE_CLIENT_REPLAY_CACHING
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ENABLE_CLIENT_REPLAY_CACHING_get();
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000963 RID: 2403 RVA: 0x00016194 File Offset: 0x00014394
	public static string OPT_DEBUG_DISABLE_SNAPSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_DISABLE_SNAPSHOT_get();
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000964 RID: 2404 RVA: 0x000161A8 File Offset: 0x000143A8
	public static string OPT_DEBUG_DISABLE_BINARY_SHADERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_DISABLE_BINARY_SHADERS_get();
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000965 RID: 2405 RVA: 0x000161BC File Offset: 0x000143BC
	public static string OPT_DEBUG_ALLOW_PROGRAM_OVERRIDES
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ALLOW_PROGRAM_OVERRIDES_get();
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000966 RID: 2406 RVA: 0x000161D0 File Offset: 0x000143D0
	public static string OPT_DEBUG_ALLOW_STD_DELIMITERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ALLOW_STD_DELIMITERS_get();
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000967 RID: 2407 RVA: 0x000161E4 File Offset: 0x000143E4
	public static string OPT_DEBUG_ALLOW_DRAWCALL_DELIMITERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_ALLOW_DRAWCALL_DELIMITERS_get();
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000968 RID: 2408 RVA: 0x000161F8 File Offset: 0x000143F8
	public static string OPT_DEBUG_USE_LOCAL_SNAPSHOT_FILE
	{
		get
		{
			return SDPCorePINVOKE.OPT_DEBUG_USE_LOCAL_SNAPSHOT_FILE_get();
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001620C File Offset: 0x0001440C
	public static string OPT_GLES_INFORMATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_GLES_INFORMATION_get();
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x0600096A RID: 2410 RVA: 0x00016220 File Offset: 0x00014420
	public static string OPT_INFO_EGL_VENDOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_EGL_VENDOR_get();
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x00016234 File Offset: 0x00014434
	public static string OPT_INFO_EGL_VERSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_EGL_VERSION_get();
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x0600096C RID: 2412 RVA: 0x00016248 File Offset: 0x00014448
	public static string OPT_INFO_GL_VENDOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_GL_VENDOR_get();
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x0001625C File Offset: 0x0001445C
	public static string OPT_INFO_GL_RENDERER
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_GL_RENDERER_get();
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x0600096E RID: 2414 RVA: 0x00016270 File Offset: 0x00014470
	public static string OPT_INFO_GL_VERSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_GL_VERSION_get();
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x00016284 File Offset: 0x00014484
	public static string OPT_INFO_GLSL_VERSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_INFO_GLSL_VERSION_get();
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000970 RID: 2416 RVA: 0x00016298 File Offset: 0x00014498
	public static string OPT_APPLICATION_INFORMATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_APPLICATION_INFORMATION_get();
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x000162AC File Offset: 0x000144AC
	public static string OPT_GENERAL_INFORMATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_GENERAL_INFORMATION_get();
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000972 RID: 2418 RVA: 0x000162C0 File Offset: 0x000144C0
	public static string OPT_APPLICATION_NAME
	{
		get
		{
			return SDPCorePINVOKE.OPT_APPLICATION_NAME_get();
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x000162D4 File Offset: 0x000144D4
	public static string OPT_APPLICATION_PROCESS_ID
	{
		get
		{
			return SDPCorePINVOKE.OPT_APPLICATION_PROCESS_ID_get();
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x000162E8 File Offset: 0x000144E8
	public static string OPT_GPU_IDENTIFIER
	{
		get
		{
			return SDPCorePINVOKE.OPT_GPU_IDENTIFIER_get();
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000975 RID: 2421 RVA: 0x000162FC File Offset: 0x000144FC
	public static string OPT_ADAPTER_DESCRIPTION
	{
		get
		{
			return SDPCorePINVOKE.OPT_ADAPTER_DESCRIPTION_get();
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000976 RID: 2422 RVA: 0x00016310 File Offset: 0x00014510
	public static string OPT_GL_CAPABILITY_INFORMATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_CAPABILITY_INFORMATION_get();
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x00016324 File Offset: 0x00014524
	public static string OPT_CAN_CANCEL_GL_SNAPSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_CAN_CANCEL_GL_SNAPSHOT_get();
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000978 RID: 2424 RVA: 0x00016338 File Offset: 0x00014538
	public static string OPT_CPU_METRICS
	{
		get
		{
			return SDPCorePINVOKE.OPT_CPU_METRICS_get();
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001634C File Offset: 0x0001454C
	public static string OPT_CPU_CATEGORY_SAMPLING
	{
		get
		{
			return SDPCorePINVOKE.OPT_CPU_CATEGORY_SAMPLING_get();
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x00016360 File Offset: 0x00014560
	public static string OPT_CPU_REALTIME_UPDATE_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_CPU_REALTIME_UPDATE_PERIOD_get();
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x00016374 File Offset: 0x00014574
	public static string OPT_CPU_TRACE_UPDATE_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_CPU_TRACE_UPDATE_PERIOD_get();
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600097C RID: 2428 RVA: 0x00016388 File Offset: 0x00014588
	public static string OPT_GGPM_GPU_METRICS
	{
		get
		{
			return SDPCorePINVOKE.OPT_GGPM_GPU_METRICS_get();
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0001639C File Offset: 0x0001459C
	public static string OPT_QGL_DISABLE_PLAYBACK_OPT
	{
		get
		{
			return SDPCorePINVOKE.OPT_QGL_DISABLE_PLAYBACK_OPT_get();
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600097E RID: 2430 RVA: 0x000163B0 File Offset: 0x000145B0
	public static string OPT_CATEGORY_SAMPLING
	{
		get
		{
			return SDPCorePINVOKE.OPT_CATEGORY_SAMPLING_get();
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x000163C4 File Offset: 0x000145C4
	public static string OPT_REALTIME_UPDATE_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_REALTIME_UPDATE_PERIOD_get();
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000980 RID: 2432 RVA: 0x000163D8 File Offset: 0x000145D8
	public static string OPT_TRACE_UPDATE_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_TRACE_UPDATE_PERIOD_get();
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x000163EC File Offset: 0x000145EC
	public static string OPT_OPENCL_CAPTURE_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENCL_CAPTURE_OPTIONS_get();
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x00016400 File Offset: 0x00014600
	public static string OPT_ENABLE_BUFFER_IMAGE_TRANSMISSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_ENABLE_BUFFER_IMAGE_TRANSMISSION_get();
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x00016414 File Offset: 0x00014614
	public static string OPT_TERMINATE_ON_RELEASE_CONTEXT
	{
		get
		{
			return SDPCorePINVOKE.OPT_TERMINATE_ON_RELEASE_CONTEXT_get();
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x00016428 File Offset: 0x00014628
	public static string OPT_ENABLE_BLOCKING
	{
		get
		{
			return SDPCorePINVOKE.OPT_ENABLE_BLOCKING_get();
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001643C File Offset: 0x0001463C
	public static string OPT_LAUNCH_SUSPENDED_OPENCL
	{
		get
		{
			return SDPCorePINVOKE.OPT_LAUNCH_SUSPENDED_OPENCL_get();
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x00016450 File Offset: 0x00014650
	public static string OPT_OPENCL_INFORMATION
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENCL_INFORMATION_get();
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x00016464 File Offset: 0x00014664
	public static string OPT_OPENCL_DRIVER_VERSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENCL_DRIVER_VERSION_get();
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x00016478 File Offset: 0x00014678
	public static string OPT_CL_VENDOR
	{
		get
		{
			return SDPCorePINVOKE.OPT_CL_VENDOR_get();
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001648C File Offset: 0x0001468C
	public static string OPT_CL_VERSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_CL_VERSION_get();
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x000164A0 File Offset: 0x000146A0
	public static string OPT_REQUEST_KERNEL_STATS
	{
		get
		{
			return SDPCorePINVOKE.OPT_REQUEST_KERNEL_STATS_get();
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x000164B4 File Offset: 0x000146B4
	public static string OPT_KERNEL_ANALYZER_SEND_DISASM
	{
		get
		{
			return SDPCorePINVOKE.OPT_KERNEL_ANALYZER_SEND_DISASM_get();
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x000164C8 File Offset: 0x000146C8
	public static string OPT_KERNEL_ANALYZER_SEND_DEBUG_INFO
	{
		get
		{
			return SDPCorePINVOKE.OPT_KERNEL_ANALYZER_SEND_DEBUG_INFO_get();
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x000164DC File Offset: 0x000146DC
	public static string OPT_LAUNCH_KERNEL_ANALYZER
	{
		get
		{
			return SDPCorePINVOKE.OPT_LAUNCH_KERNEL_ANALYZER_get();
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x0600098E RID: 2446 RVA: 0x000164F0 File Offset: 0x000146F0
	public static string OPT_SNAPSHOT_FROM_GFXR_FILE
	{
		get
		{
			return SDPCorePINVOKE.OPT_SNAPSHOT_FROM_GFXR_FILE_get();
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x00016504 File Offset: 0x00014704
	public static string OPT_VULKAN_SNAPSHOT_FROM_SESSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_SNAPSHOT_FROM_SESSION_get();
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x00016518 File Offset: 0x00014718
	public static string OPT_VULKAN_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_CATEGORY_get();
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001652C File Offset: 0x0001472C
	public static string OPT_VULKAN_REPLAY_REQUEST
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_REPLAY_REQUEST_get();
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000992 RID: 2450 RVA: 0x00016540 File Offset: 0x00014740
	public static string OPT_VULKAN_REALTIME_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_REALTIME_OPTIONS_get();
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x00016554 File Offset: 0x00014754
	public static string OPT_FRAME_TIME_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_FRAME_TIME_PERIOD_get();
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x00016568 File Offset: 0x00014768
	public static string OPT_VULKAN_SHADER_PROFILING_SUPPORT
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_SHADER_PROFILING_SUPPORT_get();
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001657C File Offset: 0x0001477C
	public static string OPT_VULKAN_SNAPSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_SNAPSHOT_get();
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x00016590 File Offset: 0x00014790
	public static string OPT_VULKAN_LAUNCH_SUSPENDED
	{
		get
		{
			return SDPCorePINVOKE.OPT_VULKAN_LAUNCH_SUSPENDED_get();
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x000165A4 File Offset: 0x000147A4
	public static string OPT_DX11_REALTIME_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DX11_REALTIME_OPTIONS_get();
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000998 RID: 2456 RVA: 0x000165B8 File Offset: 0x000147B8
	public static string OPT_DX12_SNAPSHOT
	{
		get
		{
			return SDPCorePINVOKE.OPT_DX12_SNAPSHOT_get();
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x000165CC File Offset: 0x000147CC
	public static string OPT_ENABLE_OPTIMIZATION_OPT
	{
		get
		{
			return SDPCorePINVOKE.OPT_ENABLE_OPTIMIZATION_OPT_get();
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x0600099A RID: 2458 RVA: 0x000165E0 File Offset: 0x000147E0
	public static string OPT_DX12_SNAPSHOT_FROM_SESSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_DX12_SNAPSHOT_FROM_SESSION_get();
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x000165F4 File Offset: 0x000147F4
	public static string OPT_API_OPTION_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.OPT_API_OPTION_CATEGORY_NAME_get();
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x00016608 File Offset: 0x00014808
	public static string OPT_DX12_REPLAY_REQUEST
	{
		get
		{
			return SDPCorePINVOKE.OPT_DX12_REPLAY_REQUEST_get();
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001661C File Offset: 0x0001481C
	public static string OPT_DX12_LAUNCH_SUSPENDED
	{
		get
		{
			return SDPCorePINVOKE.OPT_DX12_LAUNCH_SUSPENDED_get();
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x00016630 File Offset: 0x00014830
	public static string OPT_OPENGL_GENERAL_OPTIONS
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENGL_GENERAL_OPTIONS_get();
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x00016644 File Offset: 0x00014844
	public static string OPT_OPENGL_SNAPSHOT_FROM_SESSION
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENGL_SNAPSHOT_FROM_SESSION_get();
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00016658 File Offset: 0x00014858
	public static string OPT_SHADER_STATS_REQUEST
	{
		get
		{
			return SDPCorePINVOKE.OPT_SHADER_STATS_REQUEST_get();
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0001666C File Offset: 0x0001486C
	public static string OPT_OPENGL_GENERAL_OVERRIDES
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENGL_GENERAL_OVERRIDES_get();
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00016680 File Offset: 0x00014880
	public static string OPT_USE_AUTO_FRAME_DELIMITERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_USE_AUTO_FRAME_DELIMITERS_get();
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00016694 File Offset: 0x00014894
	public static string OPT_FRAME_DELIMITER_PERIOD
	{
		get
		{
			return SDPCorePINVOKE.OPT_FRAME_DELIMITER_PERIOD_get();
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x000166A8 File Offset: 0x000148A8
	public static string OPT_NUMBER_OF_CAPTURE_FRAMES
	{
		get
		{
			return SDPCorePINVOKE.OPT_NUMBER_OF_CAPTURE_FRAMES_get();
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060009A5 RID: 2469 RVA: 0x000166BC File Offset: 0x000148BC
	public static string OPT_CAPTURE_TIME
	{
		get
		{
			return SDPCorePINVOKE.OPT_CAPTURE_TIME_get();
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000166D0 File Offset: 0x000148D0
	public static string OPT_CAPTURE_TIMEOUT
	{
		get
		{
			return SDPCorePINVOKE.OPT_CAPTURE_TIMEOUT_get();
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060009A7 RID: 2471 RVA: 0x000166E4 File Offset: 0x000148E4
	public static string OPT_GL_REALTIME_OVERRIDES
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_REALTIME_OVERRIDES_get();
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060009A8 RID: 2472 RVA: 0x000166F8 File Offset: 0x000148F8
	public static string OPT_EGL_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001670C File Offset: 0x0001490C
	public static string OPT_DISABLE_EGL_CALLS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_EGL_CALLS_get();
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x00016720 File Offset: 0x00014920
	public static string OPT_DISABLE_EGLSWAPBUFFERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_EGLSWAPBUFFERS_get();
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x00016734 File Offset: 0x00014934
	public static string OPT_FINISH_ON_SWAP
	{
		get
		{
			return SDPCorePINVOKE.OPT_FINISH_ON_SWAP_get();
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060009AC RID: 2476 RVA: 0x00016748 File Offset: 0x00014948
	public static string OPT_GL_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001675C File Offset: 0x0001495C
	public static string OPT_DISABLE_GL
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_GL_get();
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x060009AE RID: 2478 RVA: 0x00016770 File Offset: 0x00014970
	public static string OPT_DISABLE_DRAW_ARRAYS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_DRAW_ARRAYS_get();
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060009AF RID: 2479 RVA: 0x00016784 File Offset: 0x00014984
	public static string OPT_DISABLE_DRAW_ELEMENTS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_DRAW_ELEMENTS_get();
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00016798 File Offset: 0x00014998
	public static string OPT_DISABLE_READ_PIXELS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_READ_PIXELS_get();
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060009B1 RID: 2481 RVA: 0x000167AC File Offset: 0x000149AC
	public static string OPT_DISABLE_FINISH
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_FINISH_get();
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x000167C0 File Offset: 0x000149C0
	public static string OPT_DISABLE_FLUSH
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_FLUSH_get();
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060009B3 RID: 2483 RVA: 0x000167D4 File Offset: 0x000149D4
	public static string OPT_DISABLE_COLOR_CLEARS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_COLOR_CLEARS_get();
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x000167E8 File Offset: 0x000149E8
	public static string OPT_DISABLE_DEPTH_CLEARS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_DEPTH_CLEARS_get();
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x000167FC File Offset: 0x000149FC
	public static string OPT_DISABLE_STENCIL_CLEARS
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_STENCIL_CLEARS_get();
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00016810 File Offset: 0x00014A10
	public static string OPT_GL_VERTEX_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_VERTEX_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00016824 File Offset: 0x00014A24
	public static string OPT_DISABLE_VBO_RENDERING
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_VBO_RENDERING_get();
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00016838 File Offset: 0x00014A38
	public static string OPT_DISABLE_NON_VBO_RENDERING
	{
		get
		{
			return SDPCorePINVOKE.OPT_DISABLE_NON_VBO_RENDERING_get();
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0001684C File Offset: 0x00014A4C
	public static string OPT_FORCE_CULLING_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_CULLING_OFF_get();
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x00016860 File Offset: 0x00014A60
	public static string OPT_FORCE_CULLING_REJECT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_CULLING_REJECT_get();
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x00016874 File Offset: 0x00014A74
	public static string OPT_FORCE_CULLING_BACK
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_CULLING_BACK_get();
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x00016888 File Offset: 0x00014A88
	public static string OPT_FORCE_CULLING_FRONT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_CULLING_FRONT_get();
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001689C File Offset: 0x00014A9C
	public static string OPT_GL_FRAGMENT_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_FRAGMENT_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x060009BE RID: 2494 RVA: 0x000168B0 File Offset: 0x00014AB0
	public static string OPT_FORCE_SCISSOR_TEST_REJECT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_SCISSOR_TEST_REJECT_get();
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x000168C4 File Offset: 0x00014AC4
	public static string OPT_FORCE_BLENDING_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_BLENDING_OFF_get();
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x000168D8 File Offset: 0x00014AD8
	public static string OPT_FORCE_STENCIL_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_STENCIL_OFF_get();
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000168EC File Offset: 0x00014AEC
	public static string OPT_FORCE_STENCIL_TESTING_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_STENCIL_TESTING_OFF_get();
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00016900 File Offset: 0x00014B00
	public static string OPT_FORCE_STENCIL_TEST_REJECT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_STENCIL_TEST_REJECT_get();
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00016914 File Offset: 0x00014B14
	public static string OPT_FORCE_DEPTH_TEST_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_DEPTH_TEST_OFF_get();
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00016928 File Offset: 0x00014B28
	public static string OPT_FORCE_DEPTH_TEST_REJECT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_DEPTH_TEST_REJECT_get();
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0001693C File Offset: 0x00014B3C
	public static string OPT_FORCE_COLOR_MASK_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_COLOR_MASK_OFF_get();
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00016950 File Offset: 0x00014B50
	public static string OPT_FORCE_SMALL_VIEWPORT
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_SMALL_VIEWPORT_get();
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x00016964 File Offset: 0x00014B64
	public static string OPT_FORCE_MULTI_SAMPLING_OFF
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_MULTI_SAMPLING_OFF_get();
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00016978 File Offset: 0x00014B78
	public static string OPT_GL_TEXTURE_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_TEXTURE_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0001698C File Offset: 0x00014B8C
	public static string OPT_FORCE_SMALL_TEXTURES
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_SMALL_TEXTURES_get();
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x000169A0 File Offset: 0x00014BA0
	public static string OPT_GL_DRIVER_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_DRIVER_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x000169B4 File Offset: 0x00014BB4
	public static string OPT_FORCE_IFD
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_IFD_get();
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x000169C8 File Offset: 0x00014BC8
	public static string OPT_FORCE_IFH
	{
		get
		{
			return SDPCorePINVOKE.OPT_FORCE_IFH_get();
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x000169DC File Offset: 0x00014BDC
	public static string OPT_POWER_OVERRIDE_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_POWER_OVERRIDE_CATEGORY_get();
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x000169F0 File Offset: 0x00014BF0
	public static string OPT_ENABLE_GPU_DCVS_AND_NAP
	{
		get
		{
			return SDPCorePINVOKE.OPT_ENABLE_GPU_DCVS_AND_NAP_get();
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x00016A04 File Offset: 0x00014C04
	public static string OPT_OPENGL_FRAME_DELIMITERS_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_OPENGL_FRAME_DELIMITERS_CATEGORY_get();
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00016A18 File Offset: 0x00014C18
	public static string OPT_EGL_FRAME_DELIMITER_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_FRAME_DELIMITER_CATEGORY_get();
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00016A2C File Offset: 0x00014C2C
	public static string OPT_EGL_SWAP_BUFFERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_SWAP_BUFFERS_get();
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00016A40 File Offset: 0x00014C40
	public static string OPT_EGL_MAKE_CURRENT
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_MAKE_CURRENT_get();
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00016A54 File Offset: 0x00014C54
	public static string OPT_EGL_COPY_BUFFERS
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_COPY_BUFFERS_get();
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00016A68 File Offset: 0x00014C68
	public static string OPT_EGL_WAIT_GL
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_WAIT_GL_get();
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00016A7C File Offset: 0x00014C7C
	public static string OPT_EGL_WAIT_NATIVE
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_WAIT_NATIVE_get();
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00016A90 File Offset: 0x00014C90
	public static string OPT_EGL_WAIT_CLIENT
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_WAIT_CLIENT_get();
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00016AA4 File Offset: 0x00014CA4
	public static string OPT_EGL_BIND_API
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_BIND_API_get();
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00016AB8 File Offset: 0x00014CB8
	public static string OPT_EGL_LOCK_SURFACE_KHR
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_LOCK_SURFACE_KHR_get();
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x060009D9 RID: 2521 RVA: 0x00016ACC File Offset: 0x00014CCC
	public static string OPT_EGL_UNLOCK_SURFACE_KHR
	{
		get
		{
			return SDPCorePINVOKE.OPT_EGL_UNLOCK_SURFACE_KHR_get();
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x00016AE0 File Offset: 0x00014CE0
	public static string OPT_GL_FRAME_DELIMITER_CATEGORY
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_FRAME_DELIMITER_CATEGORY_get();
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x060009DB RID: 2523 RVA: 0x00016AF4 File Offset: 0x00014CF4
	public static string OPT_GL_CLEAR
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_CLEAR_get();
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x00016B08 File Offset: 0x00014D08
	public static string OPT_GL_FINISH
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_FINISH_get();
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x060009DD RID: 2525 RVA: 0x00016B1C File Offset: 0x00014D1C
	public static string OPT_GL_FLUSH
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_FLUSH_get();
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x060009DE RID: 2526 RVA: 0x00016B30 File Offset: 0x00014D30
	public static string OPT_GL_INSERT_EVENT_MARKER_EXT
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_INSERT_EVENT_MARKER_EXT_get();
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x00016B44 File Offset: 0x00014D44
	public static string OPT_GL_WAIT_SYNC
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_WAIT_SYNC_get();
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00016B58 File Offset: 0x00014D58
	public static string OPT_GL_CLIENT_WAIT_SYNC
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_CLIENT_WAIT_SYNC_get();
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00016B6C File Offset: 0x00014D6C
	public static string OPT_GL_START_TILING_QCOM
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_START_TILING_QCOM_get();
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x060009E2 RID: 2530 RVA: 0x00016B80 File Offset: 0x00014D80
	public static string OPT_GL_END_TILING_QCOM
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_END_TILING_QCOM_get();
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00016B94 File Offset: 0x00014D94
	public static string OPT_GL_DRAW
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_DRAW_get();
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00016BA8 File Offset: 0x00014DA8
	public static string OPT_GL_DEBUG_MESSAGE_INSERT
	{
		get
		{
			return SDPCorePINVOKE.OPT_GL_DEBUG_MESSAGE_INSERT_get();
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00016BBC File Offset: 0x00014DBC
	public static StringList DSP_DATA_PROVIDER_NAME_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_DATA_PROVIDER_NAME_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00016BE8 File Offset: 0x00014DE8
	public static StringList DSP_DATA_PROVIDER_DESC_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_DATA_PROVIDER_DESC_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00016C14 File Offset: 0x00014E14
	public static StringList DSP_DATA_PROVIDER_DEV_NODE_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_DATA_PROVIDER_DEV_NODE_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x00016C40 File Offset: 0x00014E40
	public static StringList DSP_METRIC_CATEGORY_NAME_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_METRIC_CATEGORY_NAME_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00016C6C File Offset: 0x00014E6C
	public static StringList DSP_METRIC_PREFIX_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_METRIC_PREFIX_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x00016C98 File Offset: 0x00014E98
	public static StringList DSP_NAME_LIST
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.DSP_NAME_LIST_get();
			return (intPtr == IntPtr.Zero) ? null : new StringList(intPtr, false);
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x00016CC4 File Offset: 0x00014EC4
	public static int DSP_COUNT
	{
		get
		{
			return SDPCorePINVOKE.DSP_COUNT_get();
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x00016CD8 File Offset: 0x00014ED8
	public static int ADSP
	{
		get
		{
			return SDPCorePINVOKE.ADSP_get();
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x00016CEC File Offset: 0x00014EEC
	public static int SDSP
	{
		get
		{
			return SDPCorePINVOKE.SDSP_get();
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x00016D00 File Offset: 0x00014F00
	public static int CDSP
	{
		get
		{
			return SDPCorePINVOKE.CDSP_get();
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x060009EF RID: 2543 RVA: 0x00016D14 File Offset: 0x00014F14
	public static int CDSP1
	{
		get
		{
			return SDPCorePINVOKE.CDSP1_get();
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x00016D28 File Offset: 0x00014F28
	public static int NPU
	{
		get
		{
			return SDPCorePINVOKE.NPU_get();
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00016D3C File Offset: 0x00014F3C
	public static string DSP_METRIC_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.DSP_METRIC_CATEGORY_NAME_get();
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00016D50 File Offset: 0x00014F50
	public static string SLPI_METRIC_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.SLPI_METRIC_CATEGORY_NAME_get();
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00016D64 File Offset: 0x00014F64
	public static string CDSP_METRIC_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.CDSP_METRIC_CATEGORY_NAME_get();
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00016D78 File Offset: 0x00014F78
	public static string CDSP1_METRIC_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.CDSP1_METRIC_CATEGORY_NAME_get();
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00016D8C File Offset: 0x00014F8C
	public static string NPU_METRIC_CATEGORY_NAME
	{
		get
		{
			return SDPCorePINVOKE.NPU_METRIC_CATEGORY_NAME_get();
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00016DA0 File Offset: 0x00014FA0
	public static string ADSP_MODEL_NAME
	{
		get
		{
			return SDPCorePINVOKE.ADSP_MODEL_NAME_get();
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00016DB4 File Offset: 0x00014FB4
	public static string ADSP_MODEL_COUNTERS_NAME
	{
		get
		{
			return SDPCorePINVOKE.ADSP_MODEL_COUNTERS_NAME_get();
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00016DC8 File Offset: 0x00014FC8
	public static string SDSP_MODEL_NAME
	{
		get
		{
			return SDPCorePINVOKE.SDSP_MODEL_NAME_get();
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00016DDC File Offset: 0x00014FDC
	public static string SDSP_MODEL_COUNTERS_NAME
	{
		get
		{
			return SDPCorePINVOKE.SDSP_MODEL_COUNTERS_NAME_get();
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x00016DF0 File Offset: 0x00014FF0
	public static string CDSP_MODEL_NAME
	{
		get
		{
			return SDPCorePINVOKE.CDSP_MODEL_NAME_get();
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x00016E04 File Offset: 0x00015004
	public static string CDSP_MODEL_TABLE_COUNTERS_NAME
	{
		get
		{
			return SDPCorePINVOKE.CDSP_MODEL_TABLE_COUNTERS_NAME_get();
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x00016E18 File Offset: 0x00015018
	public static string NPU_MODEL_NAME
	{
		get
		{
			return SDPCorePINVOKE.NPU_MODEL_NAME_get();
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x00016E2C File Offset: 0x0001502C
	public static string NPU_MODEL_TABLE_COUNTERS_NAME
	{
		get
		{
			return SDPCorePINVOKE.NPU_MODEL_TABLE_COUNTERS_NAME_get();
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x00016E40 File Offset: 0x00015040
	public static string DSP_MODEL_ATTRIB_TIMESTAMP
	{
		get
		{
			return SDPCorePINVOKE.DSP_MODEL_ATTRIB_TIMESTAMP_get();
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x00016E54 File Offset: 0x00015054
	public static string DSP_MODEL_ATTRIB_CAPTURE_ID
	{
		get
		{
			return SDPCorePINVOKE.DSP_MODEL_ATTRIB_CAPTURE_ID_get();
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00016E68 File Offset: 0x00015068
	public static string DSP_MODEL_ATTRIB_METRIC_ID
	{
		get
		{
			return SDPCorePINVOKE.DSP_MODEL_ATTRIB_METRIC_ID_get();
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00016E7C File Offset: 0x0001507C
	public static string DSP_MODEL_ATTRIB_COUNTER_VALUE
	{
		get
		{
			return SDPCorePINVOKE.DSP_MODEL_ATTRIB_COUNTER_VALUE_get();
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00016E90 File Offset: 0x00015090
	public static string SDP_PROCESSOR_PLUGINS_PATH
	{
		get
		{
			return SDPCorePINVOKE.SDP_PROCESSOR_PLUGINS_PATH_get();
		}
	}

	// Token: 0x04000184 RID: 388
	public static readonly int INT_MAX = SDPCorePINVOKE.INT_MAX_get();

	// Token: 0x04000185 RID: 389
	public static readonly int INT32_MAX = SDPCorePINVOKE.INT32_MAX_get();

	// Token: 0x04000186 RID: 390
	public static readonly int UINT32_MAX = SDPCorePINVOKE.UINT32_MAX_get();

	// Token: 0x04000187 RID: 391
	public static readonly int SDP_VERSION_MAJOR = SDPCorePINVOKE.SDP_VERSION_MAJOR_get();

	// Token: 0x04000188 RID: 392
	public static readonly int SDP_VERSION_MINOR = SDPCorePINVOKE.SDP_VERSION_MINOR_get();

	// Token: 0x04000189 RID: 393
	public static readonly int SDP_VERSION_SUBMINOR = SDPCorePINVOKE.SDP_VERSION_SUBMINOR_get();

	// Token: 0x0400018A RID: 394
	public static readonly string SDP_VERSION_MAJOR_MINOR = SDPCorePINVOKE.SDP_VERSION_MAJOR_MINOR_get();

	// Token: 0x0400018B RID: 395
	public static readonly string LOGGER_TAG = SDPCorePINVOKE.LOGGER_TAG_get();

	// Token: 0x0400018C RID: 396
	public static readonly int SDP_DSP_MODE_ADSP = SDPCorePINVOKE.SDP_DSP_MODE_ADSP_get();

	// Token: 0x0400018D RID: 397
	public static readonly int SDP_DSP_MODE_MDSP = SDPCorePINVOKE.SDP_DSP_MODE_MDSP_get();

	// Token: 0x0400018E RID: 398
	public static readonly int SDP_DSP_MODE_SDSP = SDPCorePINVOKE.SDP_DSP_MODE_SDSP_get();

	// Token: 0x0400018F RID: 399
	public static readonly int SDP_DSP_MODE_CDSP = SDPCorePINVOKE.SDP_DSP_MODE_CDSP_get();

	// Token: 0x04000190 RID: 400
	public static readonly int SDP_NPU_MODE = SDPCorePINVOKE.SDP_NPU_MODE_get();

	// Token: 0x04000191 RID: 401
	public static readonly int SDP_DSP_MODE_CDSP1 = SDPCorePINVOKE.SDP_DSP_MODE_CDSP1_get();

	// Token: 0x04000192 RID: 402
	public static readonly string DSP_METRIC_EFFQ6 = SDPCorePINVOKE.DSP_METRIC_EFFQ6_get();

	// Token: 0x04000193 RID: 403
	public static readonly string DSP_METRIC_CORECLOCK = SDPCorePINVOKE.DSP_METRIC_CORECLOCK_get();

	// Token: 0x04000194 RID: 404
	public static readonly string DSP_METRIC_BUSCLOCKVOTE = SDPCorePINVOKE.DSP_METRIC_BUSCLOCKVOTE_get();

	// Token: 0x04000195 RID: 405
	public static readonly string DSP_METRIC_1TACTIVE = SDPCorePINVOKE.DSP_METRIC_1TACTIVE_get();

	// Token: 0x04000196 RID: 406
	public static readonly string DSP_METRIC_L2FETCH_DU_MISS = SDPCorePINVOKE.DSP_METRIC_L2FETCH_DU_MISS_get();

	// Token: 0x04000197 RID: 407
	public static readonly string DSP_METRIC_MPPS = SDPCorePINVOKE.DSP_METRIC_MPPS_get();

	// Token: 0x04000198 RID: 408
	public static readonly string DSP_METRIC_PCPP = SDPCorePINVOKE.DSP_METRIC_PCPP_get();

	// Token: 0x04000199 RID: 409
	public static readonly string DSP_METRIC_HVXTHREADMPPS = SDPCorePINVOKE.DSP_METRIC_HVXTHREADMPPS_get();

	// Token: 0x0400019A RID: 410
	public static readonly string DSP_METRIC_HVXMPPS = SDPCorePINVOKE.DSP_METRIC_HVXMPPS_get();

	// Token: 0x0400019B RID: 411
	public static readonly string DSP_METRIC_AXIRDBW = SDPCorePINVOKE.DSP_METRIC_AXIRDBW_get();

	// Token: 0x0400019C RID: 412
	public static readonly string DSP_METRIC_AXIWRBW = SDPCorePINVOKE.DSP_METRIC_AXIWRBW_get();

	// Token: 0x0400019D RID: 413
	public static readonly string DSP_METRIC_HMX_UTILZ = SDPCorePINVOKE.DSP_METRIC_HMX_UTILZ_get();

	// Token: 0x0400019E RID: 414
	public static readonly string NPU_METRIC_DDRRDBW = SDPCorePINVOKE.NPU_METRIC_DDRRDBW_get();

	// Token: 0x0400019F RID: 415
	public static readonly string NPU_METRIC_DDRWRBW = SDPCorePINVOKE.NPU_METRIC_DDRWRBW_get();

	// Token: 0x040001A0 RID: 416
	public static readonly string NPU_METRIC_DDRRDBW1 = SDPCorePINVOKE.NPU_METRIC_DDRRDBW1_get();

	// Token: 0x040001A1 RID: 417
	public static readonly string NPU_METRIC_DDRWRBW1 = SDPCorePINVOKE.NPU_METRIC_DDRWRBW1_get();
}
