using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastCustomInteraction : MonoBehaviour {

		public bool IsMenuOnLeftSide = true;
		public float HighlightDistanceMin = 0.45f;
		public float HighlightDistanceMax = 0.9f;
		public float StickyReleaseDistance = 0.6f;
		public float SelectionMilliseconds = 600;
		public float CursorForwardDistance = 0.0f;

		private InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual InteractionSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new InteractionSettings();
				vSettings.IsMenuOnLeftSide = IsMenuOnLeftSide;
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
