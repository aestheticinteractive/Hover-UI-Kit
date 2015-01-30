using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputCenter : MonoBehaviour, IInputCenter {

		public Vector3 TestCenter;
		public Quaternion TestRotation;
		public float TestGrabStrength;
		public float TestPalmTowardEyes = 1;

		public bool IsLeft { get; set; }
		public Vector3 Position { get { return TestCenter; } }
		public Quaternion Rotation { get { return TestRotation; } }
		public float GrabStrength { get { return TestGrabStrength; } }
		public float PalmTowardEyes { get { return TestPalmTowardEyes; } }

	}

}
