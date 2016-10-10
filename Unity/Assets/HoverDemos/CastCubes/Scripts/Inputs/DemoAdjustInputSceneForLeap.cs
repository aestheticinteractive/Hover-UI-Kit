using UnityEngine;

namespace HoverDemos.CastCubes.Inputs {

	/*================================================================================================*/
	public class DemoAdjustInputSceneForLeap : DemoAdjustInputSceneBase {

		private const string LeapCameraName = "CenterEyeAnchor";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (GameObject.Find(LeapCameraName) != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			ConfigureCamera(LeapCameraName);
		}

	}

}
