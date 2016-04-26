using System;
using Hover.Common.Display;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class HoverRendererHollowRectangle : MonoBehaviour {
	
		public bool ControlledByRenderer { get; set; }

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
		private float vPrevWidth;
		private float vPrevHeight;
		private float vPrevInner;
		private float vPrevOuter;
		private Color vPrevColor;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CreateMeshBuilderIfNeeded();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			CreateMeshBuilderIfNeeded();

			if ( ControlledByRenderer ) {
				return;
			}

			UpdateAfterRenderer();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterRenderer() {
			UpdateMesh();
			UpdateColor();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void CreateMeshBuilderIfNeeded() {			
			if ( vMeshBuild != null ) {
				return;
			}
			
			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			MeshFilter meshFilt = gameObject.GetComponent<MeshFilter>();
			
			if ( meshRend.sharedMaterial == null ) {
				meshRend.sharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererVertexColorMaterial");
			}

			meshFilt.sharedMesh = new Mesh();
			
			vMeshBuild = new MeshBuilder(meshFilt.sharedMesh);
			vMeshBuild.Mesh.name = gameObject.name+"Mesh";

			vPrevWidth = -1;
			vPrevColor = new Color(0, 0, 0, -1);

			UpdateAfterRenderer();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh() {
			float width = Math.Max(0, SizeX-Inset);
			float height = Math.Max(0, SizeY-Inset);

			bool hasSizeOrAmountChanged = (
				width != vPrevWidth || 
				height != vPrevHeight || 
				InnerAmount != vPrevInner || 
				OuterAmount != vPrevOuter
			);

			if ( !hasSizeOrAmountChanged ) {
				return;
			}

			MeshUtil.BuildHollowRectangleMesh(vMeshBuild, width, height, InnerAmount, OuterAmount);
			vMeshBuild.Commit();

			vPrevWidth = width;
			vPrevHeight = height;
			vPrevInner = InnerAmount;
			vPrevOuter = OuterAmount;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			if ( FillColor == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(FillColor);
			vPrevColor = FillColor;
		}
		
	}

}
