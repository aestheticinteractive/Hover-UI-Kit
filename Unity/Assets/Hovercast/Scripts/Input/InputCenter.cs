using Leap;
using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public class InputCenter : IInputCenter {

		public bool IsLeft { get; set; }
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public float GrabStrength { get; set; }
		public float PalmTowardEyes { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputCenter(Hand pHand, Vector3 pPalmDirection) {
			IsLeft = pHand.IsLeft;
			Position = pHand.PalmPosition.ToUnityScaled();
			Rotation = CalcQuaternion(pHand.Basis);
			GrabStrength = pHand.GrabStrength;
			PalmTowardEyes = Vector3.Dot(pHand.PalmNormal.ToUnity(), pPalmDirection);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static Quaternion CalcQuaternion(Matrix pBasis) {
			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html

			float[] mat = pBasis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);
			return Quaternion.LookRotation(column2, column1);
		}

	}

}
