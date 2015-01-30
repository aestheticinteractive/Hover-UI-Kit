using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public static class MeshUtil {


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
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}

		/*--------------------------------------------------------------------------------------------* /
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
					tris.Add(vi);
					tris.Add(vi-1);
				}

				angle += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}*/

	}

}
