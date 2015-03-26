using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IInteractionPlaneState {

		string Id { get; }
		Vector3 PointWorld { get; }
		Vector3 NormalWorld { get; }
		bool IsEnabled { get; set; }

		bool IsHit { get; }
		Vector3 HitPosition { get; }
		float HitDist { get; }
		bool IsNearest { get; }

	}

}
