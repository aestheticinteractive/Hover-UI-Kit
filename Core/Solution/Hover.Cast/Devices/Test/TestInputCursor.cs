using Hover.Cast.Input;
using UnityEngine;

namespace Hover.Cast.Devices.Test {

	/*================================================================================================*/
	public class TestInputCursor : MonoBehaviour, IInputCursor {

		public bool TestIsAvailable = true;

		public bool IsLeft { get; internal set; }
		public bool IsAvailable { get { return TestIsAvailable; } }
		public Vector3 Position { get { return gameObject.transform.position; } }
		public Quaternion Rotation { get { return gameObject.transform.rotation; } }

	}

}
