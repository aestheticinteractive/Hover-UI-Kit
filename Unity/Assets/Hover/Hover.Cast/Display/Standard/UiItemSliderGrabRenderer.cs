using Hover.Common.Display;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderGrabRenderer : UiItemBaseIconRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetIconOffset() {
			return Materials.IconOffset.Slider;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float sx = vSettings.TextSize*ArcCanvasScale;
			float sy = vSettings.TextSize*1.25f*ArcCanvasScale;
			return new Vector3(sx, sy, 1);
		}

	}

}
