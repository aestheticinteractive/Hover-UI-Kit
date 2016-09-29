using Hover.Core.Renderers.Items.Sliders;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverRendererSliderSegmentsArc : HoverRendererSliderSegments {

		public float TickArcDegrees = 0.34f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateInfoWithShape() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			HoverShapeArc handleShapeArc = (HoverShapeArc)rendSlider.HandleButton.GetShape();
			HoverShapeArc jumpShapeArc = (HoverShapeArc)rendSlider.JumpButton.GetShape();

			TickArcDegrees = Mathf.Max(0, TickArcDegrees);

			vInfo.TrackStartPosition = -shapeArc.ArcDegrees/2;
			vInfo.TrackEndPosition = shapeArc.ArcDegrees/2;
			vInfo.HandleSize = handleShapeArc.ArcDegrees;
			vInfo.JumpSize = (rendSlider.AllowJump ? jumpShapeArc.ArcDegrees : 0);
			vInfo.TickSize = TickArcDegrees;
		}

	}

}
