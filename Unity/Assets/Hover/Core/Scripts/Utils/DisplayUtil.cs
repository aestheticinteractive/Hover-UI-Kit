using System;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public static class DisplayUtil {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetEasedValue(int pSnaps, float pValue, float pSnappedValue, float pPower) {
			if ( pSnaps < 2 ) {
				return pValue;
			}

			float showVal = pSnappedValue;
			int snaps = pSnaps-1;
			float diff = pValue-showVal;
			int sign = Math.Sign(diff);

			diff = Math.Abs(diff); //between 0 and 1
			diff *= snaps;

			if ( diff < 0.5 ) {
				diff *= 2;
				diff = (float)Math.Pow(diff, pPower);
				diff /= 2f;
			}
			else {
				diff = (diff-0.5f)*2;
				diff = 1-(float)Math.Pow(1-diff, pPower);
				diff = diff/2f+0.5f;
			}

			diff /= snaps;
			return showVal + diff*sign;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Color FadeColor(Color pColor, float pAlpha) {
			Color faded = pColor;
			faded.a *= pAlpha;
			return faded;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//based on: http://stackoverflow.com/questions/1335426
		public static Color HsvToColor(float pHue, float pSat, float pVal) {
			float hue60 = pHue/60f;
			int i = (int)Math.Floor(hue60)%6;
			float f = hue60 - (int)Math.Floor(hue60);

			float v = pVal;
			float p = pVal * (1-pSat);
			float q = pVal * (1-f*pSat);
			float t = pVal * (1-(1-f)*pSat);

			switch ( i ) {
				case 0:
					return new Color(v, t, p);
				case 1:
					return new Color(q, v, p);
				case 2:
					return new Color(p, v, t);
				case 3:
					return new Color(p, q, v);
				case 4:
					return new Color(t, p, v);
				case 5:
					return new Color(v, p, q);
			}

			return Color.black;
		}

	}

}
