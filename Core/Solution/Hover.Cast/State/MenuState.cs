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
		private readonly IList<BaseItemState> vItems;

		private ICursorState[] vCurrentCursors;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IItemHierarchy pItemHierarchy, InteractionSettings pInteractSettings) {
			vItemHierarchy = pItemHierarchy;
			vInteractSettings = pInteractSettings;
			vItems = new List<BaseItemState>();
			vCurrentCursors = new ICursorState[0];

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

			CheckGrabGesture(pInputMenu);

			foreach ( ICursorState cursor in vCurrentCursors ) {
				UpdateWithCursor(cursor);
			}

			foreach ( BaseItemState item in vItems ) {
				if ( item.UpdateSelectionProcess() ) { //returns true if selection occurred
					break; //exit loop, since the vItems list changes after a selection
				}
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

			foreach ( BaseItemState item in vItems ) {
				pCursor.AddOrUpdateInteraction(CursorDomain.Hovercast, item);
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

			foreach ( BaseItemState item in vItems ) {
				item.SetAsNearestItem(cursorType, (item == NearestItem));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CheckGrabGesture(IInputMenu pInputMenu) {
			if ( pInputMenu == null ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing && pInputMenu.NavigateBackStrength <= 0 ) {
				vIsGrabbing = false;
				return;
			}

			if ( vIsGrabbing || pInputMenu.NavigateBackStrength < 1 ) {
				return;
			}

			vIsGrabbing = true;
			vItemHierarchy.Back();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			foreach ( ICursorState cursor in vCurrentCursors ) {
				cursor.RemoveAllInteractions(CursorDomain.Hovercast);
			}

			vItems.Clear();

			IBaseItem[] items = vItemHierarchy.CurrentLevel.Items;

			foreach ( IBaseItem item in items ) {
				var seg = new BaseItemState(item, vInteractSettings);
				vItems.Add(seg);
			}

			OnLevelChange(pDirection);
		}

	}

}
