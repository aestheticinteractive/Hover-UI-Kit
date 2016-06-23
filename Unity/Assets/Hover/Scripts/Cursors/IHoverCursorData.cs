using Hover.Items;
using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	public interface IHoverCursorData {

		CursorType Type { get; }
		bool IsActive { get; }
		bool CanCauseSelections { get; }
		CursorCapabilityType Capability { get; }
		bool IsRaycast { get; }
		float Size { get; }
		float DisplayStrength { get; }
		Vector3 WorldPosition { get; }
		Quaternion WorldRotation { get; }

		RaycastResult? BestRaycastResult { get; set; }
		float MaxItemHighlightProgress { get; set; }
		float MaxItemSelectionProgress { get; set; }

	}

}
