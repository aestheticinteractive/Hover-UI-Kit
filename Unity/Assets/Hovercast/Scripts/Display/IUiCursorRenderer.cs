using Hovercast.Settings;
using Hovercast.State;

namespace Hovercast.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, CursorState pCursorState, CursorSettings pSettings);

	}

}
