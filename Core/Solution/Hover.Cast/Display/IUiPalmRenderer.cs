using Hover.Cast.Custom;
using Hover.Cast.State;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public interface IUiPalmRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, SegmentSettings pSettings, float pAngle0, float pAngle1);

	}

}
