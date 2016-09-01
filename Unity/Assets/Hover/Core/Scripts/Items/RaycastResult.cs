using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	public struct RaycastResult {
		
		public bool IsHit;
		public Vector3 WorldPosition;
		public Quaternion WorldRotation;
		public Plane WorldPlane;

	}

}
