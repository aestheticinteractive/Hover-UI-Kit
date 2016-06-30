using System;
using Hover.Items;
using Hover.Items.Managers;
using Hover.Items.Types;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	[RequireComponent(typeof(HoverItemSelectionState))]
	public class HoverRendererUpdater : MonoBehaviour, ITreeUpdateable, 
															ISettingsController, IProximityProvider {

		public const string ButtonRendererName = "_ButtonRenderer";
		public const string SliderRendererName = "_SliderRenderer";

		public ISettingsControllerMap Controllers { get; private set; }
		
		public bool IsButtonRendererType { get; protected set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject ButtonRendererPrefab;

		[DisableWhenControlled]
		public GameObject SliderRendererPrefab;

		[SerializeField]
		[DisableWhenControlled]
		protected Component _ButtonRenderer;

		[SerializeField]
		[DisableWhenControlled]
		protected Component _SliderRenderer;

		[DisableWhenControlled]
		public bool ShowProximityDebugLines = true;

		[DisableWhenControlled]
		public bool ClickToRebuildRenderer = false;

		private GameObject vPrevButtonPrefab;
		private GameObject vPrevSliderPrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRendererUpdater() {
			Controllers = new SettingsControllerMap();
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererButton ButtonRenderer {
			get { return (_ButtonRenderer as HoverRendererButton); }
			set { _ButtonRenderer = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		private IRendererSlider SliderRenderer {
			get { return (_SliderRenderer as IRendererSlider); }
			set { _SliderRenderer = (Component)value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			ButtonRendererPrefab = Resources.Load<GameObject>(
				"Prefabs/HoverAlphaButtonRenderer-Default");
			SliderRendererPrefab = Resources.Load<GameObject>(
				"Prefabs/HoverAlphaSliderRenderer-Default");

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
			HoverRenderer activeRenderer = (ButtonRenderer /*?? SliderRenderer*/);

			UpdateRenderer(activeRenderer, hoverItem);
			UpdateRendererCanvas(activeRenderer, hoverItem);
			UpdateRendererIndicator(activeRenderer, hoverItem, highState, selState);

			if ( SliderRenderer != null ) {
				UpdateSliderSettings(hoverItem);
				UpdateSliderSettings(hoverItem, highState);
			}

			if ( ShowProximityDebugLines && Application.isPlaying ) {
				DrawProximityDebugLines();
			}

			Controllers.TryExpireControllers();
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
				//TODO: SliderRenderer = (SliderRenderer ?? FindOrBuildSlider());
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

		/*--------------------------------------------------------------------------------------------* /
		private IRendererSlider FindOrBuildSlider() {
			return RendererUtil.FindOrBuildRenderer<HoverAlphaRendererArcSlider>(
				gameObject.transform, SliderRendererPrefab, "Slider", 
				typeof(HoverAlphaRendererArcSlider));
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
		protected void UpdateRenderer(HoverRenderer pRenderer, HoverItem pHoverItem) {
			pRenderer.Controllers.Set(HoverRenderer.IsEnabledName, this);
			pRenderer.IsEnabled = pHoverItem.Data.IsEnabled;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateRendererCanvas(HoverRenderer pRenderer, HoverItem pHoverItem) {
			HoverCanvasDataUpdater canvasUp = pRenderer.GetCanvasDataUpdater();

			if ( canvasUp == null ) {
				return;
			}

			HoverItemData data = pHoverItem.Data;
			ICheckboxItem checkboxData = (data as ICheckboxItem);
			IRadioItem radioData = (data as IRadioItem);
			ISelectorItem selectorData = (data as ISelectorItem);
			IStickyItem stickyData = (data as IStickyItem);
			ISliderItem sliderData = (data as ISliderItem);
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
		private void UpdateRendererIndicator(HoverRenderer pRenderer, HoverItem pHoverItem,					
								HoverItemHighlightState pHighState, HoverItemSelectionState pSelState) {
			HoverIndicator rendInd = pRenderer.GetIndicator();
			
			rendInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			rendInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			rendInd.HighlightProgress = pHighState.MaxHighlightProgress;
			rendInd.SelectionProgress = pSelState.SelectionProgress;

			pRenderer.IsNearestToCursor = pHighState.IsNearestAcrossAllItemsForAnyCursor;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSettings(HoverItem pHoverItem) {
			ISliderItem data = (ISliderItem)pHoverItem.Data;

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
				SliderRenderer.HandleValue = showValue;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawProximityDebugLines() {
			HoverItemHighlightState.Highlight? nearHigh = 
				GetComponent<HoverItemHighlightState>().NearestHighlight;

			if ( nearHigh == null ) {
				return;
			}

			Vector3 cursorPos = nearHigh.Value.Cursor.WorldPosition;
			Vector3 nearPos = nearHigh.Value.NearestWorldPos;
			float prog = nearHigh.Value.Progress;
			Color color = (prog >= 1 ? new Color(0.3f, 1, 0.4f, 1) : 
				(prog <= 0 ? new Color(1, 0, 0, 0.25f) : new Color(1, 1, 0, prog*0.5f+0.25f)));

			Debug.DrawLine(nearPos, cursorPos, color);
		}

	}

}
