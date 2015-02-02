using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputMenu : MonoBehaviour, IInputMenu {

		public bool TestIsActive;
		public Vector3 TestPosition;
		public Quaternion TestRotation;
		public float TestRadius;
		public float TestNavigateBackStrength;
		public float TestDisplayStrength = 1;

		public bool IsLeft { get; set; }
		public bool IsActive { get { return TestIsActive; } }
		
		public Vector3 Position { get { return TestPosition; } }
		public Quaternion Rotation { get { return TestRotation; } }
		public float Radius { get { return TestRadius; } }

		public float NavigateBackStrength { get { return TestNavigateBackStrength; } }
		public float DisplayStrength { get { return TestDisplayStrength; } }

	}

}
