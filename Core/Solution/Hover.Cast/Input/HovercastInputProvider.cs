using UnityEngine;

namespace Hover.Cast.Input {

	/*================================================================================================*/
	public abstract class HovercastInputProvider : MonoBehaviour, IInputProvider {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputSide GetSide(bool pIsLeft);

	}

}
