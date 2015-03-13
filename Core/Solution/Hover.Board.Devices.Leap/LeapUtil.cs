using Leap;
using UnityEngine;

namespace Hover.Board.Devices.Leap {

	/*================================================================================================*/
	public static class LeapUtil {

		public const float INPUT_SCALE = 0.001f;
		public static readonly Vector3 Z_FLIP = new Vector3(1, 1, -1);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnity(this Vector leap_vector, bool mirror = false) {
			if ( mirror )
				return ToVector3(leap_vector);

			return FlipZ(ToVector3(leap_vector));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToUnityScaled(this Vector leap_vector, bool mirror = false) {
			if ( mirror )
				return INPUT_SCALE * ToVector3(leap_vector);

			return INPUT_SCALE * FlipZ(ToVector3(leap_vector));
		}

		/*--------------------------------------------------------------------------------------------*/
		private static Vector3 FlipZ(Vector3 vector) {
			return Vector3.Scale(vector, Z_FLIP);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static Vector3 ToVector3(Vector vector) {
			return new Vector3(vector.x, vector.y, vector.z);
		}

	}

}