using Hovercast.Settings;
using Hovercast.State;

namespace Hovercast.Display {

	/*================================================================================================*/
	public interface IUiPalmRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, float pAngle0, float pAngle1);
		
		/*--------------------------------------------------------------------------------------------*/
		void SetSettings(ArcSegmentSettings pSettings);

	}

}
