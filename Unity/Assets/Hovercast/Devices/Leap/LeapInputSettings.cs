using Leap;
using UnityEngine;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	internal class LeapInputSettings {

		public Vector3 PalmDirection { get; set; }
		public Finger.FingerType CursorFinger { get; set; }
		public float NavBackGrabThreshold { get; set; }
		public float NavBackUngrabThreshold { get; set; }
	}

}
