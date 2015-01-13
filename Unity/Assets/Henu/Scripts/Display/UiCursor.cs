using System;
using Henu.Display.Default;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private ArcState vArcState;
		private CursorState vCursorState;
		private Mesh vTestMesh;
		private GameObject vRendererObj;
		private IUiArcSegmentRenderer vRenderer;
		private Transform vCameraTx;

		private float vCurrInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, CursorState pCursorState, Renderers pRenderers) {
			vArcState = pArcState;
			vCursorState = pCursorState;

			BuildRenderer(pRenderers);

			vCameraTx = GameObject.Find("HandController").transform;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vCursorState.Position == null || vArcState.Strength <= 0 ) {
				vRendererObj.SetActive(false);
				return;
			}

			vRendererObj.SetActive(true);

			Transform tx = gameObject.transform;
			tx.localPosition = (Vector3)vCursorState.Position;
			tx.localRotation = Quaternion.identity;

			Vector3 camWorld = vCameraTx.transform.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);

			////

			ArcSegmentState nearSeg = vArcState.NearestSegment;
			bool high = (nearSeg != null && nearSeg.HighlightProgress > 0);
			float alpha = (high ? 1 : 0.75f)*UiArcSegmentRenderer.GetArcAlpha(vArcState); 

			vRendererObj.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha);
			BuildMesh(high ? 0.35f : 0.45f);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildRenderer(Renderers pRenderers) {
			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);
			vRendererObj.transform.localScale = Vector3.one*0.012f;
			vRendererObj.transform.localPosition = new Vector3(0, -0.02f, 0); //keep in front of finger
			vRendererObj.AddComponent<MeshRenderer>();
			vRendererObj.AddComponent<MeshFilter>();
			vRendererObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));

			vTestMesh = vRendererObj.GetComponent<MeshFilter>().mesh;

			/*vRenderer = (IUiMenuPointRenderer)vRendererObj.AddComponent(rendererType);
			vRenderer.Build(vArcState, vSegState, pAngle0, pAngle1);
			vRenderer.Update();*/
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(float pInnerRadius) {
			if ( pInnerRadius == vCurrInnerRadius ) {
				return;
			}

			vCurrInnerRadius = pInnerRadius;
			MeshUtil.BuildRingMesh(vTestMesh, pInnerRadius, 0.5f, 0, (float)Math.PI*2, 24);
		}

	}

}
