using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastInteractionSettings : MonoBehaviour {

		public bool IsMenuOnLeftSide = true;
		public bool ApplyScaleMultipler = true;
		public float HighlightDistanceMin = 0.03f;
		public float HighlightDistanceMax = 0.07f;
		public float StickyReleaseDistance = 0.05f;
		public float SelectionMilliseconds = 400;

		private InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual InteractionSettings GetSettings() {
			if ( vSettings == null ) {
				vSettings = new InteractionSettings();
				vSettings.IsMenuOnLeftSide = IsMenuOnLeftSide;
				vSettings.ApplyScaleMultiplier = ApplyScaleMultipler;
				vSettings.HighlightDistanceMin = HighlightDistanceMin;
				vSettings.HighlightDistanceMax = HighlightDistanceMax;
				vSettings.StickyReleaseDistance = StickyReleaseDistance;
				vSettings.SelectionMilliseconds = SelectionMilliseconds;
			}

			return vSettings;
		}

	}

}
