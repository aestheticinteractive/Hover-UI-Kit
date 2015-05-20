using Hover.Cast.State;
using Hover.Common.Custom;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public interface IUiPalmRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(MenuState pMenuState, IItemVisualSettings pSettings, float pAngle0, float pAngle1);

		/*--------------------------------------------------------------------------------------------*/
		void SetDepthHint(int pDepthHint);

	}

}
