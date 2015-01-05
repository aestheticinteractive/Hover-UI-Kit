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

using System.Collections;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Communicates the editor's Game view rect to the Oculus plugin,
/// allowing distortion rendering to target it.
/// </summary>
[InitializeOnLoad]
public class OVRGameView
{
	private static Rect cachedPosition;
	private static EditorWindow cachedEditorWindow = null;
	private static System.Reflection.MethodInfo cachedMethodInfo = null;

	static OVRGameView()
	{
		EditorApplication.update += OnUpdate;
	}

	public static EditorWindow GetMainGameView()
	{
		if (cachedEditorWindow == null)
		{
			if (cachedMethodInfo == null)
			{
				System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");
	
				cachedMethodInfo = type.GetMethod(
					"GetMainGameView",
					System.Reflection.BindingFlags.NonPublic |
					System.Reflection.BindingFlags.Static);
			}

			cachedEditorWindow = cachedMethodInfo.Invoke(null, null) as EditorWindow;
		}

		return cachedEditorWindow;
	}

	static void OnUpdate()
	{
		if (OVRManager.instance == null)
			return;

		EditorWindow gameView = GetMainGameView();
		if (gameView != null)
		{
			Rect pos = gameView.position;
			if (pos != cachedPosition)
			{
				cachedPosition = pos;

				OVRManager.display.SetViewport((int)pos.x, (int)pos.y, (int)pos.width, (int)pos.height);
			}
		}
	}
}
