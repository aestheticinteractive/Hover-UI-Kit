using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

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

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Container")]
		private GameObject _Container;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Track")]
		private HoverFillSlider _Track;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("HandleButton")]
		private HoverRendererButton _HandleButton;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("JumpButton")]
		private HoverRendererButton _JumpButton;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("ZeroValue")]
		private float _ZeroValue = 0.5f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("HandleValue")]
		private float _HandleValue = 0.5f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("JumpValue")]
		private float _JumpValue = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("AllowJump")]
		private bool _AllowJump = false;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("TickCount")]
		private int _TickCount = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("FillStartingPoint")]
		private SliderFillType _FillStartingPoint = SliderFillType.MinimumValue;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowButtonEdges")]
		private bool _ShowButtonEdges = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public GameObject Container {
			get => _Container;
			set => this.UpdateValueWithTreeMessage(ref _Container, value, "Container");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverFillSlider Track {
			get => _Track;
			set => this.UpdateValueWithTreeMessage(ref _Track, value, "Track");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererButton HandleButton {
			get => _HandleButton;
			set => this.UpdateValueWithTreeMessage(ref _HandleButton, value, "HandleButton");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererButton JumpButton {
			get => _JumpButton;
			set => this.UpdateValueWithTreeMessage(ref _JumpButton, value, "JumpButton");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ZeroValue {
			get => _ZeroValue;
			set => this.UpdateValueWithTreeMessage(ref _ZeroValue, value, "ZeroValue");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float HandleValue {
			get => _HandleValue;
			set => this.UpdateValueWithTreeMessage(ref _HandleValue, value, "HandleValue");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float JumpValue {
			get => _JumpValue;
			set => this.UpdateValueWithTreeMessage(ref _JumpValue, value, "JumpValue");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AllowJump {
			get => _AllowJump;
			set => this.UpdateValueWithTreeMessage(ref _AllowJump, value, "AllowJump");
		}

		/*--------------------------------------------------------------------------------------------*/
		public int TickCount {
			get => _TickCount;
			set => this.UpdateValueWithTreeMessage(ref _TickCount, value, "TickCount");
		}

		/*--------------------------------------------------------------------------------------------*/
		public SliderFillType FillStartingPoint {
			get => _FillStartingPoint;
			set => this.UpdateValueWithTreeMessage(ref _FillStartingPoint, value, "FillStartingPoint");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowButtonEdges {
			get => _ShowButtonEdges;
			set => this.UpdateValueWithTreeMessage(ref _ShowButtonEdges, value, "ShowButtonEdges");
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
