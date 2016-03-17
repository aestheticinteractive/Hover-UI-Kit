using Hover.Common.Display;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiItemStickyRenderer : UiItemBaseIconRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetIconOffset() {
			return Materials.IconOffset.Sticky;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3 GetIconScale() {
			float s = vSettings.TextSize*ArcCanvasScale;
			return new Vector3(s, s, 1);
		}

	}

}
