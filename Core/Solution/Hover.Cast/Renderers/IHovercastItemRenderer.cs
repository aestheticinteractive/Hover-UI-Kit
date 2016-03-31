using Hover.Cast.Renderers.Standard;
using Hover.Cast.State;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Renderers {

	/*================================================================================================*/
	public interface IHovercastItemRenderer : IHoverItemRenderer {
	
		IHovercastMenuState MenuState { get; set; }
		IBaseItemState ItemState { get; set; }
		HovercastStandardRendererSettings Settings { get; set; }
		float ArcAngle { get; set; }
		bool? AnimIsFadingIn { get; set; }
		int AnimDirection { get; set; }
		float AnimProgress { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetDepthHint(int pDepthHint);

		/*--------------------------------------------------------------------------------------------*/
		void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos);

	}

}
