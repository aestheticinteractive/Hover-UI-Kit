using System;

namespace Hover.Engines {

	/*================================================================================================*/
	public struct Vector3 {

		public static readonly Vector3 Zero = new Vector3();
		public static readonly Vector3 Right = new Vector3(1);
		public static readonly Vector3 Left = new Vector3(-1);
		public static readonly Vector3 Up = new Vector3(0, 1);
		public static readonly Vector3 Down = new Vector3(0, -1);
		public static readonly Vector3 Fore = new Vector3(0, 0, 1);
		public static readonly Vector3 Back = new Vector3(0, 0, -1);
		public static readonly Vector3 One = new Vector3(1, 1, 1);

		public float X;
		public float Y;
		public float Z;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3(float pX=0, float pY=0, float pZ=0) {
			X = pX;
			Y = pY;
			Z = pZ;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 operator +(Vector3 pVecA, Vector3 pVecB) {
			return new Vector3(pVecA.X+pVecB.X, pVecA.Y+pVecB.Y, pVecA.Z+pVecB.Z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 operator -(Vector3 pVecA, Vector3 pVecB) {
			return new Vector3(pVecA.X-pVecB.X, pVecA.Y-pVecB.Y, pVecA.Z-pVecB.Z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 operator *(Vector3 pVec, float pScalar) {
			return new Vector3(pVec.X*pScalar, pVec.Y*pScalar, pVec.Z*pScalar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static bool operator ==(Vector3 pVecA, Vector3 pVecB) {
			return (pVecA.X == pVecB.X && pVecA.Y == pVecB.Y && pVecA.Z == pVecB.Z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static bool operator !=(Vector3 pVecA, Vector3 pVecB) {
			return !(pVecA == pVecB);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float DotWith(Vector3 pVector) {
			return X*pVector.X + Y*pVector.Y + Z*pVector.Z;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 Normalized {
			get {
				float invMag = 1/Magnitude;
				return new Vector3(X*invMag, Y*invMag, Z*invMag);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float Magnitude {
			get {
				return (float)Math.Sqrt(SquareMagnitude);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SquareMagnitude {
			get {
				return DotWith(this);
			}
		}

	}

}
