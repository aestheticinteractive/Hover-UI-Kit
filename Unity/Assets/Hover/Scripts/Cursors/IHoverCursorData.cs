using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	public interface IHoverCursorData {

		CursorType Type { get; }
		bool IsActive { get; }
		bool AllowUsage { get; }
		float Size { get; }
		float DisplayStrength { get; }
		Vector3 WorldPosition { get; }
		Quaternion WorldRotation { get; }

	}

}
