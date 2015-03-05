using Hoverboard.Core.Input;
using UnityEngine;

namespace Hoverboard.Devices.Test {

	/*================================================================================================*/
	public class TestInputCursor : MonoBehaviour, IInputCursor {

		public CursorType TestCursorType;
		public bool TestIsAvailable = true;
		public float TestSize = 0.1f;

		public CursorType Type { get { return TestCursorType; } }
		public bool IsAvailable { get { return TestIsAvailable; } }
		public Vector3 Position { get { return gameObject.transform.position; } }
		public Quaternion Rotation { get { return gameObject.transform.rotation; } }
		public float Size { get { return TestSize; } }

	}

}
