using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface IPalmVisualSettingsProvider {

		bool IsDefaultSettingsComponent { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IPalmVisualSettings GetSettings(IBaseItem pItem);

	}

}
