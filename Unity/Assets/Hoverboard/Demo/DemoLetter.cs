using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoLetter : MonoBehaviour {

		public class CellData : MonoBehaviour {
			public CellData[] Surrounding;
			public float TargVal;
			public float CurrVal;
			public float Force;
			public float ForceSurround;
			public float Momentum;
			public int Delay;
		}

		private const int Width = 24;
		private const int Height = 24;

		public Vector3 RandomAxis { get; private set; }

		private readonly CellData[,] vCells;
		private readonly Color vBoxColor;
		//private readonly Color vMomenColor;

		private bool vIsAnimating;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoLetter() {
			vCells = new CellData[Width, Height];
			//vBoxColor = new Color(0.1f, 0.5f, 0.9f);
			vBoxColor = new Color(0.1f, 0.9f, 0.2f);

			var rand = new Random();
			var axis = new Vector3(rand.Next(101)-50, rand.Next(101)-50, rand.Next(101)-50);

			RandomAxis = axis.normalized;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Init() {
			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					var mainTex = new Texture2D(1, 1);
					mainTex.SetPixel(0, 0, Color.red);
					mainTex.Apply();

					GameObject boxObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					boxObj.transform.SetParent(gameObject.transform, false);
					boxObj.transform.localPosition = new Vector3(xi-Width/2, 0, yi-Width/2)*1.04f;
					boxObj.renderer.material.color = vBoxColor;
					boxObj.transform.localScale = Vector3.zero;
					boxObj.SetActive(false);

					vCells[xi, yi] = boxObj.AddComponent<CellData>();
				}
			}

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					CellData cellData = vCells[xi, yi];
					var surround = new List<CellData>();

					if ( xi-1 >= 0 ) {
						surround.Add(vCells[xi-1, yi]);
					}

					if ( xi+1 < Width ) {
						surround.Add(vCells[xi+1, yi]);
					}

					if ( yi-1 >= 0 ) {
						surround.Add(vCells[xi, yi-1]);
					}

					if ( yi+1 < Height ) {
						surround.Add(vCells[xi, yi+1]);
					}

					cellData.Surrounding = surround.ToArray();
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FixedUpdate() {
			if ( !vIsAnimating ) {
				return;
			}

			bool isAnim = false;

			foreach ( CellData boxData in vCells ) {
				if ( boxData.Delay > 0 ) {
					--boxData.Delay;
					boxData.Force = 0;
					isAnim = true;
					continue;
				}

				float valDiff = boxData.TargVal-boxData.CurrVal;
				boxData.Force = valDiff*0.25f;
			}

			foreach ( CellData boxData in vCells ) {
				boxData.ForceSurround = 0;

				foreach ( CellData surroundBoxData in boxData.Surrounding ) {
					boxData.ForceSurround += surroundBoxData.Force;
				}

				boxData.ForceSurround /= boxData.Surrounding.Length;
				boxData.Momentum += boxData.Force + boxData.ForceSurround/2;

				if ( Math.Abs(boxData.Momentum) < 0.012f ) {
					continue;
				}

				boxData.CurrVal += boxData.Momentum;
				boxData.Momentum *= 0.9f;

				GameObject boxObj = boxData.gameObject;
				boxObj.transform.localScale = Vector3.one*boxData.CurrVal;
				boxObj.SetActive(boxData.CurrVal > 0);

				//Color baseColor = Color.Lerp(vBoxColor, Color.black, (1-boxData.TargVal)*0.5f);
				boxObj.renderer.material.color = vBoxColor;
					//Color.Lerp(baseColor, Color.black, Math.Abs(boxData.Momentum*4)*0.5f);

				isAnim = true;
			}

			vIsAnimating = isAnim;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetPixels(float[,] pValues, int pWidth, int pHeight) {
			int x = (Width-pWidth)/2 + 1;
			int y = (Height-pHeight)/2;

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					CellData cellData = vCells[xi, yi];
					cellData.gameObject.transform.localScale = Vector3.zero;

					if ( xi < x || xi >= x+pWidth || yi < y || yi >= y+pHeight ) {
						cellData.TargVal = 0;
						cellData.Delay = 0;
					}
					else {
						cellData.TargVal = pValues[xi-x, pHeight-(yi-y)-1];
						cellData.Delay = (yi-y)*2 + (xi-x);
					}
				}
			}

			vIsAnimating = true;
		}

	}

}
