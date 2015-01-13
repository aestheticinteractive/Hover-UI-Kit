using System;
using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiCursorRenderer : MonoBehaviour, IUiCursorRenderer {

		private ArcState vArcState;
		private CursorState vCursorState;
		private Mesh vRingMesh;
		private GameObject vRingObj;

		private float vCurrInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ArcState pArcState, CursorState pCursorState) {
			vArcState = pArcState;
			vCursorState = pCursorState;

			vRingObj = new GameObject("Ring");
			vRingObj.transform.SetParent(gameObject.transform, false);
			vRingObj.transform.localScale = Vector3.one*0.012f;
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
			float alpha = (high ? 1 : 0.75f)*UiArcSegmentRenderer.GetArcAlpha(vArcState); 

			vRingObj.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha);
			BuildMesh(high ? 0.35f : 0.45f);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(float pInnerRadius) {
			if ( pInnerRadius == vCurrInnerRadius ) {
				return;
			}

			vCurrInnerRadius = pInnerRadius;
			MeshUtil.BuildRingMesh(vRingMesh, pInnerRadius, 0.5f, 0, (float)Math.PI*2, 24);
		}

	}

}
