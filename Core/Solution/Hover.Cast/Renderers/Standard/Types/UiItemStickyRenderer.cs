using Hover.Common.Display;
using UnityEngine;

namespace Hover.Cast.Renderers.Standard.Types {

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
