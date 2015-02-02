using System.Collections.Generic;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavProvider { 

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		private readonly Stack<NavLevel> vHistory;
		private NavLevel vCurrLevel;
		private INavDelegate vDelgate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavProvider() {
			vHistory = new Stack<NavLevel>();
			OnLevelChange += (d => {});
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Init(INavDelegate pDelgate) {
			vDelgate = pDelgate;
			vHistory.Clear();
			SetNewLevel(vDelgate.GetTopLevel(), 0);
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
			return (level == null ? vDelgate.GetTopLevelTitle() : level.LastSelectedParentItem.Label);
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
			vDelgate.HandleItemSelection(pLevel, pItem);

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
			vDelgate.HandleLevelChange(vCurrLevel, pDirection);
		}

	}

}
