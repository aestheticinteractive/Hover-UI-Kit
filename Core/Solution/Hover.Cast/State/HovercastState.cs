using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Input;
using Hover.Common.Items.Groups;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState {
		
		public delegate void SideChangeHandler();

		public event SideChangeHandler OnSideChange;

		public MenuState FullMenu { get; private set; }
		public Transform BaseTransform { get; private set; }
		public Transform MenuTransform { get; private set; }

		private readonly HovercursorSetup vHovercursorSetup;
		private readonly InteractionSettings vInteractSettings;
		private readonly IInputProvider vInputProv;
		private bool? vCurrIsMenuOnLeftSide;

		private ICursorState vCursor;
		private ICursorInteractState vCursorInteract;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInputProvider pInputProv, Transform pBaseTx) {
			vInteractSettings = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInputProv = pInputProv;
			BaseTransform = pBaseTx;

			FullMenu = new MenuState(pItemHierarchy, vInteractSettings);

			OnSideChange += (() => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorState Hovercursor {
			get {
				return vHovercursorSetup.State;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IHovercastMenuState Menu {
			get {
				return FullMenu;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(Transform pMenuTx) {
			MenuTransform = pMenuTx;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			var isMenuOnLeft = vInteractSettings.IsMenuOnLeftSide;
			bool isSideChange = (isMenuOnLeft != vCurrIsMenuOnLeftSide);

			vCurrIsMenuOnLeftSide = isMenuOnLeft;
			
			if ( isSideChange ) {
				if ( vCursor != null ) {
					vCursor.RemoveInteraction(CursorDomain.Hovercast, "");
				}

				CursorType cursorType = (isMenuOnLeft ? CursorType.RightIndex : CursorType.LeftIndex);
				vCursor = vHovercursorSetup.State.GetCursorState(cursorType);
				vCursorInteract = vCursor.AddOrGetInteraction(CursorDomain.Hovercast, "");
			}

			FullMenu.UpdateAfterInput(vInputProv.GetSide(isMenuOnLeft).Menu, new[] { vCursor });
			vCursorInteract.DisplayStrength = FullMenu.DisplayStrength;
			
			if ( isSideChange ) {
				OnSideChange();
			}
		}

	}

}
