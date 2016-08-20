using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public interface ILayoutableArc {

		bool isActiveAndEnabled { get; }
		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetArcLayout(float pOuterRadius, float pInnerRadius, 
			float pArcDegrees, ISettingsController pController);

	}

}
