using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x020002EA RID: 746
	public class Viewer3DController : IViewController
	{
		// Token: 0x06000F19 RID: 3865 RVA: 0x0002EBD8 File Offset: 0x0002CDD8
		public Viewer3DController(IViewer3DView view)
		{
			this.m_view = view;
			Viewer3DEvents viewer3DEvents = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents.OnDelete = (EventHandler<EventArgs>)Delegate.Combine(viewer3DEvents.OnDelete, new EventHandler<EventArgs>(this.OnDelete));
			Viewer3DEvents viewer3DEvents2 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents2.OnRealize = (EventHandler<Viewer3DOnRealizeArgs>)Delegate.Combine(viewer3DEvents2.OnRealize, new EventHandler<Viewer3DOnRealizeArgs>(this.OnRealize));
			Viewer3DEvents viewer3DEvents3 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents3.OnResize = (EventHandler<Viewer3DOnResizeArgs>)Delegate.Combine(viewer3DEvents3.OnResize, new EventHandler<Viewer3DOnResizeArgs>(this.OnResize));
			Viewer3DEvents viewer3DEvents4 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents4.OnMouseMove = (EventHandler<Viewer3DOnMouseMoveArgs>)Delegate.Combine(viewer3DEvents4.OnMouseMove, new EventHandler<Viewer3DOnMouseMoveArgs>(this.OnMouseMove));
			Viewer3DEvents viewer3DEvents5 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents5.OnButtonInput = (EventHandler<Viewer3DOnButtonInputArgs>)Delegate.Combine(viewer3DEvents5.OnButtonInput, new EventHandler<Viewer3DOnButtonInputArgs>(this.OnButtonInput));
			Viewer3DEvents viewer3DEvents6 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents6.OnKeyInput = (EventHandler<Viewer3DOnKeyInputArgs>)Delegate.Combine(viewer3DEvents6.OnKeyInput, new EventHandler<Viewer3DOnKeyInputArgs>(this.OnKeyInput));
			Viewer3DEvents viewer3DEvents7 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents7.RequestLoadAccelerationStructure = (EventHandler<Viewer3DLoadASArgs>)Delegate.Combine(viewer3DEvents7.RequestLoadAccelerationStructure, new EventHandler<Viewer3DLoadASArgs>(this.OnRequestLoadAccelerationStructure));
			Viewer3DEvents viewer3DEvents8 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents8.DisplayAccelerationStructure = (EventHandler<Viewer3DDisplayASArgs>)Delegate.Combine(viewer3DEvents8.DisplayAccelerationStructure, new EventHandler<Viewer3DDisplayASArgs>(this.OnDisplayAccelerationStructure));
			Viewer3DEvents viewer3DEvents9 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents9.RenderOptionChanged = (EventHandler<Viewer3DRenderOptionChangedArgs>)Delegate.Combine(viewer3DEvents9.RenderOptionChanged, new EventHandler<Viewer3DRenderOptionChangedArgs>(this.OnRenderOptionChanged));
			Viewer3DEvents viewer3DEvents10 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents10.CameraOptionChanged = (EventHandler<Viewer3DCameraOptionChangedArgs>)Delegate.Combine(viewer3DEvents10.CameraOptionChanged, new EventHandler<Viewer3DCameraOptionChangedArgs>(this.OnCameraOptionChanged));
			Viewer3DEvents viewer3DEvents11 = SdpApp.EventsManager.Viewer3DEvents;
			viewer3DEvents11.CameraCoordinateChanged = (EventHandler<Viewer3DCameraCoordinateChangedArgs>)Delegate.Combine(viewer3DEvents11.CameraCoordinateChanged, new EventHandler<Viewer3DCameraCoordinateChangedArgs>(this.OnCameraCoordinateChanged));
			this.m_lastMouseX = 0;
			this.m_lastMouseY = 0;
			this.m_cameraOptions.CameraType = Viewer3DController.CameraType.Fly;
			this.m_cameraOptions.UpVector = Viewer3DController.UpVector.Y;
			this.m_cameraOptions.InvertVertical = 1U;
			this.m_cameraOptions.InvertHorizontal = 0U;
			this.m_cameraOptions.ProjectionType = Viewer3DController.ProjectionType.Perspective;
			this.m_cameraOptions.FovDeg = 60f;
			this.m_cameraOptions.NearPlane = 0.05f;
			this.m_cameraOptions.FarPlane = 100f;
			this.m_renderOptions.ColorMode = Viewer3DController.ColorMode.RandomPerBLAS;
			this.m_renderOptions.CullMode = Viewer3DController.CullMode.None;
			this.m_renderOptions.LightingMode = Viewer3DController.LightingMode.Flat;
			this.m_renderOptions.PolygonMode = Viewer3DController.PolygonMode.Fill;
			this.m_renderOptions.BoundingVolumeMode = Viewer3DController.BoundingVolumeMode.None;
			this.m_cameraPositionUpdateCallback = new Viewer3DController.PFN_CameraPositionUpdateCallback(this.CameraPositionUpdate);
			this.m_cameraFarPlaneUpdateCallback = new Viewer3DController.PFN_CameraFarPlaneUpdateCallback(this.CameraFarPlaneUpdate);
			this.m_initialized = false;
			this.m_toLoadASID = ulong.MaxValue;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0002EEC0 File Offset: 0x0002D0C0
		private void OnDelete(object sender, EventArgs args)
		{
			if (this.m_initialized)
			{
				Viewer3DController.Viewer3D_DestroyRenderContext();
				this.m_initialized = false;
			}
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0002EED8 File Offset: 0x0002D0D8
		private void OnRealize(object sender, Viewer3DOnRealizeArgs args)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			string displayTypeName = args.DisplayTypeName;
			Viewer3DController.WSIBackend wsibackend;
			if (!(displayTypeName == "GdkWin32Display"))
			{
				if (!(displayTypeName == "GdkWaylandDisplay"))
				{
					if (!(displayTypeName == "GdkX11Display"))
					{
						new ShowMessageDialogCommand
						{
							IconType = IconType.Error,
							Message = "WSI not supported"
						}.Execute();
						this.m_initialized = false;
						return;
					}
					wsibackend = Viewer3DController.WSIBackend.X11;
					intPtr = Viewer3DController.gdk_x11_window_get_xid(args.WindowHandle);
					intPtr2 = Viewer3DController.gdk_x11_display_get_xdisplay(args.DisplayHandle);
				}
				else
				{
					wsibackend = Viewer3DController.WSIBackend.Wayland;
					intPtr = Viewer3DController.gdk_wayland_window_get_wl_surface(args.WindowHandle);
					intPtr2 = Viewer3DController.gdk_wayland_display_get_wl_display(args.DisplayHandle);
				}
			}
			else
			{
				wsibackend = Viewer3DController.WSIBackend.Win32;
				intPtr = Viewer3DController.gdk_win32_window_get_handle(args.WindowHandle);
			}
			if (this.m_initialized)
			{
				Viewer3DController.Viewer3D_UpdateRenderWindow((int)wsibackend, intPtr, intPtr2, args.Width, args.Height);
				return;
			}
			if (Viewer3DController.LoadProgress != null)
			{
				Viewer3DController.LoadProgress.CurrentValue = 0.25;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(Viewer3DController.LoadProgress));
			}
			Viewer3DController.ErrorCode errorCode = (Viewer3DController.ErrorCode)Viewer3DController.Viewer3D_InitializeRenderContext();
			if (errorCode != Viewer3DController.ErrorCode.NoError)
			{
				ShowMessageDialogCommand showMessageDialogCommand = new ShowMessageDialogCommand();
				showMessageDialogCommand.IconType = IconType.Error;
				switch (errorCode)
				{
				case Viewer3DController.ErrorCode.LoadFailed:
					showMessageDialogCommand.Message = "Vulkan load failed. Vulkan loader not found or Vulkan runtime not properly installed.";
					goto IL_0164;
				case Viewer3DController.ErrorCode.InvalidVersion:
					showMessageDialogCommand.Message = "Vulkan 1.3 or higher is required. Please update your Vulkan SDK or driver.";
					goto IL_0164;
				}
				showMessageDialogCommand.Message = "Vulkan initialization failed.";
				IL_0164:
				showMessageDialogCommand.Execute();
				this.m_initialized = false;
				return;
			}
			Viewer3DController.Viewer3D_InitializeRenderWindow((int)wsibackend, intPtr, intPtr2, args.Width, args.Height);
			IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf<Viewer3DController.CameraOptions>(this.m_cameraOptions));
			IntPtr intPtr4 = Marshal.AllocHGlobal(Marshal.SizeOf<Viewer3DController.RenderOptions>(this.m_renderOptions));
			Marshal.StructureToPtr<Viewer3DController.CameraOptions>(this.m_cameraOptions, intPtr3, false);
			Marshal.StructureToPtr<Viewer3DController.RenderOptions>(this.m_renderOptions, intPtr4, false);
			Viewer3DController.Viewer3D_InitializeResources(intPtr3, intPtr4);
			Marshal.FreeHGlobal(intPtr3);
			Marshal.FreeHGlobal(intPtr4);
			Viewer3DController.Viewer3D_RegisterCameraPositionCallback(this.m_cameraPositionUpdateCallback);
			Viewer3DController.Viewer3D_RegisterCameraFarPlaneCallback(this.m_cameraFarPlaneUpdateCallback);
			this.m_initialized = true;
			if (Viewer3DController.LoadProgress != null)
			{
				Viewer3DController.LoadProgress.CurrentValue = 0.5;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(Viewer3DController.LoadProgress));
			}
			if (this.m_toLoadASID != 18446744073709551615UL)
			{
				Viewer3DLoadASArgs viewer3DLoadASArgs = new Viewer3DLoadASArgs();
				viewer3DLoadASArgs.AccelerationStructureID = this.m_toLoadASID;
				SdpApp.EventsManager.Raise<Viewer3DLoadASArgs>(SdpApp.EventsManager.Viewer3DEvents.LoadAccelerationStructure, this, viewer3DLoadASArgs);
				this.m_toLoadASID = ulong.MaxValue;
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0002F160 File Offset: 0x0002D360
		private void OnResize(object sender, Viewer3DOnResizeArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			Viewer3DController.Viewer3D_ResizeWindow(args.Width, args.Height);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0002F17C File Offset: 0x0002D37C
		private void OnMouseMove(object sender, Viewer3DOnMouseMoveArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			int num = this.m_lastMouseX - args.X;
			int num2 = this.m_lastMouseY - args.Y;
			Viewer3DController.Viewer3D_MouseMove(num, num2);
			this.m_lastMouseX = args.X;
			this.m_lastMouseY = args.Y;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0002F1CD File Offset: 0x0002D3CD
		private void OnButtonInput(object sender, Viewer3DOnButtonInputArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			Viewer3DController.Viewer3D_MouseButtonInput((int)args.Button, (int)args.Type);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002F1E9 File Offset: 0x0002D3E9
		private void OnKeyInput(object sender, Viewer3DOnKeyInputArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			Viewer3DController.Viewer3D_KeyInput((int)args.Key, (int)args.Type);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0002F208 File Offset: 0x0002D408
		private void OnRequestLoadAccelerationStructure(object sender, Viewer3DLoadASArgs args)
		{
			Viewer3DController.LoadProgress = new ProgressObject();
			Viewer3DController.LoadProgress.CurrentValue = 0.05;
			Viewer3DController.LoadProgress.Title = "Viewer 3D";
			Viewer3DController.LoadProgress.Description = "Load Acceleration Structure";
			SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(Viewer3DController.LoadProgress));
			if (this.m_initialized)
			{
				SdpApp.EventsManager.Raise<Viewer3DLoadASArgs>(SdpApp.EventsManager.Viewer3DEvents.LoadAccelerationStructure, this, args);
				return;
			}
			this.m_toLoadASID = args.AccelerationStructureID;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0002F2A4 File Offset: 0x0002D4A4
		private void OnDisplayAccelerationStructure(object sender, Viewer3DDisplayASArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			if (Viewer3DController.LoadProgress != null)
			{
				Viewer3DController.LoadProgress.CurrentValue = 0.75;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(Viewer3DController.LoadProgress));
			}
			Viewer3DController.Viewer3D_StopRendering();
			Viewer3DController.Viewer3D_StartBuildingAccelerationStructure();
			GCHandle gchandle = GCHandle.Alloc(args.Tlas.Item2, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			Viewer3DController.Viewer3D_ImportAccelerationStructureBuffer(args.Tlas.Item1, intPtr, (uint)args.Tlas.Item2.Length);
			gchandle.Free();
			foreach (KeyValuePair<ulong, byte[]> keyValuePair in args.Blases)
			{
				gchandle = GCHandle.Alloc(keyValuePair.Value, GCHandleType.Pinned);
				IntPtr intPtr2 = gchandle.AddrOfPinnedObject();
				Viewer3DController.Viewer3D_ImportAccelerationStructureBuffer(keyValuePair.Key, intPtr2, (uint)keyValuePair.Value.Length);
				gchandle.Free();
			}
			Viewer3DController.Viewer3D_StopBuildingAccelerationStructure();
			Viewer3DController.Viewer3D_StartRendering();
			if (Viewer3DController.LoadProgress != null)
			{
				Viewer3DController.LoadProgress.CurrentValue = 1.0;
				SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(Viewer3DController.LoadProgress));
				Viewer3DController.LoadProgress = null;
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0002F408 File Offset: 0x0002D608
		private void OnRenderOptionChanged(object sender, Viewer3DRenderOptionChangedArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			switch (args.Type)
			{
			case RenderOptionType.ColorMode:
				this.m_renderOptions.ColorMode = (Viewer3DController.ColorMode)args.Value;
				break;
			case RenderOptionType.CullMode:
				this.m_renderOptions.CullMode = (Viewer3DController.CullMode)args.Value;
				break;
			case RenderOptionType.LightingMode:
				this.m_renderOptions.LightingMode = (Viewer3DController.LightingMode)args.Value;
				break;
			case RenderOptionType.PolygonMode:
				this.m_renderOptions.PolygonMode = (Viewer3DController.PolygonMode)args.Value;
				break;
			case RenderOptionType.BoundingVolumeMode:
				this.m_renderOptions.BoundingVolumeMode = (Viewer3DController.BoundingVolumeMode)args.Value;
				break;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Viewer3DController.RenderOptions>(this.m_renderOptions));
			Marshal.StructureToPtr<Viewer3DController.RenderOptions>(this.m_renderOptions, intPtr, false);
			Viewer3DController.Viewer3D_UpdateRenderOptions(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0002F4C8 File Offset: 0x0002D6C8
		private void OnCameraOptionChanged(object sender, Viewer3DCameraOptionChangedArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			switch (args.Type)
			{
			case CameraOptionType.CameraType:
				this.m_cameraOptions.CameraType = (Viewer3DController.CameraType)args.Value.Uint;
				break;
			case CameraOptionType.ProjectionType:
				this.m_cameraOptions.ProjectionType = (Viewer3DController.ProjectionType)args.Value.Uint;
				break;
			case CameraOptionType.UpVector:
				this.m_cameraOptions.UpVector = (Viewer3DController.UpVector)args.Value.Uint;
				break;
			case CameraOptionType.InvertVertical:
				this.m_cameraOptions.InvertVertical = args.Value.Uint;
				break;
			case CameraOptionType.InvertHorizontal:
				this.m_cameraOptions.InvertHorizontal = args.Value.Uint;
				break;
			case CameraOptionType.FOV:
				this.m_cameraOptions.FovDeg = args.Value.Float;
				break;
			case CameraOptionType.NearPlane:
				this.m_cameraOptions.NearPlane = args.Value.Float;
				break;
			case CameraOptionType.FarPlane:
				this.m_cameraOptions.FarPlane = args.Value.Float;
				break;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Viewer3DController.CameraOptions>(this.m_cameraOptions));
			Marshal.StructureToPtr<Viewer3DController.CameraOptions>(this.m_cameraOptions, intPtr, false);
			Viewer3DController.Viewer3D_UpdateCameraOptions(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0002F600 File Offset: 0x0002D800
		private void OnCameraCoordinateChanged(object sender, Viewer3DCameraCoordinateChangedArgs args)
		{
			if (!this.m_initialized)
			{
				return;
			}
			switch (args.Coordinate)
			{
			case Coordinate.X:
				this.m_cameraPosition[0] = args.Value;
				break;
			case Coordinate.Y:
				this.m_cameraPosition[1] = args.Value;
				break;
			case Coordinate.Z:
				this.m_cameraPosition[2] = args.Value;
				break;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(12);
			Marshal.Copy(this.m_cameraPosition, 0, intPtr, 3);
			Viewer3DController.Viewer3D_UpdateCameraPosition(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0002F684 File Offset: 0x0002D884
		private void CameraPositionUpdate(float x, float y, float z)
		{
			this.m_cameraPosition[0] = x;
			this.m_cameraPosition[1] = y;
			this.m_cameraPosition[2] = z;
			Viewer3DCameraPositionArgs viewer3DCameraPositionArgs = new Viewer3DCameraPositionArgs();
			viewer3DCameraPositionArgs.CameraPosition = this.m_cameraPosition;
			SdpApp.EventsManager.Raise<Viewer3DCameraPositionArgs>(SdpApp.EventsManager.Viewer3DEvents.CameraPositionUpdated, this, viewer3DCameraPositionArgs);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0002F6DC File Offset: 0x0002D8DC
		private void CameraFarPlaneUpdate(float currentFarPlane, float minFarPlane, float maxFarPlane)
		{
			this.m_cameraOptions.FarPlane = currentFarPlane;
			Viewer3DCameraFarPlaneArgs viewer3DCameraFarPlaneArgs = new Viewer3DCameraFarPlaneArgs();
			viewer3DCameraFarPlaneArgs.FarPlane = currentFarPlane;
			viewer3DCameraFarPlaneArgs.MinFarPlane = minFarPlane;
			viewer3DCameraFarPlaneArgs.MaxFarPlane = maxFarPlane;
			SdpApp.EventsManager.Raise<Viewer3DCameraFarPlaneArgs>(SdpApp.EventsManager.Viewer3DEvents.CameraFarPlaneUpdated, this, viewer3DCameraFarPlaneArgs);
		}

		// Token: 0x06000F27 RID: 3879
		[DllImport("libgdk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gdk_win32_window_get_handle(IntPtr window);

		// Token: 0x06000F28 RID: 3880
		[DllImport("libgdk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gdk_wayland_window_get_wl_surface(IntPtr window);

		// Token: 0x06000F29 RID: 3881
		[DllImport("libgdk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gdk_wayland_display_get_wl_display(IntPtr display);

		// Token: 0x06000F2A RID: 3882
		[DllImport("libgdk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gdk_x11_window_get_xid(IntPtr window);

		// Token: 0x06000F2B RID: 3883
		[DllImport("libgdk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr display);

		// Token: 0x06000F2C RID: 3884
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_InitializeRenderWindow(int backend, IntPtr windowHandle, IntPtr displayHandle, uint width, uint height);

		// Token: 0x06000F2D RID: 3885
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_UpdateRenderWindow(int backend, IntPtr windowHandle, IntPtr displayHandle, uint width, uint height);

		// Token: 0x06000F2E RID: 3886
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int Viewer3D_InitializeRenderContext();

		// Token: 0x06000F2F RID: 3887
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_DestroyRenderContext();

		// Token: 0x06000F30 RID: 3888
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_InitializeResources(IntPtr cameraOptions, IntPtr renderOptions);

		// Token: 0x06000F31 RID: 3889
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_StartRendering();

		// Token: 0x06000F32 RID: 3890
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_StopRendering();

		// Token: 0x06000F33 RID: 3891
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_ResizeWindow(uint width, uint height);

		// Token: 0x06000F34 RID: 3892
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_MouseMove(int deltaX, int deltaxY);

		// Token: 0x06000F35 RID: 3893
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_MouseButtonInput(int button, int inputType);

		// Token: 0x06000F36 RID: 3894
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_KeyInput(int key, int inputType);

		// Token: 0x06000F37 RID: 3895
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_UpdateCameraOptions(IntPtr cameraOptions);

		// Token: 0x06000F38 RID: 3896
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_UpdateCameraPosition(IntPtr cameraPosition);

		// Token: 0x06000F39 RID: 3897
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_RegisterCameraPositionCallback(Viewer3DController.PFN_CameraPositionUpdateCallback positionCallback);

		// Token: 0x06000F3A RID: 3898
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_RegisterCameraFarPlaneCallback(Viewer3DController.PFN_CameraFarPlaneUpdateCallback callback);

		// Token: 0x06000F3B RID: 3899
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_UpdateRenderOptions(IntPtr renderOptions);

		// Token: 0x06000F3C RID: 3900
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Viewer3D_ImportAccelerationStructureBuffer(ulong resourceID, IntPtr buffer, uint size);

		// Token: 0x06000F3D RID: 3901
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool Viewer3D_StartBuildingAccelerationStructure();

		// Token: 0x06000F3E RID: 3902
		[DllImport("Viewer3D.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool Viewer3D_StopBuildingAccelerationStructure();

		// Token: 0x06000F3F RID: 3903 RVA: 0x0002F72C File Offset: 0x0002D92C
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0002F75B File Offset: 0x0002D95B
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x04000A58 RID: 2648
		private int m_lastMouseX;

		// Token: 0x04000A59 RID: 2649
		private int m_lastMouseY;

		// Token: 0x04000A5A RID: 2650
		private Viewer3DController.CameraOptions m_cameraOptions;

		// Token: 0x04000A5B RID: 2651
		private Viewer3DController.RenderOptions m_renderOptions;

		// Token: 0x04000A5C RID: 2652
		private float[] m_cameraPosition = new float[3];

		// Token: 0x04000A5D RID: 2653
		private bool m_initialized;

		// Token: 0x04000A5E RID: 2654
		private ulong m_toLoadASID;

		// Token: 0x04000A5F RID: 2655
		private static ProgressObject LoadProgress;

		// Token: 0x04000A60 RID: 2656
		private Viewer3DController.PFN_CameraPositionUpdateCallback m_cameraPositionUpdateCallback;

		// Token: 0x04000A61 RID: 2657
		private Viewer3DController.PFN_CameraFarPlaneUpdateCallback m_cameraFarPlaneUpdateCallback;

		// Token: 0x04000A62 RID: 2658
		private const string m_gdkLocation = "libgdk-3-0.dll";

		// Token: 0x04000A63 RID: 2659
		private const string m_viewer3DLocation = "Viewer3D.dll";

		// Token: 0x04000A64 RID: 2660
		private IViewer3DView m_view;

		// Token: 0x020003E2 RID: 994
		// (Invoke) Token: 0x060012B6 RID: 4790
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void PFN_CameraPositionUpdateCallback(float x, float y, float z);

		// Token: 0x020003E3 RID: 995
		// (Invoke) Token: 0x060012BA RID: 4794
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void PFN_CameraFarPlaneUpdateCallback(float farPlane, float farPlaneMin, float farPlaneMax);

		// Token: 0x020003E4 RID: 996
		private enum CameraType
		{
			// Token: 0x04000D95 RID: 3477
			Fly,
			// Token: 0x04000D96 RID: 3478
			Orbit
		}

		// Token: 0x020003E5 RID: 997
		private enum ProjectionType
		{
			// Token: 0x04000D98 RID: 3480
			Perspective,
			// Token: 0x04000D99 RID: 3481
			Orthographic
		}

		// Token: 0x020003E6 RID: 998
		private enum UpVector
		{
			// Token: 0x04000D9B RID: 3483
			Y,
			// Token: 0x04000D9C RID: 3484
			Z
		}

		// Token: 0x020003E7 RID: 999
		private struct CameraOptions
		{
			// Token: 0x04000D9D RID: 3485
			public Viewer3DController.CameraType CameraType;

			// Token: 0x04000D9E RID: 3486
			public Viewer3DController.UpVector UpVector;

			// Token: 0x04000D9F RID: 3487
			public uint InvertVertical;

			// Token: 0x04000DA0 RID: 3488
			public uint InvertHorizontal;

			// Token: 0x04000DA1 RID: 3489
			public Viewer3DController.ProjectionType ProjectionType;

			// Token: 0x04000DA2 RID: 3490
			public float FovDeg;

			// Token: 0x04000DA3 RID: 3491
			public float NearPlane;

			// Token: 0x04000DA4 RID: 3492
			public float FarPlane;
		}

		// Token: 0x020003E8 RID: 1000
		private enum WSIBackend
		{
			// Token: 0x04000DA6 RID: 3494
			Win32,
			// Token: 0x04000DA7 RID: 3495
			Wayland,
			// Token: 0x04000DA8 RID: 3496
			X11,
			// Token: 0x04000DA9 RID: 3497
			Unknown
		}

		// Token: 0x020003E9 RID: 1001
		private enum ErrorCode
		{
			// Token: 0x04000DAB RID: 3499
			NoError,
			// Token: 0x04000DAC RID: 3500
			LoadFailed,
			// Token: 0x04000DAD RID: 3501
			InitializationFailed,
			// Token: 0x04000DAE RID: 3502
			InvalidVersion
		}

		// Token: 0x020003EA RID: 1002
		private enum ColorMode
		{
			// Token: 0x04000DB0 RID: 3504
			None,
			// Token: 0x04000DB1 RID: 3505
			RandomPerBLAS,
			// Token: 0x04000DB2 RID: 3506
			RandomPerInstance
		}

		// Token: 0x020003EB RID: 1003
		private enum CullMode
		{
			// Token: 0x04000DB4 RID: 3508
			None,
			// Token: 0x04000DB5 RID: 3509
			Back,
			// Token: 0x04000DB6 RID: 3510
			Front,
			// Token: 0x04000DB7 RID: 3511
			Both
		}

		// Token: 0x020003EC RID: 1004
		private enum LightingMode
		{
			// Token: 0x04000DB9 RID: 3513
			None,
			// Token: 0x04000DBA RID: 3514
			Flat
		}

		// Token: 0x020003ED RID: 1005
		private enum PolygonMode
		{
			// Token: 0x04000DBC RID: 3516
			Fill = 1,
			// Token: 0x04000DBD RID: 3517
			Wireframe,
			// Token: 0x04000DBE RID: 3518
			Both
		}

		// Token: 0x020003EE RID: 1006
		private enum BoundingVolumeMode
		{
			// Token: 0x04000DC0 RID: 3520
			None,
			// Token: 0x04000DC1 RID: 3521
			RootNodes,
			// Token: 0x04000DC2 RID: 3522
			InternalNodes,
			// Token: 0x04000DC3 RID: 3523
			All
		}

		// Token: 0x020003EF RID: 1007
		private struct RenderOptions
		{
			// Token: 0x04000DC4 RID: 3524
			public Viewer3DController.ColorMode ColorMode;

			// Token: 0x04000DC5 RID: 3525
			public Viewer3DController.CullMode CullMode;

			// Token: 0x04000DC6 RID: 3526
			public Viewer3DController.LightingMode LightingMode;

			// Token: 0x04000DC7 RID: 3527
			public Viewer3DController.PolygonMode PolygonMode;

			// Token: 0x04000DC8 RID: 3528
			public Viewer3DController.BoundingVolumeMode BoundingVolumeMode;
		}
	}
}
