using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Input;
using Hover.Common.Items.Groups;
using Hover.Common.State;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState, IHovercursorDelegate {
		
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
		private IBaseItemInteractionState[] vActiveCursorInteractions;
		private PlaneData[] vMenuPlanes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(IItemHierarchy pItemHierarchy, HovercursorSetup pHovercusorSetup, 
				InteractionSettings pInterSett, IInput pInput, Transform pBaseTx) {
			vInteractSettings = pInterSett;
			vHovercursorSetup = pHovercusorSetup;
			vInput = pInput;
			BaseTransform = pBaseTx;

			FullMenu = new MenuState(pItemHierarchy, vInteractSettings);
			ActiveCursorTypes = new CursorType[0];

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
		public void SetReferences(Transform pMenuTx) {
			MenuTransform = pMenuTx;

			vMenuPlanes = new [] {
				new PlaneData("Hovercast.Menu", MenuTransform, Vector3.up)
			};
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			var isMenuOnLeft = vInteractSettings.IsMenuOnLeftSide;
			IInputMenu inputMenu = vInput.GetMenu(isMenuOnLeft);

			IDictionary<HovercastCursorType, CursorType> convertMap = 
				(vInteractSettings.IsMenuOnLeftSide ? vRightCursorConvertMap : vLeftCursorConvertMap);

			ActiveCursorTypes = vInteractSettings.Cursors
				.Select(x => convertMap[x])
				.ToArray();

			vActiveCursorInteractions = FullMenu.GetItems()
				.Cast<IBaseItemInteractionState>()
				.ToArray();

			ICursorState[] cursors = ActiveCursorTypes
				.Select(x => vHovercursorSetup.State.GetCursorState(x))
				.ToArray();

			FullMenu.UpdateAfterInput(inputMenu, cursors);

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
		public CursorType[] ActiveCursorTypes { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public float CursorDisplayStrength {
			get {
				return FullMenu.DisplayStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemInteractionState[] GetActiveCursorInteractions(CursorType pCursorType) {
			return vActiveCursorInteractions;
		}

		/*--------------------------------------------------------------------------------------------*/
		public PlaneData[] GetActiveCursorPlanes(CursorType pCursorType) {
			return vMenuPlanes;
		}

	}

}
