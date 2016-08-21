using Hover.Core.Items;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Shapes.Arc;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;
using Hover.Core.Renderers.Contents;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverkeyBuilder : MonoBehaviour {

		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;
		public bool ClickToBuild = false;


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
		public void Update() {
			if ( !ClickToBuild ) {
				return;
			}

			ClickToBuild = false;
			BuilderUtil.FindOrAddHoverManagerPrefab();
			PerformBuild();
			DestroyImmediate(this, false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			gameObject.AddComponent<HoverkeyManager>();

			BuildMain();
			BuildFunctions();

			TreeUpdater treeUp = gameObject.AddComponent<TreeUpdater>();
			treeUp.Update(); //force entire interface update 
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
			rectRow.InnerPadding = 0.0175f;

			GameObject esctGo = BuildRowItem(funcGo.transform, HoverkeyBuilderData.FuncRow[0]);
			HoverLayoutRectRelativeSizer escSizer = esctGo.AddComponent<HoverLayoutRectRelativeSizer>();

			escSizer.RelativeSizeX = 0.25f;

			BuildRow("Row0", funcGo.transform, HoverkeyBuilderData.FuncRow, 1, 4);
			BuildRow("Row1", funcGo.transform, HoverkeyBuilderData.FuncRow, 5, 4);
			BuildRow("Row2", funcGo.transform, HoverkeyBuilderData.FuncRow, 9, 4);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildRow(string pName, Transform pParentTx, HoverkeyBuilderKeyInfo[] pKeys,
														int pStartIndex=0, int pCount=int.MaxValue) {
			var rowGo = new GameObject(pName);
			rowGo.transform.SetParent(pParentTx, false);

			rowGo.AddComponent<TreeUpdater>();
			rowGo.AddComponent<HoverLayoutRectRow>();

			for ( int i = pStartIndex ; i < pKeys.Length && i < pStartIndex+pCount ; i++ ) {
				BuildRowItem(rowGo.transform, pKeys[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private GameObject BuildRowItem(Transform pRowTx, HoverkeyBuilderKeyInfo pKeyInfo) {
			var itemGo = new GameObject(pKeyInfo.DefaultKey+"");
			itemGo.transform.SetParent(pRowTx, false);

			HoverItemBuilder build = itemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = pKeyInfo.ItemType;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.SliderRendererPrefab = SliderRendererPrefab;
			build.PerformBuild();

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

			HoverkeyManager inter = gameObject.GetComponent<HoverkeyManager>();
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
