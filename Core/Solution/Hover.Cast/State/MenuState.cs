using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.State;
using Hover.Common.Util;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class MenuState : IHovercastMenuState {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChanged;

		public bool IsInputAvailable { get; private set; }
		public bool IsOnLeftSide { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }
		public int DisplayDepthHint { get; set; }
		public float DisplayStrength { get; private set; }
		public float NavBackStrength { get; private set; }
		public IBaseItemState NearestItem { get; private set; }
		
		private readonly HovercastItemHierarchy vItemHierarchy;
		private readonly InteractionSettings vInteractSettings;
		private readonly List<BaseItemState> vAllItems;
		private readonly ReadList<BaseItemState> vItems;
		private readonly ReadList<IBaseItemState> vLevelItems;
		private readonly BaseItemState vPalmItem;
		private readonly List<ICursorState> vCurrentCursors;

		private bool vIsNavigateBackStarted;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(HovercastItemHierarchy pItemHierarchy, InteractionSettings pInteractSettings) {
			vItemHierarchy = pItemHierarchy;
			vInteractSettings = pInteractSettings;

			vAllItems = new List<BaseItemState>();
			vItems = new ReadList<BaseItemState>();
			vLevelItems = new ReadList<IBaseItemState>();
			vPalmItem = new BaseItemState(vItemHierarchy.NavigateBackItem, pInteractSettings);
			vCurrentCursors = new List<ICursorState>();

			OnLevelChanged += (d => {});

			vItemHierarchy.OnLevelChanged += HandleLevelChange;
			HandleLevelChange(0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<BaseItemState> GetItems() {
			return vItems.ReadOnly;
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<IBaseItemState> GetLevelItems() {
			vLevelItems.Clear();

			for ( int i = 0 ; i < vItems.ReadOnly.Count ; i++ ) {
				vLevelItems.Add(vItems.ReadOnly[i]);
			}

			return vLevelItems.ReadOnly;
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
		internal void ClearCursors() {
			vCurrentCursors.Clear();
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void AddCursor(ICursorState pCursor) {
			vCurrentCursors.Add(pCursor);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputMenu pInputMenu) {
			IsInputAvailable = pInputMenu.IsAvailable;
			IsOnLeftSide = pInputMenu.IsLeft;
			Center = pInputMenu.Position;
			Rotation = pInputMenu.Rotation;
			Size = pInputMenu.Radius;
			DisplayStrength = pInputMenu.DisplayStrength;
			NavBackStrength = pInputMenu.NavigateBackStrength;

			CheckNavigateBackAction(pInputMenu);
			
			foreach ( BaseItemState item in vAllItems ) {
				item.UpdateBeforeCursors();
			}

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
			OnLevelChanged(pDirection);
		}

	}

}
