using System;
using System.Collections.Generic;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiHoverMeshSlice : UiHoverMesh {

		public const float AngleInset = 0.0012f;
		public const float EdgeThick = 0.01f;

		public bool DrawOuterEdge { get; set; }

		protected float vRadInner;
		protected float vRadOuter;
		protected float vAngle0;
		protected float vAngle1;
		protected int vMeshSteps;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshSlice(GameObject pParent, bool pBackgroundOnly=false, string pBgName=null) {
			Build(pParent);

			if ( pBackgroundOnly ) {
				UnityEngine.Object.Destroy(Highlight);
				UnityEngine.Object.Destroy(Select);
				UnityEngine.Object.Destroy(Edge);

				Highlight = null;
				Select = null;
				Edge = null;
			}

			if ( pBgName != null ) {
				Background.name = pBgName;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Show(bool pShow) {
			Background.SetActive(pShow);

			if ( Highlight != null ) {
				Highlight.SetActive(pShow);
				Select.SetActive(pShow);
				Edge.SetActive(pShow);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSize(float pRadiusInner, float pRadiusOuter, float pArcAngle) {
			UpdateSize(pRadiusInner, pRadiusOuter, -pArcAngle/2f, pArcAngle/2f);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSize(float pRadiusInner, float pRadiusOuter, 
																		float pAngle0, float pAngle1) {
			const float ep = 0.0001f;
			float a0 = pAngle0+AngleInset;
			float a1 = Math.Max(a0, pAngle1-AngleInset);

			if ( Math.Abs(pRadiusInner-vRadInner) < ep && Math.Abs(pRadiusOuter-vRadOuter) < ep &&
					Math.Abs(a0-vAngle0) < ep && Math.Abs(a1-vAngle1) < ep ) {
				return;
			}

			vRadInner = pRadiusInner;
			vRadOuter = pRadiusOuter;
			vAngle0 = a0;
			vAngle1 = a1;

			if ( vAngle1 > vAngle0 ) {
				vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));
			}
			else {
				vMeshSteps = 0;
			}

			UpdateAfterResize();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateHoverPoints(IBaseItemPointsState pPointsState) {
			pPointsState.Points = vHoverPoints.ReadOnly;
			pPointsState.RelativeToTransform = Background.transform;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh(MeshType pType, Mesh pMesh, float pAmount=1) {
			if ( pType == MeshType.Edge ) {
				float edgeInner = (DrawOuterEdge ? vRadOuter : vRadInner-EdgeThick);
				float edgeOuter = (DrawOuterEdge ? vRadOuter+EdgeThick : vRadInner);
				MeshUtil.BuildRingMesh(EdgeMesh, edgeInner, edgeOuter, vAngle0, vAngle1, vMeshSteps);
				return;
			}

			float radOuter = vRadInner+(vRadOuter-vRadInner)*pAmount;
			MeshUtil.BuildRingMesh(pMesh, vRadInner, radOuter, vAngle0, vAngle1, vMeshSteps);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateHoverLocalPoints() {
			const int innerSteps = 5;
			Vector3[] bgVerts = BackgroundMesh.vertices;

			vHoverPoints.Clear();

			for ( int i = 3 ; i < bgVerts.Length-2 ; i += 2 ) {
				Vector3 outer = bgVerts[i];
				Vector3 inner = bgVerts[i-1];

				for ( int j = 0 ; j < innerSteps ; ++j ) {
					vHoverPoints.Add(Vector3.Lerp(outer, inner, j/(float)(innerSteps-1)));
				}
			}
		}

	}

}
