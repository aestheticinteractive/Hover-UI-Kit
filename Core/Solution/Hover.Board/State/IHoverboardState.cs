using Hover.Board.Custom;
using Hover.Board.Input;
using Hover.Board.Navigation;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public interface IHoverboardState {

		HoverboardPanelProvider[] PanelProviders { get; }
		HoverboardCustomizationProvider CustomizationProvider { get; }
		HoverboardInputProvider InputProvider { get; }

		Transform CameraTransform { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Transform GetCursorTransform(CursorType pType);

	}

}
