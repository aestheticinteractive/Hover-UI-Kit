using Hover.Common.Items.Groups;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public interface IItemGrid : IItemGroup {

		int Cols { get; }
		float RowOffset { get; }
		float ColOffset { get; }

	}

}
