using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	public interface ICursorIdle {

		bool IsActive { get; }
		float Progress { get; }
		Vector3 WorldPosition { get; }
		float DistanceThreshold { get; }

	}

}
