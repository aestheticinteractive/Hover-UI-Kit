using System;

namespace Hover.Cursor.Custom {

	/*================================================================================================*/
	public interface ICursorSettings {

		Type Renderer { get; set; }

		float CursorForwardDistance { get; set; }

	}

}
