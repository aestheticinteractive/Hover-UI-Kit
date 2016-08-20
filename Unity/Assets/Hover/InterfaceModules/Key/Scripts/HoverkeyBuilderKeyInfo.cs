using Hover.Core.Items;
using UnityEngine;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	public struct HoverkeyBuilderKeyInfo {

		public string ID;
		public HoverItem.HoverItemType ItemType;
		public HoverkeyItemLabels.KeyActionType ActionType;
		public KeyCode DefaultKey;
		public string DefaultLabel;
		public bool HasShiftLabel;
		public string ShiftLabel;
		public float RelativeSizeX;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverkeyBuilderKeyInfo(KeyCode pDefaultKey, 
							HoverkeyItemLabels.KeyActionType pActionType, string pDefaultLabel) {
			ID = "Hoverkey-"+pDefaultKey;
			ItemType = HoverItem.HoverItemType.Selector;
			ActionType = pActionType;
			DefaultKey = pDefaultKey;
			DefaultLabel = pDefaultLabel;
			HasShiftLabel = false;
			ShiftLabel = null;
			RelativeSizeX = 1;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverkeyBuilderKeyInfo Shift(string pShiftLabel) {
			HasShiftLabel = true;
			ShiftLabel = pShiftLabel;
			return this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverkeyBuilderKeyInfo RelSize(float pRelSizeX) {
			RelativeSizeX = pRelSizeX;
			return this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverkeyBuilderKeyInfo Checkbox() {
			ItemType = HoverItem.HoverItemType.Checkbox;
			return this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverkeyBuilderKeyInfo Sticky() {
			ItemType = HoverItem.HoverItemType.Sticky;
			return this;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static HoverkeyBuilderKeyInfo Char(KeyCode pKey, string pLabel) {
			return new HoverkeyBuilderKeyInfo(pKey, HoverkeyItemLabels.KeyActionType.Character, pLabel);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static HoverkeyBuilderKeyInfo Ctrl(KeyCode pKey, string pLabel) {
			return new HoverkeyBuilderKeyInfo(pKey, HoverkeyItemLabels.KeyActionType.Control, pLabel);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static HoverkeyBuilderKeyInfo Nav(KeyCode pKey, string pLabel) {
			return new HoverkeyBuilderKeyInfo(pKey, HoverkeyItemLabels.KeyActionType.Navigation, pLabel);
		}

	}

}
