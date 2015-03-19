using Hover.Board.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public interface IUiItemRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(IBaseItemState pItemState, IItemVisualSettings pSettings);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos);

	}

}
