using System;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x02000018 RID: 24
internal class libDCAPPINVOKE
{
	// Token: 0x06000590 RID: 1424
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAP_HAVE_GLES_get")]
	public static extern int DCAP_HAVE_GLES_get();

	// Token: 0x06000591 RID: 1425
	[DllImport("libDCAP", EntryPoint = "CSharp_new_PointerData")]
	public static extern IntPtr new_PointerData();

	// Token: 0x06000592 RID: 1426
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_PointerData")]
	public static extern void delete_PointerData(HandleRef jarg1);

	// Token: 0x06000593 RID: 1427
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_IsInitialized")]
	public static extern bool PointerData_IsInitialized(HandleRef jarg1);

	// Token: 0x06000594 RID: 1428
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_IsNull")]
	public static extern bool PointerData_IsNull(HandleRef jarg1);

	// Token: 0x06000595 RID: 1429
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveValue")]
	public static extern bool PointerData_HaveValue(HandleRef jarg1);

	// Token: 0x06000596 RID: 1430
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveArray")]
	public static extern bool PointerData_HaveArray(HandleRef jarg1);

	// Token: 0x06000597 RID: 1431
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveString")]
	public static extern bool PointerData_HaveString(HandleRef jarg1);

	// Token: 0x06000598 RID: 1432
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveStringArray")]
	public static extern bool PointerData_HaveStringArray(HandleRef jarg1);

	// Token: 0x06000599 RID: 1433
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveAddress")]
	public static extern bool PointerData_HaveAddress(HandleRef jarg1);

	// Token: 0x0600059A RID: 1434
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveData")]
	public static extern bool PointerData_HaveData(HandleRef jarg1);

	// Token: 0x0600059B RID: 1435
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_HaveDataStripped")]
	public static extern bool PointerData_HaveDataStripped(HandleRef jarg1);

	// Token: 0x0600059C RID: 1436
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetMask")]
	public static extern uint PointerData_GetMask(HandleRef jarg1);

	// Token: 0x0600059D RID: 1437
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetAddress")]
	public static extern ulong PointerData_GetAddress(HandleRef jarg1);

	// Token: 0x0600059E RID: 1438
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetElementSize")]
	public static extern uint PointerData_GetElementSize(HandleRef jarg1);

	// Token: 0x0600059F RID: 1439
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetCount")]
	public static extern uint PointerData_GetCount(HandleRef jarg1);

	// Token: 0x060005A0 RID: 1440
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetByteSize")]
	public static extern uint PointerData_GetByteSize(HandleRef jarg1);

	// Token: 0x060005A1 RID: 1441
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetValueData")]
	public static extern IntPtr PointerData_GetValueData(HandleRef jarg1);

	// Token: 0x060005A2 RID: 1442
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetArrayData")]
	public static extern IntPtr PointerData_GetArrayData(HandleRef jarg1);

	// Token: 0x060005A3 RID: 1443
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetStringData")]
	public static extern IntPtr PointerData_GetStringData(HandleRef jarg1);

	// Token: 0x060005A4 RID: 1444
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetStringMasks")]
	public static extern IntPtr PointerData_GetStringMasks(HandleRef jarg1);

	// Token: 0x060005A5 RID: 1445
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetStringAddresses")]
	public static extern IntPtr PointerData_GetStringAddresses(HandleRef jarg1);

	// Token: 0x060005A6 RID: 1446
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetStringLengths")]
	public static extern IntPtr PointerData_GetStringLengths(HandleRef jarg1);

	// Token: 0x060005A7 RID: 1447
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetStringArrayData")]
	public static extern IntPtr PointerData_GetStringArrayData(HandleRef jarg1);

	// Token: 0x060005A8 RID: 1448
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_GetPointer")]
	public static extern IntPtr PointerData_GetPointer(HandleRef jarg1);

	// Token: 0x060005A9 RID: 1449
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_Decode")]
	public static extern uint PointerData_Decode(HandleRef jarg1, IntPtr jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060005AA RID: 1450
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_ResizeArray")]
	public static extern int PointerData_ResizeArray(HandleRef jarg1, uint jarg2);

	// Token: 0x060005AB RID: 1451
	[DllImport("libDCAP", EntryPoint = "CSharp_PointerData_ResizeString")]
	public static extern int PointerData_ResizeString(HandleRef jarg1, uint jarg2);

	// Token: 0x060005AC RID: 1452
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_Adapter")]
	public static extern void delete_Adapter(HandleRef jarg1);

	// Token: 0x060005AD RID: 1453
	[DllImport("libDCAP", EntryPoint = "CSharp_Adapter_SetCurrentThread")]
	public static extern void Adapter_SetCurrentThread(HandleRef jarg1, uint jarg2);

	// Token: 0x060005AE RID: 1454
	[DllImport("libDCAP", EntryPoint = "CSharp_new_EGLAdapter")]
	public static extern IntPtr new_EGLAdapter();

	// Token: 0x060005AF RID: 1455
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_EGLAdapter")]
	public static extern void delete_EGLAdapter(HandleRef jarg1);

	// Token: 0x060005B0 RID: 1456
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_SetCurrentThread")]
	public static extern void EGLAdapter_SetCurrentThread(HandleRef jarg1, uint jarg2);

	// Token: 0x060005B1 RID: 1457
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetError")]
	public static extern void EGLAdapter_Process_eglGetError(HandleRef jarg1, int jarg2);

	// Token: 0x060005B2 RID: 1458
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetDisplay")]
	public static extern void EGLAdapter_Process_eglGetDisplay(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060005B3 RID: 1459
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglInitialize")]
	public static extern void EGLAdapter_Process_eglInitialize(HandleRef jarg1, int jarg2, uint jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x060005B4 RID: 1460
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglTerminate")]
	public static extern void EGLAdapter_Process_eglTerminate(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060005B5 RID: 1461
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryString")]
	public static extern void EGLAdapter_Process_eglQueryString(HandleRef jarg1, HandleRef jarg2, uint jarg3, int jarg4);

	// Token: 0x060005B6 RID: 1462
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetConfigs")]
	public static extern void EGLAdapter_Process_eglGetConfigs(HandleRef jarg1, int jarg2, uint jarg3, HandleRef jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005B7 RID: 1463
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglChooseConfig")]
	public static extern void EGLAdapter_Process_eglChooseConfig(HandleRef jarg1, int jarg2, uint jarg3, HandleRef jarg4, HandleRef jarg5, int jarg6, HandleRef jarg7);

	// Token: 0x060005B8 RID: 1464
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetConfigAttrib")]
	public static extern void EGLAdapter_Process_eglGetConfigAttrib(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005B9 RID: 1465
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateWindowSurface")]
	public static extern void EGLAdapter_Process_eglCreateWindowSurface(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060005BA RID: 1466
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreatePbufferSurface")]
	public static extern void EGLAdapter_Process_eglCreatePbufferSurface(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005BB RID: 1467
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreatePixmapSurface")]
	public static extern void EGLAdapter_Process_eglCreatePixmapSurface(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060005BC RID: 1468
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglDestroySurface")]
	public static extern void EGLAdapter_Process_eglDestroySurface(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005BD RID: 1469
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQuerySurface")]
	public static extern void EGLAdapter_Process_eglQuerySurface(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005BE RID: 1470
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglBindAPI")]
	public static extern void EGLAdapter_Process_eglBindAPI(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060005BF RID: 1471
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryAPI")]
	public static extern void EGLAdapter_Process_eglQueryAPI(HandleRef jarg1, uint jarg2);

	// Token: 0x060005C0 RID: 1472
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglWaitClient")]
	public static extern void EGLAdapter_Process_eglWaitClient(HandleRef jarg1, int jarg2);

	// Token: 0x060005C1 RID: 1473
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglReleaseThread")]
	public static extern void EGLAdapter_Process_eglReleaseThread(HandleRef jarg1, int jarg2);

	// Token: 0x060005C2 RID: 1474
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreatePbufferFromClientBuffer")]
	public static extern void EGLAdapter_Process_eglCreatePbufferFromClientBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, HandleRef jarg7);

	// Token: 0x060005C3 RID: 1475
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSurfaceAttrib")]
	public static extern void EGLAdapter_Process_eglSurfaceAttrib(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060005C4 RID: 1476
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglBindTexImage")]
	public static extern void EGLAdapter_Process_eglBindTexImage(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5);

	// Token: 0x060005C5 RID: 1477
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglReleaseTexImage")]
	public static extern void EGLAdapter_Process_eglReleaseTexImage(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5);

	// Token: 0x060005C6 RID: 1478
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSwapInterval")]
	public static extern void EGLAdapter_Process_eglSwapInterval(HandleRef jarg1, int jarg2, uint jarg3, int jarg4);

	// Token: 0x060005C7 RID: 1479
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateContext")]
	public static extern void EGLAdapter_Process_eglCreateContext(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060005C8 RID: 1480
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglDestroyContext")]
	public static extern void EGLAdapter_Process_eglDestroyContext(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005C9 RID: 1481
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglMakeCurrent")]
	public static extern void EGLAdapter_Process_eglMakeCurrent(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x060005CA RID: 1482
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetCurrentContext")]
	public static extern void EGLAdapter_Process_eglGetCurrentContext(HandleRef jarg1, uint jarg2);

	// Token: 0x060005CB RID: 1483
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetCurrentSurface")]
	public static extern void EGLAdapter_Process_eglGetCurrentSurface(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x060005CC RID: 1484
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetCurrentDisplay")]
	public static extern void EGLAdapter_Process_eglGetCurrentDisplay(HandleRef jarg1, uint jarg2);

	// Token: 0x060005CD RID: 1485
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryContext")]
	public static extern void EGLAdapter_Process_eglQueryContext(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005CE RID: 1486
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglWaitGL")]
	public static extern void EGLAdapter_Process_eglWaitGL(HandleRef jarg1, int jarg2);

	// Token: 0x060005CF RID: 1487
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglWaitNative")]
	public static extern void EGLAdapter_Process_eglWaitNative(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x060005D0 RID: 1488
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSwapBuffers")]
	public static extern void EGLAdapter_Process_eglSwapBuffers(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005D1 RID: 1489
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCopyBuffers")]
	public static extern void EGLAdapter_Process_eglCopyBuffers(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060005D2 RID: 1490
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetProcAddress")]
	public static extern void EGLAdapter_Process_eglGetProcAddress(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

	// Token: 0x060005D3 RID: 1491
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateImageKHR")]
	public static extern void EGLAdapter_Process_eglCreateImageKHR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, HandleRef jarg7);

	// Token: 0x060005D4 RID: 1492
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglDestroyImageKHR")]
	public static extern void EGLAdapter_Process_eglDestroyImageKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005D5 RID: 1493
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglLockImageQCOM")]
	public static extern void EGLAdapter_Process_eglLockImageQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005D6 RID: 1494
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglUnlockImageQCOM")]
	public static extern void EGLAdapter_Process_eglUnlockImageQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005D7 RID: 1495
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryImageQCOM")]
	public static extern void EGLAdapter_Process_eglQueryImageQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005D8 RID: 1496
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryImage64QCOM")]
	public static extern void EGLAdapter_Process_eglQueryImage64QCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005D9 RID: 1497
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSetBlobCacheFuncsANDROID")]
	public static extern void EGLAdapter_Process_eglSetBlobCacheFuncsANDROID(HandleRef jarg1, uint jarg2, HandleRef jarg3, HandleRef jarg4);

	// Token: 0x060005DA RID: 1498
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateSync")]
	public static extern void EGLAdapter_Process_eglCreateSync(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005DB RID: 1499
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateSyncKHR")]
	public static extern void EGLAdapter_Process_eglCreateSyncKHR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005DC RID: 1500
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateSync64KHR")]
	public static extern void EGLAdapter_Process_eglCreateSync64KHR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005DD RID: 1501
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglDestroySyncKHR")]
	public static extern void EGLAdapter_Process_eglDestroySyncKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005DE RID: 1502
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglClientWaitSyncKHR")]
	public static extern void EGLAdapter_Process_eglClientWaitSyncKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, ulong jarg6);

	// Token: 0x060005DF RID: 1503
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglWaitSyncKHR")]
	public static extern void EGLAdapter_Process_eglWaitSyncKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5);

	// Token: 0x060005E0 RID: 1504
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSignalSyncKHR")]
	public static extern void EGLAdapter_Process_eglSignalSyncKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060005E1 RID: 1505
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetSyncAttrib")]
	public static extern void EGLAdapter_Process_eglGetSyncAttrib(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005E2 RID: 1506
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglDupNativeFenceFDANDROID")]
	public static extern void EGLAdapter_Process_eglDupNativeFenceFDANDROID(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005E3 RID: 1507
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetSyncObjFromEglSyncQCOM")]
	public static extern void EGLAdapter_Process_eglGetSyncObjFromEglSyncQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060005E4 RID: 1508
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglLockSurfaceKHR")]
	public static extern void EGLAdapter_Process_eglLockSurfaceKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005E5 RID: 1509
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglUnlockSurfaceKHR")]
	public static extern void EGLAdapter_Process_eglUnlockSurfaceKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060005E6 RID: 1510
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQuerySurface64KHR")]
	public static extern void EGLAdapter_Process_eglQuerySurface64KHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005E7 RID: 1511
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGpuPerfHintQCOM")]
	public static extern void EGLAdapter_Process_eglGpuPerfHintQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005E8 RID: 1512
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglSetDamageRegionKHR")]
	public static extern void EGLAdapter_Process_eglSetDamageRegionKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5, int jarg6);

	// Token: 0x060005E9 RID: 1513
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetPlatformDisplay")]
	public static extern void EGLAdapter_Process_eglGetPlatformDisplay(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060005EA RID: 1514
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreatePlatformWindowSurface")]
	public static extern void EGLAdapter_Process_eglCreatePlatformWindowSurface(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060005EB RID: 1515
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreatePlatformPixmapSurface")]
	public static extern void EGLAdapter_Process_eglCreatePlatformPixmapSurface(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060005EC RID: 1516
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglGetSyncAttribKHR")]
	public static extern void EGLAdapter_Process_eglGetSyncAttribKHR(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060005ED RID: 1517
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryDmaBufFormatsEXT")]
	public static extern void EGLAdapter_Process_eglQueryDmaBufFormatsEXT(HandleRef jarg1, int jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x060005EE RID: 1518
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglQueryDmaBufModifiersEXT")]
	public static extern void EGLAdapter_Process_eglQueryDmaBufModifiersEXT(HandleRef jarg1, int jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8);

	// Token: 0x060005EF RID: 1519
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglExportDMABUFImageQueryMESA")]
	public static extern void EGLAdapter_Process_eglExportDMABUFImageQueryMESA(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7);

	// Token: 0x060005F0 RID: 1520
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglExportDMABUFImageMESA")]
	public static extern void EGLAdapter_Process_eglExportDMABUFImageMESA(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7);

	// Token: 0x060005F1 RID: 1521
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_Process_eglCreateImageKHRv1")]
	public static extern void EGLAdapter_Process_eglCreateImageKHRv1(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, HandleRef jarg7);

	// Token: 0x060005F2 RID: 1522
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_director_connect")]
	public static extern void EGLAdapter_director_connect(HandleRef jarg1, EGLAdapter.SwigDelegateEGLAdapter_0 delegate0, EGLAdapter.SwigDelegateEGLAdapter_1 delegate1, EGLAdapter.SwigDelegateEGLAdapter_2 delegate2, EGLAdapter.SwigDelegateEGLAdapter_3 delegate3, EGLAdapter.SwigDelegateEGLAdapter_4 delegate4, EGLAdapter.SwigDelegateEGLAdapter_5 delegate5, EGLAdapter.SwigDelegateEGLAdapter_6 delegate6, EGLAdapter.SwigDelegateEGLAdapter_7 delegate7, EGLAdapter.SwigDelegateEGLAdapter_8 delegate8, EGLAdapter.SwigDelegateEGLAdapter_9 delegate9, EGLAdapter.SwigDelegateEGLAdapter_10 delegate10, EGLAdapter.SwigDelegateEGLAdapter_11 delegate11, EGLAdapter.SwigDelegateEGLAdapter_12 delegate12, EGLAdapter.SwigDelegateEGLAdapter_13 delegate13, EGLAdapter.SwigDelegateEGLAdapter_14 delegate14, EGLAdapter.SwigDelegateEGLAdapter_15 delegate15, EGLAdapter.SwigDelegateEGLAdapter_16 delegate16, EGLAdapter.SwigDelegateEGLAdapter_17 delegate17, EGLAdapter.SwigDelegateEGLAdapter_18 delegate18, EGLAdapter.SwigDelegateEGLAdapter_19 delegate19, EGLAdapter.SwigDelegateEGLAdapter_20 delegate20, EGLAdapter.SwigDelegateEGLAdapter_21 delegate21, EGLAdapter.SwigDelegateEGLAdapter_22 delegate22, EGLAdapter.SwigDelegateEGLAdapter_23 delegate23, EGLAdapter.SwigDelegateEGLAdapter_24 delegate24, EGLAdapter.SwigDelegateEGLAdapter_25 delegate25, EGLAdapter.SwigDelegateEGLAdapter_26 delegate26, EGLAdapter.SwigDelegateEGLAdapter_27 delegate27, EGLAdapter.SwigDelegateEGLAdapter_28 delegate28, EGLAdapter.SwigDelegateEGLAdapter_29 delegate29, EGLAdapter.SwigDelegateEGLAdapter_30 delegate30, EGLAdapter.SwigDelegateEGLAdapter_31 delegate31, EGLAdapter.SwigDelegateEGLAdapter_32 delegate32, EGLAdapter.SwigDelegateEGLAdapter_33 delegate33, EGLAdapter.SwigDelegateEGLAdapter_34 delegate34, EGLAdapter.SwigDelegateEGLAdapter_35 delegate35, EGLAdapter.SwigDelegateEGLAdapter_36 delegate36, EGLAdapter.SwigDelegateEGLAdapter_37 delegate37, EGLAdapter.SwigDelegateEGLAdapter_38 delegate38, EGLAdapter.SwigDelegateEGLAdapter_39 delegate39, EGLAdapter.SwigDelegateEGLAdapter_40 delegate40, EGLAdapter.SwigDelegateEGLAdapter_41 delegate41, EGLAdapter.SwigDelegateEGLAdapter_42 delegate42, EGLAdapter.SwigDelegateEGLAdapter_43 delegate43, EGLAdapter.SwigDelegateEGLAdapter_44 delegate44, EGLAdapter.SwigDelegateEGLAdapter_45 delegate45, EGLAdapter.SwigDelegateEGLAdapter_46 delegate46, EGLAdapter.SwigDelegateEGLAdapter_47 delegate47, EGLAdapter.SwigDelegateEGLAdapter_48 delegate48, EGLAdapter.SwigDelegateEGLAdapter_49 delegate49, EGLAdapter.SwigDelegateEGLAdapter_50 delegate50, EGLAdapter.SwigDelegateEGLAdapter_51 delegate51, EGLAdapter.SwigDelegateEGLAdapter_52 delegate52, EGLAdapter.SwigDelegateEGLAdapter_53 delegate53, EGLAdapter.SwigDelegateEGLAdapter_54 delegate54, EGLAdapter.SwigDelegateEGLAdapter_55 delegate55, EGLAdapter.SwigDelegateEGLAdapter_56 delegate56, EGLAdapter.SwigDelegateEGLAdapter_57 delegate57, EGLAdapter.SwigDelegateEGLAdapter_58 delegate58, EGLAdapter.SwigDelegateEGLAdapter_59 delegate59, EGLAdapter.SwigDelegateEGLAdapter_60 delegate60, EGLAdapter.SwigDelegateEGLAdapter_61 delegate61, EGLAdapter.SwigDelegateEGLAdapter_62 delegate62, EGLAdapter.SwigDelegateEGLAdapter_63 delegate63, EGLAdapter.SwigDelegateEGLAdapter_64 delegate64, EGLAdapter.SwigDelegateEGLAdapter_65 delegate65);

	// Token: 0x060005F3 RID: 1523
	[DllImport("libDCAP", EntryPoint = "CSharp_new_GLAdapter")]
	public static extern IntPtr new_GLAdapter();

	// Token: 0x060005F4 RID: 1524
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_GLAdapter")]
	public static extern void delete_GLAdapter(HandleRef jarg1);

	// Token: 0x060005F5 RID: 1525
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_SetCurrentThread")]
	public static extern void GLAdapter_SetCurrentThread(HandleRef jarg1, uint jarg2);

	// Token: 0x060005F6 RID: 1526
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_ProcessVertexAttribData")]
	public static extern void GLAdapter_ProcessVertexAttribData(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, uint jarg5, int jarg6, IntPtr jarg7, uint jarg8, uint jarg9);

	// Token: 0x060005F7 RID: 1527
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_ProcessVertexAttribIData")]
	public static extern void GLAdapter_ProcessVertexAttribIData(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, IntPtr jarg6, uint jarg7, uint jarg8);

	// Token: 0x060005F8 RID: 1528
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_ProcessFlushMappedBufferRange")]
	public static extern void GLAdapter_ProcessFlushMappedBufferRange(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, IntPtr jarg5);

	// Token: 0x060005F9 RID: 1529
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_ProcessUnmapBuffer")]
	public static extern void GLAdapter_ProcessUnmapBuffer(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x060005FA RID: 1530
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glActiveTexture")]
	public static extern void GLAdapter_Process_glActiveTexture(HandleRef jarg1, uint jarg2);

	// Token: 0x060005FB RID: 1531
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glAttachShader")]
	public static extern void GLAdapter_Process_glAttachShader(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060005FC RID: 1532
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindAttribLocation")]
	public static extern void GLAdapter_Process_glBindAttribLocation(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060005FD RID: 1533
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindBuffer")]
	public static extern void GLAdapter_Process_glBindBuffer(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060005FE RID: 1534
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindFramebuffer")]
	public static extern void GLAdapter_Process_glBindFramebuffer(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060005FF RID: 1535
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindRenderbuffer")]
	public static extern void GLAdapter_Process_glBindRenderbuffer(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000600 RID: 1536
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindTexture")]
	public static extern void GLAdapter_Process_glBindTexture(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000601 RID: 1537
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendColor")]
	public static extern void GLAdapter_Process_glBlendColor(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x06000602 RID: 1538
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquation")]
	public static extern void GLAdapter_Process_glBlendEquation(HandleRef jarg1, uint jarg2);

	// Token: 0x06000603 RID: 1539
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquationSeparate")]
	public static extern void GLAdapter_Process_glBlendEquationSeparate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000604 RID: 1540
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFunc")]
	public static extern void GLAdapter_Process_glBlendFunc(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000605 RID: 1541
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFuncSeparate")]
	public static extern void GLAdapter_Process_glBlendFuncSeparate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000606 RID: 1542
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBufferData")]
	public static extern void GLAdapter_Process_glBufferData(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, uint jarg5);

	// Token: 0x06000607 RID: 1543
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBufferSubData")]
	public static extern void GLAdapter_Process_glBufferSubData(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000608 RID: 1544
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCheckFramebufferStatus")]
	public static extern void GLAdapter_Process_glCheckFramebufferStatus(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000609 RID: 1545
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClear")]
	public static extern void GLAdapter_Process_glClear(HandleRef jarg1, uint jarg2);

	// Token: 0x0600060A RID: 1546
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearColor")]
	public static extern void GLAdapter_Process_glClearColor(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x0600060B RID: 1547
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearDepthf")]
	public static extern void GLAdapter_Process_glClearDepthf(HandleRef jarg1, float jarg2);

	// Token: 0x0600060C RID: 1548
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearStencil")]
	public static extern void GLAdapter_Process_glClearStencil(HandleRef jarg1, int jarg2);

	// Token: 0x0600060D RID: 1549
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glColorMask")]
	public static extern void GLAdapter_Process_glColorMask(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

	// Token: 0x0600060E RID: 1550
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompileShader")]
	public static extern void GLAdapter_Process_glCompileShader(HandleRef jarg1, uint jarg2);

	// Token: 0x0600060F RID: 1551
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexImage2D")]
	public static extern void GLAdapter_Process_glCompressedTexImage2D(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, HandleRef jarg9);

	// Token: 0x06000610 RID: 1552
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexSubImage2D")]
	public static extern void GLAdapter_Process_glCompressedTexSubImage2D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, int jarg9, HandleRef jarg10);

	// Token: 0x06000611 RID: 1553
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyTexImage2D")]
	public static extern void GLAdapter_Process_glCopyTexImage2D(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9);

	// Token: 0x06000612 RID: 1554
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyTexSubImage2D")]
	public static extern void GLAdapter_Process_glCopyTexSubImage2D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9);

	// Token: 0x06000613 RID: 1555
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCreateProgram")]
	public static extern void GLAdapter_Process_glCreateProgram(HandleRef jarg1, uint jarg2);

	// Token: 0x06000614 RID: 1556
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCreateShader")]
	public static extern void GLAdapter_Process_glCreateShader(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000615 RID: 1557
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCullFace")]
	public static extern void GLAdapter_Process_glCullFace(HandleRef jarg1, uint jarg2);

	// Token: 0x06000616 RID: 1558
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteBuffers")]
	public static extern void GLAdapter_Process_glDeleteBuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000617 RID: 1559
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteFramebuffers")]
	public static extern void GLAdapter_Process_glDeleteFramebuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000618 RID: 1560
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteProgram")]
	public static extern void GLAdapter_Process_glDeleteProgram(HandleRef jarg1, uint jarg2);

	// Token: 0x06000619 RID: 1561
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteRenderbuffers")]
	public static extern void GLAdapter_Process_glDeleteRenderbuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600061A RID: 1562
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteShader")]
	public static extern void GLAdapter_Process_glDeleteShader(HandleRef jarg1, uint jarg2);

	// Token: 0x0600061B RID: 1563
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteTextures")]
	public static extern void GLAdapter_Process_glDeleteTextures(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600061C RID: 1564
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDepthFunc")]
	public static extern void GLAdapter_Process_glDepthFunc(HandleRef jarg1, uint jarg2);

	// Token: 0x0600061D RID: 1565
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDepthMask")]
	public static extern void GLAdapter_Process_glDepthMask(HandleRef jarg1, int jarg2);

	// Token: 0x0600061E RID: 1566
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDepthRangef")]
	public static extern void GLAdapter_Process_glDepthRangef(HandleRef jarg1, float jarg2, float jarg3);

	// Token: 0x0600061F RID: 1567
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDetachShader")]
	public static extern void GLAdapter_Process_glDetachShader(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000620 RID: 1568
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDisable")]
	public static extern void GLAdapter_Process_glDisable(HandleRef jarg1, uint jarg2);

	// Token: 0x06000621 RID: 1569
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDisableVertexAttribArray")]
	public static extern void GLAdapter_Process_glDisableVertexAttribArray(HandleRef jarg1, uint jarg2);

	// Token: 0x06000622 RID: 1570
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawArrays")]
	public static extern void GLAdapter_Process_glDrawArrays(HandleRef jarg1, uint jarg2, int jarg3, int jarg4);

	// Token: 0x06000623 RID: 1571
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawElements")]
	public static extern void GLAdapter_Process_glDrawElements(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000624 RID: 1572
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEnable")]
	public static extern void GLAdapter_Process_glEnable(HandleRef jarg1, uint jarg2);

	// Token: 0x06000625 RID: 1573
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEnableVertexAttribArray")]
	public static extern void GLAdapter_Process_glEnableVertexAttribArray(HandleRef jarg1, uint jarg2);

	// Token: 0x06000626 RID: 1574
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFinish")]
	public static extern void GLAdapter_Process_glFinish(HandleRef jarg1);

	// Token: 0x06000627 RID: 1575
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFlush")]
	public static extern void GLAdapter_Process_glFlush(HandleRef jarg1);

	// Token: 0x06000628 RID: 1576
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferRenderbuffer")]
	public static extern void GLAdapter_Process_glFramebufferRenderbuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000629 RID: 1577
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTexture2D")]
	public static extern void GLAdapter_Process_glFramebufferTexture2D(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6);

	// Token: 0x0600062A RID: 1578
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFrontFace")]
	public static extern void GLAdapter_Process_glFrontFace(HandleRef jarg1, uint jarg2);

	// Token: 0x0600062B RID: 1579
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenBuffers")]
	public static extern void GLAdapter_Process_glGenBuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600062C RID: 1580
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenerateMipmap")]
	public static extern void GLAdapter_Process_glGenerateMipmap(HandleRef jarg1, uint jarg2);

	// Token: 0x0600062D RID: 1581
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenFramebuffers")]
	public static extern void GLAdapter_Process_glGenFramebuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600062E RID: 1582
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenRenderbuffers")]
	public static extern void GLAdapter_Process_glGenRenderbuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600062F RID: 1583
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenTextures")]
	public static extern void GLAdapter_Process_glGenTextures(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000630 RID: 1584
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveAttrib")]
	public static extern void GLAdapter_Process_glGetActiveAttrib(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8);

	// Token: 0x06000631 RID: 1585
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveUniform")]
	public static extern void GLAdapter_Process_glGetActiveUniform(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8);

	// Token: 0x06000632 RID: 1586
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetAttachedShaders")]
	public static extern void GLAdapter_Process_glGetAttachedShaders(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000633 RID: 1587
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetAttribLocation")]
	public static extern void GLAdapter_Process_glGetAttribLocation(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000634 RID: 1588
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetBooleanv")]
	public static extern void GLAdapter_Process_glGetBooleanv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000635 RID: 1589
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetBufferParameteriv")]
	public static extern void GLAdapter_Process_glGetBufferParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000636 RID: 1590
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetError")]
	public static extern void GLAdapter_Process_glGetError(HandleRef jarg1, uint jarg2);

	// Token: 0x06000637 RID: 1591
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFloatv")]
	public static extern void GLAdapter_Process_glGetFloatv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000638 RID: 1592
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFramebufferAttachmentParameteriv")]
	public static extern void GLAdapter_Process_glGetFramebufferAttachmentParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000639 RID: 1593
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetIntegerv")]
	public static extern void GLAdapter_Process_glGetIntegerv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x0600063A RID: 1594
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramiv")]
	public static extern void GLAdapter_Process_glGetProgramiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600063B RID: 1595
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramInfoLog")]
	public static extern void GLAdapter_Process_glGetProgramInfoLog(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x0600063C RID: 1596
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetRenderbufferParameteriv")]
	public static extern void GLAdapter_Process_glGetRenderbufferParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600063D RID: 1597
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetShaderiv")]
	public static extern void GLAdapter_Process_glGetShaderiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600063E RID: 1598
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetShaderInfoLog")]
	public static extern void GLAdapter_Process_glGetShaderInfoLog(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x0600063F RID: 1599
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetShaderPrecisionFormat")]
	public static extern void GLAdapter_Process_glGetShaderPrecisionFormat(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000640 RID: 1600
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetShaderSource")]
	public static extern void GLAdapter_Process_glGetShaderSource(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000641 RID: 1601
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetString")]
	public static extern void GLAdapter_Process_glGetString(HandleRef jarg1, HandleRef jarg2, uint jarg3);

	// Token: 0x06000642 RID: 1602
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameterfv")]
	public static extern void GLAdapter_Process_glGetTexParameterfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000643 RID: 1603
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameteriv")]
	public static extern void GLAdapter_Process_glGetTexParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000644 RID: 1604
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformfv")]
	public static extern void GLAdapter_Process_glGetUniformfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000645 RID: 1605
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformiv")]
	public static extern void GLAdapter_Process_glGetUniformiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000646 RID: 1606
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformLocation")]
	public static extern void GLAdapter_Process_glGetUniformLocation(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000647 RID: 1607
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetVertexAttribfv")]
	public static extern void GLAdapter_Process_glGetVertexAttribfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000648 RID: 1608
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetVertexAttribiv")]
	public static extern void GLAdapter_Process_glGetVertexAttribiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000649 RID: 1609
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetVertexAttribPointerv")]
	public static extern void GLAdapter_Process_glGetVertexAttribPointerv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600064A RID: 1610
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glHint")]
	public static extern void GLAdapter_Process_glHint(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x0600064B RID: 1611
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsBuffer")]
	public static extern void GLAdapter_Process_glIsBuffer(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x0600064C RID: 1612
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsEnabled")]
	public static extern void GLAdapter_Process_glIsEnabled(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x0600064D RID: 1613
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsFramebuffer")]
	public static extern void GLAdapter_Process_glIsFramebuffer(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x0600064E RID: 1614
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsProgram")]
	public static extern void GLAdapter_Process_glIsProgram(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x0600064F RID: 1615
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsRenderbuffer")]
	public static extern void GLAdapter_Process_glIsRenderbuffer(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000650 RID: 1616
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsShader")]
	public static extern void GLAdapter_Process_glIsShader(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000651 RID: 1617
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsTexture")]
	public static extern void GLAdapter_Process_glIsTexture(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000652 RID: 1618
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glLineWidth")]
	public static extern void GLAdapter_Process_glLineWidth(HandleRef jarg1, float jarg2);

	// Token: 0x06000653 RID: 1619
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glLinkProgram")]
	public static extern void GLAdapter_Process_glLinkProgram(HandleRef jarg1, uint jarg2);

	// Token: 0x06000654 RID: 1620
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPixelStorei")]
	public static extern void GLAdapter_Process_glPixelStorei(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x06000655 RID: 1621
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPolygonOffset")]
	public static extern void GLAdapter_Process_glPolygonOffset(HandleRef jarg1, float jarg2, float jarg3);

	// Token: 0x06000656 RID: 1622
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glReadPixels")]
	public static extern void GLAdapter_Process_glReadPixels(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06000657 RID: 1623
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glReleaseShaderCompiler")]
	public static extern void GLAdapter_Process_glReleaseShaderCompiler(HandleRef jarg1);

	// Token: 0x06000658 RID: 1624
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glRenderbufferStorage")]
	public static extern void GLAdapter_Process_glRenderbufferStorage(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5);

	// Token: 0x06000659 RID: 1625
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSampleCoverage")]
	public static extern void GLAdapter_Process_glSampleCoverage(HandleRef jarg1, float jarg2, int jarg3);

	// Token: 0x0600065A RID: 1626
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glScissor")]
	public static extern void GLAdapter_Process_glScissor(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

	// Token: 0x0600065B RID: 1627
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glShaderBinary")]
	public static extern void GLAdapter_Process_glShaderBinary(HandleRef jarg1, int jarg2, HandleRef jarg3, uint jarg4, HandleRef jarg5, int jarg6);

	// Token: 0x0600065C RID: 1628
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glShaderSource")]
	public static extern void GLAdapter_Process_glShaderSource(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x0600065D RID: 1629
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilFunc")]
	public static extern void GLAdapter_Process_glStencilFunc(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4);

	// Token: 0x0600065E RID: 1630
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilFuncSeparate")]
	public static extern void GLAdapter_Process_glStencilFuncSeparate(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, uint jarg5);

	// Token: 0x0600065F RID: 1631
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilMask")]
	public static extern void GLAdapter_Process_glStencilMask(HandleRef jarg1, uint jarg2);

	// Token: 0x06000660 RID: 1632
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilMaskSeparate")]
	public static extern void GLAdapter_Process_glStencilMaskSeparate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000661 RID: 1633
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilOp")]
	public static extern void GLAdapter_Process_glStencilOp(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000662 RID: 1634
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStencilOpSeparate")]
	public static extern void GLAdapter_Process_glStencilOpSeparate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000663 RID: 1635
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexImage2D")]
	public static extern void GLAdapter_Process_glTexImage2D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, uint jarg9, HandleRef jarg10);

	// Token: 0x06000664 RID: 1636
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterf")]
	public static extern void GLAdapter_Process_glTexParameterf(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4);

	// Token: 0x06000665 RID: 1637
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterfv")]
	public static extern void GLAdapter_Process_glTexParameterfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000666 RID: 1638
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameteri")]
	public static extern void GLAdapter_Process_glTexParameteri(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x06000667 RID: 1639
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameteriv")]
	public static extern void GLAdapter_Process_glTexParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000668 RID: 1640
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexSubImage2D")]
	public static extern void GLAdapter_Process_glTexSubImage2D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, uint jarg9, HandleRef jarg10);

	// Token: 0x06000669 RID: 1641
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1f")]
	public static extern void GLAdapter_Process_glUniform1f(HandleRef jarg1, uint jarg2, float jarg3);

	// Token: 0x0600066A RID: 1642
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1fv")]
	public static extern void GLAdapter_Process_glUniform1fv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x0600066B RID: 1643
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1i")]
	public static extern void GLAdapter_Process_glUniform1i(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x0600066C RID: 1644
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1iv")]
	public static extern void GLAdapter_Process_glUniform1iv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x0600066D RID: 1645
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2f")]
	public static extern void GLAdapter_Process_glUniform2f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4);

	// Token: 0x0600066E RID: 1646
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2fv")]
	public static extern void GLAdapter_Process_glUniform2fv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x0600066F RID: 1647
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2i")]
	public static extern void GLAdapter_Process_glUniform2i(HandleRef jarg1, uint jarg2, int jarg3, int jarg4);

	// Token: 0x06000670 RID: 1648
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2iv")]
	public static extern void GLAdapter_Process_glUniform2iv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000671 RID: 1649
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3f")]
	public static extern void GLAdapter_Process_glUniform3f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x06000672 RID: 1650
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3fv")]
	public static extern void GLAdapter_Process_glUniform3fv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000673 RID: 1651
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3i")]
	public static extern void GLAdapter_Process_glUniform3i(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5);

	// Token: 0x06000674 RID: 1652
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3iv")]
	public static extern void GLAdapter_Process_glUniform3iv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000675 RID: 1653
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4f")]
	public static extern void GLAdapter_Process_glUniform4f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4, float jarg5, float jarg6);

	// Token: 0x06000676 RID: 1654
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4fv")]
	public static extern void GLAdapter_Process_glUniform4fv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000677 RID: 1655
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4i")]
	public static extern void GLAdapter_Process_glUniform4i(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x06000678 RID: 1656
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4iv")]
	public static extern void GLAdapter_Process_glUniform4iv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000679 RID: 1657
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix2fv")]
	public static extern void GLAdapter_Process_glUniformMatrix2fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600067A RID: 1658
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix3fv")]
	public static extern void GLAdapter_Process_glUniformMatrix3fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600067B RID: 1659
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix4fv")]
	public static extern void GLAdapter_Process_glUniformMatrix4fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600067C RID: 1660
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUseProgram")]
	public static extern void GLAdapter_Process_glUseProgram(HandleRef jarg1, uint jarg2);

	// Token: 0x0600067D RID: 1661
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glValidateProgram")]
	public static extern void GLAdapter_Process_glValidateProgram(HandleRef jarg1, uint jarg2);

	// Token: 0x0600067E RID: 1662
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib1f")]
	public static extern void GLAdapter_Process_glVertexAttrib1f(HandleRef jarg1, uint jarg2, float jarg3);

	// Token: 0x0600067F RID: 1663
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib1fv")]
	public static extern void GLAdapter_Process_glVertexAttrib1fv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000680 RID: 1664
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib2f")]
	public static extern void GLAdapter_Process_glVertexAttrib2f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4);

	// Token: 0x06000681 RID: 1665
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib2fv")]
	public static extern void GLAdapter_Process_glVertexAttrib2fv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000682 RID: 1666
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib3f")]
	public static extern void GLAdapter_Process_glVertexAttrib3f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x06000683 RID: 1667
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib3fv")]
	public static extern void GLAdapter_Process_glVertexAttrib3fv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000684 RID: 1668
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib4f")]
	public static extern void GLAdapter_Process_glVertexAttrib4f(HandleRef jarg1, uint jarg2, float jarg3, float jarg4, float jarg5, float jarg6);

	// Token: 0x06000685 RID: 1669
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttrib4fv")]
	public static extern void GLAdapter_Process_glVertexAttrib4fv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000686 RID: 1670
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribPointer")]
	public static extern void GLAdapter_Process_glVertexAttribPointer(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, HandleRef jarg7);

	// Token: 0x06000687 RID: 1671
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glViewport")]
	public static extern void GLAdapter_Process_glViewport(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

	// Token: 0x06000688 RID: 1672
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glReadBuffer")]
	public static extern void GLAdapter_Process_glReadBuffer(HandleRef jarg1, uint jarg2);

	// Token: 0x06000689 RID: 1673
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawRangeElements")]
	public static extern void GLAdapter_Process_glDrawRangeElements(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, uint jarg6, HandleRef jarg7);

	// Token: 0x0600068A RID: 1674
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexImage3D")]
	public static extern void GLAdapter_Process_glTexImage3D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, uint jarg9, uint jarg10, HandleRef jarg11);

	// Token: 0x0600068B RID: 1675
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexSubImage3D")]
	public static extern void GLAdapter_Process_glTexSubImage3D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, uint jarg10, uint jarg11, HandleRef jarg12);

	// Token: 0x0600068C RID: 1676
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyTexSubImage3D")]
	public static extern void GLAdapter_Process_glCopyTexSubImage3D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, int jarg10);

	// Token: 0x0600068D RID: 1677
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexImage3D")]
	public static extern void GLAdapter_Process_glCompressedTexImage3D(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, HandleRef jarg10);

	// Token: 0x0600068E RID: 1678
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexSubImage3D")]
	public static extern void GLAdapter_Process_glCompressedTexSubImage3D(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, uint jarg10, int jarg11, HandleRef jarg12);

	// Token: 0x0600068F RID: 1679
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenQueries")]
	public static extern void GLAdapter_Process_glGenQueries(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000690 RID: 1680
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteQueries")]
	public static extern void GLAdapter_Process_glDeleteQueries(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000691 RID: 1681
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsQuery")]
	public static extern void GLAdapter_Process_glIsQuery(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000692 RID: 1682
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBeginQuery")]
	public static extern void GLAdapter_Process_glBeginQuery(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000693 RID: 1683
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEndQuery")]
	public static extern void GLAdapter_Process_glEndQuery(HandleRef jarg1, uint jarg2);

	// Token: 0x06000694 RID: 1684
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryiv")]
	public static extern void GLAdapter_Process_glGetQueryiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000695 RID: 1685
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryObjectuiv")]
	public static extern void GLAdapter_Process_glGetQueryObjectuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000696 RID: 1686
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUnmapBuffer")]
	public static extern void GLAdapter_Process_glUnmapBuffer(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000697 RID: 1687
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetBufferPointerv")]
	public static extern void GLAdapter_Process_glGetBufferPointerv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000698 RID: 1688
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawBuffers")]
	public static extern void GLAdapter_Process_glDrawBuffers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000699 RID: 1689
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix2x3fv")]
	public static extern void GLAdapter_Process_glUniformMatrix2x3fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069A RID: 1690
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix3x2fv")]
	public static extern void GLAdapter_Process_glUniformMatrix3x2fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069B RID: 1691
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix2x4fv")]
	public static extern void GLAdapter_Process_glUniformMatrix2x4fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069C RID: 1692
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix4x2fv")]
	public static extern void GLAdapter_Process_glUniformMatrix4x2fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069D RID: 1693
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix3x4fv")]
	public static extern void GLAdapter_Process_glUniformMatrix3x4fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069E RID: 1694
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformMatrix4x3fv")]
	public static extern void GLAdapter_Process_glUniformMatrix4x3fv(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600069F RID: 1695
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitFramebuffer")]
	public static extern void GLAdapter_Process_glBlitFramebuffer(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, uint jarg10, uint jarg11);

	// Token: 0x060006A0 RID: 1696
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glRenderbufferStorageMultisample")]
	public static extern void GLAdapter_Process_glRenderbufferStorageMultisample(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060006A1 RID: 1697
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTextureLayer")]
	public static extern void GLAdapter_Process_glFramebufferTextureLayer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060006A2 RID: 1698
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMapBufferRange")]
	public static extern void GLAdapter_Process_glMapBufferRange(HandleRef jarg1, HandleRef jarg2, uint jarg3, int jarg4, int jarg5, uint jarg6);

	// Token: 0x060006A3 RID: 1699
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFlushMappedBufferRange")]
	public static extern void GLAdapter_Process_glFlushMappedBufferRange(HandleRef jarg1, uint jarg2, int jarg3, int jarg4);

	// Token: 0x060006A4 RID: 1700
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindVertexArray")]
	public static extern void GLAdapter_Process_glBindVertexArray(HandleRef jarg1, uint jarg2);

	// Token: 0x060006A5 RID: 1701
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteVertexArrays")]
	public static extern void GLAdapter_Process_glDeleteVertexArrays(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006A6 RID: 1702
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenVertexArrays")]
	public static extern void GLAdapter_Process_glGenVertexArrays(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006A7 RID: 1703
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsVertexArray")]
	public static extern void GLAdapter_Process_glIsVertexArray(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060006A8 RID: 1704
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetIntegeri_v")]
	public static extern void GLAdapter_Process_glGetIntegeri_v(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006A9 RID: 1705
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetBooleani_v")]
	public static extern void GLAdapter_Process_glGetBooleani_v(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006AA RID: 1706
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBeginTransformFeedback")]
	public static extern void GLAdapter_Process_glBeginTransformFeedback(HandleRef jarg1, uint jarg2);

	// Token: 0x060006AB RID: 1707
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEndTransformFeedback")]
	public static extern void GLAdapter_Process_glEndTransformFeedback(HandleRef jarg1);

	// Token: 0x060006AC RID: 1708
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindBufferRange")]
	public static extern void GLAdapter_Process_glBindBufferRange(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060006AD RID: 1709
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindBufferBase")]
	public static extern void GLAdapter_Process_glBindBufferBase(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006AE RID: 1710
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTransformFeedbackVaryings")]
	public static extern void GLAdapter_Process_glTransformFeedbackVaryings(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, uint jarg5);

	// Token: 0x060006AF RID: 1711
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTransformFeedbackVarying")]
	public static extern void GLAdapter_Process_glGetTransformFeedbackVarying(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8);

	// Token: 0x060006B0 RID: 1712
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribIPointer")]
	public static extern void GLAdapter_Process_glVertexAttribIPointer(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060006B1 RID: 1713
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetVertexAttribIiv")]
	public static extern void GLAdapter_Process_glGetVertexAttribIiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006B2 RID: 1714
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetVertexAttribIuiv")]
	public static extern void GLAdapter_Process_glGetVertexAttribIuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006B3 RID: 1715
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribI4i")]
	public static extern void GLAdapter_Process_glVertexAttribI4i(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x060006B4 RID: 1716
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribI4ui")]
	public static extern void GLAdapter_Process_glVertexAttribI4ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x060006B5 RID: 1717
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribI4iv")]
	public static extern void GLAdapter_Process_glVertexAttribI4iv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060006B6 RID: 1718
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribI4uiv")]
	public static extern void GLAdapter_Process_glVertexAttribI4uiv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060006B7 RID: 1719
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformuiv")]
	public static extern void GLAdapter_Process_glGetUniformuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006B8 RID: 1720
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFragDataLocation")]
	public static extern void GLAdapter_Process_glGetFragDataLocation(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006B9 RID: 1721
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1ui")]
	public static extern void GLAdapter_Process_glUniform1ui(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060006BA RID: 1722
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2ui")]
	public static extern void GLAdapter_Process_glUniform2ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006BB RID: 1723
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3ui")]
	public static extern void GLAdapter_Process_glUniform3ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060006BC RID: 1724
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4ui")]
	public static extern void GLAdapter_Process_glUniform4ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x060006BD RID: 1725
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform1uiv")]
	public static extern void GLAdapter_Process_glUniform1uiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006BE RID: 1726
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform2uiv")]
	public static extern void GLAdapter_Process_glUniform2uiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006BF RID: 1727
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform3uiv")]
	public static extern void GLAdapter_Process_glUniform3uiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006C0 RID: 1728
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniform4uiv")]
	public static extern void GLAdapter_Process_glUniform4uiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006C1 RID: 1729
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearBufferiv")]
	public static extern void GLAdapter_Process_glClearBufferiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006C2 RID: 1730
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearBufferuiv")]
	public static extern void GLAdapter_Process_glClearBufferuiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006C3 RID: 1731
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearBufferfv")]
	public static extern void GLAdapter_Process_glClearBufferfv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006C4 RID: 1732
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClearBufferfi")]
	public static extern void GLAdapter_Process_glClearBufferfi(HandleRef jarg1, uint jarg2, int jarg3, float jarg4, int jarg5);

	// Token: 0x060006C5 RID: 1733
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetStringi")]
	public static extern void GLAdapter_Process_glGetStringi(HandleRef jarg1, HandleRef jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006C6 RID: 1734
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyBufferSubData")]
	public static extern void GLAdapter_Process_glCopyBufferSubData(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x060006C7 RID: 1735
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformIndices")]
	public static extern void GLAdapter_Process_glGetUniformIndices(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x060006C8 RID: 1736
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveUniformsiv")]
	public static extern void GLAdapter_Process_glGetActiveUniformsiv(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060006C9 RID: 1737
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUniformBlockIndex")]
	public static extern void GLAdapter_Process_glGetUniformBlockIndex(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006CA RID: 1738
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveUniformBlockiv")]
	public static extern void GLAdapter_Process_glGetActiveUniformBlockiv(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060006CB RID: 1739
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveUniformBlockName")]
	public static extern void GLAdapter_Process_glGetActiveUniformBlockName(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x060006CC RID: 1740
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUniformBlockBinding")]
	public static extern void GLAdapter_Process_glUniformBlockBinding(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006CD RID: 1741
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawArraysInstanced")]
	public static extern void GLAdapter_Process_glDrawArraysInstanced(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5);

	// Token: 0x060006CE RID: 1742
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawElementsInstanced")]
	public static extern void GLAdapter_Process_glDrawElementsInstanced(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5, int jarg6);

	// Token: 0x060006CF RID: 1743
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFenceSync")]
	public static extern void GLAdapter_Process_glFenceSync(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006D0 RID: 1744
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsSync")]
	public static extern void GLAdapter_Process_glIsSync(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060006D1 RID: 1745
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteSync")]
	public static extern void GLAdapter_Process_glDeleteSync(HandleRef jarg1, uint jarg2);

	// Token: 0x060006D2 RID: 1746
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClientWaitSync")]
	public static extern void GLAdapter_Process_glClientWaitSync(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, ulong jarg5);

	// Token: 0x060006D3 RID: 1747
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glWaitSync")]
	public static extern void GLAdapter_Process_glWaitSync(HandleRef jarg1, uint jarg2, uint jarg3, ulong jarg4);

	// Token: 0x060006D4 RID: 1748
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetInteger64v")]
	public static extern void GLAdapter_Process_glGetInteger64v(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060006D5 RID: 1749
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSynciv")]
	public static extern void GLAdapter_Process_glGetSynciv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x060006D6 RID: 1750
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetInteger64i_v")]
	public static extern void GLAdapter_Process_glGetInteger64i_v(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006D7 RID: 1751
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetBufferParameteri64v")]
	public static extern void GLAdapter_Process_glGetBufferParameteri64v(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006D8 RID: 1752
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenSamplers")]
	public static extern void GLAdapter_Process_glGenSamplers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006D9 RID: 1753
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteSamplers")]
	public static extern void GLAdapter_Process_glDeleteSamplers(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006DA RID: 1754
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsSampler")]
	public static extern void GLAdapter_Process_glIsSampler(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060006DB RID: 1755
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindSampler")]
	public static extern void GLAdapter_Process_glBindSampler(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060006DC RID: 1756
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameteri")]
	public static extern void GLAdapter_Process_glSamplerParameteri(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x060006DD RID: 1757
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameteriv")]
	public static extern void GLAdapter_Process_glSamplerParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006DE RID: 1758
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterf")]
	public static extern void GLAdapter_Process_glSamplerParameterf(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4);

	// Token: 0x060006DF RID: 1759
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterfv")]
	public static extern void GLAdapter_Process_glSamplerParameterfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006E0 RID: 1760
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameteriv")]
	public static extern void GLAdapter_Process_glGetSamplerParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006E1 RID: 1761
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameterfv")]
	public static extern void GLAdapter_Process_glGetSamplerParameterfv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006E2 RID: 1762
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribDivisor")]
	public static extern void GLAdapter_Process_glVertexAttribDivisor(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060006E3 RID: 1763
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindTransformFeedback")]
	public static extern void GLAdapter_Process_glBindTransformFeedback(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060006E4 RID: 1764
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteTransformFeedbacks")]
	public static extern void GLAdapter_Process_glDeleteTransformFeedbacks(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006E5 RID: 1765
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenTransformFeedbacks")]
	public static extern void GLAdapter_Process_glGenTransformFeedbacks(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060006E6 RID: 1766
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsTransformFeedback")]
	public static extern void GLAdapter_Process_glIsTransformFeedback(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060006E7 RID: 1767
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPauseTransformFeedback")]
	public static extern void GLAdapter_Process_glPauseTransformFeedback(HandleRef jarg1);

	// Token: 0x060006E8 RID: 1768
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glResumeTransformFeedback")]
	public static extern void GLAdapter_Process_glResumeTransformFeedback(HandleRef jarg1);

	// Token: 0x060006E9 RID: 1769
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramBinary")]
	public static extern void GLAdapter_Process_glGetProgramBinary(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x060006EA RID: 1770
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramBinary")]
	public static extern void GLAdapter_Process_glProgramBinary(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4, int jarg5);

	// Token: 0x060006EB RID: 1771
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramParameteri")]
	public static extern void GLAdapter_Process_glProgramParameteri(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x060006EC RID: 1772
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glInvalidateFramebuffer")]
	public static extern void GLAdapter_Process_glInvalidateFramebuffer(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060006ED RID: 1773
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glInvalidateSubFramebuffer")]
	public static extern void GLAdapter_Process_glInvalidateSubFramebuffer(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, int jarg5, int jarg6, int jarg7, int jarg8);

	// Token: 0x060006EE RID: 1774
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorage2D")]
	public static extern void GLAdapter_Process_glTexStorage2D(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060006EF RID: 1775
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorage3D")]
	public static extern void GLAdapter_Process_glTexStorage3D(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x060006F0 RID: 1776
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetInternalformativ")]
	public static extern void GLAdapter_Process_glGetInternalformativ(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x060006F1 RID: 1777
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDispatchCompute")]
	public static extern void GLAdapter_Process_glDispatchCompute(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006F2 RID: 1778
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDispatchComputeIndirect")]
	public static extern void GLAdapter_Process_glDispatchComputeIndirect(HandleRef jarg1, int jarg2);

	// Token: 0x060006F3 RID: 1779
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawArraysIndirect")]
	public static extern void GLAdapter_Process_glDrawArraysIndirect(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060006F4 RID: 1780
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawElementsIndirect")]
	public static extern void GLAdapter_Process_glDrawElementsIndirect(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006F5 RID: 1781
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferParameteri")]
	public static extern void GLAdapter_Process_glFramebufferParameteri(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x060006F6 RID: 1782
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFramebufferParameteriv")]
	public static extern void GLAdapter_Process_glGetFramebufferParameteriv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060006F7 RID: 1783
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramInterfaceiv")]
	public static extern void GLAdapter_Process_glGetProgramInterfaceiv(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060006F8 RID: 1784
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramResourceIndex")]
	public static extern void GLAdapter_Process_glGetProgramResourceIndex(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060006F9 RID: 1785
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramResourceName")]
	public static extern void GLAdapter_Process_glGetProgramResourceName(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6, HandleRef jarg7);

	// Token: 0x060006FA RID: 1786
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramResourceiv")]
	public static extern void GLAdapter_Process_glGetProgramResourceiv(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6, int jarg7, HandleRef jarg8, HandleRef jarg9);

	// Token: 0x060006FB RID: 1787
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramResourceLocation")]
	public static extern void GLAdapter_Process_glGetProgramResourceLocation(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060006FC RID: 1788
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glUseProgramStages")]
	public static extern void GLAdapter_Process_glUseProgramStages(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060006FD RID: 1789
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glActiveShaderProgram")]
	public static extern void GLAdapter_Process_glActiveShaderProgram(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060006FE RID: 1790
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCreateShaderProgramv")]
	public static extern void GLAdapter_Process_glCreateShaderProgramv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060006FF RID: 1791
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindProgramPipeline")]
	public static extern void GLAdapter_Process_glBindProgramPipeline(HandleRef jarg1, uint jarg2);

	// Token: 0x06000700 RID: 1792
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteProgramPipelines")]
	public static extern void GLAdapter_Process_glDeleteProgramPipelines(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000701 RID: 1793
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenProgramPipelines")]
	public static extern void GLAdapter_Process_glGenProgramPipelines(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000702 RID: 1794
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsProgramPipeline")]
	public static extern void GLAdapter_Process_glIsProgramPipeline(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000703 RID: 1795
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramPipelineiv")]
	public static extern void GLAdapter_Process_glGetProgramPipelineiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000704 RID: 1796
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1i")]
	public static extern void GLAdapter_Process_glProgramUniform1i(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x06000705 RID: 1797
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2i")]
	public static extern void GLAdapter_Process_glProgramUniform2i(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5);

	// Token: 0x06000706 RID: 1798
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3i")]
	public static extern void GLAdapter_Process_glProgramUniform3i(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x06000707 RID: 1799
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4i")]
	public static extern void GLAdapter_Process_glProgramUniform4i(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x06000708 RID: 1800
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1ui")]
	public static extern void GLAdapter_Process_glProgramUniform1ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000709 RID: 1801
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2ui")]
	public static extern void GLAdapter_Process_glProgramUniform2ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x0600070A RID: 1802
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3ui")]
	public static extern void GLAdapter_Process_glProgramUniform3ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x0600070B RID: 1803
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4ui")]
	public static extern void GLAdapter_Process_glProgramUniform4ui(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600070C RID: 1804
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1f")]
	public static extern void GLAdapter_Process_glProgramUniform1f(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4);

	// Token: 0x0600070D RID: 1805
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2f")]
	public static extern void GLAdapter_Process_glProgramUniform2f(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4, float jarg5);

	// Token: 0x0600070E RID: 1806
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3f")]
	public static extern void GLAdapter_Process_glProgramUniform3f(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4, float jarg5, float jarg6);

	// Token: 0x0600070F RID: 1807
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4f")]
	public static extern void GLAdapter_Process_glProgramUniform4f(HandleRef jarg1, uint jarg2, uint jarg3, float jarg4, float jarg5, float jarg6, float jarg7);

	// Token: 0x06000710 RID: 1808
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1iv")]
	public static extern void GLAdapter_Process_glProgramUniform1iv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000711 RID: 1809
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2iv")]
	public static extern void GLAdapter_Process_glProgramUniform2iv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000712 RID: 1810
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3iv")]
	public static extern void GLAdapter_Process_glProgramUniform3iv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000713 RID: 1811
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4iv")]
	public static extern void GLAdapter_Process_glProgramUniform4iv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000714 RID: 1812
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1uiv")]
	public static extern void GLAdapter_Process_glProgramUniform1uiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000715 RID: 1813
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2uiv")]
	public static extern void GLAdapter_Process_glProgramUniform2uiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000716 RID: 1814
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3uiv")]
	public static extern void GLAdapter_Process_glProgramUniform3uiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000717 RID: 1815
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4uiv")]
	public static extern void GLAdapter_Process_glProgramUniform4uiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000718 RID: 1816
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform1fv")]
	public static extern void GLAdapter_Process_glProgramUniform1fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000719 RID: 1817
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform2fv")]
	public static extern void GLAdapter_Process_glProgramUniform2fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600071A RID: 1818
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform3fv")]
	public static extern void GLAdapter_Process_glProgramUniform3fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600071B RID: 1819
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniform4fv")]
	public static extern void GLAdapter_Process_glProgramUniform4fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600071C RID: 1820
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix2fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix2fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x0600071D RID: 1821
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix3fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix3fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x0600071E RID: 1822
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix4fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix4fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x0600071F RID: 1823
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix2x3fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix2x3fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000720 RID: 1824
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix3x2fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix3x2fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000721 RID: 1825
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix2x4fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix2x4fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000722 RID: 1826
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix4x2fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix4x2fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000723 RID: 1827
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix3x4fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix3x4fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000724 RID: 1828
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramUniformMatrix4x3fv")]
	public static extern void GLAdapter_Process_glProgramUniformMatrix4x3fv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000725 RID: 1829
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glValidateProgramPipeline")]
	public static extern void GLAdapter_Process_glValidateProgramPipeline(HandleRef jarg1, uint jarg2);

	// Token: 0x06000726 RID: 1830
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramPipelineInfoLog")]
	public static extern void GLAdapter_Process_glGetProgramPipelineInfoLog(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000727 RID: 1831
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetActiveAtomicCounterBufferiv")]
	public static extern void GLAdapter_Process_glGetActiveAtomicCounterBufferiv(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000728 RID: 1832
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindImageTexture")]
	public static extern void GLAdapter_Process_glBindImageTexture(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6, uint jarg7, uint jarg8);

	// Token: 0x06000729 RID: 1833
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMemoryBarrier")]
	public static extern void GLAdapter_Process_glMemoryBarrier(HandleRef jarg1, uint jarg2);

	// Token: 0x0600072A RID: 1834
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMemoryBarrierByRegion")]
	public static extern void GLAdapter_Process_glMemoryBarrierByRegion(HandleRef jarg1, uint jarg2);

	// Token: 0x0600072B RID: 1835
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorage2DMultisample")]
	public static extern void GLAdapter_Process_glTexStorage2DMultisample(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x0600072C RID: 1836
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorage3DMultisampleOES")]
	public static extern void GLAdapter_Process_glTexStorage3DMultisampleOES(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8);

	// Token: 0x0600072D RID: 1837
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetMultisamplefv")]
	public static extern void GLAdapter_Process_glGetMultisamplefv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600072E RID: 1838
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSampleMaski")]
	public static extern void GLAdapter_Process_glSampleMaski(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x0600072F RID: 1839
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexLevelParameteriv")]
	public static extern void GLAdapter_Process_glGetTexLevelParameteriv(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000730 RID: 1840
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexLevelParameterfv")]
	public static extern void GLAdapter_Process_glGetTexLevelParameterfv(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000731 RID: 1841
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindVertexBuffer")]
	public static extern void GLAdapter_Process_glBindVertexBuffer(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5);

	// Token: 0x06000732 RID: 1842
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribFormat")]
	public static extern void GLAdapter_Process_glVertexAttribFormat(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, uint jarg6);

	// Token: 0x06000733 RID: 1843
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribIFormat")]
	public static extern void GLAdapter_Process_glVertexAttribIFormat(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000734 RID: 1844
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexAttribBinding")]
	public static extern void GLAdapter_Process_glVertexAttribBinding(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000735 RID: 1845
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glVertexBindingDivisor")]
	public static extern void GLAdapter_Process_glVertexBindingDivisor(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000736 RID: 1846
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPatchParameteri")]
	public static extern void GLAdapter_Process_glPatchParameteri(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x06000737 RID: 1847
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFixedvAMD")]
	public static extern void GLAdapter_Process_glGetFixedvAMD(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000738 RID: 1848
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glLogicOpAMD")]
	public static extern void GLAdapter_Process_glLogicOpAMD(HandleRef jarg1, uint jarg2);

	// Token: 0x06000739 RID: 1849
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFogfvAMD")]
	public static extern void GLAdapter_Process_glFogfvAMD(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x0600073A RID: 1850
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetMemoryStatsQCOM")]
	public static extern void GLAdapter_Process_glGetMemoryStatsQCOM(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600073B RID: 1851
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSizedMemoryStatsQCOM")]
	public static extern void GLAdapter_Process_glGetSizedMemoryStatsQCOM(HandleRef jarg1, int jarg2, HandleRef jarg3, HandleRef jarg4);

	// Token: 0x0600073C RID: 1852
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitOverlapQCOM")]
	public static extern void GLAdapter_Process_glBlitOverlapQCOM(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x0600073D RID: 1853
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetShaderStatsQCOM")]
	public static extern void GLAdapter_Process_glGetShaderStatsQCOM(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x0600073E RID: 1854
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glExtGetSamplersQCOM")]
	public static extern void GLAdapter_Process_glExtGetSamplersQCOM(HandleRef jarg1, HandleRef jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x0600073F RID: 1855
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glClipPlanefQCOM")]
	public static extern void GLAdapter_Process_glClipPlanefQCOM(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000740 RID: 1856
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTexture2DExternalQCOM")]
	public static extern void GLAdapter_Process_glFramebufferTexture2DExternalQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6);

	// Token: 0x06000741 RID: 1857
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferRenderbufferExternalQCOM")]
	public static extern void GLAdapter_Process_glFramebufferRenderbufferExternalQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000742 RID: 1858
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEGLImageTargetTexture2DOES")]
	public static extern void GLAdapter_Process_glEGLImageTargetTexture2DOES(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000743 RID: 1859
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEGLImageTargetRenderbufferStorageOES")]
	public static extern void GLAdapter_Process_glEGLImageTargetRenderbufferStorageOES(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000744 RID: 1860
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramBinaryOES")]
	public static extern void GLAdapter_Process_glGetProgramBinaryOES(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x06000745 RID: 1861
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glProgramBinaryOES")]
	public static extern void GLAdapter_Process_glProgramBinaryOES(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4, int jarg5);

	// Token: 0x06000746 RID: 1862
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexImage3DOES")]
	public static extern void GLAdapter_Process_glTexImage3DOES(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, uint jarg9, uint jarg10, HandleRef jarg11);

	// Token: 0x06000747 RID: 1863
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexSubImage3DOES")]
	public static extern void GLAdapter_Process_glTexSubImage3DOES(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, uint jarg10, uint jarg11, HandleRef jarg12);

	// Token: 0x06000748 RID: 1864
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyTexSubImage3DOES")]
	public static extern void GLAdapter_Process_glCopyTexSubImage3DOES(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, int jarg10);

	// Token: 0x06000749 RID: 1865
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexImage3DOES")]
	public static extern void GLAdapter_Process_glCompressedTexImage3DOES(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, HandleRef jarg10);

	// Token: 0x0600074A RID: 1866
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCompressedTexSubImage3DOES")]
	public static extern void GLAdapter_Process_glCompressedTexSubImage3DOES(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6, int jarg7, int jarg8, int jarg9, uint jarg10, int jarg11, HandleRef jarg12);

	// Token: 0x0600074B RID: 1867
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTexture3DOES")]
	public static extern void GLAdapter_Process_glFramebufferTexture3DOES(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6, int jarg7);

	// Token: 0x0600074C RID: 1868
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindVertexArrayOES")]
	public static extern void GLAdapter_Process_glBindVertexArrayOES(HandleRef jarg1, uint jarg2);

	// Token: 0x0600074D RID: 1869
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteVertexArraysOES")]
	public static extern void GLAdapter_Process_glDeleteVertexArraysOES(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600074E RID: 1870
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenVertexArraysOES")]
	public static extern void GLAdapter_Process_glGenVertexArraysOES(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600074F RID: 1871
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsVertexArrayOES")]
	public static extern void GLAdapter_Process_glIsVertexArrayOES(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000750 RID: 1872
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorGroupsAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorGroupsAMD(HandleRef jarg1, HandleRef jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000751 RID: 1873
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorCountersAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorCountersAMD(HandleRef jarg1, uint jarg2, HandleRef jarg3, HandleRef jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000752 RID: 1874
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorGroupStringAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorGroupStringAMD(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000753 RID: 1875
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorCounterStringAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorCounterStringAMD(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x06000754 RID: 1876
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorCounterInfoAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorCounterInfoAMD(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x06000755 RID: 1877
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenPerfMonitorsAMD")]
	public static extern void GLAdapter_Process_glGenPerfMonitorsAMD(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000756 RID: 1878
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeletePerfMonitorsAMD")]
	public static extern void GLAdapter_Process_glDeletePerfMonitorsAMD(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000757 RID: 1879
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSelectPerfMonitorCountersAMD")]
	public static extern void GLAdapter_Process_glSelectPerfMonitorCountersAMD(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, HandleRef jarg6);

	// Token: 0x06000758 RID: 1880
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBeginPerfMonitorAMD")]
	public static extern void GLAdapter_Process_glBeginPerfMonitorAMD(HandleRef jarg1, uint jarg2);

	// Token: 0x06000759 RID: 1881
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEndPerfMonitorAMD")]
	public static extern void GLAdapter_Process_glEndPerfMonitorAMD(HandleRef jarg1, uint jarg2);

	// Token: 0x0600075A RID: 1882
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPerfMonitorCounterDataAMD")]
	public static extern void GLAdapter_Process_glGetPerfMonitorCounterDataAMD(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x0600075B RID: 1883
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glLabelObjectEXT")]
	public static extern void GLAdapter_Process_glLabelObjectEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600075C RID: 1884
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetObjectLabelEXT")]
	public static extern void GLAdapter_Process_glGetObjectLabelEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x0600075D RID: 1885
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glInsertEventMarkerEXT")]
	public static extern void GLAdapter_Process_glInsertEventMarkerEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600075E RID: 1886
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPushGroupMarkerEXT")]
	public static extern void GLAdapter_Process_glPushGroupMarkerEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x0600075F RID: 1887
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPopGroupMarkerEXT")]
	public static extern void GLAdapter_Process_glPopGroupMarkerEXT(HandleRef jarg1);

	// Token: 0x06000760 RID: 1888
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDiscardFramebufferEXT")]
	public static extern void GLAdapter_Process_glDiscardFramebufferEXT(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000761 RID: 1889
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenQueriesEXT")]
	public static extern void GLAdapter_Process_glGenQueriesEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000762 RID: 1890
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteQueriesEXT")]
	public static extern void GLAdapter_Process_glDeleteQueriesEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000763 RID: 1891
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsQueryEXT")]
	public static extern void GLAdapter_Process_glIsQueryEXT(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000764 RID: 1892
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBeginQueryEXT")]
	public static extern void GLAdapter_Process_glBeginQueryEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000765 RID: 1893
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEndQueryEXT")]
	public static extern void GLAdapter_Process_glEndQueryEXT(HandleRef jarg1, uint jarg2);

	// Token: 0x06000766 RID: 1894
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glQueryCounterEXT")]
	public static extern void GLAdapter_Process_glQueryCounterEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000767 RID: 1895
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryivEXT")]
	public static extern void GLAdapter_Process_glGetQueryivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000768 RID: 1896
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryObjectivEXT")]
	public static extern void GLAdapter_Process_glGetQueryObjectivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000769 RID: 1897
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryObjectuivEXT")]
	public static extern void GLAdapter_Process_glGetQueryObjectuivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600076A RID: 1898
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryObjecti64vEXT")]
	public static extern void GLAdapter_Process_glGetQueryObjecti64vEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600076B RID: 1899
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetQueryObjectui64vEXT")]
	public static extern void GLAdapter_Process_glGetQueryObjectui64vEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x0600076C RID: 1900
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetGraphicsResetStatusEXT")]
	public static extern void GLAdapter_Process_glGetGraphicsResetStatusEXT(HandleRef jarg1, uint jarg2);

	// Token: 0x0600076D RID: 1901
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glReadnPixelsEXT")]
	public static extern void GLAdapter_Process_glReadnPixelsEXT(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5, uint jarg6, uint jarg7, int jarg8, HandleRef jarg9);

	// Token: 0x0600076E RID: 1902
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetnUniformfvEXT")]
	public static extern void GLAdapter_Process_glGetnUniformfvEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600076F RID: 1903
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetnUniformivEXT")]
	public static extern void GLAdapter_Process_glGetnUniformivEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000770 RID: 1904
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterIivEXT")]
	public static extern void GLAdapter_Process_glTexParameterIivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000771 RID: 1905
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterIuivEXT")]
	public static extern void GLAdapter_Process_glTexParameterIuivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000772 RID: 1906
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameterIivEXT")]
	public static extern void GLAdapter_Process_glGetTexParameterIivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000773 RID: 1907
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameterIuivEXT")]
	public static extern void GLAdapter_Process_glGetTexParameterIuivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000774 RID: 1908
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterIivEXT")]
	public static extern void GLAdapter_Process_glSamplerParameterIivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000775 RID: 1909
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterIuivEXT")]
	public static extern void GLAdapter_Process_glSamplerParameterIuivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000776 RID: 1910
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameterIivEXT")]
	public static extern void GLAdapter_Process_glGetSamplerParameterIivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000777 RID: 1911
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameterIuivEXT")]
	public static extern void GLAdapter_Process_glGetSamplerParameterIuivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x06000778 RID: 1912
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glRenderbufferStorageMultisampleEXT")]
	public static extern void GLAdapter_Process_glRenderbufferStorageMultisampleEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x06000779 RID: 1913
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTexture2DMultisampleEXT")]
	public static extern void GLAdapter_Process_glFramebufferTexture2DMultisampleEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6, int jarg7);

	// Token: 0x0600077A RID: 1914
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glAlphaFuncQCOM")]
	public static extern void GLAdapter_Process_glAlphaFuncQCOM(HandleRef jarg1, uint jarg2, float jarg3);

	// Token: 0x0600077B RID: 1915
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glStartTilingQCOM")]
	public static extern void GLAdapter_Process_glStartTilingQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x0600077C RID: 1916
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEndTilingQCOM")]
	public static extern void GLAdapter_Process_glEndTilingQCOM(HandleRef jarg1, uint jarg2);

	// Token: 0x0600077D RID: 1917
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyImageSubDataEXT")]
	public static extern void GLAdapter_Process_glCopyImageSubDataEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, uint jarg9, int jarg10, int jarg11, int jarg12, int jarg13, int jarg14, int jarg15, int jarg16);

	// Token: 0x0600077E RID: 1918
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendBarrierKHR")]
	public static extern void GLAdapter_Process_glBlendBarrierKHR(HandleRef jarg1);

	// Token: 0x0600077F RID: 1919
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMinSampleShadingOES")]
	public static extern void GLAdapter_Process_glMinSampleShadingOES(HandleRef jarg1, float jarg2);

	// Token: 0x06000780 RID: 1920
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEnableiEXT")]
	public static extern void GLAdapter_Process_glEnableiEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000781 RID: 1921
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDisableiEXT")]
	public static extern void GLAdapter_Process_glDisableiEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000782 RID: 1922
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquationiEXT")]
	public static extern void GLAdapter_Process_glBlendEquationiEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000783 RID: 1923
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquationSeparateiEXT")]
	public static extern void GLAdapter_Process_glBlendEquationSeparateiEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000784 RID: 1924
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFunciEXT")]
	public static extern void GLAdapter_Process_glBlendFunciEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000785 RID: 1925
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFuncSeparateiEXT")]
	public static extern void GLAdapter_Process_glBlendFuncSeparateiEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x06000786 RID: 1926
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glColorMaskiEXT")]
	public static extern void GLAdapter_Process_glColorMaskiEXT(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x06000787 RID: 1927
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsEnablediEXT")]
	public static extern void GLAdapter_Process_glIsEnablediEXT(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000788 RID: 1928
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexBufferEXT")]
	public static extern void GLAdapter_Process_glTexBufferEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000789 RID: 1929
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexBufferRangeEXT")]
	public static extern void GLAdapter_Process_glTexBufferRangeEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x0600078A RID: 1930
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageControlKHR")]
	public static extern void GLAdapter_Process_glDebugMessageControlKHR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6, int jarg7);

	// Token: 0x0600078B RID: 1931
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageInsertKHR")]
	public static extern void GLAdapter_Process_glDebugMessageInsertKHR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6, HandleRef jarg7);

	// Token: 0x0600078C RID: 1932
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageCallbackKHR")]
	public static extern void GLAdapter_Process_glDebugMessageCallbackKHR(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

	// Token: 0x0600078D RID: 1933
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetDebugMessageLogKHR")]
	public static extern void GLAdapter_Process_glGetDebugMessageLogKHR(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8, HandleRef jarg9, HandleRef jarg10);

	// Token: 0x0600078E RID: 1934
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPushDebugGroupKHR")]
	public static extern void GLAdapter_Process_glPushDebugGroupKHR(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x0600078F RID: 1935
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPopDebugGroupKHR")]
	public static extern void GLAdapter_Process_glPopDebugGroupKHR(HandleRef jarg1);

	// Token: 0x06000790 RID: 1936
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glObjectLabelKHR")]
	public static extern void GLAdapter_Process_glObjectLabelKHR(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x06000791 RID: 1937
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetObjectLabelKHR")]
	public static extern void GLAdapter_Process_glGetObjectLabelKHR(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x06000792 RID: 1938
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glObjectPtrLabelKHR")]
	public static extern void GLAdapter_Process_glObjectPtrLabelKHR(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x06000793 RID: 1939
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetObjectPtrLabelKHR")]
	public static extern void GLAdapter_Process_glGetObjectPtrLabelKHR(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x06000794 RID: 1940
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPointervKHR")]
	public static extern void GLAdapter_Process_glGetPointervKHR(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000795 RID: 1941
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPrimitiveBoundingBoxEXT")]
	public static extern void GLAdapter_Process_glPrimitiveBoundingBoxEXT(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9);

	// Token: 0x06000796 RID: 1942
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPatchParameteriEXT")]
	public static extern void GLAdapter_Process_glPatchParameteriEXT(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x06000797 RID: 1943
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawElementsBaseVertex")]
	public static extern void GLAdapter_Process_glDrawElementsBaseVertex(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5, int jarg6);

	// Token: 0x06000798 RID: 1944
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawRangeElementsBaseVertex")]
	public static extern void GLAdapter_Process_glDrawRangeElementsBaseVertex(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, uint jarg6, HandleRef jarg7, int jarg8);

	// Token: 0x06000799 RID: 1945
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDrawElementsInstancedBaseVertex")]
	public static extern void GLAdapter_Process_glDrawElementsInstancedBaseVertex(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, HandleRef jarg5, int jarg6, int jarg7);

	// Token: 0x0600079A RID: 1946
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTextureEXT")]
	public static extern void GLAdapter_Process_glFramebufferTextureEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5);

	// Token: 0x0600079B RID: 1947
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTextureMultiviewOVR")]
	public static extern void GLAdapter_Process_glFramebufferTextureMultiviewOVR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x0600079C RID: 1948
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferTextureMultisampleMultiviewOVR")]
	public static extern void GLAdapter_Process_glFramebufferTextureMultisampleMultiviewOVR(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8);

	// Token: 0x0600079D RID: 1949
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBufferStorageEXT")]
	public static extern void GLAdapter_Process_glBufferStorageEXT(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, uint jarg5);

	// Token: 0x0600079E RID: 1950
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetGraphicsResetStatus")]
	public static extern void GLAdapter_Process_glGetGraphicsResetStatus(HandleRef jarg1, uint jarg2);

	// Token: 0x0600079F RID: 1951
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glReadnPixels")]
	public static extern void GLAdapter_Process_glReadnPixels(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5, uint jarg6, uint jarg7, int jarg8, HandleRef jarg9);

	// Token: 0x060007A0 RID: 1952
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetnUniformfv")]
	public static extern void GLAdapter_Process_glGetnUniformfv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060007A1 RID: 1953
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetnUniformiv")]
	public static extern void GLAdapter_Process_glGetnUniformiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060007A2 RID: 1954
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetnUniformuiv")]
	public static extern void GLAdapter_Process_glGetnUniformuiv(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060007A3 RID: 1955
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterIiv")]
	public static extern void GLAdapter_Process_glTexParameterIiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A4 RID: 1956
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexParameterIuiv")]
	public static extern void GLAdapter_Process_glTexParameterIuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A5 RID: 1957
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameterIiv")]
	public static extern void GLAdapter_Process_glGetTexParameterIiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A6 RID: 1958
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetTexParameterIuiv")]
	public static extern void GLAdapter_Process_glGetTexParameterIuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A7 RID: 1959
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterIiv")]
	public static extern void GLAdapter_Process_glSamplerParameterIiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A8 RID: 1960
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSamplerParameterIuiv")]
	public static extern void GLAdapter_Process_glSamplerParameterIuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007A9 RID: 1961
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameterIiv")]
	public static extern void GLAdapter_Process_glGetSamplerParameterIiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007AA RID: 1962
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetSamplerParameterIuiv")]
	public static extern void GLAdapter_Process_glGetSamplerParameterIuiv(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007AB RID: 1963
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glNumBinsPerSubmitQCOM")]
	public static extern void GLAdapter_Process_glNumBinsPerSubmitQCOM(HandleRef jarg1, uint jarg2, int jarg3);

	// Token: 0x060007AC RID: 1964
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCopyImageSubData")]
	public static extern void GLAdapter_Process_glCopyImageSubData(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, uint jarg9, int jarg10, int jarg11, int jarg12, int jarg13, int jarg14, int jarg15, int jarg16);

	// Token: 0x060007AD RID: 1965
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendBarrier")]
	public static extern void GLAdapter_Process_glBlendBarrier(HandleRef jarg1);

	// Token: 0x060007AE RID: 1966
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMinSampleShading")]
	public static extern void GLAdapter_Process_glMinSampleShading(HandleRef jarg1, float jarg2);

	// Token: 0x060007AF RID: 1967
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEnablei")]
	public static extern void GLAdapter_Process_glEnablei(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060007B0 RID: 1968
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDisablei")]
	public static extern void GLAdapter_Process_glDisablei(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060007B1 RID: 1969
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquationi")]
	public static extern void GLAdapter_Process_glBlendEquationi(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060007B2 RID: 1970
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendEquationSeparatei")]
	public static extern void GLAdapter_Process_glBlendEquationSeparatei(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060007B3 RID: 1971
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFunci")]
	public static extern void GLAdapter_Process_glBlendFunci(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060007B4 RID: 1972
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlendFuncSeparatei")]
	public static extern void GLAdapter_Process_glBlendFuncSeparatei(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6);

	// Token: 0x060007B5 RID: 1973
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glColorMaski")]
	public static extern void GLAdapter_Process_glColorMaski(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, int jarg5, int jarg6);

	// Token: 0x060007B6 RID: 1974
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsEnabledi")]
	public static extern void GLAdapter_Process_glIsEnabledi(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4);

	// Token: 0x060007B7 RID: 1975
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexBuffer")]
	public static extern void GLAdapter_Process_glTexBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060007B8 RID: 1976
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexBufferRange")]
	public static extern void GLAdapter_Process_glTexBufferRange(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6);

	// Token: 0x060007B9 RID: 1977
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageControl")]
	public static extern void GLAdapter_Process_glDebugMessageControl(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, HandleRef jarg6, int jarg7);

	// Token: 0x060007BA RID: 1978
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageInsert")]
	public static extern void GLAdapter_Process_glDebugMessageInsert(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, int jarg6, HandleRef jarg7);

	// Token: 0x060007BB RID: 1979
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDebugMessageCallback")]
	public static extern void GLAdapter_Process_glDebugMessageCallback(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

	// Token: 0x060007BC RID: 1980
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetDebugMessageLog")]
	public static extern void GLAdapter_Process_glGetDebugMessageLog(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7, HandleRef jarg8, HandleRef jarg9, HandleRef jarg10);

	// Token: 0x060007BD RID: 1981
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPushDebugGroup")]
	public static extern void GLAdapter_Process_glPushDebugGroup(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060007BE RID: 1982
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPopDebugGroup")]
	public static extern void GLAdapter_Process_glPopDebugGroup(HandleRef jarg1);

	// Token: 0x060007BF RID: 1983
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glObjectLabel")]
	public static extern void GLAdapter_Process_glObjectLabel(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5);

	// Token: 0x060007C0 RID: 1984
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetObjectLabel")]
	public static extern void GLAdapter_Process_glGetObjectLabel(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4, HandleRef jarg5, HandleRef jarg6);

	// Token: 0x060007C1 RID: 1985
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glObjectPtrLabel")]
	public static extern void GLAdapter_Process_glObjectPtrLabel(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4);

	// Token: 0x060007C2 RID: 1986
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetObjectPtrLabel")]
	public static extern void GLAdapter_Process_glGetObjectPtrLabel(HandleRef jarg1, uint jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x060007C3 RID: 1987
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetPointerv")]
	public static extern void GLAdapter_Process_glGetPointerv(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060007C4 RID: 1988
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPrimitiveBoundingBox")]
	public static extern void GLAdapter_Process_glPrimitiveBoundingBox(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9);

	// Token: 0x060007C5 RID: 1989
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitBlendColor")]
	public static extern void GLAdapter_Process_glBlitBlendColor(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x060007C6 RID: 1990
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitBlendEquationSeparate")]
	public static extern void GLAdapter_Process_glBlitBlendEquationSeparate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060007C7 RID: 1991
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitBlendFuncSeparate")]
	public static extern void GLAdapter_Process_glBlitBlendFuncSeparate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060007C8 RID: 1992
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBlitRotation")]
	public static extern void GLAdapter_Process_glBlitRotation(HandleRef jarg1, uint jarg2);

	// Token: 0x060007C9 RID: 1993
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindSharedBufferQCOM")]
	public static extern void GLAdapter_Process_glBindSharedBufferQCOM(HandleRef jarg1, uint jarg2, int jarg3, int jarg4);

	// Token: 0x060007CA RID: 1994
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCreateSharedBufferQCOM")]
	public static extern void GLAdapter_Process_glCreateSharedBufferQCOM(HandleRef jarg1, int jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060007CB RID: 1995
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDestroySharedBufferQCOM")]
	public static extern void GLAdapter_Process_glDestroySharedBufferQCOM(HandleRef jarg1, int jarg2);

	// Token: 0x060007CC RID: 1996
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTextureBarrier")]
	public static extern void GLAdapter_Process_glTextureBarrier(HandleRef jarg1);

	// Token: 0x060007CD RID: 1997
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferFoveationConfigQCOM")]
	public static extern void GLAdapter_Process_glFramebufferFoveationConfigQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x060007CE RID: 1998
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferFoveationParametersQCOM")]
	public static extern void GLAdapter_Process_glFramebufferFoveationParametersQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9);

	// Token: 0x060007CF RID: 1999
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBufferStorageExternalEXT")]
	public static extern void GLAdapter_Process_glBufferStorageExternalEXT(HandleRef jarg1, uint jarg2, int jarg3, int jarg4, uint jarg5, uint jarg6);

	// Token: 0x060007D0 RID: 2000
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferFetchBarrierQCOM")]
	public static extern void GLAdapter_Process_glFramebufferFetchBarrierQCOM(HandleRef jarg1);

	// Token: 0x060007D1 RID: 2001
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glCreateMemoryObjectsEXT")]
	public static extern void GLAdapter_Process_glCreateMemoryObjectsEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060007D2 RID: 2002
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteMemoryObjectsEXT")]
	public static extern void GLAdapter_Process_glDeleteMemoryObjectsEXT(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060007D3 RID: 2003
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsMemoryObjectEXT")]
	public static extern void GLAdapter_Process_glIsMemoryObjectEXT(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060007D4 RID: 2004
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glMemoryObjectParameterivEXT")]
	public static extern void GLAdapter_Process_glMemoryObjectParameterivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007D5 RID: 2005
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetMemoryObjectParameterivEXT")]
	public static extern void GLAdapter_Process_glGetMemoryObjectParameterivEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007D6 RID: 2006
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorageMem2DEXT")]
	public static extern void GLAdapter_Process_glTexStorageMem2DEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, uint jarg7, ulong jarg8);

	// Token: 0x060007D7 RID: 2007
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorageMem2DMultisampleEXT")]
	public static extern void GLAdapter_Process_glTexStorageMem2DMultisampleEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, ulong jarg9);

	// Token: 0x060007D8 RID: 2008
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorageMem3DEXT")]
	public static extern void GLAdapter_Process_glTexStorageMem3DEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, uint jarg8, ulong jarg9);

	// Token: 0x060007D9 RID: 2009
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexStorageMem3DMultisampleEXT")]
	public static extern void GLAdapter_Process_glTexStorageMem3DMultisampleEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8, uint jarg9, ulong jarg10);

	// Token: 0x060007DA RID: 2010
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBufferStorageMemEXT")]
	public static extern void GLAdapter_Process_glBufferStorageMemEXT(HandleRef jarg1, uint jarg2, int jarg3, uint jarg4, ulong jarg5);

	// Token: 0x060007DB RID: 2011
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGenSemaphoresKHR")]
	public static extern void GLAdapter_Process_glGenSemaphoresKHR(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060007DC RID: 2012
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glDeleteSemaphoresKHR")]
	public static extern void GLAdapter_Process_glDeleteSemaphoresKHR(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x060007DD RID: 2013
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glIsSemaphoreKHR")]
	public static extern void GLAdapter_Process_glIsSemaphoreKHR(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x060007DE RID: 2014
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glWaitSemaphoreKHR")]
	public static extern void GLAdapter_Process_glWaitSemaphoreKHR(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060007DF RID: 2015
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glSignalSemaphoreKHR")]
	public static extern void GLAdapter_Process_glSignalSemaphoreKHR(HandleRef jarg1, HandleRef jarg2, uint jarg3);

	// Token: 0x060007E0 RID: 2016
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glImportMemoryFdEXT")]
	public static extern void GLAdapter_Process_glImportMemoryFdEXT(HandleRef jarg1, uint jarg2, ulong jarg3, uint jarg4, int jarg5);

	// Token: 0x060007E1 RID: 2017
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glImportSemaphoreFdEXT")]
	public static extern void GLAdapter_Process_glImportSemaphoreFdEXT(HandleRef jarg1, uint jarg2, uint jarg3, int jarg4);

	// Token: 0x060007E2 RID: 2018
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUnsignedBytevEXT")]
	public static extern void GLAdapter_Process_glGetUnsignedBytevEXT(HandleRef jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x060007E3 RID: 2019
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetUnsignedBytei_vEXT")]
	public static extern void GLAdapter_Process_glGetUnsignedBytei_vEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007E4 RID: 2020
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTextureFoveationParametersQCOM")]
	public static extern void GLAdapter_Process_glTextureFoveationParametersQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9);

	// Token: 0x060007E5 RID: 2021
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindFragDataLocationIndexedEXT")]
	public static extern void GLAdapter_Process_glBindFragDataLocationIndexedEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060007E6 RID: 2022
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glBindFragDataLocationEXT")]
	public static extern void GLAdapter_Process_glBindFragDataLocationEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007E7 RID: 2023
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetProgramResourceLocationIndexEXT")]
	public static extern void GLAdapter_Process_glGetProgramResourceLocationIndexEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, HandleRef jarg5);

	// Token: 0x060007E8 RID: 2024
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFragDataIndexEXT")]
	public static extern void GLAdapter_Process_glGetFragDataIndexEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007E9 RID: 2025
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glShadingRateQCOM")]
	public static extern void GLAdapter_Process_glShadingRateQCOM(HandleRef jarg1, uint jarg2);

	// Token: 0x060007EA RID: 2026
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glExtrapolateTex2DQCOM")]
	public static extern void GLAdapter_Process_glExtrapolateTex2DQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, float jarg5);

	// Token: 0x060007EB RID: 2027
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTextureViewOES")]
	public static extern void GLAdapter_Process_glTextureViewOES(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8, uint jarg9);

	// Token: 0x060007EC RID: 2028
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexEstimateMotionQCOM")]
	public static extern void GLAdapter_Process_glTexEstimateMotionQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060007ED RID: 2029
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glTexEstimateMotionRegionsQCOM")]
	public static extern void GLAdapter_Process_glTexEstimateMotionRegionsQCOM(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060007EE RID: 2030
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glEGLImageTargetTexStorageEXT")]
	public static extern void GLAdapter_Process_glEGLImageTargetTexStorageEXT(HandleRef jarg1, uint jarg2, uint jarg3, HandleRef jarg4);

	// Token: 0x060007EF RID: 2031
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glPolygonOffsetClampEXT")]
	public static extern void GLAdapter_Process_glPolygonOffsetClampEXT(HandleRef jarg1, float jarg2, float jarg3, float jarg4);

	// Token: 0x060007F0 RID: 2032
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glGetFragmentShadingRatesEXT")]
	public static extern void GLAdapter_Process_glGetFragmentShadingRatesEXT(HandleRef jarg1, int jarg2, int jarg3, HandleRef jarg4, HandleRef jarg5);

	// Token: 0x060007F1 RID: 2033
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glShadingRateEXT")]
	public static extern void GLAdapter_Process_glShadingRateEXT(HandleRef jarg1, uint jarg2);

	// Token: 0x060007F2 RID: 2034
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glShadingRateCombinerOpsEXT")]
	public static extern void GLAdapter_Process_glShadingRateCombinerOpsEXT(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x060007F3 RID: 2035
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_Process_glFramebufferShadingRateEXT")]
	public static extern void GLAdapter_Process_glFramebufferShadingRateEXT(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, int jarg5, int jarg6, int jarg7, int jarg8);

	// Token: 0x060007F4 RID: 2036
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_director_connect")]
	public static extern void GLAdapter_director_connect(HandleRef jarg1, GLAdapter.SwigDelegateGLAdapter_0 delegate0, GLAdapter.SwigDelegateGLAdapter_1 delegate1, GLAdapter.SwigDelegateGLAdapter_2 delegate2, GLAdapter.SwigDelegateGLAdapter_3 delegate3, GLAdapter.SwigDelegateGLAdapter_4 delegate4, GLAdapter.SwigDelegateGLAdapter_5 delegate5, GLAdapter.SwigDelegateGLAdapter_6 delegate6, GLAdapter.SwigDelegateGLAdapter_7 delegate7, GLAdapter.SwigDelegateGLAdapter_8 delegate8, GLAdapter.SwigDelegateGLAdapter_9 delegate9, GLAdapter.SwigDelegateGLAdapter_10 delegate10, GLAdapter.SwigDelegateGLAdapter_11 delegate11, GLAdapter.SwigDelegateGLAdapter_12 delegate12, GLAdapter.SwigDelegateGLAdapter_13 delegate13, GLAdapter.SwigDelegateGLAdapter_14 delegate14, GLAdapter.SwigDelegateGLAdapter_15 delegate15, GLAdapter.SwigDelegateGLAdapter_16 delegate16, GLAdapter.SwigDelegateGLAdapter_17 delegate17, GLAdapter.SwigDelegateGLAdapter_18 delegate18, GLAdapter.SwigDelegateGLAdapter_19 delegate19, GLAdapter.SwigDelegateGLAdapter_20 delegate20, GLAdapter.SwigDelegateGLAdapter_21 delegate21, GLAdapter.SwigDelegateGLAdapter_22 delegate22, GLAdapter.SwigDelegateGLAdapter_23 delegate23, GLAdapter.SwigDelegateGLAdapter_24 delegate24, GLAdapter.SwigDelegateGLAdapter_25 delegate25, GLAdapter.SwigDelegateGLAdapter_26 delegate26, GLAdapter.SwigDelegateGLAdapter_27 delegate27, GLAdapter.SwigDelegateGLAdapter_28 delegate28, GLAdapter.SwigDelegateGLAdapter_29 delegate29, GLAdapter.SwigDelegateGLAdapter_30 delegate30, GLAdapter.SwigDelegateGLAdapter_31 delegate31, GLAdapter.SwigDelegateGLAdapter_32 delegate32, GLAdapter.SwigDelegateGLAdapter_33 delegate33, GLAdapter.SwigDelegateGLAdapter_34 delegate34, GLAdapter.SwigDelegateGLAdapter_35 delegate35, GLAdapter.SwigDelegateGLAdapter_36 delegate36, GLAdapter.SwigDelegateGLAdapter_37 delegate37, GLAdapter.SwigDelegateGLAdapter_38 delegate38, GLAdapter.SwigDelegateGLAdapter_39 delegate39, GLAdapter.SwigDelegateGLAdapter_40 delegate40, GLAdapter.SwigDelegateGLAdapter_41 delegate41, GLAdapter.SwigDelegateGLAdapter_42 delegate42, GLAdapter.SwigDelegateGLAdapter_43 delegate43, GLAdapter.SwigDelegateGLAdapter_44 delegate44, GLAdapter.SwigDelegateGLAdapter_45 delegate45, GLAdapter.SwigDelegateGLAdapter_46 delegate46, GLAdapter.SwigDelegateGLAdapter_47 delegate47, GLAdapter.SwigDelegateGLAdapter_48 delegate48, GLAdapter.SwigDelegateGLAdapter_49 delegate49, GLAdapter.SwigDelegateGLAdapter_50 delegate50, GLAdapter.SwigDelegateGLAdapter_51 delegate51, GLAdapter.SwigDelegateGLAdapter_52 delegate52, GLAdapter.SwigDelegateGLAdapter_53 delegate53, GLAdapter.SwigDelegateGLAdapter_54 delegate54, GLAdapter.SwigDelegateGLAdapter_55 delegate55, GLAdapter.SwigDelegateGLAdapter_56 delegate56, GLAdapter.SwigDelegateGLAdapter_57 delegate57, GLAdapter.SwigDelegateGLAdapter_58 delegate58, GLAdapter.SwigDelegateGLAdapter_59 delegate59, GLAdapter.SwigDelegateGLAdapter_60 delegate60, GLAdapter.SwigDelegateGLAdapter_61 delegate61, GLAdapter.SwigDelegateGLAdapter_62 delegate62, GLAdapter.SwigDelegateGLAdapter_63 delegate63, GLAdapter.SwigDelegateGLAdapter_64 delegate64, GLAdapter.SwigDelegateGLAdapter_65 delegate65, GLAdapter.SwigDelegateGLAdapter_66 delegate66, GLAdapter.SwigDelegateGLAdapter_67 delegate67, GLAdapter.SwigDelegateGLAdapter_68 delegate68, GLAdapter.SwigDelegateGLAdapter_69 delegate69, GLAdapter.SwigDelegateGLAdapter_70 delegate70, GLAdapter.SwigDelegateGLAdapter_71 delegate71, GLAdapter.SwigDelegateGLAdapter_72 delegate72, GLAdapter.SwigDelegateGLAdapter_73 delegate73, GLAdapter.SwigDelegateGLAdapter_74 delegate74, GLAdapter.SwigDelegateGLAdapter_75 delegate75, GLAdapter.SwigDelegateGLAdapter_76 delegate76, GLAdapter.SwigDelegateGLAdapter_77 delegate77, GLAdapter.SwigDelegateGLAdapter_78 delegate78, GLAdapter.SwigDelegateGLAdapter_79 delegate79, GLAdapter.SwigDelegateGLAdapter_80 delegate80, GLAdapter.SwigDelegateGLAdapter_81 delegate81, GLAdapter.SwigDelegateGLAdapter_82 delegate82, GLAdapter.SwigDelegateGLAdapter_83 delegate83, GLAdapter.SwigDelegateGLAdapter_84 delegate84, GLAdapter.SwigDelegateGLAdapter_85 delegate85, GLAdapter.SwigDelegateGLAdapter_86 delegate86, GLAdapter.SwigDelegateGLAdapter_87 delegate87, GLAdapter.SwigDelegateGLAdapter_88 delegate88, GLAdapter.SwigDelegateGLAdapter_89 delegate89, GLAdapter.SwigDelegateGLAdapter_90 delegate90, GLAdapter.SwigDelegateGLAdapter_91 delegate91, GLAdapter.SwigDelegateGLAdapter_92 delegate92, GLAdapter.SwigDelegateGLAdapter_93 delegate93, GLAdapter.SwigDelegateGLAdapter_94 delegate94, GLAdapter.SwigDelegateGLAdapter_95 delegate95, GLAdapter.SwigDelegateGLAdapter_96 delegate96, GLAdapter.SwigDelegateGLAdapter_97 delegate97, GLAdapter.SwigDelegateGLAdapter_98 delegate98, GLAdapter.SwigDelegateGLAdapter_99 delegate99, GLAdapter.SwigDelegateGLAdapter_100 delegate100, GLAdapter.SwigDelegateGLAdapter_101 delegate101, GLAdapter.SwigDelegateGLAdapter_102 delegate102, GLAdapter.SwigDelegateGLAdapter_103 delegate103, GLAdapter.SwigDelegateGLAdapter_104 delegate104, GLAdapter.SwigDelegateGLAdapter_105 delegate105, GLAdapter.SwigDelegateGLAdapter_106 delegate106, GLAdapter.SwigDelegateGLAdapter_107 delegate107, GLAdapter.SwigDelegateGLAdapter_108 delegate108, GLAdapter.SwigDelegateGLAdapter_109 delegate109, GLAdapter.SwigDelegateGLAdapter_110 delegate110, GLAdapter.SwigDelegateGLAdapter_111 delegate111, GLAdapter.SwigDelegateGLAdapter_112 delegate112, GLAdapter.SwigDelegateGLAdapter_113 delegate113, GLAdapter.SwigDelegateGLAdapter_114 delegate114, GLAdapter.SwigDelegateGLAdapter_115 delegate115, GLAdapter.SwigDelegateGLAdapter_116 delegate116, GLAdapter.SwigDelegateGLAdapter_117 delegate117, GLAdapter.SwigDelegateGLAdapter_118 delegate118, GLAdapter.SwigDelegateGLAdapter_119 delegate119, GLAdapter.SwigDelegateGLAdapter_120 delegate120, GLAdapter.SwigDelegateGLAdapter_121 delegate121, GLAdapter.SwigDelegateGLAdapter_122 delegate122, GLAdapter.SwigDelegateGLAdapter_123 delegate123, GLAdapter.SwigDelegateGLAdapter_124 delegate124, GLAdapter.SwigDelegateGLAdapter_125 delegate125, GLAdapter.SwigDelegateGLAdapter_126 delegate126, GLAdapter.SwigDelegateGLAdapter_127 delegate127, GLAdapter.SwigDelegateGLAdapter_128 delegate128, GLAdapter.SwigDelegateGLAdapter_129 delegate129, GLAdapter.SwigDelegateGLAdapter_130 delegate130, GLAdapter.SwigDelegateGLAdapter_131 delegate131, GLAdapter.SwigDelegateGLAdapter_132 delegate132, GLAdapter.SwigDelegateGLAdapter_133 delegate133, GLAdapter.SwigDelegateGLAdapter_134 delegate134, GLAdapter.SwigDelegateGLAdapter_135 delegate135, GLAdapter.SwigDelegateGLAdapter_136 delegate136, GLAdapter.SwigDelegateGLAdapter_137 delegate137, GLAdapter.SwigDelegateGLAdapter_138 delegate138, GLAdapter.SwigDelegateGLAdapter_139 delegate139, GLAdapter.SwigDelegateGLAdapter_140 delegate140, GLAdapter.SwigDelegateGLAdapter_141 delegate141, GLAdapter.SwigDelegateGLAdapter_142 delegate142, GLAdapter.SwigDelegateGLAdapter_143 delegate143, GLAdapter.SwigDelegateGLAdapter_144 delegate144, GLAdapter.SwigDelegateGLAdapter_145 delegate145, GLAdapter.SwigDelegateGLAdapter_146 delegate146, GLAdapter.SwigDelegateGLAdapter_147 delegate147, GLAdapter.SwigDelegateGLAdapter_148 delegate148, GLAdapter.SwigDelegateGLAdapter_149 delegate149, GLAdapter.SwigDelegateGLAdapter_150 delegate150, GLAdapter.SwigDelegateGLAdapter_151 delegate151, GLAdapter.SwigDelegateGLAdapter_152 delegate152, GLAdapter.SwigDelegateGLAdapter_153 delegate153, GLAdapter.SwigDelegateGLAdapter_154 delegate154, GLAdapter.SwigDelegateGLAdapter_155 delegate155, GLAdapter.SwigDelegateGLAdapter_156 delegate156, GLAdapter.SwigDelegateGLAdapter_157 delegate157, GLAdapter.SwigDelegateGLAdapter_158 delegate158, GLAdapter.SwigDelegateGLAdapter_159 delegate159, GLAdapter.SwigDelegateGLAdapter_160 delegate160, GLAdapter.SwigDelegateGLAdapter_161 delegate161, GLAdapter.SwigDelegateGLAdapter_162 delegate162, GLAdapter.SwigDelegateGLAdapter_163 delegate163, GLAdapter.SwigDelegateGLAdapter_164 delegate164, GLAdapter.SwigDelegateGLAdapter_165 delegate165, GLAdapter.SwigDelegateGLAdapter_166 delegate166, GLAdapter.SwigDelegateGLAdapter_167 delegate167, GLAdapter.SwigDelegateGLAdapter_168 delegate168, GLAdapter.SwigDelegateGLAdapter_169 delegate169, GLAdapter.SwigDelegateGLAdapter_170 delegate170, GLAdapter.SwigDelegateGLAdapter_171 delegate171, GLAdapter.SwigDelegateGLAdapter_172 delegate172, GLAdapter.SwigDelegateGLAdapter_173 delegate173, GLAdapter.SwigDelegateGLAdapter_174 delegate174, GLAdapter.SwigDelegateGLAdapter_175 delegate175, GLAdapter.SwigDelegateGLAdapter_176 delegate176, GLAdapter.SwigDelegateGLAdapter_177 delegate177, GLAdapter.SwigDelegateGLAdapter_178 delegate178, GLAdapter.SwigDelegateGLAdapter_179 delegate179, GLAdapter.SwigDelegateGLAdapter_180 delegate180, GLAdapter.SwigDelegateGLAdapter_181 delegate181, GLAdapter.SwigDelegateGLAdapter_182 delegate182, GLAdapter.SwigDelegateGLAdapter_183 delegate183, GLAdapter.SwigDelegateGLAdapter_184 delegate184, GLAdapter.SwigDelegateGLAdapter_185 delegate185, GLAdapter.SwigDelegateGLAdapter_186 delegate186, GLAdapter.SwigDelegateGLAdapter_187 delegate187, GLAdapter.SwigDelegateGLAdapter_188 delegate188, GLAdapter.SwigDelegateGLAdapter_189 delegate189, GLAdapter.SwigDelegateGLAdapter_190 delegate190, GLAdapter.SwigDelegateGLAdapter_191 delegate191, GLAdapter.SwigDelegateGLAdapter_192 delegate192, GLAdapter.SwigDelegateGLAdapter_193 delegate193, GLAdapter.SwigDelegateGLAdapter_194 delegate194, GLAdapter.SwigDelegateGLAdapter_195 delegate195, GLAdapter.SwigDelegateGLAdapter_196 delegate196, GLAdapter.SwigDelegateGLAdapter_197 delegate197, GLAdapter.SwigDelegateGLAdapter_198 delegate198, GLAdapter.SwigDelegateGLAdapter_199 delegate199, GLAdapter.SwigDelegateGLAdapter_200 delegate200, GLAdapter.SwigDelegateGLAdapter_201 delegate201, GLAdapter.SwigDelegateGLAdapter_202 delegate202, GLAdapter.SwigDelegateGLAdapter_203 delegate203, GLAdapter.SwigDelegateGLAdapter_204 delegate204, GLAdapter.SwigDelegateGLAdapter_205 delegate205, GLAdapter.SwigDelegateGLAdapter_206 delegate206, GLAdapter.SwigDelegateGLAdapter_207 delegate207, GLAdapter.SwigDelegateGLAdapter_208 delegate208, GLAdapter.SwigDelegateGLAdapter_209 delegate209, GLAdapter.SwigDelegateGLAdapter_210 delegate210, GLAdapter.SwigDelegateGLAdapter_211 delegate211, GLAdapter.SwigDelegateGLAdapter_212 delegate212, GLAdapter.SwigDelegateGLAdapter_213 delegate213, GLAdapter.SwigDelegateGLAdapter_214 delegate214, GLAdapter.SwigDelegateGLAdapter_215 delegate215, GLAdapter.SwigDelegateGLAdapter_216 delegate216, GLAdapter.SwigDelegateGLAdapter_217 delegate217, GLAdapter.SwigDelegateGLAdapter_218 delegate218, GLAdapter.SwigDelegateGLAdapter_219 delegate219, GLAdapter.SwigDelegateGLAdapter_220 delegate220, GLAdapter.SwigDelegateGLAdapter_221 delegate221, GLAdapter.SwigDelegateGLAdapter_222 delegate222, GLAdapter.SwigDelegateGLAdapter_223 delegate223, GLAdapter.SwigDelegateGLAdapter_224 delegate224, GLAdapter.SwigDelegateGLAdapter_225 delegate225, GLAdapter.SwigDelegateGLAdapter_226 delegate226, GLAdapter.SwigDelegateGLAdapter_227 delegate227, GLAdapter.SwigDelegateGLAdapter_228 delegate228, GLAdapter.SwigDelegateGLAdapter_229 delegate229, GLAdapter.SwigDelegateGLAdapter_230 delegate230, GLAdapter.SwigDelegateGLAdapter_231 delegate231, GLAdapter.SwigDelegateGLAdapter_232 delegate232, GLAdapter.SwigDelegateGLAdapter_233 delegate233, GLAdapter.SwigDelegateGLAdapter_234 delegate234, GLAdapter.SwigDelegateGLAdapter_235 delegate235, GLAdapter.SwigDelegateGLAdapter_236 delegate236, GLAdapter.SwigDelegateGLAdapter_237 delegate237, GLAdapter.SwigDelegateGLAdapter_238 delegate238, GLAdapter.SwigDelegateGLAdapter_239 delegate239, GLAdapter.SwigDelegateGLAdapter_240 delegate240, GLAdapter.SwigDelegateGLAdapter_241 delegate241, GLAdapter.SwigDelegateGLAdapter_242 delegate242, GLAdapter.SwigDelegateGLAdapter_243 delegate243, GLAdapter.SwigDelegateGLAdapter_244 delegate244, GLAdapter.SwigDelegateGLAdapter_245 delegate245, GLAdapter.SwigDelegateGLAdapter_246 delegate246, GLAdapter.SwigDelegateGLAdapter_247 delegate247, GLAdapter.SwigDelegateGLAdapter_248 delegate248, GLAdapter.SwigDelegateGLAdapter_249 delegate249, GLAdapter.SwigDelegateGLAdapter_250 delegate250, GLAdapter.SwigDelegateGLAdapter_251 delegate251, GLAdapter.SwigDelegateGLAdapter_252 delegate252, GLAdapter.SwigDelegateGLAdapter_253 delegate253, GLAdapter.SwigDelegateGLAdapter_254 delegate254, GLAdapter.SwigDelegateGLAdapter_255 delegate255, GLAdapter.SwigDelegateGLAdapter_256 delegate256, GLAdapter.SwigDelegateGLAdapter_257 delegate257, GLAdapter.SwigDelegateGLAdapter_258 delegate258, GLAdapter.SwigDelegateGLAdapter_259 delegate259, GLAdapter.SwigDelegateGLAdapter_260 delegate260, GLAdapter.SwigDelegateGLAdapter_261 delegate261, GLAdapter.SwigDelegateGLAdapter_262 delegate262, GLAdapter.SwigDelegateGLAdapter_263 delegate263, GLAdapter.SwigDelegateGLAdapter_264 delegate264, GLAdapter.SwigDelegateGLAdapter_265 delegate265, GLAdapter.SwigDelegateGLAdapter_266 delegate266, GLAdapter.SwigDelegateGLAdapter_267 delegate267, GLAdapter.SwigDelegateGLAdapter_268 delegate268, GLAdapter.SwigDelegateGLAdapter_269 delegate269, GLAdapter.SwigDelegateGLAdapter_270 delegate270, GLAdapter.SwigDelegateGLAdapter_271 delegate271, GLAdapter.SwigDelegateGLAdapter_272 delegate272, GLAdapter.SwigDelegateGLAdapter_273 delegate273, GLAdapter.SwigDelegateGLAdapter_274 delegate274, GLAdapter.SwigDelegateGLAdapter_275 delegate275, GLAdapter.SwigDelegateGLAdapter_276 delegate276, GLAdapter.SwigDelegateGLAdapter_277 delegate277, GLAdapter.SwigDelegateGLAdapter_278 delegate278, GLAdapter.SwigDelegateGLAdapter_279 delegate279, GLAdapter.SwigDelegateGLAdapter_280 delegate280, GLAdapter.SwigDelegateGLAdapter_281 delegate281, GLAdapter.SwigDelegateGLAdapter_282 delegate282, GLAdapter.SwigDelegateGLAdapter_283 delegate283, GLAdapter.SwigDelegateGLAdapter_284 delegate284, GLAdapter.SwigDelegateGLAdapter_285 delegate285, GLAdapter.SwigDelegateGLAdapter_286 delegate286, GLAdapter.SwigDelegateGLAdapter_287 delegate287, GLAdapter.SwigDelegateGLAdapter_288 delegate288, GLAdapter.SwigDelegateGLAdapter_289 delegate289, GLAdapter.SwigDelegateGLAdapter_290 delegate290, GLAdapter.SwigDelegateGLAdapter_291 delegate291, GLAdapter.SwigDelegateGLAdapter_292 delegate292, GLAdapter.SwigDelegateGLAdapter_293 delegate293, GLAdapter.SwigDelegateGLAdapter_294 delegate294, GLAdapter.SwigDelegateGLAdapter_295 delegate295, GLAdapter.SwigDelegateGLAdapter_296 delegate296, GLAdapter.SwigDelegateGLAdapter_297 delegate297, GLAdapter.SwigDelegateGLAdapter_298 delegate298, GLAdapter.SwigDelegateGLAdapter_299 delegate299, GLAdapter.SwigDelegateGLAdapter_300 delegate300, GLAdapter.SwigDelegateGLAdapter_301 delegate301, GLAdapter.SwigDelegateGLAdapter_302 delegate302, GLAdapter.SwigDelegateGLAdapter_303 delegate303, GLAdapter.SwigDelegateGLAdapter_304 delegate304, GLAdapter.SwigDelegateGLAdapter_305 delegate305, GLAdapter.SwigDelegateGLAdapter_306 delegate306, GLAdapter.SwigDelegateGLAdapter_307 delegate307, GLAdapter.SwigDelegateGLAdapter_308 delegate308, GLAdapter.SwigDelegateGLAdapter_309 delegate309, GLAdapter.SwigDelegateGLAdapter_310 delegate310, GLAdapter.SwigDelegateGLAdapter_311 delegate311, GLAdapter.SwigDelegateGLAdapter_312 delegate312, GLAdapter.SwigDelegateGLAdapter_313 delegate313, GLAdapter.SwigDelegateGLAdapter_314 delegate314, GLAdapter.SwigDelegateGLAdapter_315 delegate315, GLAdapter.SwigDelegateGLAdapter_316 delegate316, GLAdapter.SwigDelegateGLAdapter_317 delegate317, GLAdapter.SwigDelegateGLAdapter_318 delegate318, GLAdapter.SwigDelegateGLAdapter_319 delegate319, GLAdapter.SwigDelegateGLAdapter_320 delegate320, GLAdapter.SwigDelegateGLAdapter_321 delegate321, GLAdapter.SwigDelegateGLAdapter_322 delegate322, GLAdapter.SwigDelegateGLAdapter_323 delegate323, GLAdapter.SwigDelegateGLAdapter_324 delegate324, GLAdapter.SwigDelegateGLAdapter_325 delegate325, GLAdapter.SwigDelegateGLAdapter_326 delegate326, GLAdapter.SwigDelegateGLAdapter_327 delegate327, GLAdapter.SwigDelegateGLAdapter_328 delegate328, GLAdapter.SwigDelegateGLAdapter_329 delegate329, GLAdapter.SwigDelegateGLAdapter_330 delegate330, GLAdapter.SwigDelegateGLAdapter_331 delegate331, GLAdapter.SwigDelegateGLAdapter_332 delegate332, GLAdapter.SwigDelegateGLAdapter_333 delegate333, GLAdapter.SwigDelegateGLAdapter_334 delegate334, GLAdapter.SwigDelegateGLAdapter_335 delegate335, GLAdapter.SwigDelegateGLAdapter_336 delegate336, GLAdapter.SwigDelegateGLAdapter_337 delegate337, GLAdapter.SwigDelegateGLAdapter_338 delegate338, GLAdapter.SwigDelegateGLAdapter_339 delegate339, GLAdapter.SwigDelegateGLAdapter_340 delegate340, GLAdapter.SwigDelegateGLAdapter_341 delegate341, GLAdapter.SwigDelegateGLAdapter_342 delegate342, GLAdapter.SwigDelegateGLAdapter_343 delegate343, GLAdapter.SwigDelegateGLAdapter_344 delegate344, GLAdapter.SwigDelegateGLAdapter_345 delegate345, GLAdapter.SwigDelegateGLAdapter_346 delegate346, GLAdapter.SwigDelegateGLAdapter_347 delegate347, GLAdapter.SwigDelegateGLAdapter_348 delegate348, GLAdapter.SwigDelegateGLAdapter_349 delegate349, GLAdapter.SwigDelegateGLAdapter_350 delegate350, GLAdapter.SwigDelegateGLAdapter_351 delegate351, GLAdapter.SwigDelegateGLAdapter_352 delegate352, GLAdapter.SwigDelegateGLAdapter_353 delegate353, GLAdapter.SwigDelegateGLAdapter_354 delegate354, GLAdapter.SwigDelegateGLAdapter_355 delegate355, GLAdapter.SwigDelegateGLAdapter_356 delegate356, GLAdapter.SwigDelegateGLAdapter_357 delegate357, GLAdapter.SwigDelegateGLAdapter_358 delegate358, GLAdapter.SwigDelegateGLAdapter_359 delegate359, GLAdapter.SwigDelegateGLAdapter_360 delegate360, GLAdapter.SwigDelegateGLAdapter_361 delegate361, GLAdapter.SwigDelegateGLAdapter_362 delegate362, GLAdapter.SwigDelegateGLAdapter_363 delegate363, GLAdapter.SwigDelegateGLAdapter_364 delegate364, GLAdapter.SwigDelegateGLAdapter_365 delegate365, GLAdapter.SwigDelegateGLAdapter_366 delegate366, GLAdapter.SwigDelegateGLAdapter_367 delegate367, GLAdapter.SwigDelegateGLAdapter_368 delegate368, GLAdapter.SwigDelegateGLAdapter_369 delegate369, GLAdapter.SwigDelegateGLAdapter_370 delegate370, GLAdapter.SwigDelegateGLAdapter_371 delegate371, GLAdapter.SwigDelegateGLAdapter_372 delegate372, GLAdapter.SwigDelegateGLAdapter_373 delegate373, GLAdapter.SwigDelegateGLAdapter_374 delegate374, GLAdapter.SwigDelegateGLAdapter_375 delegate375, GLAdapter.SwigDelegateGLAdapter_376 delegate376, GLAdapter.SwigDelegateGLAdapter_377 delegate377, GLAdapter.SwigDelegateGLAdapter_378 delegate378, GLAdapter.SwigDelegateGLAdapter_379 delegate379, GLAdapter.SwigDelegateGLAdapter_380 delegate380, GLAdapter.SwigDelegateGLAdapter_381 delegate381, GLAdapter.SwigDelegateGLAdapter_382 delegate382, GLAdapter.SwigDelegateGLAdapter_383 delegate383, GLAdapter.SwigDelegateGLAdapter_384 delegate384, GLAdapter.SwigDelegateGLAdapter_385 delegate385, GLAdapter.SwigDelegateGLAdapter_386 delegate386, GLAdapter.SwigDelegateGLAdapter_387 delegate387, GLAdapter.SwigDelegateGLAdapter_388 delegate388, GLAdapter.SwigDelegateGLAdapter_389 delegate389, GLAdapter.SwigDelegateGLAdapter_390 delegate390, GLAdapter.SwigDelegateGLAdapter_391 delegate391, GLAdapter.SwigDelegateGLAdapter_392 delegate392, GLAdapter.SwigDelegateGLAdapter_393 delegate393, GLAdapter.SwigDelegateGLAdapter_394 delegate394, GLAdapter.SwigDelegateGLAdapter_395 delegate395, GLAdapter.SwigDelegateGLAdapter_396 delegate396, GLAdapter.SwigDelegateGLAdapter_397 delegate397, GLAdapter.SwigDelegateGLAdapter_398 delegate398, GLAdapter.SwigDelegateGLAdapter_399 delegate399, GLAdapter.SwigDelegateGLAdapter_400 delegate400, GLAdapter.SwigDelegateGLAdapter_401 delegate401, GLAdapter.SwigDelegateGLAdapter_402 delegate402, GLAdapter.SwigDelegateGLAdapter_403 delegate403, GLAdapter.SwigDelegateGLAdapter_404 delegate404, GLAdapter.SwigDelegateGLAdapter_405 delegate405, GLAdapter.SwigDelegateGLAdapter_406 delegate406, GLAdapter.SwigDelegateGLAdapter_407 delegate407, GLAdapter.SwigDelegateGLAdapter_408 delegate408, GLAdapter.SwigDelegateGLAdapter_409 delegate409, GLAdapter.SwigDelegateGLAdapter_410 delegate410, GLAdapter.SwigDelegateGLAdapter_411 delegate411, GLAdapter.SwigDelegateGLAdapter_412 delegate412, GLAdapter.SwigDelegateGLAdapter_413 delegate413, GLAdapter.SwigDelegateGLAdapter_414 delegate414, GLAdapter.SwigDelegateGLAdapter_415 delegate415, GLAdapter.SwigDelegateGLAdapter_416 delegate416, GLAdapter.SwigDelegateGLAdapter_417 delegate417, GLAdapter.SwigDelegateGLAdapter_418 delegate418, GLAdapter.SwigDelegateGLAdapter_419 delegate419, GLAdapter.SwigDelegateGLAdapter_420 delegate420, GLAdapter.SwigDelegateGLAdapter_421 delegate421, GLAdapter.SwigDelegateGLAdapter_422 delegate422, GLAdapter.SwigDelegateGLAdapter_423 delegate423, GLAdapter.SwigDelegateGLAdapter_424 delegate424, GLAdapter.SwigDelegateGLAdapter_425 delegate425, GLAdapter.SwigDelegateGLAdapter_426 delegate426, GLAdapter.SwigDelegateGLAdapter_427 delegate427, GLAdapter.SwigDelegateGLAdapter_428 delegate428, GLAdapter.SwigDelegateGLAdapter_429 delegate429, GLAdapter.SwigDelegateGLAdapter_430 delegate430, GLAdapter.SwigDelegateGLAdapter_431 delegate431, GLAdapter.SwigDelegateGLAdapter_432 delegate432, GLAdapter.SwigDelegateGLAdapter_433 delegate433, GLAdapter.SwigDelegateGLAdapter_434 delegate434, GLAdapter.SwigDelegateGLAdapter_435 delegate435, GLAdapter.SwigDelegateGLAdapter_436 delegate436, GLAdapter.SwigDelegateGLAdapter_437 delegate437, GLAdapter.SwigDelegateGLAdapter_438 delegate438, GLAdapter.SwigDelegateGLAdapter_439 delegate439, GLAdapter.SwigDelegateGLAdapter_440 delegate440, GLAdapter.SwigDelegateGLAdapter_441 delegate441, GLAdapter.SwigDelegateGLAdapter_442 delegate442, GLAdapter.SwigDelegateGLAdapter_443 delegate443, GLAdapter.SwigDelegateGLAdapter_444 delegate444, GLAdapter.SwigDelegateGLAdapter_445 delegate445, GLAdapter.SwigDelegateGLAdapter_446 delegate446, GLAdapter.SwigDelegateGLAdapter_447 delegate447, GLAdapter.SwigDelegateGLAdapter_448 delegate448, GLAdapter.SwigDelegateGLAdapter_449 delegate449, GLAdapter.SwigDelegateGLAdapter_450 delegate450, GLAdapter.SwigDelegateGLAdapter_451 delegate451, GLAdapter.SwigDelegateGLAdapter_452 delegate452, GLAdapter.SwigDelegateGLAdapter_453 delegate453, GLAdapter.SwigDelegateGLAdapter_454 delegate454, GLAdapter.SwigDelegateGLAdapter_455 delegate455, GLAdapter.SwigDelegateGLAdapter_456 delegate456, GLAdapter.SwigDelegateGLAdapter_457 delegate457, GLAdapter.SwigDelegateGLAdapter_458 delegate458, GLAdapter.SwigDelegateGLAdapter_459 delegate459, GLAdapter.SwigDelegateGLAdapter_460 delegate460, GLAdapter.SwigDelegateGLAdapter_461 delegate461, GLAdapter.SwigDelegateGLAdapter_462 delegate462, GLAdapter.SwigDelegateGLAdapter_463 delegate463, GLAdapter.SwigDelegateGLAdapter_464 delegate464, GLAdapter.SwigDelegateGLAdapter_465 delegate465, GLAdapter.SwigDelegateGLAdapter_466 delegate466, GLAdapter.SwigDelegateGLAdapter_467 delegate467, GLAdapter.SwigDelegateGLAdapter_468 delegate468, GLAdapter.SwigDelegateGLAdapter_469 delegate469, GLAdapter.SwigDelegateGLAdapter_470 delegate470, GLAdapter.SwigDelegateGLAdapter_471 delegate471, GLAdapter.SwigDelegateGLAdapter_472 delegate472, GLAdapter.SwigDelegateGLAdapter_473 delegate473, GLAdapter.SwigDelegateGLAdapter_474 delegate474, GLAdapter.SwigDelegateGLAdapter_475 delegate475, GLAdapter.SwigDelegateGLAdapter_476 delegate476, GLAdapter.SwigDelegateGLAdapter_477 delegate477, GLAdapter.SwigDelegateGLAdapter_478 delegate478, GLAdapter.SwigDelegateGLAdapter_479 delegate479, GLAdapter.SwigDelegateGLAdapter_480 delegate480, GLAdapter.SwigDelegateGLAdapter_481 delegate481, GLAdapter.SwigDelegateGLAdapter_482 delegate482, GLAdapter.SwigDelegateGLAdapter_483 delegate483, GLAdapter.SwigDelegateGLAdapter_484 delegate484, GLAdapter.SwigDelegateGLAdapter_485 delegate485, GLAdapter.SwigDelegateGLAdapter_486 delegate486, GLAdapter.SwigDelegateGLAdapter_487 delegate487, GLAdapter.SwigDelegateGLAdapter_488 delegate488, GLAdapter.SwigDelegateGLAdapter_489 delegate489, GLAdapter.SwigDelegateGLAdapter_490 delegate490, GLAdapter.SwigDelegateGLAdapter_491 delegate491, GLAdapter.SwigDelegateGLAdapter_492 delegate492, GLAdapter.SwigDelegateGLAdapter_493 delegate493, GLAdapter.SwigDelegateGLAdapter_494 delegate494, GLAdapter.SwigDelegateGLAdapter_495 delegate495, GLAdapter.SwigDelegateGLAdapter_496 delegate496, GLAdapter.SwigDelegateGLAdapter_497 delegate497, GLAdapter.SwigDelegateGLAdapter_498 delegate498, GLAdapter.SwigDelegateGLAdapter_499 delegate499, GLAdapter.SwigDelegateGLAdapter_500 delegate500, GLAdapter.SwigDelegateGLAdapter_501 delegate501, GLAdapter.SwigDelegateGLAdapter_502 delegate502, GLAdapter.SwigDelegateGLAdapter_503 delegate503, GLAdapter.SwigDelegateGLAdapter_504 delegate504, GLAdapter.SwigDelegateGLAdapter_505 delegate505, GLAdapter.SwigDelegateGLAdapter_506 delegate506, GLAdapter.SwigDelegateGLAdapter_507 delegate507, GLAdapter.SwigDelegateGLAdapter_508 delegate508, GLAdapter.SwigDelegateGLAdapter_509 delegate509, GLAdapter.SwigDelegateGLAdapter_510 delegate510);

	// Token: 0x060007F5 RID: 2037
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_MetaHandler")]
	public static extern void delete_MetaHandler(HandleRef jarg1);

	// Token: 0x060007F6 RID: 2038
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_SetCurrentThread")]
	public static extern void MetaHandler_SetCurrentThread(HandleRef jarg1, uint jarg2);

	// Token: 0x060007F7 RID: 2039
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessTextAscii")]
	public static extern void MetaHandler_ProcessTextAscii(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x060007F8 RID: 2040
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessTextXML")]
	public static extern void MetaHandler_ProcessTextXML(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x060007F9 RID: 2041
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessTextJSON")]
	public static extern void MetaHandler_ProcessTextJSON(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x060007FA RID: 2042
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessDisplayMessageCommand")]
	public static extern void MetaHandler_ProcessDisplayMessageCommand(HandleRef jarg1, string jarg2);

	// Token: 0x060007FB RID: 2043
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessSetWindowSizeCommand")]
	public static extern void MetaHandler_ProcessSetWindowSizeCommand(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x060007FC RID: 2044
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessCreateEGLImageBufferCommand")]
	public static extern void MetaHandler_ProcessCreateEGLImageBufferCommand(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x060007FD RID: 2045
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessSetEGLImageContentCommand")]
	public static extern void MetaHandler_ProcessSetEGLImageContentCommand(HandleRef jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x060007FE RID: 2046
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessEGLConfigStateDesc")]
	public static extern void MetaHandler_ProcessEGLConfigStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060007FF RID: 2047
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessEGLSurfaceStateDesc")]
	public static extern void MetaHandler_ProcessEGLSurfaceStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x06000800 RID: 2048
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLUniformsStateDesc")]
	public static extern void MetaHandler_ProcessGLUniformsStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000801 RID: 2049
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLUniformBlocksStateDesc")]
	public static extern void MetaHandler_ProcessGLUniformBlocksStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000802 RID: 2050
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLAttributesStateDesc")]
	public static extern void MetaHandler_ProcessGLAttributesStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000803 RID: 2051
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLAtomicCounterBufferStateDesc")]
	public static extern void MetaHandler_ProcessGLAtomicCounterBufferStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000804 RID: 2052
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLBufferVariablesStateDesc")]
	public static extern void MetaHandler_ProcessGLBufferVariablesStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000805 RID: 2053
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLStorageBlocksStateDesc")]
	public static extern void MetaHandler_ProcessGLStorageBlocksStateDesc(HandleRef jarg1, uint jarg2, uint jarg3, IntPtr jarg4);

	// Token: 0x06000806 RID: 2054
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessProfilerData")]
	public static extern void MetaHandler_ProcessProfilerData(HandleRef jarg1, uint jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06000807 RID: 2055
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessGLLimits")]
	public static extern void MetaHandler_ProcessGLLimits(HandleRef jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x06000808 RID: 2056
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessDefaultColorBuffer")]
	public static extern void MetaHandler_ProcessDefaultColorBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8, IntPtr jarg9);

	// Token: 0x06000809 RID: 2057
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessDefaultDepthBuffer")]
	public static extern void MetaHandler_ProcessDefaultDepthBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8, IntPtr jarg9);

	// Token: 0x0600080A RID: 2058
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessDefaultStencilBuffer")]
	public static extern void MetaHandler_ProcessDefaultStencilBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8, IntPtr jarg9);

	// Token: 0x0600080B RID: 2059
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_ProcessMultiSampleTexture")]
	public static extern void MetaHandler_ProcessMultiSampleTexture(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8, uint jarg9, uint jarg10, uint jarg11, IntPtr jarg12);

	// Token: 0x0600080C RID: 2060
	[DllImport("libDCAP", EntryPoint = "CSharp_new_MetaHandler")]
	public static extern IntPtr new_MetaHandler();

	// Token: 0x0600080D RID: 2061
	[DllImport("libDCAP", EntryPoint = "CSharp_MetaHandler_director_connect")]
	public static extern void MetaHandler_director_connect(HandleRef jarg1, MetaHandler.SwigDelegateMetaHandler_0 delegate0, MetaHandler.SwigDelegateMetaHandler_1 delegate1, MetaHandler.SwigDelegateMetaHandler_2 delegate2, MetaHandler.SwigDelegateMetaHandler_3 delegate3, MetaHandler.SwigDelegateMetaHandler_4 delegate4, MetaHandler.SwigDelegateMetaHandler_5 delegate5, MetaHandler.SwigDelegateMetaHandler_6 delegate6, MetaHandler.SwigDelegateMetaHandler_7 delegate7, MetaHandler.SwigDelegateMetaHandler_8 delegate8, MetaHandler.SwigDelegateMetaHandler_9 delegate9, MetaHandler.SwigDelegateMetaHandler_10 delegate10, MetaHandler.SwigDelegateMetaHandler_11 delegate11, MetaHandler.SwigDelegateMetaHandler_12 delegate12, MetaHandler.SwigDelegateMetaHandler_13 delegate13, MetaHandler.SwigDelegateMetaHandler_14 delegate14, MetaHandler.SwigDelegateMetaHandler_15 delegate15, MetaHandler.SwigDelegateMetaHandler_16 delegate16, MetaHandler.SwigDelegateMetaHandler_17 delegate17, MetaHandler.SwigDelegateMetaHandler_18 delegate18, MetaHandler.SwigDelegateMetaHandler_19 delegate19, MetaHandler.SwigDelegateMetaHandler_20 delegate20, MetaHandler.SwigDelegateMetaHandler_21 delegate21);

	// Token: 0x0600080E RID: 2062
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_Decoder")]
	public static extern void delete_Decoder(HandleRef jarg1);

	// Token: 0x0600080F RID: 2063
	[DllImport("libDCAP", EntryPoint = "CSharp_Decoder_SetCurrentThreadId")]
	public static extern void Decoder_SetCurrentThreadId(HandleRef jarg1, uint jarg2);

	// Token: 0x06000810 RID: 2064
	[DllImport("libDCAP", EntryPoint = "CSharp_Decoder_GetCurrentThreadId")]
	public static extern uint Decoder_GetCurrentThreadId(HandleRef jarg1);

	// Token: 0x06000811 RID: 2065
	[DllImport("libDCAP", EntryPoint = "CSharp_Decoder_SupportsId")]
	public static extern bool Decoder_SupportsId(HandleRef jarg1, int jarg2);

	// Token: 0x06000812 RID: 2066
	[DllImport("libDCAP", EntryPoint = "CSharp_Decoder_ProcessFunctionCall")]
	public static extern void Decoder_ProcessFunctionCall(HandleRef jarg1, int jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06000813 RID: 2067
	[DllImport("libDCAP", EntryPoint = "CSharp_Decoder_ProcessMethodCall")]
	public static extern void Decoder_ProcessMethodCall(HandleRef jarg1, int jarg2, uint jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x06000814 RID: 2068
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_FrameEntry_number_set")]
	public static extern void CaptureFileReader_FrameEntry_number_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000815 RID: 2069
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_FrameEntry_number_get")]
	public static extern uint CaptureFileReader_FrameEntry_number_get(HandleRef jarg1);

	// Token: 0x06000816 RID: 2070
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_FrameEntry_position_set")]
	public static extern void CaptureFileReader_FrameEntry_position_set(HandleRef jarg1, ulong jarg2);

	// Token: 0x06000817 RID: 2071
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_FrameEntry_position_get")]
	public static extern ulong CaptureFileReader_FrameEntry_position_get(HandleRef jarg1);

	// Token: 0x06000818 RID: 2072
	[DllImport("libDCAP", EntryPoint = "CSharp_new_CaptureFileReader_FrameEntry")]
	public static extern IntPtr new_CaptureFileReader_FrameEntry();

	// Token: 0x06000819 RID: 2073
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_CaptureFileReader_FrameEntry")]
	public static extern void delete_CaptureFileReader_FrameEntry(HandleRef jarg1);

	// Token: 0x0600081A RID: 2074
	[DllImport("libDCAP", EntryPoint = "CSharp_new_CaptureFileReader")]
	public static extern IntPtr new_CaptureFileReader();

	// Token: 0x0600081B RID: 2075
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_CaptureFileReader")]
	public static extern void delete_CaptureFileReader(HandleRef jarg1);

	// Token: 0x0600081C RID: 2076
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_AddBlockEventHandler")]
	public static extern void CaptureFileReader_AddBlockEventHandler(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600081D RID: 2077
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_RemoveBlockEventHandler")]
	public static extern void CaptureFileReader_RemoveBlockEventHandler(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600081E RID: 2078
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_AddDecoder")]
	public static extern void CaptureFileReader_AddDecoder(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600081F RID: 2079
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_RemoveDecoder")]
	public static extern void CaptureFileReader_RemoveDecoder(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000820 RID: 2080
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_AddMetaHandler")]
	public static extern void CaptureFileReader_AddMetaHandler(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000821 RID: 2081
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_RemoveMetaHandler")]
	public static extern void CaptureFileReader_RemoveMetaHandler(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000822 RID: 2082
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_IsActive")]
	public static extern bool CaptureFileReader_IsActive(HandleRef jarg1);

	// Token: 0x06000823 RID: 2083
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_IsTrimmedFile")]
	public static extern bool CaptureFileReader_IsTrimmedFile(HandleRef jarg1);

	// Token: 0x06000824 RID: 2084
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_IsCompressedFile")]
	public static extern bool CaptureFileReader_IsCompressedFile(HandleRef jarg1);

	// Token: 0x06000825 RID: 2085
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_OmitsTextureData")]
	public static extern bool CaptureFileReader_OmitsTextureData(HandleRef jarg1);

	// Token: 0x06000826 RID: 2086
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_OmitsAllData")]
	public static extern bool CaptureFileReader_OmitsAllData(HandleRef jarg1);

	// Token: 0x06000827 RID: 2087
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_HasThreadId")]
	public static extern bool CaptureFileReader_HasThreadId(HandleRef jarg1);

	// Token: 0x06000828 RID: 2088
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_HasTimestamp")]
	public static extern bool CaptureFileReader_HasTimestamp(HandleRef jarg1);

	// Token: 0x06000829 RID: 2089
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_HasTrailer")]
	public static extern bool CaptureFileReader_HasTrailer(HandleRef jarg1);

	// Token: 0x0600082A RID: 2090
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_Initialize__SWIG_0")]
	public static extern void CaptureFileReader_Initialize__SWIG_0(HandleRef jarg1, HandleRef jarg2, ulong jarg3);

	// Token: 0x0600082B RID: 2091
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_Initialize__SWIG_1")]
	public static extern void CaptureFileReader_Initialize__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600082C RID: 2092
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCurrentBlockPosition")]
	public static extern ulong CaptureFileReader_GetCurrentBlockPosition(HandleRef jarg1);

	// Token: 0x0600082D RID: 2093
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCurrentBlockType")]
	public static extern int CaptureFileReader_GetCurrentBlockType(HandleRef jarg1);

	// Token: 0x0600082E RID: 2094
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCurrentFrame")]
	public static extern uint CaptureFileReader_GetCurrentFrame(HandleRef jarg1);

	// Token: 0x0600082F RID: 2095
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetNumFrames")]
	public static extern uint CaptureFileReader_GetNumFrames(HandleRef jarg1);

	// Token: 0x06000830 RID: 2096
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetFileSize")]
	public static extern ulong CaptureFileReader_GetFileSize(HandleRef jarg1);

	// Token: 0x06000831 RID: 2097
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetFileVersion")]
	public static extern uint CaptureFileReader_GetFileVersion(HandleRef jarg1);

	// Token: 0x06000832 RID: 2098
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetFileVersionMajor")]
	public static extern IntPtr CaptureFileReader_GetFileVersionMajor(HandleRef jarg1);

	// Token: 0x06000833 RID: 2099
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetFileVersionMinor")]
	public static extern IntPtr CaptureFileReader_GetFileVersionMinor(HandleRef jarg1);

	// Token: 0x06000834 RID: 2100
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_SetLoopFrame")]
	public static extern void CaptureFileReader_SetLoopFrame(HandleRef jarg1, uint jarg2);

	// Token: 0x06000835 RID: 2101
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_SetLoopCount")]
	public static extern void CaptureFileReader_SetLoopCount(HandleRef jarg1, uint jarg2);

	// Token: 0x06000836 RID: 2102
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetLoopFrame")]
	public static extern uint CaptureFileReader_GetLoopFrame(HandleRef jarg1);

	// Token: 0x06000837 RID: 2103
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_SetFilterThreads")]
	public static extern void CaptureFileReader_SetFilterThreads(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000838 RID: 2104
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_SetFilterThreadStartFrame")]
	public static extern void CaptureFileReader_SetFilterThreadStartFrame(HandleRef jarg1, uint jarg2);

	// Token: 0x06000839 RID: 2105
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCurrentThreadNumber")]
	public static extern uint CaptureFileReader_GetCurrentThreadNumber(HandleRef jarg1);

	// Token: 0x0600083A RID: 2106
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCurrentThreadId")]
	public static extern uint CaptureFileReader_GetCurrentThreadId(HandleRef jarg1);

	// Token: 0x0600083B RID: 2107
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetLastCallId")]
	public static extern int CaptureFileReader_GetLastCallId(HandleRef jarg1);

	// Token: 0x0600083C RID: 2108
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_InvalidateState")]
	public static extern void CaptureFileReader_InvalidateState(HandleRef jarg1);

	// Token: 0x0600083D RID: 2109
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_SetCurrentFrame")]
	public static extern bool CaptureFileReader_SetCurrentFrame(HandleRef jarg1, uint jarg2);

	// Token: 0x0600083E RID: 2110
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_ProcessNextBlock")]
	public static extern bool CaptureFileReader_ProcessNextBlock(HandleRef jarg1);

	// Token: 0x0600083F RID: 2111
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_ProcessNextFrame")]
	public static extern bool CaptureFileReader_ProcessNextFrame(HandleRef jarg1);

	// Token: 0x06000840 RID: 2112
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GenerateFrameIndex")]
	public static extern bool CaptureFileReader_GenerateFrameIndex(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000841 RID: 2113
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCompressionAlgorithm")]
	public static extern int CaptureFileReader_GetCompressionAlgorithm(HandleRef jarg1);

	// Token: 0x06000842 RID: 2114
	[DllImport("libDCAP", EntryPoint = "CSharp_CaptureFileReader_GetCompressor")]
	public static extern IntPtr CaptureFileReader_GetCompressor(HandleRef jarg1);

	// Token: 0x06000843 RID: 2115
	[DllImport("libDCAP", EntryPoint = "CSharp_new_EGLDecoder")]
	public static extern IntPtr new_EGLDecoder();

	// Token: 0x06000844 RID: 2116
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_EGLDecoder")]
	public static extern void delete_EGLDecoder(HandleRef jarg1);

	// Token: 0x06000845 RID: 2117
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_AddAdapter")]
	public static extern void EGLDecoder_AddAdapter(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000846 RID: 2118
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_RemoveAdapter")]
	public static extern void EGLDecoder_RemoveAdapter(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000847 RID: 2119
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_SupportsId")]
	public static extern bool EGLDecoder_SupportsId(HandleRef jarg1, int jarg2);

	// Token: 0x06000848 RID: 2120
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_ProcessFunctionCall")]
	public static extern void EGLDecoder_ProcessFunctionCall(HandleRef jarg1, int jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06000849 RID: 2121
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_ProcessMethodCall")]
	public static extern void EGLDecoder_ProcessMethodCall(HandleRef jarg1, int jarg2, uint jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x0600084A RID: 2122
	[DllImport("libDCAP", EntryPoint = "CSharp_new_GLDecoder")]
	public static extern IntPtr new_GLDecoder();

	// Token: 0x0600084B RID: 2123
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_GLDecoder")]
	public static extern void delete_GLDecoder(HandleRef jarg1);

	// Token: 0x0600084C RID: 2124
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_AddAdapter")]
	public static extern void GLDecoder_AddAdapter(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600084D RID: 2125
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_RemoveAdapter")]
	public static extern void GLDecoder_RemoveAdapter(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600084E RID: 2126
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_SupportsId")]
	public static extern bool GLDecoder_SupportsId(HandleRef jarg1, int jarg2);

	// Token: 0x0600084F RID: 2127
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_ProcessFunctionCall")]
	public static extern void GLDecoder_ProcessFunctionCall(HandleRef jarg1, int jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06000850 RID: 2128
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_ProcessMethodCall")]
	public static extern void GLDecoder_ProcessMethodCall(HandleRef jarg1, int jarg2, uint jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x06000851 RID: 2129
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DataReader")]
	public static extern void delete_DataReader(HandleRef jarg1);

	// Token: 0x06000852 RID: 2130
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_IsValid")]
	public static extern bool DataReader_IsValid(HandleRef jarg1);

	// Token: 0x06000853 RID: 2131
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_Open")]
	public static extern int DataReader_Open(HandleRef jarg1);

	// Token: 0x06000854 RID: 2132
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_Read")]
	public static extern uint DataReader_Read(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06000855 RID: 2133
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_Close")]
	public static extern void DataReader_Close(HandleRef jarg1);

	// Token: 0x06000856 RID: 2134
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_AtEof")]
	public static extern bool DataReader_AtEof(HandleRef jarg1);

	// Token: 0x06000857 RID: 2135
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_Seek")]
	public static extern int DataReader_Seek(HandleRef jarg1, int jarg2, ulong jarg3);

	// Token: 0x06000858 RID: 2136
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_Tell")]
	public static extern ulong DataReader_Tell(HandleRef jarg1);

	// Token: 0x06000859 RID: 2137
	[DllImport("libDCAP", EntryPoint = "CSharp_DataReader_GetName")]
	public static extern string DataReader_GetName(HandleRef jarg1);

	// Token: 0x0600085A RID: 2138
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DataWriter")]
	public static extern void delete_DataWriter(HandleRef jarg1);

	// Token: 0x0600085B RID: 2139
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_IsValid")]
	public static extern bool DataWriter_IsValid(HandleRef jarg1);

	// Token: 0x0600085C RID: 2140
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Open")]
	public static extern int DataWriter_Open(HandleRef jarg1);

	// Token: 0x0600085D RID: 2141
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Write")]
	public static extern uint DataWriter_Write(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x0600085E RID: 2142
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_WriteAt")]
	public static extern uint DataWriter_WriteAt(HandleRef jarg1, IntPtr jarg2, uint jarg3, ulong jarg4);

	// Token: 0x0600085F RID: 2143
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Flush")]
	public static extern void DataWriter_Flush(HandleRef jarg1);

	// Token: 0x06000860 RID: 2144
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Close")]
	public static extern void DataWriter_Close(HandleRef jarg1);

	// Token: 0x06000861 RID: 2145
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Seek")]
	public static extern int DataWriter_Seek(HandleRef jarg1, int jarg2, ulong jarg3);

	// Token: 0x06000862 RID: 2146
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_Tell")]
	public static extern ulong DataWriter_Tell(HandleRef jarg1);

	// Token: 0x06000863 RID: 2147
	[DllImport("libDCAP", EntryPoint = "CSharp_DataWriter_GetName")]
	public static extern string DataWriter_GetName(HandleRef jarg1);

	// Token: 0x06000864 RID: 2148
	[DllImport("libDCAP", EntryPoint = "CSharp_new_StdioReader")]
	public static extern IntPtr new_StdioReader(string jarg1);

	// Token: 0x06000865 RID: 2149
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_StdioReader")]
	public static extern void delete_StdioReader(HandleRef jarg1);

	// Token: 0x06000866 RID: 2150
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_IsValid")]
	public static extern bool StdioReader_IsValid(HandleRef jarg1);

	// Token: 0x06000867 RID: 2151
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_Open")]
	public static extern int StdioReader_Open(HandleRef jarg1);

	// Token: 0x06000868 RID: 2152
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_Read")]
	public static extern uint StdioReader_Read(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06000869 RID: 2153
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_Close")]
	public static extern void StdioReader_Close(HandleRef jarg1);

	// Token: 0x0600086A RID: 2154
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_AtEof")]
	public static extern bool StdioReader_AtEof(HandleRef jarg1);

	// Token: 0x0600086B RID: 2155
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_Seek")]
	public static extern int StdioReader_Seek(HandleRef jarg1, int jarg2, ulong jarg3);

	// Token: 0x0600086C RID: 2156
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_Tell")]
	public static extern ulong StdioReader_Tell(HandleRef jarg1);

	// Token: 0x0600086D RID: 2157
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_GetName")]
	public static extern string StdioReader_GetName(HandleRef jarg1);

	// Token: 0x0600086E RID: 2158
	[DllImport("libDCAP", EntryPoint = "CSharp_new_StdioWriter__SWIG_0")]
	public static extern IntPtr new_StdioWriter__SWIG_0(string jarg1, bool jarg2, bool jarg3);

	// Token: 0x0600086F RID: 2159
	[DllImport("libDCAP", EntryPoint = "CSharp_new_StdioWriter__SWIG_1")]
	public static extern IntPtr new_StdioWriter__SWIG_1(string jarg1, bool jarg2);

	// Token: 0x06000870 RID: 2160
	[DllImport("libDCAP", EntryPoint = "CSharp_new_StdioWriter__SWIG_2")]
	public static extern IntPtr new_StdioWriter__SWIG_2(string jarg1);

	// Token: 0x06000871 RID: 2161
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_StdioWriter")]
	public static extern void delete_StdioWriter(HandleRef jarg1);

	// Token: 0x06000872 RID: 2162
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_IsValid")]
	public static extern bool StdioWriter_IsValid(HandleRef jarg1);

	// Token: 0x06000873 RID: 2163
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Open")]
	public static extern int StdioWriter_Open(HandleRef jarg1);

	// Token: 0x06000874 RID: 2164
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Write")]
	public static extern uint StdioWriter_Write(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06000875 RID: 2165
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_WriteAt")]
	public static extern uint StdioWriter_WriteAt(HandleRef jarg1, IntPtr jarg2, uint jarg3, ulong jarg4);

	// Token: 0x06000876 RID: 2166
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Flush")]
	public static extern void StdioWriter_Flush(HandleRef jarg1);

	// Token: 0x06000877 RID: 2167
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Close")]
	public static extern void StdioWriter_Close(HandleRef jarg1);

	// Token: 0x06000878 RID: 2168
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Seek")]
	public static extern int StdioWriter_Seek(HandleRef jarg1, int jarg2, ulong jarg3);

	// Token: 0x06000879 RID: 2169
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_Tell")]
	public static extern ulong StdioWriter_Tell(HandleRef jarg1);

	// Token: 0x0600087A RID: 2170
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_GetName")]
	public static extern string StdioWriter_GetName(HandleRef jarg1);

	// Token: 0x0600087B RID: 2171
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_CompressorQualityDefault_get")]
	public static extern int Compressor_CompressorQualityDefault_get();

	// Token: 0x0600087C RID: 2172
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_CompressorBufferSizeInBytesDefault_get")]
	public static extern uint Compressor_CompressorBufferSizeInBytesDefault_get();

	// Token: 0x0600087D RID: 2173
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_Compressor")]
	public static extern void delete_Compressor(HandleRef jarg1);

	// Token: 0x0600087E RID: 2174
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_Encode")]
	public static extern bool Compressor_Encode(HandleRef jarg1, IntPtr jarg2, uint jarg3, HandleRef jarg4, IntPtr jarg5);

	// Token: 0x0600087F RID: 2175
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_Decode__SWIG_0")]
	public static extern bool Compressor_Decode__SWIG_0(HandleRef jarg1, IntPtr jarg2, uint jarg3, HandleRef jarg4, IntPtr jarg5);

	// Token: 0x06000880 RID: 2176
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_Decode__SWIG_1")]
	public static extern bool Compressor_Decode__SWIG_1(HandleRef jarg1, IntPtr jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5);

	// Token: 0x06000881 RID: 2177
	[DllImport("libDCAP", EntryPoint = "CSharp_Compressor_GetQuality")]
	public static extern int Compressor_GetQuality(HandleRef jarg1);

	// Token: 0x06000882 RID: 2178
	[DllImport("libDCAP", EntryPoint = "CSharp_FrameEntry_number_set")]
	public static extern void FrameEntry_number_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000883 RID: 2179
	[DllImport("libDCAP", EntryPoint = "CSharp_FrameEntry_number_get")]
	public static extern uint FrameEntry_number_get(HandleRef jarg1);

	// Token: 0x06000884 RID: 2180
	[DllImport("libDCAP", EntryPoint = "CSharp_FrameEntry_position_set")]
	public static extern void FrameEntry_position_set(HandleRef jarg1, ulong jarg2);

	// Token: 0x06000885 RID: 2181
	[DllImport("libDCAP", EntryPoint = "CSharp_FrameEntry_position_get")]
	public static extern ulong FrameEntry_position_get(HandleRef jarg1);

	// Token: 0x06000886 RID: 2182
	[DllImport("libDCAP", EntryPoint = "CSharp_new_FrameEntry")]
	public static extern IntPtr new_FrameEntry();

	// Token: 0x06000887 RID: 2183
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_FrameEntry")]
	public static extern void delete_FrameEntry(HandleRef jarg1);

	// Token: 0x06000888 RID: 2184
	[DllImport("libDCAP", EntryPoint = "CSharp_GetFileSize")]
	public static extern ulong GetFileSize(string jarg1);

	// Token: 0x06000889 RID: 2185
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFileFinalized__SWIG_0")]
	public static extern bool IsFileFinalized__SWIG_0(string jarg1, ulong jarg2);

	// Token: 0x0600088A RID: 2186
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFileFinalized__SWIG_1")]
	public static extern bool IsFileFinalized__SWIG_1(string jarg1);

	// Token: 0x0600088B RID: 2187
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFileFinalized__SWIG_2")]
	public static extern bool IsFileFinalized__SWIG_2(HandleRef jarg1, ulong jarg2);

	// Token: 0x0600088C RID: 2188
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFinalFrameTrailerNeeded__SWIG_0")]
	public static extern bool IsFinalFrameTrailerNeeded__SWIG_0(string jarg1, ulong jarg2);

	// Token: 0x0600088D RID: 2189
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFinalFrameTrailerNeeded__SWIG_1")]
	public static extern bool IsFinalFrameTrailerNeeded__SWIG_1(HandleRef jarg1, ulong jarg2);

	// Token: 0x0600088E RID: 2190
	[DllImport("libDCAP", EntryPoint = "CSharp_GenerateFrameIndex__SWIG_0")]
	public static extern void GenerateFrameIndex__SWIG_0(string jarg1, HandleRef jarg2);

	// Token: 0x0600088F RID: 2191
	[DllImport("libDCAP", EntryPoint = "CSharp_GenerateFrameIndex__SWIG_1")]
	public static extern void GenerateFrameIndex__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000890 RID: 2192
	[DllImport("libDCAP", EntryPoint = "CSharp_FinalizeFile__SWIG_0")]
	public static extern void FinalizeFile__SWIG_0(string jarg1, ulong jarg2, uint jarg3, ulong jarg4, bool jarg5);

	// Token: 0x06000891 RID: 2193
	[DllImport("libDCAP", EntryPoint = "CSharp_FinalizeFile__SWIG_1")]
	public static extern void FinalizeFile__SWIG_1(HandleRef jarg1, ulong jarg2, uint jarg3, ulong jarg4, bool jarg5);

	// Token: 0x06000892 RID: 2194
	[DllImport("libDCAP", EntryPoint = "CSharp_EnsureFinalization")]
	public static extern void EnsureFinalization(string jarg1);

	// Token: 0x06000893 RID: 2195
	[DllImport("libDCAP", EntryPoint = "CSharp_EnsureDCAPFinalized")]
	public static extern bool EnsureDCAPFinalized(string jarg1);

	// Token: 0x06000894 RID: 2196
	[DllImport("libDCAP", EntryPoint = "CSharp_OmitsTextures_get")]
	public static extern uint OmitsTextures_get();

	// Token: 0x06000895 RID: 2197
	[DllImport("libDCAP", EntryPoint = "CSharp_OmitsData_get")]
	public static extern uint OmitsData_get();

	// Token: 0x06000896 RID: 2198
	[DllImport("libDCAP", EntryPoint = "CSharp_HaveThreadIdMask_get")]
	public static extern uint HaveThreadIdMask_get();

	// Token: 0x06000897 RID: 2199
	[DllImport("libDCAP", EntryPoint = "CSharp_HaveTimestampMask_get")]
	public static extern uint HaveTimestampMask_get();

	// Token: 0x06000898 RID: 2200
	[DllImport("libDCAP", EntryPoint = "CSharp_DisableFrameTrailerMask_get")]
	public static extern uint DisableFrameTrailerMask_get();

	// Token: 0x06000899 RID: 2201
	[DllImport("libDCAP", EntryPoint = "CSharp_BlockSize64BitMask_get")]
	public static extern uint BlockSize64BitMask_get();

	// Token: 0x0600089A RID: 2202
	[DllImport("libDCAP", EntryPoint = "CSharp_BlockType32BitMask_get")]
	public static extern uint BlockType32BitMask_get();

	// Token: 0x0600089B RID: 2203
	[DllImport("libDCAP", EntryPoint = "CSharp_ObjectId64BitMask_get")]
	public static extern uint ObjectId64BitMask_get();

	// Token: 0x0600089C RID: 2204
	[DllImport("libDCAP", EntryPoint = "CSharp_TrimmedFileMask_get")]
	public static extern uint TrimmedFileMask_get();

	// Token: 0x0600089D RID: 2205
	[DllImport("libDCAP", EntryPoint = "CSharp_CompressedBlockDataMask_get")]
	public static extern uint CompressedBlockDataMask_get();

	// Token: 0x0600089E RID: 2206
	[DllImport("libDCAP", EntryPoint = "CSharp_frameSize64BitMask_get")]
	public static extern uint frameSize64BitMask_get();

	// Token: 0x0600089F RID: 2207
	[DllImport("libDCAP", EntryPoint = "CSharp_StateIntervalMask_get")]
	public static extern uint StateIntervalMask_get();

	// Token: 0x060008A0 RID: 2208
	[DllImport("libDCAP", EntryPoint = "CSharp_VersionMajorMask_get")]
	public static extern uint VersionMajorMask_get();

	// Token: 0x060008A1 RID: 2209
	[DllImport("libDCAP", EntryPoint = "CSharp_VersionMinorMask_get")]
	public static extern uint VersionMinorMask_get();

	// Token: 0x060008A2 RID: 2210
	[DllImport("libDCAP", EntryPoint = "CSharp_VersionContentRevMask_get")]
	public static extern uint VersionContentRevMask_get();

	// Token: 0x060008A3 RID: 2211
	[DllImport("libDCAP", EntryPoint = "CSharp_GetFileVersionMajor")]
	public static extern IntPtr GetFileVersionMajor(uint jarg1);

	// Token: 0x060008A4 RID: 2212
	[DllImport("libDCAP", EntryPoint = "CSharp_GetFileVersionMinor")]
	public static extern IntPtr GetFileVersionMinor(uint jarg1);

	// Token: 0x060008A5 RID: 2213
	[DllImport("libDCAP", EntryPoint = "CSharp_IsTextureDataOmitted")]
	public static extern bool IsTextureDataOmitted(uint jarg1);

	// Token: 0x060008A6 RID: 2214
	[DllImport("libDCAP", EntryPoint = "CSharp_IsAllDataOmitted")]
	public static extern bool IsAllDataOmitted(uint jarg1);

	// Token: 0x060008A7 RID: 2215
	[DllImport("libDCAP", EntryPoint = "CSharp_IsThreadIdPresent")]
	public static extern bool IsThreadIdPresent(uint jarg1);

	// Token: 0x060008A8 RID: 2216
	[DllImport("libDCAP", EntryPoint = "CSharp_IsTimestampPresent")]
	public static extern bool IsTimestampPresent(uint jarg1);

	// Token: 0x060008A9 RID: 2217
	[DllImport("libDCAP", EntryPoint = "CSharp_IsFrameTrailerDisabled")]
	public static extern bool IsFrameTrailerDisabled(uint jarg1);

	// Token: 0x060008AA RID: 2218
	[DllImport("libDCAP", EntryPoint = "CSharp_IsBlockType32Bit")]
	public static extern bool IsBlockType32Bit(uint jarg1);

	// Token: 0x060008AB RID: 2219
	[DllImport("libDCAP", EntryPoint = "CSharp_IsObjectId64Bit")]
	public static extern bool IsObjectId64Bit(uint jarg1);

	// Token: 0x060008AC RID: 2220
	[DllImport("libDCAP", EntryPoint = "CSharp_IsTrimmedFile")]
	public static extern bool IsTrimmedFile(uint jarg1);

	// Token: 0x060008AD RID: 2221
	[DllImport("libDCAP", EntryPoint = "CSharp_GetStateInterval")]
	public static extern byte GetStateInterval(uint jarg1);

	// Token: 0x060008AE RID: 2222
	[DllImport("libDCAP", EntryPoint = "CSharp_GetCompressionAlgorithm")]
	public static extern int GetCompressionAlgorithm(uint jarg1);

	// Token: 0x060008AF RID: 2223
	[DllImport("libDCAP", EntryPoint = "CSharp_Is64BitFrameSize")]
	public static extern bool Is64BitFrameSize(uint jarg1);

	// Token: 0x060008B0 RID: 2224
	[DllImport("libDCAP", EntryPoint = "CSharp_SetOmitsTextures")]
	public static extern uint SetOmitsTextures(uint jarg1, bool jarg2);

	// Token: 0x060008B1 RID: 2225
	[DllImport("libDCAP", EntryPoint = "CSharp_SetOmitsData")]
	public static extern uint SetOmitsData(uint jarg1, bool jarg2);

	// Token: 0x060008B2 RID: 2226
	[DllImport("libDCAP", EntryPoint = "CSharp_SetThreadIdPresent")]
	public static extern uint SetThreadIdPresent(uint jarg1, bool jarg2);

	// Token: 0x060008B3 RID: 2227
	[DllImport("libDCAP", EntryPoint = "CSharp_SetTimestampPresent")]
	public static extern uint SetTimestampPresent(uint jarg1, bool jarg2);

	// Token: 0x060008B4 RID: 2228
	[DllImport("libDCAP", EntryPoint = "CSharp_SetFrameTrailerDisabled")]
	public static extern uint SetFrameTrailerDisabled(uint jarg1, bool jarg2);

	// Token: 0x060008B5 RID: 2229
	[DllImport("libDCAP", EntryPoint = "CSharp_SetBlockDataCompressed")]
	public static extern uint SetBlockDataCompressed(uint jarg1, int jarg2);

	// Token: 0x060008B6 RID: 2230
	[DllImport("libDCAP", EntryPoint = "CSharp_SetBlockType32Bit")]
	public static extern uint SetBlockType32Bit(uint jarg1, bool jarg2);

	// Token: 0x060008B7 RID: 2231
	[DllImport("libDCAP", EntryPoint = "CSharp_SetObjectId64Bit")]
	public static extern uint SetObjectId64Bit(uint jarg1, bool jarg2);

	// Token: 0x060008B8 RID: 2232
	[DllImport("libDCAP", EntryPoint = "CSharp_SetTrimmedFile")]
	public static extern uint SetTrimmedFile(uint jarg1, bool jarg2);

	// Token: 0x060008B9 RID: 2233
	[DllImport("libDCAP", EntryPoint = "CSharp_SetStateInterval")]
	public static extern uint SetStateInterval(uint jarg1, byte jarg2);

	// Token: 0x060008BA RID: 2234
	[DllImport("libDCAP", EntryPoint = "CSharp_new_DCAPFileProcessor")]
	public static extern IntPtr new_DCAPFileProcessor();

	// Token: 0x060008BB RID: 2235
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DCAPFileProcessor")]
	public static extern void delete_DCAPFileProcessor(HandleRef jarg1);

	// Token: 0x060008BC RID: 2236
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_IsActive")]
	public static extern bool DCAPFileProcessor_IsActive(HandleRef jarg1);

	// Token: 0x060008BD RID: 2237
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_IsTrimmedFile")]
	public static extern bool DCAPFileProcessor_IsTrimmedFile(HandleRef jarg1);

	// Token: 0x060008BE RID: 2238
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_IsCompressedFile")]
	public static extern bool DCAPFileProcessor_IsCompressedFile(HandleRef jarg1);

	// Token: 0x060008BF RID: 2239
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_OmitsTextureData")]
	public static extern bool DCAPFileProcessor_OmitsTextureData(HandleRef jarg1);

	// Token: 0x060008C0 RID: 2240
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_OmitsAllData")]
	public static extern bool DCAPFileProcessor_OmitsAllData(HandleRef jarg1);

	// Token: 0x060008C1 RID: 2241
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_HasThreadId")]
	public static extern bool DCAPFileProcessor_HasThreadId(HandleRef jarg1);

	// Token: 0x060008C2 RID: 2242
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_HasTimestamp")]
	public static extern bool DCAPFileProcessor_HasTimestamp(HandleRef jarg1);

	// Token: 0x060008C3 RID: 2243
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_HasTrailer")]
	public static extern bool DCAPFileProcessor_HasTrailer(HandleRef jarg1);

	// Token: 0x060008C4 RID: 2244
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCurrentBlockPosition")]
	public static extern ulong DCAPFileProcessor_GetCurrentBlockPosition(HandleRef jarg1);

	// Token: 0x060008C5 RID: 2245
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCurrentBlockType")]
	public static extern int DCAPFileProcessor_GetCurrentBlockType(HandleRef jarg1);

	// Token: 0x060008C6 RID: 2246
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCurrentFrame")]
	public static extern uint DCAPFileProcessor_GetCurrentFrame(HandleRef jarg1);

	// Token: 0x060008C7 RID: 2247
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetNumFrames")]
	public static extern uint DCAPFileProcessor_GetNumFrames(HandleRef jarg1);

	// Token: 0x060008C8 RID: 2248
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetFileSize")]
	public static extern ulong DCAPFileProcessor_GetFileSize(HandleRef jarg1);

	// Token: 0x060008C9 RID: 2249
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetFileVersion")]
	public static extern uint DCAPFileProcessor_GetFileVersion(HandleRef jarg1);

	// Token: 0x060008CA RID: 2250
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetFileVersionMajor")]
	public static extern IntPtr DCAPFileProcessor_GetFileVersionMajor(HandleRef jarg1);

	// Token: 0x060008CB RID: 2251
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetFileVersionMinor")]
	public static extern IntPtr DCAPFileProcessor_GetFileVersionMinor(HandleRef jarg1);

	// Token: 0x060008CC RID: 2252
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCompressionAlgorithm")]
	public static extern int DCAPFileProcessor_GetCompressionAlgorithm(HandleRef jarg1);

	// Token: 0x060008CD RID: 2253
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetBytesRead")]
	public static extern ulong DCAPFileProcessor_GetBytesRead(HandleRef jarg1);

	// Token: 0x060008CE RID: 2254
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_SetLoopFrame")]
	public static extern void DCAPFileProcessor_SetLoopFrame(HandleRef jarg1, uint jarg2);

	// Token: 0x060008CF RID: 2255
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_SetLoopCount")]
	public static extern void DCAPFileProcessor_SetLoopCount(HandleRef jarg1, uint jarg2);

	// Token: 0x060008D0 RID: 2256
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetLoopFrame")]
	public static extern uint DCAPFileProcessor_GetLoopFrame(HandleRef jarg1);

	// Token: 0x060008D1 RID: 2257
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_SetFilterThreads")]
	public static extern void DCAPFileProcessor_SetFilterThreads(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008D2 RID: 2258
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_SetFilterThreadStartFrame")]
	public static extern void DCAPFileProcessor_SetFilterThreadStartFrame(HandleRef jarg1, uint jarg2);

	// Token: 0x060008D3 RID: 2259
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCurrentThreadNumber")]
	public static extern uint DCAPFileProcessor_GetCurrentThreadNumber(HandleRef jarg1);

	// Token: 0x060008D4 RID: 2260
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetCurrentThreadId")]
	public static extern uint DCAPFileProcessor_GetCurrentThreadId(HandleRef jarg1);

	// Token: 0x060008D5 RID: 2261
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_GetLastApiCallId")]
	public static extern int DCAPFileProcessor_GetLastApiCallId(HandleRef jarg1);

	// Token: 0x060008D6 RID: 2262
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_Initialize")]
	public static extern int DCAPFileProcessor_Initialize(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, ulong jarg4);

	// Token: 0x060008D7 RID: 2263
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_AtEof")]
	public static extern bool DCAPFileProcessor_AtEof(HandleRef jarg1);

	// Token: 0x060008D8 RID: 2264
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_InvalidateState")]
	public static extern void DCAPFileProcessor_InvalidateState(HandleRef jarg1);

	// Token: 0x060008D9 RID: 2265
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_ProcessNextBlock")]
	public static extern bool DCAPFileProcessor_ProcessNextBlock(HandleRef jarg1);

	// Token: 0x060008DA RID: 2266
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPFileProcessor_ProcessNextFrame")]
	public static extern bool DCAPFileProcessor_ProcessNextFrame(HandleRef jarg1);

	// Token: 0x060008DB RID: 2267
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DCAPConsumer")]
	public static extern void delete_DCAPConsumer(HandleRef jarg1);

	// Token: 0x060008DC RID: 2268
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessFileStart")]
	public static extern void DCAPConsumer_ProcessFileStart(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008DD RID: 2269
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessFileEnd")]
	public static extern void DCAPConsumer_ProcessFileEnd(HandleRef jarg1);

	// Token: 0x060008DE RID: 2270
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessBlockStart")]
	public static extern void DCAPConsumer_ProcessBlockStart(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008DF RID: 2271
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessBlockEnd")]
	public static extern void DCAPConsumer_ProcessBlockEnd(HandleRef jarg1, ulong jarg2);

	// Token: 0x060008E0 RID: 2272
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessFrameNumber")]
	public static extern void DCAPConsumer_ProcessFrameNumber(HandleRef jarg1, uint jarg2, ulong jarg3);

	// Token: 0x060008E1 RID: 2273
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessMethodCall")]
	public static extern void DCAPConsumer_ProcessMethodCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008E2 RID: 2274
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessFunctionCall")]
	public static extern void DCAPConsumer_ProcessFunctionCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008E3 RID: 2275
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPConsumer_ProcessMetadata")]
	public static extern void DCAPConsumer_ProcessMetadata(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008E4 RID: 2276
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_EncodingBufferInitialSize_get")]
	public static extern uint DCAPCompressConsumer_EncodingBufferInitialSize_get();

	// Token: 0x060008E5 RID: 2277
	[DllImport("libDCAP", EntryPoint = "CSharp_new_DCAPCompressConsumer")]
	public static extern IntPtr new_DCAPCompressConsumer(HandleRef jarg1);

	// Token: 0x060008E6 RID: 2278
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DCAPCompressConsumer")]
	public static extern void delete_DCAPCompressConsumer(HandleRef jarg1);

	// Token: 0x060008E7 RID: 2279
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessFileStart")]
	public static extern void DCAPCompressConsumer_ProcessFileStart(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008E8 RID: 2280
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessFileEnd")]
	public static extern void DCAPCompressConsumer_ProcessFileEnd(HandleRef jarg1);

	// Token: 0x060008E9 RID: 2281
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessBlockStart")]
	public static extern void DCAPCompressConsumer_ProcessBlockStart(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008EA RID: 2282
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessBlockEnd")]
	public static extern void DCAPCompressConsumer_ProcessBlockEnd(HandleRef jarg1, ulong jarg2);

	// Token: 0x060008EB RID: 2283
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessFrameNumber")]
	public static extern void DCAPCompressConsumer_ProcessFrameNumber(HandleRef jarg1, uint jarg2, ulong jarg3);

	// Token: 0x060008EC RID: 2284
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessMethodCall")]
	public static extern void DCAPCompressConsumer_ProcessMethodCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008ED RID: 2285
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessFunctionCall")]
	public static extern void DCAPCompressConsumer_ProcessFunctionCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008EE RID: 2286
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_ProcessMetadata")]
	public static extern void DCAPCompressConsumer_ProcessMetadata(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008EF RID: 2287
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_IsInitialized")]
	public static extern bool DCAPCompressConsumer_IsInitialized(HandleRef jarg1);

	// Token: 0x060008F0 RID: 2288
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_InitializeDecompression")]
	public static extern int DCAPCompressConsumer_InitializeDecompression(HandleRef jarg1);

	// Token: 0x060008F1 RID: 2289
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_InitializeCompression")]
	public static extern int DCAPCompressConsumer_InitializeCompression(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x060008F2 RID: 2290
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_GetApiCallMaxSize")]
	public static extern uint DCAPCompressConsumer_GetApiCallMaxSize(HandleRef jarg1);

	// Token: 0x060008F3 RID: 2291
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_GetApiCallExpandedCount")]
	public static extern uint DCAPCompressConsumer_GetApiCallExpandedCount(HandleRef jarg1);

	// Token: 0x060008F4 RID: 2292
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_DecompressionEnabled")]
	public static extern bool DCAPCompressConsumer_DecompressionEnabled(HandleRef jarg1);

	// Token: 0x060008F5 RID: 2293
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_CompressionEnabled")]
	public static extern bool DCAPCompressConsumer_CompressionEnabled(HandleRef jarg1);

	// Token: 0x060008F6 RID: 2294
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_GetCompressionAlgorithm")]
	public static extern int DCAPCompressConsumer_GetCompressionAlgorithm(HandleRef jarg1);

	// Token: 0x060008F7 RID: 2295
	[DllImport("libDCAP", EntryPoint = "CSharp_new_DCAPStripConsumer")]
	public static extern IntPtr new_DCAPStripConsumer(HandleRef jarg1);

	// Token: 0x060008F8 RID: 2296
	[DllImport("libDCAP", EntryPoint = "CSharp_delete_DCAPStripConsumer")]
	public static extern void delete_DCAPStripConsumer(HandleRef jarg1);

	// Token: 0x060008F9 RID: 2297
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPStripConsumer_ProcessFileStart")]
	public static extern void DCAPStripConsumer_ProcessFileStart(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060008FA RID: 2298
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPStripConsumer_ProcessMethodCall")]
	public static extern void DCAPStripConsumer_ProcessMethodCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008FB RID: 2299
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPStripConsumer_ProcessFunctionCall")]
	public static extern void DCAPStripConsumer_ProcessFunctionCall(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008FC RID: 2300
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPStripConsumer_ProcessMetadata")]
	public static extern void DCAPStripConsumer_ProcessMetadata(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060008FD RID: 2301
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLAdapter_SWIGUpcast")]
	public static extern IntPtr EGLAdapter_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060008FE RID: 2302
	[DllImport("libDCAP", EntryPoint = "CSharp_GLAdapter_SWIGUpcast")]
	public static extern IntPtr GLAdapter_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060008FF RID: 2303
	[DllImport("libDCAP", EntryPoint = "CSharp_EGLDecoder_SWIGUpcast")]
	public static extern IntPtr EGLDecoder_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06000900 RID: 2304
	[DllImport("libDCAP", EntryPoint = "CSharp_GLDecoder_SWIGUpcast")]
	public static extern IntPtr GLDecoder_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06000901 RID: 2305
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioReader_SWIGSmartPtrUpcast")]
	public static extern IntPtr StdioReader_SWIGSmartPtrUpcast(IntPtr jarg1);

	// Token: 0x06000902 RID: 2306
	[DllImport("libDCAP", EntryPoint = "CSharp_StdioWriter_SWIGSmartPtrUpcast")]
	public static extern IntPtr StdioWriter_SWIGSmartPtrUpcast(IntPtr jarg1);

	// Token: 0x06000903 RID: 2307
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPCompressConsumer_SWIGUpcast")]
	public static extern IntPtr DCAPCompressConsumer_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06000904 RID: 2308
	[DllImport("libDCAP", EntryPoint = "CSharp_DCAPStripConsumer_SWIGUpcast")]
	public static extern IntPtr DCAPStripConsumer_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0400099E RID: 2462
	protected static libDCAPPINVOKE.SWIGExceptionHelper swigExceptionHelper = new libDCAPPINVOKE.SWIGExceptionHelper();

	// Token: 0x0400099F RID: 2463
	protected static libDCAPPINVOKE.SWIGStringHelper swigStringHelper = new libDCAPPINVOKE.SWIGStringHelper();

	// Token: 0x0200027E RID: 638
	protected class SWIGExceptionHelper
	{
		// Token: 0x060012B3 RID: 4787
		[DllImport("libDCAP")]
		public static extern void SWIGRegisterExceptionCallbacks_libDCAP(libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemExceptionDelegate);

		// Token: 0x060012B4 RID: 4788
		[DllImport("libDCAP", EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_libDCAP")]
		public static extern void SWIGRegisterExceptionCallbacksArgument_libDCAP(libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate);

		// Token: 0x060012B5 RID: 4789 RVA: 0x0001A354 File Offset: 0x00018554
		private static void SetPendingApplicationException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new ApplicationException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0001A366 File Offset: 0x00018566
		private static void SetPendingArithmeticException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new ArithmeticException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0001A378 File Offset: 0x00018578
		private static void SetPendingDivideByZeroException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new DivideByZeroException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0001A38A File Offset: 0x0001858A
		private static void SetPendingIndexOutOfRangeException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new IndexOutOfRangeException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0001A39C File Offset: 0x0001859C
		private static void SetPendingInvalidCastException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new InvalidCastException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0001A3AE File Offset: 0x000185AE
		private static void SetPendingInvalidOperationException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new InvalidOperationException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0001A3C0 File Offset: 0x000185C0
		private static void SetPendingIOException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new IOException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0001A3D2 File Offset: 0x000185D2
		private static void SetPendingNullReferenceException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new NullReferenceException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0001A3E4 File Offset: 0x000185E4
		private static void SetPendingOutOfMemoryException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new OutOfMemoryException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0001A3F6 File Offset: 0x000185F6
		private static void SetPendingOverflowException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new OverflowException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0001A408 File Offset: 0x00018608
		private static void SetPendingSystemException(string message)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new SystemException(message, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0001A41A File Offset: 0x0001861A
		private static void SetPendingArgumentException(string message, string paramName)
		{
			libDCAPPINVOKE.SWIGPendingException.Set(new ArgumentException(message, paramName, libDCAPPINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0001A430 File Offset: 0x00018630
		private static void SetPendingArgumentNullException(string message, string paramName)
		{
			Exception ex = libDCAPPINVOKE.SWIGPendingException.Retrieve();
			if (ex != null)
			{
				message = message + " Inner Exception: " + ex.Message;
			}
			libDCAPPINVOKE.SWIGPendingException.Set(new ArgumentNullException(paramName, message));
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0001A468 File Offset: 0x00018668
		private static void SetPendingArgumentOutOfRangeException(string message, string paramName)
		{
			Exception ex = libDCAPPINVOKE.SWIGPendingException.Retrieve();
			if (ex != null)
			{
				message = message + " Inner Exception: " + ex.Message;
			}
			libDCAPPINVOKE.SWIGPendingException.Set(new ArgumentOutOfRangeException(paramName, message));
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0001A4A0 File Offset: 0x000186A0
		static SWIGExceptionHelper()
		{
			libDCAPPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacks_libDCAP(libDCAPPINVOKE.SWIGExceptionHelper.applicationDelegate, libDCAPPINVOKE.SWIGExceptionHelper.arithmeticDelegate, libDCAPPINVOKE.SWIGExceptionHelper.divideByZeroDelegate, libDCAPPINVOKE.SWIGExceptionHelper.indexOutOfRangeDelegate, libDCAPPINVOKE.SWIGExceptionHelper.invalidCastDelegate, libDCAPPINVOKE.SWIGExceptionHelper.invalidOperationDelegate, libDCAPPINVOKE.SWIGExceptionHelper.ioDelegate, libDCAPPINVOKE.SWIGExceptionHelper.nullReferenceDelegate, libDCAPPINVOKE.SWIGExceptionHelper.outOfMemoryDelegate, libDCAPPINVOKE.SWIGExceptionHelper.overflowDelegate, libDCAPPINVOKE.SWIGExceptionHelper.systemDelegate);
			libDCAPPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacksArgument_libDCAP(libDCAPPINVOKE.SWIGExceptionHelper.argumentDelegate, libDCAPPINVOKE.SWIGExceptionHelper.argumentNullDelegate, libDCAPPINVOKE.SWIGExceptionHelper.argumentOutOfRangeDelegate);
		}

		// Token: 0x04000BB1 RID: 2993
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingApplicationException);

		// Token: 0x04000BB2 RID: 2994
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingArithmeticException);

		// Token: 0x04000BB3 RID: 2995
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingDivideByZeroException);

		// Token: 0x04000BB4 RID: 2996
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingIndexOutOfRangeException);

		// Token: 0x04000BB5 RID: 2997
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingInvalidCastException);

		// Token: 0x04000BB6 RID: 2998
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingInvalidOperationException);

		// Token: 0x04000BB7 RID: 2999
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingIOException);

		// Token: 0x04000BB8 RID: 3000
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingNullReferenceException);

		// Token: 0x04000BB9 RID: 3001
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingOutOfMemoryException);

		// Token: 0x04000BBA RID: 3002
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingOverflowException);

		// Token: 0x04000BBB RID: 3003
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingSystemException);

		// Token: 0x04000BBC RID: 3004
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingArgumentException);

		// Token: 0x04000BBD RID: 3005
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingArgumentNullException);

		// Token: 0x04000BBE RID: 3006
		private static libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate = new libDCAPPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(libDCAPPINVOKE.SWIGExceptionHelper.SetPendingArgumentOutOfRangeException);

		// Token: 0x02000297 RID: 663
		// (Invoke) Token: 0x06001326 RID: 4902
		public delegate void ExceptionDelegate(string message);

		// Token: 0x02000298 RID: 664
		// (Invoke) Token: 0x0600132A RID: 4906
		public delegate void ExceptionArgumentDelegate(string message, string paramName);
	}

	// Token: 0x0200027F RID: 639
	public class SWIGPendingException
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0001A5EC File Offset: 0x000187EC
		public static bool Pending
		{
			get
			{
				bool flag = false;
				if (libDCAPPINVOKE.SWIGPendingException.numExceptionsPending > 0 && libDCAPPINVOKE.SWIGPendingException.pendingException != null)
				{
					flag = true;
				}
				return flag;
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0001A610 File Offset: 0x00018810
		public static void Set(Exception e)
		{
			if (libDCAPPINVOKE.SWIGPendingException.pendingException != null)
			{
				throw new ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + libDCAPPINVOKE.SWIGPendingException.pendingException.ToString() + ")", e);
			}
			libDCAPPINVOKE.SWIGPendingException.pendingException = e;
			Type typeFromHandle = typeof(libDCAPPINVOKE);
			lock (typeFromHandle)
			{
				libDCAPPINVOKE.SWIGPendingException.numExceptionsPending++;
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0001A688 File Offset: 0x00018888
		public static Exception Retrieve()
		{
			Exception ex = null;
			if (libDCAPPINVOKE.SWIGPendingException.numExceptionsPending > 0 && libDCAPPINVOKE.SWIGPendingException.pendingException != null)
			{
				ex = libDCAPPINVOKE.SWIGPendingException.pendingException;
				libDCAPPINVOKE.SWIGPendingException.pendingException = null;
				Type typeFromHandle = typeof(libDCAPPINVOKE);
				lock (typeFromHandle)
				{
					libDCAPPINVOKE.SWIGPendingException.numExceptionsPending--;
				}
			}
			return ex;
		}

		// Token: 0x04000BBF RID: 3007
		[ThreadStatic]
		private static Exception pendingException;

		// Token: 0x04000BC0 RID: 3008
		private static int numExceptionsPending;
	}

	// Token: 0x02000280 RID: 640
	protected class SWIGStringHelper
	{
		// Token: 0x060012C9 RID: 4809
		[DllImport("libDCAP")]
		public static extern void SWIGRegisterStringCallback_libDCAP(libDCAPPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate);

		// Token: 0x060012CA RID: 4810 RVA: 0x0001A6F0 File Offset: 0x000188F0
		private static string CreateString(string cString)
		{
			return cString;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0001A6F3 File Offset: 0x000188F3
		static SWIGStringHelper()
		{
			libDCAPPINVOKE.SWIGStringHelper.SWIGRegisterStringCallback_libDCAP(libDCAPPINVOKE.SWIGStringHelper.stringDelegate);
		}

		// Token: 0x04000BC1 RID: 3009
		private static libDCAPPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate = new libDCAPPINVOKE.SWIGStringHelper.SWIGStringDelegate(libDCAPPINVOKE.SWIGStringHelper.CreateString);

		// Token: 0x02000299 RID: 665
		// (Invoke) Token: 0x0600132E RID: 4910
		public delegate string SWIGStringDelegate(string message);
	}
}
