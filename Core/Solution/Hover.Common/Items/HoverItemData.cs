using System.Collections.Generic;
using System.ComponentModel;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Items {
	
	/*================================================================================================*/
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

		[SerializeField]
		private HoverItemType vType;

		public BaseItem DataProp;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemData() {
			vType = HoverItemType.Selector;
			DataProp = BuildData(vType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemType ItemType {
			get {
				return vType;
			}
			set {
				if ( vType != value ) {
					DataProp = BuildData(value);
					Debug.Log("BUILD: "+value+" / "+DataProp);
				}

				vType = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public BaseItem Data {
			get { return DataProp; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private BaseItem BuildData(HoverItemType pType) {
			switch ( pType ) {
				case HoverItemType.Parent:
					return new ParentItem(GetChildItems);

				case HoverItemType.Selector:
					return new SelectorItem();

				case HoverItemType.Sticky:
					return new StickyItem();

				case HoverItemType.Checkbox:
					return new CheckboxItem();

				case HoverItemType.Radio:
					return new RadioItem(gameObject.transform.parent.name);

				case HoverItemType.Slider:
					return new SliderItem();

				case HoverItemType.Text:
					return new TextItem();

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
