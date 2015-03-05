using System;
using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display.Default {

	/*================================================================================================*/
	public class UiCursorRenderer : MonoBehaviour, IUiCursorRenderer {

		private CursorState vCursorState;
		private CursorSettings vSettings;
		private Mesh vRingMesh;
		private GameObject vRingObj;

		private float vCurrThickness;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(CursorState pCursorState, CursorSettings pSettings) {
			vCursorState = pCursorState;
			vSettings = pSettings;

			vRingObj = new GameObject("Ring");
			vRingObj.transform.SetParent(gameObject.transform, false);
			vRingObj.AddComponent<MeshRenderer>();
			vRingObj.AddComponent<MeshFilter>();
			vRingObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllumTop"));

			vRingMesh = vRingObj.GetComponent<MeshFilter>().mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ButtonState nearSeg = null; //vPanelState.NearestSegment;
			float highProg = (nearSeg == null ? 0 : nearSeg.MaxHighlightProgress);
			bool high = (highProg >= 1);
			float thick = Mathf.Lerp(vSettings.ThickNorm, vSettings.ThickHigh, highProg);
			float scale = Mathf.Lerp(vSettings.RadiusNorm, vSettings.RadiusHigh, highProg);

			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);
			//col.a *= UiSelectRenderer.GetArcAlpha(vPanelState);

			BuildMesh(thick);
			vRingObj.transform.localScale = Vector3.one*scale;
			vRingObj.renderer.sharedMaterial.color = col;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(float pThickness) {
			if ( pThickness == vCurrThickness ) {
				return;
			}

			vCurrThickness = pThickness;
			MeshUtil.BuildRingMesh(vRingMesh, (1-pThickness)/2f, 0.5f, 0, (float)Math.PI*2, 24);
		}

	}

}
