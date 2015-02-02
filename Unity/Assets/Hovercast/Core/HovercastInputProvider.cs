using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public abstract class HovercastInputProvider : MonoBehaviour, IInputProvider {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputSide GetSide(bool pIsLeft);

	}

}
