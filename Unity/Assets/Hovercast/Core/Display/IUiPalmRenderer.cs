using Hovercast.Core.Settings;
using Hovercast.Core.State;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public interface IUiPalmRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, float pAngle0, float pAngle1);
		
		/*--------------------------------------------------------------------------------------------*/
		void SetSettings(ArcSegmentSettings pSettings);

	}

}
