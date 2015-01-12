using System;
using System.Collections.Generic;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		private ArcState vArcState;
		private ArcSegmentState vSegState;
		private GameObject vBg;
		private Renderers vRenderers;

		private float vAnimAlpha;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ArcSegmentState pSegState, float pAngle0, float pAngle1, 
																				Renderers pRenderers) {
			vArcState = pArcState;
			vSegState = pSegState;
			vRenderers = pRenderers;

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
			alpha  = Math.Max(0, alpha*vAnimAlpha);

			vBg.renderer.sharedMaterial.color = new Color(0, 1, 0, alpha);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void BuildCurrLevel() {
			vCurrLevelObj = new GameObject("Renderer"+vRendererCount);
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

			vCurrLevel = (IUiMenuPointRenderer)vCurrLevelObj.AddComponent(rendererType);
			vCurrLevel.Build(vHand, vPoint);
			vCurrLevel.Update();

			vCurrLevelObj.transform.parent = gameObject.transform;
			vCurrLevelObj.transform.localPosition = Vector3.zero;
			vCurrLevelObj.transform.localRotation = Quaternion.identity;
			vCurrLevelObj.transform.localScale = Vector3.one;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);

			vSegState.SetIsAnimating(pProgress < 1);
		}


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
