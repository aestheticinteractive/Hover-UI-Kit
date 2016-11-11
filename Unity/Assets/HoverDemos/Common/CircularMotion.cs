using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class CircularMotion : MonoBehaviour {

		public Vector3 CenterWorldPos = Vector3.zero;
		public Vector3 RotationWorldAxis = Vector3.forward;
		public Vector3 RotationWorldUp = Vector3.up;

		[Range(0, 0.2f)]
		public float Radius = 0.06f;

		[Range(0, 360)]
		public float Speed = 20;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !Application.isPlaying ) {
				transform.position = CenterWorldPos;
				return;
			}

			Quaternion rot = Quaternion.AngleAxis(Time.time*Speed, RotationWorldAxis);
			transform.position = CenterWorldPos + rot*(RotationWorldUp*Radius);
		}

	}

}
