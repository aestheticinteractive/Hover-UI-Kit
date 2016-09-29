using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverRendererSliderSegments))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverRendererSliderRectUpdater : HoverRendererSliderUpdater {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();
			HoverShapeRect handleShapeRect = (HoverShapeRect)rendSlider.HandleButton.GetShape();
			HoverShapeRect jumpShapeRect = (HoverShapeRect)rendSlider.JumpButton.GetShape();

			shapeRect.SizeY = Mathf.Max(shapeRect.SizeY, handleShapeRect.SizeY);

			UpdateTrackShape(shapeRect, rendSlider);
			UpdateButtonWidth(shapeRect, handleShapeRect);
			UpdateButtonWidth(shapeRect, jumpShapeRect);
			UpdateButtonPositions(rendSlider);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrackShape(HoverShapeRect pShapeRect, HoverRendererSlider pRendSlider) {
			HoverShapeRect trackShapeRect = (HoverShapeRect)pRendSlider.Track.GetShape();

			trackShapeRect.Controllers.Set(HoverShapeRect.SizeXName, this);
			trackShapeRect.Controllers.Set(HoverShapeRect.SizeYName, this);

			trackShapeRect.SizeX = pShapeRect.SizeX;
			trackShapeRect.SizeY = pShapeRect.SizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonWidth(HoverShapeRect pShapeRect, HoverShapeRect pButtonShapeRect) {
			pButtonShapeRect.Controllers.Set(HoverShapeRect.SizeXName, this);
			pButtonShapeRect.SizeX = pShapeRect.SizeX;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonPositions(HoverRendererSlider pRendSlider) {
			HoverRendererSliderSegments segs = gameObject.GetComponent<HoverRendererSliderSegments>();

			for ( int i = 0 ; i < segs.SegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = segs.SegmentInfoList[i];
				bool isHandle = (segInfo.Type == SliderUtil.SegmentType.Handle);
				bool isJump = (segInfo.Type == SliderUtil.SegmentType.Jump);

				if ( !isHandle && !isJump ) {
					continue;
				}

				HoverRendererButton button = (isHandle ? 
					pRendSlider.HandleButton : pRendSlider.JumpButton);

				button.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);

				Vector3 buttonLocalPos = button.transform.localPosition;
				buttonLocalPos.y = (segInfo.StartPosition+segInfo.EndPosition)/2;
				button.transform.localPosition = buttonLocalPos;
			}
		}

	}

}
