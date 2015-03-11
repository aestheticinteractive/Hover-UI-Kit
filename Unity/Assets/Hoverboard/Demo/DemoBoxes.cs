using Hovercast.Demo;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoBoxes : MonoBehaviour {

		private struct BoxData {
			public GameObject BoxObj;
			public DemoAnimFloat Anim;
		}

		private const int Width = 60;
		private const int Height = 40;

		private readonly BoxData[,] vBoxes;
		//private readonly Texture2D[] vTextures;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoBoxes() {
			vBoxes = new BoxData[Width, Height];
			/*vTextures = new Texture2D[101];

			for ( int i = 0 ; i < 101 ; ++i ) {
				var tex = new Texture2D(1, 1, TextureFormat.Alpha8, false);
				tex.SetPixel(0, 0, new Color(0, 0, 0, i/100f));
				tex.Apply();
				vTextures[i] = tex;
			}*/
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var rand = new System.Random();

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					var mainTex = new Texture2D(1, 1);
					mainTex.SetPixel(0, 0, Color.red);
					mainTex.Apply();

					GameObject boxObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					boxObj.transform.SetParent(gameObject.transform, false);
					boxObj.transform.localPosition = new Vector3(xi-Width/2, 0, yi-Width/2)*1.02f;
					//boxObj.renderer.material = Resources.Load<Material>("BoxGlow");
					//boxObj.renderer.material.color = Color.white;
					//boxObj.renderer.material.SetTexture("_MainTex", mainTex);

					var anim = new DemoAnimFloat(600);
					//anim.Start((float)rand.NextDouble(), (float)rand.NextDouble());

					vBoxes[xi, yi] = new BoxData {
						BoxObj = boxObj,
						Anim = anim
					};
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			var rand = new System.Random();

			foreach ( BoxData boxData in vBoxes ) {
				GameObject boxObj = boxData.BoxObj;
				float animVal = boxData.Anim.GetValue();
				//int texIndex = Math.Min(100, (int)Math.Round(animVal*100));

				Vector3 pos = boxObj.transform.localPosition;
				pos.y = animVal;

				boxObj.transform.localPosition = pos;
				//boxObj.renderer.material.SetTexture("_Illum", vTextures[texIndex]);

				/*if ( boxData.Anim.GetProgress() >= 1 ) {
					boxData.Anim.Start(animVal, (float)rand.NextDouble());
				}*/
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetPixels(float[,] pValues, int pWidth, int pHeight) {
			int x = (Width-pWidth)/2 + 1;
			int y = (Height-pHeight)/2;

			for ( int xi = 0 ; xi < Width ; ++xi ) {
				for ( int yi = 0 ; yi < Height ; ++yi ) {
					BoxData boxData = vBoxes[xi, yi];
					float animVal = boxData.Anim.GetValue();

					if ( xi < x || xi >= x+pWidth || yi < y || yi >= y+pHeight ) {
						boxData.Anim.Start(animVal, 0);
					}
					else {
						boxData.Anim.Start(animVal, pValues[xi-x, pHeight-(yi-y)-1]);
					}
				}
			}
		}

	}

}
