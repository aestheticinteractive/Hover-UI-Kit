using System.Collections.Generic;
using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Helpers;
using Hover.Common.Items.Types;
using Hover.Common.Renderers;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectangleSlider : MonoBehaviour, IProximityProvider {

		//TODO: tick marks (use canvas RQ + hide when obscured by buttons)

		public bool ControlledByItem { get; set; }
	
		public GameObject Container;
		public HoverRendererFillSliderTrack Track;
		public HoverRendererRectangleButton HandleButton;
		public HoverRendererRectangleButton JumpButton;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float Alpha = 1;
		
		[Range(0, 1)]
		public float ZeroValue = 0.5f;
				
		[Range(0, 1)]
		public float HandleValue = 0.5f;
		
		[Range(0, 1)]
		public float JumpValue = 0;
		
		public bool AllowJump = false;
		public SliderItem.FillType FillStartingPoint = SliderItem.FillType.Zero;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		
		private readonly List<SliderUtil.Segment> vSegmentInfoList;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererRectangleSlider() {
			vSegmentInfoList = new List<SliderUtil.Segment>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !ControlledByItem ) {
				UpdateAfterParent();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterParent() {
			UpdateSliderSegments();
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			Track.UpdateAfterRenderer();
			HandleButton.UpdateAfterParent();
			JumpButton.UpdateAfterParent();
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
			
			JumpButton.Canvas.gameObject.SetActive(false);
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
			Track.ControlledByRenderer = true;
			HandleButton.ControlledByRenderer = true;
			JumpButton.ControlledByRenderer = true;
			
			bool isJumpSegmentVisible = false;
			
			foreach ( SliderUtil.Segment segInfo in vSegmentInfoList ) {
				bool isHandle = (segInfo.Type == SliderUtil.SegmentType.Handle);
				bool isJump = (segInfo.Type == SliderUtil.SegmentType.Jump);
				
				if ( !isHandle && !isJump ) {
					continue;
				}
				
				HoverRendererRectangleButton button = (isHandle ? HandleButton : JumpButton);
				button.SizeY = segInfo.EndPosition-segInfo.StartPosition;
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

			JumpButton.gameObject.SetActive(AllowJump && isJumpSegmentVisible);
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
