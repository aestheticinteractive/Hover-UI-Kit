using System.Collections.Generic;
using System.ComponentModel;
using Hover.Common.Items.Managers;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Items {
	
	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemData : MonoBehaviour {

		public enum HoverItemType {
			Parent,
			Selector,
			Sticky,
			Checkbox,
			Radio,
			Slider,
			Text
		}

		public delegate void ItemEvent(HoverItemData pItem);
		public ItemEvent OnTypeChanged;

		[SerializeField]
		private HoverItemType _Type = HoverItemType.Selector;

		[SerializeField]
		private BaseItem _Data;

		private HoverItemsManager vItemsMan;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if  ( _Data == null ) {
				_Data = BuildData(_Type);
			}
			else if ( FindDuplicateData() ) {
				_Data = Instantiate(_Data); //handle duplication via Unity editor
			}

			_Data.name = _Data.GetType()+":"+GetInstanceID();
			
			////

			if ( Application.isPlaying ) {
				vItemsMan = FindObjectOfType<HoverItemsManager>();

				if ( vItemsMan != null ) {
					vItemsMan.AddItem(this);
				}
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			if ( Application.isPlaying ) {
				vItemsMan = (vItemsMan ?? FindObjectOfType<HoverItemsManager>());

				if ( vItemsMan != null ) {
					vItemsMan.RemoveItem(this);
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemType ItemType {
			get {
				return _Type;
			}
			set {
				if ( _Type == value ) {
					return;
				}

				_Type = value;

				BaseItem dataToFill = BuildData(_Type);
				_Data = TransferData(dataToFill);

				if ( OnTypeChanged != null ) {
					OnTypeChanged.Invoke(this);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public BaseItem Data {
			get { return _Data; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool FindDuplicateData() {
			HoverItemData[] items = FindObjectsOfType<HoverItemData>();
			
			foreach ( HoverItemData item in items ) {
				if ( item != this && item.Data == _Data ) {
					return true;
				}
			}
			
			return false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private BaseItem TransferData(BaseItem pDataToFill) {
			BaseItem oldData = _Data;
			BaseItem newData = pDataToFill;

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
		private BaseItem BuildData(HoverItemType pType) {
			switch ( pType ) {
				case HoverItemType.Parent:
					ParentItem parent = ScriptableObject.CreateInstance<ParentItem>();
					parent.InitChildGroup(GetChildItems);
					return parent;

				case HoverItemType.Selector:
					return ScriptableObject.CreateInstance<SelectorItem>();

				case HoverItemType.Sticky:
					return ScriptableObject.CreateInstance<StickyItem>();

				case HoverItemType.Checkbox:
					return ScriptableObject.CreateInstance<CheckboxItem>();

				case HoverItemType.Radio:
					RadioItem radio = ScriptableObject.CreateInstance<RadioItem>();
					radio.InitDefaultGroupId(gameObject.transform.parent);
					return radio;

				case HoverItemType.Slider:
					return ScriptableObject.CreateInstance<SliderItem>();

				case HoverItemType.Text:
					return ScriptableObject.CreateInstance<TextItem>();

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
				HoverItemData hni = tx.GetChild(i).GetComponent<HoverItemData>();
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
