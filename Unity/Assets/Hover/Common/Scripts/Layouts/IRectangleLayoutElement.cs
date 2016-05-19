using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts {

	/*================================================================================================*/
	public interface IRectangleLayoutElement {

		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }
		float RelativeLayoutSizeX { get; }
		float RelativeLayoutSizeY { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController);

		/*--------------------------------------------------------------------------------------------*/
		void UnsetLayoutSize(ISettingsController pController);

	}

}
