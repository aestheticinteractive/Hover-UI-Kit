using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public interface IItemLayout : IItemGroup {

		Vector2 Anchor { get; }
		Vector2 Position { get; }
		Vector2 Direction { get; }

	}

}
