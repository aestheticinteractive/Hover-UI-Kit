using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiCursorRenderer : MonoBehaviour, IUiCursorRenderer {

		private ArcState vArcState;
		private CursorState vCursorState;
		private CursorSettings vSettings;
		private Mesh vRingMesh;
		private GameObject vRingObj;

		private float vCurrThickness;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ArcState pArcState, CursorState pCursorState, CursorSettings pSettings) {
			vArcState = pArcState;
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
			SegmentState nearSeg = vArcState.NearestSegment;
			float highProg = (nearSeg == null ? 0 : nearSeg.HighlightProgress);
			bool high = (highProg >= 1);
			float thick = Mathf.Lerp(vSettings.ThickNorm, vSettings.ThickHigh, highProg);
			float scale = Mathf.Lerp(vSettings.RadiusNorm, vSettings.RadiusHigh, highProg);

			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);
			col.a *= UiSelectRenderer.GetArcAlpha(vArcState);

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
