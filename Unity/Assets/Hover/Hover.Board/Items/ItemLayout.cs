using System;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class ItemLayout : ItemGroup, IItemLayout {

		public Vector2 Anchor { get; internal set; }
		public Vector2 Position { get; internal set; }
		public Vector2 Direction { get; internal set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemLayout(Func<IBaseItem[]> pGetItems) : base(pGetItems) {
		}

	}

}
