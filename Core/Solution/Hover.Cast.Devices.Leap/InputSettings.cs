using Leap;
using UnityEngine;

namespace Hover.Cast.Devices.Leap {

	/*================================================================================================*/
	public class InputSettings {

		public Vector3 PalmDirection { get; set; }
		public Finger.FingerType CursorFinger { get; set; }
		public float NavBackGrabThreshold { get; set; }
		public float NavBackUngrabThreshold { get; set; }
	}

}
