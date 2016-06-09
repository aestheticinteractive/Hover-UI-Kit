using System.Collections.Generic;
using Hover.Items.Types;
using Hover.Renderers.Contents;
using Hover.Renderers.Shapes.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverAlphaRendererRectSlider : MonoBehaviour, IRendererRectSlider,
											IProximityProvider, ISettingsController, ITreeUpdateable {

		public const string SizeXName = "_SizeX";
		public const string SizeYName = "_SizeY";
		public const string AlphaName = "Alpha";
		public const string IsEnabledName = "_IsEnabled";
		public const string ZeroValueName = "_ZeroValue";
		public const string HandleValueName = "_HandleValue";
		public const string JumpValueName = "_JumpValue";
		public const string AllowJumpName = "_AllowJump";
		public const string TickCountName = "_TickCount";
		public const string FillStartingPointName = "_FillStartingPoint";
		public const string SortingLayerName = "_SortingLayer";

		public ISettingsController RendererController { get; set; }
		public ISettingsControllerMap Controllers { get; private set; }
		public string LabelText { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public bool ShowEdge { get; set; }
	
		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject Container;

		[DisableWhenControlled]
		public HoverAlphaFillRectSlider Track;

		[DisableWhenControlled]
		public HoverAlphaRendererRectButton HandleButton;

		[DisableWhenControlled]
		public HoverAlphaRendererRectButton JumpButton;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeX = 0.1f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeY = 0.1f;
		
		[SerializeField]
		[DisableWhenControlled]
		private bool _IsEnabled = true;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float EnabledAlpha = 1;

		[DisableWhenControlled(RangeMin=0.05f, RangeMax=0.9f)]
		public float DisabledAlpha = 0.35f;
		
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
		
		[DisableWhenControlled(RangeMin=0.0001f)]
		public float TickSizeY = 0.0006f;

		[SerializeField]
		[DisableWhenControlled]
		private SliderItem.FillType _FillStartingPoint = SliderItem.FillType.Zero;
		
		[DisableWhenControlled]
		public AnchorTypeWithCustom Anchor = AnchorTypeWithCustom.MiddleCenter;
		
		[SerializeField]
		[DisableWhenControlled]
		private string _SortingLayer = "Default";
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		private readonly List<SliderUtil.SegmentInfo> vSegmentInfoList;
		private readonly List<SliderUtil.SegmentInfo> vTickInfoList;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererRectSlider() {
			Controllers = new SettingsControllerMap();
			vSegmentInfoList = new List<SliderUtil.SegmentInfo>();
			vTickInfoList = new List<SliderUtil.SegmentInfo>();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get { return _SizeX; }
			set { _SizeX = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get { return _SizeY; }
			set { _SizeY = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return _IsEnabled; }
			set { _IsEnabled = value; }
		}
		
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
		
		/*--------------------------------------------------------------------------------------------*/
		public string SortingLayer {
			get { return _SortingLayer; }
			set { _SortingLayer = value; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildElements();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			SizeY = Mathf.Max(SizeY, HandleButton.SizeY);

			UpdateControl();
			UpdateSliderSegments();
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			RendererController = null;
			Controllers.TryExpireControllers();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( AllowJump ) {
				return RendererUtil.GetNearestWorldPositionOnRectangle(
					pFromWorldPosition, Container.transform, SizeX, SizeY);
			}
			
			return RendererUtil.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, HandleButton.transform, HandleButton.SizeX, HandleButton.SizeY);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetValueViaNearestWorldPosition(Vector3 pNearestWorldPosition) {
			Vector3 nearLocalPos = Container.transform.InverseTransformPoint(pNearestWorldPosition);
			float halfTrackSizeY = (SizeY-HandleButton.SizeY)/2;
			return Mathf.InverseLerp(-halfTrackSizeY, halfTrackSizeY, nearLocalPos.y);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Container = new GameObject("Container");
			Container.transform.SetParent(gameObject.transform, false);
			Container.AddComponent<TreeUpdater>();
			
			Track = BuildTrack();
			HandleButton = BuildButton("Handle");
			JumpButton = BuildButton("Jump");

			HandleButton.SizeY = 2;
			JumpButton.SizeY = 1;

			Track.InsetL = 1;
			Track.InsetR = 1;
			
			RendererUtil.SetActiveWithUpdate(JumpButton.Canvas, false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaFillRectSlider BuildTrack() {
			var trackGo = new GameObject("Track");
			trackGo.transform.SetParent(Container.transform, false);
			return trackGo.AddComponent<HoverAlphaFillRectSlider>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaRendererRectButton BuildButton(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverAlphaRendererRectButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateControl() {
			Track.Controllers.Set(HoverFillRectSlider.SegmentInfoListName, this);
			Track.Controllers.Set(HoverFillRectSlider.TickInfoListName, this);
			Track.Controllers.Set(HoverFillRectSlider.SizeXName, this);
			Track.Controllers.Set(HoverAlphaFillRectSlider.AlphaName, this);
			Track.Controllers.Set(HoverFill.SortingLayerName, this);
			
			HandleButton.Controllers.Set("Transform.localPosition", this);
			HandleButton.Controllers.Set(HoverAlphaRendererRectButton.SizeXName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererRectButton.IsEnabledName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererRectButton.EnabledAlphaName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererRectButton.DisabledAlphaName, this);
			HandleButton.Controllers.Set(HoverAlphaRendererRectButton.SortingLayerName, this);
			
			JumpButton.Controllers.Set("Transform.localPosition", this);
			JumpButton.Controllers.Set(HoverAlphaRendererRectButton.SizeXName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererRectButton.IsEnabledName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererRectButton.EnabledAlphaName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererRectButton.DisabledAlphaName, this);
			JumpButton.Controllers.Set(HoverAlphaRendererRectButton.SortingLayerName, this);
			
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
			
			Controllers.Set(SizeXName, cont);
			Controllers.Set(SizeYName, cont);
			Controllers.Set(AlphaName, cont);
			Controllers.Set(IsEnabledName, cont);
			Controllers.Set(ZeroValueName, cont);
			Controllers.Set(HandleValueName, cont);
			Controllers.Set(JumpValueName, cont);
			Controllers.Set(AllowJumpName, cont);
			Controllers.Set(TickCountName, cont);
			Controllers.Set(FillStartingPointName, cont);
			Controllers.Set(SortingLayerName, cont);
			
			HandleButton.Fill.Controllers.Set(HoverFillRectButton.HighlightProgressName, cont);
			HandleButton.Fill.Controllers.Set(HoverFillRectButton.SelectionProgressName, cont);
			JumpButton.Fill.Controllers.Set(HoverFillRectButton.HighlightProgressName, cont);
			JumpButton.Fill.Controllers.Set(HoverFillRectButton.SelectionProgressName, cont);
			
			HandleButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);
			JumpButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", cont);
			
			HandleButton.Canvas.Label.Controllers.Set("Text.text", cont);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSegments() {
			var info = new SliderUtil.SliderInfo {
				FillType = FillStartingPoint,
				TrackStartPosition = -SizeY/2,
				TrackEndPosition = SizeY/2,
				HandleSize = HandleButton.SizeY,
				HandleValue = HandleValue,
				JumpSize = (AllowJump ? JumpButton.SizeY : 0),
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
				
				HoverAlphaRendererRectButton button = (isHandle ? HandleButton : JumpButton);
				button.transform.localPosition = 
					new Vector3(0, (segInfo.StartPosition+segInfo.EndPosition)/2, 0);
				
				if ( isJump ) {
					isJumpSegmentVisible = true;
				}
			}
			
			HandleButton.SizeX = SizeX;
			JumpButton.SizeX = SizeX;
			Track.SizeX = SizeX;

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
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorTypeWithCustom.Custom ) {
				return;
			}
			
			Vector2 anchorPos = RendererUtil.GetRelativeAnchorPosition(Anchor);
			var localPos = new Vector3(SizeX*anchorPos.x, SizeY*anchorPos.y, 0);
			
			Container.transform.localPosition = localPos;
		}
		
	}

}
