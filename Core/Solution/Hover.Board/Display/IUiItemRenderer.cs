using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public interface IUiItemRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(IHoverboardPanelState pPanelState, IHoverboardLayoutState pLayoutState, 
			IBaseItemState pItemState, IItemVisualSettings pSettings);

		/*--------------------------------------------------------------------------------------------*/
		void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos);
	}

}
