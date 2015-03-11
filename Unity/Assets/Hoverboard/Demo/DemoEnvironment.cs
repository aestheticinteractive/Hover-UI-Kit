using Hoverboard.Core.Display.Default;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;

		private System.Random vRandom;
		private Light vLight;
		private DemoTextPixels vPixels;
		private DemoBoxes vBoxes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoEnvironment() {
		}


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
			lightObj.transform.localPosition = new Vector3(0, 1.6f, 1.75f);

			vLight = lightObj.AddComponent<Light>();
			vLight.type = LightType.Point;
			vLight.range = 4;
			vLight.intensity = 1;
			vLight.shadows = LightShadows.Hard;

			////

			var pixObj = new GameObject("PixelLabel");
			pixObj.transform.SetParent(gameObject.transform, false);

			UiLabel pix = pixObj.AddComponent<UiLabel>();
			pixObj.SetActive(false);

			vPixels = new DemoTextPixels(pix);

			//// 


			var boxesObj = new GameObject("Boxes");
			boxesObj.transform.SetParent(gameObject.transform, false);
			boxesObj.transform.localPosition = new Vector3(0, 1.2f, 2.5f);
			boxesObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, new Vector3(0, 1, -1).normalized);
			boxesObj.transform.localScale = Vector3.one*0.1f;

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
