using System.Collections.Generic;
using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class ItemHierarchy : IItemHierarchy {

		public event ItemEvents.HierarchyLevelChangedHandler OnLevelChange;
		public event ItemEvents.GroupItemSelectedHandler OnItemSelection;

		public string Title { get; private set; }

		private IItemGroup vCurrLevel;
		private readonly Stack<IItemGroup> vHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemHierarchy(string pTitle) {
			Title = pTitle;
			vHistory = new Stack<IItemGroup>();

			OnLevelChange += (d => {});
			OnItemSelection += ((l,i) => {});
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
			OnItemSelection(pLevel, pItem);

			IParentItem parItem = (pItem as IParentItem);

			if ( parItem != null ) {
				vHistory.Push(vCurrLevel);
				SetNewLevel(parItem.ChildLevel, 1);
				return;
			}

			if ( pItem.NavigateBackUponSelect ) {
				Back();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewLevel(IItemGroup pNewLevel, int pDirection) {
			if ( vCurrLevel != null ) {
				vCurrLevel.OnItemSelected -= HandleItemSelected;
				vCurrLevel.IsActive = false;
			}

			vCurrLevel = pNewLevel;
			vCurrLevel.OnItemSelected += HandleItemSelected;
			vCurrLevel.IsActive = true;

			//TODO: notify "parent" items about this change to potentially reset their current value

			OnLevelChange(pDirection);
		}

	}

}
