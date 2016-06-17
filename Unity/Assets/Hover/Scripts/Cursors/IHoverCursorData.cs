using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	public interface IHoverCursorData {

		CursorType Type { get; }
		bool IsActive { get; }
		bool CanCauseSelections { get; }
		CursorCapabilityType Capability { get; }
		float Size { get; }
		float DisplayStrength { get; }
		Vector3 WorldPosition { get; }
		Quaternion WorldRotation { get; }

		float MaxItemHighlightProgress { get; set; }
		float MaxItemSelectionProgress { get; set; }

	}

}
