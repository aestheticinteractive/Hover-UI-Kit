using System;
using UnityEngine;

namespace Hover.Common.Styles {

	/*================================================================================================*/
	public struct Color4 {
		
		public float R;
		public float G;
		public float B;
		public float A;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Color4(float pRed, float pGreen, float pBlue, float pAlpha) {
			R = pRed;
			G = pGreen;
			B = pBlue;
			A = pAlpha;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Color4(float pRed, float pGreen, float pBlue) : this(pRed, pGreen, pBlue, 1) {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Color4(float pGray) : this(pGray, pGray, pGray, 1) {
			//do nothing...
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Color4 CopyWithAlpha(float pAlpha) {
			return new Color4(R, G, B, pAlpha);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Color4 CopyWithMultipliedAlpha(float pAlpha) {
			return new Color4(R, G, B, A*pAlpha);
		}

	}

}
