using System;
using System.Collections.Generic;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiHoverMeshRect : UiHoverMesh {

		public const float SizeInset = UiItem.Size*0.01f;
		public const float EdgeThick = SizeInset*2;

		public float Width { get; private set; }
		public float Height { get; private set; }

		private float vMeshW;
		private float vMeshH;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshRect(GameObject pParent, string pBgName=null) {
			Build(pParent);

			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.up);

			Background.transform.localRotation = rot;
			Edge.transform.localRotation = rot;
			Highlight.transform.localRotation = rot;
			Select.transform.localRotation = rot;

			if ( pBgName != null ) {
				Background.name = pBgName;
			}

			vMeshW = -1;
			vMeshH = -1;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSize(float pWidth, float pHeight) {
			float w = Math.Max(0, pWidth-SizeInset*2);
			float h = Math.Max(0, pHeight-SizeInset*2);

			if ( Math.Abs(w-vMeshW) < 0.005f && Math.Abs(h-vMeshH) < 0.005f ) {
				return;
			}

			Width = pWidth;
			Height = pHeight;

			vMeshW = w;
			vMeshH = h;

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
				MeshUtil.BuildBorderMesh(pMesh, vMeshW, vMeshH, EdgeThick);
				return;
			}

			float inset = (pType != MeshType.Background ? EdgeThick*2 : 0);

			MeshUtil.BuildRectangleMesh(pMesh, Math.Max(0, vMeshW-inset), 
				Math.Max(0, vMeshH-inset), pAmount);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Vector3[] CalcHoverLocalPoints() {
			return CalcHoverPoints(vMeshW, vMeshH);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Vector3[] CalcHoverPoints(float pWidth, float pHeight) {
			var points = new List<Vector3>();
			int stepsX = (int)Math.Round(pWidth/UiItem.Size)*6;
			int stepsY = (int)Math.Round(pHeight/UiItem.Size)*6;
			float x0 = -pWidth/2f;
			float y0 = -pHeight/2f;
			float xInc = pWidth/stepsX;
			float yInc = pHeight/stepsY;

			for ( int xi = 1 ; xi < stepsX ; xi += 2 ) {
				for ( int yi = 1 ; yi < stepsY ; yi += 2 ) {
					points.Add(new Vector3(x0+xInc*xi, 0, y0+yInc*yi)); //relative to parent
				}
			}

			return points.ToArray();
		}

	}

}
