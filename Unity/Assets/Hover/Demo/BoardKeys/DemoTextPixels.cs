using System;
using Hover.Common.Display;
using UnityEngine;

namespace Hover.Demo.BoardKeys {

	/*================================================================================================*/
	public class DemoTextPixels {

		private readonly UiLabel vUiLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoTextPixels(UiLabel pLabel) {
			vUiLabel = pLabel;
			vUiLabel.SetSize(1, 1, 1, 1);
			vUiLabel.FontName = "TahomaPixel16";
			vUiLabel.Alpha = 1;
			vUiLabel.Color = Color.red;
			vUiLabel.FontSize = 40;
			vUiLabel.Label = "Test";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float[,] GetPixels(char pLetter, out int pWidth, out int pHeight) {
			Texture2D tex = vUiLabel.Texture;
			CharacterInfo charInfo;

			if ( !vUiLabel.FontObject.GetCharacterInfo(pLetter, out charInfo) ) {
				throw new Exception("Character pixels not found: "+pLetter);
			}

			int x0 = (int)Math.Round(charInfo.uvTopLeft.x*tex.width);
			int y0 = (int)Math.Round(charInfo.uvTopLeft.y*tex.height);
			int x1 = (int)Math.Round(charInfo.uvBottomRight.x*tex.width);
			int y1 = (int)Math.Round(charInfo.uvBottomRight.y*tex.height);
			int texW = x1-x0;
			int texH = y1-y0;
			bool xPos = (texW > 0);
			bool yPos = (texH > 0);

			texW = Math.Abs(texW);
			texH = Math.Abs(texH);
			bool swap = (texW == charInfo.glyphHeight && texH == charInfo.glyphWidth);

			pWidth = (swap ? texH : texW);
			pHeight = (swap ? texW : texH);

			Debug.Log("WH: "+x0+"/"+x1+" ... "+y0+"/"+y1+" ... "+xPos+"/"+yPos+" ... "+
				pWidth+"/"+pHeight+" ... "+charInfo.glyphWidth+"/"+charInfo.glyphHeight+" ... "+swap);

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
