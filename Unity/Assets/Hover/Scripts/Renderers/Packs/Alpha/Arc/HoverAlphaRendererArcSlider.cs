using System.Collections.Generic;
using Hover.Items.Types;
using Hover.Renderers.Contents;
using Hover.Renderers.Shapes.Arc;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverAlphaRendererArcSlider : HoverAlphaRendererArc, IRendererArcSlider {
		
		public const string ZeroValueName = "_ZeroValue";
		public const string HandleValueName = "_HandleValue";
		public const string JumpValueName = "_JumpValue";
		public const string AllowJumpName = "_AllowJump";
		public const string TickCountName = "_TickCount";
		public const string FillStartingPointName = "_FillStartingPoint";

		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject Container;

		[DisableWhenControlled]
		public HoverAlphaFillArcSlider Track;

		[DisableWhenControlled]
		public HoverAlphaRendererArcButton HandleButton;

		[DisableWhenControlled]
		public HoverAlphaRendererArcButton JumpButton;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _ZeroValue = 0.5f;
				
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _HandleValue = 0.5f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		private float _JumpValue = 0;
		
		[SerializeField]
		[DisableWhenControlled]
		private bool _AllowJump = false;
		
		[SerializeField]
		[DisableWhenControlled]
		private int _TickCount = 0;
		
		[DisableWhenControlled(RangeMin=0.01f)]
		public float TickSizeY = 0.34f;

		[SerializeField]
		[DisableWhenControlled]
		private SliderItem.FillType _FillStartingPoint = SliderItem.FillType.Zero;

		private readonly List<SliderUtil.SegmentInfo> vSegmentInfoList;
		private readonly List<SliderUtil.SegmentInfo> vTickInfoList;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererArcSlider() {
			vSegmentInfoList = new List<SliderUtil.SegmentInfo>();
			vTickInfoList = new List<SliderUtil.SegmentInfo>();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float ZeroValue {
			get { return _ZeroValue; }
			set { _ZeroValue = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float HandleValue {
			get { return _HandleValue; }
			set { _HandleValue = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float JumpValue {
			get { return _JumpValue; }
			set { _JumpValue = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool AllowJump {
			get { return _AllowJump; }
			set { _AllowJump = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public int TickCount {
			get { return _TickCount; }
			set { _TickCount = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public SliderItem.FillType FillStartingPoint {
			get { return _FillStartingPoint; }
			set { _FillStartingPoint = value; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			ArcAngle = Mathf.Max(ArcAngle, HandleButton.ArcAngle);

			UpdateControl();
			UpdateSliderSegments();
			UpdateGeneralSettings();

			RendererController = null;
			Controllers.TryExpireControllers();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( AllowJump ) {
				return RendererUtil.GetNearestWorldPositionOnArc(
					pFromWorldPosition, Container.transform, OuterRadius, InnerRadius, ArcAngle);
			}
			
			return RendererUtil.GetNearestWorldPositionOnArc(pFromWorldPosition, HandleButton.transform, 
				HandleButton.OuterRadius, HandleButton.InnerRadius, HandleButton.ArcAngle);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetValueViaNearestWorldPosition(Vector3 pNearestWorldPosition) {
			Vector3 nearLocalPos = Container.transform.InverseTransformPoint(pNearestWorldPosition);
			float fromAngle;
			Vector3 fromAxis;
			Quaternion fromLocalRot = Quaternion.FromToRotation(Vector3.right, nearLocalPos.normalized);

			fromLocalRot.ToAngleAxis(out fromAngle, out fromAxis);
			fromAngle *= Mathf.Sign(nearLocalPos.y);

			float halfTrackAngle = (ArcAngle-HandleButton.ArcAngle)/2;
			return Mathf.InverseLerp(-halfTrackAngle, halfTrackAngle, fromAngle);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Container = new GameObject("Container");
			Container.transform.SetParent(gameObject.transform, false);
			Container.AddComponent<TreeUpdater>();
			
			Track = BuildTrack();
			HandleButton = BuildButton("Handle");
			JumpButton = BuildButton("Jump");

			HandleButton.ArcAngle = 14;
			JumpButton.ArcAngle = 7;

			Track.InsetOuter = 1;
			Track.InsetInner = 1;
			
			RendererUtil.SetActiveWithUpdate(JumpButton.Canvas, false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillArcSlider BuildTrack() {
			var trackGo = new GameObject("Track");
			trackGo.transform.SetParent(Container.transform, false);
			return trackGo.AddComponent<HoverAlphaFillArcSlider>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaRendererArcButton BuildButton(string pName) {
			var ArcGo = new GameObject(pName);
			ArcGo.transform.SetParent(Container.transform, false);
			return ArcGo.AddComponent<HoverAlphaRendererArcButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateControl() {
			Track.Controllers.Set(HoverFillArcSlider.SegmentInfoListName, this);
			Track.Controllers.Set(HoverFillArcSlider.TickInfoListName, this);
			Track.Controllers.Set(HoverFillArcSlider.OuterRadiusName, this);
			Track.Controllers.Set(HoverFillArcSlider.InnerRadiusName, this);
			Track.Controllers.Set(HoverAlphaFillArcSlider.AlphaName, this);
			Track.Controllers.Set(HoverFill.SortingLayerName, this);
			
			HandleButton.Controllers.Set("Transform.localRotation", this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.OuterRadiusName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.InnerRadiusName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.IsEnabledName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.EnabledAlphaName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.DisabledAlphaName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererArcButton.SortingLayerName, this);
			
			JumpButton.Controllers.Set("GameObject.activeSelf", this);
			JumpButton.Controllers.Set("Transform.localRotation", this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.OuterRadiusName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.InnerRadiusName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.IsEnabledName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.EnabledAlphaName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.DisabledAlphaName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererArcButton.SortingLayerName, this);
			
			HandleButton.Canvas.IconOuter.Controllers.Set(HoverIcon.IconTypeName, this);
			HandleButton.Canvas.IconInner.Controllers.Set(HoverIcon.IconTypeName, this);

			ISettingsController cont = RendererController;

			if ( cont == null ) {
				HandleButton.SliderController = null;
				JumpButton.SliderController = null;
				return;
			}
			
			HandleButton.SliderController = this;
			JumpButton.SliderController = this;
			
			Controllers.Set(OuterRadiusName, cont);
			Controllers.Set(InnerRadiusName, cont);
			Controllers.Set(ArcAngleName, cont);
			Controllers.Set(IsEnabledName, cont);
			Controllers.Set(ZeroValueName, cont);
			Controllers.Set(HandleValueName, cont);
			Controllers.Set(JumpValueName, cont);
			Controllers.Set(AllowJumpName, cont);
			Controllers.Set(TickCountName, cont);
			Controllers.Set(FillStartingPointName, cont);
			Controllers.Set(SortingLayerName, cont);
			
			HandleButton.Fill.Controllers.Set(HoverFillArcButton.HighlightProgressName, cont);
			HandleButton.Fill.Controllers.Set(HoverFillArcButton.SelectionProgressName, cont);
			JumpButton.Fill.Controllers.Set(HoverFillArcButton.HighlightProgressName, cont);
			JumpButton.Fill.Controllers.Set(HoverFillArcButton.SelectionProgressName, cont);
			
			HandleButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);
			JumpButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);
			
			HandleButton.Canvas.Label.Controllers.Set("Text.text", cont);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSegments() {
			var info = new SliderUtil.SliderInfo {
				FillType = FillStartingPoint,
				TrackStartPosition = -ArcAngle/2,
				TrackEndPosition = ArcAngle/2,
				HandleSize = HandleButton.ArcAngle,
				HandleValue = HandleValue,
				JumpSize = (AllowJump ? JumpButton.ArcAngle : 0),
				JumpValue = JumpValue,
				ZeroValue = ZeroValue,
				TickCount = TickCount,
				TickSize = TickSizeY
			};
			
			SliderUtil.CalculateSegments(info, vSegmentInfoList);
			SliderUtil.CalculateTicks(info, vSegmentInfoList, vTickInfoList);
			Track.SegmentInfoList = vSegmentInfoList;
			Track.TickInfoList = vTickInfoList;
			
			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);
			
			foreach ( SliderUtil.Segment seg in vSegmentInfoList ) {
				Debug.Log(" - "+seg.Type+": "+seg.StartPosition+" / "+seg.EndPosition);
			}*/
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			bool isJumpSegmentVisible = false;
			
			foreach ( SliderUtil.SegmentInfo segInfo in vSegmentInfoList ) {
				bool isHandle = (segInfo.Type == SliderUtil.SegmentType.Handle);
				bool isJump = (segInfo.Type == SliderUtil.SegmentType.Jump);
				
				if ( !isHandle && !isJump ) {
					continue;
				}
				
				HoverAlphaRendererArcButton button = (isHandle ? HandleButton : JumpButton);
				button.transform.localRotation = Quaternion.AngleAxis(
					(segInfo.StartPosition+segInfo.EndPosition)/2, Vector3.forward);
				
				if ( isJump ) {
					isJumpSegmentVisible = true;
				}
			}
			
			HandleButton.OuterRadius = OuterRadius;
			HandleButton.InnerRadius = InnerRadius;
			JumpButton.OuterRadius = OuterRadius;
			JumpButton.InnerRadius = InnerRadius;
			Track.OuterRadius = OuterRadius;
			Track.InnerRadius = InnerRadius;

			HandleButton.IsEnabled = IsEnabled;
			HandleButton.EnabledAlpha = EnabledAlpha;
			HandleButton.DisabledAlpha = DisabledAlpha;
			JumpButton.IsEnabled = IsEnabled;
			JumpButton.EnabledAlpha = EnabledAlpha;
			JumpButton.DisabledAlpha = DisabledAlpha;
			Track.Alpha = (IsEnabled ? EnabledAlpha : DisabledAlpha);
			
			HandleButton.SortingLayer = SortingLayer;
			JumpButton.SortingLayer = SortingLayer;
			Track.SortingLayer = SortingLayer;

			HandleButton.IconOuterType = HoverIcon.IconOffset.None;
			HandleButton.IconInnerType = HoverIcon.IconOffset.Slider;
			
			RendererUtil.SetActiveWithUpdate(JumpButton, (AllowJump && isJumpSegmentVisible));

			if ( RendererController == null ) {
				return;
			}

			HandleButton.HighlightProgress = HighlightProgress;
			JumpButton.HighlightProgress = HighlightProgress;
			HandleButton.SelectionProgress = SelectionProgress;
			JumpButton.SelectionProgress = SelectionProgress;

			HandleButton.LabelText = LabelText;
			
			RendererUtil.SetActiveWithUpdate(HandleButton.Fill.Edge, ShowEdge);
			RendererUtil.SetActiveWithUpdate(JumpButton.Fill.Edge, ShowEdge);
		}
		
	}

}
