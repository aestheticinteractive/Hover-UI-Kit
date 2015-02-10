using UnityEngine;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public class HovercastCustomInteraction : MonoBehaviour {

		public bool IsMenuOnLeftSide = true;
		public float HighlightDistanceMin = 0.05f;
		public float HighlightDistanceMax = 0.1f;
		public float StickyReleaseDistance = 0.07f;
		public float SelectionMilliseconds = 600;
		public float CursorForwardDistance = 0.0f;

		private InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vSettings = new InteractionSettings();
			vSettings.IsMenuOnLeftSide = IsMenuOnLeftSide;
			vSettings.HighlightDistanceMin = HighlightDistanceMin;
			vSettings.HighlightDistanceMax = HighlightDistanceMax;
			vSettings.StickyReleaseDistance = StickyReleaseDistance;
			vSettings.SelectionMilliseconds = SelectionMilliseconds;
			vSettings.CursorForwardDistance = CursorForwardDistance;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual InteractionSettings GetSettings() {
			return vSettings;
		}

	}

}
