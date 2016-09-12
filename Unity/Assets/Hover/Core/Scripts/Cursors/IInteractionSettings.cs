namespace Hover.Core.Cursors {

	/*================================================================================================*/
	public interface IInteractionSettings {

		float HighlightDistanceMin { get; set; }
		float HighlightDistanceMax { get; set; }
		float StickyReleaseDistance { get; set; }
		float SelectionMilliseconds { get; set; }
		float IdleDistanceThreshold { get; set; }
		float IdleMilliseconds { get; set; }

	}

}
