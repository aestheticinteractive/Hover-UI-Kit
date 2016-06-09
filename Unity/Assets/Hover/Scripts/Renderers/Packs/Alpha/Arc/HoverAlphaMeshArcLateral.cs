using Hover.Renderers.Shapes.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	public class HoverAlphaMeshArcLateral : HoverMeshArcButton {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public Color FillColor = Color.gray;

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
			float innerRad = Mathf.Lerp(InnerRadius, OuterRadius, InnerAmount);
			float outerRad = Mathf.Lerp(InnerRadius, OuterRadius, OuterAmount);
			float innerUv = (UseUvRelativeToSize ? InnerAmount : 0);
			float outerUv = (UseUvRelativeToSize ? OuterAmount : 1);
			float halfRadians = ArcAngle/180*Mathf.PI/2;
			int steps = GetArcMeshSteps();

			MeshUtil.BuildRingMesh(vMeshBuild, innerRad, outerRad, -halfRadians, halfRadians, steps);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				bool isInner = (i%2 == 0);
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.y = (isInner ? innerUv : outerUv);
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
			Color color = DisplayUtil.FadeColor(FillColor, Alpha);

			if ( !vForceUpdates && color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);
			vPrevColor = color;
		}
		
	}

}
