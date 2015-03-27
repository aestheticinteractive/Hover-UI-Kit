using System;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.Custom.Standard;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Display.Standard {

	/*================================================================================================*/
	public class UiCursorRenderer : MonoBehaviour, IUiCursorRenderer {

		private ICursorState vCursorState;
		private CursorSettingsStandard vSettings;
		private Mesh vRingMesh;
		private GameObject vRingObj;

		private float vCurrThickness;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ICursorState pCursorState, ICursorSettings pSettings) {
			vCursorState = pCursorState;
			vSettings = (CursorSettingsStandard)pSettings;

			vRingObj = new GameObject("Ring");
			vRingObj.transform.SetParent(gameObject.transform, false);
			vRingObj.AddComponent<MeshRenderer>();
			vRingObj.AddComponent<MeshFilter>();
			vRingObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllumTop"));

			vRingMesh = vRingObj.GetComponent<MeshFilter>().mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float maxProg = vCursorState.GetMaxHighlightProgress();
			bool high = (maxProg >= 1);
			float thick = Mathf.Lerp(vSettings.ThickNorm, vSettings.ThickHigh, maxProg);
			float scale = Mathf.Lerp(vSettings.RadiusNorm, vSettings.RadiusHigh, maxProg);

			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);
			col.a *= vCursorState.DisplayStrength;

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
