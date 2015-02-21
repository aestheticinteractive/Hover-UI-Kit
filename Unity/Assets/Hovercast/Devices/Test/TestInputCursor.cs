using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputCursor : MonoBehaviour, IInputCursor {

		public bool TestIsAvailable = true;

		public bool IsLeft { get; internal set; }
		public bool IsAvailable { get { return TestIsAvailable; } }
		public Vector3 Position { get { return gameObject.transform.position; } }
		public Quaternion Rotation { get { return gameObject.transform.rotation; } }

	}

}
