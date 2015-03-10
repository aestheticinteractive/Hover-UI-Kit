using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display.Default {

	/*================================================================================================*/
	public class UiProjectionRenderer : MonoBehaviour, IUiCursorRenderer {

		private CursorState vCursorState;
		private CursorSettings vSettings;
		private GameObject vDotObj;
		private GameObject vBarObj;

		private float vCurrThickness;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(CursorState pCursorState, CursorSettings pSettings) {
			vCursorState = pCursorState;
			vSettings = pSettings;

			vDotObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			vDotObj.name = "Dot";
			vDotObj.transform.SetParent(gameObject.transform, false);
			vDotObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllumTop"));
			vDotObj.renderer.sharedMaterial.renderQueue += 100;

			vBarObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vBarObj.name = "Bar";
			vBarObj.transform.SetParent(gameObject.transform, false);
			vBarObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllumTop"));
			vBarObj.renderer.sharedMaterial.renderQueue += 200;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float dist = vCursorState.ProjectedPanelDistance;
			float prog = vCursorState.ProjectedPanelProgress;
			float barThick = 0.01f*vCursorState.Size;

			Color col = vSettings.ColorNorm;
			col.a *= prog;

			vDotObj.renderer.sharedMaterial.color = col;
			vDotObj.transform.localScale = Vector3.one*barThick*5;

			vBarObj.renderer.sharedMaterial.color = col;
			vBarObj.transform.localScale = new Vector3(barThick, dist, barThick);
			vBarObj.transform.localPosition = new Vector3(0, vBarObj.transform.localScale.y/2f, 0);
		}

	}

}
