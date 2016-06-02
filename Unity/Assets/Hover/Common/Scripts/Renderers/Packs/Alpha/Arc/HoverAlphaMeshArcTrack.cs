using Hover.Common.Renderers.Shapes.Arc;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	public class HoverAlphaMeshArcTrack : HoverMeshArcTrack {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public Color TrackColor = Color.gray;

		[DisableWhenControlled]
		public Color FillColor = Color.white;
		
		private int vPrevSteps;
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
			float halfRadians = ArcAngle/180*Mathf.PI/2;
			int steps = GetArcMeshSteps();

			MeshUtil.BuildRingMesh(vMeshBuild, InnerRadius, OuterRadius,
				-halfRadians, halfRadians, steps);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(UvStartY, UvEndY, (float)(i/2)/steps);
				vMeshBuild.Uvs[i] = uv;
			}

			vMeshBuild.Commit();

			if ( steps != vPrevSteps ) {
				vPrevColor = new Color(-1, -1, -1);
				UpdateColor(); //because mesh vertex count has changed
				vPrevSteps = steps;
			}
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
