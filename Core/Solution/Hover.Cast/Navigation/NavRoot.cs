using System.Collections.Generic;

namespace Hover.Cast.Navigation {

	/*================================================================================================*/
	public class NavRoot { 

		public delegate void LevelChangeHandler(int pDirection);
		public delegate void ItemSelectionHandler(NavLevel pLevel, NavItem pItem);

		public event LevelChangeHandler OnLevelChange;
		public event ItemSelectionHandler OnItemSelection;

		public string Title { get; internal set; }

		private NavLevel vCurrLevel;
		private readonly Stack<NavLevel> vHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavRoot() {
			vHistory = new Stack<NavLevel>();
			OnLevelChange += (d => {});
			OnItemSelection += ((l,i) => {});
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Build(NavLevel pRootLevel) {
			SetNewLevel(pRootLevel, 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavLevel GetLevel() {
			return vCurrLevel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public NavLevel GetParentLevel() {
			return (vHistory.Count == 0 ? null : vHistory.Peek());
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetLevelTitle() {
			NavLevel level = GetParentLevel();
			return (level == null ? Title : level.LastSelectedParentItem.Label);
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
		private void HandleItemSelected(NavLevel pLevel, NavItem pItem) {
			OnItemSelection(pLevel, pItem);

			if ( pItem.Type == NavItem.ItemType.Parent ) {
				vHistory.Push(vCurrLevel);
				SetNewLevel(pItem.ChildLevel, 1);
				return;
			}

			if ( pItem.NavigateBackUponSelect ) {
				Back();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetNewLevel(NavLevel pNewLevel, int pDirection) {
			if ( vCurrLevel != null ) {
				vCurrLevel.OnItemSelected -= HandleItemSelected;
				vCurrLevel.SetActiveOnLevelChange(false, pDirection);
			}

			vCurrLevel = pNewLevel;
			vCurrLevel.OnItemSelected += HandleItemSelected;
			vCurrLevel.SetActiveOnLevelChange(true, pDirection);

			OnLevelChange(pDirection);
		}

	}

}
