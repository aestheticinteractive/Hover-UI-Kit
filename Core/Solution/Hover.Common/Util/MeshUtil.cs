using System;
using Hover.Common.Display;
using UnityEngine;

namespace Hover.Common.Util {

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
			float x = (float)Math.Sin(pAngle);
			float y = (float)Math.Cos(pAngle);
			return new Vector3(x*pRadius, 0, y*pRadius);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRingMesh(MeshBuilder pMeshBuild, float pInnerRadius, float pOuterRadius,
															float pAngle0, float pAngle1, int pSteps) {
			float angleFull = pAngle1-pAngle0;
			float angleInc = angleFull/pSteps;
			float angle = pAngle0;

			pMeshBuild.Resize((pSteps+1)*2, pSteps*6);
			pMeshBuild.ResetIndices();

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				float uvx = i/(float)pSteps;

				pMeshBuild.AddVertex(GetRingPoint(pInnerRadius, angle));
				pMeshBuild.AddVertex(GetRingPoint(pOuterRadius, angle));

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

			pMeshBuild.Resize(8, 24);
			pMeshBuild.ResetIndices();

			pMeshBuild.AddVertex(new Vector3( outerW,  outerH, 0)); 
			pMeshBuild.AddVertex(new Vector3( outerW, -outerH, 0));
			pMeshBuild.AddVertex(new Vector3(-outerW, -outerH, 0));
			pMeshBuild.AddVertex(new Vector3(-outerW,  outerH, 0));
			pMeshBuild.AddVertex(new Vector3( innerW,  innerH, 0));
			pMeshBuild.AddVertex(new Vector3( innerW, -innerH, 0)); 
			pMeshBuild.AddVertex(new Vector3(-innerW, -innerH, 0));
			pMeshBuild.AddVertex(new Vector3(-innerW,  innerH, 0));

			pMeshBuild.AddTriangle(0, 1, 4);
			pMeshBuild.AddTriangle(1, 5, 4);
			pMeshBuild.AddTriangle(1, 2, 5);
			pMeshBuild.AddTriangle(2, 6, 5);
			pMeshBuild.AddTriangle(2, 3, 6);
			pMeshBuild.AddTriangle(3, 7, 6);
			pMeshBuild.AddTriangle(3, 4, 7);
			pMeshBuild.AddTriangle(3, 0, 4);

			pMeshBuild.AddRemainingUvs(Vector2.zero);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildHollowRectangleMesh(MeshBuilder pMeshBuild, 
								float pWidth, float pHeight, float pInnerAmount, float pOuterAmount) {
			float outerW;
			float outerH;
			float innerW;
			float innerH;
			
			if ( pWidth >= pHeight ) {
				outerH = pHeight*pOuterAmount;
				innerH = pHeight*pInnerAmount;
				outerW = pWidth-(pHeight-outerH);
				innerW = pWidth-(pHeight-innerH);
			}
			else {
				outerW = pWidth*pOuterAmount;
				innerW = pWidth*pInnerAmount;
				outerH = pHeight-(pWidth-outerW);
				innerH = pHeight-(pWidth-innerW);
			}
			
			float halfOuterW = outerW/2;
			float halfOuterH = outerH/2;
			float halfInnerW = innerW/2;
			float halfInnerH = innerH/2;
			
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
			
			pMeshBuild.AddTriangle(0, 1, 4);
			pMeshBuild.AddTriangle(1, 5, 4);
			pMeshBuild.AddTriangle(1, 2, 5);
			pMeshBuild.AddTriangle(2, 6, 5);
			pMeshBuild.AddTriangle(2, 3, 6);
			pMeshBuild.AddTriangle(3, 7, 6);
			pMeshBuild.AddTriangle(3, 4, 7);
			pMeshBuild.AddTriangle(3, 0, 4);
			
			pMeshBuild.AddRemainingUvs(Vector2.zero); //TODO: set UVs
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRectangleMesh(MeshBuilder pMeshBuild, float pWidth, float pHeight, 
																						float pAmount) {
			float fullW;
			float fullH;

			if ( pWidth >= pHeight ) {
				fullH = pHeight*pAmount;
				fullW = pWidth-(pHeight-fullH);
			}
			else {
				fullW = pWidth*pAmount;
				fullH = pHeight-(pWidth-fullW);
			}

			float halfW = fullW/2f;
			float halfH = fullH/2f;

			pMeshBuild.Resize(4, 6);
			pMeshBuild.ResetIndices();

			pMeshBuild.AddVertex(new Vector3( halfW,  halfH, 0));
			pMeshBuild.AddVertex(new Vector3( halfW, -halfH, 0)); 
			pMeshBuild.AddVertex(new Vector3(-halfW, -halfH, 0));
			pMeshBuild.AddVertex(new Vector3(-halfW,  halfH, 0));

			pMeshBuild.AddTriangle(0, 1, 2);
			pMeshBuild.AddTriangle(0, 2, 3);

			pMeshBuild.AddRemainingUvs(Vector2.zero);
		}

	}

}
