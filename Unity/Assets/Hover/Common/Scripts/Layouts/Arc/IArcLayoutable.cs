using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Arc {

	/*================================================================================================*/
	public interface IArcLayoutable {

		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetArcLayout(float pOuterRadius, float pInnerRadius, 
			float pArcAngle, ISettingsController pController);

	}

}
