using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.Items.Types;

namespace Hover.Cast.Items.Groups {

	/*================================================================================================*/
	public class HovercastItemHierarchy : HoverItemHierarchy { 

		public const string NavigateBackItemId = "__NavigateBackItem__";

		private HoverSelectorItem vNavigateBackItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ISelectorItem NavigateBackItem {
			get {
				if ( vNavigateBackItem != null ) {
					return vNavigateBackItem;
				}

				if ( gameObject == null ) {
					return null;
				}

				vNavigateBackItem = gameObject.GetComponent<HoverSelectorItem>();

				if ( vNavigateBackItem != null && vNavigateBackItem.Id == NavigateBackItemId ) {
					return vNavigateBackItem;
				}

				vNavigateBackItem = gameObject.AddComponent<HoverSelectorItem>();
				vNavigateBackItem.Id = NavigateBackItemId;
				vNavigateBackItem.IsEnabled = true;
				vNavigateBackItem.IsVisible = true;
				vNavigateBackItem.NavigateBackUponSelect = true;
				vNavigateBackItem.Label = "Back";
				vNavigateBackItem.OnSelected += HandleNavigateBackItemSelected;

				return vNavigateBackItem;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleNavigateBackItemSelected(ISelectableItem pItem) {
			Back();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void SetNewLevel(IItemGroup pNewLevel, int pDirection) {
			NavigateBackItem.IsEnabled = (ParentLevel != null);
			base.SetNewLevel(pNewLevel, pDirection);
		}

	}

}
