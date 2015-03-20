using System;
using System.Collections.Generic;
using Hover.Common.Display;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiHoverMeshSquare : UiHoverMesh {

		private const float AngleInset = UiItem.Size*0.01f;
		private const float EdgeThick = AngleInset*2;

		private float vWidth;
		private float vHeight;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshSquare(GameObject pParent) {
			Build(pParent);

			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

			Background.transform.localRotation = rot;
			Edge.transform.localRotation = rot;
			Highlight.transform.localRotation = rot;
			Select.transform.localRotation = rot;
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

			UpdateAfterResize();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh(MeshType pType, Mesh pMesh, float pAmount=1) {
			if ( pType == MeshType.Edge ) {
				UpdateEdgeMesh(pMesh);
				return;
			}

			UpdateSquareMesh(pMesh, pAmount, (pType != MeshType.Background));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSquareMesh(Mesh pMesh, float pAmount, bool pUseEdgeInset) {
			float w = vWidth-(pUseEdgeInset ? EdgeThick*2 : 0);
			float h = vHeight-(pUseEdgeInset ? EdgeThick*2 : 0);
			float wt;
			float ht;

			if ( w >= h ) {
				ht = h*pAmount;
				wt = w-(h-ht);
			}
			else {
				wt = w*pAmount;
				ht = h-(w-wt);
			}

			float halfW = wt/2f;
			float halfH = ht/2f;

			pMesh.vertices = new[] {
				new Vector3( halfW,  halfH, 0), 
				new Vector3( halfW, -halfH, 0), 
				new Vector3(-halfW, -halfH, 0), 
				new Vector3(-halfW,  halfH, 0)
			};

			pMesh.triangles = new[] {
				0, 1, 2,
				0, 2, 3
			};

			pMesh.uv = new Vector2[4];
			pMesh.RecalculateBounds();
			pMesh.RecalculateNormals();
			pMesh.Optimize();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateEdgeMesh(Mesh pMesh) {
			float innerW = vWidth/2-EdgeThick;
			float innerH = vHeight/2-EdgeThick;
			float outerW = vWidth/2;
			float outerH = vHeight/2;

			pMesh.vertices = new[] {
				new Vector3( outerW,  outerH, 0), 
				new Vector3( outerW, -outerH, 0), 
				new Vector3(-outerW, -outerH, 0), 
				new Vector3(-outerW,  outerH, 0), 
				new Vector3( innerW,  innerH, 0), 
				new Vector3( innerW, -innerH, 0), 
				new Vector3(-innerW, -innerH, 0), 
				new Vector3(-innerW,  innerH, 0)
			};

			pMesh.triangles = new[] {
				0, 1, 4,
				1, 5, 4,
				1, 2, 5,
				2, 6, 5,
				2, 3, 6,
				3, 7, 6,
				3, 4, 7,
				3, 0, 4
			};

			pMesh.uv = new Vector2[8];
			pMesh.RecalculateBounds();
			pMesh.RecalculateNormals();
			pMesh.Optimize();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3[] CalcSelectionPoints() {
			var points = new List<Vector3>();
			int stepsX = (int)Math.Round(vWidth/UiItem.Size)*6;
			int stepsY = (int)Math.Round(vHeight/UiItem.Size)*6;
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
