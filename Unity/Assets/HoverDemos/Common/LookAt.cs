using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class LookAt : MonoBehaviour {

		public Transform Target;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			transform.LookAt(Target);
			//transform.rotation *= Quaternion.FromToRotation(Vector3.forward, Vector3.back);
		}

	}

}
