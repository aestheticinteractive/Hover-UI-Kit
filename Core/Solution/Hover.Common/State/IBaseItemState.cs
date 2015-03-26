using Hover.Common.Input;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemState {

		IBaseItem Item { get; }

		bool IsSelectionPrevented { get; }
		float MinHighlightDistance { get; }
		float MaxHighlightProgress { get; }
		bool IsNearestHighlight { get; }
		float SelectionProgress { get; }
		Vector3? NearestCursorWorldPos { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightDistance(CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		float GetHighlightProgress(CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		void PreventSelectionViaDisplay(string pName, bool pPrevent);

	}

}
