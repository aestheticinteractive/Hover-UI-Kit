using System;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public static class MeshUtil {

		/*[Flags]
		public enum TabDir : uint {
			N  = 0x01,
			NE = 0x02,
			E  = 0x04,
			SE = 0x08,
			S  = 0x10,
			SW = 0x20,
			W  = 0x40,
			NW = 0x80
		}

		private static readonly MeshBuilder TabRectBuilder = new MeshBuilder();
		private static readonly List<Vector3> TabEdgePoints = new List<Vector3>();
		private static readonly List<Vector3> PathSegmentDirs = new List<Vector3>();
		private static readonly List<Vector3> PathSegmentTangents = new List<Vector3>();
		private static readonly List<Vector3> PathVertexTangents = new List<Vector3>();*/


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildQuadMesh(MeshBuilder pMeshBuild, float pSizeX=1, float pSizeY=1) {
			float halfSizeX = pSizeX/2;
			float halfSizeY = pSizeY/2;

			pMeshBuild.Resize(4, 6);
			pMeshBuild.ResetIndices();

			pMeshBuild.AddVertex(new Vector3( halfSizeX,  halfSizeY, 0));
			pMeshBuild.AddVertex(new Vector3( halfSizeX, -halfSizeY, 0));
			pMeshBuild.AddVertex(new Vector3(-halfSizeX, -halfSizeY, 0));
			pMeshBuild.AddVertex(new Vector3(-halfSizeX,  halfSizeY, 0));

			pMeshBuild.AddUv(new Vector2(1, 1));
			pMeshBuild.AddUv(new Vector2(1, 0));
			pMeshBuild.AddUv(new Vector2(0, 0));
			pMeshBuild.AddUv(new Vector2(0, 1));

			pMeshBuild.AddTriangle(0, 1, 2);
			pMeshBuild.AddTriangle(0, 2, 3);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 GetRingPoint(float pRadius, float pAngle) {
			return new Vector3(
				Mathf.Cos(pAngle)*pRadius, 
				Mathf.Sin(pAngle)*pRadius,
				0
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRingMesh(MeshBuilder pMeshBuild, float pInnerRadius, float pOuterRadius,
				float pAngle0, float pAngle1, Vector3 pInnerOffset, Vector3 pOuterOffset, int pSteps) {
			float angleFull = pAngle1-pAngle0;
			float angleInc = angleFull/pSteps;
			float angle = pAngle0;

			pMeshBuild.Resize((pSteps+1)*2, pSteps*6);
			pMeshBuild.ResetIndices();

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				float uvx = i/(float)pSteps;

				pMeshBuild.AddVertex(pInnerOffset+GetRingPoint(pInnerRadius, angle));
				pMeshBuild.AddVertex(pOuterOffset+GetRingPoint(pOuterRadius, angle));

				pMeshBuild.AddUv(new Vector2(uvx, 0));
				pMeshBuild.AddUv(new Vector2(uvx, 1));

				if ( i > 0 ) {
					int vi = pMeshBuild.VertexIndex;
					pMeshBuild.AddTriangle(vi-3, vi-4, vi-2);
					pMeshBuild.AddTriangle(vi-1, vi-3, vi-2);
				}

				angle += angleInc;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRingMesh(MeshBuilder pMeshBuild, float pInnerRadius, float pOuterRadius,
															float pAngle0, float pAngle1, int pSteps) {
			BuildRingMesh(pMeshBuild, pInnerRadius, pOuterRadius, pAngle0, pAngle1,
				Vector3.zero, Vector3.zero, pSteps);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildCircleMesh(MeshBuilder pMeshBuild, float pRadius, int pSteps) {
			const float angleFull = (float)Math.PI*2;
			float angleInc = angleFull/pSteps;
			float angle = 0;

			pMeshBuild.Resize(pSteps+2, pSteps*3);

			pMeshBuild.AddVertex(Vector3.zero);
			pMeshBuild.AddUv(new Vector2(0, 0));

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				pMeshBuild.AddVertex(GetRingPoint(pRadius, angle));
				pMeshBuild.AddUv(new Vector2(i/(float)pSteps, 1));

				if ( i > 0 ) {
					int vi = pMeshBuild.VertexIndex;
					pMeshBuild.AddTriangle(0, vi-2, vi-1);
				}

				angle += angleInc;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public static void BuildBorderMesh(MeshBuilder pMeshBuild, float pWidth, float pHeight, 
																					float pThickness) {
			float innerW = pWidth/2-pThickness;
			float innerH = pHeight/2-pThickness;
			float outerW = pWidth/2;
			float outerH = pHeight/2;
			
			BuildHollowRectangleMesh(pMeshBuild, outerW, outerH, innerW, innerH);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildHollowRectangleMesh(MeshBuilder pMeshBuild,
										float pOuterW, float pOuterH, float pInnerW, float pInnerH) {
			float halfOuterW = pOuterW/2;
			float halfOuterH = pOuterH/2;
			float halfInnerW = pInnerW/2;
			float halfInnerH = pInnerH/2;
			float innerUvMaxX = 0.5f + halfInnerW/halfOuterW/2;
			float innerUvMinX = 0.5f - halfInnerW/halfOuterW/2;
			float innerUvMaxY = 0.5f + halfInnerH/halfOuterH/2;
			float innerUvMinY = 0.5f - halfInnerH/halfOuterH/2;
			
			pMeshBuild.Resize(8, 24);
			pMeshBuild.ResetIndices();
			
			pMeshBuild.AddVertex(new Vector3( halfOuterW,  halfOuterH, 0)); 
			pMeshBuild.AddVertex(new Vector3( halfOuterW, -halfOuterH, 0));
			pMeshBuild.AddVertex(new Vector3(-halfOuterW, -halfOuterH, 0));
			pMeshBuild.AddVertex(new Vector3(-halfOuterW,  halfOuterH, 0));
			pMeshBuild.AddVertex(new Vector3( halfInnerW,  halfInnerH, 0));
			pMeshBuild.AddVertex(new Vector3( halfInnerW, -halfInnerH, 0)); 
			pMeshBuild.AddVertex(new Vector3(-halfInnerW, -halfInnerH, 0));
			pMeshBuild.AddVertex(new Vector3(-halfInnerW,  halfInnerH, 0));
			
			pMeshBuild.AddUv(new Vector2(1, 1));
			pMeshBuild.AddUv(new Vector2(1, 0));
			pMeshBuild.AddUv(new Vector2(0, 0));
			pMeshBuild.AddUv(new Vector2(0, 1));
			pMeshBuild.AddUv(new Vector2(innerUvMaxX, innerUvMaxY));
			pMeshBuild.AddUv(new Vector2(innerUvMaxX, innerUvMinY));
			pMeshBuild.AddUv(new Vector2(innerUvMinX, innerUvMinY));
			pMeshBuild.AddUv(new Vector2(innerUvMinX, innerUvMaxY));
			
			pMeshBuild.AddTriangle(0, 1, 4);
			pMeshBuild.AddTriangle(1, 5, 4);
			pMeshBuild.AddTriangle(1, 2, 5);
			pMeshBuild.AddTriangle(2, 6, 5);
			pMeshBuild.AddTriangle(2, 3, 6);
			pMeshBuild.AddTriangle(3, 7, 6);
			pMeshBuild.AddTriangle(3, 4, 7);
			pMeshBuild.AddTriangle(3, 0, 4);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildHollowRectangleTabMesh(MeshBuilder pMeshBuild, float pOuterW, 
				float pOuterH, float pInnerW, float pInnerH, float pOuterTabPush, float pOuterTabThick, 
				float pInnerRatio, bool pShowTabN, bool pShowTabE, bool pShowTabS, bool pShowTabW) {
			float halfOuterW = pOuterW/2;
			float halfOuterH = pOuterH/2;
			float halfOuterTw = Mathf.Min(pOuterTabThick, pOuterW)/2;
			float halfOuterTh = Mathf.Min(pOuterTabThick, pOuterH)/2;
			float outerToInnerW = pInnerW/pOuterW;
			float outerToInnerH = pInnerH/pOuterH;
			float innerTabPush = pOuterTabPush*pInnerRatio;
			float halfInnerTw = halfOuterTw*pInnerRatio;
			float halfInnerTh = halfOuterTh*pInnerRatio;

			pMeshBuild.Resize(32, 32*3);
			pMeshBuild.ResetIndices();
			
			pMeshBuild.AddVertex(new Vector3(          0,  halfOuterH)); //V0 (N)
			pMeshBuild.AddVertex(new Vector3( halfOuterTw, halfOuterH));
			pMeshBuild.AddVertex(new Vector3( halfOuterW,  halfOuterH)); //V2 (NE)
			pMeshBuild.AddVertex(new Vector3( halfOuterW,  halfOuterTh));
			pMeshBuild.AddVertex(new Vector3( halfOuterW,           0)); //V4 (E)
			pMeshBuild.AddVertex(new Vector3( halfOuterW, -halfOuterTh));
			pMeshBuild.AddVertex(new Vector3( halfOuterW, -halfOuterH)); //V6 (SE)
			pMeshBuild.AddVertex(new Vector3( halfOuterTw,-halfOuterH));
			pMeshBuild.AddVertex(new Vector3(          0, -halfOuterH)); //V8 (S)
			pMeshBuild.AddVertex(new Vector3(-halfOuterTw,-halfOuterH));
			pMeshBuild.AddVertex(new Vector3(-halfOuterW, -halfOuterH)); //V10 (SW)
			pMeshBuild.AddVertex(new Vector3(-halfOuterW, -halfOuterTh));
			pMeshBuild.AddVertex(new Vector3(-halfOuterW,           0)); //V12 (W)
			pMeshBuild.AddVertex(new Vector3(-halfOuterW,  halfOuterTh));
			pMeshBuild.AddVertex(new Vector3(-halfOuterW,  halfOuterH)); //V14 (NW)
			pMeshBuild.AddVertex(new Vector3(-halfOuterTw, halfOuterH));

			for ( int i = 0 ; i < 16 ; i++ ) {
				Vector3 vert = pMeshBuild.Vertices[i];

				pMeshBuild.AddVertex(new Vector3(
					vert.x*outerToInnerW,
					vert.y*outerToInnerH
				));
			}

			if ( pShowTabN ) {
				pMeshBuild.Vertices[ 0].y += pOuterTabPush;
				pMeshBuild.Vertices[16].y += innerTabPush;
				pMeshBuild.Vertices[31].x = -halfInnerTw;
				pMeshBuild.Vertices[17].x =  halfInnerTw;
			}

			if ( pShowTabE ) {
				pMeshBuild.Vertices[ 4].x += pOuterTabPush;
				pMeshBuild.Vertices[20].x += innerTabPush;
				pMeshBuild.Vertices[19].y =  halfInnerTh;
				pMeshBuild.Vertices[21].y = -halfInnerTh;
			}

			if ( pShowTabS ) {
				pMeshBuild.Vertices[ 8].y -= pOuterTabPush;
				pMeshBuild.Vertices[24].y -= innerTabPush;
				pMeshBuild.Vertices[23].x =  halfInnerTw;
				pMeshBuild.Vertices[25].x = -halfInnerTw;
			}

			if ( pShowTabW ) {
				pMeshBuild.Vertices[12].x -= pOuterTabPush;
				pMeshBuild.Vertices[28].x -= innerTabPush;
				pMeshBuild.Vertices[27].y = -halfInnerTh;
				pMeshBuild.Vertices[29].y =  halfInnerTh;
			}

			for ( int i = 0 ; i < 32 ; i++ ) {
				Vector3 vert = pMeshBuild.Vertices[i];

				pMeshBuild.AddUv(new Vector2(
					vert.x/pOuterW + 0.5f,
					vert.y/pOuterH + 0.5f
				));
			}

			for ( int i = 0 ; i < 16 ; i++ ) {
				int i2 = (i+1)%16;
				int i3 = i+16;
				pMeshBuild.AddTriangle(i, i2, i3);
				pMeshBuild.AddTriangle(i2, i2+16, i3);
			}
		}

	}

}
