using Leap;

namespace Hover.Board.Devices.Leap.Touch {

	/*================================================================================================*/
	public class LeapInputSettings {

		public Finger.FingerType CursorPrimaryFinger { get; set; }
		public Finger.FingerType CursorSecondaryFinger { get; set; }
		public bool UseSecondary { get; set; }

	}

}
