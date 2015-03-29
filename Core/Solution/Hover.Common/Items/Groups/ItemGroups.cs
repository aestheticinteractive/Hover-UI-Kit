using System;
using System.Linq;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemGroups : IItemGroups {

		public event ItemEvents.GroupItemSelectedHandler OnItemSelection;

		public string Title { get; set; }
		public object DisplayContainer { get; set; }

		private readonly Func<IItemGroup[]> vGetGroups;
		private IItemGroup[] vActiveGroups;
		private bool vIsEnabled;
		private bool vIsVisible;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemGroups(Func<IItemGroup[]> pGetGroups) {
			vGetGroups = pGetGroups;
			vIsEnabled = true;
			vIsVisible = true;
			
			OnItemSelection += ((l,i) => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemGroup[] Groups {
			get {
				if ( vActiveGroups == null ) {
					vActiveGroups = GetLatestActiveGroups();
				}

				return vActiveGroups;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public T[] GetTypedGroups<T>() where T : class, IItemGroup {
			return GetTypedGroups<T>(Groups);
		}

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
					ResetActiveGroups();
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
					ResetActiveGroups();
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static T[] GetTypedGroups<T>(IItemGroup[] pGroups) where T : class, IItemGroup {
			return pGroups
				.Select(x => (x as T))
				.Where(x => (x != null))
				.ToArray();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void ResetActiveGroups() {
			if ( vActiveGroups == null ) {
				return;
			}

			foreach ( IItemGroup group in vActiveGroups ) {
				group.OnItemSelected -= HandleItemSelected;

				foreach ( IBaseItem item in group.Items ) {
					ISelectableItem selItem = (item as ISelectableItem);

					if ( selItem == null ) {
						continue;
					}

					selItem.DeselectStickySelections();
				}
			}

			vActiveGroups = null; //list will be reloaded upon next "Items" property use
		}

		/*--------------------------------------------------------------------------------------------*/
		private IItemGroup[] GetLatestActiveGroups() {
			IItemGroup[] groups = vGetGroups();

			foreach ( IItemGroup group in groups ) {
				group.SetParentsEnabledFunc(() => IsEnabled);
				group.SetParentsVisibleFunc(() => IsVisible);
				group.OnItemSelected += HandleItemSelected;
			}

			return groups;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(IItemGroup pGroup, ISelectableItem pItem) {
			OnItemSelection(pGroup, pItem);
		}

	}

}
