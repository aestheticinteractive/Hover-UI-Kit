using System.Collections.Generic;
using System.ComponentModel;
using Hover.Common.Items.Managers;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Items {
	
	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItem : MonoBehaviour {

		public enum HoverItemType {
			Parent,
			Selector,
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

		private HoverItemsManager vItemsMan;
		private HoverItemData vOldData;
		private HoverItemType vPrevItemType;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevItemType = _ItemType;

			BuildDataIfNeeded();
			UpdateItemsManager(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vOldData != null ) {
				DestroyImmediate(vOldData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			UpdateItemsManager(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemType ItemType {
			get { return _ItemType; }
			set { _ItemType = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverItemData Data {
			get { return _Data; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInspector() {
			//TODO: when causeing a button/slider switch, the new renderer is not updated immediately
			UpdateWithLatestItemTypeIfNeeded();

			Data.IsVisible = gameObject.activeSelf;
			Data.IsAncestryVisible = gameObject.activeInHierarchy;
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
			else {
				return;
			}

			_Data.name = _Data.GetType()+":"+GetInstanceID();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithLatestItemTypeIfNeeded() {
			if ( _ItemType == vPrevItemType ) {
				return;
			}

			HoverItemData newData = BuildData(_ItemType);
			TransferData(newData);

			vOldData = _Data;
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

			vItemsMan = (vItemsMan ?? FindObjectOfType<HoverItemsManager>());

			if ( vItemsMan == null ) {
				return;
			}

			if ( pAdd ) {
				vItemsMan.AddItem(this);
			}
			else {
				vItemsMan.RemoveItem(this);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool FindDuplicateData() {
			HoverItem[] items = FindObjectsOfType<HoverItem>();
			
			foreach ( HoverItem item in items ) {
				if ( item != this && item.Data == _Data ) {
					return true;
				}
			}
			
			return false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemData TransferData(HoverItemData pDataToFill) {
			HoverItemData oldData = _Data;
			HoverItemData newData = pDataToFill;

			if ( oldData == null ) {
				return newData;
			}

			newData.AutoId = oldData.AutoId;
			newData.IsAncestryEnabled = oldData.IsAncestryEnabled;
			newData.IsAncestryVisible = oldData.IsAncestryVisible;
			newData.Id = oldData.Id;
			newData.Label = oldData.Label;
			newData.IsEnabled = oldData.IsEnabled;
			newData.IsVisible = oldData.IsVisible;

			SelectableItem oldSelData = (oldData as SelectableItem);
			SelectableItem newSelData = (newData as SelectableItem);

			if ( oldSelData == null || newSelData == null ) {
				return newData;
			}

			newSelData.OnSelectedEvent = oldSelData.OnSelectedEvent;
			newSelData.OnDeselectedEvent = oldSelData.OnDeselectedEvent;
			//newSelData.OnSelected += oldSelData.OnSelected;
			//newSelData.OnDeselected += oldSelData.OnDeselected;
			
			SelectableItemBool oldSelBoolData = (oldData as SelectableItemBool);
			SelectableItemBool newSelBoolData = (newData as SelectableItemBool);

			if ( oldSelBoolData != null && newSelBoolData != null ) {
				newSelBoolData.Value = oldSelBoolData.Value;
				newSelBoolData.OnValueChangedEvent = oldSelBoolData.OnValueChangedEvent;
				//newSelBoolData.OnValueChanged += oldSelBoolData.OnValueChanged;
			}

			SelectableItemFloat oldSelFloatData = (oldData as SelectableItemFloat);
			SelectableItemFloat newSelFloatData = (newData as SelectableItemFloat);

			if ( oldSelFloatData != null && newSelFloatData != null ) {
				newSelFloatData.Value = oldSelFloatData.Value;
				newSelFloatData.OnValueChangedEvent = oldSelFloatData.OnValueChangedEvent;
				//newSelFloatData.OnValueChanged += oldSelFloatData.OnValueChanged;
			}

			return newData;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemData BuildData(HoverItemType pType) {
			switch ( pType ) {
				case HoverItemType.Parent:
					ParentItem parent = gameObject.AddComponent<ParentItem>();
					parent.InitChildGroup(GetChildItems);
					return parent;

				case HoverItemType.Selector:
					return gameObject.AddComponent<SelectorItem>();

				case HoverItemType.Sticky:
					return gameObject.AddComponent<StickyItem>();

				case HoverItemType.Checkbox:
					return gameObject.AddComponent<CheckboxItem>();

				case HoverItemType.Radio:
					RadioItem radio = gameObject.AddComponent<RadioItem>();
					radio.InitDefaultGroupId(gameObject.transform.parent);
					return radio;

				case HoverItemType.Slider:
					return gameObject.AddComponent<SliderItem>();

				case HoverItemType.Text:
					return gameObject.AddComponent<TextItem>();

				default:
					throw new InvalidEnumArgumentException("Unhandled type: "+pType);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			return GetChildItemsFromGameObject(gameObject);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static IBaseItem[] GetChildItemsFromGameObject(GameObject pParentObj) {
			Transform tx = pParentObj.transform;
			int childCount = tx.childCount;
			var items = new List<IBaseItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverItem hni = tx.GetChild(i).GetComponent<HoverItem>();
				IBaseItem item = hni.Data;

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
