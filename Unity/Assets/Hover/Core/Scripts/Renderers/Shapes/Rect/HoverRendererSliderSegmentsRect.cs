using Hover.Core.Renderers.Items.Sliders;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverRendererSliderSegmentsRect : HoverRendererSliderSegments {

		public float TickSizeY = 0.001f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateInfoWithShape() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();
			HoverShapeRect handleShapeRect = (HoverShapeRect)rendSlider.HandleButton.GetShape();
			HoverShapeRect jumpShapeRect = (HoverShapeRect)rendSlider.JumpButton.GetShape();

			TickSizeY = Mathf.Max(0, TickSizeY);

			vInfo.TrackStartPosition = -shapeRect.SizeY/2;
			vInfo.TrackEndPosition = shapeRect.SizeY/2;
			vInfo.HandleSize = handleShapeRect.SizeY;
			vInfo.JumpSize = (rendSlider.AllowJump ? jumpShapeRect.SizeY : 0);
			vInfo.TickSize = TickSizeY;
		}

	}

}
