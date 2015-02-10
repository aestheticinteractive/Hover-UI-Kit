using Hovercast.Core.Custom;
using Hovercast.Core.State;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public interface IUiCursorRenderer {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, CursorState pCursorState, CursorSettings pSettings);

	}

}
