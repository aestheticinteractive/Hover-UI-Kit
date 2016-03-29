using UnityEngine;

namespace Hover.Cast.Input {

	/*================================================================================================*/
	public abstract class HovercastInput : MonoBehaviour, IInput {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput(); //TODO: can this just use MonoBehaviour.Update()?

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputMenu GetMenu(bool pIsLeft);

	}

}
