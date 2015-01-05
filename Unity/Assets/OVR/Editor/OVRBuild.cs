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

using UnityEngine;
using UnityEditor;

//-------------------------------------------------------------------------------------
// ***** OculusBuildDemo
//
// OculusBuild allows for command line building of the Oculus main demo (Tuscany).
//
class OculusBuildDemo
{
	static void PerformBuildStandaloneWindows ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "./Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneMac ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "./Mac_OculusUnityDemoScene.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneLinux ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "./Linux_OculusUnityDemoScene", BuildTarget.StandaloneLinux, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneLinux64 ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "./Linux_OculusUnityDemoScene", BuildTarget.StandaloneLinux64, BuildOptions.None);
    }
}

//-------------------------------------------------------------------------------------
// ***** OculusBuild
//
// OculusBuild adds menu functionality for a user to build the currently selected scene, 
// and to also build and run the standalone build. These menu items can be found under the
// Oculus/Build menu from the main Unity Editor menu bar.
//
class OculusBuild
{
	// Build the Android APK and place into main project folder
	static void PerformBuildAndroidAPK()
	{
		if (Application.isEditor)
		{
			string[] scenes = { EditorApplication.currentScene };
			BuildPipeline.BuildPlayer(scenes, "OculusUnityDemoScene.apk", BuildTarget.Android, BuildOptions.None);
		}
	}
}

//-------------------------------------------------------------------------------------
// ***** OculusBuildApp
//
// OculusBuildApp allows us to build other Oculus apps from the command line. 
//
partial class OculusBuildApp
{
    static void SetAndroidTarget()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        }
    }
}
