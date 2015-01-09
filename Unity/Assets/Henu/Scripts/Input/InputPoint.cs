using System;
using Leap;
using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputPoint {

		public InputPointZone Zone { get; private set; }
		public Vector3 Position { get; private set; }
		public Vector3 Direction { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Extension { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputPoint(Hand pLeapHand, Finger pLeapFinger) {
			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);
			float[] mat = bone.Basis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);

			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Direction = bone.Direction.ToUnity();
			Rotation = Quaternion.LookRotation(column2, column1);
			//Rotation = Quaternion.FromToRotation(Vector3.forward, Direction);

			Extension = Vector3.Dot(Direction, -pLeapHand.Direction.ToUnity());
			Extension = Math.Max(0, Extension);

			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html
		}

		/*--------------------------------------------------------------------------------------------*/
		private InputPoint() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static InputPoint Lerp(InputPoint pPoint0, InputPoint pPoint1, float pAmount) {
			if ( pPoint0 == null || pPoint1 == null ) {
				return null;
			}

			var point = new InputPoint();
			point.Position = Vector3.Slerp(pPoint0.Position, pPoint1.Position, pAmount);
			point.Direction = Vector3.Slerp(pPoint0.Direction, pPoint1.Direction, pAmount);
			point.Rotation = Quaternion.Slerp(pPoint0.Rotation, pPoint1.Rotation, pAmount);
			point.Extension = pPoint0.Extension*(1-pAmount) + pPoint1.Extension*pAmount;
			return point;
		}

	}

}
