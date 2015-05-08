using System;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Board.Items {
	
	/*================================================================================================*/
	public class HoverboardItem : MonoBehaviour, IHovercommonItem {

		public SelectableItemType Type;
		public string Id = "";
		public string Label = "";
		public float Width = 1;
		public float Height = 1;
		public bool IsVisible = true;
		public bool IsEnabled = true;

		private BaseItem vItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem GetItem() {
			if ( vItem == null ) {
				BuildItem();
			}

			return vItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildItem() {
			switch ( Type ) {
				case SelectableItemType.Selector:
					vItem = new SelectorItem();
					break;

				case SelectableItemType.Sticky:
					vItem = new StickyItem();
					break;

				default:
					throw new Exception("Unhandled item type: "+Type);
			}

			if ( !string.IsNullOrEmpty(Id) ) {
				vItem.Id = Id;
			}

			vItem.DisplayContainer = gameObject;
			vItem.Label = (string.IsNullOrEmpty(Label) ? gameObject.name : Label);
			vItem.Width = Width;
			vItem.Height = Height;
			vItem.IsVisible = IsVisible;
			vItem.IsEnabled = IsEnabled;
		}

	}

}
