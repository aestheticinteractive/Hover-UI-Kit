using Hover.Renderers.Elements.Sliders;
using UnityEngine;

namespace Hover.Renderers.Elements.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverRendererSliderSegmentsRect : HoverRendererSliderSegments {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateInfoWithShape() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();
			HoverShapeRect handleShapeRect = (HoverShapeRect)rendSlider.HandleButton.GetShape();
			HoverShapeRect jumpShapeRect = (HoverShapeRect)rendSlider.JumpButton.GetShape();

			vInfo.TrackStartPosition = -shapeRect.SizeY/2;
			vInfo.TrackEndPosition = shapeRect.SizeY/2;
			vInfo.HandleSize = handleShapeRect.SizeY;
			vInfo.JumpSize = (rendSlider.AllowJump ? jumpShapeRect.SizeY : 0);
		}

	}

}
