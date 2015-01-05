/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ovr
{
	/// <summary>
	/// A 2D vector with integer components.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2i
	{
		public int x, y;

		public Vector2i(int _x, int _y)
		{
			x = _x;
			y = _y;
		}
	};

	/// <summary>
	/// A 2D size with integer components.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Sizei
	{
		public int w, h;

		public Sizei(int _w, int _h)
		{
			w = _w;
			h = _h;
		}
	};

	/// <summary>
	/// A 2D rectangle with a position and size.
	/// All components are integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Recti
	{
		public Vector2i Pos;
		public Sizei Size;
	};

	/// <summary>
	/// A quaternion rotation.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Quatf
	{
		public float x, y, z, w;

		public Quatf(float _x, float _y, float _z, float _w)
		{
			x = _x;
			y = _y;
			z = _z;
			w = _w;
		}
	};

	/// <summary>
	/// A 2D vector with float components.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2f
	{
		public float x, y;

		public Vector2f(float _x, float _y)
		{
			x = _x;
			y = _y;
		}
	};

	/// <summary>
	/// A 3D vector with float components.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3f
	{
		public float x, y, z;

		public Vector3f(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}
	};

	/// <summary>
	/// A 4x4 matrix with float elements.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix4f
	{
		public float[,] m;

		internal Matrix4f(Matrix4f_Raw raw)
		{
			this.m = new float[,] {
				{ raw.m00, raw.m01, raw.m02, raw.m03 },
				{ raw.m10, raw.m11, raw.m12, raw.m13 },
				{ raw.m20, raw.m21, raw.m22, raw.m23 },
				{ raw.m30, raw.m31, raw.m32, raw.m33 } };
		}
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct Matrix4f_Raw
	{
		public float m00;
		public float m01;
		public float m02;
		public float m03;

		public float m10;
		public float m11;
		public float m12;
		public float m13;

		public float m20;
		public float m21;
		public float m22;
		public float m23;

		public float m30;
		public float m31;
		public float m32;
		public float m33;
	};

	/// <summary>
	/// Position and orientation together.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Posef
	{
		public Quatf Orientation;
		public Vector3f Position;

		public Posef(Quatf q, Vector3f p)
		{
			Orientation = q;
			Position = p;
		}
	};

	/// <summary>
	/// A full pose (rigid body) configuration with first and second derivatives.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PoseStatef
	{
	    /// <summary>
        /// The body's position and orientation.
	    /// </summary>
		public Posef ThePose;
	    /// <summary>
        /// The body's angular velocity in radians per second.        
	    /// </summary>
		public Vector3f AngularVelocity;
	    /// <summary>
        /// The body's velocity in meters per second.   
	    /// </summary>
		public Vector3f LinearVelocity;
	    /// <summary>
        /// The body's angular acceleration in radians per second per second.    
	    /// </summary>
		public Vector3f AngularAcceleration;
	    /// <summary>
        /// The body's acceleration in meters per second per second.
	    /// </summary>
		public Vector3f LinearAcceleration;
	    /// <summary>
        /// Absolute time of this state sample.
	    /// </summary>
		public double TimeInSeconds;         
	};

	/// <summary>
	/// Field Of View (FOV) in tangent of the angle units.
	/// As an example, for a standard 90 degree vertical FOV, we would
	/// have: { UpTan = tan(90 degrees / 2), DownTan = tan(90 degrees / 2) }.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct FovPort
	{
		/// <summary>
		/// The tangent of the angle between the viewing vector and the top edge of the field of view.
		/// </summary>
		public float UpTan;
		/// <summary>
		/// The tangent of the angle between the viewing vector and the bottom edge of the field of view.
		/// </summary>
		public float DownTan;
		/// <summary>
		/// The tangent of the angle between the viewing vector and the left edge of the field of view.
		/// </summary>
		public float LeftTan;
		/// <summary>
		/// The tangent of the angle between the viewing vector and the right edge of the field of view.
		/// </summary>
		public float RightTan;
	};

	//-----------------------------------------------------------------------------------
	// ***** HMD Types

	/// <summary>
	/// Enumerates all HMD types that we support.
	/// </summary>
	public enum HmdType
	{
		None              = 0,
		DK1               = 3,
		DKHD              = 4,
		DK2               = 6,
		Other // Some HMD other then the one in the enumeration.
	};

	/// <summary>
	/// HMD capability bits reported by device.
	/// </summary>
	public enum HmdCaps
	{
		// Read-only flags.
		/// <summary>
		/// The HMD is plugged in and detected by the system.
		/// </summary>
		Present           = 0x0001,
        /// <summary>
		/// The HMD and its sensor are available for ownership use.
		/// i.e. it is not already owned by another application.
        /// </summary>
		Available         = 0x0002,
        /// <summary>
		/// Set to 'true' if we captured ownership of this HMD.
        /// </summary>
		Captured          = 0x0004,

		// These flags are intended for use with the new driver display mode.

        /// <summary>
		/// (read only) Means the display driver is in compatibility mode.
        /// </summary>
		ExtendDesktop     = 0x0008,

		// Modifiable flags (through ovrHmd_SetEnabledCaps).

        /// <summary>
		/// Disables mirroring of HMD output to the window. This may improve
		/// rendering performance slightly (only if 'ExtendDesktop' is off).
        /// </summary>
		NoMirrorToWindow  = 0x2000,

        /// <summary>
		/// Turns off HMD screen and output (only if 'ExtendDesktop' is off).
        /// </summary>
		DisplayOff        = 0x0040,

        /// <summary>
		/// HMD supports low persistence mode.
        /// </summary>
		LowPersistence    = 0x0080,
        /// <summary>
		/// Adjust prediction dynamically based on internally measured latency.
        /// </summary>
		DynamicPrediction = 0x0200,
        /// <summary>
		/// Support rendering without VSync for debugging.
        /// </summary>
		NoVSync           = 0x1000,

        /// <summary>
		/// These bits can be modified by ovrHmd_SetEnabledCaps.
        /// </summary>
		WritableMask      = 0x33F0,

        /// <summary>
		/// These flags are currently passed into the service. May change without notice.
        /// </summary>
		ServiceMask       = 0x23F0,
	};

    /// <summary>
	/// Tracking capability bits reported by the device.
	/// Used with ovrHmd_ConfigureTracking.
	/// </summary>
	public enum TrackingCaps
	{
        /// <summary>
		/// Supports orientation tracking (IMU).
        /// </summary>
		Orientation       = 0x0010,
        /// <summary>
		/// Supports yaw drift correction via a magnetometer or other means.
        /// </summary>
		MagYawCorrection  = 0x0020,
        /// <summary>
		/// Supports positional tracking.
        /// </summary>
		Position          = 0x0040,
        /// <summary>
		/// Overrides the other flags. Indicates that the application
		/// doesn't care about tracking settings. This is the internal
		/// default before ovrHmd_ConfigureTracking is called.
        /// </summary>
		Idle              = 0x0100,
	};

	/// <summary>
	/// Distortion capability bits reported by device.
	/// Used with ovrHmd_ConfigureRendering and ovrHmd_CreateDistortionMesh.
	/// </summary>
	public enum DistortionCaps
	{
        /// <summary>
		/// Supports chromatic aberration correction.
        /// </summary>
		Chromatic         = 0x01,
        /// <summary>
		/// Supports timewarp.
        /// </summary>
		TimeWarp          = 0x02,
        /// <summary>
		/// Supports vignetting around the edges of the view.
        /// </summary>
		Vignette          = 0x08,
        /// <summary>
		/// Do not save and restore the graphics state when rendering distortion.
        /// </summary>
		NoRestore         = 0x10,
        /// <summary>
		/// Flip the vertical texture coordinate of input images.
        /// </summary>
		FlipInput         = 0x20,
        /// <summary>
		/// Assume input images are in sRGB gamma-corrected color space.
        /// </summary>
		SRGB              = 0x40,
        /// <summary>
		/// Overdrive brightness transitions to reduce artifacts on DK2+ displays
        /// </summary>
		Overdrive         = 0x80,
        /// <summary>
		/// High-quality sampling of distortion buffer for anti-aliasing
        /// </summary>
		HqDistortion      = 0x100,
        /// <summary>
		/// Use when profiling with timewarp to remove false positives
        /// </summary>
		ProfileNoTimewarpSpinWaits = 0x10000,
	};

    /// <summary>
	/// Specifies which eye is being used for rendering.
	/// This type explicitly does not include a third "NoStereo" option, as such is
	/// not required for an HMD-centered API.
	/// </summary>
	public enum Eye
	{
		Left  = 0,
		Right = 1,
		Count = 2,
	};

    /// <summary>
	/// This is a complete descriptor of the HMD.
	/// </summary>
	public struct HmdDesc
	{
        /// <summary>
		/// Internal handle of this HMD.
        /// </summary>
		public IntPtr Handle;

        /// <summary>
		/// This HMD's type.
        /// </summary>
		public HmdType Type;

        /// <summary>
		/// Name string describing the product: "Oculus Rift DK1", etc.
        /// </summary>
		public string ProductName;
		public string Manufacturer;

        /// <summary>
		/// HID Vendor and ProductId of the device.
        /// </summary>
		public short VendorId;
		public short ProductId;
        /// <summary>
		/// Sensor (and display) serial number.
        /// </summary>
		public string SerialNumber;
        /// <summary>
		/// Sensor firmware version.
        /// </summary>
		public short FirmwareMajor;
		public short FirmwareMinor;
        /// <summary>
		/// External tracking camera frustum dimensions (if present).
        /// </summary>
		public float CameraFrustumHFovInRadians;
		public float CameraFrustumVFovInRadians;
		public float CameraFrustumNearZInMeters;
		public float CameraFrustumFarZInMeters;

        /// <summary>
		/// Capability bits described by ovrHmdCaps.
        /// </summary>
		public uint HmdCaps;
        /// <summary>
		/// Capability bits described by ovrTrackingCaps.
        /// </summary>
		public uint TrackingCaps;
        /// <summary>
		/// Capability bits described by ovrDistortionCaps.
        /// </summary>
		public uint DistortionCaps;

        /// <summary>
		/// Defines the recommended optical FOV for the HMD.
        /// </summary>
		public FovPort[] DefaultEyeFov;
        /// <summary>
		/// Defines the maximum optical FOV for the HMD.
        /// </summary>
		public FovPort[] MaxEyeFov;

        /// <summary>
		/// Preferred eye rendering order for best performance.
		/// Can help reduce latency on sideways-scanned screens.
        /// </summary>
		public Eye[] EyeRenderOrder;

        /// <summary>
		/// Resolution of the full HMD screen (both eyes) in pixels.
        /// </summary>
		public Sizei Resolution;
        /// <summary>
		/// Location of the application window on the desktop (or 0,0).
        /// </summary>
		public Vector2i WindowsPos;

        /// <summary>
		/// Display that the HMD should present on.
		/// TBD: It may be good to remove this information relying on WindowPos instead.
		/// Ultimately, we may need to come up with a more convenient alternative,
		/// such as API-specific functions that return adapter, or something that will
		/// work with our monitor driver.
		/// Windows: (e.g. "\\\\.\\DISPLAY3", can be used in EnumDisplaySettings/CreateDC).
        /// </summary>
		public string DisplayDeviceName;
        /// <summary>
		/// MacOS:
        /// </summary>
		public int DisplayId;

		internal HmdDesc(HmdDesc_Raw raw)
		{
			this.Handle                     = raw.Handle;
			this.Type                       = (HmdType)raw.Type;
			this.ProductName                = Marshal.PtrToStringAnsi(raw.ProductName);
			this.Manufacturer               = Marshal.PtrToStringAnsi(raw.Manufacturer);
			this.VendorId                   = raw.VendorId;
			this.ProductId                  = raw.ProductId;
			this.SerialNumber               = raw.SerialNumber;
			this.FirmwareMajor              = raw.FirmwareMajor;
			this.FirmwareMinor              = raw.FirmwareMinor;
			this.CameraFrustumHFovInRadians = raw.CameraFrustumHFovInRadians;
			this.CameraFrustumVFovInRadians = raw.CameraFrustumVFovInRadians;
			this.CameraFrustumNearZInMeters = raw.CameraFrustumNearZInMeters;
			this.CameraFrustumFarZInMeters  = raw.CameraFrustumFarZInMeters;
			this.HmdCaps                    = raw.HmdCaps;
			this.TrackingCaps               = raw.TrackingCaps;
			this.DistortionCaps             = raw.DistortionCaps;
			this.Resolution                 = raw.Resolution;
			this.WindowsPos                 = raw.WindowsPos;
			this.DefaultEyeFov              = new FovPort[2] { raw.DefaultEyeFov_0, raw.DefaultEyeFov_1 };
			this.MaxEyeFov                  = new FovPort[2] { raw.MaxEyeFov_0, raw.MaxEyeFov_1 };
			this.EyeRenderOrder             = new Eye[2] { Eye.Left, Eye.Right };
			this.DisplayDeviceName          = Marshal.PtrToStringAnsi(raw.DisplayDeviceName);
			this.DisplayId                  = raw.DisplayId;
		}
	};

	// Internal description for HMD; must match C 'ovrHmdDesc' layout.
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	internal struct HmdDesc_Raw
	{
		public IntPtr Handle;
		public uint Type;
		// Use IntPtr so that CLR doesn't try to deallocate string.
		public IntPtr ProductName;
		public IntPtr Manufacturer;
		// HID Vendor and ProductId of the device.
		public short VendorId;
		public short ProductId;
		// Sensor (and display) serial number.
		[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 24)]
		public string SerialNumber;
		// Sensor firmware
		public short FirmwareMajor;
		public short FirmwareMinor;
		// Fixed camera frustum dimensions, if present
		public float CameraFrustumHFovInRadians;
		public float CameraFrustumVFovInRadians;
		public float CameraFrustumNearZInMeters;
		public float CameraFrustumFarZInMeters;
		public uint HmdCaps;
		public uint TrackingCaps;
		public uint DistortionCaps;
		// C# arrays are dynamic and thus not supported as return values, so just expand the struct.
		public FovPort DefaultEyeFov_0;
		public FovPort DefaultEyeFov_1;
		public FovPort MaxEyeFov_0;
		public FovPort MaxEyeFov_1;
		public Eye EyeRenderOrder_0;
		public Eye EyeRenderOrder_1;
		public Sizei Resolution;
		public Vector2i WindowsPos;
		public IntPtr DisplayDeviceName;
		public int DisplayId;
	};

    /// <summary>
	/// Bit flags describing the current status of sensor tracking.
	/// </summary>
	public enum StatusBits
	{
        /// <summary>
		/// Orientation is currently tracked (connected and in use).
        /// </summary>
		OrientationTracked    = 0x0001,
        /// <summary>
		/// Position is currently tracked (false if out of range).
        /// </summary>
		PositionTracked       = 0x0002,
        /// <summary>
		/// Camera pose is currently tracked.
        /// </summary>
		CameraPoseTracked     = 0x0004,
        /// <summary>
		/// Position tracking hardware is connected.
        /// </summary>
		PositionConnected     = 0x0020,
        /// <summary>
		/// HMD Display is available and connected.
        /// </summary>
		HmdConnected          = 0x0080,
	};

    /// <summary>
	/// Specifies a reading we can query from the sensor.
	/// </summary>
	public struct SensorData
	{
        /// <summary>
		/// Acceleration reading in m/s^2.
        /// </summary>
		public Vector3f Accelerometer;
        /// <summary>
		/// Rotation rate in rad/s.
        /// </summary>
		public Vector3f Gyro;
        /// <summary>
		/// Magnetic field in Gauss.
        /// </summary>
		public Vector3f Magnetometer;
        /// <summary>
		/// Temperature of the sensor in degrees Celsius.
        /// </summary>
		public float Temperature;
        /// <summary>
		/// Time when the reported IMU reading took place, in seconds.
        /// </summary>
		public float TimeInSeconds;
	};

    /// <summary>
	/// Tracking state at a given absolute time (describes predicted HMD pose etc).
	/// Returned by ovrHmd_GetTrackingState.
    /// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TrackingState
	{
        /// <summary>
		/// Predicted head pose (and derivatives) at the requested absolute time.
		/// The look-ahead interval is equal to (HeadPose.TimeInSeconds - RawSensorData.TimeInSeconds).
        /// </summary>
		public PoseStatef HeadPose;

        /// <summary>
		/// Current pose of the external camera (if present).
		/// This pose includes camera tilt (roll and pitch). For a leveled coordinate
		/// system use LeveledCameraPose.
        /// </summary>
		public Posef CameraPose;

        /// <summary>
		/// Camera frame aligned with gravity.
		/// This value includes position and yaw of the camera, but not roll and pitch.
		/// It can be used as a reference point to render real-world objects in the correct location.
        /// </summary>
		public Posef LeveledCameraPose;

        /// <summary>
		/// The most recent sensor data received from the HMD.
        /// </summary>
		public SensorData RawSensorData;

        /// <summary>
		/// Tracking status described by ovrStatusBits.
        /// </summary>
		public uint StatusFlags;

		//// 0.4.1

		// Measures the time from receiving the camera frame until vision CPU processing completes.
		public double LastVisionProcessingTime;

		//// 0.4.3

        /// <summary>
		/// Measures the time from exposure until the pose is available for the frame, including processing time.
        /// </summary>
		public double LastVisionFrameLatency;

        /// <summary>
		/// Tag the vision processing results to a certain frame counter number.
        /// </summary>
		public uint LastCameraFrameCounter;
	};

    /// <summary>
	/// Frame timing data reported by ovrHmd_BeginFrameTiming() or ovrHmd_BeginFrame().
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct FrameTiming
	{
        /// <summary>
		/// The amount of time that has passed since the previous frame's
		/// ThisFrameSeconds value (usable for movement scaling).
		/// This will be clamped to no more than 0.1 seconds to prevent
		/// excessive movement after pauses due to loading or initialization.
        /// </summary>
		public float DeltaSeconds;

		// It is generally expected that the following holds:
		// ThisFrameSeconds < TimewarpPointSeconds < NextFrameSeconds <
		// EyeScanoutSeconds[EyeOrder[0]] <= ScanoutMidpointSeconds <= EyeScanoutSeconds[EyeOrder[1]].

        /// <summary>
		/// Absolute time value when rendering of this frame began or is expected to
		/// begin. Generally equal to NextFrameSeconds of the previous frame. Can be used
		/// for animation timing.
        /// </summary>
		public double ThisFrameSeconds;
        /// <summary>
		/// Absolute point when IMU expects to be sampled for this frame.
        /// </summary>
		public double TimewarpPointSeconds;
        /// <summary>
		/// Absolute time when frame Present followed by GPU Flush will finish and the next frame begins.
        /// </summary>
        public double NextFrameSeconds;

        /// <summary>
		/// Time when half of the screen will be scanned out. Can be passed as an absolute time
		/// to ovrHmd_GetTrackingState() to get the predicted general orientation.
        /// </summary>
		public double ScanoutMidpointSeconds;
        /// <summary>
		/// Timing points when each eye will be scanned out to display. Used when rendering each eye.
        /// </summary>
		public double[] EyeScanoutSeconds;

		internal FrameTiming(FrameTiming_Raw raw)
		{
			this.DeltaSeconds           = raw.DeltaSeconds;
			this.ThisFrameSeconds       = raw.ThisFrameSeconds;
			this.TimewarpPointSeconds   = raw.TimewarpPointSeconds;
			this.NextFrameSeconds       = raw.NextFrameSeconds;
			this.ScanoutMidpointSeconds = raw.ScanoutMidpointSeconds;
			this.EyeScanoutSeconds      = new double[2] { raw.EyeScanoutSeconds_0, raw.EyeScanoutSeconds_1 };
		}
	};

	// Internal description for ovrFrameTiming; must match C 'ovrFrameTiming' layout.
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	internal struct FrameTiming_Raw
	{
		public float DeltaSeconds;
		public double ThisFrameSeconds;
		public double TimewarpPointSeconds;
		public double NextFrameSeconds;
		public double ScanoutMidpointSeconds;
		// C# arrays are dynamic and thus not supported as return values, so just expand the struct.
		public double EyeScanoutSeconds_0;
		public double EyeScanoutSeconds_1;
	};

    /// <summary>
    /// Rendering information for each eye. Computed by either ovrHmd_ConfigureRendering()
	/// or ovrHmd_GetRenderDesc() based on the specified FOV. Note that the rendering viewport
	/// is not included here as it can be specified separately and modified per frame through:
	///    (a) ovrHmd_GetRenderScaleAndOffset in the case of client rendered distortion,
	/// or (b) passing different values via ovrTexture in the case of SDK rendered distortion.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EyeRenderDesc
	{
        /// <summary>
        /// The eye index this instance corresponds to.
        /// </summary>
		public Eye Eye;
        /// <summary>
        /// The field of view.
        /// </summary>
		public FovPort Fov;
        /// <summary>
		/// Distortion viewport.
        /// </summary>
		public Recti DistortedViewport;
        /// <summary>
		/// How many display pixels will fit in tan(angle) = 1.
        /// </summary>
		public Vector2f PixelsPerTanAngleAtCenter;
        /// <summary>
		/// Translation to be applied to view matrix for each eye offset.
        /// </summary>
		public Vector3f HmdToEyeViewOffset;
	};

	//-----------------------------------------------------------------------------------
	// ***** Platform-independent Rendering Configuration

    /// <summary>
	/// These types are used to hide platform-specific details when passing
	/// render device, OS, and texture data to the API.
	///
	/// The benefit of having these wrappers versus platform-specific API functions is
	/// that they allow game glue code to be portable. A typical example is an
	/// engine that has multiple back ends, say GL and D3D. Portable code that calls
	/// these back ends may also use LibOVR. To do this, back ends can be modified
	/// to return portable types such as ovrTexture and ovrRenderAPIConfig.
	/// </summary>
	public enum RenderAPIType
	{
		None,
		OpenGL,
		Android_GLES,  // May include extra native window pointers, etc.
		D3D9,
		D3D10,
		D3D11,
		Count,
	};

	/// <summary>
	/// Platform-independent part of rendering API-configuration data.
	/// It is a part of ovrRenderAPIConfig, passed to ovrHmd_Configure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RenderAPIConfigHeader
	{
		public RenderAPIType API;
		public Sizei RTSize;
		public int Multisample;
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct RenderAPIConfig_Raw
	{
		public RenderAPIConfigHeader Header;
		public IntPtr PlatformData0;
		public IntPtr PlatformData1;
		public IntPtr PlatformData2;
		public IntPtr PlatformData3;
		public IntPtr PlatformData4;
		public IntPtr PlatformData5;
		public IntPtr PlatformData6;
		public IntPtr PlatformData7;
	};

	/// <summary>
	/// Contains platform-specific information for rendering.
	/// </summary>
	public abstract class RenderAPIConfig
	{
		public RenderAPIConfig() { Header.API = RenderAPIType.None; }

		public RenderAPIConfigHeader Header;

		internal abstract RenderAPIConfig_Raw ToRaw();
	}

	/// <summary>
	/// Contains OpenGL-specific rendering information for Windows.
	/// </summary>
	public class OpenGLWindowsConfig : RenderAPIConfig
	{
		public OpenGLWindowsConfig(Sizei rtSize, int multisample, IntPtr hwnd, IntPtr HDCDeviceContext)
		{
			Header.API = RenderAPIType.OpenGL;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
			_hwnd = hwnd;
			_HDCDeviceContext = HDCDeviceContext;
		}

        internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
            config.Header = this.Header;
			config.PlatformData0 = this._hwnd;
			config.PlatformData1 = this._HDCDeviceContext;
			return config;
		}

		public IntPtr _hwnd;
		public IntPtr _HDCDeviceContext;
	}

	/// <summary>
	/// Contains OpenGL-specific rendering information for Linux.
	/// </summary>
	public class OpenGLLinuxConfig : RenderAPIConfig
	{
		public OpenGLLinuxConfig(Sizei rtSize, int multisample, IntPtr optionalXDisplay, IntPtr optionalWindow)
		{
			Header.API = RenderAPIType.OpenGL;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
			_OptionalXDisplay = optionalXDisplay;
			_OptionalWindow = optionalWindow;
		}

		internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
            config.Header = this.Header;
			config.PlatformData0 = this._OptionalXDisplay;
			config.PlatformData1 = this._OptionalWindow;
			return config;
		}

		IntPtr _OptionalXDisplay;
		IntPtr _OptionalWindow;
	}

	/// <summary>
	/// Contains OpenGL ES-specific rendering information.
	/// </summary>
	public class AndroidGLESConfig : RenderAPIConfig
	{
		public AndroidGLESConfig(Sizei rtSize, int multisample)
		{
			Header.API = RenderAPIType.Android_GLES;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
		}

		internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
			config.Header = this.Header;
			return config;
		}
	}

	/// <summary>
	/// Contains D3D9-specific rendering information.
	/// </summary>
	public class D3D9Config : RenderAPIConfig
	{
		public D3D9Config(Sizei rtSize, int multisample, IntPtr IDirect3DDevice9_pDevice, IntPtr IDirect3DSwapChain9_pSwapChain)
		{
			Header.API = RenderAPIType.D3D9;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
			_IDirect3DDevice9_pDevice = IDirect3DDevice9_pDevice;
			_IDirect3DSwapChain9_pSwapChain = IDirect3DSwapChain9_pSwapChain;
		}

		internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
            config.Header = this.Header;
			config.PlatformData0 = this._IDirect3DDevice9_pDevice;
			config.PlatformData1 = this._IDirect3DSwapChain9_pSwapChain;
			return config;
		}

		IntPtr _IDirect3DDevice9_pDevice;
		IntPtr _IDirect3DSwapChain9_pSwapChain;
	}

	/// <summary>
	/// Contains D3D10-specific rendering information.
	/// </summary>
	public class D3D10Config : RenderAPIConfig
	{
		public D3D10Config(Sizei rtSize, int multisample, IntPtr ID3D10RenderTargetView_pBackBufferRT, IntPtr IDXGISwapChain_pSwapChain)
		{
			Header.API = RenderAPIType.D3D10;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
			_ID3D10RenderTargetView_pBackBufferRT = ID3D10RenderTargetView_pBackBufferRT;
			_IDXGISwapChain_pSwapChain = IDXGISwapChain_pSwapChain;
		}

		internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
			config.Header = this.Header;
			config.PlatformData0 = IntPtr.Zero;
			config.PlatformData1 = this._ID3D10RenderTargetView_pBackBufferRT;
			config.PlatformData2 = this._IDXGISwapChain_pSwapChain;
			return config;
		}

		IntPtr _ID3D10RenderTargetView_pBackBufferRT;
		IntPtr _IDXGISwapChain_pSwapChain;
	}

	/// <summary>
	/// Contains D3D11-specific rendering information.
	/// </summary>
	public class D3D11Config : RenderAPIConfig
	{
		public D3D11Config(Sizei rtSize, int multisample, IntPtr ID3D11Device_pDevice, IntPtr ID3D11DeviceContext_pDeviceContext, IntPtr ID3D11RenderTargetView_pBackBufferRT, IntPtr IDXGISwapChain_pSwapChain)
		{
			Header.API = RenderAPIType.D3D11;
			Header.RTSize = rtSize;
			Header.Multisample = multisample;
			_ID3D11Device_pDevice = ID3D11Device_pDevice;
			_ID3D11DeviceContext_pDeviceContext = ID3D11DeviceContext_pDeviceContext;
			_ID3D11RenderTargetView_pBackBufferRT = ID3D11RenderTargetView_pBackBufferRT;
			_IDXGISwapChain_pSwapChain = IDXGISwapChain_pSwapChain;
		}

		internal override RenderAPIConfig_Raw ToRaw()
		{
			RenderAPIConfig_Raw config = new RenderAPIConfig_Raw();
			config.Header = this.Header;
			config.PlatformData0 = this._ID3D11Device_pDevice;
			config.PlatformData1 = this._ID3D11DeviceContext_pDeviceContext;
			config.PlatformData2 = this._ID3D11RenderTargetView_pBackBufferRT;
			config.PlatformData3 = this._IDXGISwapChain_pSwapChain;
			return config;
		}

		IntPtr _ID3D11Device_pDevice;
		IntPtr _ID3D11DeviceContext_pDeviceContext;
		IntPtr _ID3D11RenderTargetView_pBackBufferRT;
		IntPtr _IDXGISwapChain_pSwapChain;
	}

	/// <summary>
	/// Platform-independent part of the eye texture descriptor.
	/// It is a part of ovrTexture, passed to ovrHmd_EndFrame.
	/// If RenderViewport is all zeros then the full texture will be used.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TextureHeader
	{
		public RenderAPIType API;
		public Sizei TextureSize;
		public Recti RenderViewport;  // Pixel viewport in texture that holds eye image.
	};

    /// <summary>
    /// Contains platform-specific information for rendering.
    /// </summary>
    public abstract class Texture
    {
        public Texture() { Header.API = RenderAPIType.None; }

        public TextureHeader Header;

        internal abstract Texture_Raw ToRaw();
    }

    /// <summary>
    /// Contains OpenGL-specific texture information
    /// </summary>
    public class GLTextureData : Texture
    {
        public GLTextureData(Sizei textureSize, Recti renderViewport, IntPtr texId)
        {
            Header.API = RenderAPIType.OpenGL;
            Header.TextureSize = textureSize;
            Header.RenderViewport = renderViewport;
            _texId = texId;
        }

        internal override Texture_Raw ToRaw()
        {
            Texture_Raw config = new Texture_Raw();
            config.Header = this.Header;
            config.PlatformData_0 = this._texId;
            return config;
        }

        public IntPtr _texId;
    }

    /// <summary>
    /// Contains D3D9-specific texture information
    /// </summary>
    public class D3D9TextureData : Texture
    {
        public D3D9TextureData(Sizei textureSize, Recti renderViewport, IntPtr IDirect3DTexture9_pTexture)
        {
            Header.API = RenderAPIType.D3D9;
            Header.TextureSize = textureSize;
            Header.RenderViewport = renderViewport;
            _IDirect3DTexture9_pTexture = IDirect3DTexture9_pTexture;
        }

        internal override Texture_Raw ToRaw()
        {
            Texture_Raw config = new Texture_Raw();
            config.Header = this.Header;
            config.PlatformData_0 = this._IDirect3DTexture9_pTexture;
            return config;
        }

        public IntPtr _IDirect3DTexture9_pTexture;
    }

    /// <summary>
    /// Contains D3D10-specific texture information
    /// </summary>
    public class D3D10TextureData : Texture
    {
        public D3D10TextureData(Sizei textureSize, Recti renderViewport, IntPtr ID3D10Texture2D_pTexture, IntPtr ID3D10ShaderResourceView_pSRView)
        {
            Header.API = RenderAPIType.D3D10;
            Header.TextureSize = textureSize;
            Header.RenderViewport = renderViewport;
            _ID3D10Texture2D_pTexture = ID3D10Texture2D_pTexture;
            _ID3D10ShaderResourceView_pSRView = ID3D10ShaderResourceView_pSRView;
        }

        internal override Texture_Raw ToRaw()
        {
            Texture_Raw config = new Texture_Raw();
            config.Header = this.Header;
            config.PlatformData_0 = this._ID3D10Texture2D_pTexture;
            config.PlatformData_1 = this._ID3D10ShaderResourceView_pSRView;
            return config;
        }

        public IntPtr _ID3D10Texture2D_pTexture, _ID3D10ShaderResourceView_pSRView;
    }


    /// <summary>
    /// Contains D3D11-specific texture information
    /// </summary>
    public class D3D11TextureData : Texture
    {
        public D3D11TextureData(Sizei textureSize, Recti renderViewport, IntPtr ID3D11Texture2D_pTexture, IntPtr ID3D11ShaderResourceView_pSRView)
        {
            Header.API = RenderAPIType.D3D11;
            Header.TextureSize = textureSize;
            Header.RenderViewport = renderViewport;
            _ID3D11Texture2D_pTexture = ID3D11Texture2D_pTexture;
            _ID3D11ShaderResourceView_pSRView = ID3D11ShaderResourceView_pSRView;
        }

        internal override Texture_Raw ToRaw()
        {
            Texture_Raw config = new Texture_Raw();
            config.Header = this.Header;
            config.PlatformData_0 = this._ID3D11Texture2D_pTexture;
            config.PlatformData_1 = this._ID3D11ShaderResourceView_pSRView;
            return config;
        }

        public IntPtr _ID3D11Texture2D_pTexture, _ID3D11ShaderResourceView_pSRView;
    }

	// Internal description for ovrTexture; must match C 'ovrTexture' layout.
	[StructLayout(LayoutKind.Sequential)]
	internal struct Texture_Raw
	{
		public TextureHeader Header;
		public IntPtr PlatformData_0;
		public IntPtr PlatformData_1;
		public IntPtr PlatformData_2;
		public IntPtr PlatformData_3;
		public IntPtr PlatformData_4;
		public IntPtr PlatformData_5;
		public IntPtr PlatformData_6;
		public IntPtr PlatformData_7;
	};

	/// <summary>
	/// Describes a vertex used by the distortion mesh. This is intended to be converted into
	/// the engine-specific format. Some fields may be unused based on the ovrDistortionCaps
	/// flags selected. TexG and TexB, for example, are not used if chromatic correction is
	/// not requested.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DistortionVertex
	{
        /// <summary>
		/// [-1,+1],[-1,+1] over the entire framebuffer.
        /// </summary>
		public Vector2f ScreenPosNDC;
        /// <summary>
		/// Lerp factor between time-warp matrices. Can be encoded in Pos.z.
        /// </summary>
		public float TimeWarpFactor;
        /// <summary>
		/// Vignette fade factor. Can be encoded in Pos.w.
        /// </summary>
		public float VignetteFactor;
        /// <summary>
        /// The tangents of the horizontal and vertical eye angles for the red channel.
        /// </summary>
		public Vector2f TanEyeAnglesR;
        /// <summary>
        /// The tangents of the horizontal and vertical eye angles for the green channel.
        /// </summary>
		public Vector2f TanEyeAnglesG;
        /// <summary>
        /// The tangents of the horizontal and vertical eye angles for the blue channel.
        /// </summary>
		public Vector2f TanEyeAnglesB;
	};

    /// <summary>
    /// Describes a full set of distortion mesh data, filled in by ovrHmd_CreateDistortionMesh.
	/// Contents of this data structure, if not null, should be freed by ovrHmd_DestroyDistortionMesh.
	/// </summary>
	public struct DistortionMesh
	{
        /// <summary>
        /// The distortion vertices representing each point in the mesh.
        /// </summary>
		public DistortionVertex[] pVertexData;
        /// <summary>
        /// Indices for connecting the mesh vertices into polygons.
        /// </summary>
		public short[] pIndexData;
        /// <summary>
        /// The number of vertices in the mesh.
        /// </summary>
		public uint VertexCount;
        /// <summary>
        /// The number of indices in the mesh.        
        /// </summary>
		public uint IndexCount;

		internal DistortionMesh(DistortionMesh_Raw raw)
		{
			this.VertexCount = raw.VertexCount;
			this.pVertexData = new DistortionVertex[this.VertexCount];
			this.IndexCount  = raw.IndexCount;
			this.pIndexData  = new short[this.IndexCount];

			// Copy data
			System.Type vertexType = typeof(DistortionVertex);
			Int32 vertexSize = Marshal.SizeOf(vertexType);
			Int32 indexSize  = sizeof(short);
			Int64 pvertices  = raw.pVertexData.ToInt64();
			Int64 pindices   = raw.pIndexData.ToInt64();

			// TODO: Investigate using Marshal.Copy() or Buffer.BlockCopy() for improved performance

			for (int i = 0; i < raw.VertexCount; i++)
			{
				pVertexData[i] = (DistortionVertex)Marshal.PtrToStructure(new IntPtr(pvertices), vertexType);
				pvertices += vertexSize;
			}
			// Indices are stored as shorts.
			for (int j = 0; j < raw.IndexCount; j++)
			{
				pIndexData[j] = Marshal.ReadInt16(new IntPtr(pindices));
				pindices += indexSize;
			}
		}
	};

	// Internal description for ovrDistortionMesh; must match C 'ovrDistortionMesh' layout.
	[StructLayout(LayoutKind.Sequential)]
	internal struct DistortionMesh_Raw
	{
		public IntPtr pVertexData;
		public IntPtr pIndexData;
		public uint VertexCount;
		public uint IndexCount;
	};

	/// <summary>
	/// Used by ovrhmd_GetHSWDisplayState to report the current display state.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct HSWDisplayState
	{
        /// <summary>
		/// If true then the warning should be currently visible
		/// and the following variables have meaning. Else there is no
		/// warning being displayed for this application on the given HMD.
        /// True if the Health&Safety Warning is currently displayed.
        /// </summary>
		public bool Displayed;
        /// <summary>
		/// Absolute time when the warning was first displayed. See ovr_GetTimeInSeconds().
        /// </summary>
		public double StartTime;
        /// <summary>
		/// Earliest absolute time when the warning can be dismissed. May be a time in the past.
        /// </summary>
		public double DismissibleTime;
	};

	/// <summary>
	/// Provides an interface to a CAPI HMD object.  The ovrHmd instance is normally
	/// created by ovrHmd::Create, after which its other methods can be called.
	/// The typical process would involve calling:
	///
	/// Setup:
	///   - Initialize() to initialize the OVR SDK.
	///   - Create() to create an HMD.
	///   - Use hmd members and ovrHmd_GetFovTextureSize() to determine graphics configuration.
	///   - ConfigureTracking() to configure and initialize tracking.
	///   - ConfigureRendering() to setup graphics for SDK rendering.
	///   - If ovrHmdCap_ExtendDesktop is not set, use ovrHmd_AttachToWindow to associate the window with an Hmd.
	///   - Allocate textures as needed.
	///
	/// Game Loop:
	///   - Call ovrHmd_BeginFrame() to get frame timing and orientation information.
	///   - Render each eye in between, using ovrHmd_GetEyePoses or ovrHmd_GetHmdPosePerEye to get the predicted hmd pose and each eye pose.
	///   - Call ovrHmd_EndFrame() to render distorted textures to the back buffer
	///     and present them on the Hmd.
	///
	/// Shutdown:
	///   - Destroy() to release the HMD.
	///   - ovr_Shutdown() to shutdown the OVR SDK.
	/// </summary>
	public class Hmd
	{
		public const string OVR_VERSION_STRING                    = "0.4.4";
		public const string OVR_KEY_USER                          = "User";
		public const string OVR_KEY_NAME                          = "Name";
		public const string OVR_KEY_GENDER                        = "Gender";
		public const string OVR_KEY_PLAYER_HEIGHT                 = "PlayerHeight";
		public const string OVR_KEY_EYE_HEIGHT                    = "EyeHeight";
		public const string OVR_KEY_IPD                           = "IPD";
		public const string OVR_KEY_NECK_TO_EYE_DISTANCE          = "NeckEyeDistance";
		public const string OVR_KEY_EYE_RELIEF_DIAL               = "EyeReliefDial";
		public const string OVR_KEY_EYE_TO_NOSE_DISTANCE          = "EyeToNoseDist";
		public const string OVR_KEY_MAX_EYE_TO_PLATE_DISTANCE     = "MaxEyeToPlateDist";
		public const string OVR_KEY_EYE_CUP                       = "EyeCup";
		public const string OVR_KEY_CUSTOM_EYE_RENDER             = "CustomEyeRender";
		public const string OVR_KEY_CAMERA_POSITION               = "CenteredFromWorld";

		// Default measurements empirically determined at Oculus to make us happy
		// The neck model numbers were derived as an average of the male and female averages from ANSUR-88
		// NECK_TO_EYE_HORIZONTAL = H22 - H43 = INFRAORBITALE_BACK_OF_HEAD - TRAGION_BACK_OF_HEAD
		// NECK_TO_EYE_VERTICAL = H21 - H15 = GONION_TOP_OF_HEAD - ECTOORBITALE_TOP_OF_HEAD
		// These were determined to be the best in a small user study, clearly beating out the previous default values
		public const string OVR_DEFAULT_GENDER                = "Unknown";
		public const float OVR_DEFAULT_PLAYER_HEIGHT          = 1.778f;
		public const float OVR_DEFAULT_EYE_HEIGHT             = 1.675f;
		public const float OVR_DEFAULT_IPD                    = 0.064f;
		public const float OVR_DEFAULT_NECK_TO_EYE_HORIZONTAL = 0.0805f;
		public const float OVR_DEFAULT_NECK_TO_EYE_VERTICAL   = 0.075f;
		public const float OVR_DEFAULT_EYE_RELIEF_DIAL        = 3;
		public readonly float[] OVR_DEFAULT_CAMERA_POSITION   = {0,0,0,1,0,0,0};

		private IntPtr HmdPtr;

		// Used to return color result to avoid per-frame allocation.
		private byte[] LatencyTestRgb = new byte[3];
		
		// -----------------------------------------------------------------------------------
		// Static Methods

		// ovr_InitializeRenderingShim initializes the rendering shim appart from everything
		// else in LibOVR. This may be helpful if the application prefers to avoid
		// creating any OVR resources (allocations, service connections, etc) at this point.
		// ovr_InitializeRenderingShim does not bring up anything within LibOVR except the
		// necessary hooks to enable the Direct-to-Rift functionality.
		//
		// Either ovr_InitializeRenderingShim() or ovr_Initialize() must be called before any
		// Direct3D or OpenGL initilization is done by applictaion (creation of devices, etc).
		// ovr_Initialize() must still be called after to use the rest of LibOVR APIs.
		public static void InitializeRenderingShim()
		{
			ovr_InitializeRenderingShim();
		}

		// Library init/shutdown, must be called around all other OVR code.
		// No other functions calls besides ovr_InitializeRenderingShim are allowed
		// before ovr_Initialize succeeds or after ovr_Shutdown.

        /// <summary>
		/// Initializes all Oculus functionality.
        /// </summary>
		public static bool Initialize()
		{
			return ovr_Initialize() != 0;
		}

        /// <summary>
		/// Shuts down all Oculus functionality.
        /// </summary>
		public static void Shutdown()
		{
			ovr_Shutdown();
		}

        /// <summary>
		/// Returns version string representing libOVR version.
        /// </summary>
		public static string GetVersionString()
		{
			return Marshal.PtrToStringAnsi(ovr_GetVersionString());
		}

        /// <summary>
		/// Detects or re-detects HMDs and reports the total number detected.
		/// Users can get information about each HMD by calling ovrHmd_Create with an index.
        /// </summary>
		public static int Detect()
		{
			return ovrHmd_Detect();
		}

        /// <summary>
		/// Creates a handle to an HMD which doubles as a description structure.
		/// Index can [0 .. ovrHmd_Detect()-1]. Index mappings can cange after each ovrHmd_Detect call.
        /// </summary>
		public static Hmd Create(int index)
		{
			IntPtr hmdPtr = ovrHmd_Create(index);
			if (hmdPtr == IntPtr.Zero)
				return null;

			return new Hmd(hmdPtr);
		}

        /// <summary>
		/// Creates a 'fake' HMD used for debugging only. This is not tied to specific hardware,
		/// but may be used to debug some of the related rendering.
        /// </summary>
		public static Hmd CreateDebug(HmdType type)
		{
			IntPtr hmdPtr = ovrHmd_CreateDebug(type);
			if (hmdPtr == IntPtr.Zero)
				return null;

			return new Hmd(hmdPtr);
		}

        /// <summary>
		/// Used to generate projection from ovrEyeDesc::Fov.
        /// </summary>
		public static Matrix4f GetProjection(FovPort fov, float znear, float zfar, bool rightHanded)
		{
			return new Matrix4f(ovrMatrix4f_Projection(fov, znear, zfar, rightHanded));
		}

        /// <summary>
		/// Used for 2D rendering, Y is down
		/// orthoScale = 1.0f / pixelsPerTanAngleAtCenter
		/// orthoDistance = distance from camera, such as 0.8m
        /// </summary>
		public static Matrix4f GetOrthoSubProjection(Matrix4f projection, Vector2f orthoScale, float orthoDistance, float hmdToEyeViewOffsetX)
		{
			return new Matrix4f(ovrMatrix4f_OrthoSubProjection(projection, orthoScale, orthoDistance, hmdToEyeViewOffsetX));
		}

        /// <summary>
		/// Returns global, absolute high-resolution time in seconds. This is the same
		/// value as used in sensor messages.
        /// </summary>
		public static double GetTimeInSeconds()
		{
			return ovr_GetTimeInSeconds();
		}

        /// <summary>
		/// Waits until the specified absolute time.
        /// </summary>
		public static double WaitTillTime(double absTime)
		{
			return ovr_WaitTillTime(absTime);
		}

		// -----------------------------------------------------------------------------------
		// **** Constructor

		public Hmd(IntPtr hmdPtr)
		{
			this.HmdPtr = hmdPtr;
		}

        /// <summary>
		/// Returns last error for HMD state. Returns null for no error.
		/// String is valid until next call or GetLastError or HMD is destroyed.
        /// </summary>
		public string GetLastError()
		{
			return ovrHmd_GetLastError(HmdPtr);
		}

        /// <summary>
		/// Platform specific function to specify the application window whose output will be 
		/// displayed on the HMD. Only used if the ovrHmdCap_ExtendDesktop flag is false.
		///   Windows: SwapChain associated with this window will be displayed on the HMD.
		///            Specify 'destMirrorRect' in window coordinates to indicate an area
		///            of the render target output that will be mirrored from 'sourceRenderTargetRect'.
		///            Null pointers mean "full size".
		/// @note Source and dest mirror rects are not yet implemented.
        /// </summary>
        public bool AttachToWindow(Recti destMirrorRect, Recti sourceRenderTargetRect, IntPtr WindowPtr = default(IntPtr))
		{
            return ovrHmd_AttachToWindow(HmdPtr, WindowPtr, destMirrorRect, sourceRenderTargetRect) != 0;
		}

        /// <summary>
		/// Returns capability bits that are enabled at this time as described by ovrHmdCaps.
		/// Note that this value is different font ovrHmdDesc::HmdCaps, which describes what
		/// capabilities are available for that HMD.
        /// </summary>
		public uint GetEnabledCaps()
		{
			return ovrHmd_GetEnabledCaps(HmdPtr);
		}
		
        /// <summary>
		/// Modifies capability bits described by ovrHmdCaps that can be modified,
		/// such as ovrHmdCap_LowPersistance.
        /// </summary>
		public void SetEnabledCaps(uint capsBits)
		{
			ovrHmd_SetEnabledCaps(HmdPtr, capsBits);
		}

        /// <summary>
		/// Returns an ovrHmdDesc, which provides a complete description for the HMD
        /// </summary>
		public HmdDesc GetDesc()
		{
			HmdDesc_Raw rawDesc = (HmdDesc_Raw)Marshal.PtrToStructure(HmdPtr, typeof(HmdDesc_Raw));
			return new HmdDesc(rawDesc);
		}

		//-------------------------------------------------------------------------------------
		// ***** Tracking Interface
		
        /// <summary>
		/// All tracking interface functions are thread-safe, allowing tracking state to be sampled
		/// from different threads.
		/// ConfigureTracking starts sensor sampling, enabling specified capabilities,
		///    described by ovrTrackingCaps.
		///  - supportedTrackingCaps specifies support that is requested. The function will succeed
		///   even if these caps are not available (i.e. sensor or camera is unplugged). Support
		///    will automatically be enabled if such device is plugged in later. Software should
		///    check ovrTrackingState.StatusFlags for real-time status.
		///  - requiredTrackingCaps specify sensor capabilities required at the time of the call.
		///    If they are not available, the function will fail. Pass 0 if only specifying
		///    supportedTrackingCaps.
		///  - Pass 0 for both supportedTrackingCaps and requiredTrackingCaps to disable tracking.
        /// </summary>
		public bool ConfigureTracking(uint supportedTrackingCaps, uint requiredTrackingCaps)
		{
			return ovrHmd_ConfigureTracking(HmdPtr, supportedTrackingCaps, requiredTrackingCaps) != 0;
		}

        /// <summary>
		/// Re-centers the sensor orientation.
		/// Normally this will recenter the (x,y,z) translational components and the yaw
		/// component of orientation.
        /// </summary>
		public void RecenterPose()
		{
			ovrHmd_RecenterPose(HmdPtr);
		}

        /// <summary>
		/// Returns tracking state reading based on the specified absolute system time.
		/// Pass an absTime value of 0.0 to request the most recent sensor reading. In this case
		/// both PredictedPose and SamplePose will have the same value.
		/// ovrHmd_GetEyePoses relies on this function internally.
		/// This may also be used for more refined timing of FrontBuffer rendering logic, etc.
        /// </summary>
		public TrackingState GetTrackingState(double absTime = 0.0d)
		{
			return ovrHmd_GetTrackingState(HmdPtr, absTime);
		}

		//-------------------------------------------------------------------------------------
		// ***** Graphics Setup
		
        /// <summary>
		/// Calculates the recommended texture size for rendering a given eye within the HMD
		/// with a given FOV cone. Higher FOV will generally require larger textures to
		/// maintain quality.
		///  - pixelsPerDisplayPixel specifies the ratio of the number of render target pixels
		///    to display pixels at the center of distortion. 1.0 is the default value. Lower
		///    values can improve performance.
        /// </summary>
		public Sizei GetFovTextureSize(Eye eye, FovPort fov, float pixelsPerDisplayPixel = 1.0f)
		{
			return ovrHmd_GetFovTextureSize(HmdPtr, eye, fov, pixelsPerDisplayPixel);
		}

		//-------------------------------------------------------------------------------------
		// *****  Rendering API Thread Safety

		//  All of rendering functions including the configure and frame functions
		// are *NOT thread safe*. It is ok to use ConfigureRendering on one thread and handle
		//  frames on another thread, but explicit synchronization must be done since
		//  functions that depend on configured state are not reentrant.
		//
		//  As an extra requirement, any of the following calls must be done on
		//  the render thread, which is the same thread that calls ovrHmd_BeginFrame
		//  or ovrHmd_BeginFrameTiming.
		//    - ovrHmd_EndFrame
		//    - ovrHmd_GetEyeTimewarpMatrices

		//-------------------------------------------------------------------------------------
		// *****  SDK Distortion Rendering Functions

		// These functions support rendering of distortion by the SDK through direct
		// access to the underlying rendering API, such as D3D or GL.
		// This is the recommended approach since it allows better support for future
		// Oculus hardware, and enables a range of low-level optimizations.

        /// <summary>
		/// Configures rendering and fills in computed render parameters.
		/// This function can be called multiple times to change rendering settings.
		/// eyeRenderDescOut is a pointer to an array of two EyeRenderDesc structs
		/// that are used to return complete rendering information for each eye.
		///  - apiConfig provides D3D/OpenGL specific parameters. Pass null
		///    to shutdown rendering and release all resources.
		///  - distortionCaps describe desired distortion settings.
        /// </summary>
        public EyeRenderDesc[] ConfigureRendering(ref RenderAPIConfig renderAPIConfig, FovPort[] eyeFovIn, uint distortionCaps)
		{
			EyeRenderDesc[] eyeRenderDesc = new EyeRenderDesc[] { new EyeRenderDesc(), new EyeRenderDesc() };
			RenderAPIConfig_Raw rawConfig = renderAPIConfig.ToRaw();

			bool result = ovrHmd_ConfigureRendering(HmdPtr, ref rawConfig, distortionCaps, eyeFovIn, eyeRenderDesc) != 0;
            if (result)
				return eyeRenderDesc;
			return null;
		}

        /// <summary>
		/// Begins a frame, returning timing information.
		/// This should be called at the beginning of the game rendering loop (on the render thread).
		/// Pass 0 for the frame index if not using ovrHmd_GetFrameTiming.
        /// </summary>
		public FrameTiming BeginFrame(uint frameIndex = 0)
		{
			FrameTiming_Raw raw = ovrHmd_BeginFrame(HmdPtr, frameIndex);
			return new FrameTiming(raw);
		}

        /// <summary>
		/// Ends a frame, submitting the rendered textures to the frame buffer.
		/// - RenderViewport within each eyeTexture can change per frame if necessary.
		/// - 'renderPose' will typically be the value returned from ovrHmd_GetEyePoses,
		///   ovrHmd_GetHmdPosePerEye but can be different if a different head pose was
	   	///   used for rendering.
		/// - This may perform distortion and scaling internally, assuming is it not
		///   delegated to another thread.
		/// - Must be called on the same thread as BeginFrame.
		/// - *** This Function will call Present/SwapBuffers and potentially wait for GPU Sync ***.
        /// </summary>
		public void EndFrame(Posef[] renderPose, Texture[] eyeTexture)
		{
            Texture_Raw[] raw = new Texture_Raw[eyeTexture.Length];
            for (int i = 0; i < eyeTexture.Length; i++)
            {
                raw[i] = eyeTexture[i].ToRaw();
            }
            
            ovrHmd_EndFrame(HmdPtr, renderPose, raw);
		}

        /// <summary>
		/// Returns predicted head pose in outHmdTrackingState and offset eye poses in outEyePoses
		/// as an atomic operation. Caller need not worry about applying HmdToEyeViewOffset to the
		/// returned outEyePoses variables.
		/// - Thread-safe function where caller should increment frameIndex with every frame
		///   and pass the index where applicable to functions called on the  rendering thread.
		/// - hmdToEyeViewOffset[2] can be EyeRenderDesc.HmdToEyeViewOffset returned from 
		///   ovrHmd_ConfigureRendering or ovrHmd_GetRenderDesc. For monoscopic rendering,
		///   use a vector that is the average of the two vectors for both eyes.
		/// - If frameIndex is not being used, pass in 0.
		/// - Assuming outEyePoses are used for rendering, it should be passed into ovrHmd_EndFrame.
		/// - If called doesn't need outHmdTrackingState, it can be NULL
        /// </summary>
		public Posef[] GetEyePoses(uint frameIndex)
		{
			FovPort leftFov = GetDesc().DefaultEyeFov[(int)Eye.Left];
			FovPort rightFov = GetDesc().DefaultEyeFov[(int)Eye.Right];

			EyeRenderDesc leftDesc = GetRenderDesc(Eye.Left, leftFov);
			EyeRenderDesc rightDesc = GetRenderDesc(Eye.Right, rightFov);

			TrackingState trackingState = new TrackingState();
			Vector3f[] eyeOffsets = { leftDesc.HmdToEyeViewOffset, rightDesc.HmdToEyeViewOffset };
			Posef[] eyePoses = { new Posef(), new Posef() };

            ovrHmd_GetEyePoses(HmdPtr, frameIndex, eyeOffsets, eyePoses, ref trackingState);

			return eyePoses;
		}

        /// <summary>
		/// Function was previously called ovrHmd_GetEyePose
		/// Returns the predicted head pose to use when rendering the specified eye.
		/// - Important: Caller must apply HmdToEyeViewOffset before using ovrPosef for rendering
		/// - Must be called between ovrHmd_BeginFrameTiming and ovrHmd_EndFrameTiming.
		/// - If the pose is used for rendering the eye, it should be passed to ovrHmd_EndFrame.
		/// - Parameter 'eye' is used for prediction timing only
        /// </summary>
		public Posef GetHmdPosePerEye(Eye eye)
		{
			return ovrHmd_GetHmdPosePerEye(HmdPtr, eye);
		}

		//-------------------------------------------------------------------------------------
		// *****  Client Distortion Rendering Functions

		// These functions provide the distortion data and render timing support necessary to allow
		// client rendering of distortion. Client-side rendering involves the following steps:
		//
		//  1. Setup ovrEyeDesc based on the desired texture size and FOV.
		//     Call ovrHmd_GetRenderDesc to get the necessary rendering parameters for each eye.
		//
		//  2. Use ovrHmd_CreateDistortionMesh to generate the distortion mesh.
		//
		//  3. Use ovrHmd_BeginFrameTiming, ovrHmd_GetEyePoses, and ovrHmd_BeginFrameTiming in
		//     the rendering loop to obtain timing and predicted head orientation when rendering each eye.
		//      - When using timewarp, use ovr_WaitTillTime after the rendering and gpu flush, followed
		//        by ovrHmd_GetEyeTimewarpMatrices to obtain the timewarp matrices used
		//        by the distortion pixel shader. This will minimize latency.
		//

        /// <summary>
		/// Computes the distortion viewport, view adjust, and other rendering parameters for
		/// the specified eye. This can be used instead of ovrHmd_ConfigureRendering to do
		/// setup for client rendered distortion.
        /// </summary>
		public EyeRenderDesc GetRenderDesc(Eye eyeType, FovPort fov)
		{
			return ovrHmd_GetRenderDesc(HmdPtr, eyeType, fov);
		}

        /// <summary>
		/// Generate distortion mesh per eye.
		/// Distortion capabilities will depend on 'distortionCaps' flags. Users should
		/// render using the appropriate shaders based on their settings.
		/// Distortion mesh data will be allocated and written into the ovrDistortionMesh data structure,
		/// which should be explicitly freed with ovrHmd_DestroyDistortionMesh.
		/// Users should call ovrHmd_GetRenderScaleAndOffset to get uvScale and Offset values for rendering.
		/// The function shouldn't fail unless theres is a configuration or memory error, in which case
		/// ovrDistortionMesh values will be set to null.
		/// This is the only function in the SDK reliant on eye relief, currently imported from profiles,
		/// or overridden here.
        /// </summary>
		public DistortionMesh? CreateDistortionMesh(Eye eye, FovPort fov, uint distortionCaps)
		{
			DistortionMesh_Raw rawMesh = new DistortionMesh_Raw();

            bool result = ovrHmd_CreateDistortionMesh(HmdPtr, eye, fov, distortionCaps, out rawMesh) != 0;
            if (!result)
			{
				return null;
			}

			DistortionMesh mesh = new DistortionMesh(rawMesh);
			ovrHmd_DestroyDistortionMesh(ref rawMesh);
			return mesh;
		}

        /// <summary>
		/// Computes updated 'uvScaleOffsetOut' to be used with a distortion if render target size or
		/// viewport changes after the fact. This can be used to adjust render size every frame if desired.
        /// </summary>
		public Vector2f[] GetRenderScaleAndOffset(FovPort fov, Sizei textureSize, Recti renderViewport)
		{
			Vector2f[] uvScaleOffsetOut = new Vector2f[] { new Vector2f(), new Vector2f() };
			ovrHmd_GetRenderScaleAndOffset(fov, textureSize, renderViewport, uvScaleOffsetOut);
			return uvScaleOffsetOut;
		}

        /// <summary>
		/// Thread-safe timing function for the main thread. Caller should increment frameIndex
		/// with every frame and pass the index where applicable to functions called on the
		/// rendering thread.
        /// </summary>
		public FrameTiming GetFrameTiming(uint frameIndex)
		{
			FrameTiming_Raw raw = ovrHmd_GetFrameTiming(HmdPtr, frameIndex);
			return new FrameTiming(raw);
		}

        /// <summary>
		/// Called at the beginning of the frame on the rendering thread.
		/// Pass frameIndex == 0 if ovrHmd_GetFrameTiming isn't being used. Otherwise,
		/// pass the same frame index as was used for GetFrameTiming on the main thread.
        /// </summary>
		public FrameTiming BeginFrameTiming(uint frameIndex)
		{
			FrameTiming_Raw raw = ovrHmd_BeginFrameTiming(HmdPtr, frameIndex);
			return new FrameTiming(raw);
		}

        /// <summary>
		/// Marks the end of client distortion rendered frame, tracking the necessary timing information.
		/// This function must be called immediately after Present/SwapBuffers + GPU sync. GPU sync is
		/// important before this call to reduce latency and ensure proper timing.
        /// </summary>
		public void EndFrameTiming()
		{
			ovrHmd_EndFrameTiming(HmdPtr);
		}

        /// <summary>
		/// Initializes and resets frame time tracking. This is typically not necessary, but
		/// is helpful if game changes vsync state or video mode. vsync is assumed to be on if this
		/// isn't called. Resets internal frame index to the specified number.
        /// </summary>
		public void ResetFrameTiming(uint frameIndex)
		{
			ovrHmd_ResetFrameTiming(HmdPtr, frameIndex);
		}

        /// <summary>
		/// Computes timewarp matrices used by distortion mesh shader, these are used to adjust
		/// for head orientation change since the last call to ovrHmd_GetEyePoses
		/// when rendering this eye. The ovrDistortionVertex::TimeWarpFactor is used to blend between the
		/// matrices, usually representing two different sides of the screen.
		/// Must be called on the same thread as ovrHmd_BeginFrameTiming.
        /// </summary>
		public Matrix4f[] GetEyeTimewarpMatrices(Eye eye, Posef renderPose)
		{
			Matrix4f_Raw[] rawMats = {new Matrix4f_Raw(), new Matrix4f_Raw()};
			ovrHmd_GetEyeTimewarpMatrices(HmdPtr, eye, renderPose, rawMats);

			Matrix4f[] mats = {new Matrix4f(rawMats[0]), new Matrix4f(rawMats[1])};
			return mats;
		}

		// -----------------------------------------------------------------------------------
		// ***** Latency Test interface

        /// <summary>
		/// Does latency test processing and returns 'TRUE' if specified rgb color should
		/// be used to clear the screen.
        /// </summary>
		public byte[] ProcessLatencyTest()
		{
            if (ovrHmd_ProcessLatencyTest(HmdPtr, LatencyTestRgb) != 0)
				return LatencyTestRgb;
			return null;
		}

        /// <summary>
		/// Returns non-null string once with latency test result, when it is available.
		/// Buffer is valid until next call.
        /// </summary>
		public string GetLatencyTestResult()
		{
			IntPtr p = ovrHmd_GetLatencyTestResult(HmdPtr);
			return (p == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(p);
		}

        /// <summary>
		/// Returns the latency testing color in rgbColorOut to render when using a DK2
		/// Returns false if this feature is disabled or not-applicable (e.g. using a DK1)
        /// </summary>
		public byte[] GetLatencyTest2DrawColor()
		{
            if (ovrHmd_GetLatencyTest2DrawColor(HmdPtr, LatencyTestRgb) != 0)
				return LatencyTestRgb;
			return null;
		}

		//-------------------------------------------------------------------------------------
		// ***** Health and Safety Warning Display interface
		//

        /// <summary>
		/// Returns the current state of the HSW display. If the application is doing the rendering of
		/// the HSW display then this function serves to indicate that the warning should be
		/// currently displayed. If the application is using SDK-based eye rendering then the SDK by
		/// default automatically handles the drawing of the HSW display. An application that uses
		/// application-based eye rendering should use this function to know when to start drawing the
		/// HSW display itself and can optionally use it in conjunction with ovrhmd_DismissHSWDisplay
		/// as described below.
		///
		/// Example usage for application-based rendering:
		///    bool HSWDisplayCurrentlyDisplayed = false; // global or class member variable
		///    ovrHSWDisplayState hswDisplayState = hmd.GetHSWDisplayState();
		///
		///    if (hswDisplayState.Displayed && !HSWDisplayCurrentlyDisplayed)
		///    {
		///        <insert model into the scene that stays in front of the user>
		///        HSWDisplayCurrentlyDisplayed = true;
		///    }
        /// </summary>
		public HSWDisplayState GetHSWDisplayState()
		{
			HSWDisplayState hswDisplayState;
			ovrHmd_GetHSWDisplayState(HmdPtr, out hswDisplayState);
			return hswDisplayState;
		}

        /// <summary>
		/// Dismisses the HSW display if the warning is dismissible and the earliest dismissal time
		/// has occurred. Returns true if the display is valid and could be dismissed. The application
		/// should recognize that the HSW display is being displayed (via ovrhmd_GetHSWDisplayState)
		/// and if so then call this function when the appropriate user input to dismiss the warning
		/// occurs.
		///
		/// Example usage :
		///    void ProcessEvent(int key)
		///    {
		///        if (key == escape)
		///        {
		///            ovrHSWDisplayState hswDisplayState = hmd.GetHSWDisplayState();
		///
		///            if (hswDisplayState.Displayed && hmd.DismissHSWDisplay())
		///            {
		///                <remove model from the scene>
		///                HSWDisplayCurrentlyDisplayed = false;
		///            }
		///        }
		///    }
        /// <summary>
		public bool DismissHSWDisplay()
		{
            return ovrHmd_DismissHSWDisplay(HmdPtr) != 0;
		}

		// -----------------------------------------------------------------------------------
		// ***** Property Access

		// NOTICE: This is experimental part of API that is likely to go away or change.

		// These allow accessing different properties of the HMD and profile.
		// Some of the properties may go away with profile/HMD versions, so software should
		// use defaults and/or proper fallbacks.

        /// <summary>
		/// Get boolean property. Returns first element if property is a boolean array.
		/// Returns defaultValue if property doesn't exist.
        /// </summary>
		public bool GetBool(string propertyName, bool defaultVal = false)
		{
            return ovrHmd_GetBool(HmdPtr, propertyName, defaultVal) != 0;
		}

        /// <summary>
        /// Modify bool property; false if property doesn't exist or is readonly.
        /// </summary>
		public bool SetBool(string propertyName, bool val)
		{
            return ovrHmd_SetBool(HmdPtr, propertyName, val) != 0;
		}

        /// <summary>
		/// Get integer property. Returns first element if property is an integer array.
		/// Returns defaultValue if property doesn't exist.
        /// </summary>
		public int GetInt(string propertyName, int defaultVal = 0)
		{
			return ovrHmd_GetInt(HmdPtr, propertyName, defaultVal);
		}

        /// <summary>
		/// Modify integer property; false if property doesn't exist or is readonly.
        /// </summary>
		public bool SetInt(string propertyName, int val)
		{
            return ovrHmd_SetInt(HmdPtr, propertyName, val) != 0;
		}

        /// <summary>
		/// Get float property. Returns first element if property is a float array.
		/// Returns defaultValue if property doesn't exist.
        /// </summary>
		public float GetFloat(string propertyName, float defaultVal = 0.0f)
		{
			return ovrHmd_GetFloat(HmdPtr, propertyName, defaultVal);
		}

        /// <summary>
		/// Modify float property; false if property doesn't exist or is readonly.
        /// </summary>
		public bool SetFloat(string propertyName, float val)
		{
            return ovrHmd_SetFloat(HmdPtr, propertyName, val) != 0;
		}

        /// <summary>
		/// Get float[] property. Returns the number of elements filled in, 0 if property doesn't exist.
		/// Maximum of arraySize elements will be written.
        /// </summary>
		public float[] GetFloatArray(string propertyName, float[] values)
		{
			if (values == null)
				return null;

			ovrHmd_GetFloatArray(HmdPtr, propertyName, values, (uint)values.Length);
			return values;
		}

        /// <summary>
		/// Modify float[] property; false if property doesn't exist or is readonly.
        /// </summary>
		public bool SetFloatArray(string propertyName, float[] values)
		{
			if (values == null)
				values = new float[0];

            return ovrHmd_SetFloatArray(HmdPtr, propertyName, values, (uint)values.Length) != 0;
		}

        /// <summary>
		/// Get string property. Returns first element if property is a string array.
		/// Returns defaultValue if property doesn't exist.
		/// String memory is guaranteed to exist until next call to GetString or GetStringArray, or HMD is destroyed.
        /// </summary>
		public string GetString(string propertyName, string defaultVal = null)
		{
			IntPtr p = ovrHmd_GetString(HmdPtr, propertyName, null);
			if (p == IntPtr.Zero)
				return defaultVal;
			return Marshal.PtrToStringAnsi(p);
		}

        /// <summary>
		/// Set string property
        /// </summary>
		public bool SetString(string propertyName, string val)
		{
            return ovrHmd_SetString(HmdPtr, propertyName, val) != 0;
		}

		// -----------------------------------------------------------------------------------
		// ***** Logging

        /// <summary>
		/// Start performance logging. guid is optional and if included is written with each file entry.
		/// If called while logging is already active with the same filename, only the guid will be updated
		/// If called while logging is already active with a different filename, ovrHmd_StopPerfLog() will be called, followed by ovrHmd_StartPerfLog()
        /// </summary>
		public bool StartPerfLog(string fileName, string userData1)
		{
            return ovrHmd_StartPerfLog(HmdPtr, fileName, userData1) != 0;
		}

        /// <summary>
		/// Stop performance logging.
        /// </summary>
		public bool StopPerfLog()
		{
            return ovrHmd_StopPerfLog(HmdPtr) != 0;
		}

		public const string LibFile = "OculusPlugin";

		// Imported functions from
		// OVRPlugin.dll    (PC)
		// OVRPlugin.bundle (OSX)
		// OVRPlugin.so     (Linux, Android)

		// -----------------------------------------------------------------------------------
		// ***** Private Interface to libOVR
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovr_InitializeRenderingShim();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovr_Initialize();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovr_Shutdown();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ovr_GetVersionString();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern int ovrHmd_Detect();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ovrHmd_Create(int index);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_Destroy(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ovrHmd_CreateDebug(HmdType type);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern string ovrHmd_GetLastError(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_AttachToWindow(
				IntPtr hmd,
			   	IntPtr window,
			   	Recti destMirrorRect,
			   	Recti sourceRenderTargetRect);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint ovrHmd_GetEnabledCaps(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_SetEnabledCaps(IntPtr hmd, uint capsBits);

		//-------------------------------------------------------------------------------------
		// ***** Sensor Interface
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_ConfigureTracking(
				IntPtr hmd,
			   	uint supportedTrackingCaps,
			   	uint requiredTrackingCaps);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_RecenterPose(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern TrackingState ovrHmd_GetTrackingState(IntPtr hmd, double absTime);

		//-------------------------------------------------------------------------------------
		// ***** Graphics Setup
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern Sizei ovrHmd_GetFovTextureSize(
				IntPtr hmd,
				Eye eye,
			   	FovPort fov,
			   	float pixelsPerDisplayPixel);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_ConfigureRendering(
				IntPtr hmd,
				ref RenderAPIConfig_Raw apiConfig,
				uint distortionCaps,
				[In] FovPort[] eyeFovIn,
				[In, Out] EyeRenderDesc[] eyeRenderDescOut);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern FrameTiming_Raw ovrHmd_BeginFrame(IntPtr hmd, uint frameIndex);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_EndFrame(
				IntPtr hmd,
			   	[In] Posef[] renderPose,
			   	[In] Texture_Raw[] eyeTexture);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_GetEyePoses(
                IntPtr hmd,
                uint frameIndex,
				[In] Vector3f[] hmdToEyeViewOffset,
				[In, Out] Posef[] eyePosesOut,
				[In, Out] ref TrackingState hmdTrackingStateOut);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern Posef ovrHmd_GetHmdPosePerEye(
				IntPtr hmd,
				Eye eye);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern EyeRenderDesc ovrHmd_GetRenderDesc(IntPtr hmd, Eye eye, FovPort fov);

		// -----------------------------------------------------------------------------------
		// **** Game-side rendering API
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_CreateDistortionMesh(
				IntPtr hmd,
				Eye eye,
				FovPort fov,
				uint distortionCaps,
				[Out] out DistortionMesh_Raw meshData);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_DestroyDistortionMesh(ref DistortionMesh_Raw meshData);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_GetRenderScaleAndOffset(
				FovPort fov,
				Sizei textureSize,
				Recti renderViewport,
				[MarshalAs(UnmanagedType.LPArray, SizeConst = 2)]
				[Out] Vector2f[] uvScaleOffsetOut);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern FrameTiming_Raw ovrHmd_GetFrameTiming(IntPtr hmd, uint frameIndex);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern FrameTiming_Raw ovrHmd_BeginFrameTiming(IntPtr hmd, uint frameIndex);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_EndFrameTiming(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_ResetFrameTiming(IntPtr hmd, uint frameIndex);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_GetEyeTimewarpMatrices(
				IntPtr hmd,
			   	Eye eye,
			   	Posef renderPose,
				[MarshalAs(UnmanagedType.LPArray, SizeConst = 2)]
				[Out] Matrix4f_Raw[] twnOut);

		//-------------------------------------------------------------------------------------
		// Stateless math setup functions
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern Matrix4f_Raw ovrMatrix4f_Projection(
				FovPort fov,
			   	float znear,
			   	float zfar,
			   	bool rightHanded);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern Matrix4f_Raw ovrMatrix4f_OrthoSubProjection(
				Matrix4f projection,
				Vector2f orthoScale,
				float orthoDistance,
				float hmdToEyeViewOffsetX);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern double ovr_GetTimeInSeconds();
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern double ovr_WaitTillTime(double absTime);

		// -----------------------------------------------------------------------------------
		// ***** Latency Test interface
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_ProcessLatencyTest(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPArray, SizeConst = 3)]
				[Out] byte[] rgbColorOut);
		// Returns IntPtr to avoid allocation.
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ovrHmd_GetLatencyTestResult(IntPtr hmd);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_GetLatencyTest2DrawColor(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPArray, SizeConst = 3)]
				[Out] byte[] rgbColorOut);

		//-------------------------------------------------------------------------------------
		// ***** Health and Safety Warning Display interface
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern void ovrHmd_GetHSWDisplayState(
				IntPtr hmd,
			   	[Out] out HSWDisplayState hasWarningState);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern sbyte ovrHmd_DismissHSWDisplay(IntPtr hmd);
		
		// -----------------------------------------------------------------------------------
		// ***** Property Access
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_GetBool(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				bool defaultVal);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_SetBool(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				bool val);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern int ovrHmd_GetInt(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				int defaultVal);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_SetInt(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				int val);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern float ovrHmd_GetFloat(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				float defaultVal);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_SetFloat(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				float val);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint ovrHmd_GetFloatArray(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				float[] values, // TBD: Passing var size?
				uint arraySize);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_SetFloatArray(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				float[] values, // TBD: Passing var size?
				uint arraySize);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr ovrHmd_GetString(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				[MarshalAs(UnmanagedType.LPStr)]
				string defaultVal);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_SetString(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string propertyName,
				[MarshalAs(UnmanagedType.LPStr)]
				string val);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_StartPerfLog(
				IntPtr hmd,
				[MarshalAs(UnmanagedType.LPStr)]
				string fileName,
				[MarshalAs(UnmanagedType.LPStr)]
				string userData1);
		[DllImport(LibFile, CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte ovrHmd_StopPerfLog(IntPtr hmd);
	}
}
