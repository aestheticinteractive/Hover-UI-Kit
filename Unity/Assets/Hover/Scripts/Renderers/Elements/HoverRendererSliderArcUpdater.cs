using System.Collections.Generic;
using Hover.Renderers.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverRendererSliderArcUpdater : HoverRendererSliderUpdater {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override SliderUtil.SliderInfo FillShapeRelatedSliderInfo() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
			HoverShapeArc handleShapeArc = (HoverShapeArc)rendSlider.HandleButton.GetShape();
			HoverShapeArc jumpShapeArc = (HoverShapeArc)rendSlider.JumpButton.GetShape();

			return new SliderUtil.SliderInfo {
				TrackStartPosition = -shapeArc.ArcDegrees/2,
				TrackEndPosition = shapeArc.ArcDegrees/2,
				HandleSize = handleShapeArc.ArcDegrees,
				JumpSize = (rendSlider.AllowJump ? jumpShapeArc.ArcDegrees : 0),
			};
		}


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
			List<SliderUtil.SegmentInfo> segInfoList = pRendSlider.GetSegmentInfoList();

			for ( int i = 0 ; i < segInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = segInfoList[i];
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
