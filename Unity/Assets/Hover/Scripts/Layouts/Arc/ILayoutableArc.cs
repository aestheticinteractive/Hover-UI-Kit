using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	public interface ILayoutableArc {

		bool isActiveAndEnabled { get; }
		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetArcLayout(float pOuterRadius, float pInnerRadius, 
			float pArcAngle, ISettingsController pController);

	}

}
