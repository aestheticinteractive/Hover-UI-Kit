using System;
using UnityEngine;
using UnityEngine.UI;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	public class DemoTextPixels {

		private readonly Text vText;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoTextPixels(Text pText) {
			vText = pText;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float[,] GetPixels(char pLetter, out int pWidth, out int pHeight) {
			Texture2D tex = (Texture2D)vText.mainTexture;
			CharacterInfo charInfo;

			if ( !vText.font.GetCharacterInfo(pLetter, out charInfo) ) {
				throw new Exception("Character pixels not found: "+pLetter);
			}

			int x0 = Mathf.RoundToInt(charInfo.uvTopLeft.x*tex.width);
			int y0 = Mathf.RoundToInt(charInfo.uvTopLeft.y*tex.height);
			int x1 = Mathf.RoundToInt(charInfo.uvBottomRight.x*tex.width);
			int y1 = Mathf.RoundToInt(charInfo.uvBottomRight.y*tex.height);
			int texW = x1-x0;
			int texH = y1-y0;
			bool xPos = (texW > 0);
			bool yPos = (texH > 0);

			texW = Mathf.Abs(texW);
			texH = Mathf.Abs(texH);
			bool swap = (texW == charInfo.glyphHeight && texH == charInfo.glyphWidth);

			pWidth = (swap ? texH : texW);
			pHeight = (swap ? texW : texH);

			//Debug.Log("WH: "+x0+"/"+x1+" ... "+y0+"/"+y1+" ... "+xPos+"/"+yPos+" ... "+
			//	pWidth+"/"+pHeight+" ... "+charInfo.glyphWidth+"/"+charInfo.glyphHeight+" ... "+swap);

			var pixels = new float[pWidth, pHeight];

			for ( int yi = 0 ; yi < texH ; yi++ ) {
				for ( int xi = 0 ; xi < texW ; xi++ ) {
					int xt = (xPos ? x0+xi : x0-xi);
					int yt = (yPos ? y0+yi : y0-yi);
					float a = tex.GetPixel(xt, yt).a;

					pixels[(swap ? yi : xi), (swap ? xi : yi)] = a;
				}
			}

			return pixels;
		}

	}

}
