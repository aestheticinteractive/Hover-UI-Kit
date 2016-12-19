using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Utils {

	/*================================================================================================*/
	public static class RendererUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T FindOrBuildRenderer<T>(Transform pParentTx, GameObject pPrefab,
						string pDisplayName, Type pDefaultType) where T : class, IGameObjectProvider {
			T existing = FindInImmediateChildren<T>(pParentTx);

			if ( existing != null ) {
				return existing;
			}

			return BuildRenderer<T>(pParentTx, pPrefab, pDisplayName, pDefaultType);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static T BuildRenderer<T>(Transform pParentTx, GameObject pPrefab,
					string pDisplayTypeName, Type pDefaultType) where T : class, IGameObjectProvider {
			if ( pPrefab != null ) {
				T prefabRend = TryBuildPrefabRenderer<T>(pPrefab);

				if ( prefabRend != null ) {
					prefabRend.gameObject.transform.SetParent(pParentTx, false);

					TreeUpdater treeUp = prefabRend.gameObject.GetComponent<TreeUpdater>();

					if ( treeUp != null ) {
						treeUp.UpdateAtAndBelowThisLevel();
					}

					return prefabRend;
				}

				Debug.LogError(pDisplayTypeName+" prefab '"+pPrefab.name+"' must contain a '"+
					typeof(T)+"' component. ", pParentTx);
			}

			Debug.LogWarning("Could not find existing renderer, and no prefab provided.", pParentTx);
			return default(T);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T TryBuildPrefabRenderer<T>(GameObject pPrefab) {
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
		public static void DestroyRenderer<T>(T pRenderer) where T : IGameObjectProvider {
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
		private static T FindInImmediateChildren<T>(Transform pParentTx) where T : IGameObjectProvider {
			int childCount = pParentTx.childCount;

			for ( int i = 0 ; i < childCount ; i++ ) {
				Transform childTx = pParentTx.GetChild(i);
				T renderer = childTx.GetComponent<T>();
				
				if ( renderer != null ) {
					return renderer;
				}
			}

			return default(T);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void SetActiveWithUpdate(MonoBehaviour pBehaviour, bool pIsActive) {
			SetActiveWithUpdate(pBehaviour.gameObject, pIsActive);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void SetActiveWithUpdate(GameObject pGameObj, bool pIsActive) {
			bool wasActive = pGameObj.activeSelf;

			pGameObj.SetActive(pIsActive);

			if ( pIsActive && !wasActive ) {
				pGameObj.SendMessage("TreeUpdate", SendMessageOptions.DontRequireReceiver);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Plane GetWorldPlane(Transform pTx) {
			return new Plane(pTx.rotation*Vector3.back, pTx.position);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3? GetNearestWorldPositionOnPlane(Ray pFromWorldRay, Plane pWorldPlane) {
			float enter;
			pWorldPlane.Raycast(pFromWorldRay, out enter);

			if ( enter <= 0 ) {
				return null;
			}

			return pFromWorldRay.GetPoint(enter);
		}

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
						Transform pArcTx, float pOuterRadius, float pInnerRadius, float pArcDegrees) {
			Vector3 fromLocalPos = pArcTx.InverseTransformPoint(pFromWorldPosition);

			if ( fromLocalPos.x == 0 && fromLocalPos.y == 0 ) {
				return pArcTx.TransformPoint(new Vector3(pInnerRadius, 0, 0));
			}

			fromLocalPos.z = 0;

			float fromLocalPosMag = fromLocalPos.magnitude;
			Vector3 fromLocalDir = fromLocalPos/fromLocalPosMag;
			Quaternion fromLocalRot = Quaternion.FromToRotation(Vector3.right, fromLocalDir);
			float halfDeg = pArcDegrees/2;
			float fromRadius = Mathf.Clamp(fromLocalPosMag, pInnerRadius, pOuterRadius);
			float fromDeg;
			Vector3 fromAxis;

			fromLocalRot.ToAngleAxis(out fromDeg, out fromAxis);

			if ( fromLocalPos.x > 0 && fromDeg >= -halfDeg && fromDeg <= halfDeg ) {
				Quaternion nearLocalRot = Quaternion.AngleAxis(fromDeg, fromAxis);
				Vector3 nearLocalPos = nearLocalRot*new Vector3(fromRadius, 0, 0);
				return pArcTx.TransformPoint(nearLocalPos);
			}

			float rotatedDeg = -halfDeg*Mathf.Sign(fromLocalPos.y);
			Quaternion rotatedRot = Quaternion.AngleAxis(rotatedDeg, Vector3.forward);

			Vector3 fromRotatedPos = rotatedRot*fromLocalPos;
			fromRotatedPos.x = Mathf.Clamp(fromRotatedPos.x, pInnerRadius, pOuterRadius);
			fromRotatedPos.y = 0;

			Vector3 fromClampedRotatedPos = Quaternion.Inverse(rotatedRot)*fromRotatedPos;
			return pArcTx.TransformPoint(fromClampedRotatedPos);
		}

	}

}
