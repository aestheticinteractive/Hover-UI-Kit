using UnityEngine;

namespace Hover.Cast.Input {

	/*================================================================================================*/
	public interface IInputMenu {

		bool IsLeft { get; }
		bool IsAvailable { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }
		float Radius { get; }

		float NavigateBackStrength { get; }
		float DisplayStrength { get; }

	}

}
