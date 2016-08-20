using System;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public static class MeshUtil {


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
		/*--------------------------------------------------------------------------------------------*/
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

	}

}
