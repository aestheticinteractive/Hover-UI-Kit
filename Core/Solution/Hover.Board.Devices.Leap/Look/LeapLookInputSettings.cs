using Hover.Board.Devices.Leap.Touch;
using Hover.Engines;

namespace Hover.Board.Devices.Leap.Look {

	/*================================================================================================*/
	public class LeapLookInputSettings : LeapInputSettings {

		//TODO: hide the "Cursor Finger" setting
		//TODO: custom editor with a slider with range [0,1] for "Cursor Horizontal Offset"

		public ITransform LeapTransform { get; set; }
		public ITransform CameraTransform { get; set; }
		public float CursorHorizontalOffset { get; set; }

	}

}
