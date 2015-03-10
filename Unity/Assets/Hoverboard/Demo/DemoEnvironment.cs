using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;

		private System.Random vRandom;
		private Light vLight;


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

			vLight = GameObject.Find("Light").GetComponent<Light>();

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
