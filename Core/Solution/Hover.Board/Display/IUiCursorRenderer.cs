using Hover.Board.Custom;
using Hover.Board.State;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(CursorState pCursorState, CursorSettings pSettings);

	}

}
