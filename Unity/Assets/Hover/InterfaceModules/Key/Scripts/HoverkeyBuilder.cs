using Hover.Core.Items;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Shapes.Arc;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;

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
					"Prefabs/HoverAlphaButtonArcRenderer-Default");
			}

			if ( SliderRendererPrefab == null ) {
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaSliderArcRenderer-Default");
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
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMain() {
			var fullGo = new GameObject("Main");
			fullGo.transform.SetParent(gameObject.transform, false);
			fullGo.transform.localPosition = new Vector3(0.01125f, 0, 0);

			fullGo.AddComponent<TreeUpdater>();

			HoverLayoutRectRow rectRow = fullGo.AddComponent<HoverLayoutRectRow>();
			rectRow.Arrangement = HoverLayoutRectRow.ArrangementType.TopToBottom;
			rectRow.SizeX = 0.4425f;
			rectRow.SizeY = 0.15f;

			BuildRow("Row0", fullGo.transform, HoverkeyBuilderData.MainRow0);
			BuildRow("Row1", fullGo.transform, HoverkeyBuilderData.MainRow1);
			BuildRow("Row2", fullGo.transform, HoverkeyBuilderData.MainRow2);
			BuildRow("Row3", fullGo.transform, HoverkeyBuilderData.MainRow3);
			BuildRow("Row4", fullGo.transform, HoverkeyBuilderData.MainRow4);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildRow(string pName, Transform pParentTx, HoverkeyBuilderKeyInfo[] pKeys) {
			var rowGo = new GameObject(pName);
			rowGo.transform.SetParent(pParentTx, false);

			rowGo.AddComponent<TreeUpdater>();
			rowGo.AddComponent<HoverLayoutRectRow>();

			foreach ( HoverkeyBuilderKeyInfo keyInfo in pKeys ) {
				BuildRowItem(rowGo.transform, keyInfo);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRowItem(Transform pRowTx, HoverkeyBuilderKeyInfo pKeyInfo) {
			var itemGo = new GameObject(pKeyInfo.DefaultKey+"");
			itemGo.transform.SetParent(pRowTx, false);

			HoverItemBuilder build = itemGo.AddComponent<HoverItemBuilder>();
			build.ItemType = pKeyInfo.ItemType;
			build.ButtonRendererPrefab = ButtonRendererPrefab;
			build.SliderRendererPrefab = SliderRendererPrefab;
			build.PerformBuild();

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
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildRowSliderItem(GameObject pRowGo, string pId, string pLabel) {
			GameObject sliderItemGo =
				BuildRowItem(pRowGo, "Row2-ItemA", "BA", HoverItem.HoverItemType.Slider);


			HoverItemDataSlider data = sliderItemGo.GetComponent<HoverItemDataSlider>();
			data.Value = 0.825f;
			data.Ticks = 5;
			data.AllowJump = true;
		}

	}

}
