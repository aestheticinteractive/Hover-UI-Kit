using Hover.Board.Custom;
using Hover.Board.State;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public interface IUiProjectionRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ProjectionState pProjectionState, IProjectionVisualSettings pSettings);

	}

}
