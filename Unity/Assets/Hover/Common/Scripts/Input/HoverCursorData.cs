using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorData : MonoBehaviour { //TODO: use an interface

		public CursorType Type;
		public float Size;
		public float DisplayStrength; //read-only
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetUsage(bool pIsUsed) {
			enabled = pIsUsed;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ActivateBasedOnUsage() {
			gameObject.SetActive(enabled);
		}

	}

}
