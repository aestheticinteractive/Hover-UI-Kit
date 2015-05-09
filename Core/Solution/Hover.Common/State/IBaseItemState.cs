using Hover.Common.Input;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemState : IBaseItemInteractionState {

		IBaseItem Item { get; }

		float MinHighlightDistance { get; }
		bool IsNearestHighlight { get; }
		Vector3? NearestCursorWorldPos { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightDistance(CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightProgress(CursorType pCursorType);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void PreventSelectionViaDisplay(string pName, bool pPrevent);

		/*--------------------------------------------------------------------------------------------*/
		bool IsSelectionPreventedViaDisplay();

		/*--------------------------------------------------------------------------------------------*/
		bool IsSelectionPreventedViaDisplay(string pName);

	}

}
