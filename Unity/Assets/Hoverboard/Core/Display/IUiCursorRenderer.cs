using Hoverboard.Core.Custom;
using Hoverboard.Core.State;

namespace Hoverboard.Core.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(CursorState pCursorState, CursorSettings pSettings);

	}

}
