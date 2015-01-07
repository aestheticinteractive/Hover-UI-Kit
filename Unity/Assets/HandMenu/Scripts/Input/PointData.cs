using System;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class PointData {

		public enum PointZone {
			Index,
			IndexMiddle,
			Middle,
			MiddleRing,
			Ring,
			RingPinky,
			Pinky
		};

		public PointZone Zone { get; private set; }
		public Vector3 Position { get; private set; }
		public Vector3 Direction { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Extension { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PointData(Hand pHand, Finger pFinger) {
			Bone bone = pFinger.Bone(Bone.BoneType.TYPE_DISTAL);
			float[] mat = bone.Basis.ToArray4x4();
			var column2 = new Vector3(mat[8], mat[9], -mat[10]);
			var column1 = new Vector3(mat[4], mat[5], -mat[6]);

			Position = pFinger.TipPosition.ToUnityScaled();
			Direction = bone.Direction.ToUnity();
			Rotation = Quaternion.LookRotation(column2, column1);
			//Rotation = Quaternion.FromToRotation(Vector3.forward, Direction);

			Extension = Vector3.Dot(Direction, -pHand.Direction.ToUnity());
			Extension = Math.Max(0, Extension);

			//Quaternion created using notes from:
			//answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html
		}

		/*--------------------------------------------------------------------------------------------*/
		private PointData() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static PointData Lerp(PointData pData0, PointData pData1, float pAmount) {
			if ( pData0 == null || pData1 == null ) {
				return null;
			}

			var data = new PointData();
			data.Position = Vector3.Slerp(pData0.Position, pData1.Position, pAmount);
			data.Direction = Vector3.Slerp(pData0.Direction, pData1.Direction, pAmount);
			data.Rotation = Quaternion.Slerp(pData0.Rotation, pData1.Rotation, pAmount);
			data.Extension = pData0.Extension*(1-pAmount) + pData1.Extension*pAmount;
			return data;
		}

	}

}
