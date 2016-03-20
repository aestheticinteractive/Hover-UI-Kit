using UnityEngine;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class InputSettings {

		public UnityEngine.Transform CameraTransform { get; set; }
		public float DistanceFromPalm { get; set; }
		public float NavBackGrabThreshold { get; set; }
		public float NavBackUngrabThreshold { get; set; }
	}

}
