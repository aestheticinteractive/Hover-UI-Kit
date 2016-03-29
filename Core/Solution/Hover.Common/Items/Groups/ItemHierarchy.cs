using System.Collections.Generic;
using Hover.Common.Components.Items.Types;
using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemHierarchy : IItemHierarchy {

		public const string NavigateBackItemId = "__NavigateBackItem__";

		public event ItemEvents.HierarchyLevelChangedHandler OnLevelChanged;
		public event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		public string Title { get; set; }
		public ISelectorItem NavigateBackItem { get; private set; }

		private IItemGroup vCurrLevel;
		private readonly Stack<IItemGroup> vHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemHierarchy() {
			vHistory = new Stack<IItemGroup>();

			NavigateBackItem = new HoverSelectorItem(); //TODO: fail
			NavigateBackItem.Id = NavigateBackItemId;
			NavigateBackItem.IsEnabled = true;
			NavigateBackItem.IsVisible = true;
			NavigateBackItem.NavigateBackUponSelect = true;
			NavigateBackItem.Label = "Back";
			NavigateBackItem.OnSelected += HandleNavigateBackItemSelected;

			OnLevelChanged += (d => {});
			OnItemSelected += ((l,i) => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(IItemGroup pRootLevel) {
			SetNewLevel(pRootLevel, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Back() {
			if ( vHistory.Count == 0 ) {
				return;
			}

			SetNewLevel(vHistory.Pop(), -1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemGroup CurrentLevel {
			get {
				return vCurrLevel;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IItemGroup ParentLevel {
			get {
				return (vHistory.Count == 0 ? null : vHistory.Peek());
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string CurrentLevelTitle {
			get {
				IItemGroup parLevel = ParentLevel;
				return (parLevel == null ? Title : parLevel.LastSelectedItem.Label);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(IItemGroup pLevel, ISelectableItem pItem) {
			OnItemSelected(pLevel, pItem);

			IParentItem parItem = (pItem as IParentItem);
			ISelectorItem selectorItem = (pItem as ISelectorItem);

			if ( parItem != null ) {
				vHistory.Push(vCurrLevel);
				SetNewLevel(parItem.ChildGroup, 1);
				return;
			}

			if ( selectorItem != null && selectorItem.NavigateBackUponSelect ) {
				Back();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleNavigateBackItemSelected(ISelectableItem pItem) {
			Back();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewLevel(IItemGroup pNewLevel, int pDirection) {
			if ( vCurrLevel != null ) {
				vCurrLevel.OnItemSelected -= HandleItemSelected;
				vCurrLevel.IsEnabled = false;
			}

			vCurrLevel = pNewLevel;
			vCurrLevel.ReloadActiveItems();
			vCurrLevel.IsEnabled = true;
			vCurrLevel.OnItemSelected += HandleItemSelected;

			NavigateBackItem.IsEnabled = (vHistory.Count > 0);

			OnLevelChanged(pDirection);
		}

	}

}
