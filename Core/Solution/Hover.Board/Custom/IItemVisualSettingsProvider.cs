using Hover.Common.Items;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public interface IItemVisualSettingsProvider {

		bool IsDefaultSettingsComponent { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ItemVisualSettings GetSettings(IBaseItem pItem);

	}

}
