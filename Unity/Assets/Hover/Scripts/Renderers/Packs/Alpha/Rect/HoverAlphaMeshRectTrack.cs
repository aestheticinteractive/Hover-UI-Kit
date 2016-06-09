using Hover.Renderers.Shapes.Rect;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	public class HoverAlphaMeshRectTrack : HoverMeshRectTrack {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public Color TrackColor = Color.gray;

		[DisableWhenControlled]
		public Color FillColor = Color.white;
		
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
			MeshUtil.BuildQuadMesh(vMeshBuild, SizeX, SizeY);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.y = (i == 1 || i == 2 ? UvStartY : UvEndY);
				vMeshBuild.Uvs[i] = uv;
			}

			vMeshBuild.Commit();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			Color color = DisplayUtil.FadeColor((IsFill ? FillColor : TrackColor), Alpha);

			if ( !vForceUpdates && color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);
			vPrevColor = color;
		}
		
	}

}
