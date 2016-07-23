using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	public interface IHoverCursorIdle {

		float Progress { get; }
		Vector3 WorldPosition { get; }
		float DistanceThreshold { get; }

	}

}
