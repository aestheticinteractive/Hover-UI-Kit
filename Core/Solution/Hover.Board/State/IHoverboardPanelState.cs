using Hover.Board.Items;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardPanelState {

		IItemPanel ItemPanel { get; }
		IHoverboardLayoutState[] Layouts { get; }
		int DisplayDepthHint { get; set; }
		float DisplayStrength { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void PreventEveryItemSelectionViaDisplay(string pName, bool pPrevent);

		/*--------------------------------------------------------------------------------------------*/
		bool IsEveryItemSelectionPreventedViaDisplay();

	}

}
