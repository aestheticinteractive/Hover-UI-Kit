using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public static class MeshUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildQuadMesh(Mesh pMesh) {
			const float size = 0.5f;

			pMesh.vertices = new[] {
				new Vector3( size,  size, 0), 
				new Vector3( size, -size, 0), 
				new Vector3(-size, -size, 0), 
				new Vector3(-size,  size, 0)
			};

			pMesh.uv = new[] {
				new Vector2(1, 1), 
				new Vector2(1, 0), 
				new Vector2(0, 0), 
				new Vector2(0, 1)
			};

			pMesh.triangles = new[] {
				0, 1, 2,
				0, 2, 3
			};

			pMesh.colors32 = new Color32[4];
			pMesh.RecalculateBounds();
			pMesh.RecalculateNormals();
			pMesh.Optimize();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector3 GetRingPoint(float pRadius, float pAngle) {
			float x = (float)Math.Sin(pAngle);
			float y = (float)Math.Cos(pAngle);
			return new Vector3(x*pRadius, 0, y*pRadius);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRingMesh(Mesh pMesh, float pInnerRadius, float pOuterRadius,
															float pAngle0, float pAngle1, int pSteps) {
			float angleFull = pAngle1-pAngle0;
			float angleInc = angleFull/pSteps;
			float angle = pAngle0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				int vi = verts.Count;
				float uvx = i/(float)pSteps;

				verts.Add(GetRingPoint(pInnerRadius, angle));
				verts.Add(GetRingPoint(pOuterRadius, angle));

				uvs.Add(new Vector2(uvx, 0));
				uvs.Add(new Vector2(uvx, 1));

				if ( i > 0 ) {
					tris.Add(vi-1);
					tris.Add(vi-2);
					tris.Add(vi);

					tris.Add(vi+1);
					tris.Add(vi-1);
					tris.Add(vi);
				}

				angle += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.colors32 = new Color32[verts.Count];
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildCircleMesh(Mesh pMesh, float pRadius, int pSteps) {
			const float angleFull = (float)Math.PI*2;
			float angleInc = angleFull/pSteps;
			float angle = 0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			verts.Add(Vector3.zero);
			uvs.Add(new Vector2(0, 0));

			for ( int i = 0 ; i <= pSteps ; ++i ) {
				int vi = verts.Count;
				float uvx = i/(float)pSteps;

				verts.Add(GetRingPoint(pRadius, angle));
				uvs.Add(new Vector2(uvx, 1));

				if ( i > 0 ) {
					tris.Add(0);
					tris.Add(vi-1);
					tris.Add(vi);
				}

				angle += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.colors32 = new Color32[verts.Count];
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildBorderMesh(Mesh pMesh, float pWidth, float pHeight, float pThickness) {
			float innerW = pWidth/2-pThickness;
			float innerH = pHeight/2-pThickness;
			float outerW = pWidth/2;
			float outerH = pHeight/2;

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
			pMesh.colors32 = new Color32[8];
			pMesh.RecalculateBounds();
			pMesh.RecalculateNormals();
			pMesh.Optimize();
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildRectangleMesh(Mesh pMesh, float pWidth, float pHeight, float pAmount) {
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
			pMesh.colors32 = new Color32[4];
			pMesh.RecalculateBounds();
			pMesh.RecalculateNormals();
			pMesh.Optimize();
		}

	}

}
