using Hover.Common.Items.Groups;

namespace Hover.Board.Navigation {

	/*================================================================================================*/
	public interface IItemGrid : IItemGroup {

		int Cols { get; }
		float RowOffset { get; }
		float ColOffset { get; }

	}

}
