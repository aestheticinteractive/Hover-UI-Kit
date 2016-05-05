using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Meshes {

	/*================================================================================================*/
	public class HoverRendererMeshSliderRectangle : HoverRendererMesh {
	
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;

		public bool IsFill = false;
		
		public Color TrackColor = Color.gray;
		public Color FillColor = Color.white;
		
		private float vPrevSizeX;
		private float vPrevSizeY;
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
			if ( SizeX == vPrevSizeX && SizeY == vPrevSizeY ) {
				return;
			}

			MeshUtil.BuildQuadMesh(vMeshBuild, SizeX, SizeY);
			vMeshBuild.Commit();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			Color color = (IsFill ? FillColor : TrackColor);

			if ( color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);

			vPrevColor = color;
		}
		
	}

}
