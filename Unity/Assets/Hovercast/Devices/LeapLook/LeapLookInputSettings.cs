using Hovercast.Devices.Leap;
using UnityEngine;

namespace Hovercast.Devices.LeapLook {

	/*================================================================================================*/
	public class LeapLookInputSettings : LeapInputSettings {

		//TODO: hide the "Cursor Finger" setting
		//TODO: custom editor with a slider with range [0,1] for "Cursor Horizontal Offset"

		public Transform LeapTransform { get; set; }
		public Transform CameraTransform { get; set; }
		public float CursorHorizontalOffset { get; set; }

	}

}
