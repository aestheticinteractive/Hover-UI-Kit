using Hover.Common.Renderers.Shapes.Arc;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha {

	/*================================================================================================*/
	public class HoverAlphaMeshArcLateral : HoverMeshArcButton {
	
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
			//float uvToCenterX = (UseUvRelativeToSize ? 1-outerW/SizeX : 0);
			//float uvToCenterY = (UseUvRelativeToSize ? 1-outerH/SizeY : 0);
			float innerRad = Mathf.Lerp(InnerRadius, OuterRadius, InnerAmount);
			float outerRad = Mathf.Lerp(InnerRadius, OuterRadius, OuterAmount);
			float halfRadians = ArcAngle/180*Mathf.PI/2;
			int steps = (int)Mathf.Max(2, ArcAngle/2);

			MeshUtil.BuildRingMesh(vMeshBuild, innerRad, outerRad, -halfRadians, halfRadians, steps);
			
			/*for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(uv.x, 0.5f, uvToCenterX);
				uv.y = Mathf.Lerp(uv.y, 0.5f, uvToCenterY);
				vMeshBuild.Uvs[i] = uv;
			}*/
			
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
