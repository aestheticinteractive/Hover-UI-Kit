using Hover.Common.Input;
using UnityEngine;

namespace Hover.Cursor.Input {

	/*================================================================================================*/
	public abstract class HovercursorInputProvider : MonoBehaviour, IInputProvider {

		public bool IsEnabled { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercursorInputProvider() {
			IsEnabled = true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputCursor GetCursor(CursorType pType);

	}

}


