using System;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

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
			vRingObj.transform.localPosition = new Vector3(0, -0.02f, 0); //keep in front of finger
			vRingObj.AddComponent<MeshRenderer>();
			vRingObj.AddComponent<MeshFilter>();
			vRingObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));

			vRingMesh = vRingObj.GetComponent<MeshFilter>().mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ArcSegmentState nearSeg = vArcState.NearestSegment;
			bool high = (nearSeg != null && nearSeg.HighlightProgress > 0);
			float alpha = UiArcSegmentRenderer.GetArcAlpha(vArcState);

			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);
			col.a *= alpha;

			vRingObj.transform.localScale = 
				Vector3.one*(high ? vSettings.RadiusHigh : vSettings.RadiusNorm);

			vRingObj.renderer.sharedMaterial.color = col;
			BuildMesh(high ? vSettings.ThickHigh : vSettings.ThickNorm);
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
