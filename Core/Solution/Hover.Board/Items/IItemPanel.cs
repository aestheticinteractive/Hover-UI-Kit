using Hover.Common.Items.Groups;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public interface IItemPanel : IItemGroups { 

		IItemGrid[] Grids { get; }

	}

}
