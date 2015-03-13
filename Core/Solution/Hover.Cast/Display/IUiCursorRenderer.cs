using Hover.Cast.Custom;
using Hover.Cast.State;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, CursorState pCursorState, CursorSettings pSettings);

	}

}
