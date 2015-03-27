using System;
using System.Linq;
using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemGroup : IItemGroup {

		public event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		public object DisplayContainer { get; set; }
		public ISelectableItem LastSelectedItem { get; private set; }

		private readonly Func<IBaseItem[]> vGetItems;
		private IBaseItem[] vActiveItems;
		private bool vIsEnabled;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemGroup(Func<IBaseItem[]> pGetItems) {
			vGetItems = pGetItems;
			OnItemSelected += ((l,i) => {});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem[] Items {
			get {
				if ( vActiveItems == null ) {
					vActiveItems = vGetItems();
				}
				
				return vActiveItems;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public T[] GetTypedItems<T>() where T : class, IBaseItem {
			return Items
				.Select(x => (x as T))
				.Where(x => (x != null))
				.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get {
				return vIsEnabled;
			}
			set {
				if ( value == vIsEnabled ) {
					return;
				}

				vIsEnabled = value;
				vActiveItems = null; //reloads list upon "Items" property usage below

				foreach ( IBaseItem item in Items ) {
					ISelectableItem selItem = (item as ISelectableItem);

					if ( selItem == null ) {
						continue;
					}

					if ( IsEnabled ) {
						selItem.OnSelected += HandleItemSelected;
					}
					else {
						selItem.OnSelected -= HandleItemSelected;
						selItem.DeselectStickySelections();
					}
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(ISelectableItem pItem) {
			IRadioItem radItem = (pItem as IRadioItem);

			if ( radItem != null ) {
				DeselectRadioSiblings(radItem);
			}

			LastSelectedItem = pItem;
			OnItemSelected(this, pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRadioSiblings(IRadioItem pSelectedRadioItem) {
			foreach ( IBaseItem item in Items ) {
				if ( item == pSelectedRadioItem ) {
					continue;
				}

				IRadioItem radItem = (item as IRadioItem);

				if ( radItem == null ) {
					continue;
				}

				radItem.Value = false;
			}
		}

	}

}
