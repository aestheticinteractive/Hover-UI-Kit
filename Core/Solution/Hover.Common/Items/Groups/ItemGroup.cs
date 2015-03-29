using System;
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
		private Func<bool> vAreParentsEnabledFunc;
		private Func<bool> vAreParentsVisibleFunc;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemGroup(Func<IBaseItem[]> pGetItems) {
			vGetItems = pGetItems;
			vIsEnabled = true;
			vIsVisible = true;

			OnItemSelected += ((l,i) => {});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem[] Items {
			get {
				if ( vActiveItems == null ) {
					vActiveItems = GetLatestActiveItems();
				}
				
				return vActiveItems;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public T[] GetTypedItems<T>() where T : class, IBaseItem {
			return GetTypedItems<T>(Items);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetParentsEnabledFunc(Func<bool> pFunc) {
			vAreParentsEnabledFunc = pFunc;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetParentsVisibleFunc(Func<bool> pFunc) {
			vAreParentsVisibleFunc = pFunc;
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

				if ( !vIsEnabled ) {
					ResetActiveItems();
				}

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

				if ( !vIsVisible ) {
					ResetActiveItems();
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AreParentsEnabled {
			get {
				if ( vAreParentsEnabledFunc == null ) {
					throw new Exception("Use 'SetParentsEnabledFunc' before using this property.");
				}

				return (vIsEnabled && vAreParentsEnabledFunc());
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AreParentsVisible {
			get {
				if ( vAreParentsVisibleFunc == null ) {
					throw new Exception("Use 'SetParentsVisibleFunc' before using this property.");
				}

				return vAreParentsVisibleFunc();
			}
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
		private void ResetActiveItems() {
			ISelectableItem[] selItems = GetTypedItems<ISelectableItem>();

			foreach ( ISelectableItem selItem in selItems ) {
				selItem.OnSelected -= HandleItemSelected;
				selItem.DeselectStickySelections();
			}

			vActiveItems = null; //list will be reloaded upon next "Items" property use
		}

		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetLatestActiveItems() {
			IBaseItem[] items = vGetItems();

			foreach ( IBaseItem item in items ) {
				item.SetParentsEnabledFunc(() => IsEnabled && AreParentsEnabled);
				item.SetParentsVisibleFunc(() => IsVisible && AreParentsVisible);
				ISelectableItem selItem = (item as ISelectableItem);

				if ( selItem != null ) {
					selItem.OnSelected += HandleItemSelected;
				}
			}

			return items;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRadioSiblings(IRadioItem pSelectedRadioItem) {
			foreach ( IBaseItem item in vActiveItems ) {
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
