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

		private readonly bool vBackgroundOnly;

		private float vRadInner;
		private float vRadOuter;
		private float vAngle0;
		private float vAngle1;
		private int vMeshSteps;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiHoverMeshSlice(GameObject pParent, bool pBackgroundOnly=false, string pBgName=null) {
			vBackgroundOnly = pBackgroundOnly;

			Build(pParent);

			if ( vBackgroundOnly ) {
				UnityEngine.Object.Destroy(Highlight);
				UnityEngine.Object.Destroy(Select);
				UnityEngine.Object.Destroy(Edge);
			}

			if ( pBgName != null ) {
				Background.name = pBgName;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Resize(float pRadiusInner, float pRadiusOuter, float pArcAngle) {
			Resize(pRadiusInner, pRadiusOuter, -pArcAngle/2f+AngleInset, pArcAngle/2f-AngleInset);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Resize(float pRadiusInner, float pRadiusOuter, float pAngle0, float pAngle1) {
			const float ep = 0.0001f;

			if ( Math.Abs(pRadiusInner-vRadInner) < ep && Math.Abs(pRadiusOuter-vRadOuter) < ep &&
					Math.Abs(pAngle0-vAngle0) < ep && Math.Abs(pAngle1-vAngle1) < ep ) {
				return;
			}

			vRadInner = pRadiusInner;
			vRadOuter = pRadiusOuter;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1;

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
			pPointsState.Points = vHoverPoints;
			pPointsState.RelativeToTransform = Background.transform;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh(MeshType pType, Mesh pMesh, float pAmount=1) {
			if ( vBackgroundOnly && pType != MeshType.Background ) {
				return;
			}

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
		protected override Vector3[] CalcHoverLocalPoints() {
			const int innerSteps = 5;

			var points = new List<Vector3>();
			Vector3[] bgVerts = BackgroundMesh.vertices;

			for ( int i = 3 ; i < bgVerts.Length-2 ; i += 2 ) {
				Vector3 outer = bgVerts[i];
				Vector3 inner = bgVerts[i-1];

				for ( int j = 0 ; j < innerSteps ; ++j ) {
					points.Add(Vector3.Lerp(outer, inner, j/(float)(innerSteps-1)));
				}
			}

			return points.ToArray();
		}

	}

}
