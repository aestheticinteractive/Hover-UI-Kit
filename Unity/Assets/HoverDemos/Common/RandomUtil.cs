using System;
using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public static class RandomUtil {

		public static System.Random Rand;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void Init(int pSeed) {
			if ( pSeed == 0 ) {
				Rand = new System.Random();
			}
			else {
				Rand = new System.Random(pSeed);
				UnityEngine.Random.InitState(pSeed);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 UnitVector() {
			var v = new Vector3(
				Float(-1, 1),
				Float(-1, 1),
				Float(-1, 1)
			);

			return v.normalized;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Color UnitColor(float pMin, float pMax) {
			int major = Rand.Next()%3;
			int minor = (major+(Rand.Next()%2)+1)%3;

			Func<int, float> getVal = (i => {
				if ( i == major ) {
					return pMax;
				}

				if ( i == minor ) {
					return Float(pMin, pMax);
				}

				return Float(0, pMin);
			});

			return new Color(getVal(0), getVal(1), getVal(2));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 UnitVector(float pMinDimension) {
			var v = UnitVector();
			v.x = Math.Max(v.x, pMinDimension);
			v.y = Math.Max(v.y, pMinDimension);
			v.z = Math.Max(v.z, pMinDimension);
			return v.normalized;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static float Float(float pMax) {
			return Float(0, pMax);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static float Float(float pMin, float pMax) {
			return (float)Rand.NextDouble()*(pMax-pMin) + pMin;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static float Float(float pMin, float pMax, float pPow) {
			return (float)Math.Pow(Float(pMin, pMax), pPow);
		}

	}

}
