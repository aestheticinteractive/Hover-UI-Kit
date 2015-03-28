using Hover.Common.Custom;
using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface IPalmVisualSettingsProvider : IItemVisualSettingsProvider {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IPalmVisualSettings GetPalmSettings(IBaseItem pItem);

	}

}
