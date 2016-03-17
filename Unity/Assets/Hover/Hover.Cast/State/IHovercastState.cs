using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast.State {

	/*================================================================================================*/
	public interface IHovercastState {

		IHovercursorState Hovercursor { get; }
		IHovercastMenuState Menu { get; }
		
		Transform BaseTransform { get; }
		Transform MenuTransform { get; }

	}

}
