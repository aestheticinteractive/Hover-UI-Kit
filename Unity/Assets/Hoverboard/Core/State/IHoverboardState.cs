using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.State {

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
