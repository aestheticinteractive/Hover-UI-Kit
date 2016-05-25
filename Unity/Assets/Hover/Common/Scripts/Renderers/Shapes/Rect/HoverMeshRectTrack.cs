using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public class HoverMeshRectTrack : HoverMesh {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string UvStartYName = "UvStartY";
		public const string UvEndYName = "UvEndY";
		public const string IsFillName = "IsFill";

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float UvStartY = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float UvEndY = 1;

		[DisableWhenControlled]
		public bool IsFill = false;

		[DisableWhenControlled]
		public Color TrackColor = Color.gray;

		[DisableWhenControlled]
		public Color FillColor = Color.white;
		
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevUvStartY;
		private float vPrevUvEndY;
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
			bool settingsAreUnchanged = (
				!vForceUpdates &&
				SizeX == vPrevSizeX &&
				SizeY == vPrevSizeY && 
				UvStartY == vPrevUvStartY &&
				UvEndY == vPrevUvEndY
			);

			if ( settingsAreUnchanged ) {
				return;
			}

			MeshUtil.BuildQuadMesh(vMeshBuild, SizeX, SizeY);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.y = (i == 1 || i == 2 ? UvStartY : UvEndY);
				vMeshBuild.Uvs[i] = uv;
			}

			vMeshBuild.Commit();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
			vPrevUvStartY = UvStartY;
			vPrevUvEndY = UvEndY;
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
