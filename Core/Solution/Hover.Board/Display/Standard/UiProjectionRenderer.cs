using System;
using System.Collections.Generic;
using Hover.Board.Custom;
using Hover.Board.Custom.Standard;
using Hover.Board.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiProjectionRenderer : MonoBehaviour, IUiProjectionRenderer {

		private ProjectionState vProjectionState;
		private ProjectionVisualSettingsStandard vSettings;
		private GameObject vSpotObj;
		private GameObject vLineObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(ProjectionState pProjectionState, IProjectionVisualSettings pSettings) {
			vProjectionState = pProjectionState;
			vSettings = (ProjectionVisualSettingsStandard)pSettings;

			////

			vSpotObj = new GameObject("Spot");
			vSpotObj.transform.SetParent(gameObject.transform, false);
			vSpotObj.transform.localScale = Vector3.zero;

			MeshFilter spotMeshFilt = vSpotObj.AddComponent<MeshFilter>();
			BuildCircleMesh(spotMeshFilt.mesh, 0.5f, 32);

			MeshRenderer spotMeshRend = vSpotObj.AddComponent<MeshRenderer>();
			spotMeshRend.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			spotMeshRend.sharedMaterial.renderQueue += 100;

			////

			vLineObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			vLineObj.name = "Line";
			vLineObj.transform.SetParent(gameObject.transform, false);
			vLineObj.transform.localScale = Vector3.zero;
			vLineObj.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vLineObj.renderer.sharedMaterial.renderQueue += 200;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			float dist = vProjectionState.ProjectedPanelDistance;
			float prog = vProjectionState.ProjectedPanelProgress;
			float lineThick = 0.01f*vProjectionState.Cursor.Size;
			float spotSize = (1-prog)*60 + 5;
			float alphaMult = (float)Math.Pow(prog, 2);
			Color colSpot = vSettings.SpotlightColor;
			Color colLine = vSettings.LineColor;

			Vector3 spotScale = Vector3.one*lineThick*spotSize;
			spotScale.y *= (vProjectionState.ProjectedFromFront ? -1 : 1);

			colSpot.a *= alphaMult;
			colLine.a *= alphaMult;

			vSpotObj.renderer.sharedMaterial.color = colSpot;
			vSpotObj.transform.localScale = spotScale;

			vLineObj.renderer.sharedMaterial.color = colLine;
			vLineObj.transform.localScale = new Vector3(lineThick, dist, lineThick);
			vLineObj.transform.localPosition = new Vector3(0, vLineObj.transform.localScale.y/2f, 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Vector3 GetRingPoint(float pRadius, float pAngle) {
			float x = (float)Math.Sin(pAngle);
			float y = (float)Math.Cos(pAngle);
			return new Vector3(x*pRadius, 0, y*pRadius);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static void BuildCircleMesh(Mesh pMesh, float pRadius, int pSteps) {
			const float angleFull = (float)Math.PI*2;
			float angleInc = angleFull/pSteps;
			float angle = 0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			verts.Add(Vector3.zero);
			uvs.Add(new Vector2(0, 0));

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				int vi = verts.Count;
				float uvx = i/(float)pSteps;

				verts.Add(GetRingPoint(pRadius, angle));
				uvs.Add(new Vector2(uvx, 1));

				if ( i > 0 ) {
					tris.Add(0);
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
