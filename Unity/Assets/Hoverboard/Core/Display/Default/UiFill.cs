using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hoverboard.Core.Display.Default {

	/*================================================================================================*/
	public class UiFill {

		public const float AngleInset = UiButton.Size*0.01f;
		public const float EdgeThick = AngleInset*2;

		private readonly bool vCalcSelections;
		private float vWidth;
		private float vHeight;

		private readonly GameObject vBackground;
		private readonly GameObject vEdge;
		private readonly GameObject vHighlight;
		private readonly GameObject vSelect;

		private Vector3[] vSelectionPoints;
		private float vPrevHighAmount;
		private float vPrevSelAmount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiFill(GameObject pGameObj, bool pBackgroundOnly=false, string pName="Background") {
			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.name = pName;
			vBackground.transform.SetParent(pGameObj.transform, false);
			vBackground.transform.localRotation = rot;
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			if ( pBackgroundOnly ) {
				return;
			}

			vEdge = new GameObject("Edge");
			vEdge.transform.SetParent(pGameObj.transform, false);
			vEdge.transform.localRotation = rot;
			vEdge.AddComponent<MeshRenderer>();
			vEdge.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vEdge.renderer.sharedMaterial.renderQueue -= 50;
			vEdge.renderer.sharedMaterial.color = Color.clear;
			vEdge.AddComponent<MeshFilter>();

			vHighlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vHighlight.name = "Highlight";
			vHighlight.transform.SetParent(pGameObj.transform, false);
			vHighlight.transform.localRotation = rot;
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vSelect.name = "Select";
			vSelect.transform.SetParent(pGameObj.transform, false);
			vSelect.transform.localRotation = rot;
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 100;
			vSelect.renderer.sharedMaterial.color = Color.clear;

			UpdateSize(UiButton.Size, UiButton.Size);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSize(float pWidth, float pHeight) {
			var w = pWidth-AngleInset*2;
			var h = pHeight-AngleInset*2;

			if ( Math.Abs(w-vWidth) < 0.005f && Math.Abs(h-vHeight) < 0.005f ) {
				return;
			}

			vWidth = w;
			vHeight = h;

			UpdateQuad(vBackground, 1, false);

			if ( vHighlight != null ) {
				UpdateEdgeMesh();
				UpdateQuad(vHighlight, 0, true);
				UpdateQuad(vSelect, 0, true);
			}

			vSelectionPoints = CalcSelectionPoints();
		}

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
				UpdateQuad(vHighlight, pAmount, true);
				vPrevHighAmount = pAmount;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSelect(Color pColor, float pAmount) {
			vSelect.renderer.sharedMaterial.color = pColor;
			vSelect.SetActive(pAmount > 0 && pColor.a > 0);

			if ( Math.Abs(pAmount-vPrevSelAmount) > 0.005f ) {
				UpdateQuad(vSelect, pAmount, true);
				vPrevSelAmount = pAmount;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos) {
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
		private void UpdateQuad(GameObject pObj, float pThickness, bool pEdgeInset) {
			float w = vWidth-(pEdgeInset ? EdgeThick*2 : 0);
			float h = vHeight-(pEdgeInset ? EdgeThick*2 : 0);
			float wt;
			float ht;

			if ( w >= h ) {
				ht = h*pThickness;
				wt = w-(h-ht);
			}
			else {
				wt = w*pThickness;
				ht = h-(w-wt);
			}

			pObj.transform.localScale = new Vector3(wt, ht, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateEdgeMesh() {
			float innerW = vWidth/2-EdgeThick;
			float innerH = vHeight/2-EdgeThick;
			float outerW = vWidth/2;
			float outerH = vHeight/2;

			Mesh mesh = vEdge.GetComponent<MeshFilter>().mesh;

			mesh.vertices = new[] {
				new Vector3( outerW,  outerH, 0), 
				new Vector3( outerW, -outerH, 0), 
				new Vector3(-outerW, -outerH, 0), 
				new Vector3(-outerW,  outerH, 0), 
				new Vector3( innerW,  innerH, 0), 
				new Vector3( innerW, -innerH, 0), 
				new Vector3(-innerW, -innerH, 0), 
				new Vector3(-innerW,  innerH, 0)
			};

			mesh.triangles = new[] {
				0, 1, 4,
				1, 5, 4,
				1, 2, 5,
				2, 6, 5,
				2, 3, 6,
				3, 7, 6,
				3, 4, 7,
				3, 0, 4
			};

			mesh.uv = new Vector2[8];
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.Optimize();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Vector3[] CalcSelectionPoints() {
			var points = new List<Vector3>();
			int stepsX = (int)Math.Round(vWidth/UiButton.Size)*6;
			int stepsY = (int)Math.Round(vHeight/UiButton.Size)*6;
			float x0 = -vWidth/2f;
			float y0 = -vHeight/2f;
			float xInc = vWidth/stepsX;
			float yInc = vHeight/stepsY;

			for ( int xi = 1 ; xi < stepsX ; xi += 2 ) {
				for ( int yi = 1 ; yi < stepsY ; yi += 2 ) {
					points.Add(new Vector3(x0+xInc*xi, 0, y0+yInc*yi));
				}
			}

			return points.ToArray();
		}

	}

}
