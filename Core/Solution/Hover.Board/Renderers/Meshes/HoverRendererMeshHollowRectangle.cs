using Hover.Common.Display;
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
		public float Alpha = 1;

		[Range(0, 1)]
		public float OuterAmount = 1;
		
		[Range(0, 1)]
		public float InnerAmount = 0.5f;
		
		public bool UseUvRelativeToSize = false;
		public Color FillColor = Color.gray;
		
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevInner;
		private float vPrevOuter;
		private bool vPrevUseUv;
		private Color vPrevColor;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool UpdateAfterRenderer() {
			if ( !base.UpdateAfterRenderer() ) {
				return false;
			}

			UpdateColor();
			return true;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			bool hasSizeOrAmountChanged = (
				SizeX != vPrevSizeX || 
				SizeY != vPrevSizeY || 
				InnerAmount != vPrevInner || 
				OuterAmount != vPrevOuter ||
				UseUvRelativeToSize != vPrevUseUv
			);

			if ( !hasSizeOrAmountChanged ) {
				return;
			}
			
			float outerW;
			float outerH;
			float innerW;
			float innerH;
			
			if ( SizeX >= SizeY ) {
				outerH = SizeY*OuterAmount;
				innerH = SizeY*InnerAmount;
				outerW = SizeX-(SizeY-outerH);
				innerW = SizeX-(SizeY-innerH);
			}
			else {
				outerW = SizeX*OuterAmount;
				innerW = SizeX*InnerAmount;
				outerH = SizeY-(SizeX-outerW);
				innerH = SizeY-(SizeX-innerW);
			}
			
			float uvToCenterX = (UseUvRelativeToSize ? 1-outerW/SizeX : 0);
			float uvToCenterY = (UseUvRelativeToSize ? 1-outerH/SizeY : 0);

			MeshUtil.BuildHollowRectangleMesh(vMeshBuild, outerW, outerH, innerW, innerH);
			
			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(uv.x, 0.5f, uvToCenterX);
				uv.y = Mathf.Lerp(uv.y, 0.5f, uvToCenterY);
				vMeshBuild.Uvs[i] = uv;
			}
			
			vMeshBuild.Commit();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
			vPrevInner = InnerAmount;
			vPrevOuter = OuterAmount;
			vPrevUseUv = UseUvRelativeToSize;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			Color color = DisplayUtil.FadeColor(FillColor, Alpha);

			if ( color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);
			vPrevColor = color;
		}
		
	}

}
