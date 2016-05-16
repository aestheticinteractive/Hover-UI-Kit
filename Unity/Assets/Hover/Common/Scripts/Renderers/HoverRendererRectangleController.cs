using System;
using Hover.Common.Items;
using Hover.Common.Items.Managers;
using Hover.Common.Items.Types;
using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Fills;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

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
		public void Awake() {
			Update();
		}
		
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

			ButtonRenderer.Controllers.Set(HoverRendererRectangleButton.SizeXName, this);
			ButtonRenderer.Controllers.Set(HoverRendererRectangleButton.SizeYName, this);
			ButtonRenderer.Controllers.Set(HoverRendererRectangleButton.AlphaName, this);
			ButtonRenderer.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.HighlightProgressName, this);
			ButtonRenderer.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.SelectionProgressName, this);
			ButtonRenderer.Fill.Edge.Controllers.Set("GameObject.activeSelf", this);
			ButtonRenderer.Canvas.Label.Controllers.Set("Text.text", this);
			iconOuter.Controllers.Set(HoverRendererIcon.IconTypeName, this);
			iconInner.Controllers.Set(HoverRendererIcon.IconTypeName, this);

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
			HoverRendererIcon handleIconOuter = handleCanvas.IconOuter;
			HoverRendererIcon handleIconInner = handleCanvas.IconInner;

			SizeY = Mathf.Max(SizeY, SliderRenderer.HandleButton.SizeY);

			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.SizeXName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.SizeYName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.AlphaName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.ZeroValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.HandleValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.JumpValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.AllowJumpName, this);
			SliderRenderer.Controllers.Set(HoverRendererRectangleSlider.FillStartingPointName, this);
			SliderRenderer.HandleButton.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.HighlightProgressName, this);
			SliderRenderer.HandleButton.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.SelectionProgressName, this);
			SliderRenderer.HandleButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", this);
			SliderRenderer.JumpButton.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.HighlightProgressName, this);
			SliderRenderer.JumpButton.Fill.Controllers
				.Set(HoverRendererFillRectangleFromCenter.SelectionProgressName, this);
			SliderRenderer.JumpButton.Fill.Edge.Controllers.Set("GameObject.activeSelf", this);
			handleCanvas.Label.Controllers.Set("Text.text", this);
			handleIconOuter.Controllers.Set(HoverRendererIcon.IconTypeName, this);
			handleIconInner.Controllers.Set(HoverRendererIcon.IconTypeName, this);

			SliderRenderer.SizeX = SizeX;
			SliderRenderer.SizeY = SizeY;
			SliderRenderer.Alpha = (data.IsEnabled ? 1 : DisabledAlpha);

			handleCanvas.Label.TextComponent.text = data.GetFormattedLabel(data);
			handleIconOuter.IconType = HoverRendererIcon.IconOffset.Slider;
			handleIconInner.IconType = HoverRendererIcon.IconOffset.None;

			SliderRenderer.HandleValue = data.SnappedValue;
			SliderRenderer.FillStartingPoint = data.FillStartingPoint;
			SliderRenderer.ZeroValue = Mathf.InverseLerp(data.RangeMin, data.RangeMax, 0);
			SliderRenderer.AllowJump = data.AllowJump;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemHighlightState pHighState) {
			ButtonRenderer.Fill.HighlightProgress = pHighState.MaxHighlightProgress;

			RendererHelper.SetActiveWithUpdate(ButtonRenderer.Fill.Edge,
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
			
			RendererHelper.SetActiveWithUpdate(SliderRenderer.HandleButton.Fill.Edge,
				pHighState.IsNearestAcrossAllItemsForAnyCursor);
			RendererHelper.SetActiveWithUpdate(SliderRenderer.JumpButton.Fill.Edge,
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
