using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public interface IUiItemRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState, float pArcAngle,
																		IItemVisualSettings pSettings);

		/*--------------------------------------------------------------------------------------------*/
		void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress);

		/*--------------------------------------------------------------------------------------------*/
		void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos);

	}

}
