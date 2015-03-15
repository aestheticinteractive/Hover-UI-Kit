namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public class EngineMath : IEngineMath {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 NewVector3Random() {
			return UnityEngine.Random.insideUnitSphere.ToHover();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Quaternion NewQuaternionAngleAxis(float pAngle, Vector3 pAxis) {
			return UnityEngine.Quaternion.AngleAxis(pAngle, pAxis.ToUnity()).ToHover();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion NewQuaternionFromTo(Vector3 pFrom, Vector3 pTo) {
			return UnityEngine.Quaternion.FromToRotation(pFrom.ToUnity(), pTo.ToUnity()).ToHover();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion NewQuaternionRandom() {
			return UnityEngine.Random.rotationUniform.ToHover();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Quaternion RotateQuaternion(Quaternion pQuaternion, Quaternion pRotation) {
			return (pQuaternion.ToUnity()*pRotation.ToUnity()).ToHover();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 RotateVector(Vector3 pVector, Quaternion pRotation) {
			return (pRotation.ToUnity()*pVector.ToUnity()).ToHover();
		}

	}

}
