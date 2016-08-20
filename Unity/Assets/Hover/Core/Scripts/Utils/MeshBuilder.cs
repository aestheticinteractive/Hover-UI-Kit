using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public class MeshBuilder {

		public Mesh Mesh { get; private set; }

		public Vector3[] Vertices { get; private set; }
		public Vector2[] Uvs { get; private set; }
		public Color32[] Colors { get; private set; }
		public int[] Triangles { get; private set; }

		public int VertexIndex { get; private set; }
		public int UvIndex { get; private set; }
		public int TriangleIndex { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MeshBuilder(Mesh pMesh=null) {
			Mesh = (pMesh ?? new Mesh());
			Mesh.MarkDynamic();

			if ( pMesh != null ) {
				Vertices = pMesh.vertices;
				Uvs = pMesh.uv;
				Colors = pMesh.colors32;
				Triangles = pMesh.triangles;

				if ( Colors.Length != Vertices.Length ) {
					Colors = new Color32[Vertices.Length];
					CommitColors(Color.white);
				}
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Resize(int pVertexCount, int pTriangleCount) {
			if ( Vertices == null || pVertexCount != Vertices.Length ) {
				Vertices = new Vector3[pVertexCount];
				Uvs = new Vector2[pVertexCount];
				Colors = new Color32[pVertexCount];
				Mesh.Clear();
			}

			if ( Triangles == null || pTriangleCount != Triangles.Length ) {
				Triangles = new int[pTriangleCount];
				Mesh.Clear();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ResetIndices() {
			VertexIndex = 0;
			UvIndex = 0;
			TriangleIndex = 0;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddVertex(Vector3 pVertex) {
			Vertices[VertexIndex++] = pVertex;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddUv(Vector2 pUv) {
			Uvs[UvIndex++] = pUv;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddRemainingUvs(Vector2 pUv) {
			while ( UvIndex < Uvs.Length ) {
				Uvs[UvIndex++] = pUv;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddTriangle(int pA, int pB, int pC) {
			Triangles[TriangleIndex++] = pA;
			Triangles[TriangleIndex++] = pB;
			Triangles[TriangleIndex++] = pC;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Commit(bool pRecalcNormals=false, bool pOptimize=false) {
			Mesh.vertices = Vertices;
			Mesh.uv = Uvs;
			Mesh.triangles = Triangles;

			Mesh.RecalculateBounds();

			if ( pRecalcNormals ) {
				Mesh.RecalculateNormals();
			}

			if ( pOptimize ) {
				Mesh.Optimize();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void CommitColors() {
			Mesh.colors32 = Colors;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void CommitColors(Color32 pColor) {
			for ( int i = 0 ; i < Colors.Length ; i++ ) {
				Colors[i] = pColor;
			}

			Mesh.colors32 = Colors;
		}

	}

}
