using System.Collections.Generic;
using Hover.Common.Items.Types;
using Hover.Common.Renderers.Fills;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectangleSlider : MonoBehaviour, IProximityProvider, ISettingsController {

		//TODO: tick marks (use canvas RQ + hide when obscured by buttons)
		
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string ZeroValueName = "ZeroValue";
		public const string HandleValueName = "HandleValue";
		public const string JumpValueName = "JumpValue";
		public const string AllowJumpName = "AllowJump";
		public const string FillStartingPointName = "FillStartingPoint";

		public ISettingsControllerMap Controllers { get; private set; }
	
		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject Container;

		[DisableWhenControlled]
		public HoverRendererFillSliderTrack Track;

		[DisableWhenControlled]
		public HoverRendererRectangleButton HandleButton;

		[DisableWhenControlled]
		public HoverRendererRectangleButton JumpButton;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeX = 10;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeY = 10;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float Alpha = 1;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float ZeroValue = 0.5f;
				
		[Range(0, 1)]
		[DisableWhenControlled]
		public float HandleValue = 0.5f;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float JumpValue = 0;
		
		[DisableWhenControlled]
		public bool AllowJump = false;

		[DisableWhenControlled]
		public SliderItem.FillType FillStartingPoint = SliderItem.FillType.Zero;
		
		[DisableWhenControlled]
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		private readonly List<SliderUtil.Segment> vSegmentInfoList;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererRectangleSlider() {
			Controllers = new SettingsControllerMap();
			vSegmentInfoList = new List<SliderUtil.Segment>();
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
		public void Update() {
			SizeY = Mathf.Max(SizeY, HandleButton.SizeY);

			UpdateSliderSegments();
			UpdateGeneralSettings();
			UpdateAnchorSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( AllowJump ) {
				return RendererHelper.GetNearestWorldPositionOnRectangle(
					pFromWorldPosition, Container.transform, SizeX, SizeY);
			}
			
			return RendererHelper.GetNearestWorldPositionOnRectangle(
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
			
			Track = BuildTrack();
			HandleButton = BuildButton("Handle");
			JumpButton = BuildButton("Jump");
			
			HandleButton.SizeY = 2;
			JumpButton.SizeY = 1;

			Track.InsetL = 1;
			Track.InsetR = 1;
			
			RendererHelper.SetActiveWithUpdate(JumpButton.Canvas, false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererFillSliderTrack BuildTrack() {
			var trackGo = new GameObject("Track");
			trackGo.transform.SetParent(Container.transform, false);
			return trackGo.AddComponent<HoverRendererFillSliderTrack>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererRectangleButton BuildButton(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererRectangleButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
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
			};
			
			SliderUtil.CalculateSegments(info, vSegmentInfoList);
			Track.SegmentInfoList = vSegmentInfoList;
			
			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);
			
			foreach ( SliderUtil.Segment seg in vSegmentInfoList ) {
				Debug.Log(" - "+seg.Type+": "+seg.StartPosition+" / "+seg.EndPosition);
			}*/
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			Track.Controllers.Set(HoverRendererFillSliderTrack.SizeXName, this);
			Track.Controllers.Set(HoverRendererFillSliderTrack.AlphaName, this);

			HandleButton.Controllers.Set("Transform.localPosition", this);
			HandleButton.Controllers.Set(HoverRendererRectangleButton.SizeXName, this);
			HandleButton.Controllers.Set(HoverRendererRectangleButton.AlphaName, this);
			
			JumpButton.Controllers.Set("GameObject.activeSelf", this);
			JumpButton.Controllers.Set("Transform.localPosition", this);
			JumpButton.Controllers.Set(HoverRendererRectangleButton.SizeXName, this);
			JumpButton.Controllers.Set(HoverRendererRectangleButton.AlphaName, this);
			
			bool isJumpSegmentVisible = false;
			
			foreach ( SliderUtil.Segment segInfo in vSegmentInfoList ) {
				bool isHandle = (segInfo.Type == SliderUtil.SegmentType.Handle);
				bool isJump = (segInfo.Type == SliderUtil.SegmentType.Jump);
				
				if ( !isHandle && !isJump ) {
					continue;
				}
				
				HoverRendererRectangleButton button = (isHandle ? HandleButton : JumpButton);
				button.transform.localPosition = 
					new Vector3(0, (segInfo.StartPosition+segInfo.EndPosition)/2, 0);
				
				if ( isJump ) {
					isJumpSegmentVisible = true;
				}
			}
			
			HandleButton.SizeX = SizeX;
			JumpButton.SizeX = SizeX;
			Track.SizeX = SizeX;

			HandleButton.Alpha = Alpha;
			JumpButton.Alpha = Alpha;
			Track.Alpha = Alpha;

			RendererHelper.SetActiveWithUpdate(JumpButton, (AllowJump && isJumpSegmentVisible));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			Vector2 anchorPos = RendererHelper.GetRelativeAnchorPosition(Anchor);
			var localPos = new Vector3(SizeX*anchorPos.x, SizeY*anchorPos.y, 0);
			
			Container.transform.localPosition = localPos;
		}
		
	}

}
