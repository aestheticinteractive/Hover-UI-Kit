namespace Hover.Engines {

	/*================================================================================================*/
	public struct Quaternion {

		public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

		public float X;
		public float Y;
		public float Z;
		public float W;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Quaternion(float pX=0, float pY=0, float pZ=0, float pW=0) {
			X = pX;
			Y = pY;
			Z = pZ;
			W = pW;
		}

	}

}
