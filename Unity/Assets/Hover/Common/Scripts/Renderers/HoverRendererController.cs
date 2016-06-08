using System;
using Hover.Common.Items;
using Hover.Common.Items.Managers;
using Hover.Common.Items.Types;
using Hover.Common.Renderers.Contents;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItem))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	[RequireComponent(typeof(HoverItemSelectionState))]
	public abstract class HoverRendererController : MonoBehaviour,
											ISettingsController, IProximityProvider, ITreeUpdateable {
		
		public const string ButtonRendererName = "_ButtonRenderer";
		public const string SliderRendererName = "_SliderRenderer";

		public ISettingsControllerMap Controllers { get; private set; }
		public abstract string DefaultButtonPrefabResourcePath { get; }
		public abstract string DefaultSliderPrefabResourcePath { get; }
		
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
		public string SortingLayer = "Default";

		[DisableWhenControlled]
		public bool ShowProximityDebugLines = true;
		
		[DisableWhenControlled]
		public bool ClickToRebuildRenderer = false;

		[HideInInspector]
		[SerializeField]
		protected bool _IsBuilt;

		private GameObject vPrevButtonPrefab;
		private GameObject vPrevSliderPrefab;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRendererController() {
			Controllers = new SettingsControllerMap();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IRendererButton ButtonRenderer {
			get { return (_ButtonRenderer as IRendererButton); }
			set { _ButtonRenderer = (Component)value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IRendererSlider SliderRenderer {
			get { return (_SliderRenderer as IRendererSlider); }
			set { _SliderRenderer = (Component)value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(DefaultButtonPrefabResourcePath);
				SliderRendererPrefab = Resources.Load<GameObject>(DefaultSliderPrefabResourcePath);
				_IsBuilt = true;
			}

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
			HoverItemHighlightState highState = GetComponent<HoverItemHighlightState>();
			HoverItemSelectionState selState = GetComponent<HoverItemSelectionState>();

			DestroyRenderersIfNecessary();
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
		protected abstract IRendererButton FindOrBuildButton();
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract IRendererSlider FindOrBuildSlider();


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
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateButtonSettings(HoverItem pHoverItem) {
			HoverItemData data = pHoverItem.Data;
			ICheckboxItem checkboxData = (data as ICheckboxItem);
			IRadioItem radioData = (data as IRadioItem);
			IParentItem parentData = (data as IParentItem);
			IStickyItem stickyData = (data as IStickyItem);
			
			ButtonRenderer.IsEnabled = data.IsEnabled;
			ButtonRenderer.SortingLayer = SortingLayer;
			ButtonRenderer.LabelText = data.Label;

			if ( checkboxData != null ) {
				ButtonRenderer.IconOuterType = HoverIcon.IconOffset.CheckOuter;
				ButtonRenderer.IconInnerType = (checkboxData.Value ?
					HoverIcon.IconOffset.CheckInner : HoverIcon.IconOffset.None);
			}
			else if ( radioData != null ) {
				ButtonRenderer.IconOuterType = HoverIcon.IconOffset.RadioOuter;
				ButtonRenderer.IconInnerType = (radioData.Value ?
					HoverIcon.IconOffset.RadioInner : HoverIcon.IconOffset.None);
			}
			else if ( parentData != null ) {
				ButtonRenderer.IconOuterType = HoverIcon.IconOffset.Parent;
				ButtonRenderer.IconInnerType = HoverIcon.IconOffset.None;
			}
			else if ( stickyData != null ) {
				ButtonRenderer.IconOuterType = HoverIcon.IconOffset.Sticky;
				ButtonRenderer.IconInnerType = HoverIcon.IconOffset.None;
			}
			else {
				ButtonRenderer.IconOuterType = HoverIcon.IconOffset.None;
				ButtonRenderer.IconInnerType = HoverIcon.IconOffset.None;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSliderSettings(HoverItem pHoverItem) {
			ISliderItem data = (ISliderItem)pHoverItem.Data;

			SliderRenderer.IsEnabled = data.IsEnabled;
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
		protected virtual void UpdateButtonSettings(HoverItemHighlightState pHighState) {
			ButtonRenderer.HighlightProgress = pHighState.MaxHighlightProgress;
			ButtonRenderer.ShowEdge = pHighState.IsNearestAcrossAllItemsForAnyCursor;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSliderSettings(
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
		protected virtual void UpdateButtonSettings(HoverItemSelectionState pSelState) {
			ButtonRenderer.SelectionProgress = pSelState.SelectionProgress;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSliderSettings(HoverItemSelectionState pSelState) {
			float selProg = pSelState.SelectionProgress;
			
			SliderRenderer.SelectionProgress = selProg;
			SliderRenderer.SelectionProgress = selProg;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void DrawProximityDebugLines() {
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
