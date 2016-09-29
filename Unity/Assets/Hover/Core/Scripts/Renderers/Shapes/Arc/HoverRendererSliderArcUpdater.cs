using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Arc {

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

			UpdateTrackShape(shapeArc, rendSlider);
			UpdateButtonRadii(shapeArc, handleShapeArc);
			UpdateButtonRadii(shapeArc, jumpShapeArc);
			UpdateButtonRotations(rendSlider);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrackShape(HoverShapeArc pShapeArc, HoverRendererSlider pRendSlider) {
			HoverShapeArc trackShapeArc = (HoverShapeArc)pRendSlider.Track.GetShape();

			trackShapeArc.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			trackShapeArc.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			trackShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			trackShapeArc.OuterRadius = pShapeArc.OuterRadius;
			trackShapeArc.InnerRadius = pShapeArc.InnerRadius;
			trackShapeArc.ArcDegrees = pShapeArc.ArcDegrees;
		}

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

				button.Controllers.Set(SettingsControllerMap.TransformLocalRotation+".z", this);

				Vector3 buttonLocalEuler = button.transform.localRotation.eulerAngles;
				buttonLocalEuler.z = (segInfo.StartPosition+segInfo.EndPosition)/2;
				button.transform.localRotation = Quaternion.Euler(buttonLocalEuler);
			}
		}

	}

}
