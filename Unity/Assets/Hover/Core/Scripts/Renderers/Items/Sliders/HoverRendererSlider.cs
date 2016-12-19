using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverRendererSliderSegments))]
	public class HoverRendererSlider : HoverRenderer {

		public const string ZeroValueName = "ZeroValue";
		public const string HandleValueName = "HandleValue";
		public const string JumpValueName = "JumpValue";
		public const string AllowJumpName = "AllowJump";
		public const string TickCountName = "TickCount";
		public const string FillStartingPointName = "FillStartingPoint";
		public const string ShowButtonEdgesName = "ShowButtonEdges";
		
		[DisableWhenControlled]
		public GameObject Container;

		[DisableWhenControlled]
		public HoverFillSlider Track;

		[DisableWhenControlled]
		public HoverRendererButton HandleButton;

		[DisableWhenControlled]
		public HoverRendererButton JumpButton;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float ZeroValue = 0.5f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float HandleValue = 0.5f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float JumpValue = 0;

		[DisableWhenControlled]
		public bool AllowJump = false;

		[DisableWhenControlled]
		public int TickCount = 0;

		[DisableWhenControlled]
		public SliderFillType FillStartingPoint = SliderFillType.MinimumValue;
		
		[DisableWhenControlled]
		public bool ShowButtonEdges = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildFillCount() {
			return 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverFill GetChildFill(int pIndex) {
			switch ( pIndex ) {
				case 0: return Track;
			}

			throw new ArgumentOutOfRangeException();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildRendererCount() {
			return 2;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverRenderer GetChildRenderer(int pIndex) {
			switch ( pIndex ) {
				case 0: return HandleButton;
				case 1: return JumpButton;
			}

			throw new ArgumentOutOfRangeException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvas GetCanvas() {
			return HandleButton.Canvas;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvasDataUpdater GetCanvasDataUpdater() {
			return HandleButton.Canvas.GetComponent<HoverCanvasDataUpdater>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetCenterWorldPosition() {
			return HandleButton.GetShape().GetCenterWorldPosition();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( AllowJump ) {
				return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldPosition);
			}

			return HandleButton.GetShape().GetNearestWorldPosition(pFromWorldPosition);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			if ( AllowJump ) {
				return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldRay, out pRaycast);
			}

			return HandleButton.GetShape().GetNearestWorldPosition(pFromWorldRay, out pRaycast);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetValueViaNearestWorldPosition(Vector3 pNearestWorldPosition) {
			return GetComponent<HoverShape>().GetSliderValueViaNearestWorldPosition(
				pNearestWorldPosition, Container.transform, HandleButton.GetShape());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			UpdateTrack();
			UpdateButtons();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrack() {
			Track.Controllers.Set(HoverFillSlider.SegmentInfoName, this);
			Track.SegmentInfo = gameObject.GetComponent<HoverRendererSliderSegments>();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtons() {
			HoverRendererSliderSegments segs = gameObject.GetComponent<HoverRendererSliderSegments>();

			HandleButton.Controllers.Set(HoverRendererButton.IsEnabledName, this);
			JumpButton.Controllers.Set(HoverRendererButton.IsEnabledName, this);
			HandleButton.Fill.Controllers.Set(HoverFillButton.ShowEdgeName, this);
			JumpButton.Fill.Controllers.Set(HoverFillButton.ShowEdgeName, this);

			HandleButton.IsEnabled = IsEnabled;
			JumpButton.IsEnabled = IsEnabled;

			HandleButton.Fill.ShowEdge = ShowButtonEdges;
			JumpButton.Fill.ShowEdge = ShowButtonEdges;

			RendererUtil.SetActiveWithUpdate(JumpButton, (AllowJump && segs.IsJumpVisible));
		}
	}

}
