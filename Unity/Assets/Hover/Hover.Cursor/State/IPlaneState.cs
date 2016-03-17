using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IPlaneState {

		PlaneData Data { get; }
		Vector3 PointWorld { get; }
		Vector3 NormalWorld { get; }

		bool IsHit { get; }
		Vector3 HitPosition { get; }
		float HitDist { get; }
		bool IsNearest { get; }

	}

}
