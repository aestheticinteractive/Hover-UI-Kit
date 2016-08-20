using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	public interface ICursorIdle {

		float Progress { get; }
		Vector3 WorldPosition { get; }
		float DistanceThreshold { get; }

	}

}
