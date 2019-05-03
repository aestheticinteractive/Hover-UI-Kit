using System;
using UnityEngine;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	[Serializable]
	public struct HoverLayoutRectPaddingSettings {

		public float Top;
		public float Bottom;
		public float Left;
		public float Right;
		public float Between;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void ClampValues(HoverLayoutRectRow pRectRow) {
			if ( pRectRow.ChildCount <= 0 ) {
				return;
			}

			Top = Mathf.Clamp(Top, 0, pRectRow.SizeY);
			Bottom = Mathf.Clamp(Bottom, 0, pRectRow.SizeY-Top);
			Left = Mathf.Clamp(Left, 0, pRectRow.SizeX);
			Right = Mathf.Clamp(Right, 0, pRectRow.SizeX-Left);

			float maxTotalBetweenSize = (pRectRow.IsHorizontal ? 
				pRectRow.SizeX-Left-Right : pRectRow.SizeY-Top-Bottom);

			Between = Mathf.Clamp(Between, 0, maxTotalBetweenSize/(pRectRow.ChildCount-1));
		}

	}

}
