using Hover.Common.Items;

namespace Hover.Common.Custom {

	/*================================================================================================*/
	public interface IItemVisualSettingsProvider {

		bool IsDefaultSettingsComponent { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IItemVisualSettings GetSettings(IBaseItem pItem);

	}

}
