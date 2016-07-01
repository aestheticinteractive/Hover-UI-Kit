using System;
using Hover.Items;
using Hover.Renderers.Contents;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverRendererSliderSegments))]
	public class HoverRendererSlider : HoverRenderer {

		public const string ZeroValueName = "ZeroValue";
		public const string HandleValueName = "HandleValue";
		public const string JumpValueName = "JumpValue";
		public const string AllowJumpName = "AllowJump";
		public const string TickCountName = "TickCount";
		public const string FillStartingPointName = "FillStartingPoint";
		
		[DisableWhenControlled]
		public GameObject Container;

		[DisableWhenControlled]
		public HoverFill Track;

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

		[DisableWhenControlled(RangeMin=0.01f)]
		public float TickSizeY = 0.34f;

		[DisableWhenControlled]
		public SliderFillType FillStartingPoint = SliderFillType.Zero;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildFillCount() {
			return 0; //TODO: temporary
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
			/*Track.Controllers.Set(HoverFillTrack.SegmentsName, this);
			Track.Segments = gameObject.GetComponent<HoverRendererSliderSegments>();

			Track.OuterRadius = OuterRadius;
			Track.InnerRadius = InnerRadius;
			Track.Alpha = MasterAlpha*(IsEnabled ? EnabledAlpha : DisabledAlpha);
			Track.SortingLayer = SortingLayer;*/
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtons() {
			HoverRendererSliderSegments segs = gameObject.GetComponent<HoverRendererSliderSegments>();

			RendererUtil.SetActiveWithUpdate(JumpButton, (AllowJump && segs.IsJumpVisible));

			HandleButton.Controllers.Set(IsEnabledName, this);
			JumpButton.Controllers.Set(IsEnabledName, this);

			HandleButton.IsEnabled = IsEnabled;
			JumpButton.IsEnabled = IsEnabled;

			/*HandleButton.IconOuterType = HoverIcon.IconOffset.None;
			HandleButton.IconInnerType = HoverIcon.IconOffset.Slider;

			HandleButton.ShowEdge = ShowEdge;
			JumpButton.ShowEdge = ShowEdge;*/
		}
	}

}
