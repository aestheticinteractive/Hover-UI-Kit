using Hover.Core.Items;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverpanelBuilder : MonoBehaviour {

		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;
		public bool IncludeExampleRows = true;

		[TriggerButton("Build Hoverpanel Interface")]
		public bool ClickToBuild;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ButtonRendererPrefab == null ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaButtonRectRenderer-Default");
			}

			if ( SliderRendererPrefab == null ) {
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaSliderRectRenderer-Default");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			UnityUtil.FindOrAddHoverKitPrefab();
			PerformBuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToBuild ) {
				DestroyImmediate(this, false);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			TreeUpdater treeUp = gameObject.AddComponent<TreeUpdater>();
			HoverShapeRect shapeRect = gameObject.AddComponent<HoverShapeRect>();
			HoverpanelInterface inter = gameObject.AddComponent<HoverpanelInterface>();

			////
			
			treeUp.enabled = false;

			var row0Go = new GameObject("RowRoot");
			row0Go.transform.SetParent(gameObject.transform, false);
			BuildRow(row0Go);

			if ( IncludeExampleRows ) {
				BuildExampleRows(row0Go);
			}

			treeUp.enabled = true;

			////

			shapeRect.SizeX = 0.3f;
			shapeRect.SizeY = 0.2f;

			inter.ActiveRow = row0Go.GetComponent<HoverLayoutRectRow>();

			gameObject.AddComponent<HoverpanelRowSizer>();

			HoverpanelRowTransitioner rowTrans = gameObject.AddComponent<HoverpanelRowTransitioner>();
			rowTrans.RowEntryTransition = HoverpanelRowSwitchingInfo.RowEntryType.SlideFromTop;

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				inter.OnRowSwitchedEvent, rowTrans.OnRowSwitched);
#else
			inter.OnRowSwitchedEvent.AddListener(rowTrans.OnRowSwitched);
#endif

			gameObject.AddComponent<HoverpanelHighlightPreventer>();
			gameObject.AddComponent<HoverpanelActiveDirection>();
			gameObject.AddComponent<HoverpanelAlphaUpdater>();
			
			treeUp.Update(); //forces entire interface to update
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildExampleRows(GameObject pRootRowGo) {
			var row1Go = new GameObject("RowA");
			row1Go.transform.SetParent(gameObject.transform, false);
			BuildRow(row1Go);

			var row2Go = new GameObject("RowB");
			row2Go.transform.SetParent(gameObject.transform, false);
			BuildRow(row2Go);

			////

			BuildRowNavItem(pRootRowGo, "Row0-ItemA", "A", row1Go);
			BuildRowNavItem(pRootRowGo, "Row0-ItemB", "B", row2Go);
			BuildRowItem(pRootRowGo, "Row0-ItemC", "C", HoverItem.HoverItemType.Selector);
			BuildRowItem(pRootRowGo, "Row0-ItemD", "D", HoverItem.HoverItemType.Selector);

			BuildRowItem(row1Go, "Row1-ItemA", "AA", HoverItem.HoverItemType.Radio);
			BuildRowItem(row1Go, "Row1-ItemB", "AB", HoverItem.HoverItemType.Radio);
			BuildRowItem(row1Go, "Row1-ItemC", "AC", HoverItem.HoverItemType.Radio);
			BuildRowItem(row1Go, "Row1-ItemD", "AD", HoverItem.HoverItemType.Radio);
			BuildRowItem(row1Go, "Row1-ItemE", "AE", HoverItem.HoverItemType.Radio);
			BuildRowItem(row1Go, "Row1-ItemF", "AF", HoverItem.HoverItemType.Radio);
			BuildRowNavItem(row1Go, "Row1-ItemBack", "Back", null);

			BuildRowSliderItem(row2Go, "Row2-ItemA", "BA");
			BuildRowItem(row2Go, "Row2-ItemB", "BB", HoverItem.HoverItemType.Checkbox);
			BuildRowItem(row2Go, "Row2-ItemC", "BC", HoverItem.HoverItemType.Checkbox);
			BuildRowNavItem(row2Go, "Row2-ItemBack", "Back", null);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRow(GameObject pRowGo) {
			pRowGo.AddComponent<TreeUpdater>();
			pRowGo.AddComponent<HoverLayoutRectRow>();
		}

		/*--------------------------------------------------------------------------------------------*/
		private GameObject BuildRowItem(GameObject pRowGo, string pId, string pLabel,
																		HoverItem.HoverItemType pType) {
			var itemGo = new GameObject("Item"+pLabel);
			itemGo.transform.SetParent(pRowGo.transform, false);

			HoverItemBuilder build = itemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = pType;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.SliderRendererPrefab = SliderRendererPrefab;
			build.OnEditorTriggerButtonSelected();
			DestroyImmediate(build, false);

			itemGo.AddComponent<HoverShapeRect>();

			HoverItemData data = itemGo.GetComponent<HoverItemData>();
			data.Id = pId;
			data.Label = pLabel;

			return itemGo;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRowNavItem(GameObject pRowGo, string pId, string pLabel, 
																			GameObject pNavToRowGo) {
			GameObject itemGo = BuildRowItem(pRowGo, pId, pLabel, HoverItem.HoverItemType.Selector);
			HoverpanelInterface inter = gameObject.GetComponent<HoverpanelInterface>();
			bool isBack = (pNavToRowGo == null);

			HoverItemDataSelector data = itemGo.GetComponent<HoverItemDataSelector>();
			data.Action = (isBack ? SelectorActionType.NavigateOut : SelectorActionType.NavigateIn);

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				data.OnSelectedEvent, inter.OnRowSwitched);
#else
			data.OnSelectedEvent.AddListener(inter.OnRowSwitched);
#endif

			HoverpanelRowSwitchingInfo rowSwitch = itemGo.AddComponent<HoverpanelRowSwitchingInfo>();
			rowSwitch.NavigateBack = isBack;
			rowSwitch.NavigateToRow = (isBack ? null : pNavToRowGo.GetComponent<HoverLayoutRectRow>());
			rowSwitch.RowEntryTransition = (isBack ? 
				HoverpanelRowSwitchingInfo.RowEntryType.SlideFromFront : 
				HoverpanelRowSwitchingInfo.RowEntryType.SlideFromBack
			);

			if ( !isBack ) {
				HoverCanvas can = itemGo.GetComponentInChildren<HoverCanvas>();
				can.Alignment = HoverCanvas.CanvasAlignmentType.TextLeftAndIconRight;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRowSliderItem(GameObject pRowGo, string pId, string pLabel) {
			GameObject sliderItemGo =
				BuildRowItem(pRowGo, "Row2-ItemA", "BA", HoverItem.HoverItemType.Slider);

			HoverLayoutArcRelativeSizer sliderSizer =
				sliderItemGo.AddComponent<HoverLayoutArcRelativeSizer>();
			sliderSizer.RelativeArcDegrees = 3;

			HoverItemDataSlider data = sliderItemGo.GetComponent<HoverItemDataSlider>();
			data.Value = 0.825f;
			data.Ticks = 5;
			data.AllowJump = true;
		}

	}

}
