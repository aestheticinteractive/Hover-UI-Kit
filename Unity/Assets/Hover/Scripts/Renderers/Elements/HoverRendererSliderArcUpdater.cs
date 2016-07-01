using Hover.Renderers.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverRendererSliderSegments))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverRendererSliderArcUpdater : HoverRendererSliderUpdater {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			HoverShapeArc handleShapeArc = (HoverShapeArc)rendSlider.HandleButton.GetShape();
			HoverShapeArc jumpShapeArc = (HoverShapeArc)rendSlider.JumpButton.GetShape();

			shapeArc.ArcDegrees = Mathf.Max(shapeArc.ArcDegrees, handleShapeArc.ArcDegrees);

			UpdateButtonRadii(shapeArc, handleShapeArc);
			UpdateButtonRadii(shapeArc, jumpShapeArc);
			UpdateButtonRotations(rendSlider);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonRadii(HoverShapeArc pShapeArc, HoverShapeArc pButtonShapeArc) {
			pButtonShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			pButtonShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);

			pButtonShapeArc.OuterRadius = pShapeArc.OuterRadius;
			pButtonShapeArc.InnerRadius = pShapeArc.InnerRadius;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonRotations(HoverRendererSlider pRendSlider) {
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

				button.transform.localRotation = Quaternion.AngleAxis(
					(segInfo.StartPosition+segInfo.EndPosition)/2, Vector3.forward);
			}
		}

	}

}
