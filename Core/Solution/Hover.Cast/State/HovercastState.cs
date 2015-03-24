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
		private readonly IInputProvider vInputProv;
		private readonly IDictionary<CursorType, ICursorState> vCursorMap;
		private readonly IDictionary<HovercastCursorType, CursorType> vLeftCursorConvertMap;
		private readonly IDictionary<HovercastCursorType, CursorType> vRightCursorConvertMap;

		private CursorType[] vCursorTypes;
		private bool? vCurrIsMenuOnLeftSide;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInputProvider pInputProv, Transform pBaseTx) {
			vInteractSettings = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInputProv = pInputProv;
			BaseTransform = pBaseTx;

			FullMenu = new MenuState(pItemHierarchy, vInteractSettings);

			vCursorMap = new Dictionary<CursorType, ICursorState>();

			vLeftCursorConvertMap = new Dictionary<HovercastCursorType, CursorType> {
				{ HovercastCursorType.Palm, CursorType.LeftPalm },
				{ HovercastCursorType.Thumb, CursorType.LeftThumb },
				{ HovercastCursorType.Index, CursorType.LeftIndex },
				{ HovercastCursorType.Middle, CursorType.LeftMiddle },
				{ HovercastCursorType.Ring, CursorType.LeftRing },
				{ HovercastCursorType.Pinky, CursorType.LeftPinky }
			};

			vRightCursorConvertMap = new Dictionary<HovercastCursorType, CursorType> {
				{ HovercastCursorType.Palm, CursorType.RightPalm },
				{ HovercastCursorType.Thumb, CursorType.RightThumb },
				{ HovercastCursorType.Index, CursorType.RightIndex },
				{ HovercastCursorType.Middle, CursorType.RightMiddle },
				{ HovercastCursorType.Ring, CursorType.RightRing },
				{ HovercastCursorType.Pinky, CursorType.RightPinky }
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
		public void SetReferences(Transform pMenuTx) {
			MenuTransform = pMenuTx;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			IHovercursorState hovercursor = vHovercursorSetup.State;
			var cursors = new List<ICursorState>();
			var isMenuOnLeft = vInteractSettings.IsMenuOnLeftSide;
			bool isSideChange = (isMenuOnLeft != vCurrIsMenuOnLeftSide);

			vCurrIsMenuOnLeftSide = isMenuOnLeft;

			UpdateCursors();

			foreach ( CursorType cursorType in vCursorTypes ) {
				ICursorState cursor = hovercursor.GetCursorState(cursorType);
				cursors.Add(cursor);

				ICursorInteractState inter = cursor.AddOrGetInteraction(CursorDomain.Hovercast, "");
				inter.DisplayStrength = Menu.DisplayStrength;
			}

			FullMenu.UpdateAfterInput(vInputProv.GetSide(isMenuOnLeft).Menu, cursors.ToArray());
			
			if ( isSideChange ) {
				OnSideChange();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursors() {
			IDictionary<HovercastCursorType, CursorType> convMap = (vInteractSettings.IsMenuOnLeftSide ? 
				vRightCursorConvertMap : vLeftCursorConvertMap);

			List<CursorType> cursorTypes = vInteractSettings.Cursors
				.Select(x => convMap[x])
				.ToList();

			CursorType[] remTypes = vCursorMap.Keys.Except(cursorTypes).ToArray();
			//CursorType[] addTypes = cursorTypes.Except(vCursorMap.Keys).ToArray();
			IHovercursorState hovercursor = vHovercursorSetup.State;

			foreach ( CursorType cursorType in remTypes ) {
				ICursorState cursor = hovercursor.GetCursorState(cursorType);
				cursor.RemoveInteraction(CursorDomain.Hovercast, "");
			}

			/*foreach ( CursorType cursorType in addTypes ) {
				ICursorState cursor = hovercursor.GetCursorState(cursorType);
				cursor.AddOrGetInteraction(CursorDomain.Hovercast, "");
			}*/

			vCursorTypes = cursorTypes.ToArray();
		}

	}

}
