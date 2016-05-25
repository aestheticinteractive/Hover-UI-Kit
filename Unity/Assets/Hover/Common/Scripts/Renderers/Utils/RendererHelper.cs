using System;
using System.Collections.Generic;
using Hover.Common.Renderers.Shapes.Rect;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Utils {

	/*================================================================================================*/
	public static class RendererHelper {

		private const string BulletText = "\n - ";
		private const string ControlledSettingsText =
			"The following settings are controlled externally.{0}";
		private const string ValueControlledBySelfText = "self (locked)";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetSettingsControllerName(ISettingsController pController) {
			return pController.name+" ("+pController.GetType().Name+")";
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetControlledSettingsText(
									UnityEngine.Object pSelf, ISettingsControllerMap pControllerMap) {
			if ( !Application.isEditor ) {
				throw new Exception("This method is meant for editor mode only.");
			}

			List<string> valueNames = pControllerMap.GetNewListOfControlledValueNames();
			string text = "";

			for ( int i = 0 ; i < valueNames.Count ; i++ ) {
				string valueName = valueNames[i];
				string valueNameDisplay = valueName.Replace("_", "");
				ISettingsController controller = pControllerMap.Get(valueName);
				string contName = ((controller as UnityEngine.Object) == pSelf ? 
					ValueControlledBySelfText : GetSettingsControllerName(controller));

				text += BulletText+valueNameDisplay+": "+contName;
			}

			return string.Format(ControlledSettingsText, text);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T FindOrBuildRenderer<T>(Transform pParentTx, GameObject pPrefab,
							string pDisplayName, Type pDefaultType) where T : class, IRenderer {
			T existing = FindInImmediateChildren<T>(pParentTx);

			if ( existing != null ) {
				return existing;
			}

			return BuildRenderer<T>(pParentTx, pPrefab, pDisplayName, pDefaultType);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static T BuildRenderer<T>(Transform pParentTx, GameObject pPrefab,
						string pDisplayTypeName, Type pDefaultType) where T : class, IRenderer {
			if ( pPrefab != null ) {
				T prefabRend = TryBuildPrefabRenderer<T>(pPrefab);

				if ( prefabRend != null ) {
					prefabRend.gameObject.transform.SetParent(pParentTx, false);
					return prefabRend;
				}

				Debug.LogError(pDisplayTypeName+" prefab '"+pPrefab.name+"' must contain a '"+
					typeof(T)+"' component. ", pParentTx);
			}

			Debug.Log("Building default "+pDisplayTypeName.ToLower()+" renderer.", pParentTx);

			var buttonGo = new GameObject(pDisplayTypeName+"Renderer");
			buttonGo.transform.SetParent(pParentTx, false);
			return (buttonGo.AddComponent(pDefaultType) as T);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static T TryBuildPrefabRenderer<T>(GameObject pPrefab) where T : IRenderer {
			if ( pPrefab.GetComponent<T>() == null ) {
				return default(T);
			}

#if UNITY_EDITOR
			GameObject prefabGo = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pPrefab);
#else
			GameObject prefabGo = UnityEngine.Object.Instantiate(pPrefab);
#endif
			return prefabGo.GetComponent<T>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void DestroyRenderer<T>(T pRenderer) where T : IRenderer {
			if ( pRenderer == null || pRenderer.gameObject == null ) {
				return;
			}

#if UNITY_EDITOR
			UnityEditor.PrefabUtility.DisconnectPrefabInstance(pRenderer.gameObject);
#endif

			if ( Application.isPlaying ) {
				UnityEngine.Object.Destroy(pRenderer.gameObject);
			}
			else {
				UnityEngine.Object.DestroyImmediate(pRenderer.gameObject, false);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private static T FindInImmediateChildren<T>(Transform pParentTx) where T : IRenderer {
			foreach ( Transform childTx in pParentTx ) {
				T renderer = childTx.GetComponent<T>();
				
				if ( renderer != null ) {
					return renderer;
				}
			}

			return default(T);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector2 GetRelativeAnchorPosition(AnchorType pAnchor) {
			if ( pAnchor == AnchorType.Custom ) {
				throw new Exception("There is no pre-set position for the '"+pAnchor+"' type.");
			}
			
			int ai = (int)pAnchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			return new Vector2(-x, y);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void SetActiveWithUpdate(MonoBehaviour pBehaviour, bool pIsActive) {
			GameObject go = pBehaviour.gameObject;
			bool wasActive = go.activeSelf;

			go.SetActive(pIsActive);

			if ( pIsActive && !wasActive ) {
				go.SendMessage("Update", SendMessageOptions.DontRequireReceiver);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 GetNearestWorldPositionOnRectangle(Vector3 pFromWorldPosition, 
												Transform pRectangleTx, float pSizeX, float pSizeY) {
			Vector3 fromLocalPos = pRectangleTx.InverseTransformPoint(pFromWorldPosition);

			var nearLocalPos = new Vector3(
				Mathf.Clamp(fromLocalPos.x, -pSizeX/2, pSizeX/2),
				Mathf.Clamp(fromLocalPos.y, -pSizeY/2, pSizeY/2),
				0
			);

			return pRectangleTx.TransformPoint(nearLocalPos);
		}

	}

}
