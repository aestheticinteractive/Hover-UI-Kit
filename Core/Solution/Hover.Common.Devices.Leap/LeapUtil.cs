using Leap;
using UnityEngine;

namespace Hover.Common.Devices.Leap {

	/*================================================================================================*/
	public static class LeapUtil {

		public const float InputScale = 0.001f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnity(this Vector pLeapVector) {
			return new Vector3(pLeapVector.x, pLeapVector.y, -pLeapVector.z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnityScaled(this Vector pLeapVector) {
			return ToUnity(pLeapVector)*InputScale;
		}

	}

}
