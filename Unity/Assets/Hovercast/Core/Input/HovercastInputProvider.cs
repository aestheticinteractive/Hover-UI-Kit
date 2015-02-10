using UnityEngine;

namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public abstract class HovercastInputProvider : MonoBehaviour, IInputProvider {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputSide GetSide(bool pIsLeft);

	}

}
