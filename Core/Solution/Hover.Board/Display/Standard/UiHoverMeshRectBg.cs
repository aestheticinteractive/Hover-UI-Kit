using System;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiHoverMeshRectBg : UiHoverMesh {

		public float Width { get; private set; }
		public float Height { get; private set; }

		protected float vMeshW;
		protected float vMeshH;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshRectBg(GameObject pParent, string pBgName=null) {
			Build(pParent);

			Background.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);
			
			UnityEngine.Object.Destroy(Highlight);
			UnityEngine.Object.Destroy(Select);
			UnityEngine.Object.Destroy(Edge);

			Highlight = null;
			Select = null;
			Edge = null;

			if ( pBgName != null ) {
				Background.name = pBgName;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Show(bool pShow) {
			Background.SetActive(pShow);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateSize(float pWidth, float pHeight) {
			const float totalInset = UiHoverMeshRect.SizeInset*2;
			var w = pWidth-totalInset;
			var h = pHeight-totalInset;

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
			pPointsState.Points = vHoverPoints.ReadOnly;
			pPointsState.RelativeToTransform = vParent.transform;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh(MeshType pType, MeshBuilder pMeshBuild, float pAmount=1) {
			MeshUtil.BuildRectangleMesh(pMeshBuild, vMeshW, vMeshH, pAmount);
			pMeshBuild.Commit();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateHoverLocalPoints() {
			UiHoverMeshRect.CalcHoverPoints(vMeshW, vMeshH, vHoverPoints);
		}

	}

}
