using Hover.Common.Input;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface ICursorState {

		CursorType Type { get; }
		bool IsInputAvailable { get; }
		Vector3 Position { get; }
		float Size { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ICursorInteractState AddOrGetInteractionState(CursorDomain pDomain, string pId);

		/*--------------------------------------------------------------------------------------------*/
		bool RemoveInteractionState(CursorDomain pDomain, string pId);

		/*--------------------------------------------------------------------------------------------*/
		float GetMaxDisplayStrength();

		/*--------------------------------------------------------------------------------------------*/
		float GetMaxHighlightProgress();

		/*--------------------------------------------------------------------------------------------*/
		float GetMaxSelectionProgress();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetWorldPosition();

	}

}
