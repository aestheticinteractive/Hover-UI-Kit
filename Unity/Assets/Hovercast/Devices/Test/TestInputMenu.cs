using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputMenu : MonoBehaviour, IInputMenu {

		public bool TestIsActive = true;
		public float TestRadius = 0.1f;
		public float TestNavigateBackStrength;
		public float TestDisplayStrength = 1;

		public bool IsLeft { get; internal set; }
		public bool IsActive { get { return TestIsActive; } }

		public Vector3 Position { get { return gameObject.transform.position; } }
		public Quaternion Rotation { get { return gameObject.transform.rotation; } }
		public float Radius { get { return TestRadius; } }

		public float NavigateBackStrength { get { return TestNavigateBackStrength; } }
		public float DisplayStrength { get { return TestDisplayStrength; } }

	}

}
