using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverSceneLoader : MonoBehaviour {

		[Serializable]
		public class SceneLoadedEventHandler : UnityEvent<HoverSceneLoader> {}

		public string SceneFolderPath = "Hover/InputModules/NAME/Scenes/";
		public string SceneName = "HoverInputModule-NAME";

		[Header("Disable this setting when creating builds!")]
		public bool AutoLoadInEditor = false;

		public SceneLoadedEventHandler OnSceneLoadedEvent;

		[TriggerButton("Reload Scene")]
		public bool ClickToReloadScene;

		private Scene? vLoadedScene;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Application.isPlaying || AutoLoadInEditor ) {
				//StartCoroutine(LoadWhenReady());
				LoadScene();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
#if UNITY_EDITOR
			if ( !Application.isPlaying || vLoadedScene == null ) {
				return;
			}

			SceneManager.UnloadScene((Scene)vLoadedScene);
			Debug.Log("Removed scene for editor: "+vLoadedScene, this);
#endif
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			LoadScene();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private IEnumerator LoadWhenReady() {
			yield return new WaitUntil(() => {
				int sceneCount = SceneManager.sceneCount;
#if UNITY_EDITOR
				sceneCount = UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount+1;
#endif
				Debug.Log("TRY LOAD SCENE: "+name+"... "+sceneCount+" >= "+LoadAfterSceneCount);
				return (sceneCount >= LoadAfterSceneCount);
			});

			LoadScene();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void LoadScene() {
			if ( !Application.isPlaying ) {
				LoadSceneForNonplayingEditor();
				return;
			}

			LoadSceneForRuntime();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void LoadSceneForNonplayingEditor() {
#if UNITY_EDITOR
			string fullPath = Application.dataPath+"/"+SceneFolderPath+SceneName+".unity";

			vLoadedScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
				fullPath, UnityEditor.SceneManagement.OpenSceneMode.Additive);

			Debug.Log("Loaded scene for editor: "+fullPath, this);
			OnSceneLoadedEvent.Invoke(this);
#endif
		}

		/*--------------------------------------------------------------------------------------------*/
		private void LoadSceneForRuntime() {
			if ( SceneManager.GetSceneByName(SceneName).IsValid() ) {
				Debug.Log("Scene already loaded: "+SceneName, this);
				return;
			}

			string scenePathAndName = SceneFolderPath+SceneName;

			if ( SceneManager.GetSceneByName(scenePathAndName).IsValid() ) {
				Debug.Log("Scene already loaded: "+scenePathAndName, this);
				return;
			}

			SceneManager.LoadScene(scenePathAndName, LoadSceneMode.Additive);
			vLoadedScene = SceneManager.GetSceneByName(scenePathAndName);
			Debug.Log("Loaded scene: "+scenePathAndName, this);
			OnSceneLoadedEvent.Invoke(this);
		}

	}

}
