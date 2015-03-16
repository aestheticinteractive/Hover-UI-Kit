using System;
using System.Linq;
using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemGroup : IItemGroup {

		public event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		public ISelectableItem LastSelectedItem { get; private set; }

		private readonly Func<IBaseItem[]> vGetItems;
		private IBaseItem[] vActiveItems;
		private bool vIsActive;


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
		public bool IsActive {
			get {
				return vIsActive;
			}
			set {
				if ( value == vIsActive ) {
					return;
				}

				vIsActive = value;
				vActiveItems = null; //reloads list upon "Items" property usage below

				foreach ( IBaseItem item in Items ) {
					//TODO: item.UpdateValueOnLevelChange(pDirection);

					ISelectableItem selItem = (item as ISelectableItem);

					if ( selItem == null ) {
						continue;
					}

					if ( IsActive ) {
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
		protected virtual void HandleItemSelected(ISelectableItem pItem) {
			IRadioItem radItem = (pItem as IRadioItem);

			if ( radItem != null ) {
				DeselectRadioSiblings(radItem);
			}

			LastSelectedItem = pItem;
			OnItemSelected(this, pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DeselectRadioSiblings(IRadioItem pSelectedRadioItem) {
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
