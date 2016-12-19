using Hover.Core.Items;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverkeyBuilder : MonoBehaviour {

		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;

		[TriggerButton("Build Hoverkey Interface")]
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
			gameObject.AddComponent<HoverkeyInterface>();
			gameObject.AddComponent<HoverkeyTextInput>();

			BuildMain();
			BuildFunctions();
			BuildThree();
			BuildSix();
			BuildArrows();
			BuildNumpad();

			TreeUpdater treeUp = gameObject.AddComponent<TreeUpdater>();
			treeUp.Update(); //force entire interface update 
			DestroyImmediate(treeUp, false); //remove the updater
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMain() {
			var mainGo = new GameObject("Main");
			mainGo.transform.SetParent(gameObject.transform, false);
			mainGo.transform.localPosition = new Vector3(0.01125f, 0, 0);

			mainGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow rectRow = mainGo.AddComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
			rectRow.SizeX = 0.4425f;
			rectRow.SizeY = 0.15f;

			BuildRow("Row0", mainGo.transform, HoverkeyBuilderData.MainRow0);
			BuildRow("Row1", mainGo.transform, HoverkeyBuilderData.MainRow1);
			BuildRow("Row2", mainGo.transform, HoverkeyBuilderData.MainRow2);
			BuildRow("Row3", mainGo.transform, HoverkeyBuilderData.MainRow3);
			BuildRow("Row4", mainGo.transform, HoverkeyBuilderData.MainRow4);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildFunctions() {
			var funcGo = new GameObject("Functions");
			funcGo.transform.SetParent(gameObject.transform, false);
			funcGo.transform.localPosition = new Vector3(0.01125f, 0.105f, 0);

			funcGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow rectRow = funcGo.AddComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.LeftToRight;
			rectRow.SizeX = 0.4425f;
			rectRow.SizeY = 0.03f;
			rectRow.Padding.Between = 0.0175f;

			BuildRowItem(funcGo.transform, HoverkeyBuilderData.FuncRow[0]);
			BuildRow("Row0", funcGo.transform, HoverkeyBuilderData.FuncRow, 1, 4);
			BuildRow("Row1", funcGo.transform, HoverkeyBuilderData.FuncRow, 5, 4);
			BuildRow("Row2", funcGo.transform, HoverkeyBuilderData.FuncRow, 9, 4);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildThree() {
			GameObject threeGo = BuildRow("Three", gameObject.transform, HoverkeyBuilderData.ThreeRow);
			threeGo.transform.localPosition = new Vector3(0.3f, 0.105f, 0);

			HoverLayoutRectRow rectRow = threeGo.GetComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.LeftToRight;
			rectRow.SizeX = 0.09f;
			rectRow.SizeY = 0.03f;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildSix() {
			var sixGo = new GameObject("Six");
			sixGo.transform.SetParent(gameObject.transform, false);
			sixGo.transform.localPosition = new Vector3(0.3f, 0.045f, 0);

			sixGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow rectRow = sixGo.AddComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
			rectRow.SizeX = 0.09f;
			rectRow.SizeY = 0.06f;

			BuildRow("Row0", sixGo.transform, HoverkeyBuilderData.SixRow0);
			BuildRow("Row1", sixGo.transform, HoverkeyBuilderData.SixRow1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildArrows() {
			var arrowsGo = new GameObject("Arrows");
			arrowsGo.transform.SetParent(gameObject.transform, false);
			arrowsGo.transform.localPosition = new Vector3(0.3f, -0.045f, 0);

			arrowsGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow rectRow = arrowsGo.AddComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
			rectRow.SizeX = 0.09f;
			rectRow.SizeY = 0.06f;

			GameObject row0Go = BuildRow("Row0", arrowsGo.transform, HoverkeyBuilderData.Arrows, 0, 1);
			BuildRow("Row1", arrowsGo.transform, HoverkeyBuilderData.Arrows, 1);

			HoverLayoutRectRelativeSizer row0Sizer = 
				row0Go.AddComponent<HoverLayoutRectRelativeSizer>();
			row0Sizer.RelativeSizeX = 0.333f;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildNumpad() {
			var numGo = new GameObject("Numpad");
			numGo.transform.SetParent(gameObject.transform, false);
			numGo.transform.localPosition = new Vector3(0.43f, 0, 0);

			numGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow numRectRow = numGo.AddComponent<HoverLayoutRectRow>();
			numRectRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
			numRectRow.SizeX = 0.12f;
			numRectRow.SizeY = 0.15f;

			BuildRow("Row0", numGo.transform, HoverkeyBuilderData.NumRow0);

			////

			var noKeys = new HoverkeyBuilderKeyInfo[0];

			GameObject botGo = BuildRow("Bottom", numGo.transform, noKeys);
			HoverLayoutRectRelativeSizer botRectSizer =
				botGo.AddComponent<HoverLayoutRectRelativeSizer>();
			botRectSizer.RelativeSizeY = 4;

			GameObject botLeftGo = BuildRow("Left", botGo.transform, noKeys);
			HoverLayoutRectRelativeSizer botLeftRectSizer =
				botLeftGo.AddComponent<HoverLayoutRectRelativeSizer>();
			botLeftRectSizer.RelativeSizeX = 3;
			HoverLayoutRectRow botLeftRow = botLeftGo.GetComponent<HoverLayoutRectRow>();
			botLeftRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;

			BuildRow("Row1", botLeftGo.transform, HoverkeyBuilderData.NumRow1);
			BuildRow("Row2", botLeftGo.transform, HoverkeyBuilderData.NumRow2);
			BuildRow("Row3", botLeftGo.transform, HoverkeyBuilderData.NumRow3);
			BuildRow("Row4", botLeftGo.transform, HoverkeyBuilderData.NumRow4);

			GameObject botRightGo = BuildRow("Right", botGo.transform, HoverkeyBuilderData.NumCol);
			HoverLayoutRectRow botRightRow = botRightGo.GetComponent<HoverLayoutRectRow>();
			botRightRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private GameObject BuildRow(string pName, Transform pParentTx, HoverkeyBuilderKeyInfo[] pKeys,
																int pStartIndex=0, int pCount=99999) {
			var rowGo = new GameObject(pName);
			rowGo.transform.SetParent(pParentTx, false);

			rowGo.AddComponent<TreeUpdater>();
			rowGo.AddComponent<HoverLayoutRectRow>();

			for ( int i = pStartIndex ; i < pKeys.Length && i < pStartIndex+pCount ; i++ ) {
				BuildRowItem(rowGo.transform, pKeys[i]);
			}

			return rowGo;
		}

		/*--------------------------------------------------------------------------------------------*/
		private GameObject BuildRowItem(Transform pRowTx, HoverkeyBuilderKeyInfo pKeyInfo) {
			var itemGo = new GameObject(pKeyInfo.DefaultKey+"");
			itemGo.transform.SetParent(pRowTx, false);

			HoverItemBuilder build = itemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = pKeyInfo.ItemType;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.SliderRendererPrefab = SliderRendererPrefab;
			build.OnEditorTriggerButtonSelected();
			DestroyImmediate(build, false);

			////

			HoverItemData data = itemGo.GetComponent<HoverItemData>();
			data.Id = pKeyInfo.ID;
			data.Label = pKeyInfo.DefaultLabel;

			itemGo.AddComponent<HoverShapeRect>();

			HoverkeyItemLabels labels = itemGo.AddComponent<HoverkeyItemLabels>();
			labels.ActionType = pKeyInfo.ActionType;
			labels.DefaultKey = pKeyInfo.DefaultKey;
			labels.DefaultLabel = pKeyInfo.DefaultLabel;
			labels.HasShiftLabel = pKeyInfo.HasShiftLabel;
			labels.ShiftLabel = pKeyInfo.ShiftLabel;

			if ( pKeyInfo.RelativeSizeX != 1 ) {
				HoverLayoutRectRelativeSizer sizer = 
					itemGo.AddComponent<HoverLayoutRectRelativeSizer>();
				sizer.RelativeSizeX = pKeyInfo.RelativeSizeX;
			}

			HoverCanvas can = itemGo.GetComponentInChildren<HoverCanvas>();
			can.Alignment = HoverCanvas.CanvasAlignmentType.Center;
			can.PaddingX = 0.001f;

			////

			HoverkeyInterface inter = gameObject.GetComponent<HoverkeyInterface>();
			HoverItemDataSelector selData = (data as HoverItemDataSelector);
			HoverItemDataSticky stickyData = (data as HoverItemDataSticky);
			HoverItemDataCheckbox checkboxData = (data as HoverItemDataCheckbox);

			if ( selData != null ) {
#if UNITY_EDITOR
				UnityEditor.Events.UnityEventTools.AddPersistentListener(
					selData.OnSelectedEvent, inter.HandleItemSelected);
#else
				selData.OnSelectedEvent.AddListener(inter.HandleItemSelected);
#endif
			}
			else if ( stickyData != null ) {
#if UNITY_EDITOR
				UnityEditor.Events.UnityEventTools.AddPersistentListener(
					stickyData.OnSelectedEvent, inter.HandleItemSelected);
				UnityEditor.Events.UnityEventTools.AddPersistentListener(
					stickyData.OnDeselectedEvent, inter.HandleItemDeselected);
#else
				stickyData.OnSelectedEvent.AddListener(inter.HandleItemSelected);
				stickyData.OnDeselectedEvent.AddListener(inter.HandleItemDeselected);
#endif
			}
			else if ( checkboxData != null ) {
#if UNITY_EDITOR
				UnityEditor.Events.UnityEventTools.AddPersistentListener(
					checkboxData.OnSelectedEvent, inter.HandleItemSelected);
				UnityEditor.Events.UnityEventTools.AddPersistentListener(
					checkboxData.OnValueChangedEvent, inter.HandleItemValueChanged);
#else
				checkboxData.OnSelectedEvent.AddListener(inter.HandleItemSelected);
				checkboxData.OnValueChangedEvent.AddListener(inter.HandleItemValueChanged);
#endif
			}

			return itemGo;
		}

	}

}
