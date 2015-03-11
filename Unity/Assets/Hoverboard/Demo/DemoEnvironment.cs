using Hoverboard.Core.Display.Default;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;

		private System.Random vRandom;
		private Light vLight;
		private Light vLight2;
		private DemoTextPixels vPixels;
		private DemoBoxes vBoxes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( RandomSeed == 0 ) {
				vRandom = new System.Random();
			}
			else {
				vRandom = new System.Random(RandomSeed);
				UnityEngine.Random.seed = RandomSeed;
			}

			////

			var lightObj = new GameObject("Light");
			lightObj.transform.SetParent(gameObject.transform, false);
			lightObj.transform.localPosition = new Vector3(0, 2, 2);

			vLight = lightObj.AddComponent<Light>();
			vLight.type = LightType.Point;
			vLight.range = 4;
			vLight.intensity = 1;
			vLight.shadows = LightShadows.Hard;

			////

			lightObj = new GameObject("Light2");
			lightObj.transform.SetParent(gameObject.transform, false);
			lightObj.transform.localPosition = new Vector3(2, 2, 0);

			vLight2 = lightObj.AddComponent<Light>();
			vLight2.type = LightType.Point;
			vLight2.range = 7;
			vLight2.intensity = 1;
			vLight2.shadows = LightShadows.Hard;

			////

			var pixObj = new GameObject("PixelLabel");
			pixObj.transform.SetParent(gameObject.transform, false);

			UiLabel pix = pixObj.AddComponent<UiLabel>();
			pixObj.SetActive(false);

			vPixels = new DemoTextPixels(pix);

			//// 


			var boxesObj = new GameObject("Boxes");
			boxesObj.transform.SetParent(gameObject.transform, false);
			boxesObj.transform.localPosition = new Vector3(0, 0, 3.5f);
			boxesObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, new Vector3(0, 1, -1.5f).normalized);
			boxesObj.transform.localScale = Vector3.one*0.2f;

			vBoxes = boxesObj.AddComponent<DemoBoxes>();

			////

			GameObject ovrObj = GameObject.Find("LeapOVRPlayerController");

			if ( ovrObj != null ) {
				OVRPlayerController ovrPlayer = ovrObj.GetComponent<OVRPlayerController>();
				ovrPlayer.SetSkipMouseRotation(true);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			UpdateOculus();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddLetter(char pLetter) {
			int w;
			int h;
			float[,] pixels = vPixels.GetPixels(pLetter, out w, out h);

			vBoxes.SetPixels(pixels, w, h);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveLatestLetter() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void UpdateOculus() {
			if ( OVRManager.capiHmd == null ) {
				return;
			}

			if ( Input.GetKey(KeyCode.R) ) {
				OVRManager.display.RecenterPose();
			}

			if ( !OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				return;
			}

			OVRManager.capiHmd.DismissHSWDisplay();
			OVRManager.display.RecenterPose();
		}

	}

}
