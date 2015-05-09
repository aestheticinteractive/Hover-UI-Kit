using Hover.Board.Items;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardPanelState {

		IItemPanel ItemPanel { get; }
		IHoverboardLayoutState[] Layouts { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void PreventEveryItemSelectionViaDisplay(string pName, bool pPrevent);

		/*--------------------------------------------------------------------------------------------*/
		bool IsEveryItemSelectionPreventedViaDisplay();

	}

}
