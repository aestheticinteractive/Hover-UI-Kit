using Hover.Common.Renderers.Shapes.Rect;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha {

	/*================================================================================================*/
	public class HoverAlphaMeshRectCentral : HoverMeshRectButton {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public Color FillColor = Color.gray;
		
		private Color vPrevColor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateColor();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
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
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			Color color = DisplayUtil.FadeColor(FillColor, Alpha);

			if ( !vForceUpdates && color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);
			vPrevColor = color;
		}
		
	}

}
