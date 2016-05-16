namespace Hover.Common.Input {

	/*================================================================================================*/
	public class BaseInteractionSettings {

		public float HighlightDistanceMin { get; set; }
		public float HighlightDistanceMax { get; set; }
		public float StickyReleaseDistance { get; set; }
		public float SelectionMilliseconds { get; set; }
		public bool ApplyScaleMultiplier { get; set; }
		public float ScaleMultiplier { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseInteractionSettings() {
			HighlightDistanceMin = 3;
			HighlightDistanceMax = 7;
			StickyReleaseDistance = 5;
			SelectionMilliseconds = 400;
			ApplyScaleMultiplier = false;
			ScaleMultiplier = 1;
		}

	}

}
