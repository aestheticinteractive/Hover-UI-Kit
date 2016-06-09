using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Rect {

	/*================================================================================================*/
	public interface IRectLayoutable {

		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController);

	}

}
