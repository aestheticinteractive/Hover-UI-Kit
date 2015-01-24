using Hovercast.Input;
using UnityEngine;

namespace HovercastTest.Input {

	/*================================================================================================*/
	public class TestInputPoint : MonoBehaviour, IInputPoint {

		public Vector3 TestPosition;
		public Quaternion TestRotation;

		public Vector3 Position { get { return TestPosition; } }
		public Quaternion Rotation { get { return TestRotation; } }

	}

}
