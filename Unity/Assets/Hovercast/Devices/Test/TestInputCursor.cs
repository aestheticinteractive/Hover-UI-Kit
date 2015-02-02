using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputCursor : MonoBehaviour, IInputCursor {

		public bool TestIsActive = true;

		public bool IsLeft { get; internal set; }
		public bool IsActive { get { return TestIsActive; } }
		public Vector3 Position { get { return gameObject.transform.position; } }
		public Quaternion Rotation { get { return gameObject.transform.rotation; } }

	}

}
