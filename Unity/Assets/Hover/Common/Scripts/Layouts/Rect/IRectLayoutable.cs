using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Rect {

	/*================================================================================================*/
	public interface IRectLayoutable {

		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController);

		/*--------------------------------------------------------------------------------------------*/
		void UnsetLayoutSize(ISettingsController pController);

	}

}
