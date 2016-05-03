using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Meshes {

	/*================================================================================================*/
	public class HoverRendererMeshHollowRectangle : HoverRendererMesh {
	
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float OuterAmount = 1;
		
		[Range(0, 1)]
		public float InnerAmount = 0.5f;
		
		public Color FillColor = Color.gray;
		
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevInner;
		private float vPrevOuter;
		private Color vPrevColor;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateAfterRenderer() {
			base.UpdateAfterRenderer();
			UpdateColor();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool CreateMeshBuilderIfNeeded(bool pNewMesh) {			
			if ( !base.CreateMeshBuilderIfNeeded(pNewMesh) ) {
				return false;
			}
			
			vPrevSizeX = -1;
			vPrevColor = new Color(0, 0, 0, -1);

			UpdateAfterRenderer();
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
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
