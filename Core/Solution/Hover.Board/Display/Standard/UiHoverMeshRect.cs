using System;
using System.Collections.Generic;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiHoverMeshRect : UiHoverMesh {

		private const float AngleInset = UiItem.Size*0.01f;
		private const float EdgeThick = AngleInset*2;

		private float vWidth;
		private float vHeight;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshRect(GameObject pParent) {
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

		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateHoverPoints(IBaseItemPointsState pPointsState) {
			pPointsState.Points = vHoverPoints;
			pPointsState.RelativeToTransform = vParent.transform;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh(MeshType pType, Mesh pMesh, float pAmount=1) {
			if ( pType == MeshType.Edge ) {
				MeshUtil.BuildBorderMesh(pMesh, vWidth, vHeight, EdgeThick);
				return;
			}

			float inset = (pType != MeshType.Background ? EdgeThick*2 : 0);
			MeshUtil.BuildRectangleMesh(pMesh, vWidth-inset, vHeight-inset, pAmount);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3[] CalcHoverLocalPoints() {
			var points = new List<Vector3>();
			int stepsX = (int)Math.Round(vWidth/UiItem.Size)*6;
			int stepsY = (int)Math.Round(vHeight/UiItem.Size)*6;
			float x0 = -vWidth/2f;
			float y0 = -vHeight/2f;
			float xInc = vWidth/stepsX;
			float yInc = vHeight/stepsY;

			for ( int xi = 1 ; xi < stepsX ; xi += 2 ) {
				for ( int yi = 1 ; yi < stepsY ; yi += 2 ) {
					points.Add(new Vector3(x0+xInc*xi, 0, y0+yInc*yi)); //relative to parent
				}
			}

			return points.ToArray();
		}

	}

}
