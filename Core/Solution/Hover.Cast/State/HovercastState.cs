using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Input;
using Hover.Common.Items.Groups;
using Hover.Common.State;
using Hover.Common.Util;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState, IHovercursorDelegate {

		private static readonly EnumIntKeyComparer<HovercastCursorType> HovercastCursorTypeComparer = 
			new EnumIntKeyComparer<HovercastCursorType>(((a, b) => (a == b)), (a => (int)a));

		public delegate void SideChangeHandler();
		public event SideChangeHandler OnSideChange;

		public MenuState FullMenu { get; private set; }
		public Transform BaseTransform { get; private set; }
		public Transform MenuTransform { get; private set; }

		private readonly HovercursorSetup vHovercursorSetup;
		private readonly InteractionSettings vInteractSettings;
		private readonly IInput vInput;
		private readonly Dictionary<HovercastCursorType, CursorType> vLeftCursorConvertMap;
		private readonly Dictionary<HovercastCursorType, CursorType> vRightCursorConvertMap;

		private bool? vCurrIsMenuOnLeftSide;
		private readonly ReadList<IBaseItemInteractionState> vActiveCursorInteractions;
		private readonly ReadList<PlaneData> vMenuPlanes;
		private readonly ReadList<CursorType> vActiveCursorTypes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInput pInput, Transform pBaseTx) {
			vInteractSettings = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInput = pInput;
			BaseTransform = pBaseTx;

			FullMenu = new MenuState(pItemHierarchy, vInteractSettings);

			vActiveCursorInteractions = new ReadList<IBaseItemInteractionState>();
			vMenuPlanes = new ReadList<PlaneData>();
			vActiveCursorTypes = new ReadList<CursorType>();

			ActiveCursorTypes = vActiveCursorTypes.ReadOnly;

			vLeftCursorConvertMap = 
				new Dictionary<HovercastCursorType, CursorType>(HovercastCursorTypeComparer);
			vLeftCursorConvertMap.Add(HovercastCursorType.Palm, CursorType.LeftPalm);
			vLeftCursorConvertMap.Add(HovercastCursorType.Thumb, CursorType.LeftThumb);
			vLeftCursorConvertMap.Add(HovercastCursorType.Index, CursorType.LeftIndex);
			vLeftCursorConvertMap.Add(HovercastCursorType.Middle, CursorType.LeftMiddle);
			vLeftCursorConvertMap.Add(HovercastCursorType.Ring, CursorType.LeftRing);
			vLeftCursorConvertMap.Add(HovercastCursorType.Pinky, CursorType.LeftPinky);
			vLeftCursorConvertMap.Add(HovercastCursorType.Look, CursorType.Look);

			vRightCursorConvertMap =
				new Dictionary<HovercastCursorType, CursorType>(HovercastCursorTypeComparer);
			vRightCursorConvertMap.Add(HovercastCursorType.Palm, CursorType.RightPalm);
			vRightCursorConvertMap.Add(HovercastCursorType.Thumb, CursorType.RightThumb);
			vRightCursorConvertMap.Add(HovercastCursorType.Index, CursorType.RightIndex);
			vRightCursorConvertMap.Add(HovercastCursorType.Middle, CursorType.RightMiddle);
			vRightCursorConvertMap.Add(HovercastCursorType.Ring, CursorType.RightRing);
			vRightCursorConvertMap.Add(HovercastCursorType.Pinky, CursorType.RightPinky);
			vRightCursorConvertMap.Add(HovercastCursorType.Look, CursorType.Look);

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

			vMenuPlanes.Clear();
			vMenuPlanes.Add(new PlaneData("Hovercast.Menu", MenuTransform, Vector3.up));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			var isMenuOnLeft = vInteractSettings.IsMenuOnLeftSide;
			IInputMenu inputMenu = vInput.GetMenu(isMenuOnLeft);
			HovercastCursorType[] cursorTypes = vInteractSettings.Cursors;
			ReadOnlyCollection<BaseItemState> items = FullMenu.GetItems();

			Dictionary<HovercastCursorType, CursorType> convertMap = 
				(vInteractSettings.IsMenuOnLeftSide ? vRightCursorConvertMap : vLeftCursorConvertMap);

			vActiveCursorTypes.Clear();
			vActiveCursorInteractions.Clear();
			FullMenu.ClearCursors();

			foreach ( HovercastCursorType unsidedCursorType in cursorTypes ) {
				CursorType cursorType = convertMap[unsidedCursorType];
				ICursorState cursor = vHovercursorSetup.State.GetCursorState(cursorType);

				vActiveCursorTypes.Add(cursorType);
				FullMenu.AddCursor(cursor);
			}

			for ( int i = 0 ; i < items.Count ; i++ ) {
				vActiveCursorInteractions.Add(items[i]);
			}

			FullMenu.UpdateAfterInput(inputMenu);

			if ( isMenuOnLeft != vCurrIsMenuOnLeftSide ) {
				vCurrIsMenuOnLeftSide = isMenuOnLeft;
				FullMenu.ResetAllItemCursorInteractions();
				OnSideChange();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		// IHovercursorDelegate
		/*--------------------------------------------------------------------------------------------*/
		public CursorDomain Domain {
			get { 
				return CursorDomain.Hovercast;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsCursorInteractionEnabled {
			get {
				return (FullMenu.DisplayStrength > 0);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<CursorType> ActiveCursorTypes { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public float CursorDisplayStrength {
			get {
				return FullMenu.DisplayStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<IBaseItemInteractionState> GetActiveCursorInteractions(
																			CursorType pCursorType) {
			return vActiveCursorInteractions.ReadOnly;
		}

		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<PlaneData> GetActiveCursorPlanes(CursorType pCursorType) {
			return vMenuPlanes.ReadOnly;
		}

	}

}
