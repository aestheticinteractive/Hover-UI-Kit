using System;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Helpers;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemData))]
	[RequireComponent(typeof(HoverItemCursorActivity))]
	public class HoverRendererRectangleController : HoverRendererController {
	
		public HoverRendererRectangleButton ButtonRenderer;
		public HoverRendererRectangleSlider SliderRenderer;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();
			HoverItemData hoverItemData = GetComponent<HoverItemData>();
			HoverItemCursorActivity hoverItemCursorAct = GetComponent<HoverItemCursorActivity>();
			HoverItemSelectionActivity hoverItemSelectAct = GetComponent<HoverItemSelectionActivity>();

			TryRebuildWithItemType(hoverItemData.ItemType);

			if ( ButtonRenderer != null ) {
				UpdateButtonSettings(hoverItemData);
				//UpdateButtonSettings(hoverItemCursorAct);
				UpdateButtonSettings(hoverItemSelectAct);
				ButtonRenderer.UpdateAfterParent();
			}

			if ( SliderRenderer != null ) {
				UpdateSliderSettings(hoverItemData);
				UpdateSliderSettings(hoverItemCursorAct);
				UpdateSliderSettings(hoverItemSelectAct);
				SliderRenderer.UpdateAfterParent();
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
			}
			else {
				SliderRenderer = RendererHelper.DestroyRenderer(SliderRenderer);
				ButtonRenderer = UseOrFindOrBuildButton();
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

			ButtonRenderer.ControlledByItem = true;
			ButtonRenderer.SizeX = SizeX;
			ButtonRenderer.SizeY = SizeY;

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

			SliderRenderer.ControlledByItem = true;
			SliderRenderer.SizeX = SizeX;
			SliderRenderer.SizeY = SizeY;

			handleCanvas.Label.TextComponent.text = data.GetFormattedLabel(data);
			handleCanvas.IconOuter.IconType = HoverRendererIcon.IconOffset.Slider;
			handleCanvas.IconInner.IconType = HoverRendererIcon.IconOffset.None;

			SliderRenderer.HandleValue = data.SnappedValue;
			SliderRenderer.FillStartingPoint = data.FillStartingPoint;
			SliderRenderer.ZeroValue = Mathf.InverseLerp(data.RangeMin, data.RangeMax, 0);
			SliderRenderer.ShowJump = data.AllowJump;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void UpdateButtonSettings(HoverItemCursorActivity pHoverItemCursorAct) {
			HoverItemCursorActivity.Highlight? high = pHoverItemCursorAct.NearestHighlight;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItemCursorActivity pHoverItemCursorAct) {
			HoverItemCursorActivity.Highlight? high = pHoverItemCursorAct.NearestHighlight;

			if ( high != null ) {
				SliderRenderer.SetJumpValueViaNearestWorldPosition(high.Value.NearestWorldPos);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemSelectionActivity pHoverItemSelectAct) {
			ButtonRenderer.Fill.HighlightProgress = pHoverItemSelectAct.MaxHighlightProgress;
			ButtonRenderer.Fill.SelectionProgress = pHoverItemSelectAct.SelectionProgress;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItemSelectionActivity pHoverItemSelectAct) {
			float highProg = pHoverItemSelectAct.MaxHighlightProgress;
			float selProg = pHoverItemSelectAct.SelectionProgress;
			
			SliderRenderer.HandleButton.Fill.HighlightProgress = highProg;
			SliderRenderer.HandleButton.Fill.SelectionProgress = selProg;
			
			if ( highProg > 0 ) {
				SliderRenderer.JumpButton.Fill.HighlightProgress = highProg;
				SliderRenderer.JumpButton.Fill.SelectionProgress = selProg;
			}
			else {
				SliderRenderer.ShowJump = false;
			}
		}
		
	}

}
