using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Items;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Renderers {

	/*================================================================================================*/
	public interface IHovercastItemRenderer<T> : IHoverItemRenderer where T : IBaseItem {
	
		IHovercastMenuState MenuState { get; set; }
		IBaseItemState ItemState { get; set; }
		
		float ArcAngle { get; set; }
		IItemVisualSettings Settings { get; set; }
		
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
