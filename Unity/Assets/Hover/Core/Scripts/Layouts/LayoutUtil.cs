using UnityEngine;

namespace Hover.Core.Layouts {

	/*================================================================================================*/
	public static class LayoutUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector2 GetRelativeAnchorPosition(AnchorType pAnchor) {
			int ai = (int)pAnchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			return new Vector2(-x, y);
		}

	}

}
