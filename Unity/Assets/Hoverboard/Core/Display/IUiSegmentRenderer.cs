using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display {

	/*================================================================================================*/
	public interface IUiSegmentRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ButtonState pButtonState, ButtonSettings pSegState);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetPointNearestToCursor(Vector3 pCursorLocalPos);

	}

}
