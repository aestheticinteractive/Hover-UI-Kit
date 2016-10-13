using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	public class DemoBoxes : MonoBehaviour {

		public class BoxData : MonoBehaviour {
			public BoxData[] Surrounding;
			public float TargVal;
			public float CurrVal;
			public float Force;
			public float ForceSurround;
			public float Momentum;
			public int Delay;
		}

		private const int Width = 24;
		private const int Height = 24;

		private readonly BoxData[,] vBoxes;
		private readonly Color vBoxColor;
		private readonly Color vCharColor;

		private bool vIsAnimating;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoBoxes() {
			vBoxes = new BoxData[Width, Height];
			vBoxColor = new Color(0.5f, 0.5f, 0.5f);
			//vCharColor = new Color(0.1f, 0.9f, 0.2f);
			vCharColor = new Color(0.1f, 0.5f, 0.9f);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					var mainTex = new Texture2D(1, 1);
					mainTex.SetPixel(0, 0, Color.red);
					mainTex.Apply();

					GameObject boxObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					boxObj.transform.SetParent(gameObject.transform, false);
					boxObj.transform.localPosition = new Vector3(xi-Width/2, 0, yi-Width/2)*1.04f;
					boxObj.transform.localScale = new Vector3(1, 5, 1);
					boxObj.GetComponent<Renderer>().material.color = vBoxColor;

					vBoxes[xi, yi] = boxObj.AddComponent<BoxData>();
				}
			}

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					BoxData boxData = vBoxes[xi, yi];
					var surround = new List<BoxData>();

					if ( xi-1 >= 0 ) {
						surround.Add(vBoxes[xi-1, yi]);
					}

					if ( xi+1 < Width ) {
						surround.Add(vBoxes[xi+1, yi]);
					}

					if ( yi-1 >= 0 ) {
						surround.Add(vBoxes[xi, yi-1]);
					}

					if ( yi+1 < Height ) {
						surround.Add(vBoxes[xi, yi+1]);
					}

					/*for ( int sx = Math.Max(0, xi-1) ; sx < Math.Min(Width, xi+2) ; ++sx ) {
						for ( int sy = Math.Max(0, yi-1) ; sy < Math.Min(Height, yi+2) ; ++sy ) {
							if ( sx == xi && sy == yi ) {
								continue;
							}

							surround.Add(vBoxes[sx, sy]);
						}
					}*/

					boxData.Surrounding = surround.ToArray();
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FixedUpdate() {
			if ( !vIsAnimating ) {
				return;
			}

			bool isAnim = false;
			//float maxMomen = 0;

			foreach ( BoxData boxData in vBoxes ) {
				if ( boxData.Delay > 0 ) {
					--boxData.Delay;
					boxData.Force = 0;
					isAnim = true;
					continue;
				}

				float valDiff = boxData.TargVal-boxData.CurrVal;
				boxData.Force = valDiff*0.15f;
			}

			foreach ( BoxData boxData in vBoxes ) {
				boxData.ForceSurround = 0;

				foreach ( BoxData surroundBoxData in boxData.Surrounding ) {
					boxData.ForceSurround += surroundBoxData.Force;
				}

				boxData.ForceSurround /= boxData.Surrounding.Length;
				boxData.Momentum += boxData.Force+boxData.ForceSurround;

				if ( Math.Abs(boxData.Momentum) < 0.012f ) {
					continue;
				}

				//maxMomen = Math.Max(maxMomen, boxData.Momentum);

				boxData.CurrVal += boxData.Momentum;
				boxData.Momentum *= 0.85f;

				GameObject boxObj = boxData.gameObject;
				Vector3 pos = boxObj.transform.localPosition;
				pos.y = boxData.CurrVal*3;
				boxObj.transform.localPosition = pos;

				var col = vBoxColor;

				if ( boxData.CurrVal > 0 ) {
					col = Color.Lerp(col, vCharColor, Mathf.Clamp(boxData.CurrVal, 0, 1));
				}

				boxObj.GetComponent<Renderer>().material.color = col;
				isAnim = true;
			}

			vIsAnimating = isAnim;
			//Debug.Log(maxMomen);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetPixels(float[,] pValues, int pWidth, int pHeight) {
			int x = (Width-pWidth)/2 + 1;
			int y = (Height-pHeight)/2;

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					BoxData boxData = vBoxes[xi, yi];

					if ( xi < x || xi >= x+pWidth || yi < y || yi >= y+pHeight ) {
						boxData.TargVal = 0;
						boxData.Delay = 0;
					}
					else {
						float val = pValues[xi-x, pHeight-(yi-y)-1];
						boxData.TargVal = Math.Min(1, val*1.1f);
						boxData.Delay = yi-y;
					}
				}
			}

			vIsAnimating = true;
		}

	}

}
