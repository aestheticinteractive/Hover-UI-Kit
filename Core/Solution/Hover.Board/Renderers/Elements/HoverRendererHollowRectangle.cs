using UnityEngine;
using Hover.Common.Util;
using Hover.Common.Display;
using System;

namespace Hover.Board.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererHollowRectangle : MonoBehaviour {
	
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float OuterAmount = 1;
		
		[Range(0, 1)]
		public float InnerAmount = 0.5f;
		
		[Range(0, 10)]
		public float Inset = 0;
		
		public Color FillColor = Color.gray;
		
		private MeshBuilder vMeshBuild;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CreateMeshBuilderIfNeeded();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			CreateMeshBuilderIfNeeded();
			
			float width = Math.Max(0, SizeX-Inset);
			float height = Math.Max(0, SizeY-Inset);
			
			MeshUtil.BuildHollowRectangleMesh(vMeshBuild, width, height, InnerAmount, OuterAmount);
			vMeshBuild.Commit();
			vMeshBuild.CommitColors(FillColor);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CreateMeshBuilderIfNeeded() {			
			if ( vMeshBuild != null ) {
				return;
			}
			
			vMeshBuild = new MeshBuilder();
			
			Mesh mesh = vMeshBuild.Mesh;
			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			MeshFilter meshFilt = gameObject.GetComponent<MeshFilter>();
			
			if ( meshRend == null ) {
				meshRend = gameObject.AddComponent<MeshRenderer>();
			}
			
			if ( meshFilt == null ) {
				meshFilt = gameObject.AddComponent<MeshFilter>();
			}
			
			if ( meshRend.sharedMaterial == null ) {
				meshRend.sharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererVertexColorMaterial");
			}
			
			meshFilt.sharedMesh = mesh;
		}
		
	}

}
