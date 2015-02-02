using UnityEngine;

namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputMenu {

		bool IsLeft { get; }
		bool IsActive { get; }

		Vector3 Position { get; }
		Quaternion Rotation { get; }
		float Radius { get; }

		float NavigateBackStrength { get; }
		float DisplayStrength { get; }

	}

}
