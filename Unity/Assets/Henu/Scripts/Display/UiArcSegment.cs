using System;
using System.Collections.Generic;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		public static float ItemChangeMilliseconds = 1000;
		public static float ItemChangeDistance = 0.08f;

		private ArcState vArcState;
		private ArcSegmentState vSegState;
		private GameObject vBg;
		//private Renderers vRenderers;

		/*private GameObject vPrevRendererObj;
		private GameObject vCurrRendererObj;
		private IUiMenuPointRenderer vPrevRenderer;
		private IUiMenuPointRenderer vCurrRenderer;

		private int vRendererCount;
		private DateTime? vChangeTime;
		private int vChangeDir;*/


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ArcState pHand, ArcSegmentState pPoint, float pAngle0, float pAngle1, 
																				Renderers pRenderers) {
			vArcState = pHand;
			vSegState = pPoint;
			//vRenderers = pRenderers;

			vBg = new GameObject("Background");
			vBg.transform.SetParent(gameObject.transform, false);
			vBg.AddComponent<MeshFilter>();
			vBg.AddComponent<MeshRenderer>();
			vBg.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBg.renderer.sharedMaterial.color = Color.clear;

			BuildMesh(vBg.GetComponent<MeshFilter>().mesh, 0.5f, pAngle0, pAngle1-0.003f);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float alpha = 1-(float)Math.Pow(1-vArcState.Strength, 2);
			alpha -= (float)Math.Pow(vArcState.GrabStrength, 2);

			vBg.renderer.sharedMaterial.color = new Color(0, 1, 0, Math.Max(0, alpha));

			/*if ( !vPoint.IsActive ) {
				return;
			}

			Transform tx = gameObject.transform;
			tx.localPosition = vPoint.Position;
			tx.localRotation = vPoint.Rotation;

			if ( !vHand.IsLeft ) {
				tx.localRotation *= Quaternion.FromToRotation(Vector3.left, Vector3.right);
			}*/

			//UpdateItemChangeAnim();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsActive() {
			return false; //(vPoint != null && vPoint.IsActive);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void HandleNavItemChange(int pDirection) {
			DestroyPrevRenderer();
			vPrevRendererObj = vCurrRendererObj;
			vPrevRenderer = vCurrRenderer;

			if ( vSegState.NavItem == null ) {
				vCurrRendererObj = null;
				vCurrRenderer = null;
			}
			else {
				BuildCurrRenderer();
			}

			vChangeTime = DateTime.UtcNow;
			vChangeDir = pDirection;
			UpdateItemChangeAnim();
		}
		
		/*--------------------------------------------------------------------------------------------* /
		private void DestroyPrevRenderer() {
			if ( vPrevRendererObj == null ) {
				return;
			}

			vPrevRendererObj.SetActive(false);
			Destroy(vPrevRendererObj);

			vPrevRendererObj = null;
			vPrevRenderer = null;
		}

		/*--------------------------------------------------------------------------------------------* /
		private void BuildCurrRenderer() {
			vCurrRendererObj = new GameObject("Renderer"+vRendererCount);
			vRendererCount++;

			Type rendererType;

			switch ( vPoint.NavItem.Type ) {
				case NavItem.ItemType.Parent:
					rendererType = vRenderers.PointParent;
					break;

				case NavItem.ItemType.Checkbox:
					rendererType = vRenderers.PointCheckbox;
					break;

				case NavItem.ItemType.Radio:
					rendererType = vRenderers.PointRadio;
					break;

				default:
					rendererType = vRenderers.PointSelection;
					break;
			}

			vCurrRenderer = (IUiMenuPointRenderer)vCurrRendererObj.AddComponent(rendererType);
			vCurrRenderer.Build(vHand, vPoint);
			vCurrRenderer.Update();

			vCurrRendererObj.transform.parent = gameObject.transform;
			vCurrRendererObj.transform.localPosition = Vector3.zero;
			vCurrRendererObj.transform.localRotation = Quaternion.identity;
			vCurrRendererObj.transform.localScale = Vector3.one;
		}

		/*--------------------------------------------------------------------------------------------* /
		private void UpdateItemChangeAnim() {
			if ( vChangeTime == null ) {
				return;
			}

			float ms = (float)(DateTime.UtcNow-(DateTime)vChangeTime).TotalMilliseconds;
			float prog = Math.Min(1, ms/ItemChangeMilliseconds);
			float push = 1-(float)Math.Pow(1-prog, 3);
			float dist = -ItemChangeDistance*vChangeDir;

			if ( vPrevRenderer != null ) {
				vPrevRenderer.HandleChangeAnimation(false, vChangeDir, prog);
				vPrevRendererObj.transform.localScale = Vector3.one*(-dist*push);
			}

			if ( vCurrRenderer != null ) {
				vCurrRenderer.HandleChangeAnimation(true, vChangeDir, prog);
				vCurrRendererObj.transform.localScale = Vector3.one*(dist*(1-push)));
			}

			if ( prog >= 1 ) {
				vChangeTime = null;
				DestroyPrevRenderer();
			}

			vSegState.SetIsAnimating(vChangeTime != null);
		}*/



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildMesh(Mesh pMesh, float pThickness, float pAngle0, float pAngle1) {
			int steps = (int)Math.Round(Math.Max(2, (pAngle1-pAngle0)/Math.PI*60));
			BuildMesh(pMesh, 1, 1+pThickness, pAngle0, pAngle1, steps);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildMesh(Mesh pMesh, float pInnerRadius, float pOuterRadius, 
															float pAngle0, float pAngle1, int pSteps) {
			float angleFull = pAngle1-pAngle0;
			float angleInc = angleFull/pSteps;
			float angle = pAngle0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				float x = (float)Math.Sin(angle);
				float y = (float)Math.Cos(angle);
				int vi = verts.Count;

				verts.Add(new Vector3(x*pInnerRadius, 0, y*pInnerRadius));
				verts.Add(new Vector3(x*pOuterRadius, 0, y*pOuterRadius));

				uvs.Add(new Vector2(0, 0));
				uvs.Add(new Vector2(0, 0));

				if ( i > 0 ) {
					tris.Add(vi-1);
					tris.Add(vi-2);
					tris.Add(vi);

					tris.Add(vi+1);
					tris.Add(vi-1);
					tris.Add(vi);
				}

				angle += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}

	}

}
