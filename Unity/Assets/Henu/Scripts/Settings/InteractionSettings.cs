namespace Henu.Settings {

	/*================================================================================================*/
	public class InteractionSettings {

		public virtual float NavBackGrabThreshold { get; set; }
		public virtual float NavBackUngrabThreshold { get; set; }

		public virtual float HighlightDistanceMin { get; set; }
		public virtual float HighlightDistanceMax { get; set; }
		public virtual float StickyReleaseDistance { get; set; }
		public virtual float SelectionMilliseconds { get; set; }

		public virtual float CursorForwardDistance { get; set; }

	}

}
