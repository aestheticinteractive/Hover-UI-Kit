using Hover.Core.Utils;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public class DemoAdjustInputSceneCamera : HoverSceneAdjust {

		public string CameraName;
		public string MainSceneCameraName = "MainCamera";
		public bool UseBloom = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (GameObject.Find(CameraName) != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			Camera mainSceneCam = UnityUtil.FindComponentAll<Camera>(MainSceneCameraName);
			mainSceneCam.gameObject.SetActive(false);

			Camera cam = UnityUtil.FindComponentAll<Camera>(CameraName);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;

			if ( UseBloom && cam.GetComponent<BloomOptimized>() == null ) {
				BloomOptimized bloom = cam.gameObject.AddComponent<BloomOptimized>();
				bloom.threshold = 0.35f;
				bloom.intensity = 0.6f;
				bloom.blurSize = 1;
				bloom.blurIterations = 2;
				bloom.blurType = BloomOptimized.BlurType.Standard;
				bloom.fastBloomShader = Shader.Find("Hidden/FastBloom");
			}
		}

	}

}
