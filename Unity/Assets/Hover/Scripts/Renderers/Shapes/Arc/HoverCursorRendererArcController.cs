using Hover.Cursors;
using Hover.Renderers.Packs.Alpha.Arc;
using Hover.Renderers.CursorPacks.Alpha.Arc;
using Hover.Renderers.Utils;
using Hover.Utils;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public class HoverCursorRendererArcController : HoverCursorRendererController {
	
		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.75f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.55f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 270;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorRendererArc CursorArcRenderer {
			get { return (_CursorRenderer as ICursorRendererArc); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string DefaultCursorPrefabResourcePath {
			get { return "Prefabs/HoverAlphaCursorRendererArc-Default"; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override ICursorRenderer FindOrBuildCursor() {
			return RendererUtil.FindOrBuildRenderer<ICursorRendererArc>(
				gameObject.transform, CursorRendererPrefab, "Cursor", 
				typeof(HoverAlphaCursorRendererArc));
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateCursorSettings(HoverCursorFollower pFollower) {
			base.UpdateCursorSettings(pFollower);
			
			CursorArcRenderer.OuterRadius = OuterRadius;
			CursorArcRenderer.InnerRadius = InnerRadius;
			CursorArcRenderer.ArcAngle = ArcAngle;
		}
		
	}

}
