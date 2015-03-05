using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public class HovercastCustomInteraction : MonoBehaviour {

		public float HighlightDistanceMin = 1;
		public float HighlightDistanceMax = 2.5f;
		public float StickyReleaseDistance = 1.8f;
		public float SelectionMilliseconds = 250;
		public float CursorForwardDistance = 0.0f;

		private InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual InteractionSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new InteractionSettings();
				vSettings.HighlightDistanceMin = HighlightDistanceMin;
				vSettings.HighlightDistanceMax = HighlightDistanceMax;
				vSettings.StickyReleaseDistance = StickyReleaseDistance;
				vSettings.SelectionMilliseconds = SelectionMilliseconds;
				vSettings.CursorForwardDistance = CursorForwardDistance;
			}

			return vSettings;
		}

	}

}
