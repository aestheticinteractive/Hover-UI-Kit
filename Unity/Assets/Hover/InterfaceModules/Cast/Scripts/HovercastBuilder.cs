using Hover.Core.Cursors;
using Hover.Core.Items;
using Hover.Core.Items.Helpers;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes.Arc;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastBuilder : MonoBehaviour {

		public bool AttachToLeftHand = true;
		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;
		public bool AutoRotateHandToFaceCamera = true;
		public bool IncludeExampleRows = true;

		[TriggerButton("Build Hovercast Interface")]
		public bool ClickToBuild;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ButtonRendererPrefab == null ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaButtonArcRenderer-Default");
			}

			if ( SliderRendererPrefab == null ) {
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaSliderArcRenderer-Default");
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
			HoverCursorFollower follow = gameObject.AddComponent<HoverCursorFollower>();
			HovercastInterface inter = gameObject.AddComponent<HovercastInterface>();

			////

			var adjustGo = new GameObject("TransformAdjuster");
			adjustGo.transform.SetParent(transform, false);
			adjustGo.transform.localPosition = new Vector3(0, 0, 0.02f);
			adjustGo.transform.localRotation = Quaternion.Euler(0, 180, 80);

			var openItemGo = new GameObject("OpenItem");
			openItemGo.transform.SetParent(adjustGo.transform, false);
			openItemGo.transform.localPosition = new Vector3(0, -0.05f, 0);

			var titleItemGo = new GameObject("TitleItem");
			titleItemGo.transform.SetParent(adjustGo.transform, false);

			var backItemGo = new GameObject("BackItem");
			backItemGo.transform.SetParent(adjustGo.transform, false);

			var rowContGo = new GameObject("Rows");
			rowContGo.transform.SetParent(adjustGo.transform, false);
			rowContGo.AddComponent<TreeUpdater>();

			var row0Go = new GameObject("Root");
			row0Go.transform.SetParent(rowContGo.transform, false);

			////

			BuildOpenItem(openItemGo);
			BuildTitleItem(titleItemGo);
			BuildBackItem(backItemGo);
			BuildRow(row0Go, "Hovercast");

			if ( IncludeExampleRows ) {
				BuildExampleRows(rowContGo, row0Go);
			}

			adjustGo.AddComponent<TreeUpdater>(); //after building items

			////

			follow.CursorType = (AttachToLeftHand ? CursorType.LeftPalm : CursorType.RightPalm);
			follow.ObjectsToActivate = new[] { openItemGo, titleItemGo, backItemGo, rowContGo };

			inter.RowContainer = rowContGo.transform;
			inter.ActiveRow = row0Go.GetComponent<HoverLayoutArcRow>();
			inter.OpenItem = openItemGo.GetComponent<HoverItemDataSelector>();
			inter.TitleItem = titleItemGo.GetComponent<HoverItemDataText>();
			inter.BackItem = backItemGo.GetComponent<HoverItemDataSelector>();

			HovercastOpenTransitioner openTrans = gameObject.AddComponent<HovercastOpenTransitioner>();

			HovercastRowTransitioner rowTrans = gameObject.AddComponent<HovercastRowTransitioner>();
			rowTrans.RowEntryTransition = HovercastRowSwitchingInfo.RowEntryType.FromInside;

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				inter.OnOpenToggledEvent, openTrans.OnOpenToggled);
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				inter.OnRowSwitchedEvent, rowTrans.OnRowSwitched);
#else
			inter.OnOpenToggledEvent.AddListener(openTrans.OnOpenToggled);
			inter.OnRowSwitchedEvent.AddListener(rowTrans.OnRowSwitched);
#endif

			gameObject.AddComponent<HovercastHighlightPreventer>();

			gameObject.AddComponent<HovercastBackCursorTrigger>();

			HovercastActiveDirection actDir = gameObject.AddComponent<HovercastActiveDirection>();
			actDir.ChildForActivation = adjustGo;

			HovercastMirrorSwitcher mirror = gameObject.AddComponent<HovercastMirrorSwitcher>();
			mirror.UseMirrorLayout = !AttachToLeftHand;

			gameObject.AddComponent<HovercastAlphaUpdater>();

			if ( AutoRotateHandToFaceCamera ) {
				HoverCursorDataProvider curDataProv = FindObjectOfType<HoverCursorDataProvider>();
				ICursorDataForInput curData = curDataProv.GetCursorDataForInput(follow.CursorType);
				float addRotZ = 80*(AttachToLeftHand ? 1 : -1);

				actDir.TreeUpdate(); //forces search for the "facing" transform
				curData.transform.LookAt(actDir.ActiveWhenFacingTransform, Vector3.up);
				curData.transform.localRotation *= Quaternion.Euler(0, 0, addRotZ);
			}

			follow.Update(); //moves interface to the correct cursor transform
			treeUp.Update(); //forces entire interface to update
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildOpenItem(GameObject pItemGo) {
			HoverItemBuilder build = pItemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = HoverItem.HoverItemType.Selector;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.OnEditorTriggerButtonSelected();
			DestroyImmediate(build, false);

			////

			HovercastInterface inter = gameObject.GetComponent<HovercastInterface>();

			HoverItemDataSelector data = pItemGo.GetComponent<HoverItemDataSelector>();
			data.Id = "HovercastOpenItem";
			data.Label = "";

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				data.OnSelectedEvent, inter.OnOpenToggled);
#else
			data.OnSelectedEvent.AddListener(inter.OnRowSwitched);
#endif

			HoverFillButtonArcUpdater fillArcUp = 
				pItemGo.GetComponentInChildren<HoverFillButtonArcUpdater>();
			fillArcUp.EdgePosition = HoverFillButtonArcUpdater.EdgePositionType.Outer;

			HoverCanvas can = pItemGo.GetComponentInChildren<HoverCanvas>();

			var iconPivotGo = new GameObject("IconPivot");
			iconPivotGo.transform.SetParent(can.transform, false);

			var iconOpenGo = new GameObject("IconOpen");
			iconOpenGo.transform.SetParent(iconPivotGo.transform, false);

			var iconCloseGo = new GameObject("IconClose");
			iconCloseGo.transform.SetParent(iconPivotGo.transform, false);

			RectTransform iconPivotRectTx = iconPivotGo.AddComponent<RectTransform>();
			iconPivotRectTx.localPosition = new Vector3(-47.5f, 0, 0);
			iconPivotRectTx.localScale = new Vector3(2.2f, 2.2f, 1);
			iconPivotRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
			iconPivotRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

			RectTransform iconOpenRectTx = iconOpenGo.AddComponent<RectTransform>();
			iconOpenRectTx.localPosition = new Vector3(19, 0, 0);
			iconOpenRectTx.localRotation = Quaternion.Euler(0, 0, -45);
			iconOpenRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
			iconOpenRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);

			RectTransform iconCloseRectTx = iconCloseGo.AddComponent<RectTransform>();
			iconCloseRectTx.localPosition = new Vector3(19, 0, 0);
			iconCloseRectTx.localRotation = Quaternion.Euler(0, 0, -45);
			iconCloseRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
			iconCloseRectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);

			RawImage openImage = iconOpenGo.AddComponent<RawImage>();
			openImage.material = Resources.Load<Material>("Materials/HovercastIconsMaterial");
			openImage.uvRect = new Rect(0.002f, 0.002f, 0.496f, 0.996f);

			RawImage closeImage = iconCloseGo.AddComponent<RawImage>();
			closeImage.material = openImage.material;
			closeImage.uvRect = new Rect(0.502f, 0.002f, 0.496f, 0.996f);

			////

			HoverShapeArc shapeArc = pItemGo.AddComponent<HoverShapeArc>();
			shapeArc.OuterRadius = 0.02f;
			shapeArc.InnerRadius = 0;
			shapeArc.ArcDegrees = 360;

			HovercastOpenIcons icons = pItemGo.AddComponent<HovercastOpenIcons>();
			icons.OpenIcon = iconOpenGo;
			icons.CloseIcon = iconCloseGo;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildTitleItem(GameObject pItemGo) {
			HoverItemBuilder build = pItemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = HoverItem.HoverItemType.Text;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.OnEditorTriggerButtonSelected();
			DestroyImmediate(build, false);

			////

			HoverFillButtonArcUpdater fillArcUp =
				pItemGo.GetComponentInChildren<HoverFillButtonArcUpdater>();
			fillArcUp.EdgePosition = HoverFillButtonArcUpdater.EdgePositionType.Outer;

			HoverItemDataText data = pItemGo.GetComponent<HoverItemDataText>();
			data.Id = "HovercastTitleItem";
			data.Label = "Hovercast";

			HoverShapeArc shapeArc = pItemGo.AddComponent<HoverShapeArc>();
			shapeArc.OuterRadius = 0.075f;
			shapeArc.InnerRadius = 0.015f;
			shapeArc.ArcDegrees = 90;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildBackItem(GameObject pItemGo) {
			HoverItemBuilder build = pItemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = HoverItem.HoverItemType.Selector;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.OnEditorTriggerButtonSelected();
			DestroyImmediate(build, false);

			////

			HovercastInterface inter = gameObject.GetComponent<HovercastInterface>();

			HoverItemDataSelector data = pItemGo.GetComponent<HoverItemDataSelector>();
			data.Id = "HovercastBackItem";
			data.Label = "";
			data.Action = SelectorActionType.NavigateOut;

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				data.OnSelectedEvent, inter.OnRowSwitched);
#else
			data.OnSelectedEvent.AddListener(inter.OnRowSwitched);
#endif

			HoverFillButtonArcUpdater fillArcUp =
				pItemGo.GetComponentInChildren<HoverFillButtonArcUpdater>();
			fillArcUp.EdgePosition = HoverFillButtonArcUpdater.EdgePositionType.Outer;

			HoverCanvas can = pItemGo.GetComponentInChildren<HoverCanvas>();
			can.Alignment = HoverCanvas.CanvasAlignmentType.Custom;
			can.IconOuter.transform.localPosition = new Vector3(-40, 0, 0);

			HoverShapeArc shapeArc = pItemGo.AddComponent<HoverShapeArc>();
			shapeArc.OuterRadius = 0.015f;
			shapeArc.InnerRadius = 0;
			shapeArc.ArcDegrees = 360;

			HovercastRowSwitchingInfo rowSwitch = pItemGo.AddComponent<HovercastRowSwitchingInfo>();
			rowSwitch.NavigateBack = true;
			rowSwitch.RowEntryTransition = HovercastRowSwitchingInfo.RowEntryType.FromInside;

			pItemGo.AddComponent<HoverIndicatorOverrider>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildExampleRows(GameObject pContainerGo, GameObject pRootRowGo) {
			var row1Go = new GameObject("RowA");
			row1Go.transform.SetParent(pContainerGo.transform, false);
			BuildRow(row1Go, "Row A");

			var row2Go = new GameObject("RowB");
			row2Go.transform.SetParent(pContainerGo.transform, false);
			BuildRow(row2Go, "Row B");

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
		private void BuildRow(GameObject pRowGo, string pRowTitle) {
			pRowGo.AddComponent<TreeUpdater>();

			HoverLayoutArcRow layoutArc = pRowGo.AddComponent<HoverLayoutArcRow>();
			layoutArc.ArcDegrees = 90;
			layoutArc.Padding.Between = 0.15f;

			HovercastRowTitle title = pRowGo.AddComponent<HovercastRowTitle>();
			title.RowTitle = pRowTitle;
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

			itemGo.AddComponent<HoverShapeArc>();

			HoverItemData data = itemGo.GetComponent<HoverItemData>();
			data.Id = pId;
			data.Label = pLabel;

			return itemGo;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRowNavItem(GameObject pRowGo, string pId, string pLabel, 
																			GameObject pNavToRowGo) {
			GameObject itemGo = BuildRowItem(pRowGo, pId, pLabel, HoverItem.HoverItemType.Selector);
			HovercastInterface inter = gameObject.GetComponent<HovercastInterface>();
			bool isBack = (pNavToRowGo == null);

			HoverItemDataSelector data = itemGo.GetComponent<HoverItemDataSelector>();
			data.Action = (isBack ? SelectorActionType.NavigateOut : SelectorActionType.NavigateIn);

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(
				data.OnSelectedEvent, inter.OnRowSwitched);
#else
			data.OnSelectedEvent.AddListener(inter.OnRowSwitched);
#endif

			HovercastRowSwitchingInfo rowSwitch = itemGo.AddComponent<HovercastRowSwitchingInfo>();
			rowSwitch.NavigateBack = isBack;
			rowSwitch.NavigateToRow = (isBack ? null : pNavToRowGo.GetComponent<HoverLayoutArcRow>());
			rowSwitch.RowEntryTransition = (isBack ? 
				HovercastRowSwitchingInfo.RowEntryType.FromInside : 
				HovercastRowSwitchingInfo.RowEntryType.FromOutside
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
