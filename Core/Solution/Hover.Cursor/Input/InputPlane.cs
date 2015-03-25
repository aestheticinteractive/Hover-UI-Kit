using UnityEngine;

namespace Hover.Cursor.Input {

	/*================================================================================================*/
	public class InputPlane {

		public string Id { get; set; }
		public Vector3 PointWorld { get; set; }
		public Vector3 NormalWorld { get; set; }

		public bool IsHit { get; set; }
		public Vector3 HitPosition { get; set; }
		public float HitDist { get; set; }
		public bool IsNearest { get; set; }

	}

}
