using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public class FollowTransform : MonoBehaviour {

		public Transform Follow;
		public bool FollowMainCamera;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			Transform tx = (FollowMainCamera ? Camera.main.transform : Follow);
			transform.position = tx.position;
			transform.rotation = tx.rotation;
		}

	}

}
