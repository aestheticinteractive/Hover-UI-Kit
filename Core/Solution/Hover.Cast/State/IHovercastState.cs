using UnityEngine;
using Hover.Cursor.State;
using Hover.Common.State;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastState {

		ICursorState Cursor { get; }
		
		bool IsMenuInputAvailable { get; }
		bool IsMenuVisible { get; }
		float MenuDisplayStrength { get; }
		float NavigateBackStrength { get; }
		HovercastSideName MenuSide { get; }

		IBaseItemState[] CurrentItems { get; }
		IBaseItemState NearestItem { get; }

		Transform BaseTransform { get; }
		Transform MenuTransform { get; }

	}

}
