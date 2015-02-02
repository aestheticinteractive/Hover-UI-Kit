using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputCursor : MonoBehaviour, IInputCursor {

		public Vector3 TestPosition;
		public Quaternion TestRotation;

		public bool IsLeft { get; internal set; }
		public Vector3 Position { get { return TestPosition; } }
		public Quaternion Rotation { get { return TestRotation; } }

	}

}
