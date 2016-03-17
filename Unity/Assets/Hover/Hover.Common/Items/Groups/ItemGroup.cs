using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemGroup : IItemGroup {

		public event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		public string Title { get; set; }
		public object DisplayContainer { get; set; }
		public ISelectableItem LastSelectedItem { get; private set; }

		private readonly Func<IBaseItem[]> vGetItems;
		private IBaseItem[] vActiveItems;
		private bool vIsEnabled;
		private bool vIsVisible;
		private bool vIsAncestryEnabled;
		private bool vIsAncestryVisible;
		private Func<IRadioItem, IList<IRadioItem>> vRadioSiblingsFunc;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemGroup(Func<IBaseItem[]> pGetItems) {
			vGetItems = pGetItems;
			vIsEnabled = true;
			vIsVisible = true;
			vIsAncestryEnabled = true;
			vIsAncestryVisible = true;

			OnItemSelected += ((l,i) => {});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem[] Items {
			get {
				if ( vActiveItems == null ) {
					vActiveItems = GetLatestActiveItems();
					UpdateActiveEnabledAndVisible();
				}
				
				return vActiveItems;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem[] GetAllItems() {
			return vGetItems();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T[] GetTypedItems<T>() where T : class, IBaseItem {
			return GetTypedItems<T>(Items);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ReloadActiveItems() {
			if ( vActiveItems == null ) {
				return;
			}

			ISelectableItem[] selItems = GetTypedItems<ISelectableItem>();

			foreach ( ISelectableItem selItem in selItems ) {
				selItem.OnSelected -= HandleItemSelected;
				selItem.DeselectStickySelections();
			}

			vActiveItems = null; //list will be reloaded upon next "Items" property use
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
				UpdateActiveEnabledAndVisible();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsVisible {
			get {
				return vIsVisible;
			}
			set {
				if ( value == vIsVisible ) {
					return;
				}

				vIsVisible = value;
				UpdateActiveEnabledAndVisible();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsAncestryEnabled {
			get {
				return vIsAncestryEnabled;
			}
			set {
				if ( value == vIsAncestryEnabled ) {
					return;
				}

				vIsAncestryEnabled = value;
				UpdateActiveEnabledAndVisible();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsAncestryVisible {
			get {
				return vIsAncestryVisible;
			}
			set {
				if ( value == vIsAncestryVisible ) {
					return;
				}

				vIsAncestryVisible = value;
				UpdateActiveEnabledAndVisible();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetRadioSiblingsFunc(Func<IRadioItem, IList<IRadioItem>> pFunc) {
			vRadioSiblingsFunc = pFunc;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static T[] GetTypedItems<T>(IBaseItem[] pItems) where T : class, IBaseItem {
			return pItems
				.Select(x => (x as T))
				.Where(x => (x != null))
				.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveEnabledAndVisible() {
			foreach ( IBaseItem item in Items ) { //loads new items if necessary
				item.IsAncestryEnabled = (IsEnabled && IsAncestryEnabled);
				item.IsAncestryVisible = (IsVisible && IsAncestryVisible);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetLatestActiveItems() {
			IBaseItem[] items = vGetItems();

			foreach ( IBaseItem item in items ) {
				ISelectableItem selItem = (item as ISelectableItem);

				if ( selItem != null ) {
					selItem.OnSelected += HandleItemSelected;
				}
			}

			return items;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRadioSiblings(IRadioItem pSelectedRadioItem) {
			IList<IRadioItem> siblings;

			if ( vRadioSiblingsFunc != null ) {
				siblings = vRadioSiblingsFunc(pSelectedRadioItem);
			}
			else {
				siblings = new List<IRadioItem>();

				foreach ( IBaseItem item in vActiveItems ) {
					IRadioItem radItem = (item as IRadioItem);

					if ( radItem != null && radItem != pSelectedRadioItem ) {
						siblings.Add(radItem);
					}
				}
			}

			for ( int i = 0 ; i < siblings.Count ; i++ ) {
				siblings[i].Value = false;
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

	}

}
