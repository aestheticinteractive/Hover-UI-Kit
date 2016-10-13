using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	public class DemoLetter : MonoBehaviour {

		private static Material BoxMat;

		public class CellData : MonoBehaviour {
			public CellData[] Surrounding;
			public float TargVal;
			public float CurrVal;
			public float Force;
			public float ForceSurround;
			public float Momentum;
			public int Delay;
		}

		private const int Width = 16;
		private const int Height = 16;

		public Vector3 RandomAxis { get; set; }

		private CellData[,] vCells;
		private bool vIsAnimating;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( BoxMat == null ) {
				BoxMat = new Material(Shader.Find("Diffuse"));
				BoxMat.color = new Color(0.1f, 0.9f, 0.2f)*0.5f;
			}

			vCells = new CellData[Width, Height];

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					GameObject boxObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Transform boxTx = boxObj.transform;
					boxTx.SetParent(gameObject.transform, false);
					boxTx.localPosition = new Vector3(xi-Width/2, 0, yi-Width/2)*1.1f;
					boxTx.localScale = Vector3.zero;
					boxObj.GetComponent<Renderer>().sharedMaterial = BoxMat;
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
				boxData.Force = valDiff*0.01f;
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
				boxData.Momentum *= 0.98f;

				GameObject boxObj = boxData.gameObject;
				boxObj.transform.localScale = Vector3.one*boxData.CurrVal;
				boxObj.SetActive(boxData.CurrVal > 0);

				isAnim = true;
			}

			vIsAnimating = isAnim;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetPixels(float[,] pValues, int pWidth, int pHeight) {
			int x = (Width-pWidth)/2;
			int y = (Height-pHeight)/2;

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					CellData cellData = vCells[xi, yi];
					cellData.gameObject.transform.localScale = Vector3.zero;
					cellData.CurrVal = 0;
					cellData.Momentum = 0;

					if ( xi < x || xi >= x+pWidth || yi < y || yi >= y+pHeight ) {
						cellData.TargVal = 0;
						cellData.Delay = 0;
					}
					else {
						float val = pValues[xi-x, pHeight-(yi-y)-1];
						cellData.TargVal = Mathf.Min(1, val*1.1f);
						cellData.Delay = (yi-y)*2 + (xi-x);
					}
				}
			}

			vIsAnimating = true;
		}

	}

}
