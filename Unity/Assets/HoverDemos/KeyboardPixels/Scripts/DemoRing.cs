using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	public class DemoRing : MonoBehaviour {

		private static Material RingMat;

		public float Radius { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( RingMat == null ) {
				RingMat = new Material(Shader.Find("Diffuse"));
				RingMat.color = new Color(0.2f, 0.2f, 0.2f);
			}

			MeshRenderer rend = gameObject.AddComponent<MeshRenderer>();
			rend.sharedMaterial = RingMat;

			MeshFilter filt = gameObject.AddComponent<MeshFilter>();
			filt.mesh = GetMesh();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Mesh GetMesh() {
			const int steps = 32;
			const float angleInc = ((float)Math.PI*2)/steps;
			const float halfThick = 0.02f;
			float angle = 0;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			for ( int i = 0 ; i <= steps ; ++i ) {
				int vi = verts.Count;
				float uvx = i/(float)steps;
				float x = (float)Math.Sin(angle);
				float y = (float)Math.Cos(angle);
				float z = 0; //halfThick*2*(i%2 == 0 ? 1 : -1);
				verts.Add(new Vector3(x*Radius,  halfThick+z, y*Radius));
				verts.Add(new Vector3(x*Radius, -halfThick+z, y*Radius));

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

			var mesh = new Mesh();
			mesh.vertices = verts.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.triangles = tris.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

	}

}
