namespace Hover.Cursors {

	/*================================================================================================*/
	public interface IInteractionSettings {

		float HighlightDistanceMin { get; set; }
		float HighlightDistanceMax { get; set; }
		float StickyReleaseDistance { get; set; }
		float SelectionMilliseconds { get; set; }

	}

}
