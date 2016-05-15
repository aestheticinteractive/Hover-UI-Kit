using System;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Helpers;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemData))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverRendererRectangleController : HoverRendererController, ISettingsController {
	
		public bool IsButtonRendererType { get; private set; }

		public HoverRendererRectangleButton ButtonRenderer;
		public HoverRendererRectangleSlider SliderRenderer;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;

		[Range(0.05f, 0.9f)]
		public float DisabledAlpha = 0.35f;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();
			HoverItemData hoverItemData = GetComponent<HoverItemData>();
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemSelectionState selState = GetComponent<HoverItemSelectionState>();

			TryRebuildWithItemType(hoverItemData.ItemType);

			if ( ButtonRenderer != null ) {
				UpdateButtonSettings(hoverItemData);
				UpdateButtonSettings(highState);
				UpdateButtonSettings(selState);
			}

			if ( SliderRenderer != null ) {
				UpdateSliderSettings(hoverItemData);
				UpdateSliderSettings(hoverItemData, highState);
				UpdateSliderSettings(selState);
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( ButtonRenderer != null ) {
				return ButtonRenderer.GetNearestWorldPosition(pFromWorldPosition);
			}

			if ( SliderRenderer != null ) {
				return SliderRenderer.GetNearestWorldPosition(pFromWorldPosition);
			}

			throw new Exception("No button or slider renderer.");
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryRebuildWithItemType(HoverItemData.HoverItemType pType) {
			if ( pType == HoverItemData.HoverItemType.Slider ) {
				ButtonRenderer = RendererHelper.DestroyRenderer(ButtonRenderer);
				SliderRenderer = UseOrFindOrBuildSlider();
				IsButtonRendererType = false;
			}
			else {
				SliderRenderer = RendererHelper.DestroyRenderer(SliderRenderer);
				ButtonRenderer = UseOrFindOrBuildButton();
				IsButtonRendererType = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererRectangleButton UseOrFindOrBuildButton() {
			if ( ButtonRenderer != null ) {
				return ButtonRenderer;
			}

			HoverRendererRectangleButton existingButton = RendererHelper
				.FindInImmediateChildren<HoverRendererRectangleButton>(gameObject.transform);

			if ( existingButton != null ) {
				return existingButton;
			}

			var buttonGo = new GameObject("ButtonRenderer");
			buttonGo.transform.SetParent(gameObject.transform, false);
			return buttonGo.AddComponent<HoverRendererRectangleButton>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererRectangleSlider UseOrFindOrBuildSlider() {
			if ( SliderRenderer != null ) {
				return SliderRenderer;
			}

			HoverRendererRectangleSlider existingSlider = RendererHelper
				.FindInImmediateChildren<HoverRendererRectangleSlider>(gameObject.transform);

			if ( existingSlider != null ) {
				return existingSlider;
			}

			var sliderGo = new GameObject("SliderRenderer");
			sliderGo.transform.SetParent(gameObject.transform, false);
			return sliderGo.AddComponent<HoverRendererRectangleSlider>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemData pHoverItemData) {
			BaseItem data = pHoverItemData.Data;
			ICheckboxItem checkboxData = (data as ICheckboxItem);
			IRadioItem radioData = (data as IRadioItem);
			IParentItem parentData = (data as IParentItem);
			IStickyItem stickyData = (data as IStickyItem);
			
			HoverRendererIcon iconOuter = ButtonRenderer.Canvas.IconOuter;
			HoverRendererIcon iconInner = ButtonRenderer.Canvas.IconInner;

			ButtonRenderer.ParentController = this;
			ButtonRenderer.SizeX = SizeX;
			ButtonRenderer.SizeY = SizeY;
			ButtonRenderer.Alpha = (data.IsEnabled ? 1 : DisabledAlpha);

			ButtonRenderer.Canvas.Label.TextComponent.text = data.Label;

			if ( checkboxData != null ) {
				iconOuter.IconType = HoverRendererIcon.IconOffset.CheckOuter;
				iconInner.IconType = (checkboxData.Value ?
					HoverRendererIcon.IconOffset.CheckInner : HoverRendererIcon.IconOffset.None);
			}
			else if ( radioData != null ) {
				iconOuter.IconType = HoverRendererIcon.IconOffset.RadioOuter;
				iconInner.IconType = (radioData.Value ?
					HoverRendererIcon.IconOffset.RadioInner : HoverRendererIcon.IconOffset.None);
			}
			else if ( parentData != null ) {
				iconOuter.IconType = HoverRendererIcon.IconOffset.Parent;
				iconInner.IconType = HoverRendererIcon.IconOffset.None;
			}
			else if ( stickyData != null ) {
				iconOuter.IconType = HoverRendererIcon.IconOffset.Sticky;
				iconInner.IconType = HoverRendererIcon.IconOffset.None;
			}
			else {
				iconOuter.IconType = HoverRendererIcon.IconOffset.None;
				iconInner.IconType = HoverRendererIcon.IconOffset.None;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItemData pHoverItemData) {
			ISliderItem data = (ISliderItem)pHoverItemData.Data;
			HoverRendererCanvas handleCanvas = SliderRenderer.HandleButton.Canvas;

			SliderRenderer.ParentController = this;
			SliderRenderer.SizeX = SizeX;
			SliderRenderer.SizeY = SizeY;
			SliderRenderer.Alpha = (data.IsEnabled ? 1 : DisabledAlpha);

			handleCanvas.Label.TextComponent.text = data.GetFormattedLabel(data);
			handleCanvas.IconOuter.IconType = HoverRendererIcon.IconOffset.Slider;
			handleCanvas.IconInner.IconType = HoverRendererIcon.IconOffset.None;

			SliderRenderer.HandleValue = data.SnappedValue;
			SliderRenderer.FillStartingPoint = data.FillStartingPoint;
			SliderRenderer.ZeroValue = Mathf.InverseLerp(data.RangeMin, data.RangeMax, 0);
			SliderRenderer.AllowJump = data.AllowJump;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemHighlightState pHighState) {
			HoverItemHighlightState.Highlight? high = pHighState.NearestHighlight;
			
			ButtonRenderer.Fill.HighlightProgress = pHighState.MaxHighlightProgress;
			ButtonRenderer.Fill.Edge.gameObject.SetActive(
				pHighState.IsNearestAcrossAllItemsForAnyCursor);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(
									HoverItemData pHoverItemData, HoverItemHighlightState pHighState) {
			ISliderItem data = (ISliderItem)pHoverItemData.Data;
			HoverItemHighlightState.Highlight? high = pHighState.NearestHighlight;
			float highProg = pHighState.MaxHighlightProgress;

			SliderRenderer.HandleButton.Fill.HighlightProgress = highProg;
			SliderRenderer.JumpButton.Fill.HighlightProgress = highProg;
			
			SliderRenderer.HandleButton.Fill.Edge.gameObject.SetActive(
				pHighState.IsNearestAcrossAllItemsForAnyCursor);
			SliderRenderer.JumpButton.Fill.Edge.gameObject.SetActive(
				pHighState.IsNearestAcrossAllItemsForAnyCursor);

			if ( high == null ) {
				data.HoverValue = null;
				SliderRenderer.JumpValue = -1;
				return;
			}
			
			float value = SliderRenderer.GetValueViaNearestWorldPosition(high.Value.NearestWorldPos);
			
			data.HoverValue = value;
			
			float snapValue = (float)data.SnappedHoverValue;
			//float easePower = (1-high.Value.Progress)*5+1; //gets "snappier" as you pull away
			float showValue = DisplayUtil.GetEasedValue(data.Snaps, value, snapValue, 3);
				
			SliderRenderer.JumpValue = showValue;
			
			if ( data.IsStickySelected ) {
				data.Value = value;
				SliderRenderer.HandleValue = showValue;
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemSelectionState pSelState) {
			ButtonRenderer.Fill.SelectionProgress = pSelState.SelectionProgress;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItemSelectionState pSelState) {
			float selProg = pSelState.SelectionProgress;
			
			SliderRenderer.HandleButton.Fill.SelectionProgress = selProg;
			SliderRenderer.JumpButton.Fill.SelectionProgress = selProg;
		}
		
	}

}
