using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using UnityEngine;
using Hover.Cursor.State;
using Hover.Cursor;
using Hover.Common.Input;
using Hover.Common.Items.Groups;
using Hover.Common.State;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState {
		
		public delegate void SideChangeHandler();
		public event SideChangeHandler OnSideChange;

		public ArcState Arc { get; private set; }
		public ICursorState Cursor { get; private set; }
		public Transform BaseTransform { get; private set; }
		public Transform MenuTransform { get; private set; }

		private readonly HovercursorSetup vHovercursorSetup;
		private readonly InteractionSettings vInteractSett;
		private readonly IInputProvider vInputProv;
		private bool? vCurrIsMenuOnLeftSide;
		private ICursorInteractState vCursorInteract;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInputProvider pInputProv, Transform pBaseTx) {
			vInteractSett = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInputProv = pInputProv;

			BaseTransform = pBaseTx;
			Arc = new ArcState(pItemHierarchy, vInteractSett);

			OnSideChange += (() => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(Transform pMenuTx) {
			MenuTransform = pMenuTx;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			bool isMenuOnLeft = vInteractSett.IsMenuOnLeftSide;
			IInputSide menuSide = vInputProv.GetSide(isMenuOnLeft);
			bool isSideChange = (isMenuOnLeft != vCurrIsMenuOnLeftSide);
			vCurrIsMenuOnLeftSide = isMenuOnLeft;
			
			if ( isSideChange ) {
				if ( Cursor != null ) {
					Cursor.RemoveInteraction(CursorDomain.Hovercast, "");
				}

				CursorType cursorType = (isMenuOnLeft ? CursorType.RightIndex : CursorType.LeftIndex);
				Cursor = vHovercursorSetup.State.GetCursorState(cursorType);
				vCursorInteract = Cursor.AddOrGetInteraction(CursorDomain.Hovercast, "");
			}

			Arc.UpdateAfterInput(menuSide.Menu);
			Arc.UpdateWithCursor(Cursor);
			vCursorInteract.DisplayStrength = Arc.DisplayStrength;
			
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
