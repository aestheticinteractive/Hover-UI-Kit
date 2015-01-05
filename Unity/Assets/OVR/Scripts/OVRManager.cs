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
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Ovr;

/// <summary>
/// Configuration data for Oculus virtual reality.
/// </summary>
public class OVRManager : MonoBehaviour
{
	/// <summary>
	/// Contains information about the user's preferences and body dimensions.
	/// </summary>
	public struct Profile
	{
		public float ipd;
		public float eyeHeight;
		public float eyeDepth;
		public float neckHeight;
	}

	/// <summary>
	/// Gets the singleton instance.
	/// </summary>
	public static OVRManager instance { get; private set; }
		
	/// <summary>
	/// Gets a reference to the low-level C API Hmd Wrapper
	/// </summary>
	private static Hmd _capiHmd;
	public static Hmd capiHmd
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			if (_capiHmd == null)
			{
				IntPtr hmdPtr = IntPtr.Zero;
				OVR_GetHMD(ref hmdPtr);
				_capiHmd = (hmdPtr != IntPtr.Zero) ? new Hmd(hmdPtr) : null;
			}
#else
			_capiHmd = null;
#endif
			return _capiHmd;
		}
	}
		
	/// <summary>
	/// Gets a reference to the active OVRDisplay
	/// </summary>
	public static OVRDisplay display { get; private set; }

	/// <summary>
	/// Gets a reference to the active OVRTracker
	/// </summary>
	public static OVRTracker tracker { get; private set; }
	
	/// <summary>
	/// Gets the current profile, which contains information about the user's settings and body dimensions.
	/// </summary>
	private static bool _profileIsCached = false;
	private static Profile _profile;
	public static Profile profile
	{
		get {
			if (!_profileIsCached)
			{
#if !UNITY_ANDROID || UNITY_EDITOR
				float ipd = capiHmd.GetFloat(Hmd.OVR_KEY_IPD, Hmd.OVR_DEFAULT_IPD);
				float eyeHeight = capiHmd.GetFloat(Hmd.OVR_KEY_EYE_HEIGHT, Hmd.OVR_DEFAULT_EYE_HEIGHT);
				float[] defaultOffset = new float[] { Hmd.OVR_DEFAULT_NECK_TO_EYE_HORIZONTAL, Hmd.OVR_DEFAULT_NECK_TO_EYE_VERTICAL };
				float[] neckToEyeOffset = capiHmd.GetFloatArray(Hmd.OVR_KEY_NECK_TO_EYE_DISTANCE, defaultOffset);
				float neckHeight = eyeHeight - neckToEyeOffset[1];
				
				_profile = new Profile
				{
					ipd = ipd,
					eyeHeight = eyeHeight,
					eyeDepth = neckToEyeOffset[0],
					neckHeight = neckHeight,
				};
#else
				float ipd = 0.0f;
				OVR_GetInterpupillaryDistance(ref ipd);
				
				float eyeHeight = 0.0f;
				OVR_GetPlayerEyeHeight(ref eyeHeight);
				
				_profile = new Profile
				{
					ipd = ipd,
					eyeHeight = eyeHeight,
					eyeDepth = 0f, //TODO
					neckHeight = 0.0f, // TODO
				};
#endif
				_profileIsCached = true;
			}

			return _profile;
		}
	}

	/// <summary>
	/// Occurs when an HMD attached.
	/// </summary>
	public static event Action HMDAcquired;

	/// <summary>
	/// Occurs when an HMD detached.
	/// </summary>
	public static event Action HMDLost;

	/// <summary>
	/// Occurs when the tracker gained tracking.
	/// </summary>
	public static event Action TrackingAcquired;

	/// <summary>
	/// Occurs when the tracker lost tracking.
	/// </summary>
	public static event Action TrackingLost;
	
	/// <summary>
	/// Occurs when HSW dismissed.
	/// </summary>
	public static event Action HSWDismissed;
	
	/// <summary>
	/// If true, then the Oculus health and safety warning (HSW) is currently visible.
	/// </summary>
	public static bool isHSWDisplayed
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return capiHmd.GetHSWDisplayState().Displayed;
#else
			return false;
#endif
		}
	}
	
	/// <summary>
	/// If the HSW has been visible for the necessary amount of time, this will make it disappear.
	/// </summary>
	public static void DismissHSWDisplay()
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		capiHmd.DismissHSWDisplay();
#endif
	}
	
	/// <summary>
	/// Gets the current battery level.
	/// </summary>
	/// <returns><c>battery level in the range [0.0,1.0]</c>
	/// <param name="batteryLevel">Battery level.</param>
	public static float batteryLevel
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return 1.0f;
#else
			return OVR_GetBatteryLevel();
#endif
		}
	}
	
	/// <summary>
	/// Gets the current battery temperature.
	/// </summary>
	/// <returns><c>battery temperature in Celsius</c>
	/// <param name="batteryTemperature">Battery temperature.</param>
	public static float batteryTemperature
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return 0.0f;
#else
			return OVR_GetBatteryTemperature();
#endif
		}
	}
	
	/// <summary>
	/// Gets the current battery status.
	/// </summary>
	/// <returns><c>battery status</c>
	/// <param name="batteryStatus">Battery status.</param>
	public static int batteryStatus
	{
		get {
#if !UNITY_ANDROID || UNITY_EDITOR
			return 0;
#else
			return OVR_GetBatteryStatus();
#endif
		}
	}

	/// <summary>
	/// Controls the size of the eye textures.
	/// Values must be above 0.
	/// Values below 1 permit sub-sampling for improved performance.
	/// Values above 1 permit super-sampling for improved sharpness.
	/// </summary>
	public float nativeTextureScale = 1.0f;
	
	/// <summary>
	/// Controls the size of the rendering viewport.
	/// Values must be between 0 and 1.
	/// Values below 1 permit dynamic sub-sampling for improved performance.
	/// </summary>
	public float virtualTextureScale = 1.0f;

	/// <summary>
	/// If true, head tracking will affect the orientation of each OVRCameraRig's cameras.
	/// </summary>
	public bool usePositionTracking = true;

	/// <summary>
	/// The format of each eye texture.
	/// </summary>
	public RenderTextureFormat eyeTextureFormat = RenderTextureFormat.Default;

	/// <summary>
	/// The depth of each eye texture in bits.
	/// </summary>
	public int eyeTextureDepth = 24;

	/// <summary>
	/// If true, TimeWarp will be used to correct the output of each OVRCameraRig for rotational latency.
	/// </summary>
	public bool timeWarp = true;

	/// <summary>
	/// If this is true and TimeWarp is true, each OVRCameraRig will stop tracking and only TimeWarp will respond to head motion.
	/// </summary>
	public bool freezeTimeWarp = false;

	/// <summary>
	/// If true, each scene load will cause the head pose to reset.
	/// </summary>
	public bool resetTrackerOnLoad = true;

	/// <summary>
	/// If true, the eyes see the same image, which is rendered only by the left camera.
	/// </summary>
	public bool monoscopic = false;

	/// <summary>
	/// True if the current platform supports virtual reality.
	/// </summary>
    public bool isSupportedPlatform { get; private set; }
	
	private static bool usingPositionTracking = false;
	private static bool wasHmdPresent = false;
	private static bool wasPositionTracked = false;
	private static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

#if UNITY_ANDROID && !UNITY_EDITOR
	// Get this from Unity on startup so we can call Activity java functions
	private static bool androidJavaInit = false;
	private static AndroidJavaObject activity;
	private static AndroidJavaClass javaVrActivityClass;
	internal static int timeWarpViewNumber = 0;
	public static event Action OnCustomPostRender;
#else
	private static bool ovrIsInitialized;
	private static bool isQuitting;
#endif

    public static bool isPaused
    {
        get { return _isPaused; }
        set
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			RenderEventType eventType = (value) ? RenderEventType.Pause : RenderEventType.Resume;
			OVRPluginEvent.Issue(eventType);
#endif
            _isPaused = value;
        }
    }
    private static bool _isPaused;

#region Unity Messages

	private void Awake()
	{
		// Only allow one instance at runtime.
		if (instance != null)
		{
			enabled = false;
			DestroyImmediate(this);
			return;
		}

		instance = this;

#if !UNITY_ANDROID || UNITY_EDITOR
		if (!ovrIsInitialized)
		{
			OVR_Initialize();
			OVRPluginEvent.Issue(RenderEventType.Initialize);

			ovrIsInitialized = true;
		}

		var netVersion = new System.Version(Ovr.Hmd.OVR_VERSION_STRING);
		var ovrVersion = new System.Version(Ovr.Hmd.GetVersionString());
		if (netVersion > ovrVersion)
			Debug.LogWarning("Using an older version of LibOVR.");
#endif

        // Detect whether this platform is a supported platform
        RuntimePlatform currPlatform = Application.platform;
        isSupportedPlatform |= currPlatform == RuntimePlatform.Android;
        isSupportedPlatform |= currPlatform == RuntimePlatform.LinuxPlayer;
        isSupportedPlatform |= currPlatform == RuntimePlatform.OSXEditor;
        isSupportedPlatform |= currPlatform == RuntimePlatform.OSXPlayer;
        isSupportedPlatform |= currPlatform == RuntimePlatform.WindowsEditor;
        isSupportedPlatform |= currPlatform == RuntimePlatform.WindowsPlayer;
        if (!isSupportedPlatform)
        {
            Debug.LogWarning("This platform is unsupported");
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
		Application.targetFrameRate = 60;
		// don't allow the app to run in the background
		Application.runInBackground = false;
		// Disable screen dimming
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		if (!androidJavaInit)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			javaVrActivityClass = new AndroidJavaClass("com.oculusvr.vrlib.VrActivity");
			// Prepare for the RenderThreadInit()
			SetInitVariables(activity.GetRawObject(), javaVrActivityClass.GetRawClass());
			
			androidJavaInit = true;
		}

		// We want to set up our touchpad messaging system
		OVRTouchpad.Create();
		// This will trigger the init on the render thread
		InitRenderThread();
#else
		SetEditorPlay(Application.isEditor);
#endif

		if (display == null)
			display = new OVRDisplay();
		if (tracker == null)
			tracker = new OVRTracker();

		if (resetTrackerOnLoad)
			display.RecenterPose();

		// Except for D3D9, SDK rendering forces vsync unless you pass ovrHmdCap_NoVSync to Hmd.SetEnabledCaps().
		if (timeWarp)
		{
			bool useUnityVSync = SystemInfo.graphicsDeviceVersion.Contains("Direct3D 9");
			QualitySettings.vSyncCount = useUnityVSync ? 1 : 0;
		}

#if (UNITY_STANDALONE_WIN && (UNITY_4_6 || UNITY_4_5))
		bool unity_4_6 = false;
		bool unity_4_5_2 = false;
		bool unity_4_5_3 = false;
		bool unity_4_5_4 = false;
		bool unity_4_5_5 = false;

#if (UNITY_4_6)
		unity_4_6 = true;
#elif (UNITY_4_5_2)
		unity_4_5_2 = true;
#elif (UNITY_4_5_3)
		unity_4_5_3 = true;
#elif (UNITY_4_5_4)
		unity_4_5_4 = true;
#elif (UNITY_4_5_5)
		unity_4_5_5 = true;
#endif

		// Detect correct Unity releases which contain the fix for D3D11 exclusive mode.
		string version = Application.unityVersion;
		int releaseNumber;
		bool releaseNumberFound = Int32.TryParse(Regex.Match(version, @"\d+$").Value, out releaseNumber);

		// Exclusive mode was broken for D3D9 in Unity 4.5.2p2 - 4.5.4 and 4.6 builds prior to beta 21
		bool unsupportedExclusiveModeD3D9 = (unity_4_6 && version.Last(char.IsLetter) == 'b' && releaseNumberFound && releaseNumber < 21)
			|| (unity_4_5_2 && version.Last(char.IsLetter) == 'p' && releaseNumberFound && releaseNumber >= 2)
			|| (unity_4_5_3)
			|| (unity_4_5_4);

		// Exclusive mode was broken for D3D11 in Unity 4.5.2p2 - 4.5.5p2 and 4.6 builds prior to f1
		bool unsupportedExclusiveModeD3D11 = (unity_4_6 && version.Last(char.IsLetter) == 'b')
			|| (unity_4_5_2 && version.Last(char.IsLetter) == 'p' && releaseNumberFound && releaseNumber >= 2)
			|| (unity_4_5_3)
			|| (unity_4_5_4)
			|| (unity_4_5_5 && version.Last(char.IsLetter) == 'f')
			|| (unity_4_5_5 && version.Last(char.IsLetter) == 'p' && releaseNumberFound && releaseNumber < 3);

		if (unsupportedExclusiveModeD3D9 && !display.isDirectMode && SystemInfo.graphicsDeviceVersion.Contains("Direct3D 9"))
		{
			MessageBox(0, "Direct3D 9 extended mode is not supported in this configuration. "
				+ "Please use direct display mode, a different graphics API, or rebuild the application with a newer Unity version."
				, "VR Configuration Warning", 0);
		}

		if (unsupportedExclusiveModeD3D11 && !display.isDirectMode && SystemInfo.graphicsDeviceVersion.Contains("Direct3D 11"))
		{
			MessageBox(0, "Direct3D 11 extended mode is not supported in this configuration. "
				+ "Please use direct display mode, a different graphics API, or rebuild the application with a newer Unity version."
				, "VR Configuration Warning", 0);
		}
#endif
	}

#if !UNITY_ANDROID || UNITY_EDITOR
	private void OnApplicationQuit()
	{
		isQuitting = true;
	}

	private void OnDisable()
	{
		if (!isQuitting)
			return;

		if (ovrIsInitialized)
		{
			OVR_Destroy();
			OVRPluginEvent.Issue(RenderEventType.Destroy);
			_capiHmd = null;

			ovrIsInitialized = false;
		}
	}
#endif

	private void Start()
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		Camera cam = GetComponent<Camera>();
		if (cam == null)
		{
			// Ensure there is a non-RT camera in the scene to force rendering of the left and right eyes.
			cam = gameObject.AddComponent<Camera>();
			cam.cullingMask = 0;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.0f, 0.0f, 0.0f);
			cam.renderingPath = RenderingPath.Forward;
			cam.orthographic = true;
			cam.useOcclusionCulling = false;
		}
#endif

		bool isD3d = SystemInfo.graphicsDeviceVersion.Contains("Direct3D") ||
			Application.platform == RuntimePlatform.WindowsEditor &&
				SystemInfo.graphicsDeviceVersion.Contains("emulated");
		display.flipInput = isD3d;

		StartCoroutine(CallbackCoroutine());
	}

	private void Update()
	{
		if (usePositionTracking != usingPositionTracking)
		{
			tracker.isEnabled = usePositionTracking;
			usingPositionTracking = usePositionTracking;
		}

		// Dispatch any events.
		if (HMDLost != null && wasHmdPresent && !display.isPresent)
			HMDLost();

		if (HMDAcquired != null && !wasHmdPresent && display.isPresent)
			HMDAcquired();

		wasHmdPresent = display.isPresent;

		if (TrackingLost != null && wasPositionTracked && !tracker.isPositionTracked)
			TrackingLost();

		if (TrackingAcquired != null && !wasPositionTracked && tracker.isPositionTracked)
			TrackingAcquired();

		wasPositionTracked = tracker.isPositionTracked;
		
		if (isHSWDisplayed && Input.anyKeyDown)
		{
			DismissHSWDisplay();
			
			if (HSWDismissed != null)
				HSWDismissed();
		}		
		
		display.timeWarp = timeWarp;

#if (!UNITY_ANDROID || UNITY_EDITOR)
		display.Update();
#endif
	}

#if (UNITY_EDITOR_OSX)
	private void OnPreCull() // TODO: Fix Mac Unity Editor memory corruption issue requiring OnPreCull workaround.
#else
	private void LateUpdate()
#endif
	{
#if (!UNITY_ANDROID || UNITY_EDITOR)
		display.BeginFrame();
#endif
	}

	private IEnumerator CallbackCoroutine()
	{
        while (true)
        {
			yield return waitForEndOfFrame;

#if UNITY_ANDROID && !UNITY_EDITOR
			OVRManager.DoTimeWarp(timeWarpViewNumber);
#else
			display.EndFrame();
#endif
        }
	}
	
#if UNITY_ANDROID && !UNITY_EDITOR
	private void OnPause()
	{
		isPaused = true;
	}
	
	private void OnApplicationPause(bool pause)
	{
		Debug.Log("OnApplicationPause() " + pause);
		if (pause)
		{
			OnPause();
		}
		else
		{
			StartCoroutine(OnResume());
		}
	}
	
	void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator OnResume()
	{
		yield return null; // delay 1 frame to allow Unity enough time to create the windowSurface

		isPaused = false;
	}

	/// <summary>
	/// Leaves the application/game and returns to the launcher/dashboard
	/// </summary>
	public void ReturnToLauncher()
	{
		// show the platform UI quit prompt
		OVRManager.PlatformUIConfirmQuit();
	}
	
	private void OnPostRender()
	{
		// Allow custom code to render before we kick off the plugin
		if (OnCustomPostRender != null)
		{
			OnCustomPostRender();
		}
		
		EndEye(OVREye.Left, display.GetEyeTextureId(OVREye.Left));
		EndEye(OVREye.Right, display.GetEyeTextureId(OVREye.Right));
	}
#endif
#endregion

    public static void SetEditorPlay(bool isEditor)
    {
#if !UNITY_ANDROID || UNITY_EDITOR
        OVR_SetEditorPlay(isEditor);
#endif
    }

    public static void SetDistortionCaps(uint distortionCaps)
    {
#if !UNITY_ANDROID || UNITY_EDITOR
        OVR_SetDistortionCaps(distortionCaps);
#endif
    }

    public static void SetInitVariables(IntPtr activity, IntPtr vrActivityClass)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		OVR_SetInitVariables(activity, vrActivityClass);
#endif
    }

    public static void PlatformUIConfirmQuit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRPluginEvent.Issue(RenderEventType.PlatformUIConfirmQuit);
#endif
    }

    public static void PlatformUIGlobalMenu()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRPluginEvent.Issue(RenderEventType.PlatformUI);
#endif
    }

    public static void DoTimeWarp(int timeWarpViewNumber)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRPluginEvent.IssueWithData(RenderEventType.TimeWarp, timeWarpViewNumber);
#endif
    }

    public static void EndEye(OVREye eye, int eyeTextureId)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		RenderEventType eventType = (eye == OVREye.Left) ?
			RenderEventType.LeftEyeEndFrame :
			RenderEventType.RightEyeEndFrame;

		OVRPluginEvent.IssueWithData(eventType, eyeTextureId);
#endif
    }

    public static void InitRenderThread()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRPluginEvent.Issue(RenderEventType.InitRenderThread);
#endif
    }

    private const string LibOVR = "OculusPlugin";

#if !UNITY_ANDROID || UNITY_EDITOR
	[DllImport(LibOVR, CallingConvention = CallingConvention.Cdecl)]
	private static extern void OVR_GetHMD(ref IntPtr hmdPtr);
    [DllImport(LibOVR, CallingConvention = CallingConvention.Cdecl)]
    private static extern void OVR_SetEditorPlay(bool isEditorPlay);
    [DllImport(LibOVR, CallingConvention = CallingConvention.Cdecl)]
    private static extern void OVR_SetDistortionCaps(uint distortionCaps);
	[DllImport(LibOVR, CallingConvention = CallingConvention.Cdecl)]
	private static extern void OVR_Initialize();
	[DllImport(LibOVR, CallingConvention = CallingConvention.Cdecl)]
	private static extern void OVR_Destroy();

#if UNITY_STANDALONE_WIN
	[DllImport("user32", EntryPoint = "MessageBoxA", CharSet = CharSet.Ansi)]
	public static extern bool MessageBox(int hWnd,
	                                     [MarshalAs(UnmanagedType.LPStr)]string text,
	                                     [MarshalAs(UnmanagedType.LPStr)]string caption, uint type);
#endif

#else
	[DllImport(LibOVR)]
	private static extern void OVR_SetInitVariables(IntPtr activity, IntPtr vrActivityClass);
	[DllImport(LibOVR)]
	private static extern float OVR_GetBatteryLevel();
	[DllImport(LibOVR)]
	private static extern int OVR_GetBatteryStatus();
	[DllImport(LibOVR)]
	private static extern float OVR_GetBatteryTemperature();

	[DllImport(LibOVR)]
	private static extern bool OVR_GetPlayerEyeHeight(ref float eyeHeight);
	[DllImport(LibOVR)]
	private static extern bool OVR_GetInterpupillaryDistance(ref float interpupillaryDistance);
#endif
}
