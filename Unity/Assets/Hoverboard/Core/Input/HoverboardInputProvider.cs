using UnityEngine;

namespace Hoverboard.Core.Input {

	/*================================================================================================*/
	public abstract class HoverboardInputProvider : MonoBehaviour, IInputProvider {

		public bool IsEnabled { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverboardInputProvider() {
			IsEnabled = true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputCursor GetCursor(CursorType pType);

	}

}
