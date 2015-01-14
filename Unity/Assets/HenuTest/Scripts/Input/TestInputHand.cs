using Henu.Input;
using UnityEngine;

namespace HenuTest.Input {

	/*================================================================================================*/
	public class TestInputHand : MonoBehaviour, IInputHand {

		public Vector3 TestCenter;
		public Quaternion TestRotation;
		public float TestGrabStrength;
		public float TestPalmTowardEyes = 1;

		public bool IsLeft { get; set; }
		public Vector3 Center { get { return TestCenter; } }
		public Quaternion Rotation { get { return TestRotation; } }
		public float GrabStrength { get { return TestGrabStrength; } }
		public float PalmTowardEyes { get { return TestPalmTowardEyes; } }

	}

}
