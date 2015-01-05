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

// Use the Unity new GUI with Unity 4.6 or above.
#if UNITY_4_6 || UNITY_5_0
#define USE_NEW_GUI
#endif

using UnityEngine;
#if USE_NEW_GUI
using UnityEngine.UI;
# endif
using System.Collections;

//-------------------------------------------------------------------------------------
/// <summary>
/// Class for Unity new GUI built in 4.6
/// </summary>
public class OVRUGUI
{
#if USE_NEW_GUI

    #region UIGameObject
    private static GameObject NewGUIManager;
    private static GameObject RiftPresent;
    private static GameObject LowPersistence;
    private static GameObject VisionMode;
    private static GameObject FPS;
    private static GameObject Prediction;
    private static GameObject IPD;
    private static GameObject FOV;
    private static GameObject Height;
    private static GameObject SpeedRotationMutipler;
    private static GameObject DeviceDetection;
    private static GameObject ResolutionEyeTexture;
    private static GameObject Latencies;
    #endregion

    #region VRVariables
    [HideInInspector]
    public static string strRiftPresent = null;
    [HideInInspector]
    public static string strLPM = null; //"LowPersistenceMode: ON";
    [HideInInspector]
    public static string strVisionMode = null;//"Vision Enabled: ON";
    [HideInInspector]
    public static string strFPS = null;//"FPS: 0";
    [HideInInspector]
    public static string strIPD = null;//"IPD: 0.000";
    [HideInInspector]
    public static string strPrediction = null;//"Pred: OFF";
    [HideInInspector]
    public static string strFOV = null;//"FOV: 0.0f";
    [HideInInspector]
    public static string strHeight = null;//"Height: 0.0f";
    [HideInInspector]
    public static string strSpeedRotationMultipler = null;//"Spd. X: 0.0f Rot. X: 0.0f";
    [HideInInspector]
    public static OVRPlayerController PlayerController = null;
    [HideInInspector]
    public static OVRCameraRig CameraController = null;
    //[HideInInspector]
    //public static string strDeviceDetection = null;// Device attach / detach
    [HideInInspector]
    public static string strResolutionEyeTexture = null;// "Resolution : {0} x {1}"
    [HideInInspector]
    public static string strLatencies = null;// Device attach / detach
    #endregion

    [HideInInspector]
    public static bool InitUIComponent = false;
    private static float offsetY = 55.0f;
    private static bool isInited = false;
    private static int numOfGUI = 0;
    private static GameObject text;

    /// <summary>
    /// It's for rift present GUI
    /// </summary>
    public static void RiftPresentGUI(GameObject GUIMainOBj)
    {
        RiftPresent = ComponentComposition(RiftPresent);
        RiftPresent.transform.parent = GUIMainOBj.transform;
        RiftPresent.name = "RiftPresent";
		RectTransform r = RiftPresent.GetComponent<RectTransform>();
        r.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        r.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        r.localEulerAngles = Vector3.zero;

		Text t = RiftPresent.GetComponentInChildren<Text>();
        t.text = strRiftPresent;
        t.fontSize = 20;        
    }  

    /// <summary>
    /// It's for rift present GUI
    /// </summary>
    public static void UpdateGUI()
    {
        if (InitUIComponent && !isInited)
        {
            InitUIComponents();
        }

        UpdateVariable();
    }


    /// <summary>
    /// Update VR Variables
    /// </summary>
    static void UpdateVariable()
    {
        NewGUIManager.transform.localPosition = new Vector3(0.0f, 100.0f, 0.0f);

        if (!string.IsNullOrEmpty(strLPM))
            LowPersistence.GetComponentInChildren<Text>().text = strLPM;
        if (!string.IsNullOrEmpty(strVisionMode))
            VisionMode.GetComponentInChildren<Text>().text = strVisionMode;
        if (!string.IsNullOrEmpty(strFPS))
             FPS.GetComponentInChildren<Text>().text = strFPS;
        if (!string.IsNullOrEmpty(strPrediction))
            Prediction.GetComponentInChildren<Text>().text = strPrediction;
        if (!string.IsNullOrEmpty(strIPD))
            IPD.GetComponentInChildren<Text>().text = strIPD;
        if (!string.IsNullOrEmpty(strFOV))
            FOV.GetComponentInChildren<Text>().text = strFOV;
        if (!string.IsNullOrEmpty(strResolutionEyeTexture))
            ResolutionEyeTexture.GetComponentInChildren<Text>().text = strResolutionEyeTexture;
        if (!string.IsNullOrEmpty(strLatencies))
            Latencies.GetComponentInChildren<Text>().text = strLatencies;

        if (PlayerController != null)
        {
            if (!string.IsNullOrEmpty(strHeight))
                Height.GetComponentInChildren<Text>().text = strHeight;
            if (!string.IsNullOrEmpty(strSpeedRotationMultipler))
                SpeedRotationMutipler.GetComponentInChildren<Text>().text = strSpeedRotationMultipler;
        }
    }

    /// <summary>
    /// Initialize UI GameObjects
    /// </summary>
    static void InitUIComponents()
    {
        float posY = 0.0f;
        int fontSize = 20;

        NewGUIManager = new GameObject();
        NewGUIManager.name = "GUIManager";
        NewGUIManager.transform.parent = GameObject.Find("OVRGUIMain").transform;
        NewGUIManager.transform.localPosition = Vector3.zero;
        NewGUIManager.transform.localEulerAngles = Vector3.zero;
        NewGUIManager.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        // Print out for Low Persistence Mode
        if (!string.IsNullOrEmpty(strLPM))
        {           
            LowPersistence = UIObjectManager(LowPersistence, "LowPersistence", posY -= offsetY, strLPM, fontSize);
        }

        // Print out for VisionMode
        if (!string.IsNullOrEmpty(strVisionMode))
        {
            VisionMode = UIObjectManager(VisionMode, "VisionMode", posY -= offsetY, strVisionMode, fontSize);
        }

        // Print out for FPS
        if (!string.IsNullOrEmpty(strFPS))
        {
            FPS = UIObjectManager(FPS, "FPS", posY -= offsetY, strFPS, fontSize);
        }

        // Print out for Prediction
        if (!string.IsNullOrEmpty(strPrediction))
        {
            Prediction = UIObjectManager(Prediction, "Prediction", posY -= offsetY, strPrediction, fontSize);
        }

        // Print out for IPD
        if (!string.IsNullOrEmpty(strIPD))
        {
            IPD = UIObjectManager(IPD, "IPD", posY -= offsetY, strIPD, fontSize);
        }

        // Print out for FOV
        if (!string.IsNullOrEmpty(strFOV))
        {
            FOV = UIObjectManager(FOV, "FOV", posY -= offsetY, strFOV, fontSize);
        }

        if (PlayerController != null)
        {
            // Print out for Height
            if (!string.IsNullOrEmpty(strHeight))
            {
                Height = UIObjectManager(Height, "Height", posY -= offsetY, strHeight, fontSize);
            }

            // Print out for Speed Rotation Multiplier
            if (!string.IsNullOrEmpty(strSpeedRotationMultipler))
            {
                SpeedRotationMutipler = UIObjectManager(SpeedRotationMutipler, "SpeedRotationMutipler", posY -= offsetY, strSpeedRotationMultipler, fontSize);
            }
        }

        // Print out for Resoulution of Eye Texture
        if (!string.IsNullOrEmpty(strResolutionEyeTexture))
        {
            ResolutionEyeTexture = UIObjectManager(ResolutionEyeTexture, "Resolution", posY -= offsetY, strResolutionEyeTexture, fontSize);
        }

        // Print out for Latency
        if (!string.IsNullOrEmpty(strLatencies))
        {
            Latencies = UIObjectManager(Latencies, "Latency", posY -= offsetY, strLatencies, 17);
            posY = 0.0f;
        }

        InitUIComponent = false;
        isInited = true;

    }

    static GameObject UIObjectManager(GameObject gameObject, string name, float posY, string text, int fontSize)
    {
        gameObject = ComponentComposition(gameObject);
        gameObject.name = name;
        gameObject.transform.parent = NewGUIManager.transform;

		RectTransform r = gameObject.GetComponent<RectTransform>();
        r.localPosition = new Vector3(0.0f, posY -= offsetY, 0.0f);

		Text t = gameObject.GetComponentInChildren<Text>();
        t.text = text;
        t.fontSize = fontSize;
        gameObject.transform.localEulerAngles = Vector3.zero;

        r.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        return gameObject;

    }
    
    /// <summary>
    /// Component composition
    /// </summary>
    /// <returns> Composed game object. </returns>
    static GameObject ComponentComposition(GameObject GO)
    {
        GO = new GameObject();
        GO.AddComponent<RectTransform>();
        GO.AddComponent<CanvasRenderer>();
        GO.AddComponent<Image>();
        GO.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
        GO.GetComponent<Image>().color = new Color(7f / 255f, 45f / 255f, 71f / 255f, 200f / 255f);

        text = new GameObject();
        text.AddComponent<RectTransform>();
        text.AddComponent<CanvasRenderer>();
        text.AddComponent<Text>();
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
        text.GetComponent<Text>().font = (Font)Resources.Load("DINPro-Bold");
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        text.transform.parent = GO.transform;
        text.name = "TextBox";

        return GO;
    }

#endif
}
