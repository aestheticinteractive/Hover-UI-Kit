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

// #define SHOW_DK2_VARIABLES

// Use the Unity new GUI with Unity 4.6 or above.
#if UNITY_4_6 || UNITY_5_0
#define USE_NEW_GUI
#endif

using System;
using System.Collections;
using UnityEngine;
#if USE_NEW_GUI
using UnityEngine.UI;
# endif

//-------------------------------------------------------------------------------------
// ***** OVRMainMenu
//
/// <summary>
/// OVRMainMenu is used to control the loading of different scenes. It also renders out 
/// a menu that allows a user to modify various Rift settings, and allow for storing 
/// these settings for recall later.
/// 
/// A user of this component can add as many scenes that they would like to be able to 
/// have access to.
///
/// OVRMainMenu is currently attached to the OVRPlayerController prefab for convenience, 
/// but can safely removed from it and added to another GameObject that is used for general 
/// Unity logic.
///
/// </summary>
public class OVRMainMenu : MonoBehaviour
{
	/// <summary>
	/// The amount of time in seconds that it takes for the menu to fade in.
	/// </summary>
	public float 	FadeInTime    	= 2.0f;

	/// <summary>
	/// An optional texture that appears before the menu fades in.
	/// </summary>
	public UnityEngine.Texture 	FadeInTexture 	= null;

	/// <summary>
	/// An optional font that replaces Unity's default Arial.
	/// </summary>
	public Font 	FontReplace		= null;

	/// <summary>
	/// The key that toggles the menu.
	/// </summary>
	public KeyCode	MenuKey			= KeyCode.Space;

	/// <summary>
	/// The key that quits the application.
	/// </summary>
	public KeyCode	QuitKey			= KeyCode.Escape;
	
	/// <summary>
	/// Scene names to show on-screen for each of the scenes in Scenes.
	/// </summary>
	public string [] SceneNames;

	/// <summary>
	/// The set of scenes that the user can jump to.
	/// </summary>
	public string [] Scenes;
	
	private bool ScenesVisible   	= false;
	
	// Spacing for scenes menu
	private int    	StartX			= 490;
	private int    	StartY			= 250;
	private int    	WidthX			= 300;
	private int    	WidthY			= 23;
	
	// Spacing for variables that users can change
	private int    	VRVarsSX		= 553;
	private int		VRVarsSY		= 250;
	private int    	VRVarsWidthX 	= 175;
	private int    	VRVarsWidthY 	= 23;

    private int    	StepY			= 25;

	// Handle to OVRCameraRig
	private OVRCameraRig CameraController = null;
	
	// Handle to OVRPlayerController
	private OVRPlayerController PlayerController = null;
	
	// Controller buttons
	private bool  PrevStartDown;
	private bool  PrevHatDown;
	private bool  PrevHatUp;
	
	private bool  ShowVRVars;
	
	private bool  OldSpaceHit;
	
	// FPS 
	private float  UpdateInterval 	= 0.5f;
	private float  Accum   			= 0; 	
	private int    Frames  			= 0; 	
	private float  TimeLeft			= 0; 				
	private string strFPS			= "FPS: 0";
	
	private string strIPD 			= "IPD: 0.000";	
	
	/// <summary>
	/// Prediction (in ms)
	/// </summary>
	public float   PredictionIncrement = 0.001f; // 1 ms
	private string strPrediction       = "Pred: OFF";	
	
	private string strFOV     		= "FOV: 0.0f";
	private string strHeight     	 = "Height: 0.0f";
	
	/// <summary>
	/// Controls how quickly the player's speed and rotation change based on input.
	/// </summary>
	public float   SpeedRotationIncrement   	= 0.05f;
	private string strSpeedRotationMultipler    = "Spd. X: 0.0f Rot. X: 0.0f";
	
	private bool   LoadingLevel 	= false;		
	private float  AlphaFadeValue	= 1.0f;
	private int    CurrentLevel		= 0;
	
	// Rift detection
	private bool   HMDPresent           = false;
	private float  RiftPresentTimeout   = 0.0f;
	private string strRiftPresent		= "";

	// Replace the GUI with our own texture and 3D plane that
	// is attached to the rendder camera for true 3D placement
	private OVRGUI  		GuiHelper 		 = new OVRGUI();
	private GameObject      GUIRenderObject  = null;
	private RenderTexture	GUIRenderTexture = null;

    // We want to use new Unity GUI built in 4.6 for OVRMainMenu GUI
    // Enable the UsingNewGUI option in the editor, 
    // if you want to use new GUI and Unity version is higher than 4.6    
#if USE_NEW_GUI
    private GameObject NewGUIObject                 = null;
	private GameObject RiftPresentGUIObject         = null;
#endif
    
	/// <summary>
	/// We can set the layer to be anything we want to, this allows
	/// a specific camera to render it.
	/// </summary>
	public string 			LayerName 		 = "Default";

	/// <summary>
	/// Crosshair rendered onto 3D plane.
	/// </summary>
	public UnityEngine.Texture  CrosshairImage 			= null;
	private OVRCrosshair Crosshair        	= new OVRCrosshair();

    // Resolution Eye Texture
    private string strResolutionEyeTexture = "Resolution: 0 x 0";

    // Latency values
    private string strLatencies = "Ren: 0.0f TWrp: 0.0f PostPresent: 0.0f";

	// Vision mode on/off
	private bool VisionMode = true;
#if	SHOW_DK2_VARIABLES
	private string strVisionMode = "Vision Enabled: ON";
#endif

	// We want to hold onto GridCube, for potential sharing
	// of the menu RenderTarget
	OVRGridCube GridCube = null;

	// We want to hold onto the VisionGuide so we can share
	// the menu RenderTarget
	OVRVisionGuide VisionGuide = null;

	#region MonoBehaviour Message Handlers
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{    
        // Find camera controller
		OVRCameraRig[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraRig>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRMainMenu: No OVRCameraRig attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRMainMenu: More then 1 OVRCameraRig attached.");
		else{
			CameraController = CameraControllers[0];
#if USE_NEW_GUI
			OVRUGUI.CameraController = CameraController;
#endif
		}
	
		// Find player controller
		OVRPlayerController[] PlayerControllers;
		PlayerControllers = gameObject.GetComponentsInChildren<OVRPlayerController>();
		
		if(PlayerControllers.Length == 0)
			Debug.LogWarning("OVRMainMenu: No OVRPlayerController attached.");
		else if (PlayerControllers.Length > 1)
			Debug.LogWarning("OVRMainMenu: More then 1 OVRPlayerController attached.");
		else{
            PlayerController = PlayerControllers[0];
#if USE_NEW_GUI
            OVRUGUI.PlayerController = PlayerController;
#endif
        }

#if USE_NEW_GUI
	        // Create canvas for using new GUI
	        NewGUIObject = new GameObject();
		    NewGUIObject.name = "OVRGUIMain";
            NewGUIObject.transform.parent = GameObject.Find("LeftEyeAnchor").transform;
	        RectTransform r = NewGUIObject.AddComponent<RectTransform>();
			r.sizeDelta = new Vector2(100f, 100f);
	        r.localScale = new Vector3(0.001f, 0.001f, 0.001f);
	        r.localPosition = new Vector3(0.01f, 0.17f, 0.53f);
	        r.localEulerAngles = Vector3.zero;

			Canvas c = NewGUIObject.AddComponent<Canvas>();
#if (UNITY_5_0)
			// TODO: Unity 5.0b11 has an older version of the new GUI being developed in Unity 4.6.
			// Remove this once Unity 5 has a more recent merge of Unity 4.6.
	        c.renderMode = RenderMode.World;
#else
	        c.renderMode = RenderMode.WorldSpace;
#endif
	        c.pixelPerfect = false;
#endif
    }
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{		
		AlphaFadeValue = 1.0f;
		CurrentLevel   = 0;
		PrevStartDown  = false;
		PrevHatDown    = false;
		PrevHatUp      = false;
		ShowVRVars	   = false;
		OldSpaceHit    = false;
		strFPS         = "FPS: 0";
		LoadingLevel   = false;	
		ScenesVisible  = false;
      
        // Set the GUI target
		GUIRenderObject = GameObject.Instantiate(Resources.Load("OVRGUIObjectMain")) as GameObject;

		if(GUIRenderObject != null)
		{
			// Chnge the layer
			GUIRenderObject.layer = LayerMask.NameToLayer(LayerName);

			if(GUIRenderTexture == null)
			{
				int w = Screen.width;
				int h = Screen.height;

				// We don't need a depth buffer on this texture
				GUIRenderTexture = new RenderTexture(w, h, 0);	
				GuiHelper.SetPixelResolution(w, h);
				// NOTE: All GUI elements are being written with pixel values based
				// from DK1 (1280x800). These should change to normalized locations so 
				// that we can scale more cleanly with varying resolutions
				GuiHelper.SetDisplayResolution(1280.0f, 800.0f);
			}
		}
		
		// Attach GUI texture to GUI object and GUI object to Camera
		if(GUIRenderTexture != null && GUIRenderObject != null)
		{
			GUIRenderObject.GetComponent<Renderer>().material.mainTexture = GUIRenderTexture;
			
			if(CameraController != null)
            {
                // Grab transform of GUI object
                Vector3 ls = GUIRenderObject.transform.localScale;
                Vector3 lp = GUIRenderObject.transform.localPosition;
                Quaternion lr = GUIRenderObject.transform.localRotation;

                // Attach the GUI object to the camera
				GUIRenderObject.transform.parent = CameraController.centerEyeAnchor;
                // Reset the transform values (we will be maintaining state of the GUI object
                // in local state)

                GUIRenderObject.transform.localScale = ls;
                GUIRenderObject.transform.localPosition = lp;
                GUIRenderObject.transform.localRotation = lr;

                // Deactivate object until we have completed the fade-in
                // Also, we may want to deactive the render object if there is nothing being rendered
                // into the UI
                GUIRenderObject.SetActive(false);
            }
		}
		
		// Make sure to hide cursor 
		if(Application.isEditor == false)
		{
#if UNITY_5_0
			Cursor.visible = false; 
			Cursor.lockState = CursorLockMode.Locked;
#else
			Screen.showCursor = false; 
			Screen.lockCursor = true;
#endif
		}
		
		// CameraController updates
		if(CameraController != null)
		{
			// Add a GridCube component to this object
			GridCube = gameObject.AddComponent<OVRGridCube>();
			GridCube.SetOVRCameraController(ref CameraController);

			// Add a VisionGuide component to this object
			VisionGuide = gameObject.AddComponent<OVRVisionGuide>();
			VisionGuide.SetOVRCameraController(ref CameraController);
			VisionGuide.SetFadeTexture(ref FadeInTexture);
			VisionGuide.SetVisionGuideLayer(ref LayerName);
		}
		
		// Crosshair functionality
		Crosshair.Init();
		Crosshair.SetCrosshairTexture(ref CrosshairImage);
		Crosshair.SetOVRCameraController (ref CameraController);
		Crosshair.SetOVRPlayerController(ref PlayerController);
		
		// Check for HMD and sensor
        CheckIfRiftPresent();

#if USE_NEW_GUI
        if (!string.IsNullOrEmpty(strRiftPresent)){
            ShowRiftPresentGUI();
        }
#endif
	} 
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{		
		if(LoadingLevel == true)
			return;

		// Main update
		UpdateFPS();
		
		// CameraController updates
		if(CameraController != null)
		{
			UpdateIPD();
			
			UpdateRecenterPose();
			UpdateVisionMode();
			UpdateFOV();
			UpdateEyeHeightOffset();
			UpdateResolutionEyeTexture();
			UpdateLatencyValues();
		}
		
		// PlayerController updates
		if(PlayerController != null)
		{
			UpdateSpeedAndRotationScaleMultiplier();
			UpdatePlayerControllerMovement();
		}
		
		// MainMenu updates
		UpdateSelectCurrentLevel();
		
		// Device updates
		UpdateDeviceDetection();
		
		// Crosshair functionality
		Crosshair.UpdateCrosshair();

#if USE_NEW_GUI
        if (ShowVRVars && RiftPresentTimeout <= 0.0f)
        {
            NewGUIObject.SetActive(true);
            UpdateNewGUIVars();
            OVRUGUI.UpdateGUI();
        }
        else
        {
            NewGUIObject.SetActive(false);
        }
#endif
     
        // Toggle Fullscreen
		if(Input.GetKeyDown(KeyCode.F11))
			Screen.fullScreen = !Screen.fullScreen;

        if (Input.GetKeyDown(KeyCode.M))
			OVRManager.display.mirrorMode = !OVRManager.display.mirrorMode;
        
		// Escape Application
		if (Input.GetKeyDown(QuitKey))
			Application.Quit();
	}

    /// <summary>
    /// Updates Variables for new GUI.
    /// </summary>
#if USE_NEW_GUI
    void UpdateNewGUIVars()
    {
#if	SHOW_DK2_VARIABLES		
        // Print out Vision Mode
        OVRUGUI.strVisionMode = strVisionMode;		
#endif
        // Print out FPS
        OVRUGUI.strFPS = strFPS;

        // Don't draw these vars if CameraController is not present
        if (CameraController != null)
        {
            OVRUGUI.strPrediction = strPrediction;
            OVRUGUI.strIPD = strIPD;
            OVRUGUI.strFOV = strFOV;
            OVRUGUI.strResolutionEyeTexture = strResolutionEyeTexture;
            OVRUGUI.strLatencies = strLatencies;
        }

        // Don't draw these vars if PlayerController is not present
        if (PlayerController != null)
        {
            OVRUGUI.strHeight = strHeight;
            OVRUGUI.strSpeedRotationMultipler = strSpeedRotationMultipler;
        }

        OVRUGUI.strRiftPresent = strRiftPresent;
    }
#endif

	void OnGUI()
	{	
		// Important to keep from skipping render events
		if (Event.current.type != EventType.Repaint)
			return;

#if !USE_NEW_GUI		
		// Fade in screen
		if(AlphaFadeValue > 0.0f)
		{
			AlphaFadeValue -= Mathf.Clamp01(Time.deltaTime / FadeInTime);
			if(AlphaFadeValue < 0.0f)
			{
				AlphaFadeValue = 0.0f;	
			}
			else
			{
				GUI.color = new Color(0, 0, 0, AlphaFadeValue);
				GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), FadeInTexture ); 
				return;
			}
        }
#endif
        // We can turn on the render object so we can render the on-screen menu
		if(GUIRenderObject != null)
		{
			if (ScenesVisible || 
			    ShowVRVars || 
			    Crosshair.IsCrosshairVisible() || 
			    RiftPresentTimeout > 0.0f || 
			    VisionGuide.GetFadeAlphaValue() > 0.0f)
			{
				GUIRenderObject.SetActive(true);
			}
			else
			{
				GUIRenderObject.SetActive(false);
			}
		}
		
		//***
		// Set the GUI matrix to deal with portrait mode
		Vector3 scale = Vector3.one;
		Matrix4x4 svMat = GUI.matrix; // save current matrix
		// substitute matrix - only scale is altered from standard
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		
		// Cache current active render texture
		RenderTexture previousActive = RenderTexture.active;
		
		// if set, we will render to this texture
		if(GUIRenderTexture != null && GUIRenderObject.activeSelf)
		{
			RenderTexture.active = GUIRenderTexture;
			GL.Clear (false, true, new Color (0.0f, 0.0f, 0.0f, 0.0f));
		}
		
		// Update OVRGUI functions (will be deprecated eventually when 2D renderingc
		// is removed from GUI)
		GuiHelper.SetFontReplace(FontReplace);

		// If true, we are displaying information about the Rift not being detected
		// So do not show anything else
		if(GUIShowRiftDetected() != true)
		{	
			GUIShowLevels();
			GUIShowVRVariables();            
		}
		
		// The cross-hair may need to go away at some point, unless someone finds it 
		// useful
		Crosshair.OnGUICrosshair();
		
		// Since we want to draw into the main GUI that is shared within the MainMenu,
		// we call the OVRVisionGuide GUI function here
		VisionGuide.OnGUIVisionGuide();
		
		// Restore active render texture
		if (GUIRenderObject.activeSelf)
		{
			RenderTexture.active = previousActive;
		}
		
		// ***
		// Restore previous GUI matrix
		GUI.matrix = svMat;
	}
	#endregion

	#region Internal State Management Functions
	/// <summary>
	/// Updates the FPS.
	/// </summary>
	void UpdateFPS()
	{
		TimeLeft -= Time.deltaTime;
		Accum += Time.timeScale/Time.deltaTime;
		++Frames;
 
    	// Interval ended - update GUI text and start new interval
    	if( TimeLeft <= 0.0 )
    	{
        	// display two fractional digits (f2 format)
			float fps = Accum / Frames;
			
			if(ShowVRVars == true)// limit gc
				strFPS = System.String.Format("FPS: {0:F2}",fps);

       		TimeLeft += UpdateInterval;
        	Accum  = 0.0f;
        	Frames = 0;
    	}
	}
	
	/// <summary>
	/// Updates the IPD.
	/// </summary>
	void UpdateIPD()
	{
		if(ShowVRVars == true) // limit gc
		{	
			strIPD = System.String.Format("IPD (mm): {0:F4}", OVRManager.profile.ipd * 1000.0f);
		}
	}
	
	void UpdateRecenterPose()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			OVRManager.display.RecenterPose();
		}
	}
	
	/// <summary>
	/// Updates the vision mode.
	/// </summary>
	void UpdateVisionMode()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			VisionMode = !VisionMode;
			OVRManager.tracker.isEnabled = VisionMode;

#if SHOW_DK2_VARIABLES
			strVisionMode = VisionMode ? "Vision Enabled: ON" : "Vision Enabled: OFF";
#endif
		}
	}
	
	/// <summary>
	/// Updates the FOV.
	/// </summary>
	void UpdateFOV()
	{
		if(ShowVRVars == true)// limit gc
		{
			OVRDisplay.EyeRenderDesc eyeDesc = OVRManager.display.GetEyeRenderDesc(OVREye.Left);

			strFOV = System.String.Format ("FOV (deg): {0:F3}", eyeDesc.fov.y);
		}
	}

    /// <summary>
    /// Updates resolution of eye texture
    /// </summary>
    void UpdateResolutionEyeTexture()
    {
        if (ShowVRVars == true) // limit gc
        {
			OVRDisplay.EyeRenderDesc leftEyeDesc = OVRManager.display.GetEyeRenderDesc(OVREye.Left);
			OVRDisplay.EyeRenderDesc rightEyeDesc = OVRManager.display.GetEyeRenderDesc(OVREye.Right);

			float scale = OVRManager.instance.nativeTextureScale * OVRManager.instance.virtualTextureScale;
			float w = (int)(scale * (float)(leftEyeDesc.resolution.x + rightEyeDesc.resolution.x));
			float h = (int)(scale * (float)Mathf.Max(leftEyeDesc.resolution.y, rightEyeDesc.resolution.y));

            strResolutionEyeTexture = System.String.Format("Resolution : {0} x {1}", w, h);
        }
    }

    /// <summary>
    /// Updates latency values
    /// </summary>
    void UpdateLatencyValues()
    {
#if !UNITY_ANDROID || UNITY_EDITOR
        if (ShowVRVars == true) // limit gc
        {
			OVRDisplay.LatencyData latency = OVRManager.display.latency;
            if (latency.render < 0.000001f && latency.timeWarp < 0.000001f && latency.postPresent < 0.000001f)
                strLatencies = System.String.Format("Ren : N/A TWrp: N/A PostPresent: N/A");
            else
                strLatencies = System.String.Format("Ren : {0:F3} TWrp: {1:F3} PostPresent: {2:F3}",
					latency.render,
					latency.timeWarp,
					latency.postPresent);
        }
#endif
    }
		
	/// <summary>
	/// Updates the eye height offset.
	/// </summary>
	void UpdateEyeHeightOffset()
	{
		if(ShowVRVars == true)// limit gc
		{
			float eyeHeight = OVRManager.profile.eyeHeight;
			
			strHeight = System.String.Format ("Eye Height (m): {0:F3}", eyeHeight);
		}
	}
	
	/// <summary>
	/// Updates the speed and rotation scale multiplier.
	/// </summary>
	void UpdateSpeedAndRotationScaleMultiplier()
	{
		float moveScaleMultiplier = 0.0f;
		PlayerController.GetMoveScaleMultiplier(ref moveScaleMultiplier);
		if(Input.GetKeyDown(KeyCode.Alpha7))
			moveScaleMultiplier -= SpeedRotationIncrement;
		else if (Input.GetKeyDown(KeyCode.Alpha8))
			moveScaleMultiplier += SpeedRotationIncrement;		
		PlayerController.SetMoveScaleMultiplier(moveScaleMultiplier);
		
		float rotationScaleMultiplier = 0.0f;
		PlayerController.GetRotationScaleMultiplier(ref rotationScaleMultiplier);
		if(Input.GetKeyDown(KeyCode.Alpha9))
			rotationScaleMultiplier -= SpeedRotationIncrement;
		else if (Input.GetKeyDown(KeyCode.Alpha0))
			rotationScaleMultiplier += SpeedRotationIncrement;	
		PlayerController.SetRotationScaleMultiplier(rotationScaleMultiplier);
		
		if(ShowVRVars == true)// limit gc
			strSpeedRotationMultipler = System.String.Format ("Spd.X: {0:F2} Rot.X: {1:F2}", 
									moveScaleMultiplier, 
									rotationScaleMultiplier);
	}
	
	/// <summary>
	/// Updates the player controller movement.
	/// </summary>
	void UpdatePlayerControllerMovement()
	{
		if(PlayerController != null)
			PlayerController.SetHaltUpdateMovement(ScenesVisible);
	}
	
	/// <summary>
	/// Updates the select current level.
	/// </summary>
	void UpdateSelectCurrentLevel()
	{
		ShowLevels();
				
		if (!ScenesVisible)
			return;
			
		CurrentLevel = GetCurrentLevel();
		
		if (Scenes.Length != 0
			&& (OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.A)
				|| Input.GetKeyDown(KeyCode.Return)))
		{
			LoadingLevel = true;
			Application.LoadLevelAsync(Scenes[CurrentLevel]);
		}
	}
	
	/// <summary>
	/// Shows the levels.
	/// </summary>
	/// <returns><c>true</c>, if levels was shown, <c>false</c> otherwise.</returns>
	bool ShowLevels()
	{
		if (Scenes.Length == 0)
		{
			ScenesVisible = false;
			return ScenesVisible;
		}
		
		bool curStartDown = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Start);
		bool startPressed = (curStartDown && !PrevStartDown) || Input.GetKeyDown(KeyCode.RightShift);
		PrevStartDown = curStartDown;
		
		if (startPressed)
		{
			ScenesVisible = !ScenesVisible;
		}
		
		return ScenesVisible;
	}
	
	/// <summary>
	/// Gets the current level.
	/// </summary>
	/// <returns>The current level.</returns>
	int GetCurrentLevel()
	{
		bool curHatDown = false;
		if(OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Down) == true)
			curHatDown = true;
		
		bool curHatUp = false;
		if(OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Down) == true)
			curHatUp = true;
		
		if((PrevHatDown == false) && (curHatDown == true) ||
			Input.GetKeyDown(KeyCode.DownArrow))
		{
			CurrentLevel = (CurrentLevel + 1) % SceneNames.Length;	
		}
		else if((PrevHatUp == false) && (curHatUp == true) ||
			Input.GetKeyDown(KeyCode.UpArrow))
		{
			CurrentLevel--;	
			if(CurrentLevel < 0)
				CurrentLevel = SceneNames.Length - 1;
		}
					
		PrevHatDown = curHatDown;
		PrevHatUp = curHatUp;
		
		return CurrentLevel;
	}

	#endregion

	#region Internal GUI Functions

	/// <summary>
	/// Show the GUI levels.
	/// </summary>
	void GUIShowLevels()
	{
		if(ScenesVisible == true)
		{   
			// Darken the background by rendering fade texture 
			GUI.color = new Color(0, 0, 0, 0.5f);
  			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), FadeInTexture );
 			GUI.color = Color.white;
		
			if(LoadingLevel == true)
			{
				string loading = "LOADING...";
				GuiHelper.StereoBox (StartX, StartY, WidthX, WidthY, ref loading, Color.yellow);
				return;
			}
			
			for (int i = 0; i < SceneNames.Length; i++)
			{
				Color c;
				if(i == CurrentLevel)
					c = Color.yellow;
				else
					c = Color.black;
				
				int y   = StartY + (i * StepY);
				
				GuiHelper.StereoBox (StartX, y, WidthX, WidthY, ref SceneNames[i], c);
			}
		}				
	}
	
	/// <summary>
	/// Show the VR variables.   
	/// </summary>
    void GUIShowVRVariables()
    {
        bool SpaceHit = Input.GetKey(MenuKey);
        if ((OldSpaceHit == false) && (SpaceHit == true))
        {
            if (ShowVRVars == true)
            {
                ShowVRVars = false;
            }
            else
            {
                ShowVRVars = true;
#if USE_NEW_GUI
                OVRUGUI.InitUIComponent = ShowVRVars;
#endif
            }
        }

        OldSpaceHit = SpaceHit;

        // Do not render if we are not showing
        if (ShowVRVars == false)
            return;

        

#if !USE_NEW_GUI
        int y = VRVarsSY;
#if	SHOW_DK2_VARIABLES
		// Print out Vision Mode
		GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
							 ref strVisionMode, Color.green);
#endif

        // Draw FPS
        GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strFPS, Color.green);

        // Don't draw these vars if CameraController is not present
        if (CameraController != null)
        {
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strPrediction, Color.white);
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strIPD, Color.yellow);
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strFOV, Color.white);
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strResolutionEyeTexture, Color.white);
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                             ref strLatencies, Color.white);
        }

        // Don't draw these vars if PlayerController is not present
        if (PlayerController != null)
        {
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                                 ref strHeight, Color.yellow);
            GuiHelper.StereoBox(VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY,
                                 ref strSpeedRotationMultipler, Color.white);
        }
#endif
    }
	
	// RIFT DETECTION
	
	/// <summary>
	/// Checks to see if HMD and / or sensor is available, and displays a 
	/// message if it is not.
	/// </summary>
	void CheckIfRiftPresent()
	{
		HMDPresent = OVRManager.display.isPresent;
		
		if (!HMDPresent)
		{
			RiftPresentTimeout = 15.0f;
			
			if (!HMDPresent)
				strRiftPresent = "NO HMD DETECTED";
#if USE_NEW_GUI
            OVRUGUI.strRiftPresent = strRiftPresent;
#endif
        }
	}

	/// <summary>
	/// Show if Rift is detected.
	/// </summary>
	/// <returns><c>true</c>, if show rift detected was GUIed, <c>false</c> otherwise.</returns>
	bool GUIShowRiftDetected()
	{
#if !USE_NEW_GUI
		if(RiftPresentTimeout > 0.0f)
		{
			GuiHelper.StereoBox (StartX, StartY, WidthX, WidthY, 
								 ref strRiftPresent, Color.white);
		
			return true;
        }
#else
         if(RiftPresentTimeout < 0.0f)
            DestroyImmediate(RiftPresentGUIObject);
#endif
        return false;
	}
	
	/// <summary>
	/// Updates the device detection.
	/// </summary>
	void UpdateDeviceDetection()
	{
		if(RiftPresentTimeout > 0.0f)
			RiftPresentTimeout -= Time.deltaTime;
	}

    /// <summary>
    /// Show rift present GUI with new GUI
    /// </summary>
    void ShowRiftPresentGUI()
    {
#if USE_NEW_GUI
        RiftPresentGUIObject = new GameObject();
        RiftPresentGUIObject.name = "RiftPresentGUIMain";
        RiftPresentGUIObject.transform.parent = GameObject.Find("LeftEyeAnchor").transform;

        RectTransform r = RiftPresentGUIObject.AddComponent<RectTransform>();
        r.sizeDelta = new Vector2(100f, 100f);
        r.localPosition = new Vector3(0.01f, 0.17f, 0.53f);
        r.localEulerAngles = Vector3.zero;
        r.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        Canvas c = RiftPresentGUIObject.AddComponent<Canvas>();
#if UNITY_5_0
		// TODO: Unity 5.0b11 has an older version of the new GUI being developed in Unity 4.6.
	   	// Remove this once Unity 5 has a more recent merge of Unity 4.6.
		c.renderMode = RenderMode.World;
#else
		c.renderMode = RenderMode.WorldSpace;
#endif
        c.pixelPerfect = false;
        OVRUGUI.RiftPresentGUI(RiftPresentGUIObject);
#endif
    }
	#endregion
}
