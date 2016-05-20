using System;
using System.Collections.Generic;
using Hover.Common.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Common.Renderers.Helpers {

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
											Object pSelf, ISettingsControllerMap pControllerMap) {
			if ( !Application.isEditor ) {
				throw new Exception("This method is meant for editor mode only.");
			}

			List<string> valueNames = pControllerMap.GetNewListOfControlledValueNames();
			string text = "";

			for ( int i = 0 ; i < valueNames.Count ; i++ ) {
				string valueName = valueNames[i];
				string valueNameDisplay = valueName.Replace("_", "");
				ISettingsController controller = pControllerMap.Get(valueName);
				string contName = ((controller as Object) == pSelf ? 
					ValueControlledBySelfText : GetSettingsControllerName(controller));

				text += BulletText+valueNameDisplay+": "+contName;
			}

			return string.Format(ControlledSettingsText, text);
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
