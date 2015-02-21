using System.Linq;
using Hovercast.Core.Custom;
using Hovercast.Core.Input;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class HovercastState : IHovercastState {

		public HovercastNavProvider NavigationProvider { get; private set; }
		public HovercastCustomizationProvider CustomizationProvider { get; private set; }
		public HovercastInputProvider InputProvider { get; private set; }

		public Transform MenuTransform { get; private set; }
		public Transform CursorTransform { get; private set; }
		public Transform CameraTransform { get; private set; }

		private MenuState vMenuState;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastState(HovercastNavProvider pNav, HovercastCustomizationProvider pCustom,
													HovercastInputProvider pInput, Transform pCamera) {
			NavigationProvider = pNav;
			CustomizationProvider = pCustom;
			InputProvider = pInput;
			CameraTransform = pCamera;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetReferences(MenuState pMenuState, Transform pMenuTx, Transform pCursorTx) {
			vMenuState = pMenuState;
			MenuTransform = pMenuTx;
			CursorTransform = pCursorTx;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsMenuInputAvailable {
			get {
				return vMenuState.Arc.IsInputAvailable;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsCursorInputAvailable {
			get {
				return vMenuState.Cursor.IsInputAvailable;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsMenuVisible {
			get {
				return (vMenuState.Arc.DisplayStrength > 0);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float MenuDisplayStrength {
			get {
				return vMenuState.Arc.DisplayStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float NavigateBackStrength {
			get {
				return vMenuState.Arc.NavBackStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public HovercastSideName MenuSide {
			get {
				return (vMenuState.Arc.IsLeft ? HovercastSideName.Left : HovercastSideName.Right);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public HovercastSideName CursorSide {
			get {
				return (vMenuState.Cursor.IsLeft ? HovercastSideName.Left : HovercastSideName.Right);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercastItemState[] CurrentItems {
			get {
				return vMenuState.Arc.GetSegments().Cast<IHovercastItemState>().ToArray();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IHovercastItemState NearestItem {
			get {
				return vMenuState.Arc.NearestSegment;
			}
		}

	}

}
