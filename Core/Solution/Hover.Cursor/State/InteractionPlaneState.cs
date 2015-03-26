using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class InteractionPlaneState : IInteractionPlaneState {

		public string Id { get; private set; }
		public bool IsEnabled { get; set; }

		public bool IsHit { get; set; }
		public Vector3 HitPosition { get; set; }
		public float HitDist { get; set; }
		public bool IsNearest { get; set; }

		private readonly Transform vTransform;
		private readonly Vector3 vLocalNormal;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InteractionPlaneState(string pId, Transform pTransform, Vector3 pLocalNormal) {
			Id = pId;
			IsEnabled = true;

			vTransform = pTransform;
			vLocalNormal = pLocalNormal;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 PointWorld {
			get {
				return vTransform.position;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 NormalWorld {
			get {
				return vTransform.rotation*vLocalNormal;
			}
		}

	}

}
