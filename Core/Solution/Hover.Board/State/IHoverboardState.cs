using Hover.Board.Custom;
using Hover.Board.Navigation;
using Hover.Cursor.State;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardState {

		HoverboardCustomizationProvider CustomizationProvider { get; }
		HoverboardPanelProvider[] PanelProviders { get; }
		IHovercursorState HovercursorState { get; }

	}

}
