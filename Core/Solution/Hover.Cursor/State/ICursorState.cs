using Hover.Common.Input;
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
		ICursorInteractState AddOrGetInteraction(CursorDomain pDomain, string pId);

		/*--------------------------------------------------------------------------------------------*/
		bool RemoveInteraction(CursorDomain pDomain, string pId); //TODO: perform this every frame?

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
