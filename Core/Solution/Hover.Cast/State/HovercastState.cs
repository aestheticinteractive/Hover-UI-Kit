using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Cast.Items;
using UnityEngine;
using Hover.Cursor.State;
using Hover.Cursor;
using Hover.Common.Input;
using Hover.Common.State;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState {
		
		public delegate void SideChangeHandler();
		public event SideChangeHandler OnSideChange;

		public HovercastItemsProvider NavigationProvider { get; private set; }
		public HovercastCustomizationProvider CustomizationProvider { get; private set; }
		public HovercastInputProvider InputProvider { get; private set; }

		public ArcState Arc { get; private set; }
		public ICursorState Cursor { get; private set; }
		public Transform MenuTransform { get; private set; }

		private HovercursorSetup vHovercursorSetup;
		private Transform vBaseTx;
		private bool? vCurrIsMenuOnLeftSide;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(HovercastItemsProvider pNav, HovercastCustomizationProvider pCustom,
				                      HovercastInputProvider pInput, HovercursorSetup pHovercusorSetup,
				                      Transform pBaseTx) {
			NavigationProvider = pNav;
			CustomizationProvider = pCustom;
			InputProvider = pInput;
			vHovercursorSetup = pHovercusorSetup;
			vBaseTx = pBaseTx;

			OnSideChange += (() => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(Transform pMenuTx) {
			MenuTransform = pMenuTx;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			bool isMenuOnLeft = CustomizationProvider.GetInteractionSettings().IsMenuOnLeftSide;
			IInputSide menuSide = InputProvider.GetSide(isMenuOnLeft);
			bool isSideChange = (isMenuOnLeft != vCurrIsMenuOnLeftSide);
			vCurrIsMenuOnLeftSide = isMenuOnLeft;
			
			if ( isSideChange ) {
				if ( Cursor != null ) {
					Cursor.RemoveInteraction(CursorDomain.Hovercast, "");
				}

				CursorType cursorType = (isMenuOnLeft ? CursorType.RightIndex : CursorType.LeftIndex);
				Cursor = vHovercursorSetup.State.GetCursorState(cursorType);
				Cursor.AddOrGetInteraction(CursorDomain.Hovercast, "");
			}

			Arc.UpdateAfterInput(menuSide.Menu);
			Arc.UpdateWithCursor(Cursor);
			
			if ( isSideChange ) {
				OnSideChange();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorState Hovercursor {
			get {
				return vHovercursorSetup.State;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsMenuInputAvailable {
			get {
				return Arc.IsInputAvailable;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsMenuVisible {
			get {
				return (Arc.DisplayStrength > 0);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float MenuDisplayStrength {
			get {
				return Arc.DisplayStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float NavigateBackStrength {
			get {
				return Arc.NavBackStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public HovercastSideName MenuSide {
			get {
				return (Arc.IsLeft ? HovercastSideName.Left : HovercastSideName.Right);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemState[] CurrentItems {
			get {
				return Arc.GetItems().Cast<IBaseItemState>().ToArray();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemState NearestItem {
			get {
				return Arc.NearestItem;
			}
		}

	}

}
