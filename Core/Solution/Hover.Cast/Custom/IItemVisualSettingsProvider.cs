using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public interface IItemVisualSettingsProvider {

		bool IsDefaultSettingsComponent { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IItemVisualSettings GetSettings(IBaseItem pItem);

	}

}
