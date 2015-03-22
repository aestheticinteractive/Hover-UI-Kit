using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Cast.Items;
using UnityEngine;
using Hover.Cursor.State;
using Hover.Common.State;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastState {

		HovercastItemsProvider NavigationProvider { get; }
		HovercastCustomizationProvider CustomizationProvider { get; }
		HovercastInputProvider InputProvider { get; }

		ICursorState Cursor { get; }
		
		bool IsMenuInputAvailable { get; }
		bool IsMenuVisible { get; }
		float MenuDisplayStrength { get; }
		float NavigateBackStrength { get; }
		HovercastSideName MenuSide { get; }

		IBaseItemState[] CurrentItems { get; }
		IBaseItemState NearestItem { get; }

		Transform MenuTransform { get; }

	}

}
