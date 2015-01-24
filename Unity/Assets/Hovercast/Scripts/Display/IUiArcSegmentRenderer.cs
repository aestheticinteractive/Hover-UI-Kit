using Hovercast.Settings;
using Hovercast.State;
using UnityEngine;

namespace Hovercast.Display {

	/*================================================================================================*/
	public interface IUiArcSegmentRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, ArcSegmentState pSegState, float pArcAngle,
																		ArcSegmentSettings pSettings);

		/*--------------------------------------------------------------------------------------------*/
		void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float CalculateCursorDistance(Vector3 pCursorWorldPosition);

	}

}
