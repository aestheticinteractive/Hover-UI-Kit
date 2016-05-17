using Hover.Common.Utils;

namespace Hover.Common.Layouts {

	/*================================================================================================*/
	public interface IRectangleLayoutElement {

		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController);

		/*--------------------------------------------------------------------------------------------*/
		void UnsetLayoutSize(ISettingsController pController);

	}

}
