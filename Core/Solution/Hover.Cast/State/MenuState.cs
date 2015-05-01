using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.State;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class MenuState : IHovercastMenuState {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		public bool IsInputAvailable { get; private set; }
		public bool IsOnLeftSide { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }
		public float DisplayStrength { get; private set; }
		public float NavBackStrength { get; private set; }
		public IBaseItemState NearestItem { get; private set; }
		
		private readonly IItemHierarchy vItemHierarchy;
		private readonly InteractionSettings vInteractSettings;
		private readonly IList<BaseItemState> vAllItems;
		private readonly IList<BaseItemState> vItems;
		private readonly BaseItemState vPalmItem;

		private ICursorState[] vCurrentCursors;
		private bool vIsNavigateBackStarted;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IItemHierarchy pItemHierarchy, InteractionSettings pInteractSettings) {
			vItemHierarchy = pItemHierarchy;
			vInteractSettings = pInteractSettings;

			vAllItems = new List<BaseItemState>();
			vItems = new List<BaseItemState>();
			vCurrentCursors = new ICursorState[0];
			vPalmItem = new BaseItemState(vItemHierarchy.NavigateBackItem, pInteractSettings);

			OnLevelChange += (d => {});

			vItemHierarchy.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState[] GetItems() {
			return vItems.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemState[] GetLevelItems() {
			return vItems.Cast<IBaseItemState>().ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState GetPalmItem() {
			return vPalmItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem GetLevelParentItem() {
			IItemGroup parGroup = vItemHierarchy.ParentLevel;
			return (parGroup == null ? null : parGroup.LastSelectedItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetLevelTitle() {
			return vItemHierarchy.CurrentLevelTitle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputMenu pInputMenu, ICursorState[] pCursors) {
			vCurrentCursors = pCursors;

			IsInputAvailable = pInputMenu.IsAvailable;
			IsOnLeftSide = pInputMenu.IsLeft;
			Center = pInputMenu.Position;
			Rotation = pInputMenu.Rotation;
			Size = pInputMenu.Radius;
			DisplayStrength = pInputMenu.DisplayStrength;
			NavBackStrength = pInputMenu.NavigateBackStrength;

			CheckNavigateBackAction(pInputMenu);

			foreach ( ICursorState cursor in vCurrentCursors ) {
				UpdateWithCursor(cursor);
			}

			foreach ( BaseItemState item in vAllItems ) {
				if ( item.UpdateSelectionProcess() ) { //returns true if selection occurred
					break; //exit loop, since the items list changes after a selection
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ResetAllItemCursorInteractions() {
			foreach ( BaseItemState item in vAllItems ) {
				item.ResetAllCursorInteractions();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithCursor(ICursorState pCursor) {
			bool allowSelect = (pCursor.IsInputAvailable && DisplayStrength > 0);
			Vector3? cursorWorldPos = (allowSelect ? pCursor.GetWorldPosition() : (Vector3?)null);
			CursorType cursorType = pCursor.Type;
			float nearestDist = float.MaxValue;

			NearestItem = null;

			foreach ( BaseItemState item in vAllItems ) {
				item.UpdateWithCursor(cursorType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				float itemDist = item.GetHighlightDistance(cursorType);

				if ( itemDist >= nearestDist ) {
					continue;
				}

				NearestItem = item;
				nearestDist = itemDist;
			}

			foreach ( BaseItemState item in vAllItems ) {
				item.SetAsNearestItem(cursorType, (item == NearestItem));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CheckNavigateBackAction(IInputMenu pInputMenu) {
			if ( pInputMenu == null ) {
				vIsNavigateBackStarted = false;
				return;
			}

			if ( vIsNavigateBackStarted && pInputMenu.NavigateBackStrength <= 0 ) {
				vIsNavigateBackStarted = false;
				return;
			}

			if ( vIsNavigateBackStarted || pInputMenu.NavigateBackStrength < 1 ) {
				return;
			}

			vIsNavigateBackStarted = true;
			vItemHierarchy.Back();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vAllItems.Clear();
			vItems.Clear();

			IBaseItem[] items = vItemHierarchy.CurrentLevel.Items;

			foreach ( IBaseItem item in items ) {
				var seg = new BaseItemState(item, vInteractSettings);
				vAllItems.Add(seg);
				vItems.Add(seg);
			}

			vAllItems.Add(vPalmItem);
			OnLevelChange(pDirection);
		}

	}

}
