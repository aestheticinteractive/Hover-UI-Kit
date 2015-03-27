using System.Collections.Generic;
using System.Linq;
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
		private readonly IInput vInput;
		private readonly IDictionary<CursorType, ICursorState> vCursorMap;
		private readonly IDictionary<HovercastCursorType, CursorType> vLeftCursorConvertMap;
		private readonly IDictionary<HovercastCursorType, CursorType> vRightCursorConvertMap;

		private bool? vCurrIsMenuOnLeftSide;
		private IInteractionPlaneState vInteractionPlane;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInput pInput, Transform pBaseTx) {
			vInteractSettings = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInput = pInput;
			BaseTransform = pBaseTx;

			FullMenu = new MenuState(pItemHierarchy, vInteractSettings);

			vCursorMap = new Dictionary<CursorType, ICursorState>();

			vLeftCursorConvertMap = new Dictionary<HovercastCursorType, CursorType> {
				{ HovercastCursorType.Palm, CursorType.LeftPalm },
				{ HovercastCursorType.Thumb, CursorType.LeftThumb },
				{ HovercastCursorType.Index, CursorType.LeftIndex },
				{ HovercastCursorType.Middle, CursorType.LeftMiddle },
				{ HovercastCursorType.Ring, CursorType.LeftRing },
				{ HovercastCursorType.Pinky, CursorType.LeftPinky },
				{ HovercastCursorType.Look, CursorType.Look }
			};

			vRightCursorConvertMap = new Dictionary<HovercastCursorType, CursorType> {
				{ HovercastCursorType.Palm, CursorType.RightPalm },
				{ HovercastCursorType.Thumb, CursorType.RightThumb },
				{ HovercastCursorType.Index, CursorType.RightIndex },
				{ HovercastCursorType.Middle, CursorType.RightMiddle },
				{ HovercastCursorType.Ring, CursorType.RightRing },
				{ HovercastCursorType.Pinky, CursorType.RightPinky },
				{ HovercastCursorType.Look, CursorType.Look }
			};

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
		public void SetReferences(Transform pMenuTx, IInteractionPlaneState pInteractionPlane) {
			MenuTransform = pMenuTx;
			vInteractionPlane = pInteractionPlane;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			vInteractionPlane.IsEnabled = (FullMenu.DisplayStrength > 0);

			var isMenuOnLeft = vInteractSettings.IsMenuOnLeftSide;
			IInputMenu inputMenu = vInput.GetMenu(isMenuOnLeft);
			ICursorState[] cursors = UpdateCursors();

			FullMenu.UpdateAfterInput(inputMenu, cursors);

			if ( isMenuOnLeft != vCurrIsMenuOnLeftSide ) {
				vCurrIsMenuOnLeftSide = isMenuOnLeft;
				OnSideChange();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private ICursorState[] UpdateCursors() {
			IDictionary<HovercastCursorType, CursorType> convMap = 
				(vInteractSettings.IsMenuOnLeftSide ? vRightCursorConvertMap : vLeftCursorConvertMap);

			List<CursorType> activeTypes = vInteractSettings.Cursors
				.Select(x => convMap[x])
				.ToList();

			CursorType[] removeTypes = vCursorMap.Keys.Except(activeTypes).ToArray();
			IHovercursorState hovercursor = vHovercursorSetup.State;
			var cursors = new List<ICursorState>();

			foreach ( CursorType cursorType in removeTypes ) {
				ICursorState cursor = hovercursor.GetCursorState(cursorType);
				cursor.RemoveAllInteractions(CursorDomain.Hovercast);
				cursor.SetDisplayStrength(CursorDomain.Hovercast, 0);
			}

			foreach ( CursorType cursorType in activeTypes ) {
				ICursorState cursor = hovercursor.GetCursorState(cursorType);
				cursor.SetDisplayStrength(CursorDomain.Hovercast, FullMenu.DisplayStrength);
				cursors.Add(cursor);
			}

			return cursors.ToArray();
		}

	}

}
