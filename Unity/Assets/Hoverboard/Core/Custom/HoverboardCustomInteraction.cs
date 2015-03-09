using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public class HoverboardCustomInteraction : MonoBehaviour {

		public float HighlightDistanceMin = 0.5f;
		public float HighlightDistanceMax = 1.2f;
		public float StickyReleaseDistance = 0.8f;
		public float SelectionMilliseconds = 400;
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
