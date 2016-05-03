using System;
using System.Collections.Generic;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Meshes;
using Hover.Board.Renderers.Utils;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererSlider : MonoBehaviour {
	
		public GameObject Container;
		public HoverRendererFillSliderTrack Track;
		public HoverRendererButton HandleButton;
		public HoverRendererButton JumpButton;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float ZeroValue = 0.5f;
				
		[Range(0, 1)]
		public float HandleValue = 0.5f;
		
		[Range(0, 1)]
		public float JumpValue = 0;
		
		public bool ShowJump = false;
		public SliderItem.FillType FillType = SliderItem.FillType.Zero;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		
		private readonly List<SliderUtil.Segment> vSegmentInfoList;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererSlider() {
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
			UpdateSliderSegments();
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			Track.UpdateAfterRenderer();
			HandleButton.UpdateAfterRenderer();
			JumpButton.UpdateAfterRenderer();
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
			
			JumpButton.Canvas.gameObject.SetActive(false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererFillSliderTrack BuildTrack() {
			var trackGo = new GameObject("Track");
			trackGo.transform.SetParent(Container.transform, false);
			return trackGo.AddComponent<HoverRendererFillSliderTrack>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererButton BuildButton(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSegments() {
			var info = new SliderUtil.SliderInfo {
				FillType = FillType,
				TrackStartPosition = -SizeY/2,
				TrackEndPosition = SizeY/2,
				HandleSize = HandleButton.SizeY,
				HandleValue = HandleValue,
				JumpSize = (ShowJump ? JumpButton.SizeY : 0),
				JumpValue = JumpValue,
				ZeroValue = ZeroValue,
			};
			
			SliderUtil.CalculateSegments(info, vSegmentInfoList);
			Track.SegmentInfoList = vSegmentInfoList;
			
			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);
			
			foreach ( SliderUtil.Segment seg in vSegments ) {
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
				
				HoverRendererButton button = (isHandle ? HandleButton : JumpButton);
				button.SizeY = segInfo.EndPosition-segInfo.StartPosition;
				button.transform.localPosition = 
					new Vector3(0, (segInfo.StartPosition+segInfo.EndPosition)/2, 0);
				
				if ( isJump ) {
					isJumpSegmentVisible = true;
				}
			}
			
			Track.SizeX = SizeX*0.8f;
			
			JumpButton.gameObject.SetActive(ShowJump && isJumpSegmentVisible);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			int ai = (int)Anchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			var localPos = new Vector3(-SizeX*x, SizeY*y, 0);
			
			Container.transform.localPosition = localPos;
		}
		
	}

}
