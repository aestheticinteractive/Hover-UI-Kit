using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public abstract class HovercastInputProvider : MonoBehaviour, IInputProvider {

		public virtual Vector3 PalmDirection { get; protected set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputSide GetSide(bool pIsLeft);

	}

}
