using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Cast.Navigation;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastState {

		HovercastNavProvider NavigationProvider { get; }
		HovercastCustomizationProvider CustomizationProvider { get; }
		HovercastInputProvider InputProvider { get; }

		bool IsMenuInputAvailable { get; }
		bool IsCursorInputAvailable { get; }
		bool IsMenuVisible { get; }
		float MenuDisplayStrength { get; }
		float NavigateBackStrength { get; }
		HovercastSideName MenuSide { get; }
		HovercastSideName CursorSide { get; }

		IHovercastItemState[] CurrentItems { get; }
		IHovercastItemState NearestItem { get; }

		Transform MenuTransform { get; }
		Transform CursorTransform { get; }
		Transform CameraTransform { get; }

	}

}
