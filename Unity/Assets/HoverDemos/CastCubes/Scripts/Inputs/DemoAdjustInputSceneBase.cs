using Hover.Core.Utils;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace HoverDemos.CastCubes.Inputs {

	/*================================================================================================*/
	public abstract class DemoAdjustInputSceneBase : HoverSceneAdjust {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void ConfigureCamera(string pCameraName) {
			Camera cam = UnityUtil.FindComponentAll<Camera>(pCameraName);
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;

			if ( cam.GetComponent<BloomOptimized>() == null ) {
				BloomOptimized bloom = cam.gameObject.AddComponent<BloomOptimized>();
				bloom.threshold = 0.35f;
				bloom.intensity = 1;
				bloom.blurSize = 1;
				bloom.blurIterations = 2;
				bloom.blurType = BloomOptimized.BlurType.Standard;
				bloom.fastBloomShader = Shader.Find("Hidden/FastBloom");
			}
		}

	}

}
