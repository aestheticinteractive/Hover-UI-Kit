using System;
using Hover.Common.Items;
using Hover.Common.Items.Managers;
using Hover.Common.Items.Types;
using Hover.Common.Layouts;
using Hover.Common.Renderers.Rect.Button;
using Hover.Common.Renderers.Rect.Slider;
using Hover.Common.Renderers.Shared.Bases;
using Hover.Common.Renderers.Shared.Contents;
using Hover.Common.Renderers.Shared.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverRendererRectController : HoverRendererController, 
															IRectangleLayoutElement, ITreeUpdateable {
	
		public const string ButtonRendererName = "_ButtonRenderer";
		public const string SliderRendererName = "_SliderRenderer";
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public bool IsButtonRendererType { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject ButtonRendererPrefab; //TODO: check for prefab changes, then rebuild

		[DisableWhenControlled]
		public GameObject SliderRendererPrefab;

		[SerializeField]
		[DisableWhenControlled]
		private Component _ButtonRenderer;

		[SerializeField]
		[DisableWhenControlled]
		private Component _SliderRenderer;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;

		[DisableWhenControlled(RangeMin=0.05f, RangeMax=0.9f)]
		public float DisabledAlpha = 0.35f;
		
		[DisableWhenControlled]
		public string SortingLayer = "Default";

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHoverRendererRectButton ButtonRenderer {
			get { return (_ButtonRenderer as IHoverRendererRectButton); }
			set { _ButtonRenderer = (Component)value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IHoverRendererRectSlider SliderRenderer {
			get { return (_SliderRenderer as IHoverRendererRectSlider); }
			set { _SliderRenderer = (Component)value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverRendererRectangleButtonStandard");
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverRendererRectangleSliderStandard");
				_IsBuilt = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			
			HoverItem hoverItem = GetComponent<HoverItem>();
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemSelectionState selState = GetComponent<HoverItemSelectionState>();

			TryRebuildWithItemType(hoverItem.ItemType);

			if ( ButtonRenderer != null ) {
				ButtonRenderer.RendererController = this;
				UpdateButtonSettings(hoverItem);
				UpdateButtonSettings(highState);
				UpdateButtonSettings(selState);
			}

			if ( SliderRenderer != null ) {
				SliderRenderer.RendererController = this;
				UpdateSliderSettings(hoverItem);
				UpdateSliderSettings(hoverItem, highState);
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
		public void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(SizeXName, pController);
			Controllers.Set(SizeYName, pController);

			SizeX = pSizeX;
			SizeY = pSizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UnsetLayoutSize(ISettingsController pController) {
			Controllers.Unset(SizeXName, pController);
			Controllers.Unset(SizeYName, pController);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryRebuildWithItemType(HoverItem.HoverItemType pType) {
			if ( pType == HoverItem.HoverItemType.Slider ) {
				Controllers.Set(ButtonRendererName, this);
				Controllers.Unset(SliderRendererName, this);

				RendererHelper.DestroyRenderer(ButtonRenderer);
				ButtonRenderer = null;
				SliderRenderer = UseOrFindOrBuildSlider();
				IsButtonRendererType = false;
			}
			else {
				Controllers.Set(SliderRendererName, this);
				Controllers.Unset(ButtonRendererName, this);

				RendererHelper.DestroyRenderer(SliderRenderer);
				SliderRenderer = null;
				ButtonRenderer = UseOrFindOrBuildButton();
				IsButtonRendererType = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IHoverRendererRectButton UseOrFindOrBuildButton() {
			if ( ButtonRenderer != null ) {
				return ButtonRenderer;
			}

			return RendererHelper.FindOrBuildRenderer<IHoverRendererRectButton>(
				gameObject.transform, ButtonRendererPrefab, "Button", 
				typeof(HoverRendererRectButton));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IHoverRendererRectSlider UseOrFindOrBuildSlider() {
			if ( SliderRenderer != null ) {
				return SliderRenderer;
			}

			return RendererHelper.FindOrBuildRenderer<HoverRendererRectSlider>(
				gameObject.transform, SliderRendererPrefab, "Slider", 
				typeof(HoverRendererRectSlider));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItem pHoverItem) {
			HoverItemData data = pHoverItem.Data;
			ICheckboxItem checkboxData = (data as ICheckboxItem);
			IRadioItem radioData = (data as IRadioItem);
			IParentItem parentData = (data as IParentItem);
			IStickyItem stickyData = (data as IStickyItem);
			
			ButtonRenderer.SizeX = SizeX;
			ButtonRenderer.SizeY = SizeY;
			ButtonRenderer.Alpha = (data.IsEnabled ? 1 : DisabledAlpha);
			ButtonRenderer.SortingLayer = SortingLayer;
			ButtonRenderer.LabelText = data.Label;

			if ( checkboxData != null ) {
				ButtonRenderer.IconOuterType = HoverRendererIcon.IconOffset.CheckOuter;
				ButtonRenderer.IconInnerType = (checkboxData.Value ?
					HoverRendererIcon.IconOffset.CheckInner : HoverRendererIcon.IconOffset.None);
			}
			else if ( radioData != null ) {
				ButtonRenderer.IconOuterType = HoverRendererIcon.IconOffset.RadioOuter;
				ButtonRenderer.IconInnerType = (radioData.Value ?
					HoverRendererIcon.IconOffset.RadioInner : HoverRendererIcon.IconOffset.None);
			}
			else if ( parentData != null ) {
				ButtonRenderer.IconOuterType = HoverRendererIcon.IconOffset.Parent;
				ButtonRenderer.IconInnerType = HoverRendererIcon.IconOffset.None;
			}
			else if ( stickyData != null ) {
				ButtonRenderer.IconOuterType = HoverRendererIcon.IconOffset.Sticky;
				ButtonRenderer.IconInnerType = HoverRendererIcon.IconOffset.None;
			}
			else {
				ButtonRenderer.IconOuterType = HoverRendererIcon.IconOffset.None;
				ButtonRenderer.IconInnerType = HoverRendererIcon.IconOffset.None;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItem pHoverItem) {
			ISliderItem data = (ISliderItem)pHoverItem.Data;

			SliderRenderer.SizeX = SizeX;
			SliderRenderer.SizeY = SizeY;
			SliderRenderer.Alpha = (data.IsEnabled ? 1 : DisabledAlpha);
			SliderRenderer.LabelText = data.GetFormattedLabel(data);
			SliderRenderer.HandleValue = data.SnappedValue;
			SliderRenderer.FillStartingPoint = data.FillStartingPoint;
			SliderRenderer.ZeroValue = Mathf.InverseLerp(data.RangeMin, data.RangeMax, 0);
			SliderRenderer.AllowJump = data.AllowJump;
			SliderRenderer.TickCount = data.Ticks;
			SliderRenderer.SortingLayer = SortingLayer;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemHighlightState pHighState) {
			ButtonRenderer.HighlightProgress = pHighState.MaxHighlightProgress;
			ButtonRenderer.ShowEdge = pHighState.IsNearestAcrossAllItemsForAnyCursor;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(
									HoverItem pHoverItem, HoverItemHighlightState pHighState) {
			SliderItem data = (SliderItem)pHoverItem.Data;
			HoverItemHighlightState.Highlight? high = pHighState.NearestHighlight;
			float highProg = pHighState.MaxHighlightProgress;

			SliderRenderer.HighlightProgress = highProg;

			if ( high == null || highProg <= 0 ) {
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
			ButtonRenderer.SelectionProgress = pSelState.SelectionProgress;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItemSelectionState pSelState) {
			float selProg = pSelState.SelectionProgress;
			
			SliderRenderer.SelectionProgress = selProg;
			SliderRenderer.SelectionProgress = selProg;
		}
		
	}

}
