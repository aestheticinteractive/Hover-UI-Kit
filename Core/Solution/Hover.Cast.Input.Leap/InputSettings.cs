using UnityEngine;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class InputSettings {

		public Vector3 PalmDirection { get; set; }
		public float DistanceFromPalm { get; set; }
		public float NavBackGrabThreshold { get; set; }
		public float NavBackUngrabThreshold { get; set; }
	}

}
