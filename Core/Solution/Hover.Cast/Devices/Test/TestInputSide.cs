using Hover.Cast.Input;
using UnityEngine;

namespace Hover.Cast.Devices.Test {

	/*================================================================================================*/
	public class TestInputSide : MonoBehaviour, IInputSide {

		public bool IsLeft { get; private set;  }

		private TestInputMenu vMenu;
		private TestInputCursor vCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Init(bool pIsLeft) {
			IsLeft = pIsLeft;

			vMenu = HovercastTestInputProvider.GetChild<TestInputMenu>(gameObject, "Menu");
			vCursor = HovercastTestInputProvider.GetChild<TestInputCursor>(gameObject, "Cursor");

			vMenu.IsLeft = IsLeft;
			vCursor.IsLeft = IsLeft;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IInputMenu Menu {
			get {
				return vMenu;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IInputCursor Cursor {
			get {
				return vCursor;
			}
		}

	}

}
