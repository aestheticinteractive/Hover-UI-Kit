using UnityEngine;

namespace Hoverboard.Core.Input {

	/*================================================================================================*/
	public abstract class HoverboardInputProvider : MonoBehaviour, IInputProvider {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputCursor GetCursor(CursorType pType);

	}

}
