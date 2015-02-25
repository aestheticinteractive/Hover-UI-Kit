using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiSlice {

		public const float AngleInset = 0.0012f;

		private readonly bool vCalcSelections;
		private readonly float vRadiusInner;
		private readonly float vRadiusOuter;

		private readonly GameObject vBackground;
		private readonly GameObject vEdge;
		private readonly GameObject vHighlight;
		private readonly GameObject vSelect;

		private float vAngle0;
		private float vAngle1;
		private int vMeshSteps;
		
		private Vector3[] vSelectionPoints;
		private float vPrevHighAmount;
		private float vPrevSelAmount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiSlice(GameObject pGameObj, bool pBackgroundOnly=false, string pName="Background",
														float pRadiusInner=1, float pRadiusOuter=1.5f) {
			vRadiusInner = pRadiusInner;
			vRadiusOuter = pRadiusOuter;

			vBackground = new GameObject(pName);
			vBackground.transform.SetParent(pGameObj.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			if ( pBackgroundOnly ) {
				return;
			}

			vEdge = new GameObject("Edge");
			vEdge.transform.SetParent(pGameObj.transform, false);
			vEdge.AddComponent<MeshFilter>();
			vEdge.AddComponent<MeshRenderer>();
			vEdge.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vEdge.renderer.sharedMaterial.renderQueue -= 300;
			vEdge.renderer.sharedMaterial.color = Color.clear;

			vHighlight = new GameObject("Highlight");
			vHighlight.transform.SetParent(pGameObj.transform, false);
			vHighlight.AddComponent<MeshFilter>();
			vHighlight.AddComponent<MeshRenderer>();
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = new GameObject("Select");
			vSelect.transform.SetParent(pGameObj.transform, false);
			vSelect.AddComponent<MeshFilter>();
			vSelect.AddComponent<MeshRenderer>();
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 10;
			vSelect.renderer.sharedMaterial.color = Color.clear;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Resize(float pArcAngle) {
			Resize(-pArcAngle/2f+AngleInset, pArcAngle/2f-AngleInset);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Resize(float pAngle0, float pAngle1) {
			if ( Math.Abs(pAngle0-vAngle0) < 0.0001f && Math.Abs(pAngle1-vAngle1) < 0.0001f ) {
				return;
			}

			vAngle0 = pAngle0;
			vAngle1 = pAngle1;

			if ( vAngle1 > vAngle0 ) {
				vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));
			}
			else {
				vMeshSteps = 0;
			}

			BuildMesh(vBackground.GetComponent<MeshFilter>().mesh, 1);

			if ( vHighlight != null ) {
				BuildMesh(vHighlight.GetComponent<MeshFilter>().mesh, vPrevHighAmount);
				BuildMesh(vSelect.GetComponent<MeshFilter>().mesh, vPrevSelAmount);

				MeshUtil.BuildRingMesh(vEdge.GetComponent<MeshFilter>().mesh,
					vRadiusInner-0.01f, vRadiusInner, vAngle0, vAngle1, vMeshSteps);
			}

			vSelectionPoints = null;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateBackground(Color pColor) {
			vBackground.renderer.sharedMaterial.color = pColor;
			vBackground.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateEdge(Color pColor) {
			vEdge.renderer.sharedMaterial.color = pColor;
			vEdge.SetActive(pColor.a > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHighlight(Color pColor, float pAmount) {
			vHighlight.renderer.sharedMaterial.color = pColor;
			vHighlight.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevHighAmount) > 0.005f ) {
				BuildMesh(vHighlight.GetComponent<MeshFilter>().mesh, pAmount);
				vPrevHighAmount = pAmount;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSelect(Color pColor, float pAmount) {
			vSelect.renderer.sharedMaterial.color = pColor;
			vSelect.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevSelAmount) > 0.005f ) {
				BuildMesh(vSelect.GetComponent<MeshFilter>().mesh, pAmount);
				vPrevSelAmount = pAmount;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
			if ( vSelectionPoints == null ) {
				vSelectionPoints = CalcSelectionPoints();
			}

			//TODO: Optimize this somehow, probably by reducing the number of points/distances to 
			//check. This could be done by sampling some key points, finding the closest one, then only
			//doing further checks for the points nearest to it.

			float sqrMagMin = float.MaxValue;
			Vector3 nearest = Vector3.zero;
			//Transform tx = vBackground.transform.parent;
			//Vector3 worldCurs = tx.TransformPoint(pCursorLocalPos);
			//Vector3 worldPos;

			foreach ( Vector3 pos in vSelectionPoints ) {
				float sqrMag = (pos-pCursorLocalPos).sqrMagnitude;

				if ( sqrMag < sqrMagMin ) {
					sqrMagMin = sqrMag;
					nearest = pos;
				}

				//worldPos = tx.TransformPoint(pos);
				//Debug.DrawLine(worldPos, worldCurs, Color.yellow);
			}

			//worldPos = tx.TransformPoint(nearest);
			//Debug.DrawLine(worldPos, worldCurs, Color.red);
			return nearest;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh, float pThickness) {
			float radOuter = vRadiusInner+(vRadiusOuter-vRadiusInner)*pThickness;
			MeshUtil.BuildRingMesh(pMesh, vRadiusInner, radOuter, vAngle0, vAngle1, vMeshSteps);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Vector3[] CalcSelectionPoints() {
			var points = new List<Vector3>();

			const int innerSteps = 5;
			Mesh bgMesh = vBackground.GetComponent<MeshFilter>().mesh;

			for ( int i = 3 ; i < bgMesh.vertices.Length-2 ; i += 2 ) {
				Vector3 outer = bgMesh.vertices[i];
				Vector3 inner = bgMesh.vertices[i-1];

				for ( int j = 0 ; j < innerSteps ; ++j ) {
					points.Add(Vector3.Lerp(outer, inner, j/(float)(innerSteps-1)));
				}
			}

			return points.ToArray();
		}

	}

}
