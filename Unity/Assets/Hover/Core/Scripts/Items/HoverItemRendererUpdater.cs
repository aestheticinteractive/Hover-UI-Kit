using System;
using Hover.Core.Items.Managers;
using Hover.Core.Items.Types;
using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	[RequireComponent(typeof(HoverItemSelectionState))]
	public class HoverItemRendererUpdater : MonoBehaviour, ITreeUpdateable, 
															ISettingsController, IProximityProvider {

		public const string ButtonRendererName = "_ButtonRenderer";
		public const string SliderRendererName = "_SliderRenderer";

		public ISettingsControllerMap Controllers { get; private set; }
		
		public bool IsButtonRendererType { get; protected set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public GameObject ButtonRendererPrefab;

		[DisableWhenControlled]
		public GameObject SliderRendererPrefab;

		[SerializeField]
		[DisableWhenControlled]
		private Component _ButtonRenderer;

		[SerializeField]
		[DisableWhenControlled]
		private Component _SliderRenderer;

		[TriggerButton("Rebuild Item Renderer")]
		public bool ClickToRebuildRenderer;

		private GameObject vPrevButtonPrefab;
		private GameObject vPrevSliderPrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemRendererUpdater() {
			Controllers = new SettingsControllerMap();
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererButton ButtonRenderer {
			get { return (_ButtonRenderer as HoverRendererButton); }
			set { _ButtonRenderer = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererSlider SliderRenderer {
			get { return (_SliderRenderer as HoverRendererSlider); }
			set { _SliderRenderer = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverRenderer ActiveRenderer {
			get { return ((HoverRenderer)ButtonRenderer ?? SliderRenderer); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			/*ButtonRendererPrefab = Resources.Load<GameObject>(
				"Prefabs/HoverAlphaButtonRenderer-Default");
			SliderRendererPrefab = Resources.Load<GameObject>(
				"Prefabs/HoverAlphaSliderRenderer-Default");*/

			vPrevButtonPrefab = ButtonRendererPrefab;
			vPrevSliderPrefab = SliderRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			HoverItem hoverItem = GetComponent<HoverItem>();

			DestroyRenderersIfNecessary();
			TryRebuildWithItemType(hoverItem.ItemType);

			////

			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemSelectionState selState = GetComponent<HoverItemSelectionState>();
			HoverRenderer activeRenderer = ((HoverRenderer)ButtonRenderer ?? SliderRenderer);

			UpdateRenderer(activeRenderer, hoverItem);
			UpdateRendererCanvas(activeRenderer, hoverItem);
			UpdateRendererIndicator(activeRenderer, highState, selState);

			if ( ButtonRenderer != null ) {
				UpdateButtonSettings(highState);
			}

			if ( SliderRenderer != null ) {
				UpdateSliderSettings(hoverItem);
				UpdateSliderSettings(hoverItem, highState);
			}

			Controllers.TryExpireControllers();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			//do nothing here, check for (ClickToRebuildRenderer == true) elsewhere...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRenderersIfNecessary() {
			if ( ClickToRebuildRenderer || ButtonRendererPrefab != vPrevButtonPrefab ) {
				vPrevButtonPrefab = ButtonRendererPrefab;
				RendererUtil.DestroyRenderer(ButtonRenderer);
				ButtonRenderer = null;
			}

			if ( ClickToRebuildRenderer || SliderRendererPrefab != vPrevSliderPrefab ) {
				vPrevSliderPrefab = SliderRendererPrefab;
				RendererUtil.DestroyRenderer(SliderRenderer);
				SliderRenderer = null;
			}

			ClickToRebuildRenderer = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryRebuildWithItemType(HoverItem.HoverItemType pType) {
			if ( pType == HoverItem.HoverItemType.Slider ) {
				Controllers.Set(ButtonRendererName, this);
				Controllers.Unset(SliderRendererName, this);

				RendererUtil.DestroyRenderer(ButtonRenderer);
				ButtonRenderer = null;
				SliderRenderer = (SliderRenderer ?? FindOrBuildSlider());
				IsButtonRendererType = false;
			}
			else {
				Controllers.Set(SliderRendererName, this);
				Controllers.Unset(ButtonRendererName, this);

				RendererUtil.DestroyRenderer(SliderRenderer);
				SliderRenderer = null;
				ButtonRenderer = (ButtonRenderer ?? FindOrBuildButton());
				IsButtonRendererType = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererButton FindOrBuildButton() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererButton>(gameObject.transform, 
				ButtonRendererPrefab, "Button", typeof(HoverRendererButton));
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererSlider FindOrBuildSlider() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererSlider>(gameObject.transform,
				SliderRendererPrefab, "Slider", typeof(HoverRendererSlider));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			if ( ButtonRenderer != null ) {
				return ButtonRenderer.GetNearestWorldPosition(pFromWorldPosition);
			}

			if ( SliderRenderer != null ) {
				return SliderRenderer.GetNearestWorldPosition(pFromWorldPosition);
			}

			throw new Exception("No button or slider renderer.");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			if ( ButtonRenderer != null ) {
				return ButtonRenderer.GetNearestWorldPosition(pFromWorldRay, out pRaycast);
			}

			if ( SliderRenderer != null ) {
				return SliderRenderer.GetNearestWorldPosition(pFromWorldRay, out pRaycast);
			}

			throw new Exception("No button or slider renderer.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRenderer(HoverRenderer pRenderer, HoverItem pHoverItem) {
			pRenderer.Controllers.Set(HoverRenderer.IsEnabledName, this);
			pRenderer.IsEnabled = pHoverItem.Data.IsEnabled;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRendererCanvas(HoverRenderer pRenderer, HoverItem pHoverItem) {
			HoverCanvasDataUpdater canvasUp = pRenderer.GetCanvasDataUpdater();

			if ( canvasUp == null ) {
				return;
			}

			IItemData data = pHoverItem.Data;
			IItemDataCheckbox checkboxData = (data as IItemDataCheckbox);
			IItemDataRadio radioData = (data as IItemDataRadio);
			IItemDataSelector selectorData = (data as IItemDataSelector);
			IItemDataSticky stickyData = (data as IItemDataSticky);
			IItemDataSlider sliderData = (data as IItemDataSlider);
			var icon = HoverCanvasDataUpdater.IconPairType.Unspecified;

			if ( checkboxData != null ) {
				icon = (checkboxData.Value ? 
					HoverCanvasDataUpdater.IconPairType.CheckboxOn : 
					HoverCanvasDataUpdater.IconPairType.CheckboxOff);
			}
			else if ( radioData != null ) {
				icon = (radioData.Value ? 
					HoverCanvasDataUpdater.IconPairType.RadioOn : 
					HoverCanvasDataUpdater.IconPairType.RadioOff);
			}
			else if ( selectorData != null ) {
				if ( selectorData.Action == SelectorActionType.NavigateIn ) {
					icon = HoverCanvasDataUpdater.IconPairType.NavigateIn;
				}
				else if ( selectorData.Action == SelectorActionType.NavigateOut ) {
					icon = HoverCanvasDataUpdater.IconPairType.NavigateOut;
				}
			}
			else if ( stickyData != null ) {
				icon = HoverCanvasDataUpdater.IconPairType.Sticky;
			}
			else if ( sliderData != null ) {
				icon = HoverCanvasDataUpdater.IconPairType.Slider;
			}

			canvasUp.Controllers.Set(HoverCanvasDataUpdater.LabelTextName, this);
			canvasUp.Controllers.Set(HoverCanvasDataUpdater.IconTypeName, this);

			canvasUp.LabelText = (sliderData == null ? data.Label : 
				sliderData.GetFormattedLabel(sliderData));
			canvasUp.IconType = icon;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRendererIndicator(HoverRenderer pRenderer,
								HoverItemHighlightState pHighState, HoverItemSelectionState pSelState) {
			HoverIndicator rendInd = pRenderer.GetIndicator();
			
			rendInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			rendInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			rendInd.HighlightProgress = pHighState.MaxHighlightProgress;
			rendInd.SelectionProgress = pSelState.SelectionProgress;

			if ( pSelState.WasSelectedThisFrame ) {
				rendInd.LatestSelectionTime = DateTime.UtcNow;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonSettings(HoverItemHighlightState pHighState) {
			ButtonRenderer.Fill.Controllers.Set(HoverFillButton.ShowEdgeName, this);
			ButtonRenderer.Fill.ShowEdge = (pHighState.IsNearestAcrossAllItemsForAnyCursor && 
				pHighState.MaxHighlightProgress > 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItem pHoverItem) {
			IItemDataSlider data = (IItemDataSlider)pHoverItem.Data;

			SliderRenderer.Controllers.Set(HoverRendererSlider.HandleValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererSlider.FillStartingPointName, this);
			SliderRenderer.Controllers.Set(HoverRendererSlider.ZeroValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererSlider.AllowJumpName, this);
			SliderRenderer.Controllers.Set(HoverRendererSlider.TickCountName, this);

			SliderRenderer.HandleValue = data.SnappedValue;
			SliderRenderer.FillStartingPoint = data.FillStartingPoint;
			SliderRenderer.ZeroValue = Mathf.InverseLerp(data.RangeMin, data.RangeMax, 0);
			SliderRenderer.AllowJump = data.AllowJump;
			SliderRenderer.TickCount = data.Ticks;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItem pHoverItem, HoverItemHighlightState pHighState) {
			HoverItemDataSlider data = (HoverItemDataSlider)pHoverItem.Data;
			HoverItemHighlightState.Highlight? high = pHighState.NearestHighlight;
			float highProg = pHighState.MaxHighlightProgress;
			bool isNearest = pHighState.IsNearestAcrossAllItemsForAnyCursor;

			SliderRenderer.Controllers.Set(HoverRendererSlider.JumpValueName, this);
			SliderRenderer.Controllers.Set(HoverRendererSlider.ShowButtonEdgesName, this);

			SliderRenderer.ShowButtonEdges = (isNearest && highProg > 0);

			if ( high == null || highProg <= 0 || !isNearest ) {
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
				SliderRenderer.Controllers.Set(HoverRendererSlider.HandleValueName, this);
				SliderRenderer.HandleValue = showValue;
			}
		}

	}

}
