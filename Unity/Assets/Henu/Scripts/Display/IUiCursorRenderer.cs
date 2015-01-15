using Henu.Settings;
using Henu.State;

namespace Henu.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, CursorState pCursorState, CursorSettings pSettings);

	}

}
