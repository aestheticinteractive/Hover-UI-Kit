using Hover.Renderers.Elements.Sliders;
using UnityEngine;

namespace Hover.Renderers.Elements.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverRendererSliderSegmentsArc : HoverRendererSliderSegments {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateInfoWithShape() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			HoverShapeArc handleShapeArc = (HoverShapeArc)rendSlider.HandleButton.GetShape();
			HoverShapeArc jumpShapeArc = (HoverShapeArc)rendSlider.JumpButton.GetShape();

			vInfo.TrackStartPosition = -shapeArc.ArcDegrees/2;
			vInfo.TrackEndPosition = shapeArc.ArcDegrees/2;
			vInfo.HandleSize = handleShapeArc.ArcDegrees;
			vInfo.JumpSize = (rendSlider.AllowJump ? jumpShapeArc.ArcDegrees : 0);
		}

	}

}
