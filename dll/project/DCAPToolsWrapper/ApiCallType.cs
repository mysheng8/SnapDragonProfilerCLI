using System;

// Token: 0x02000003 RID: 3
public enum ApiCallType
{
	// Token: 0x04000004 RID: 4
	ApiCallTypeUnknown,
	// Token: 0x04000005 RID: 5
	ApiCallType_IUnknown__QueryInterface,
	// Token: 0x04000006 RID: 6
	ApiCallType_IUnknown__AddRef,
	// Token: 0x04000007 RID: 7
	ApiCallType_IUnknown__Release,
	// Token: 0x04000008 RID: 8
	ApiCallType_ID3D11DeviceChild__GetDevice,
	// Token: 0x04000009 RID: 9
	ApiCallType_ID3D11DeviceChild__GetPrivateData,
	// Token: 0x0400000A RID: 10
	ApiCallType_ID3D11DeviceChild__SetPrivateData,
	// Token: 0x0400000B RID: 11
	ApiCallType_ID3D11DeviceChild__SetPrivateDataInterface,
	// Token: 0x0400000C RID: 12
	ApiCallType_ID3D11DepthStencilState__GetDesc,
	// Token: 0x0400000D RID: 13
	ApiCallType_ID3D11BlendState__GetDesc,
	// Token: 0x0400000E RID: 14
	ApiCallType_ID3D11RasterizerState__GetDesc,
	// Token: 0x0400000F RID: 15
	ApiCallType_ID3D11Resource__GetType,
	// Token: 0x04000010 RID: 16
	ApiCallType_ID3D11Resource__SetEvictionPriority,
	// Token: 0x04000011 RID: 17
	ApiCallType_ID3D11Resource__GetEvictionPriority,
	// Token: 0x04000012 RID: 18
	ApiCallType_ID3D11Buffer__GetDesc,
	// Token: 0x04000013 RID: 19
	ApiCallType_ID3D11Texture1D__GetDesc,
	// Token: 0x04000014 RID: 20
	ApiCallType_ID3D11Texture2D__GetDesc,
	// Token: 0x04000015 RID: 21
	ApiCallType_ID3D11Texture3D__GetDesc,
	// Token: 0x04000016 RID: 22
	ApiCallType_ID3D11View__GetResource,
	// Token: 0x04000017 RID: 23
	ApiCallType_ID3D11ShaderResourceView__GetDesc,
	// Token: 0x04000018 RID: 24
	ApiCallType_ID3D11RenderTargetView__GetDesc,
	// Token: 0x04000019 RID: 25
	ApiCallType_ID3D11DepthStencilView__GetDesc,
	// Token: 0x0400001A RID: 26
	ApiCallType_ID3D11UnorderedAccessView__GetDesc,
	// Token: 0x0400001B RID: 27
	ApiCallType_ID3D11SamplerState__GetDesc,
	// Token: 0x0400001C RID: 28
	ApiCallType_ID3D11Asynchronous__GetDataSize,
	// Token: 0x0400001D RID: 29
	ApiCallType_ID3D11Query__GetDesc,
	// Token: 0x0400001E RID: 30
	ApiCallType_ID3D11Counter__GetDesc,
	// Token: 0x0400001F RID: 31
	ApiCallType_ID3D11ClassInstance__GetClassLinkage,
	// Token: 0x04000020 RID: 32
	ApiCallType_ID3D11ClassInstance__GetDesc,
	// Token: 0x04000021 RID: 33
	ApiCallType_ID3D11ClassInstance__GetInstanceName,
	// Token: 0x04000022 RID: 34
	ApiCallType_ID3D11ClassInstance__GetTypeName,
	// Token: 0x04000023 RID: 35
	ApiCallType_ID3D11ClassLinkage__GetClassInstance,
	// Token: 0x04000024 RID: 36
	ApiCallType_ID3D11ClassLinkage__CreateClassInstance,
	// Token: 0x04000025 RID: 37
	ApiCallType_ID3D11CommandList__GetContextFlags,
	// Token: 0x04000026 RID: 38
	ApiCallType_ID3D11DeviceContext__VSSetConstantBuffers,
	// Token: 0x04000027 RID: 39
	ApiCallType_ID3D11DeviceContext__PSSetShaderResources,
	// Token: 0x04000028 RID: 40
	ApiCallType_ID3D11DeviceContext__PSSetShader,
	// Token: 0x04000029 RID: 41
	ApiCallType_ID3D11DeviceContext__PSSetSamplers,
	// Token: 0x0400002A RID: 42
	ApiCallType_ID3D11DeviceContext__VSSetShader,
	// Token: 0x0400002B RID: 43
	ApiCallType_ID3D11DeviceContext__DrawIndexed,
	// Token: 0x0400002C RID: 44
	ApiCallType_ID3D11DeviceContext__Draw,
	// Token: 0x0400002D RID: 45
	ApiCallType_ID3D11DeviceContext__Map,
	// Token: 0x0400002E RID: 46
	ApiCallType_ID3D11DeviceContext__Unmap,
	// Token: 0x0400002F RID: 47
	ApiCallType_ID3D11DeviceContext__PSSetConstantBuffers,
	// Token: 0x04000030 RID: 48
	ApiCallType_ID3D11DeviceContext__IASetInputLayout,
	// Token: 0x04000031 RID: 49
	ApiCallType_ID3D11DeviceContext__IASetVertexBuffers,
	// Token: 0x04000032 RID: 50
	ApiCallType_ID3D11DeviceContext__IASetIndexBuffer,
	// Token: 0x04000033 RID: 51
	ApiCallType_ID3D11DeviceContext__DrawIndexedInstanced,
	// Token: 0x04000034 RID: 52
	ApiCallType_ID3D11DeviceContext__DrawInstanced,
	// Token: 0x04000035 RID: 53
	ApiCallType_ID3D11DeviceContext__GSSetConstantBuffers,
	// Token: 0x04000036 RID: 54
	ApiCallType_ID3D11DeviceContext__GSSetShader,
	// Token: 0x04000037 RID: 55
	ApiCallType_ID3D11DeviceContext__IASetPrimitiveTopology,
	// Token: 0x04000038 RID: 56
	ApiCallType_ID3D11DeviceContext__VSSetShaderResources,
	// Token: 0x04000039 RID: 57
	ApiCallType_ID3D11DeviceContext__VSSetSamplers,
	// Token: 0x0400003A RID: 58
	ApiCallType_ID3D11DeviceContext__Begin,
	// Token: 0x0400003B RID: 59
	ApiCallType_ID3D11DeviceContext__End,
	// Token: 0x0400003C RID: 60
	ApiCallType_ID3D11DeviceContext__GetData,
	// Token: 0x0400003D RID: 61
	ApiCallType_ID3D11DeviceContext__SetPredication,
	// Token: 0x0400003E RID: 62
	ApiCallType_ID3D11DeviceContext__GSSetShaderResources,
	// Token: 0x0400003F RID: 63
	ApiCallType_ID3D11DeviceContext__GSSetSamplers,
	// Token: 0x04000040 RID: 64
	ApiCallType_ID3D11DeviceContext__OMSetRenderTargets,
	// Token: 0x04000041 RID: 65
	ApiCallType_ID3D11DeviceContext__OMSetRenderTargetsAndUnorderedAccessViews,
	// Token: 0x04000042 RID: 66
	ApiCallType_ID3D11DeviceContext__OMSetBlendState,
	// Token: 0x04000043 RID: 67
	ApiCallType_ID3D11DeviceContext__OMSetDepthStencilState,
	// Token: 0x04000044 RID: 68
	ApiCallType_ID3D11DeviceContext__SOSetTargets,
	// Token: 0x04000045 RID: 69
	ApiCallType_ID3D11DeviceContext__DrawAuto,
	// Token: 0x04000046 RID: 70
	ApiCallType_ID3D11DeviceContext__DrawIndexedInstancedIndirect,
	// Token: 0x04000047 RID: 71
	ApiCallType_ID3D11DeviceContext__DrawInstancedIndirect,
	// Token: 0x04000048 RID: 72
	ApiCallType_ID3D11DeviceContext__Dispatch,
	// Token: 0x04000049 RID: 73
	ApiCallType_ID3D11DeviceContext__DispatchIndirect,
	// Token: 0x0400004A RID: 74
	ApiCallType_ID3D11DeviceContext__RSSetState,
	// Token: 0x0400004B RID: 75
	ApiCallType_ID3D11DeviceContext__RSSetViewports,
	// Token: 0x0400004C RID: 76
	ApiCallType_ID3D11DeviceContext__RSSetScissorRects,
	// Token: 0x0400004D RID: 77
	ApiCallType_ID3D11DeviceContext__CopySubresourceRegion,
	// Token: 0x0400004E RID: 78
	ApiCallType_ID3D11DeviceContext__CopyResource,
	// Token: 0x0400004F RID: 79
	ApiCallType_ID3D11DeviceContext__UpdateSubresource,
	// Token: 0x04000050 RID: 80
	ApiCallType_ID3D11DeviceContext__CopyStructureCount,
	// Token: 0x04000051 RID: 81
	ApiCallType_ID3D11DeviceContext__ClearRenderTargetView,
	// Token: 0x04000052 RID: 82
	ApiCallType_ID3D11DeviceContext__ClearUnorderedAccessViewUint,
	// Token: 0x04000053 RID: 83
	ApiCallType_ID3D11DeviceContext__ClearUnorderedAccessViewFloat,
	// Token: 0x04000054 RID: 84
	ApiCallType_ID3D11DeviceContext__ClearDepthStencilView,
	// Token: 0x04000055 RID: 85
	ApiCallType_ID3D11DeviceContext__GenerateMips,
	// Token: 0x04000056 RID: 86
	ApiCallType_ID3D11DeviceContext__SetResourceMinLOD,
	// Token: 0x04000057 RID: 87
	ApiCallType_ID3D11DeviceContext__GetResourceMinLOD,
	// Token: 0x04000058 RID: 88
	ApiCallType_ID3D11DeviceContext__ResolveSubresource,
	// Token: 0x04000059 RID: 89
	ApiCallType_ID3D11DeviceContext__ExecuteCommandList,
	// Token: 0x0400005A RID: 90
	ApiCallType_ID3D11DeviceContext__HSSetShaderResources,
	// Token: 0x0400005B RID: 91
	ApiCallType_ID3D11DeviceContext__HSSetShader,
	// Token: 0x0400005C RID: 92
	ApiCallType_ID3D11DeviceContext__HSSetSamplers,
	// Token: 0x0400005D RID: 93
	ApiCallType_ID3D11DeviceContext__HSSetConstantBuffers,
	// Token: 0x0400005E RID: 94
	ApiCallType_ID3D11DeviceContext__DSSetShaderResources,
	// Token: 0x0400005F RID: 95
	ApiCallType_ID3D11DeviceContext__DSSetShader,
	// Token: 0x04000060 RID: 96
	ApiCallType_ID3D11DeviceContext__DSSetSamplers,
	// Token: 0x04000061 RID: 97
	ApiCallType_ID3D11DeviceContext__DSSetConstantBuffers,
	// Token: 0x04000062 RID: 98
	ApiCallType_ID3D11DeviceContext__CSSetShaderResources,
	// Token: 0x04000063 RID: 99
	ApiCallType_ID3D11DeviceContext__CSSetUnorderedAccessViews,
	// Token: 0x04000064 RID: 100
	ApiCallType_ID3D11DeviceContext__CSSetShader,
	// Token: 0x04000065 RID: 101
	ApiCallType_ID3D11DeviceContext__CSSetSamplers,
	// Token: 0x04000066 RID: 102
	ApiCallType_ID3D11DeviceContext__CSSetConstantBuffers,
	// Token: 0x04000067 RID: 103
	ApiCallType_ID3D11DeviceContext__VSGetConstantBuffers,
	// Token: 0x04000068 RID: 104
	ApiCallType_ID3D11DeviceContext__PSGetShaderResources,
	// Token: 0x04000069 RID: 105
	ApiCallType_ID3D11DeviceContext__PSGetShader,
	// Token: 0x0400006A RID: 106
	ApiCallType_ID3D11DeviceContext__PSGetSamplers,
	// Token: 0x0400006B RID: 107
	ApiCallType_ID3D11DeviceContext__VSGetShader,
	// Token: 0x0400006C RID: 108
	ApiCallType_ID3D11DeviceContext__PSGetConstantBuffers,
	// Token: 0x0400006D RID: 109
	ApiCallType_ID3D11DeviceContext__IAGetInputLayout,
	// Token: 0x0400006E RID: 110
	ApiCallType_ID3D11DeviceContext__IAGetVertexBuffers,
	// Token: 0x0400006F RID: 111
	ApiCallType_ID3D11DeviceContext__IAGetIndexBuffer,
	// Token: 0x04000070 RID: 112
	ApiCallType_ID3D11DeviceContext__GSGetConstantBuffers,
	// Token: 0x04000071 RID: 113
	ApiCallType_ID3D11DeviceContext__GSGetShader,
	// Token: 0x04000072 RID: 114
	ApiCallType_ID3D11DeviceContext__IAGetPrimitiveTopology,
	// Token: 0x04000073 RID: 115
	ApiCallType_ID3D11DeviceContext__VSGetShaderResources,
	// Token: 0x04000074 RID: 116
	ApiCallType_ID3D11DeviceContext__VSGetSamplers,
	// Token: 0x04000075 RID: 117
	ApiCallType_ID3D11DeviceContext__GetPredication,
	// Token: 0x04000076 RID: 118
	ApiCallType_ID3D11DeviceContext__GSGetShaderResources,
	// Token: 0x04000077 RID: 119
	ApiCallType_ID3D11DeviceContext__GSGetSamplers,
	// Token: 0x04000078 RID: 120
	ApiCallType_ID3D11DeviceContext__OMGetRenderTargets,
	// Token: 0x04000079 RID: 121
	ApiCallType_ID3D11DeviceContext__OMGetRenderTargetsAndUnorderedAccessViews,
	// Token: 0x0400007A RID: 122
	ApiCallType_ID3D11DeviceContext__OMGetBlendState,
	// Token: 0x0400007B RID: 123
	ApiCallType_ID3D11DeviceContext__OMGetDepthStencilState,
	// Token: 0x0400007C RID: 124
	ApiCallType_ID3D11DeviceContext__SOGetTargets,
	// Token: 0x0400007D RID: 125
	ApiCallType_ID3D11DeviceContext__RSGetState,
	// Token: 0x0400007E RID: 126
	ApiCallType_ID3D11DeviceContext__RSGetViewports,
	// Token: 0x0400007F RID: 127
	ApiCallType_ID3D11DeviceContext__RSGetScissorRects,
	// Token: 0x04000080 RID: 128
	ApiCallType_ID3D11DeviceContext__HSGetShaderResources,
	// Token: 0x04000081 RID: 129
	ApiCallType_ID3D11DeviceContext__HSGetShader,
	// Token: 0x04000082 RID: 130
	ApiCallType_ID3D11DeviceContext__HSGetSamplers,
	// Token: 0x04000083 RID: 131
	ApiCallType_ID3D11DeviceContext__HSGetConstantBuffers,
	// Token: 0x04000084 RID: 132
	ApiCallType_ID3D11DeviceContext__DSGetShaderResources,
	// Token: 0x04000085 RID: 133
	ApiCallType_ID3D11DeviceContext__DSGetShader,
	// Token: 0x04000086 RID: 134
	ApiCallType_ID3D11DeviceContext__DSGetSamplers,
	// Token: 0x04000087 RID: 135
	ApiCallType_ID3D11DeviceContext__DSGetConstantBuffers,
	// Token: 0x04000088 RID: 136
	ApiCallType_ID3D11DeviceContext__CSGetShaderResources,
	// Token: 0x04000089 RID: 137
	ApiCallType_ID3D11DeviceContext__CSGetUnorderedAccessViews,
	// Token: 0x0400008A RID: 138
	ApiCallType_ID3D11DeviceContext__CSGetShader,
	// Token: 0x0400008B RID: 139
	ApiCallType_ID3D11DeviceContext__CSGetSamplers,
	// Token: 0x0400008C RID: 140
	ApiCallType_ID3D11DeviceContext__CSGetConstantBuffers,
	// Token: 0x0400008D RID: 141
	ApiCallType_ID3D11DeviceContext__ClearState,
	// Token: 0x0400008E RID: 142
	ApiCallType_ID3D11DeviceContext__Flush,
	// Token: 0x0400008F RID: 143
	ApiCallType_ID3D11DeviceContext__GetType,
	// Token: 0x04000090 RID: 144
	ApiCallType_ID3D11DeviceContext__GetContextFlags,
	// Token: 0x04000091 RID: 145
	ApiCallType_ID3D11DeviceContext__FinishCommandList,
	// Token: 0x04000092 RID: 146
	ApiCallType_ID3D11Device__CreateBuffer,
	// Token: 0x04000093 RID: 147
	ApiCallType_ID3D11Device__CreateTexture1D,
	// Token: 0x04000094 RID: 148
	ApiCallType_ID3D11Device__CreateTexture2D,
	// Token: 0x04000095 RID: 149
	ApiCallType_ID3D11Device__CreateTexture3D,
	// Token: 0x04000096 RID: 150
	ApiCallType_ID3D11Device__CreateShaderResourceView,
	// Token: 0x04000097 RID: 151
	ApiCallType_ID3D11Device__CreateUnorderedAccessView,
	// Token: 0x04000098 RID: 152
	ApiCallType_ID3D11Device__CreateRenderTargetView,
	// Token: 0x04000099 RID: 153
	ApiCallType_ID3D11Device__CreateDepthStencilView,
	// Token: 0x0400009A RID: 154
	ApiCallType_ID3D11Device__CreateInputLayout,
	// Token: 0x0400009B RID: 155
	ApiCallType_ID3D11Device__CreateVertexShader,
	// Token: 0x0400009C RID: 156
	ApiCallType_ID3D11Device__CreateGeometryShader,
	// Token: 0x0400009D RID: 157
	ApiCallType_ID3D11Device__CreateGeometryShaderWithStreamOutput,
	// Token: 0x0400009E RID: 158
	ApiCallType_ID3D11Device__CreatePixelShader,
	// Token: 0x0400009F RID: 159
	ApiCallType_ID3D11Device__CreateHullShader,
	// Token: 0x040000A0 RID: 160
	ApiCallType_ID3D11Device__CreateDomainShader,
	// Token: 0x040000A1 RID: 161
	ApiCallType_ID3D11Device__CreateComputeShader,
	// Token: 0x040000A2 RID: 162
	ApiCallType_ID3D11Device__CreateClassLinkage,
	// Token: 0x040000A3 RID: 163
	ApiCallType_ID3D11Device__CreateBlendState,
	// Token: 0x040000A4 RID: 164
	ApiCallType_ID3D11Device__CreateDepthStencilState,
	// Token: 0x040000A5 RID: 165
	ApiCallType_ID3D11Device__CreateRasterizerState,
	// Token: 0x040000A6 RID: 166
	ApiCallType_ID3D11Device__CreateSamplerState,
	// Token: 0x040000A7 RID: 167
	ApiCallType_ID3D11Device__CreateQuery,
	// Token: 0x040000A8 RID: 168
	ApiCallType_ID3D11Device__CreatePredicate,
	// Token: 0x040000A9 RID: 169
	ApiCallType_ID3D11Device__CreateCounter,
	// Token: 0x040000AA RID: 170
	ApiCallType_ID3D11Device__CreateDeferredContext,
	// Token: 0x040000AB RID: 171
	ApiCallType_ID3D11Device__OpenSharedResource,
	// Token: 0x040000AC RID: 172
	ApiCallType_ID3D11Device__CheckFormatSupport,
	// Token: 0x040000AD RID: 173
	ApiCallType_ID3D11Device__CheckMultisampleQualityLevels,
	// Token: 0x040000AE RID: 174
	ApiCallType_ID3D11Device__CheckCounterInfo,
	// Token: 0x040000AF RID: 175
	ApiCallType_ID3D11Device__CheckCounter,
	// Token: 0x040000B0 RID: 176
	ApiCallType_ID3D11Device__CheckFeatureSupport,
	// Token: 0x040000B1 RID: 177
	ApiCallType_ID3D11Device__GetPrivateData,
	// Token: 0x040000B2 RID: 178
	ApiCallType_ID3D11Device__SetPrivateData,
	// Token: 0x040000B3 RID: 179
	ApiCallType_ID3D11Device__SetPrivateDataInterface,
	// Token: 0x040000B4 RID: 180
	ApiCallType_ID3D11Device__GetFeatureLevel,
	// Token: 0x040000B5 RID: 181
	ApiCallType_ID3D11Device__GetCreationFlags,
	// Token: 0x040000B6 RID: 182
	ApiCallType_ID3D11Device__GetDeviceRemovedReason,
	// Token: 0x040000B7 RID: 183
	ApiCallType_ID3D11Device__GetImmediateContext,
	// Token: 0x040000B8 RID: 184
	ApiCallType_ID3D11Device__SetExceptionMode,
	// Token: 0x040000B9 RID: 185
	ApiCallType_ID3D11Device__GetExceptionMode,
	// Token: 0x040000BA RID: 186
	ApiCallType_ID3D11BlendState1__GetDesc1,
	// Token: 0x040000BB RID: 187
	ApiCallType_ID3D11RasterizerState1__GetDesc1,
	// Token: 0x040000BC RID: 188
	ApiCallType_ID3D11DeviceContext1__CopySubresourceRegion1,
	// Token: 0x040000BD RID: 189
	ApiCallType_ID3D11DeviceContext1__UpdateSubresource1,
	// Token: 0x040000BE RID: 190
	ApiCallType_ID3D11DeviceContext1__DiscardResource,
	// Token: 0x040000BF RID: 191
	ApiCallType_ID3D11DeviceContext1__DiscardView,
	// Token: 0x040000C0 RID: 192
	ApiCallType_ID3D11DeviceContext1__VSSetConstantBuffers1,
	// Token: 0x040000C1 RID: 193
	ApiCallType_ID3D11DeviceContext1__HSSetConstantBuffers1,
	// Token: 0x040000C2 RID: 194
	ApiCallType_ID3D11DeviceContext1__DSSetConstantBuffers1,
	// Token: 0x040000C3 RID: 195
	ApiCallType_ID3D11DeviceContext1__GSSetConstantBuffers1,
	// Token: 0x040000C4 RID: 196
	ApiCallType_ID3D11DeviceContext1__PSSetConstantBuffers1,
	// Token: 0x040000C5 RID: 197
	ApiCallType_ID3D11DeviceContext1__CSSetConstantBuffers1,
	// Token: 0x040000C6 RID: 198
	ApiCallType_ID3D11DeviceContext1__VSGetConstantBuffers1,
	// Token: 0x040000C7 RID: 199
	ApiCallType_ID3D11DeviceContext1__HSGetConstantBuffers1,
	// Token: 0x040000C8 RID: 200
	ApiCallType_ID3D11DeviceContext1__DSGetConstantBuffers1,
	// Token: 0x040000C9 RID: 201
	ApiCallType_ID3D11DeviceContext1__GSGetConstantBuffers1,
	// Token: 0x040000CA RID: 202
	ApiCallType_ID3D11DeviceContext1__PSGetConstantBuffers1,
	// Token: 0x040000CB RID: 203
	ApiCallType_ID3D11DeviceContext1__CSGetConstantBuffers1,
	// Token: 0x040000CC RID: 204
	ApiCallType_ID3D11DeviceContext1__SwapDeviceContextState,
	// Token: 0x040000CD RID: 205
	ApiCallType_ID3D11Device1__GetImmediateContext1,
	// Token: 0x040000CE RID: 206
	ApiCallType_ID3D11Device1__CreateDeferredContext1,
	// Token: 0x040000CF RID: 207
	ApiCallType_ID3D11Device1__CreateBlendState1,
	// Token: 0x040000D0 RID: 208
	ApiCallType_ID3D11Device1__CreateRasterizerState1,
	// Token: 0x040000D1 RID: 209
	ApiCallType_ID3D11Device1__CreateDeviceContextState,
	// Token: 0x040000D2 RID: 210
	ApiCallType_ID3D11Device1__OpenSharedResource1,
	// Token: 0x040000D3 RID: 211
	ApiCallType_ID3D11Device1__OpenSharedResourceByName,
	// Token: 0x040000D4 RID: 212
	ApiCallType_IDXGISwapChain__Present,
	// Token: 0x040000D5 RID: 213
	ApiCallType_IDXGISwapChain__GetBuffer,
	// Token: 0x040000D6 RID: 214
	ApiCallType_IDXGISwapChain__ResizeBuffers,
	// Token: 0x040000D7 RID: 215
	ApiCallType_IDXGISwapChain__ResizeTarget,
	// Token: 0x040000D8 RID: 216
	ApiCallType_D3D11CreateDevice,
	// Token: 0x040000D9 RID: 217
	ApiCallType_IDXGIFactory__CreateSwapChain,
	// Token: 0x040000DA RID: 218
	ApiCallType_IDXGISwapChain__SetFullscreenState,
	// Token: 0x040000DB RID: 219
	ApiCallType_IDXGISwapChain__GetFullscreenState,
	// Token: 0x040000DC RID: 220
	ApiCallType_IDXGISwapChain__GetDesc,
	// Token: 0x040000DD RID: 221
	ApiCallType_IDXGISwapChain__GetContainingOutput,
	// Token: 0x040000DE RID: 222
	ApiCallType_IDXGISwapChain__GetFrameStatistics,
	// Token: 0x040000DF RID: 223
	ApiCallType_IDXGISwapChain__GetLastPresentCount,
	// Token: 0x040000E0 RID: 224
	ApiCallType_IDXGISwapChain1__GetDesc1,
	// Token: 0x040000E1 RID: 225
	ApiCallType_IDXGISwapChain1__GetFullscreenDesc,
	// Token: 0x040000E2 RID: 226
	ApiCallType_IDXGISwapChain1__GetHwnd,
	// Token: 0x040000E3 RID: 227
	ApiCallType_IDXGISwapChain1__GetCoreWindow,
	// Token: 0x040000E4 RID: 228
	ApiCallType_IDXGISwapChain1__Present1,
	// Token: 0x040000E5 RID: 229
	ApiCallType_IDXGISwapChain1__IsTemporaryMonoSupported,
	// Token: 0x040000E6 RID: 230
	ApiCallType_IDXGISwapChain1__GetRestrictToOutput,
	// Token: 0x040000E7 RID: 231
	ApiCallType_IDXGISwapChain1__SetBackgroundColor,
	// Token: 0x040000E8 RID: 232
	ApiCallType_IDXGISwapChain1__GetBackgroundColor,
	// Token: 0x040000E9 RID: 233
	ApiCallType_IDXGISwapChain1__SetRotation,
	// Token: 0x040000EA RID: 234
	ApiCallType_IDXGISwapChain1__GetRotation,
	// Token: 0x040000EB RID: 235
	ApiCallType_IDXGIFactory__EnumAdapters,
	// Token: 0x040000EC RID: 236
	ApiCallType_IDXGIFactory__MakeWindowAssociation,
	// Token: 0x040000ED RID: 237
	ApiCallType_IDXGIFactory__GetWindowAssociation,
	// Token: 0x040000EE RID: 238
	ApiCallType_IDXGIFactory__CreateSoftwareAdapter,
	// Token: 0x040000EF RID: 239
	ApiCallType_IDXGIFactory1__EnumAdapters1,
	// Token: 0x040000F0 RID: 240
	ApiCallType_IDXGIFactory1__IsCurrent,
	// Token: 0x040000F1 RID: 241
	ApiCallType_IDXGIFactory2__IsWindowedStereoEnabled,
	// Token: 0x040000F2 RID: 242
	ApiCallType_IDXGIFactory2__CreateSwapChainForHwnd,
	// Token: 0x040000F3 RID: 243
	ApiCallType_IDXGIFactory2__CreateSwapChainForCoreWindow,
	// Token: 0x040000F4 RID: 244
	ApiCallType_IDXGIFactory2__GetSharedResourceAdapterLuid,
	// Token: 0x040000F5 RID: 245
	ApiCallType_IDXGIFactory2__RegisterStereoStatusWindow,
	// Token: 0x040000F6 RID: 246
	ApiCallType_IDXGIFactory2__RegisterStereoStatusEvent,
	// Token: 0x040000F7 RID: 247
	ApiCallType_IDXGIFactory2__UnregisterStereoStatus,
	// Token: 0x040000F8 RID: 248
	ApiCallType_IDXGIFactory2__RegisterOcclusionStatusWindow,
	// Token: 0x040000F9 RID: 249
	ApiCallType_IDXGIFactory2__RegisterOcclusionStatusEvent,
	// Token: 0x040000FA RID: 250
	ApiCallType_IDXGIFactory2__UnregisterOcclusionStatus,
	// Token: 0x040000FB RID: 251
	ApiCallType_IDXGIFactory2__CreateSwapChainForComposition,
	// Token: 0x040000FC RID: 252
	ApiCallType_IDXGIObject__SetPrivateData,
	// Token: 0x040000FD RID: 253
	ApiCallType_IDXGIObject__SetPrivateDataInterface,
	// Token: 0x040000FE RID: 254
	ApiCallType_IDXGIObject__GetPrivateData,
	// Token: 0x040000FF RID: 255
	ApiCallType_IDXGIObject__GetParent,
	// Token: 0x04000100 RID: 256
	ApiCallType_IDXGIDeviceSubObject__GetDevice,
	// Token: 0x04000101 RID: 257
	ApiCallType_IDXGIResource__GetSharedHandle,
	// Token: 0x04000102 RID: 258
	ApiCallType_IDXGIResource__GetUsage,
	// Token: 0x04000103 RID: 259
	ApiCallType_IDXGIResource__SetEvictionPriority,
	// Token: 0x04000104 RID: 260
	ApiCallType_IDXGIResource__GetEvictionPriority,
	// Token: 0x04000105 RID: 261
	ApiCallType_IDXGIResource1__CreateSubresourceSurface,
	// Token: 0x04000106 RID: 262
	ApiCallType_IDXGIResource1__CreateSharedHandle,
	// Token: 0x04000107 RID: 263
	ApiCallType_IDXGIKeyedMutex__AcquireSync,
	// Token: 0x04000108 RID: 264
	ApiCallType_IDXGIKeyedMutex__ReleaseSync,
	// Token: 0x04000109 RID: 265
	ApiCallType_IDXGISurface__GetDesc,
	// Token: 0x0400010A RID: 266
	ApiCallType_IDXGISurface__Map,
	// Token: 0x0400010B RID: 267
	ApiCallType_IDXGISurface__Unmap,
	// Token: 0x0400010C RID: 268
	ApiCallType_IDXGISurface1__GetDC,
	// Token: 0x0400010D RID: 269
	ApiCallType_IDXGISurface1__ReleaseDC,
	// Token: 0x0400010E RID: 270
	ApiCallType_IDXGISurface2__GetResource,
	// Token: 0x0400010F RID: 271
	ApiCallType_IDXGIAdapter__EnumOutputs,
	// Token: 0x04000110 RID: 272
	ApiCallType_IDXGIAdapter__GetDesc,
	// Token: 0x04000111 RID: 273
	ApiCallType_IDXGIAdapter__CheckInterfaceSupport,
	// Token: 0x04000112 RID: 274
	ApiCallType_IDXGIAdapter1__GetDesc1,
	// Token: 0x04000113 RID: 275
	ApiCallType_IDXGIAdapter2__GetDesc2,
	// Token: 0x04000114 RID: 276
	ApiCallType_IDXGIOutput__GetDesc,
	// Token: 0x04000115 RID: 277
	ApiCallType_IDXGIOutput__GetDisplayModeList,
	// Token: 0x04000116 RID: 278
	ApiCallType_IDXGIOutput__FindClosestMatchingMode,
	// Token: 0x04000117 RID: 279
	ApiCallType_IDXGIOutput__WaitForVBlank,
	// Token: 0x04000118 RID: 280
	ApiCallType_IDXGIOutput__TakeOwnership,
	// Token: 0x04000119 RID: 281
	ApiCallType_IDXGIOutput__ReleaseOwnership,
	// Token: 0x0400011A RID: 282
	ApiCallType_IDXGIOutput__GetGammaControlCapabilities,
	// Token: 0x0400011B RID: 283
	ApiCallType_IDXGIOutput__SetGammaControl,
	// Token: 0x0400011C RID: 284
	ApiCallType_IDXGIOutput__GetGammaControl,
	// Token: 0x0400011D RID: 285
	ApiCallType_IDXGIOutput__SetDisplaySurface,
	// Token: 0x0400011E RID: 286
	ApiCallType_IDXGIOutput__GetDisplaySurfaceData,
	// Token: 0x0400011F RID: 287
	ApiCallType_IDXGIOutput__GetFrameStatistics,
	// Token: 0x04000120 RID: 288
	ApiCallType_IDXGIOutput1__GetDisplayModeList1,
	// Token: 0x04000121 RID: 289
	ApiCallType_IDXGIOutput1__FindClosestMatchingMode1,
	// Token: 0x04000122 RID: 290
	ApiCallType_IDXGIOutput1__GetDisplaySurfaceData1,
	// Token: 0x04000123 RID: 291
	ApiCallType_IDXGIOutput1__DuplicateOutput,
	// Token: 0x04000124 RID: 292
	ApiCallType_IDXGIDevice__GetAdapter,
	// Token: 0x04000125 RID: 293
	ApiCallType_IDXGIDevice__CreateSurface,
	// Token: 0x04000126 RID: 294
	ApiCallType_IDXGIDevice__QueryResourceResidency,
	// Token: 0x04000127 RID: 295
	ApiCallType_IDXGIDevice__SetGPUThreadPriority,
	// Token: 0x04000128 RID: 296
	ApiCallType_IDXGIDevice__GetGPUThreadPriority,
	// Token: 0x04000129 RID: 297
	ApiCallType_IDXGIDevice1__SetMaximumFrameLatency,
	// Token: 0x0400012A RID: 298
	ApiCallType_IDXGIDevice1__GetMaximumFrameLatency,
	// Token: 0x0400012B RID: 299
	ApiCallType_IDXGIDevice2__OfferResources,
	// Token: 0x0400012C RID: 300
	ApiCallType_IDXGIDevice2__ReclaimResources,
	// Token: 0x0400012D RID: 301
	ApiCallType_IDXGIDevice2__EnqueueSetEvent,
	// Token: 0x0400012E RID: 302
	ApiCallType_IDXGIDisplayControl__IsStereoEnabled,
	// Token: 0x0400012F RID: 303
	ApiCallType_IDXGIDisplayControl__SetStereoEnabled,
	// Token: 0x04000130 RID: 304
	ApiCallType_IDXGIOutputDuplication__GetDesc,
	// Token: 0x04000131 RID: 305
	ApiCallType_IDXGIOutputDuplication__AcquireNextFrame,
	// Token: 0x04000132 RID: 306
	ApiCallType_IDXGIOutputDuplication__GetFrameDirtyRects,
	// Token: 0x04000133 RID: 307
	ApiCallType_IDXGIOutputDuplication__GetFrameMoveRects,
	// Token: 0x04000134 RID: 308
	ApiCallType_IDXGIOutputDuplication__GetFramePointerShape,
	// Token: 0x04000135 RID: 309
	ApiCallType_IDXGIOutputDuplication__MapDesktopSurface,
	// Token: 0x04000136 RID: 310
	ApiCallType_IDXGIOutputDuplication__UnMapDesktopSurface,
	// Token: 0x04000137 RID: 311
	ApiCallType_IDXGIOutputDuplication__ReleaseFrame,
	// Token: 0x04000138 RID: 312
	ApiCallType_CreateDXGIFactory,
	// Token: 0x04000139 RID: 313
	ApiCallType_CreateDXGIFactory1,
	// Token: 0x0400013A RID: 314
	ApiCallType_D3D11CreateDeviceAndSwapChain,
	// Token: 0x0400013B RID: 315
	ApiCallType_ID3D11DeviceContext1__ClearView,
	// Token: 0x0400013C RID: 316
	ApiCallType_ID3D11DeviceContext1__DiscardView1,
	// Token: 0x0400013D RID: 317
	ApiCallType_dxLast,
	// Token: 0x0400013E RID: 318
	ApiCallType_eglGetError = 32768,
	// Token: 0x0400013F RID: 319
	ApiCallType_eglGetDisplay,
	// Token: 0x04000140 RID: 320
	ApiCallType_eglInitialize,
	// Token: 0x04000141 RID: 321
	ApiCallType_eglTerminate,
	// Token: 0x04000142 RID: 322
	ApiCallType_eglQueryString,
	// Token: 0x04000143 RID: 323
	ApiCallType_eglGetConfigs,
	// Token: 0x04000144 RID: 324
	ApiCallType_eglChooseConfig,
	// Token: 0x04000145 RID: 325
	ApiCallType_eglGetConfigAttrib,
	// Token: 0x04000146 RID: 326
	ApiCallType_eglCreateWindowSurface,
	// Token: 0x04000147 RID: 327
	ApiCallType_eglCreatePbufferSurface,
	// Token: 0x04000148 RID: 328
	ApiCallType_eglCreatePixmapSurface,
	// Token: 0x04000149 RID: 329
	ApiCallType_eglDestroySurface,
	// Token: 0x0400014A RID: 330
	ApiCallType_eglQuerySurface,
	// Token: 0x0400014B RID: 331
	ApiCallType_eglBindAPI,
	// Token: 0x0400014C RID: 332
	ApiCallType_eglQueryAPI,
	// Token: 0x0400014D RID: 333
	ApiCallType_eglWaitClient,
	// Token: 0x0400014E RID: 334
	ApiCallType_eglReleaseThread,
	// Token: 0x0400014F RID: 335
	ApiCallType_eglCreatePbufferFromClientBuffer,
	// Token: 0x04000150 RID: 336
	ApiCallType_eglSurfaceAttrib,
	// Token: 0x04000151 RID: 337
	ApiCallType_eglBindTexImage,
	// Token: 0x04000152 RID: 338
	ApiCallType_eglReleaseTexImage,
	// Token: 0x04000153 RID: 339
	ApiCallType_eglSwapInterval,
	// Token: 0x04000154 RID: 340
	ApiCallType_eglCreateContext,
	// Token: 0x04000155 RID: 341
	ApiCallType_eglDestroyContext,
	// Token: 0x04000156 RID: 342
	ApiCallType_eglMakeCurrent,
	// Token: 0x04000157 RID: 343
	ApiCallType_eglGetCurrentContext,
	// Token: 0x04000158 RID: 344
	ApiCallType_eglGetCurrentSurface,
	// Token: 0x04000159 RID: 345
	ApiCallType_eglGetCurrentDisplay,
	// Token: 0x0400015A RID: 346
	ApiCallType_eglQueryContext,
	// Token: 0x0400015B RID: 347
	ApiCallType_eglWaitGL,
	// Token: 0x0400015C RID: 348
	ApiCallType_eglWaitNative,
	// Token: 0x0400015D RID: 349
	ApiCallType_eglSwapBuffers,
	// Token: 0x0400015E RID: 350
	ApiCallType_eglCopyBuffers,
	// Token: 0x0400015F RID: 351
	ApiCallType_eglGetProcAddress,
	// Token: 0x04000160 RID: 352
	ApiCallType_eglCreateImageKHRv1,
	// Token: 0x04000161 RID: 353
	ApiCallType_eglDestroyImageKHR,
	// Token: 0x04000162 RID: 354
	ApiCallType_eglQueryImageQCOM,
	// Token: 0x04000163 RID: 355
	ApiCallType_eglLockImageQCOM,
	// Token: 0x04000164 RID: 356
	ApiCallType_eglUnlockImageQCOM,
	// Token: 0x04000165 RID: 357
	ApiCallType_eglQueryImage64QCOM,
	// Token: 0x04000166 RID: 358
	ApiCallType_eglSetBlobCacheFuncsANDROID,
	// Token: 0x04000167 RID: 359
	ApiCallType_eglCreateSync,
	// Token: 0x04000168 RID: 360
	ApiCallType_eglDestroySyncKHR,
	// Token: 0x04000169 RID: 361
	ApiCallType_eglClientWaitSyncKHR,
	// Token: 0x0400016A RID: 362
	ApiCallType_eglWaitSyncKHR,
	// Token: 0x0400016B RID: 363
	ApiCallType_eglGetSyncAttrib,
	// Token: 0x0400016C RID: 364
	ApiCallType_eglDupNativeFenceFDANDROID,
	// Token: 0x0400016D RID: 365
	ApiCallType_eglLockSurfaceKHR,
	// Token: 0x0400016E RID: 366
	ApiCallType_eglUnlockSurfaceKHR,
	// Token: 0x0400016F RID: 367
	ApiCallType_eglQuerySurface64KHR,
	// Token: 0x04000170 RID: 368
	ApiCallType_eglGpuPerfHintQCOM,
	// Token: 0x04000171 RID: 369
	ApiCallType_eglCreateSync64KHR,
	// Token: 0x04000172 RID: 370
	ApiCallType_eglGetSyncObjFromEglSyncQCOM,
	// Token: 0x04000173 RID: 371
	ApiCallType_eglSignalSyncKHR,
	// Token: 0x04000174 RID: 372
	ApiCallType_eglImageAcquireInternal,
	// Token: 0x04000175 RID: 373
	ApiCallType_eglImageReleaseInternal,
	// Token: 0x04000176 RID: 374
	ApiCallType_eglCreateImageKHR,
	// Token: 0x04000177 RID: 375
	ApiCallType_eglSetDamageRegionKHR,
	// Token: 0x04000178 RID: 376
	ApiCallType_eglBindNativeDisplay,
	// Token: 0x04000179 RID: 377
	ApiCallType_eglUnbindNativeDisplay,
	// Token: 0x0400017A RID: 378
	ApiCallType_eglQueryNativeBuffer,
	// Token: 0x0400017B RID: 379
	ApiCallType_eglGetNativeBufferFromImage,
	// Token: 0x0400017C RID: 380
	ApiCallType_eglGetPlatformDisplayEXT,
	// Token: 0x0400017D RID: 381
	ApiCallType_eglGetPlatformDisplay,
	// Token: 0x0400017E RID: 382
	ApiCallType_eglCreatePlatformWindowSurface,
	// Token: 0x0400017F RID: 383
	ApiCallType_eglCreatePlatformPixmapSurface,
	// Token: 0x04000180 RID: 384
	ApiCallType_eglCreateSyncKHR,
	// Token: 0x04000181 RID: 385
	ApiCallType_eglGetSyncAttribKHR = 32836,
	// Token: 0x04000182 RID: 386
	ApiCallType_eglQueryDmaBufFormatsEXT,
	// Token: 0x04000183 RID: 387
	ApiCallType_eglQueryDmaBufModifiersEXT,
	// Token: 0x04000184 RID: 388
	ApiCallType_eglExportDMABUFImageQueryMESA,
	// Token: 0x04000185 RID: 389
	ApiCallType_eglExportDMABUFImageMESA,
	// Token: 0x04000186 RID: 390
	ApiCallType_eglLast,
	// Token: 0x04000187 RID: 391
	ApiCallType_glActiveTexture = 36864,
	// Token: 0x04000188 RID: 392
	ApiCallType_glAttachShader,
	// Token: 0x04000189 RID: 393
	ApiCallType_glBindAttribLocation,
	// Token: 0x0400018A RID: 394
	ApiCallType_glBindBuffer,
	// Token: 0x0400018B RID: 395
	ApiCallType_glBindFramebuffer,
	// Token: 0x0400018C RID: 396
	ApiCallType_glBindRenderbuffer,
	// Token: 0x0400018D RID: 397
	ApiCallType_glBindTexture,
	// Token: 0x0400018E RID: 398
	ApiCallType_glBlendColor,
	// Token: 0x0400018F RID: 399
	ApiCallType_glBlendEquation,
	// Token: 0x04000190 RID: 400
	ApiCallType_glBlendEquationSeparate,
	// Token: 0x04000191 RID: 401
	ApiCallType_glBlendFunc,
	// Token: 0x04000192 RID: 402
	ApiCallType_glBlendFuncSeparate,
	// Token: 0x04000193 RID: 403
	ApiCallType_glBufferData,
	// Token: 0x04000194 RID: 404
	ApiCallType_glBufferSubData,
	// Token: 0x04000195 RID: 405
	ApiCallType_glCheckFramebufferStatus,
	// Token: 0x04000196 RID: 406
	ApiCallType_glClear,
	// Token: 0x04000197 RID: 407
	ApiCallType_glClearColor,
	// Token: 0x04000198 RID: 408
	ApiCallType_glClearDepthf,
	// Token: 0x04000199 RID: 409
	ApiCallType_glClearStencil,
	// Token: 0x0400019A RID: 410
	ApiCallType_glColorMask,
	// Token: 0x0400019B RID: 411
	ApiCallType_glCompileShader,
	// Token: 0x0400019C RID: 412
	ApiCallType_glCompressedTexImage2D,
	// Token: 0x0400019D RID: 413
	ApiCallType_glCompressedTexSubImage2D,
	// Token: 0x0400019E RID: 414
	ApiCallType_glCopyTexImage2D,
	// Token: 0x0400019F RID: 415
	ApiCallType_glCopyTexSubImage2D,
	// Token: 0x040001A0 RID: 416
	ApiCallType_glCreateProgram,
	// Token: 0x040001A1 RID: 417
	ApiCallType_glCreateShader,
	// Token: 0x040001A2 RID: 418
	ApiCallType_glCullFace,
	// Token: 0x040001A3 RID: 419
	ApiCallType_glDeleteBuffers,
	// Token: 0x040001A4 RID: 420
	ApiCallType_glDeleteFramebuffers,
	// Token: 0x040001A5 RID: 421
	ApiCallType_glDeleteProgram,
	// Token: 0x040001A6 RID: 422
	ApiCallType_glDeleteRenderbuffers,
	// Token: 0x040001A7 RID: 423
	ApiCallType_glDeleteShader,
	// Token: 0x040001A8 RID: 424
	ApiCallType_glDeleteTextures,
	// Token: 0x040001A9 RID: 425
	ApiCallType_glDepthFunc,
	// Token: 0x040001AA RID: 426
	ApiCallType_glDepthMask,
	// Token: 0x040001AB RID: 427
	ApiCallType_glDepthRangef,
	// Token: 0x040001AC RID: 428
	ApiCallType_glDetachShader,
	// Token: 0x040001AD RID: 429
	ApiCallType_glDisable,
	// Token: 0x040001AE RID: 430
	ApiCallType_glDisableVertexAttribArray,
	// Token: 0x040001AF RID: 431
	ApiCallType_glDrawArrays,
	// Token: 0x040001B0 RID: 432
	ApiCallType_glDrawElements,
	// Token: 0x040001B1 RID: 433
	ApiCallType_glEnable,
	// Token: 0x040001B2 RID: 434
	ApiCallType_glEnableVertexAttribArray,
	// Token: 0x040001B3 RID: 435
	ApiCallType_glFinish,
	// Token: 0x040001B4 RID: 436
	ApiCallType_glFlush,
	// Token: 0x040001B5 RID: 437
	ApiCallType_glFramebufferRenderbuffer,
	// Token: 0x040001B6 RID: 438
	ApiCallType_glFramebufferTexture2D,
	// Token: 0x040001B7 RID: 439
	ApiCallType_glFrontFace,
	// Token: 0x040001B8 RID: 440
	ApiCallType_glGenBuffers,
	// Token: 0x040001B9 RID: 441
	ApiCallType_glGenerateMipmap,
	// Token: 0x040001BA RID: 442
	ApiCallType_glGenFramebuffers,
	// Token: 0x040001BB RID: 443
	ApiCallType_glGenRenderbuffers,
	// Token: 0x040001BC RID: 444
	ApiCallType_glGenTextures,
	// Token: 0x040001BD RID: 445
	ApiCallType_glGetActiveAttrib,
	// Token: 0x040001BE RID: 446
	ApiCallType_glGetActiveUniform,
	// Token: 0x040001BF RID: 447
	ApiCallType_glGetAttachedShaders,
	// Token: 0x040001C0 RID: 448
	ApiCallType_glGetAttribLocation,
	// Token: 0x040001C1 RID: 449
	ApiCallType_glGetBooleanv,
	// Token: 0x040001C2 RID: 450
	ApiCallType_glGetBufferParameteriv,
	// Token: 0x040001C3 RID: 451
	ApiCallType_glGetError,
	// Token: 0x040001C4 RID: 452
	ApiCallType_glGetFloatv,
	// Token: 0x040001C5 RID: 453
	ApiCallType_glGetFramebufferAttachmentParameteriv,
	// Token: 0x040001C6 RID: 454
	ApiCallType_glGetIntegerv,
	// Token: 0x040001C7 RID: 455
	ApiCallType_glGetProgramiv,
	// Token: 0x040001C8 RID: 456
	ApiCallType_glGetProgramInfoLog,
	// Token: 0x040001C9 RID: 457
	ApiCallType_glGetRenderbufferParameteriv,
	// Token: 0x040001CA RID: 458
	ApiCallType_glGetShaderiv,
	// Token: 0x040001CB RID: 459
	ApiCallType_glGetShaderInfoLog,
	// Token: 0x040001CC RID: 460
	ApiCallType_glGetShaderPrecisionFormat,
	// Token: 0x040001CD RID: 461
	ApiCallType_glGetShaderSource,
	// Token: 0x040001CE RID: 462
	ApiCallType_glGetString,
	// Token: 0x040001CF RID: 463
	ApiCallType_glGetTexParameterfv,
	// Token: 0x040001D0 RID: 464
	ApiCallType_glGetTexParameteriv,
	// Token: 0x040001D1 RID: 465
	ApiCallType_glGetUniformfv,
	// Token: 0x040001D2 RID: 466
	ApiCallType_glGetUniformiv,
	// Token: 0x040001D3 RID: 467
	ApiCallType_glGetUniformLocation,
	// Token: 0x040001D4 RID: 468
	ApiCallType_glGetVertexAttribfv,
	// Token: 0x040001D5 RID: 469
	ApiCallType_glGetVertexAttribiv,
	// Token: 0x040001D6 RID: 470
	ApiCallType_glGetVertexAttribPointerv,
	// Token: 0x040001D7 RID: 471
	ApiCallType_glHint,
	// Token: 0x040001D8 RID: 472
	ApiCallType_glIsBuffer,
	// Token: 0x040001D9 RID: 473
	ApiCallType_glIsEnabled,
	// Token: 0x040001DA RID: 474
	ApiCallType_glIsFramebuffer,
	// Token: 0x040001DB RID: 475
	ApiCallType_glIsProgram,
	// Token: 0x040001DC RID: 476
	ApiCallType_glIsRenderbuffer,
	// Token: 0x040001DD RID: 477
	ApiCallType_glIsShader,
	// Token: 0x040001DE RID: 478
	ApiCallType_glIsTexture,
	// Token: 0x040001DF RID: 479
	ApiCallType_glLineWidth,
	// Token: 0x040001E0 RID: 480
	ApiCallType_glLinkProgram,
	// Token: 0x040001E1 RID: 481
	ApiCallType_glPixelStorei,
	// Token: 0x040001E2 RID: 482
	ApiCallType_glPolygonOffset,
	// Token: 0x040001E3 RID: 483
	ApiCallType_glReadPixels,
	// Token: 0x040001E4 RID: 484
	ApiCallType_glReleaseShaderCompiler,
	// Token: 0x040001E5 RID: 485
	ApiCallType_glRenderbufferStorage,
	// Token: 0x040001E6 RID: 486
	ApiCallType_glSampleCoverage,
	// Token: 0x040001E7 RID: 487
	ApiCallType_glScissor,
	// Token: 0x040001E8 RID: 488
	ApiCallType_glShaderBinary,
	// Token: 0x040001E9 RID: 489
	ApiCallType_glShaderSource,
	// Token: 0x040001EA RID: 490
	ApiCallType_glStencilFunc,
	// Token: 0x040001EB RID: 491
	ApiCallType_glStencilFuncSeparate,
	// Token: 0x040001EC RID: 492
	ApiCallType_glStencilMask,
	// Token: 0x040001ED RID: 493
	ApiCallType_glStencilMaskSeparate,
	// Token: 0x040001EE RID: 494
	ApiCallType_glStencilOp,
	// Token: 0x040001EF RID: 495
	ApiCallType_glStencilOpSeparate,
	// Token: 0x040001F0 RID: 496
	ApiCallType_glTexImage2D,
	// Token: 0x040001F1 RID: 497
	ApiCallType_glTexParameterf,
	// Token: 0x040001F2 RID: 498
	ApiCallType_glTexParameterfv,
	// Token: 0x040001F3 RID: 499
	ApiCallType_glTexParameteri,
	// Token: 0x040001F4 RID: 500
	ApiCallType_glTexParameteriv,
	// Token: 0x040001F5 RID: 501
	ApiCallType_glTexSubImage2D,
	// Token: 0x040001F6 RID: 502
	ApiCallType_glUniform1f,
	// Token: 0x040001F7 RID: 503
	ApiCallType_glUniform1fv,
	// Token: 0x040001F8 RID: 504
	ApiCallType_glUniform1i,
	// Token: 0x040001F9 RID: 505
	ApiCallType_glUniform1iv,
	// Token: 0x040001FA RID: 506
	ApiCallType_glUniform2f,
	// Token: 0x040001FB RID: 507
	ApiCallType_glUniform2fv,
	// Token: 0x040001FC RID: 508
	ApiCallType_glUniform2i,
	// Token: 0x040001FD RID: 509
	ApiCallType_glUniform2iv,
	// Token: 0x040001FE RID: 510
	ApiCallType_glUniform3f,
	// Token: 0x040001FF RID: 511
	ApiCallType_glUniform3fv,
	// Token: 0x04000200 RID: 512
	ApiCallType_glUniform3i,
	// Token: 0x04000201 RID: 513
	ApiCallType_glUniform3iv,
	// Token: 0x04000202 RID: 514
	ApiCallType_glUniform4f,
	// Token: 0x04000203 RID: 515
	ApiCallType_glUniform4fv,
	// Token: 0x04000204 RID: 516
	ApiCallType_glUniform4i,
	// Token: 0x04000205 RID: 517
	ApiCallType_glUniform4iv,
	// Token: 0x04000206 RID: 518
	ApiCallType_glUniformMatrix2fv,
	// Token: 0x04000207 RID: 519
	ApiCallType_glUniformMatrix3fv,
	// Token: 0x04000208 RID: 520
	ApiCallType_glUniformMatrix4fv,
	// Token: 0x04000209 RID: 521
	ApiCallType_glUseProgram,
	// Token: 0x0400020A RID: 522
	ApiCallType_glValidateProgram,
	// Token: 0x0400020B RID: 523
	ApiCallType_glVertexAttrib1f,
	// Token: 0x0400020C RID: 524
	ApiCallType_glVertexAttrib1fv,
	// Token: 0x0400020D RID: 525
	ApiCallType_glVertexAttrib2f,
	// Token: 0x0400020E RID: 526
	ApiCallType_glVertexAttrib2fv,
	// Token: 0x0400020F RID: 527
	ApiCallType_glVertexAttrib3f,
	// Token: 0x04000210 RID: 528
	ApiCallType_glVertexAttrib3fv,
	// Token: 0x04000211 RID: 529
	ApiCallType_glVertexAttrib4f,
	// Token: 0x04000212 RID: 530
	ApiCallType_glVertexAttrib4fv,
	// Token: 0x04000213 RID: 531
	ApiCallType_glVertexAttribPointer,
	// Token: 0x04000214 RID: 532
	ApiCallType_glViewport,
	// Token: 0x04000215 RID: 533
	ApiCallType_glReadBuffer,
	// Token: 0x04000216 RID: 534
	ApiCallType_glDrawRangeElements,
	// Token: 0x04000217 RID: 535
	ApiCallType_glTexImage3D,
	// Token: 0x04000218 RID: 536
	ApiCallType_glTexSubImage3D,
	// Token: 0x04000219 RID: 537
	ApiCallType_glCopyTexSubImage3D,
	// Token: 0x0400021A RID: 538
	ApiCallType_glCompressedTexImage3D,
	// Token: 0x0400021B RID: 539
	ApiCallType_glCompressedTexSubImage3D,
	// Token: 0x0400021C RID: 540
	ApiCallType_glGenQueries,
	// Token: 0x0400021D RID: 541
	ApiCallType_glDeleteQueries,
	// Token: 0x0400021E RID: 542
	ApiCallType_glIsQuery,
	// Token: 0x0400021F RID: 543
	ApiCallType_glBeginQuery,
	// Token: 0x04000220 RID: 544
	ApiCallType_glEndQuery,
	// Token: 0x04000221 RID: 545
	ApiCallType_glGetQueryiv,
	// Token: 0x04000222 RID: 546
	ApiCallType_glGetQueryObjectuiv,
	// Token: 0x04000223 RID: 547
	ApiCallType_glUnmapBuffer,
	// Token: 0x04000224 RID: 548
	ApiCallType_glGetBufferPointerv,
	// Token: 0x04000225 RID: 549
	ApiCallType_glDrawBuffers,
	// Token: 0x04000226 RID: 550
	ApiCallType_glUniformMatrix2x3fv,
	// Token: 0x04000227 RID: 551
	ApiCallType_glUniformMatrix3x2fv,
	// Token: 0x04000228 RID: 552
	ApiCallType_glUniformMatrix2x4fv,
	// Token: 0x04000229 RID: 553
	ApiCallType_glUniformMatrix4x2fv,
	// Token: 0x0400022A RID: 554
	ApiCallType_glUniformMatrix3x4fv,
	// Token: 0x0400022B RID: 555
	ApiCallType_glUniformMatrix4x3fv,
	// Token: 0x0400022C RID: 556
	ApiCallType_glBlitFramebuffer,
	// Token: 0x0400022D RID: 557
	ApiCallType_glRenderbufferStorageMultisample,
	// Token: 0x0400022E RID: 558
	ApiCallType_glFramebufferTextureLayer,
	// Token: 0x0400022F RID: 559
	ApiCallType_glMapBufferRange,
	// Token: 0x04000230 RID: 560
	ApiCallType_glFlushMappedBufferRange,
	// Token: 0x04000231 RID: 561
	ApiCallType_glBindVertexArray,
	// Token: 0x04000232 RID: 562
	ApiCallType_glDeleteVertexArrays,
	// Token: 0x04000233 RID: 563
	ApiCallType_glGenVertexArrays,
	// Token: 0x04000234 RID: 564
	ApiCallType_glIsVertexArray,
	// Token: 0x04000235 RID: 565
	ApiCallType_glGetIntegeri_v,
	// Token: 0x04000236 RID: 566
	ApiCallType_glBeginTransformFeedback,
	// Token: 0x04000237 RID: 567
	ApiCallType_glEndTransformFeedback,
	// Token: 0x04000238 RID: 568
	ApiCallType_glBindBufferRange,
	// Token: 0x04000239 RID: 569
	ApiCallType_glBindBufferBase,
	// Token: 0x0400023A RID: 570
	ApiCallType_glTransformFeedbackVaryings,
	// Token: 0x0400023B RID: 571
	ApiCallType_glGetTransformFeedbackVarying,
	// Token: 0x0400023C RID: 572
	ApiCallType_glVertexAttribIPointer,
	// Token: 0x0400023D RID: 573
	ApiCallType_glGetVertexAttribIiv,
	// Token: 0x0400023E RID: 574
	ApiCallType_glGetVertexAttribIuiv,
	// Token: 0x0400023F RID: 575
	ApiCallType_glVertexAttribI4i,
	// Token: 0x04000240 RID: 576
	ApiCallType_glVertexAttribI4ui,
	// Token: 0x04000241 RID: 577
	ApiCallType_glVertexAttribI4iv,
	// Token: 0x04000242 RID: 578
	ApiCallType_glVertexAttribI4uiv,
	// Token: 0x04000243 RID: 579
	ApiCallType_glGetUniformuiv,
	// Token: 0x04000244 RID: 580
	ApiCallType_glGetFragDataLocation,
	// Token: 0x04000245 RID: 581
	ApiCallType_glUniform1ui,
	// Token: 0x04000246 RID: 582
	ApiCallType_glUniform2ui,
	// Token: 0x04000247 RID: 583
	ApiCallType_glUniform3ui,
	// Token: 0x04000248 RID: 584
	ApiCallType_glUniform4ui,
	// Token: 0x04000249 RID: 585
	ApiCallType_glUniform1uiv,
	// Token: 0x0400024A RID: 586
	ApiCallType_glUniform2uiv,
	// Token: 0x0400024B RID: 587
	ApiCallType_glUniform3uiv,
	// Token: 0x0400024C RID: 588
	ApiCallType_glUniform4uiv,
	// Token: 0x0400024D RID: 589
	ApiCallType_glClearBufferiv,
	// Token: 0x0400024E RID: 590
	ApiCallType_glClearBufferuiv,
	// Token: 0x0400024F RID: 591
	ApiCallType_glClearBufferfv,
	// Token: 0x04000250 RID: 592
	ApiCallType_glClearBufferfi,
	// Token: 0x04000251 RID: 593
	ApiCallType_glGetStringi,
	// Token: 0x04000252 RID: 594
	ApiCallType_glCopyBufferSubData,
	// Token: 0x04000253 RID: 595
	ApiCallType_glGetUniformIndices,
	// Token: 0x04000254 RID: 596
	ApiCallType_glGetActiveUniformsiv,
	// Token: 0x04000255 RID: 597
	ApiCallType_glGetUniformBlockIndex,
	// Token: 0x04000256 RID: 598
	ApiCallType_glGetActiveUniformBlockiv,
	// Token: 0x04000257 RID: 599
	ApiCallType_glGetActiveUniformBlockName,
	// Token: 0x04000258 RID: 600
	ApiCallType_glUniformBlockBinding,
	// Token: 0x04000259 RID: 601
	ApiCallType_glDrawArraysInstanced,
	// Token: 0x0400025A RID: 602
	ApiCallType_glDrawElementsInstanced,
	// Token: 0x0400025B RID: 603
	ApiCallType_glFenceSync,
	// Token: 0x0400025C RID: 604
	ApiCallType_glIsSync,
	// Token: 0x0400025D RID: 605
	ApiCallType_glDeleteSync,
	// Token: 0x0400025E RID: 606
	ApiCallType_glClientWaitSync,
	// Token: 0x0400025F RID: 607
	ApiCallType_glWaitSync,
	// Token: 0x04000260 RID: 608
	ApiCallType_glGetInteger64v,
	// Token: 0x04000261 RID: 609
	ApiCallType_glGetSynciv,
	// Token: 0x04000262 RID: 610
	ApiCallType_glGetInteger64i_v,
	// Token: 0x04000263 RID: 611
	ApiCallType_glGetBufferParameteri64v,
	// Token: 0x04000264 RID: 612
	ApiCallType_glGenSamplers,
	// Token: 0x04000265 RID: 613
	ApiCallType_glDeleteSamplers,
	// Token: 0x04000266 RID: 614
	ApiCallType_glIsSampler,
	// Token: 0x04000267 RID: 615
	ApiCallType_glBindSampler,
	// Token: 0x04000268 RID: 616
	ApiCallType_glSamplerParameteri,
	// Token: 0x04000269 RID: 617
	ApiCallType_glSamplerParameteriv,
	// Token: 0x0400026A RID: 618
	ApiCallType_glSamplerParameterf,
	// Token: 0x0400026B RID: 619
	ApiCallType_glSamplerParameterfv,
	// Token: 0x0400026C RID: 620
	ApiCallType_glGetSamplerParameteriv,
	// Token: 0x0400026D RID: 621
	ApiCallType_glGetSamplerParameterfv,
	// Token: 0x0400026E RID: 622
	ApiCallType_glVertexAttribDivisor,
	// Token: 0x0400026F RID: 623
	ApiCallType_glBindTransformFeedback,
	// Token: 0x04000270 RID: 624
	ApiCallType_glDeleteTransformFeedbacks,
	// Token: 0x04000271 RID: 625
	ApiCallType_glGenTransformFeedbacks,
	// Token: 0x04000272 RID: 626
	ApiCallType_glIsTransformFeedback,
	// Token: 0x04000273 RID: 627
	ApiCallType_glPauseTransformFeedback,
	// Token: 0x04000274 RID: 628
	ApiCallType_glResumeTransformFeedback,
	// Token: 0x04000275 RID: 629
	ApiCallType_glGetProgramBinary,
	// Token: 0x04000276 RID: 630
	ApiCallType_glProgramBinary,
	// Token: 0x04000277 RID: 631
	ApiCallType_glProgramParameteri,
	// Token: 0x04000278 RID: 632
	ApiCallType_glInvalidateFramebuffer,
	// Token: 0x04000279 RID: 633
	ApiCallType_glInvalidateSubFramebuffer,
	// Token: 0x0400027A RID: 634
	ApiCallType_glTexStorage2D,
	// Token: 0x0400027B RID: 635
	ApiCallType_glTexStorage3D,
	// Token: 0x0400027C RID: 636
	ApiCallType_glGetInternalformativ,
	// Token: 0x0400027D RID: 637
	ApiCallType_glDispatchCompute,
	// Token: 0x0400027E RID: 638
	ApiCallType_glDispatchComputeIndirect,
	// Token: 0x0400027F RID: 639
	ApiCallType_glDrawArraysIndirect,
	// Token: 0x04000280 RID: 640
	ApiCallType_glDrawElementsIndirect,
	// Token: 0x04000281 RID: 641
	ApiCallType_glFramebufferParameteri,
	// Token: 0x04000282 RID: 642
	ApiCallType_glGetFramebufferParameteriv,
	// Token: 0x04000283 RID: 643
	ApiCallType_glGetProgramInterfaceiv,
	// Token: 0x04000284 RID: 644
	ApiCallType_glGetProgramResourceIndex,
	// Token: 0x04000285 RID: 645
	ApiCallType_glGetProgramResourceName,
	// Token: 0x04000286 RID: 646
	ApiCallType_glGetProgramResourceiv,
	// Token: 0x04000287 RID: 647
	ApiCallType_glGetProgramResourceLocation,
	// Token: 0x04000288 RID: 648
	ApiCallType_glUseProgramStages,
	// Token: 0x04000289 RID: 649
	ApiCallType_glActiveShaderProgram,
	// Token: 0x0400028A RID: 650
	ApiCallType_glCreateShaderProgramv,
	// Token: 0x0400028B RID: 651
	ApiCallType_glBindProgramPipeline,
	// Token: 0x0400028C RID: 652
	ApiCallType_glDeleteProgramPipelines,
	// Token: 0x0400028D RID: 653
	ApiCallType_glGenProgramPipelines,
	// Token: 0x0400028E RID: 654
	ApiCallType_glIsProgramPipeline,
	// Token: 0x0400028F RID: 655
	ApiCallType_glGetProgramPipelineiv,
	// Token: 0x04000290 RID: 656
	ApiCallType_glProgramUniform1i,
	// Token: 0x04000291 RID: 657
	ApiCallType_glProgramUniform2i,
	// Token: 0x04000292 RID: 658
	ApiCallType_glProgramUniform3i,
	// Token: 0x04000293 RID: 659
	ApiCallType_glProgramUniform4i,
	// Token: 0x04000294 RID: 660
	ApiCallType_glProgramUniform1ui,
	// Token: 0x04000295 RID: 661
	ApiCallType_glProgramUniform2ui,
	// Token: 0x04000296 RID: 662
	ApiCallType_glProgramUniform3ui,
	// Token: 0x04000297 RID: 663
	ApiCallType_glProgramUniform4ui,
	// Token: 0x04000298 RID: 664
	ApiCallType_glProgramUniform1f,
	// Token: 0x04000299 RID: 665
	ApiCallType_glProgramUniform2f,
	// Token: 0x0400029A RID: 666
	ApiCallType_glProgramUniform3f,
	// Token: 0x0400029B RID: 667
	ApiCallType_glProgramUniform4f,
	// Token: 0x0400029C RID: 668
	ApiCallType_glProgramUniform1iv,
	// Token: 0x0400029D RID: 669
	ApiCallType_glProgramUniform2iv,
	// Token: 0x0400029E RID: 670
	ApiCallType_glProgramUniform3iv,
	// Token: 0x0400029F RID: 671
	ApiCallType_glProgramUniform4iv,
	// Token: 0x040002A0 RID: 672
	ApiCallType_glProgramUniform1uiv,
	// Token: 0x040002A1 RID: 673
	ApiCallType_glProgramUniform2uiv,
	// Token: 0x040002A2 RID: 674
	ApiCallType_glProgramUniform3uiv,
	// Token: 0x040002A3 RID: 675
	ApiCallType_glProgramUniform4uiv,
	// Token: 0x040002A4 RID: 676
	ApiCallType_glProgramUniform1fv,
	// Token: 0x040002A5 RID: 677
	ApiCallType_glProgramUniform2fv,
	// Token: 0x040002A6 RID: 678
	ApiCallType_glProgramUniform3fv,
	// Token: 0x040002A7 RID: 679
	ApiCallType_glProgramUniform4fv,
	// Token: 0x040002A8 RID: 680
	ApiCallType_glProgramUniformMatrix2fv,
	// Token: 0x040002A9 RID: 681
	ApiCallType_glProgramUniformMatrix3fv,
	// Token: 0x040002AA RID: 682
	ApiCallType_glProgramUniformMatrix4fv,
	// Token: 0x040002AB RID: 683
	ApiCallType_glProgramUniformMatrix2x3fv,
	// Token: 0x040002AC RID: 684
	ApiCallType_glProgramUniformMatrix3x2fv,
	// Token: 0x040002AD RID: 685
	ApiCallType_glProgramUniformMatrix2x4fv,
	// Token: 0x040002AE RID: 686
	ApiCallType_glProgramUniformMatrix4x2fv,
	// Token: 0x040002AF RID: 687
	ApiCallType_glProgramUniformMatrix3x4fv,
	// Token: 0x040002B0 RID: 688
	ApiCallType_glProgramUniformMatrix4x3fv,
	// Token: 0x040002B1 RID: 689
	ApiCallType_glValidateProgramPipeline,
	// Token: 0x040002B2 RID: 690
	ApiCallType_glGetProgramPipelineInfoLog,
	// Token: 0x040002B3 RID: 691
	ApiCallType_glGetActiveAtomicCounterBufferiv,
	// Token: 0x040002B4 RID: 692
	ApiCallType_glBindImageTexture,
	// Token: 0x040002B5 RID: 693
	ApiCallType_glMemoryBarrier,
	// Token: 0x040002B6 RID: 694
	ApiCallType_glTexStorage2DMultisample,
	// Token: 0x040002B7 RID: 695
	ApiCallType_glTexStorage3DMultisampleOES,
	// Token: 0x040002B8 RID: 696
	ApiCallType_glGetMultisamplefv,
	// Token: 0x040002B9 RID: 697
	ApiCallType_glSampleMaski,
	// Token: 0x040002BA RID: 698
	ApiCallType_glGetTexLevelParameteriv,
	// Token: 0x040002BB RID: 699
	ApiCallType_glGetTexLevelParameterfv,
	// Token: 0x040002BC RID: 700
	ApiCallType_glBindVertexBuffer,
	// Token: 0x040002BD RID: 701
	ApiCallType_glVertexAttribFormat,
	// Token: 0x040002BE RID: 702
	ApiCallType_glVertexAttribIFormat,
	// Token: 0x040002BF RID: 703
	ApiCallType_glVertexAttribBinding,
	// Token: 0x040002C0 RID: 704
	ApiCallType_glVertexBindingDivisor,
	// Token: 0x040002C1 RID: 705
	ApiCallType_glFramebufferTextureEXT,
	// Token: 0x040002C2 RID: 706
	ApiCallType_glPatchParameteriEXT,
	// Token: 0x040002C3 RID: 707
	ApiCallType_glPatchParameterfv,
	// Token: 0x040002C4 RID: 708
	ApiCallType_glGetFixedvAMD,
	// Token: 0x040002C5 RID: 709
	ApiCallType_glLogicOpAMD,
	// Token: 0x040002C6 RID: 710
	ApiCallType_glFogfvAMD,
	// Token: 0x040002C7 RID: 711
	ApiCallType_glGetMemoryStatsQCOM,
	// Token: 0x040002C8 RID: 712
	ApiCallType_glGetSizedMemoryStatsQCOM,
	// Token: 0x040002C9 RID: 713
	ApiCallType_glBlitOverlapQCOM,
	// Token: 0x040002CA RID: 714
	ApiCallType_glGetShaderStatsQCOM,
	// Token: 0x040002CB RID: 715
	ApiCallType_glExtGetBinningConfigParamivQCOM,
	// Token: 0x040002CC RID: 716
	ApiCallType_glExtGetSamplersQCOM,
	// Token: 0x040002CD RID: 717
	ApiCallType_glClipPlanefQCOM,
	// Token: 0x040002CE RID: 718
	ApiCallType_glFramebufferTexture2DExternalQCOM,
	// Token: 0x040002CF RID: 719
	ApiCallType_glFramebufferRenderbufferExternalQCOM,
	// Token: 0x040002D0 RID: 720
	ApiCallType_glEGLImageTargetTexture2DOES,
	// Token: 0x040002D1 RID: 721
	ApiCallType_glEGLImageTargetRenderbufferStorageOES,
	// Token: 0x040002D2 RID: 722
	ApiCallType_glGetProgramBinaryOES,
	// Token: 0x040002D3 RID: 723
	ApiCallType_glProgramBinaryOES,
	// Token: 0x040002D4 RID: 724
	ApiCallType_glTexImage3DOES,
	// Token: 0x040002D5 RID: 725
	ApiCallType_glTexSubImage3DOES,
	// Token: 0x040002D6 RID: 726
	ApiCallType_glCopyTexSubImage3DOES,
	// Token: 0x040002D7 RID: 727
	ApiCallType_glCompressedTexImage3DOES,
	// Token: 0x040002D8 RID: 728
	ApiCallType_glCompressedTexSubImage3DOES,
	// Token: 0x040002D9 RID: 729
	ApiCallType_glFramebufferTexture3DOES,
	// Token: 0x040002DA RID: 730
	ApiCallType_glBindVertexArrayOES,
	// Token: 0x040002DB RID: 731
	ApiCallType_glDeleteVertexArraysOES,
	// Token: 0x040002DC RID: 732
	ApiCallType_glGenVertexArraysOES,
	// Token: 0x040002DD RID: 733
	ApiCallType_glIsVertexArrayOES,
	// Token: 0x040002DE RID: 734
	ApiCallType_glGetPerfMonitorGroupsAMD,
	// Token: 0x040002DF RID: 735
	ApiCallType_glGetPerfMonitorCountersAMD,
	// Token: 0x040002E0 RID: 736
	ApiCallType_glGetPerfMonitorGroupStringAMD,
	// Token: 0x040002E1 RID: 737
	ApiCallType_glGetPerfMonitorCounterStringAMD,
	// Token: 0x040002E2 RID: 738
	ApiCallType_glGetPerfMonitorCounterInfoAMD,
	// Token: 0x040002E3 RID: 739
	ApiCallType_glGenPerfMonitorsAMD,
	// Token: 0x040002E4 RID: 740
	ApiCallType_glDeletePerfMonitorsAMD,
	// Token: 0x040002E5 RID: 741
	ApiCallType_glSelectPerfMonitorCountersAMD,
	// Token: 0x040002E6 RID: 742
	ApiCallType_glBeginPerfMonitorAMD,
	// Token: 0x040002E7 RID: 743
	ApiCallType_glEndPerfMonitorAMD,
	// Token: 0x040002E8 RID: 744
	ApiCallType_glGetPerfMonitorCounterDataAMD,
	// Token: 0x040002E9 RID: 745
	ApiCallType_glBlitFramebufferANGLE,
	// Token: 0x040002EA RID: 746
	ApiCallType_glRenderbufferStorageMultisampleANGLE,
	// Token: 0x040002EB RID: 747
	ApiCallType_glLabelObjectEXT,
	// Token: 0x040002EC RID: 748
	ApiCallType_glGetObjectLabelEXT,
	// Token: 0x040002ED RID: 749
	ApiCallType_glInsertEventMarkerEXT,
	// Token: 0x040002EE RID: 750
	ApiCallType_glPushGroupMarkerEXT,
	// Token: 0x040002EF RID: 751
	ApiCallType_glPopGroupMarkerEXT,
	// Token: 0x040002F0 RID: 752
	ApiCallType_glDiscardFramebufferEXT,
	// Token: 0x040002F1 RID: 753
	ApiCallType_glGenQueriesEXT,
	// Token: 0x040002F2 RID: 754
	ApiCallType_glDeleteQueriesEXT,
	// Token: 0x040002F3 RID: 755
	ApiCallType_glIsQueryEXT,
	// Token: 0x040002F4 RID: 756
	ApiCallType_glBeginQueryEXT,
	// Token: 0x040002F5 RID: 757
	ApiCallType_glEndQueryEXT,
	// Token: 0x040002F6 RID: 758
	ApiCallType_glQueryCounterEXT,
	// Token: 0x040002F7 RID: 759
	ApiCallType_glGetQueryivEXT,
	// Token: 0x040002F8 RID: 760
	ApiCallType_glGetQueryObjectivEXT,
	// Token: 0x040002F9 RID: 761
	ApiCallType_glGetQueryObjectuivEXT,
	// Token: 0x040002FA RID: 762
	ApiCallType_glGetQueryObjecti64vEXT,
	// Token: 0x040002FB RID: 763
	ApiCallType_glGetQueryObjectui64vEXT,
	// Token: 0x040002FC RID: 764
	ApiCallType_glGetGraphicsResetStatusEXT,
	// Token: 0x040002FD RID: 765
	ApiCallType_glReadnPixelsEXT,
	// Token: 0x040002FE RID: 766
	ApiCallType_glGetnUniformfvEXT,
	// Token: 0x040002FF RID: 767
	ApiCallType_glGetnUniformivEXT,
	// Token: 0x04000300 RID: 768
	ApiCallType_glDeleteFencesNV,
	// Token: 0x04000301 RID: 769
	ApiCallType_glGenFencesNV,
	// Token: 0x04000302 RID: 770
	ApiCallType_glIsFenceNV,
	// Token: 0x04000303 RID: 771
	ApiCallType_glTestFenceNV,
	// Token: 0x04000304 RID: 772
	ApiCallType_glGetFenceivNV,
	// Token: 0x04000305 RID: 773
	ApiCallType_glFinishFenceNV,
	// Token: 0x04000306 RID: 774
	ApiCallType_glSetFenceNV,
	// Token: 0x04000307 RID: 775
	ApiCallType_glAlphaFuncQCOM,
	// Token: 0x04000308 RID: 776
	ApiCallType_glGetDriverControlsQCOM,
	// Token: 0x04000309 RID: 777
	ApiCallType_glGetDriverControlStringQCOM,
	// Token: 0x0400030A RID: 778
	ApiCallType_glEnableDriverControlQCOM,
	// Token: 0x0400030B RID: 779
	ApiCallType_glDisableDriverControlQCOM,
	// Token: 0x0400030C RID: 780
	ApiCallType_glExtGetTexturesQCOM,
	// Token: 0x0400030D RID: 781
	ApiCallType_glExtGetBuffersQCOM,
	// Token: 0x0400030E RID: 782
	ApiCallType_glExtGetRenderbuffersQCOM,
	// Token: 0x0400030F RID: 783
	ApiCallType_glExtGetFramebuffersQCOM,
	// Token: 0x04000310 RID: 784
	ApiCallType_glExtGetTexLevelParameterivQCOM,
	// Token: 0x04000311 RID: 785
	ApiCallType_glExtTexObjectStateOverrideiQCOM,
	// Token: 0x04000312 RID: 786
	ApiCallType_glExtGetTexSubImageQCOM,
	// Token: 0x04000313 RID: 787
	ApiCallType_glExtGetBufferPointervQCOM,
	// Token: 0x04000314 RID: 788
	ApiCallType_glExtGetShadersQCOM,
	// Token: 0x04000315 RID: 789
	ApiCallType_glExtGetProgramsQCOM,
	// Token: 0x04000316 RID: 790
	ApiCallType_glExtIsProgramBinaryQCOM,
	// Token: 0x04000317 RID: 791
	ApiCallType_glExtGetProgramBinarySourceQCOM,
	// Token: 0x04000318 RID: 792
	ApiCallType_glStartTilingQCOM,
	// Token: 0x04000319 RID: 793
	ApiCallType_glEndTilingQCOM,
	// Token: 0x0400031A RID: 794
	ApiCallType_glTexParameterIivEXT,
	// Token: 0x0400031B RID: 795
	ApiCallType_glTexParameterIuivEXT,
	// Token: 0x0400031C RID: 796
	ApiCallType_glGetTexParameterIivEXT,
	// Token: 0x0400031D RID: 797
	ApiCallType_glGetTexParameterIuivEXT,
	// Token: 0x0400031E RID: 798
	ApiCallType_glSamplerParameterIivEXT,
	// Token: 0x0400031F RID: 799
	ApiCallType_glSamplerParameterIuivEXT,
	// Token: 0x04000320 RID: 800
	ApiCallType_glGetSamplerParameterIivEXT,
	// Token: 0x04000321 RID: 801
	ApiCallType_glGetSamplerParameterIuivEXT,
	// Token: 0x04000322 RID: 802
	ApiCallType_glCopyImageSubDataEXT,
	// Token: 0x04000323 RID: 803
	ApiCallType_glTexBufferEXT,
	// Token: 0x04000324 RID: 804
	ApiCallType_glTexBufferRangeEXT,
	// Token: 0x04000325 RID: 805
	ApiCallType_glGetBooleani_v,
	// Token: 0x04000326 RID: 806
	ApiCallType_glEnableiEXT,
	// Token: 0x04000327 RID: 807
	ApiCallType_glDisableiEXT,
	// Token: 0x04000328 RID: 808
	ApiCallType_glBlendEquationiEXT,
	// Token: 0x04000329 RID: 809
	ApiCallType_glBlendEquationSeparateiEXT,
	// Token: 0x0400032A RID: 810
	ApiCallType_glBlendFunciEXT,
	// Token: 0x0400032B RID: 811
	ApiCallType_glBlendFuncSeparateiEXT,
	// Token: 0x0400032C RID: 812
	ApiCallType_glColorMaskiEXT,
	// Token: 0x0400032D RID: 813
	ApiCallType_glIsEnablediEXT,
	// Token: 0x0400032E RID: 814
	ApiCallType_glMinSampleShadingOES,
	// Token: 0x0400032F RID: 815
	ApiCallType_glMemoryBarrierByRegion,
	// Token: 0x04000330 RID: 816
	ApiCallType_glRenderbufferStorageMultisampleEXT,
	// Token: 0x04000331 RID: 817
	ApiCallType_glFramebufferTexture2DMultisampleEXT,
	// Token: 0x04000332 RID: 818
	ApiCallType_glBlendBarrierKHR,
	// Token: 0x04000333 RID: 819
	ApiCallType_glPrimitiveBoundingBoxEXT,
	// Token: 0x04000334 RID: 820
	ApiCallType_glDebugMessageControlKHR,
	// Token: 0x04000335 RID: 821
	ApiCallType_glDebugMessageInsertKHR,
	// Token: 0x04000336 RID: 822
	ApiCallType_glDebugMessageCallbackKHR,
	// Token: 0x04000337 RID: 823
	ApiCallType_glGetDebugMessageLogKHR,
	// Token: 0x04000338 RID: 824
	ApiCallType_glPushDebugGroupKHR,
	// Token: 0x04000339 RID: 825
	ApiCallType_glPopDebugGroupKHR,
	// Token: 0x0400033A RID: 826
	ApiCallType_glObjectLabelKHR,
	// Token: 0x0400033B RID: 827
	ApiCallType_glGetObjectLabelKHR,
	// Token: 0x0400033C RID: 828
	ApiCallType_glObjectPtrLabelKHR,
	// Token: 0x0400033D RID: 829
	ApiCallType_glGetObjectPtrLabelKHR,
	// Token: 0x0400033E RID: 830
	ApiCallType_glGetPointervKHR,
	// Token: 0x0400033F RID: 831
	ApiCallType_glFramebufferTextureMultiviewOVR,
	// Token: 0x04000340 RID: 832
	ApiCallType_glNumBinsPerSubmitQCOM,
	// Token: 0x04000341 RID: 833
	ApiCallType_glFramebufferTextureMultisampleMultiviewOVR,
	// Token: 0x04000342 RID: 834
	ApiCallType_glBlitBlendColor,
	// Token: 0x04000343 RID: 835
	ApiCallType_glBlitBlendEquationSeparate,
	// Token: 0x04000344 RID: 836
	ApiCallType_glBlitBlendFuncSeparate,
	// Token: 0x04000345 RID: 837
	ApiCallType_glBlitRotation,
	// Token: 0x04000346 RID: 838
	ApiCallType_glBufferStorageEXT,
	// Token: 0x04000347 RID: 839
	ApiCallType_glDebugMessageControl,
	// Token: 0x04000348 RID: 840
	ApiCallType_glDebugMessageInsert,
	// Token: 0x04000349 RID: 841
	ApiCallType_glDebugMessageCallback,
	// Token: 0x0400034A RID: 842
	ApiCallType_glGetDebugMessageLog,
	// Token: 0x0400034B RID: 843
	ApiCallType_glGetPointerv,
	// Token: 0x0400034C RID: 844
	ApiCallType_glPushDebugGroup,
	// Token: 0x0400034D RID: 845
	ApiCallType_glObjectLabel,
	// Token: 0x0400034E RID: 846
	ApiCallType_glGetObjectLabel,
	// Token: 0x0400034F RID: 847
	ApiCallType_glObjectPtrLabel,
	// Token: 0x04000350 RID: 848
	ApiCallType_glGetObjectPtrLabel,
	// Token: 0x04000351 RID: 849
	ApiCallType_glPopDebugGroup,
	// Token: 0x04000352 RID: 850
	ApiCallType_glBlendBarrier,
	// Token: 0x04000353 RID: 851
	ApiCallType_glMinSampleShading,
	// Token: 0x04000354 RID: 852
	ApiCallType_glGetGraphicsResetStatus,
	// Token: 0x04000355 RID: 853
	ApiCallType_glReadnPixels,
	// Token: 0x04000356 RID: 854
	ApiCallType_glGetnUniformfv,
	// Token: 0x04000357 RID: 855
	ApiCallType_glGetnUniformiv,
	// Token: 0x04000358 RID: 856
	ApiCallType_glCopyImageSubData,
	// Token: 0x04000359 RID: 857
	ApiCallType_glEnablei,
	// Token: 0x0400035A RID: 858
	ApiCallType_glDisablei,
	// Token: 0x0400035B RID: 859
	ApiCallType_glBlendEquationi,
	// Token: 0x0400035C RID: 860
	ApiCallType_glBlendEquationSeparatei,
	// Token: 0x0400035D RID: 861
	ApiCallType_glBlendFunci,
	// Token: 0x0400035E RID: 862
	ApiCallType_glBlendFuncSeparatei,
	// Token: 0x0400035F RID: 863
	ApiCallType_glColorMaski,
	// Token: 0x04000360 RID: 864
	ApiCallType_glIsEnabledi,
	// Token: 0x04000361 RID: 865
	ApiCallType_glPrimitiveBoundingBox,
	// Token: 0x04000362 RID: 866
	ApiCallType_glTexParameterIiv,
	// Token: 0x04000363 RID: 867
	ApiCallType_glTexParameterIuiv,
	// Token: 0x04000364 RID: 868
	ApiCallType_glGetTexParameterIiv,
	// Token: 0x04000365 RID: 869
	ApiCallType_glGetTexParameterIuiv,
	// Token: 0x04000366 RID: 870
	ApiCallType_glSamplerParameterIiv,
	// Token: 0x04000367 RID: 871
	ApiCallType_glSamplerParameterIuiv,
	// Token: 0x04000368 RID: 872
	ApiCallType_glGetSamplerParameterIiv,
	// Token: 0x04000369 RID: 873
	ApiCallType_glGetSamplerParameterIuiv,
	// Token: 0x0400036A RID: 874
	ApiCallType_glTexBuffer,
	// Token: 0x0400036B RID: 875
	ApiCallType_glTexBufferRange,
	// Token: 0x0400036C RID: 876
	ApiCallType_glPatchParameteri,
	// Token: 0x0400036D RID: 877
	ApiCallType_glDrawElementsBaseVertex,
	// Token: 0x0400036E RID: 878
	ApiCallType_glDrawRangeElementsBaseVertex,
	// Token: 0x0400036F RID: 879
	ApiCallType_glDrawElementsInstancedBaseVertex,
	// Token: 0x04000370 RID: 880
	ApiCallType_glGetnUniformuiv,
	// Token: 0x04000371 RID: 881
	ApiCallType_glCreateSharedBufferQCOM = 37355,
	// Token: 0x04000372 RID: 882
	ApiCallType_glDestroySharedBufferQCOM,
	// Token: 0x04000373 RID: 883
	ApiCallType_glTextureBarrier,
	// Token: 0x04000374 RID: 884
	ApiCallType_glFramebufferFoveationConfigQCOM,
	// Token: 0x04000375 RID: 885
	ApiCallType_glFramebufferFoveationParametersQCOM,
	// Token: 0x04000376 RID: 886
	ApiCallType_glBufferStorageExternalEXT,
	// Token: 0x04000377 RID: 887
	ApiCallType_glFramebufferFetchBarrierQCOM,
	// Token: 0x04000378 RID: 888
	ApiCallType_glCreateMemoryObjectsEXT,
	// Token: 0x04000379 RID: 889
	ApiCallType_glDeleteMemoryObjectsEXT,
	// Token: 0x0400037A RID: 890
	ApiCallType_glIsMemoryObjectEXT,
	// Token: 0x0400037B RID: 891
	ApiCallType_glTexStorageMem2DEXT,
	// Token: 0x0400037C RID: 892
	ApiCallType_glTexStorageMem2DMultisampleEXT,
	// Token: 0x0400037D RID: 893
	ApiCallType_glTexStorageMem3DEXT,
	// Token: 0x0400037E RID: 894
	ApiCallType_glTexStorageMem3DMultisampleEXT,
	// Token: 0x0400037F RID: 895
	ApiCallType_glBufferStorageMemEXT,
	// Token: 0x04000380 RID: 896
	ApiCallType_glGenSemaphoresKHR,
	// Token: 0x04000381 RID: 897
	ApiCallType_glDeleteSemaphoresKHR,
	// Token: 0x04000382 RID: 898
	ApiCallType_glIsSemaphoreKHR,
	// Token: 0x04000383 RID: 899
	ApiCallType_glWaitSemaphoreKHR,
	// Token: 0x04000384 RID: 900
	ApiCallType_glSignalSemaphoreKHR,
	// Token: 0x04000385 RID: 901
	ApiCallType_glImportMemoryFdEXT,
	// Token: 0x04000386 RID: 902
	ApiCallType_glImportSemaphoreFdEXT,
	// Token: 0x04000387 RID: 903
	ApiCallType_glGetUnsignedBytevEXT,
	// Token: 0x04000388 RID: 904
	ApiCallType_glGetUnsignedBytei_vEXT,
	// Token: 0x04000389 RID: 905
	ApiCallType_glMemoryObjectParameterivEXT,
	// Token: 0x0400038A RID: 906
	ApiCallType_glGetMemoryObjectParameterivEXT,
	// Token: 0x0400038B RID: 907
	ApiCallType_glBindSharedBufferQCOM,
	// Token: 0x0400038C RID: 908
	ApiCallType_glTextureFoveationParametersQCOM,
	// Token: 0x0400038D RID: 909
	ApiCallType_glEGLImageTargetTexStorageEXT,
	// Token: 0x0400038E RID: 910
	ApiCallType_glBindFragDataLocationIndexedEXT,
	// Token: 0x0400038F RID: 911
	ApiCallType_glBindFragDataLocationEXT,
	// Token: 0x04000390 RID: 912
	ApiCallType_glGetProgramResourceLocationIndexEXT,
	// Token: 0x04000391 RID: 913
	ApiCallType_glGetFragDataIndexEXT,
	// Token: 0x04000392 RID: 914
	ApiCallType_glClipControlEXT,
	// Token: 0x04000393 RID: 915
	ApiCallType_glTextureViewOES,
	// Token: 0x04000394 RID: 916
	ApiCallType_glShadingRateQCOM = 37393,
	// Token: 0x04000395 RID: 917
	ApiCallType_glExtrapolateTex2DQCOM,
	// Token: 0x04000396 RID: 918
	ApiCallType_glTexEstimateMotionQCOM = 37396,
	// Token: 0x04000397 RID: 919
	ApiCallType_glTexEstimateMotionRegionsQCOM,
	// Token: 0x04000398 RID: 920
	ApiCallType_glPolygonOffsetClampEXT,
	// Token: 0x04000399 RID: 921
	ApiCallType_glGetFragmentShadingRatesEXT,
	// Token: 0x0400039A RID: 922
	ApiCallType_glShadingRateEXT,
	// Token: 0x0400039B RID: 923
	ApiCallType_glShadingRateCombinerOpsEXT,
	// Token: 0x0400039C RID: 924
	ApiCallType_glFramebufferShadingRateEXT,
	// Token: 0x0400039D RID: 925
	ApiCallType_glLast,
	// Token: 0x0400039E RID: 926
	ApiCallType_vkCreateInstance = 40960,
	// Token: 0x0400039F RID: 927
	ApiCallType_vkDestroyInstance,
	// Token: 0x040003A0 RID: 928
	ApiCallType_vkEnumeratePhysicalDevices,
	// Token: 0x040003A1 RID: 929
	ApiCallType_vkGetPhysicalDeviceFeatures,
	// Token: 0x040003A2 RID: 930
	ApiCallType_vkGetPhysicalDeviceFormatProperties,
	// Token: 0x040003A3 RID: 931
	ApiCallType_vkGetPhysicalDeviceImageFormatProperties,
	// Token: 0x040003A4 RID: 932
	ApiCallType_vkGetPhysicalDeviceProperties,
	// Token: 0x040003A5 RID: 933
	ApiCallType_vkGetPhysicalDeviceQueueFamilyProperties,
	// Token: 0x040003A6 RID: 934
	ApiCallType_vkGetPhysicalDeviceMemoryProperties,
	// Token: 0x040003A7 RID: 935
	ApiCallType_vkGetInstanceProcAddr,
	// Token: 0x040003A8 RID: 936
	ApiCallType_vkGetDeviceProcAddr,
	// Token: 0x040003A9 RID: 937
	ApiCallType_vkCreateDevice,
	// Token: 0x040003AA RID: 938
	ApiCallType_vkDestroyDevice,
	// Token: 0x040003AB RID: 939
	ApiCallType_vkEnumerateInstanceExtensionProperties,
	// Token: 0x040003AC RID: 940
	ApiCallType_vkEnumerateDeviceExtensionProperties,
	// Token: 0x040003AD RID: 941
	ApiCallType_vkEnumerateInstanceLayerProperties,
	// Token: 0x040003AE RID: 942
	ApiCallType_vkEnumerateDeviceLayerProperties,
	// Token: 0x040003AF RID: 943
	ApiCallType_vkGetDeviceQueue,
	// Token: 0x040003B0 RID: 944
	ApiCallType_vkQueueSubmit,
	// Token: 0x040003B1 RID: 945
	ApiCallType_vkQueueWaitIdle,
	// Token: 0x040003B2 RID: 946
	ApiCallType_vkDeviceWaitIdle,
	// Token: 0x040003B3 RID: 947
	ApiCallType_vkAllocateMemory,
	// Token: 0x040003B4 RID: 948
	ApiCallType_vkFreeMemory,
	// Token: 0x040003B5 RID: 949
	ApiCallType_vkMapMemory,
	// Token: 0x040003B6 RID: 950
	ApiCallType_vkUnmapMemory,
	// Token: 0x040003B7 RID: 951
	ApiCallType_vkFlushMappedMemoryRanges,
	// Token: 0x040003B8 RID: 952
	ApiCallType_vkInvalidateMappedMemoryRanges,
	// Token: 0x040003B9 RID: 953
	ApiCallType_vkGetDeviceMemoryCommitment,
	// Token: 0x040003BA RID: 954
	ApiCallType_vkBindBufferMemory,
	// Token: 0x040003BB RID: 955
	ApiCallType_vkBindImageMemory,
	// Token: 0x040003BC RID: 956
	ApiCallType_vkGetBufferMemoryRequirements,
	// Token: 0x040003BD RID: 957
	ApiCallType_vkGetImageMemoryRequirements,
	// Token: 0x040003BE RID: 958
	ApiCallType_vkGetImageSparseMemoryRequirements,
	// Token: 0x040003BF RID: 959
	ApiCallType_vkGetPhysicalDeviceSparseImageFormatProperties,
	// Token: 0x040003C0 RID: 960
	ApiCallType_vkQueueBindSparse,
	// Token: 0x040003C1 RID: 961
	ApiCallType_vkCreateFence,
	// Token: 0x040003C2 RID: 962
	ApiCallType_vkDestroyFence,
	// Token: 0x040003C3 RID: 963
	ApiCallType_vkResetFences,
	// Token: 0x040003C4 RID: 964
	ApiCallType_vkGetFenceStatus,
	// Token: 0x040003C5 RID: 965
	ApiCallType_vkWaitForFences,
	// Token: 0x040003C6 RID: 966
	ApiCallType_vkCreateSemaphore,
	// Token: 0x040003C7 RID: 967
	ApiCallType_vkDestroySemaphore,
	// Token: 0x040003C8 RID: 968
	ApiCallType_vkCreateEvent,
	// Token: 0x040003C9 RID: 969
	ApiCallType_vkDestroyEvent,
	// Token: 0x040003CA RID: 970
	ApiCallType_vkGetEventStatus,
	// Token: 0x040003CB RID: 971
	ApiCallType_vkSetEvent,
	// Token: 0x040003CC RID: 972
	ApiCallType_vkResetEvent,
	// Token: 0x040003CD RID: 973
	ApiCallType_vkCreateQueryPool,
	// Token: 0x040003CE RID: 974
	ApiCallType_vkDestroyQueryPool,
	// Token: 0x040003CF RID: 975
	ApiCallType_vkGetQueryPoolResults,
	// Token: 0x040003D0 RID: 976
	ApiCallType_vkCreateBuffer,
	// Token: 0x040003D1 RID: 977
	ApiCallType_vkDestroyBuffer,
	// Token: 0x040003D2 RID: 978
	ApiCallType_vkCreateBufferView,
	// Token: 0x040003D3 RID: 979
	ApiCallType_vkDestroyBufferView,
	// Token: 0x040003D4 RID: 980
	ApiCallType_vkCreateImage,
	// Token: 0x040003D5 RID: 981
	ApiCallType_vkDestroyImage,
	// Token: 0x040003D6 RID: 982
	ApiCallType_vkGetImageSubresourceLayout,
	// Token: 0x040003D7 RID: 983
	ApiCallType_vkCreateImageView,
	// Token: 0x040003D8 RID: 984
	ApiCallType_vkDestroyImageView,
	// Token: 0x040003D9 RID: 985
	ApiCallType_vkCreateShaderModule,
	// Token: 0x040003DA RID: 986
	ApiCallType_vkDestroyShaderModule,
	// Token: 0x040003DB RID: 987
	ApiCallType_vkCreatePipelineCache,
	// Token: 0x040003DC RID: 988
	ApiCallType_vkDestroyPipelineCache,
	// Token: 0x040003DD RID: 989
	ApiCallType_vkGetPipelineCacheData,
	// Token: 0x040003DE RID: 990
	ApiCallType_vkMergePipelineCaches,
	// Token: 0x040003DF RID: 991
	ApiCallType_vkCreateGraphicsPipelines,
	// Token: 0x040003E0 RID: 992
	ApiCallType_vkCreateComputePipelines,
	// Token: 0x040003E1 RID: 993
	ApiCallType_vkDestroyPipeline,
	// Token: 0x040003E2 RID: 994
	ApiCallType_vkCreatePipelineLayout,
	// Token: 0x040003E3 RID: 995
	ApiCallType_vkDestroyPipelineLayout,
	// Token: 0x040003E4 RID: 996
	ApiCallType_vkCreateSampler,
	// Token: 0x040003E5 RID: 997
	ApiCallType_vkDestroySampler,
	// Token: 0x040003E6 RID: 998
	ApiCallType_vkCreateDescriptorSetLayout,
	// Token: 0x040003E7 RID: 999
	ApiCallType_vkDestroyDescriptorSetLayout,
	// Token: 0x040003E8 RID: 1000
	ApiCallType_vkCreateDescriptorPool,
	// Token: 0x040003E9 RID: 1001
	ApiCallType_vkDestroyDescriptorPool,
	// Token: 0x040003EA RID: 1002
	ApiCallType_vkResetDescriptorPool,
	// Token: 0x040003EB RID: 1003
	ApiCallType_vkAllocateDescriptorSets,
	// Token: 0x040003EC RID: 1004
	ApiCallType_vkFreeDescriptorSets,
	// Token: 0x040003ED RID: 1005
	ApiCallType_vkUpdateDescriptorSets,
	// Token: 0x040003EE RID: 1006
	ApiCallType_vkCreateFramebuffer,
	// Token: 0x040003EF RID: 1007
	ApiCallType_vkDestroyFramebuffer,
	// Token: 0x040003F0 RID: 1008
	ApiCallType_vkCreateRenderPass,
	// Token: 0x040003F1 RID: 1009
	ApiCallType_vkDestroyRenderPass,
	// Token: 0x040003F2 RID: 1010
	ApiCallType_vkGetRenderAreaGranularity,
	// Token: 0x040003F3 RID: 1011
	ApiCallType_vkCreateCommandPool,
	// Token: 0x040003F4 RID: 1012
	ApiCallType_vkDestroyCommandPool,
	// Token: 0x040003F5 RID: 1013
	ApiCallType_vkResetCommandPool,
	// Token: 0x040003F6 RID: 1014
	ApiCallType_vkAllocateCommandBuffers,
	// Token: 0x040003F7 RID: 1015
	ApiCallType_vkFreeCommandBuffers,
	// Token: 0x040003F8 RID: 1016
	ApiCallType_vkBeginCommandBuffer,
	// Token: 0x040003F9 RID: 1017
	ApiCallType_vkEndCommandBuffer,
	// Token: 0x040003FA RID: 1018
	ApiCallType_vkResetCommandBuffer,
	// Token: 0x040003FB RID: 1019
	ApiCallType_vkCmdBindPipeline,
	// Token: 0x040003FC RID: 1020
	ApiCallType_vkCmdSetViewport,
	// Token: 0x040003FD RID: 1021
	ApiCallType_vkCmdSetScissor,
	// Token: 0x040003FE RID: 1022
	ApiCallType_vkCmdSetLineWidth,
	// Token: 0x040003FF RID: 1023
	ApiCallType_vkCmdSetDepthBias,
	// Token: 0x04000400 RID: 1024
	ApiCallType_vkCmdSetBlendConstants,
	// Token: 0x04000401 RID: 1025
	ApiCallType_vkCmdSetDepthBounds,
	// Token: 0x04000402 RID: 1026
	ApiCallType_vkCmdSetStencilCompareMask,
	// Token: 0x04000403 RID: 1027
	ApiCallType_vkCmdSetStencilWriteMask,
	// Token: 0x04000404 RID: 1028
	ApiCallType_vkCmdSetStencilReference,
	// Token: 0x04000405 RID: 1029
	ApiCallType_vkCmdBindDescriptorSets,
	// Token: 0x04000406 RID: 1030
	ApiCallType_vkCmdBindIndexBuffer,
	// Token: 0x04000407 RID: 1031
	ApiCallType_vkCmdBindVertexBuffers,
	// Token: 0x04000408 RID: 1032
	ApiCallType_vkCmdDraw,
	// Token: 0x04000409 RID: 1033
	ApiCallType_vkCmdDrawIndexed,
	// Token: 0x0400040A RID: 1034
	ApiCallType_vkCmdDrawIndirect,
	// Token: 0x0400040B RID: 1035
	ApiCallType_vkCmdDrawIndexedIndirect,
	// Token: 0x0400040C RID: 1036
	ApiCallType_vkCmdDispatch,
	// Token: 0x0400040D RID: 1037
	ApiCallType_vkCmdDispatchIndirect,
	// Token: 0x0400040E RID: 1038
	ApiCallType_vkCmdCopyBuffer,
	// Token: 0x0400040F RID: 1039
	ApiCallType_vkCmdCopyImage,
	// Token: 0x04000410 RID: 1040
	ApiCallType_vkCmdBlitImage,
	// Token: 0x04000411 RID: 1041
	ApiCallType_vkCmdCopyBufferToImage,
	// Token: 0x04000412 RID: 1042
	ApiCallType_vkCmdCopyImageToBuffer,
	// Token: 0x04000413 RID: 1043
	ApiCallType_vkCmdUpdateBuffer,
	// Token: 0x04000414 RID: 1044
	ApiCallType_vkCmdFillBuffer,
	// Token: 0x04000415 RID: 1045
	ApiCallType_vkCmdClearColorImage,
	// Token: 0x04000416 RID: 1046
	ApiCallType_vkCmdClearDepthStencilImage,
	// Token: 0x04000417 RID: 1047
	ApiCallType_vkCmdClearAttachments,
	// Token: 0x04000418 RID: 1048
	ApiCallType_vkCmdResolveImage,
	// Token: 0x04000419 RID: 1049
	ApiCallType_vkCmdSetEvent,
	// Token: 0x0400041A RID: 1050
	ApiCallType_vkCmdResetEvent,
	// Token: 0x0400041B RID: 1051
	ApiCallType_vkCmdWaitEvents,
	// Token: 0x0400041C RID: 1052
	ApiCallType_vkCmdPipelineBarrier,
	// Token: 0x0400041D RID: 1053
	ApiCallType_vkCmdBeginQuery,
	// Token: 0x0400041E RID: 1054
	ApiCallType_vkCmdEndQuery,
	// Token: 0x0400041F RID: 1055
	ApiCallType_vkCmdResetQueryPool,
	// Token: 0x04000420 RID: 1056
	ApiCallType_vkCmdWriteTimestamp,
	// Token: 0x04000421 RID: 1057
	ApiCallType_vkCmdCopyQueryPoolResults,
	// Token: 0x04000422 RID: 1058
	ApiCallType_vkCmdPushConstants,
	// Token: 0x04000423 RID: 1059
	ApiCallType_vkCmdBeginRenderPass,
	// Token: 0x04000424 RID: 1060
	ApiCallType_vkCmdNextSubpass,
	// Token: 0x04000425 RID: 1061
	ApiCallType_vkCmdEndRenderPass,
	// Token: 0x04000426 RID: 1062
	ApiCallType_vkCmdExecuteCommands,
	// Token: 0x04000427 RID: 1063
	ApiCallType_vkAcquireNextImageKHR,
	// Token: 0x04000428 RID: 1064
	ApiCallType_vkCreateSwapchainKHR,
	// Token: 0x04000429 RID: 1065
	ApiCallType_vkDestroySwapchainKHR,
	// Token: 0x0400042A RID: 1066
	ApiCallType_vkGetSwapchainImagesKHR,
	// Token: 0x0400042B RID: 1067
	ApiCallType_vkQueuePresentKHR,
	// Token: 0x0400042C RID: 1068
	ApiCallType_updateSharedMemory = 48059,
	// Token: 0x0400042D RID: 1069
	ApiCallType_vkLast,
	// Token: 0x0400042E RID: 1070
	ApiCallType_D3D12CreateDevice = 49152,
	// Token: 0x0400042F RID: 1071
	ApiCallType_ID3D12Object__GetPrivateData,
	// Token: 0x04000430 RID: 1072
	ApiCallType_ID3D12Object__SetPrivateData,
	// Token: 0x04000431 RID: 1073
	ApiCallType_ID3D12Object__SetPrivateDataInterface,
	// Token: 0x04000432 RID: 1074
	ApiCallType_ID3D12Object__SetName,
	// Token: 0x04000433 RID: 1075
	ApiCallType_ID3D12DeviceChild__GetDevice,
	// Token: 0x04000434 RID: 1076
	ApiCallType_ID3D12RootSignatureDeserializer__GetRootSignatureDesc,
	// Token: 0x04000435 RID: 1077
	ApiCallType_ID3D12VersionedRootSignatureDeserializer__GetRootSignatureDescAtVersion,
	// Token: 0x04000436 RID: 1078
	ApiCallType_ID3D12VersionedRootSignatureDeserializer__GetUnconvertedRootSignatureDesc,
	// Token: 0x04000437 RID: 1079
	ApiCallType_ID3D12Heap__GetDesc,
	// Token: 0x04000438 RID: 1080
	ApiCallType_ID3D12Resource__Map,
	// Token: 0x04000439 RID: 1081
	ApiCallType_ID3D12Resource__Unmap,
	// Token: 0x0400043A RID: 1082
	ApiCallType_ID3D12Resource__GetDesc,
	// Token: 0x0400043B RID: 1083
	ApiCallType_ID3D12Resource__GetGPUVirtualAddress,
	// Token: 0x0400043C RID: 1084
	ApiCallType_ID3D12Resource__WriteToSubresource,
	// Token: 0x0400043D RID: 1085
	ApiCallType_ID3D12Resource__ReadFromSubresource,
	// Token: 0x0400043E RID: 1086
	ApiCallType_ID3D12Resource__GetHeapProperties,
	// Token: 0x0400043F RID: 1087
	ApiCallType_ID3D12CommandAllocator__Reset,
	// Token: 0x04000440 RID: 1088
	ApiCallType_ID3D12Fence__GetCompletedValue,
	// Token: 0x04000441 RID: 1089
	ApiCallType_ID3D12Fence__SetEventOnCompletion,
	// Token: 0x04000442 RID: 1090
	ApiCallType_ID3D12Fence__Signal,
	// Token: 0x04000443 RID: 1091
	ApiCallType_ID3D12Fence1__GetCreationFlags,
	// Token: 0x04000444 RID: 1092
	ApiCallType_ID3D12PipelineState__GetCachedBlob,
	// Token: 0x04000445 RID: 1093
	ApiCallType_ID3D12DescriptorHeap__GetDesc,
	// Token: 0x04000446 RID: 1094
	ApiCallType_ID3D12DescriptorHeap__GetCPUDescriptorHandleForHeapStart,
	// Token: 0x04000447 RID: 1095
	ApiCallType_ID3D12DescriptorHeap__GetGPUDescriptorHandleForHeapStart,
	// Token: 0x04000448 RID: 1096
	ApiCallType_ID3D12CommandList__GetType,
	// Token: 0x04000449 RID: 1097
	ApiCallType_ID3D12GraphicsCommandList__Close,
	// Token: 0x0400044A RID: 1098
	ApiCallType_ID3D12GraphicsCommandList__Reset,
	// Token: 0x0400044B RID: 1099
	ApiCallType_ID3D12GraphicsCommandList__ClearState,
	// Token: 0x0400044C RID: 1100
	ApiCallType_ID3D12GraphicsCommandList__DrawInstanced,
	// Token: 0x0400044D RID: 1101
	ApiCallType_ID3D12GraphicsCommandList__DrawIndexedInstanced,
	// Token: 0x0400044E RID: 1102
	ApiCallType_ID3D12GraphicsCommandList__Dispatch,
	// Token: 0x0400044F RID: 1103
	ApiCallType_ID3D12GraphicsCommandList__CopyBufferRegion,
	// Token: 0x04000450 RID: 1104
	ApiCallType_ID3D12GraphicsCommandList__CopyTextureRegion,
	// Token: 0x04000451 RID: 1105
	ApiCallType_ID3D12GraphicsCommandList__CopyResource,
	// Token: 0x04000452 RID: 1106
	ApiCallType_ID3D12GraphicsCommandList__CopyTiles,
	// Token: 0x04000453 RID: 1107
	ApiCallType_ID3D12GraphicsCommandList__ResolveSubresource,
	// Token: 0x04000454 RID: 1108
	ApiCallType_ID3D12GraphicsCommandList__IASetPrimitiveTopology,
	// Token: 0x04000455 RID: 1109
	ApiCallType_ID3D12GraphicsCommandList__RSSetViewports,
	// Token: 0x04000456 RID: 1110
	ApiCallType_ID3D12GraphicsCommandList__RSSetScissorRects,
	// Token: 0x04000457 RID: 1111
	ApiCallType_ID3D12GraphicsCommandList__OMSetBlendFactor,
	// Token: 0x04000458 RID: 1112
	ApiCallType_ID3D12GraphicsCommandList__OMSetStencilRef,
	// Token: 0x04000459 RID: 1113
	ApiCallType_ID3D12GraphicsCommandList__SetPipelineState,
	// Token: 0x0400045A RID: 1114
	ApiCallType_ID3D12GraphicsCommandList__ResourceBarrier,
	// Token: 0x0400045B RID: 1115
	ApiCallType_ID3D12GraphicsCommandList__ExecuteBundle,
	// Token: 0x0400045C RID: 1116
	ApiCallType_ID3D12GraphicsCommandList__SetDescriptorHeaps,
	// Token: 0x0400045D RID: 1117
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRootSignature,
	// Token: 0x0400045E RID: 1118
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRootSignature,
	// Token: 0x0400045F RID: 1119
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRootDescriptorTable,
	// Token: 0x04000460 RID: 1120
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRootDescriptorTable,
	// Token: 0x04000461 RID: 1121
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRoot32BitConstant,
	// Token: 0x04000462 RID: 1122
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRoot32BitConstant,
	// Token: 0x04000463 RID: 1123
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRoot32BitConstants,
	// Token: 0x04000464 RID: 1124
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRoot32BitConstants,
	// Token: 0x04000465 RID: 1125
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRootConstantBufferView,
	// Token: 0x04000466 RID: 1126
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRootConstantBufferView,
	// Token: 0x04000467 RID: 1127
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRootShaderResourceView,
	// Token: 0x04000468 RID: 1128
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRootShaderResourceView,
	// Token: 0x04000469 RID: 1129
	ApiCallType_ID3D12GraphicsCommandList__SetComputeRootUnorderedAccessView,
	// Token: 0x0400046A RID: 1130
	ApiCallType_ID3D12GraphicsCommandList__SetGraphicsRootUnorderedAccessView,
	// Token: 0x0400046B RID: 1131
	ApiCallType_ID3D12GraphicsCommandList__IASetIndexBuffer,
	// Token: 0x0400046C RID: 1132
	ApiCallType_ID3D12GraphicsCommandList__IASetVertexBuffers,
	// Token: 0x0400046D RID: 1133
	ApiCallType_ID3D12GraphicsCommandList__SOSetTargets,
	// Token: 0x0400046E RID: 1134
	ApiCallType_ID3D12GraphicsCommandList__OMSetRenderTargets,
	// Token: 0x0400046F RID: 1135
	ApiCallType_ID3D12GraphicsCommandList__ClearDepthStencilView,
	// Token: 0x04000470 RID: 1136
	ApiCallType_ID3D12GraphicsCommandList__ClearRenderTargetView,
	// Token: 0x04000471 RID: 1137
	ApiCallType_ID3D12GraphicsCommandList__ClearUnorderedAccessViewUint,
	// Token: 0x04000472 RID: 1138
	ApiCallType_ID3D12GraphicsCommandList__ClearUnorderedAccessViewFloat,
	// Token: 0x04000473 RID: 1139
	ApiCallType_ID3D12GraphicsCommandList__DiscardResource,
	// Token: 0x04000474 RID: 1140
	ApiCallType_ID3D12GraphicsCommandList__BeginQuery,
	// Token: 0x04000475 RID: 1141
	ApiCallType_ID3D12GraphicsCommandList__EndQuery,
	// Token: 0x04000476 RID: 1142
	ApiCallType_ID3D12GraphicsCommandList__ResolveQueryData,
	// Token: 0x04000477 RID: 1143
	ApiCallType_ID3D12GraphicsCommandList__SetPredication,
	// Token: 0x04000478 RID: 1144
	ApiCallType_ID3D12GraphicsCommandList__SetMarker,
	// Token: 0x04000479 RID: 1145
	ApiCallType_ID3D12GraphicsCommandList__BeginEvent,
	// Token: 0x0400047A RID: 1146
	ApiCallType_ID3D12GraphicsCommandList__EndEvent,
	// Token: 0x0400047B RID: 1147
	ApiCallType_ID3D12GraphicsCommandList__ExecuteIndirect,
	// Token: 0x0400047C RID: 1148
	ApiCallType_ID3D12GraphicsCommandList1__AtomicCopyBufferUINT,
	// Token: 0x0400047D RID: 1149
	ApiCallType_ID3D12GraphicsCommandList1__AtomicCopyBufferUINT64,
	// Token: 0x0400047E RID: 1150
	ApiCallType_ID3D12GraphicsCommandList1__OMSetDepthBounds,
	// Token: 0x0400047F RID: 1151
	ApiCallType_ID3D12GraphicsCommandList1__SetSamplePositions,
	// Token: 0x04000480 RID: 1152
	ApiCallType_ID3D12GraphicsCommandList1__ResolveSubresourceRegion,
	// Token: 0x04000481 RID: 1153
	ApiCallType_ID3D12GraphicsCommandList1__SetViewInstanceMask,
	// Token: 0x04000482 RID: 1154
	ApiCallType_ID3D12GraphicsCommandList2__WriteBufferImmediate,
	// Token: 0x04000483 RID: 1155
	ApiCallType_ID3D12CommandQueue__UpdateTileMappings,
	// Token: 0x04000484 RID: 1156
	ApiCallType_ID3D12CommandQueue__CopyTileMappings,
	// Token: 0x04000485 RID: 1157
	ApiCallType_ID3D12CommandQueue__ExecuteCommandLists,
	// Token: 0x04000486 RID: 1158
	ApiCallType_ID3D12CommandQueue__SetMarker,
	// Token: 0x04000487 RID: 1159
	ApiCallType_ID3D12CommandQueue__BeginEvent,
	// Token: 0x04000488 RID: 1160
	ApiCallType_ID3D12CommandQueue__EndEvent,
	// Token: 0x04000489 RID: 1161
	ApiCallType_ID3D12CommandQueue__Signal,
	// Token: 0x0400048A RID: 1162
	ApiCallType_ID3D12CommandQueue__Wait,
	// Token: 0x0400048B RID: 1163
	ApiCallType_ID3D12CommandQueue__GetTimestampFrequency,
	// Token: 0x0400048C RID: 1164
	ApiCallType_ID3D12CommandQueue__GetClockCalibration,
	// Token: 0x0400048D RID: 1165
	ApiCallType_ID3D12CommandQueue__GetDesc,
	// Token: 0x0400048E RID: 1166
	ApiCallType_ID3D12Device__GetNodeCount,
	// Token: 0x0400048F RID: 1167
	ApiCallType_ID3D12Device__CreateCommandQueue,
	// Token: 0x04000490 RID: 1168
	ApiCallType_ID3D12Device__CreateCommandAllocator,
	// Token: 0x04000491 RID: 1169
	ApiCallType_ID3D12Device__CreateGraphicsPipelineState,
	// Token: 0x04000492 RID: 1170
	ApiCallType_ID3D12Device__CreateComputePipelineState,
	// Token: 0x04000493 RID: 1171
	ApiCallType_ID3D12Device__CreateCommandList,
	// Token: 0x04000494 RID: 1172
	ApiCallType_ID3D12Device__CheckFeatureSupport,
	// Token: 0x04000495 RID: 1173
	ApiCallType_ID3D12Device__CreateDescriptorHeap,
	// Token: 0x04000496 RID: 1174
	ApiCallType_ID3D12Device__GetDescriptorHandleIncrementSize,
	// Token: 0x04000497 RID: 1175
	ApiCallType_ID3D12Device__CreateRootSignature,
	// Token: 0x04000498 RID: 1176
	ApiCallType_ID3D12Device__CreateConstantBufferView,
	// Token: 0x04000499 RID: 1177
	ApiCallType_ID3D12Device__CreateShaderResourceView,
	// Token: 0x0400049A RID: 1178
	ApiCallType_ID3D12Device__CreateUnorderedAccessView,
	// Token: 0x0400049B RID: 1179
	ApiCallType_ID3D12Device__CreateRenderTargetView,
	// Token: 0x0400049C RID: 1180
	ApiCallType_ID3D12Device__CreateDepthStencilView,
	// Token: 0x0400049D RID: 1181
	ApiCallType_ID3D12Device__CreateSampler,
	// Token: 0x0400049E RID: 1182
	ApiCallType_ID3D12Device__CopyDescriptors,
	// Token: 0x0400049F RID: 1183
	ApiCallType_ID3D12Device__CopyDescriptorsSimple,
	// Token: 0x040004A0 RID: 1184
	ApiCallType_ID3D12Device__GetResourceAllocationInfo,
	// Token: 0x040004A1 RID: 1185
	ApiCallType_ID3D12Device__GetCustomHeapProperties,
	// Token: 0x040004A2 RID: 1186
	ApiCallType_ID3D12Device__CreateCommittedResource,
	// Token: 0x040004A3 RID: 1187
	ApiCallType_ID3D12Device__CreateHeap,
	// Token: 0x040004A4 RID: 1188
	ApiCallType_ID3D12Device__CreatePlacedResource,
	// Token: 0x040004A5 RID: 1189
	ApiCallType_ID3D12Device__CreateReservedResource,
	// Token: 0x040004A6 RID: 1190
	ApiCallType_ID3D12Device__CreateSharedHandle,
	// Token: 0x040004A7 RID: 1191
	ApiCallType_ID3D12Device__OpenSharedHandle,
	// Token: 0x040004A8 RID: 1192
	ApiCallType_ID3D12Device__OpenSharedHandleByName,
	// Token: 0x040004A9 RID: 1193
	ApiCallType_ID3D12Device__MakeResident,
	// Token: 0x040004AA RID: 1194
	ApiCallType_ID3D12Device__Evict,
	// Token: 0x040004AB RID: 1195
	ApiCallType_ID3D12Device__CreateFence,
	// Token: 0x040004AC RID: 1196
	ApiCallType_ID3D12Device__GetDeviceRemovedReason,
	// Token: 0x040004AD RID: 1197
	ApiCallType_ID3D12Device__GetCopyableFootprints,
	// Token: 0x040004AE RID: 1198
	ApiCallType_ID3D12Device__CreateQueryHeap,
	// Token: 0x040004AF RID: 1199
	ApiCallType_ID3D12Device__SetStablePowerState,
	// Token: 0x040004B0 RID: 1200
	ApiCallType_ID3D12Device__CreateCommandSignature,
	// Token: 0x040004B1 RID: 1201
	ApiCallType_ID3D12Device__GetResourceTiling,
	// Token: 0x040004B2 RID: 1202
	ApiCallType_ID3D12Device__GetAdapterLuid,
	// Token: 0x040004B3 RID: 1203
	ApiCallType_ID3D12PipelineLibrary__StorePipeline,
	// Token: 0x040004B4 RID: 1204
	ApiCallType_ID3D12PipelineLibrary__LoadGraphicsPipeline,
	// Token: 0x040004B5 RID: 1205
	ApiCallType_ID3D12PipelineLibrary__LoadComputePipeline,
	// Token: 0x040004B6 RID: 1206
	ApiCallType_ID3D12PipelineLibrary__GetSerializedSize,
	// Token: 0x040004B7 RID: 1207
	ApiCallType_ID3D12PipelineLibrary__Serialize,
	// Token: 0x040004B8 RID: 1208
	ApiCallType_ID3D12PipelineLibrary1__LoadPipeline,
	// Token: 0x040004B9 RID: 1209
	ApiCallType_ID3D12Device1__CreatePipelineLibrary,
	// Token: 0x040004BA RID: 1210
	ApiCallType_ID3D12Device1__SetEventOnMultipleFenceCompletion,
	// Token: 0x040004BB RID: 1211
	ApiCallType_ID3D12Device1__SetResidencyPriority,
	// Token: 0x040004BC RID: 1212
	ApiCallType_ID3D12Device2__CreatePipelineState,
	// Token: 0x040004BD RID: 1213
	ApiCallType_ID3D12Device3__OpenExistingHeapFromAddress,
	// Token: 0x040004BE RID: 1214
	ApiCallType_ID3D12Device3__OpenExistingHeapFromFileMapping,
	// Token: 0x040004BF RID: 1215
	ApiCallType_ID3D12Device3__EnqueueMakeResident,
	// Token: 0x040004C0 RID: 1216
	ApiCallType_ID3D12ProtectedSession__GetStatusFence,
	// Token: 0x040004C1 RID: 1217
	ApiCallType_ID3D12ProtectedSession__GetSessionStatus,
	// Token: 0x040004C2 RID: 1218
	ApiCallType_ID3D12ProtectedResourceSession__GetDesc,
	// Token: 0x040004C3 RID: 1219
	ApiCallType_ID3D12Device4__CreateCommandList1,
	// Token: 0x040004C4 RID: 1220
	ApiCallType_ID3D12Device4__CreateProtectedResourceSession,
	// Token: 0x040004C5 RID: 1221
	ApiCallType_ID3D12Device4__CreateCommittedResource1,
	// Token: 0x040004C6 RID: 1222
	ApiCallType_ID3D12Device4__CreateHeap1,
	// Token: 0x040004C7 RID: 1223
	ApiCallType_ID3D12Device4__CreateReservedResource1,
	// Token: 0x040004C8 RID: 1224
	ApiCallType_ID3D12Device4__GetResourceAllocationInfo1,
	// Token: 0x040004C9 RID: 1225
	ApiCallType_ID3D12Resource1__GetProtectedResourceSession,
	// Token: 0x040004CA RID: 1226
	ApiCallType_ID3D12Heap1__GetProtectedResourceSession,
	// Token: 0x040004CB RID: 1227
	ApiCallType_ID3D12GraphicsCommandList3__SetProtectedResourceSession,
	// Token: 0x040004CC RID: 1228
	ApiCallType_ID3D12Tools__EnableShaderInstrumentation,
	// Token: 0x040004CD RID: 1229
	ApiCallType_ID3D12Tools__ShaderInstrumentationEnabled,
	// Token: 0x040004CE RID: 1230
	ApiCallType_D3D12Last = 53247
}
