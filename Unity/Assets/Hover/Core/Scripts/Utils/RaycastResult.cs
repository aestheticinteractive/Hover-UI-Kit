using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public struct RaycastResult {
		
		public bool IsHit;
		public Vector3 WorldPosition;
		public Quaternion WorldRotation;
		public Plane WorldPlane;

	}

}
