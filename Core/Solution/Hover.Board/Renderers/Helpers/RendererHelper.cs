using System;
using UnityEngine;

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
		
	}

}
