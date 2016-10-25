using System;
using UnityEngine;

namespace Hover.Core.Layouts.Rect {

    /*================================================================================================*/
    [Serializable]
	public class HoverLayoutRectPaddingSettings {

		public float Top = 0;
		public float Bottom = 0;
		public float Left = 0;
		public float Right = 0;
		public float Between = 0;


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
