using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerData {

		public Vector3 Position { get; set; }
		public Vector3 Direction { get; set; }
		public Quaternion Rotation { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public FingerData() {
		}

		/*--------------------------------------------------------------------------------------------*/
		public FingerData(Finger pFinger) {
			Bone bone = pFinger.Bone(Bone.BoneType.TYPE_DISTAL);
			float[] mat = bone.Basis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);

			Position = pFinger.TipPosition.ToUnityScaled();
			Direction = bone.Direction.ToUnity();
			Rotation = Quaternion.LookRotation(column2, column1);

			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html
		}

	}

}
