using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Cursor.State;
using UnityEngine;
using Hover.Common.State;
using Hover.Common.Input;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class ArcState {

		public delegate void LevelChangeHandler(int pDirection);
		public event LevelChangeHandler OnLevelChange;

		public bool IsInputAvailable { get; private set; }
		public bool IsLeft { get; private set; }
		public Vector3 Center { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }
		public float DisplayStrength { get; private set; }
		public float NavBackStrength { get; private set; }
		public IBaseItemState NearestItem { get; private set; }

		private readonly IItemHierarchy vItemRoot;
		private readonly IList<BaseItemState> vItems;
		private readonly InteractionSettings vSettings;
		private bool vIsGrabbing;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArcState(IItemHierarchy pItemRoot, InteractionSettings pSettings) {
			vItemRoot = pItemRoot;
			vItems = new List<BaseItemState>();
			vSettings = pSettings;

			IsLeft = vSettings.IsMenuOnLeftSide;

			OnLevelChange += (d => {});

			vItemRoot.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState[] GetItems() {
			return vItems.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItem GetLevelParentItem() {
			IItemGroup parGroup = vItemRoot.ParentLevel;
			return (parGroup == null ? null : parGroup.LastSelectedItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetLevelTitle() {
			return vItemRoot.CurrentLevelTitle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputMenu pInputMenu) {
			IsInputAvailable = pInputMenu.IsAvailable;
			IsLeft = pInputMenu.IsLeft;
			Center = pInputMenu.Position;
			Rotation = pInputMenu.Rotation;
			Size = pInputMenu.Radius;
			DisplayStrength = pInputMenu.DisplayStrength;
			NavBackStrength = pInputMenu.NavigateBackStrength;

			CheckGrabGesture(pInputMenu);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithCursor(ICursorState pCursor) {
			bool allowSelect = (pCursor.IsInputAvailable && DisplayStrength > 0);
			Vector3? cursorWorldPos = (allowSelect ? pCursor.GetWorldPosition() : (Vector3?)null);
			CursorType cursorType = pCursor.Type;
			float minProg = float.MaxValue;

			NearestItem = null;

			foreach ( BaseItemState item in vItems ) {
				item.UpdateWithCursor(cursorType, cursorWorldPos);

				if ( !allowSelect ) {
					continue;
				}

				if ( NearestItem == null ) {
					NearestItem = item;
					continue;
				}

				float prog = item.GetHighlightProgress(cursorType);

				if ( prog < minProg ) {
					NearestItem = item;
					minProg = prog;
				}
			}

			foreach ( BaseItemState item in vItems ) {
				item.SetAsNearestItem(cursorType, (item == NearestItem));
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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

			if ( !vIsGrabbing && pInputMenu.NavigateBackStrength >= 1 ) {
				vIsGrabbing = true;
				vItemRoot.Back();
				return;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vItems.Clear();

			IBaseItem[] items = vItemRoot.CurrentLevel.Items;

			foreach ( IBaseItem item in items ) {
				var seg = new BaseItemState(item, vSettings);
				vItems.Add(seg);
			}

			OnLevelChange(pDirection);
		}

	}

}
