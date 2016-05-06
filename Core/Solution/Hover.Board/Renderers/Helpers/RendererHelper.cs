using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Board.Renderers.Helpers {

	/*================================================================================================*/
	public static class RendererHelper {


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
		
	}

}
