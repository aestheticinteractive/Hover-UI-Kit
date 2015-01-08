using UnityEngine;

namespace HandMenu.Util {

	/*================================================================================================*/
	public static class MeshUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildArrow(Mesh pMesh) {
			var verts = new[] {
				new Vector3(1, 0, 0),
				new Vector3(-1, 0, -1),
				new Vector3(-1, 0, 1)
			};

			var uvs = new[] {
				Vector2.zero,
				Vector2.zero,
				Vector2.zero
			};

			var tris = new[] {
				0,
				1,
				2
			};

			pMesh.Clear();
			pMesh.vertices = verts;
			pMesh.uv = uvs;
			pMesh.triangles = tris;
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}

	}

}
