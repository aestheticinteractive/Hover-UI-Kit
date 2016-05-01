using System;
using Hover.Common.Display;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Fills {

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
		
		public Color FillColor = Color.gray;
		
		private MeshBuilder vMeshBuild;
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevInner;
		private float vPrevOuter;
		private Color vPrevColor;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int MaterialRenderQueue {
			get {
				return GetComponent<MeshRenderer>().sharedMaterial.renderQueue;
			}
		}

		
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

			vPrevSizeX = -1;
			vPrevColor = new Color(0, 0, 0, -1);

			UpdateAfterRenderer();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh() {
			bool hasSizeOrAmountChanged = (
				SizeX != vPrevSizeX || 
				SizeY != vPrevSizeY || 
				InnerAmount != vPrevInner || 
				OuterAmount != vPrevOuter
			);

			if ( !hasSizeOrAmountChanged ) {
				return;
			}

			MeshUtil.BuildHollowRectangleMesh(vMeshBuild, SizeX, SizeY, InnerAmount, OuterAmount);
			vMeshBuild.Commit();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
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
