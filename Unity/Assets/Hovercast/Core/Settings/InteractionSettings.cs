namespace Hovercast.Core.Settings {

	/*================================================================================================*/
	public class InteractionSettings {

		public virtual bool IsMenuOnLeftSide { get; set; }

		public virtual float HighlightDistanceMin { get; set; }
		public virtual float HighlightDistanceMax { get; set; }
		public virtual float StickyReleaseDistance { get; set; }
		public virtual float SelectionMilliseconds { get; set; }

		public virtual float CursorForwardDistance { get; set; }

	}

}
