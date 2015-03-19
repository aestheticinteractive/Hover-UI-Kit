using System;
using Hover.Common.Items;
using Hover.Common.Items.Groups;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class ItemGrid : ItemGroup, IItemGrid {

		public int Cols { get; internal set; }
		public float RowOffset { get; internal set; }
		public float ColOffset { get; internal set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemGrid(Func<IBaseItem[]> pGetItems) : base(pGetItems) {
		}

	}

}
