using System.Collections.Generic;
using System.ComponentModel;
using Hover.Core.Items.Managers;
using Hover.Core.Items.Types;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverItem : TreeUpdateableBehavior {

		public enum HoverItemType {
			Selector = 1,
			Sticky,
			Checkbox,
			Radio,
			Slider,
			Text
		}

		public delegate void ItemEvent(HoverItem pItem);
		public ItemEvent OnTypeChanged;

		[SerializeField]
		private HoverItemType _ItemType = HoverItemType.Selector;

		[SerializeField]
		private HoverItemData _Data;

		private readonly List<HoverItemData> vDataComponentBuffer;
		private HoverItemType vPrevItemType;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItem() {
			vDataComponentBuffer = new List<HoverItemData>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemType ItemType {
			get => _ItemType;
			set => this.UpdateValueWithTreeMessage(ref _ItemType, value, "ItemType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverItemData Data {
			get => _Data;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevItemType = _ItemType;

			BuildDataIfNeeded();
			UpdateItemsManager(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			UpdateWithLatestItemTypeIfNeeded();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void OnEnable() {
			base.OnEnable();
			HoverItemsManager.Instance?.SetItemActiveState(this, true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void OnDisable() {
			base.OnDisable();
			HoverItemsManager.Instance?.SetItemActiveState(this, false);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			UpdateItemsManager(false);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildDataIfNeeded() {
			if ( _Data == null ) {
				_Data = gameObject.GetComponent<HoverItemData>();
			}

			if ( _Data == null ) {
				_Data = BuildData(_ItemType);
			}
			else if ( FindDuplicateData() ) {
				_Data = Instantiate(_Data); //handle duplication via Unity editor
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithLatestItemTypeIfNeeded() {
			if ( _ItemType == vPrevItemType ) {
				return;
			}

			HoverItemData newData = BuildData(_ItemType);

			TransferData(newData);
			DestroyData(_Data, newData);

			_Data = newData;

			if ( OnTypeChanged != null ) {
				OnTypeChanged.Invoke(this);
			}

			vPrevItemType = _ItemType;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItemsManager(bool pAdd) {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverItemsManager itemsMan = HoverItemsManager.Instance;

			if ( itemsMan == null ) {
				return;
			}

			if ( pAdd ) {
				itemsMan.AddItem(this);
			}
			else {
				itemsMan.RemoveItem(this);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool FindDuplicateData() {
			HoverItem[] items = GetComponents<HoverItem>();

			for ( int i = 0 ; i < items.Length ; i++ ) {
				HoverItem item = items[i];

				if ( item != this && item.Data == _Data ) {
					return true;
				}
			}

			return false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TransferData(HoverItemData pDataToFill) {
			HoverItemData oldData = _Data;
			HoverItemData newData = pDataToFill;

			if ( oldData == null ) {
				return;
			}

			newData.AutoId = oldData.AutoId;
			newData.Id = oldData.Id;
			newData.Label = oldData.Label;
			newData.IsEnabled = oldData.IsEnabled;

			HoverItemDataSelectable oldSelData = (oldData as HoverItemDataSelectable);
			HoverItemDataSelectable newSelData = (newData as HoverItemDataSelectable);

			if ( oldSelData == null || newSelData == null ) {
				return;
			}

			newSelData.OnSelectedEvent = oldSelData.OnSelectedEvent;
			newSelData.OnDeselectedEvent = oldSelData.OnDeselectedEvent;
			//newSelData.OnSelected += oldSelData.OnSelected;
			//newSelData.OnDeselected += oldSelData.OnDeselected;
			
			HoverItemDataSelectableBool oldSelBoolData = (oldData as HoverItemDataSelectableBool);
			HoverItemDataSelectableBool newSelBoolData = (newData as HoverItemDataSelectableBool);

			if ( oldSelBoolData != null && newSelBoolData != null ) {
				newSelBoolData.Value = oldSelBoolData.Value;
				newSelBoolData.OnValueChangedEvent = oldSelBoolData.OnValueChangedEvent;
				//newSelBoolData.OnValueChanged += oldSelBoolData.OnValueChanged;
			}

			HoverItemDataSelectableFloat oldSelFloatData = (oldData as HoverItemDataSelectableFloat);
			HoverItemDataSelectableFloat newSelFloatData = (newData as HoverItemDataSelectableFloat);

			if ( oldSelFloatData != null && newSelFloatData != null ) {
				newSelFloatData.Value = oldSelFloatData.Value;
				newSelFloatData.OnValueChangedEvent = oldSelFloatData.OnValueChangedEvent;
				//newSelFloatData.OnValueChanged += oldSelFloatData.OnValueChanged;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemData BuildData(HoverItemType pType) {
			switch ( pType ) {
				case HoverItemType.Selector:
					return gameObject.AddComponent<HoverItemDataSelector>();

				case HoverItemType.Sticky:
					return gameObject.AddComponent<HoverItemDataSticky>();

				case HoverItemType.Checkbox:
					return gameObject.AddComponent<HoverItemDataCheckbox>();

				case HoverItemType.Radio:
					HoverItemDataRadio radioData = gameObject.AddComponent<HoverItemDataRadio>();
					radioData.InitDefaultGroupId(gameObject.transform.parent);
					return radioData;

				case HoverItemType.Slider:
					return gameObject.AddComponent<HoverItemDataSlider>();

				case HoverItemType.Text:
					return gameObject.AddComponent<HoverItemDataText>();

				default:
					throw new InvalidEnumArgumentException("Unhandled type: "+pType);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void DestroyData(HoverItemData pData, HoverItemData pIgnoreNewData) {
			gameObject.GetComponents(vDataComponentBuffer);

			for ( int i = 0 ; i < vDataComponentBuffer.Count ; i++ ) {
				HoverItemData data = vDataComponentBuffer[i];

				if ( data == pIgnoreNewData ) {
					continue;
				}

				if ( data != pData ) {
					Debug.LogWarning("Removed unexpected "+typeof(HoverItemData).Name+": "+data, this);
				}

				DestroyImmediate(data, false);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemData[] GetChildItems() {
			return GetChildItemsFromGameObject(gameObject);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static IItemData[] GetChildItemsFromGameObject(GameObject pParentObj) {
			Transform tx = pParentObj.transform;
			int childCount = tx.childCount;
			var items = new List<IItemData>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverItem hni = tx.GetChild(i).GetComponent<HoverItem>();
				IItemData item = hni.Data;

				if ( !item.gameObject.activeSelf ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
