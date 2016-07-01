using System;
using System.Collections.Generic;
using Hover.Items;
using Hover.Renderers.Contents;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverRendererSliderUpdater))]
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

		private readonly List<SliderUtil.SegmentInfo> vSegmentInfoList;
		private readonly List<SliderUtil.SegmentInfo> vTickInfoList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererSlider() {
			vSegmentInfoList = new List<SliderUtil.SegmentInfo>();
			vTickInfoList = new List<SliderUtil.SegmentInfo>();
		}


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

		/*--------------------------------------------------------------------------------------------*/
		public List<SliderUtil.SegmentInfo> GetSegmentInfoList() {
			return vSegmentInfoList;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			UpdateSliderSegments();
			UpdateGeneralSettings();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSegments() {
			SliderUtil.SliderInfo info = gameObject
				.GetComponent<HoverRendererSliderUpdater>()
				.FillShapeRelatedSliderInfo();

			info.FillType = FillStartingPoint;
			info.HandleValue = HandleValue;
			info.JumpValue = JumpValue;
			info.ZeroValue = ZeroValue;
			info.TickCount = TickCount;
			info.TickSize = TickSizeY;

			SliderUtil.CalculateSegments(info, vSegmentInfoList);
			SliderUtil.CalculateTicks(info, vSegmentInfoList, vTickInfoList);
			//TODO: Track.SegmentInfoList = vSegmentInfoList;
			//TODO: Track.TickInfoList = vTickInfoList;

			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);

			foreach ( SliderUtil.Segment seg in vSegmentInfoList ) {
				Debug.Log(" - "+seg.Type+": "+seg.StartPosition+" / "+seg.EndPosition);
			}*/
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			bool isJumpSegmentVisible = false;

			for ( int i = 0 ; i < vSegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = vSegmentInfoList[i];

				if ( segInfo.Type == SliderUtil.SegmentType.Jump ) {
					isJumpSegmentVisible = true;
					break;
				}
			}

			RendererUtil.SetActiveWithUpdate(JumpButton, (AllowJump && isJumpSegmentVisible));

			/*Track.OuterRadius = OuterRadius;
			Track.InnerRadius = InnerRadius;

			HandleButton.IsEnabled = IsEnabled;
			HandleButton.EnabledAlpha = MasterAlpha*EnabledAlpha;
			HandleButton.DisabledAlpha = MasterAlpha*DisabledAlpha;
			JumpButton.IsEnabled = IsEnabled;
			JumpButton.EnabledAlpha = MasterAlpha*EnabledAlpha;
			JumpButton.DisabledAlpha = MasterAlpha*DisabledAlpha;
			Track.Alpha = MasterAlpha*(IsEnabled ? EnabledAlpha : DisabledAlpha);
			
			HandleButton.SortingLayer = SortingLayer;
			JumpButton.SortingLayer = SortingLayer;
			Track.SortingLayer = SortingLayer;

			HandleButton.IconOuterType = HoverIcon.IconOffset.None;
			HandleButton.IconInnerType = HoverIcon.IconOffset.Slider;

			if ( RendererController == null ) {
				return;
			}

			HandleButton.ShowEdge = ShowEdge;
			JumpButton.ShowEdge = ShowEdge;*/
		}
	}

}
