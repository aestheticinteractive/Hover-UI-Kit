using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Board.Renderers.Helpers {

	/*================================================================================================*/
	public static class RendererHelper {

		private const string BulletText = "- ";
		private const string DisabledSettingsText =
			"The disabled settings below are controlled by:\n{0}";

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsUpdatePreventedBy(ISettingsController pController) {
			return (pController != null && pController.isActiveAndEnabled);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetSettingsControllerName(ISettingsController pController) {
			return pController.name+" ("+pController.GetType().Name+")";
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetDisabledSettingsText(params ISettingsController[] pControllers) {
			if ( pControllers.Length == 1 ) {
				return string.Format(DisabledSettingsText, 
					BulletText+GetSettingsControllerName(pControllers[0]));
			}

			string list = "";

			foreach ( ISettingsController cont in pControllers ) {
				if ( IsUpdatePreventedBy(cont) ) {
					list += BulletText+GetSettingsControllerName(cont);
				}
			}

			return string.Format(DisabledSettingsText, list);
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
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T DestroyRenderer<T>(T pRenderer) where T : Component {
			if ( pRenderer == null ) {
				return default(T);
			}

			if ( Application.isPlaying ) {
				Object.Destroy(pRenderer.gameObject);
			}
			else {
				Object.DestroyImmediate(pRenderer.gameObject, false);
			}

			return default(T);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T FindInImmediateChildren<T>(Transform pParentTx) where T : Component {
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
