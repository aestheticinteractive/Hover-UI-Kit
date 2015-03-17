using System;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Display.Default {

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
			float highProg = 0; //TODO: vCursorState.NearestButtonHighlightProgress;
			bool high = (highProg >= 1);
			float thick = Mathf.Lerp(vSettings.ThickNorm, vSettings.ThickHigh, highProg);
			float scale = Mathf.Lerp(vSettings.RadiusNorm, vSettings.RadiusHigh, highProg);
			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);

			BuildMesh(thick);
			vRingObj.transform.localScale = Vector3.one*scale*vCursorState.Size;
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
