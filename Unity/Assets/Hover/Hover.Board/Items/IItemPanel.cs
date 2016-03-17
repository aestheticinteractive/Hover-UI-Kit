using Hover.Common.Items.Groups;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public interface IItemPanel : IItemGroups { 

		IItemLayout[] Layouts { get; }

	}

}
