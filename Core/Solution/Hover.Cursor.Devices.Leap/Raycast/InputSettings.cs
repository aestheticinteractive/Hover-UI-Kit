using UnityEngine;

namespace Hover.Cursor.Devices.Leap.Raycast {

	/*================================================================================================*/
	public class InputSettings : Hands.InputSettings {

		//TODO: hide the "Cursor Finger" setting
		//TODO: custom editor with a slider with range [0,1] for "Cursor Horizontal Offset"

		public Transform LeapTransform { get; set; }
		public Transform CameraTransform { get; set; }
		public float CursorHorizontalOffset { get; set; }

	}

}
