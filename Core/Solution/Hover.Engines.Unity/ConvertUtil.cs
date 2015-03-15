namespace Hover.Engines.Unity {

	/*================================================================================================*/
	public static class ConvertUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static UnityEngine.Color ToUnity(this Color pSelf) {
			return new UnityEngine.Color(pSelf.R, pSelf.G, pSelf.B, pSelf.A);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Color ToHover(this UnityEngine.Color pSelf) {
			return new Color(pSelf.r, pSelf.g, pSelf.b, pSelf.a);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static UnityEngine.Vector3 ToUnity(this Vector3 pSelf) {
			return new UnityEngine.Vector3(pSelf.X, pSelf.Y, pSelf.Z);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 ToHover(this UnityEngine.Vector3 pSelf) {
			return new Vector3(pSelf.x, pSelf.y, pSelf.z);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static UnityEngine.Quaternion ToUnity(this Quaternion pSelf) {
			return new UnityEngine.Quaternion(pSelf.X, pSelf.Y, pSelf.Z, pSelf.W);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Quaternion ToHover(this UnityEngine.Quaternion pSelf) {
			return new Quaternion(pSelf.w, pSelf.x, pSelf.y, pSelf.z);
		}

	}

}
