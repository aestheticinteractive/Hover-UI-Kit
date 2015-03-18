using Hover.Common.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface ICursorInteractState {

		CursorType Type { get; }
		CursorDomain Domain { get; }
		string Id { get; }
		float DisplayStrength { get; set; }
		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		Vector3 CursorToTarget { get; set; }

	}

}
