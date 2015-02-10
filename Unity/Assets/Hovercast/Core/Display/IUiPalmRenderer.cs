using Hovercast.Core.Custom;
using Hovercast.Core.State;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public interface IUiPalmRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, SegmentSettings pSettings, float pAngle0, float pAngle1);

	}

}
