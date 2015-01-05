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
using System.Collections;
using UnityEditor;

//-------------------------------------------------------------------------------------
// ***** OVRPrefabs
//
// OculusPrefabs adds menu items under the Oculus main menu. It allows for quick creation
// of the main Oculus prefabs without having to open the Prefab folder and dragging/dropping
// into the scene.
class OVRPrefabs
{
	static void CreateOVRCameraController ()
	{
		Object ovrcam = AssetDatabase.LoadAssetAtPath ("Assets/OVR/Prefabs/OVRCameraRig.prefab", typeof(UnityEngine.Object));
		PrefabUtility.InstantiatePrefab(ovrcam);
    }	
	
	static void CreateOVRPlayerController ()
	{
		Object ovrcam = AssetDatabase.LoadAssetAtPath ("Assets/OVR/Prefabs/OVRPlayerController.prefab", typeof(UnityEngine.Object));
		PrefabUtility.InstantiatePrefab(ovrcam);
    }	
}
