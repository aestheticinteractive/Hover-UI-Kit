using System;
using System.Collections.Generic;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Utils {

	/*================================================================================================*/
	public static class RendererUtil {

#if UNITY_EDITOR
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
#endif


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
			return GetRelativeAnchorPosition((AnchorTypeWithCustom)pAnchor);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector2 GetRelativeAnchorPosition(AnchorTypeWithCustom pAnchor) {
			if ( pAnchor == AnchorTypeWithCustom.Custom ) {
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
													Transform pRectTx, float pSizeX, float pSizeY) {
			Vector3 fromLocalPos = pRectTx.InverseTransformPoint(pFromWorldPosition);

			var nearLocalPos = new Vector3(
				Mathf.Clamp(fromLocalPos.x, -pSizeX/2, pSizeX/2),
				Mathf.Clamp(fromLocalPos.y, -pSizeY/2, pSizeY/2),
				0
			);

			return pRectTx.TransformPoint(nearLocalPos);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 GetNearestWorldPositionOnArc(Vector3 pFromWorldPosition, 
						Transform pArcTx, float pOuterRadius, float pInnerRadius, float pArcAngle) {
			Vector3 fromLocalPos = pArcTx.InverseTransformPoint(pFromWorldPosition);
			fromLocalPos.z = 0;

			float fromLocalPosMag = fromLocalPos.magnitude;
			Vector3 fromLocalDir = fromLocalPos/fromLocalPosMag;
			float halfAngle = pArcAngle/2;
			float fromRadius = Mathf.Clamp(fromLocalPosMag, pInnerRadius, pOuterRadius);
			float fromAngle;
			Vector3 fromAxis;

			if ( Mathf.Abs(Vector3.Dot(Vector3.right, fromLocalDir)) < 0.999f ) {
				Quaternion fromLocalRot = Quaternion.FromToRotation(Vector3.right, fromLocalDir);
				fromLocalRot.ToAngleAxis(out fromAngle, out fromAxis);
			}
			else {
				fromAxis = Vector3.forward;
				fromAngle = 0;
			}

			if ( fromLocalPos.x > 0 && fromAngle >= -halfAngle && fromAngle <= halfAngle ) {
				Quaternion nearLocalRot = Quaternion.AngleAxis(fromAngle, fromAxis);
				Vector3 nearLocalPos = nearLocalRot*new Vector3(fromRadius, 0, 0);
				return pArcTx.TransformPoint(nearLocalPos);
			}

			float rotatedAngle = -halfAngle*Mathf.Sign(fromLocalPos.y);
			Quaternion rotatedRot = Quaternion.AngleAxis(rotatedAngle, Vector3.forward);

			Vector3 fromRotatedPos = rotatedRot*fromLocalPos;
			fromRotatedPos.x = Mathf.Clamp(fromRotatedPos.x, pInnerRadius, pOuterRadius);
			fromRotatedPos.y = 0;

			Vector3 fromClampedRotatedPos = Quaternion.Inverse(rotatedRot)*fromRotatedPos;
			return pArcTx.TransformPoint(fromClampedRotatedPos);
		}

	}

}
