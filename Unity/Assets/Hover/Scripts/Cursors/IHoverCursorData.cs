using System.Collections.Generic;
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
		Vector3 RaycastLocalDirection { get; }
		float Size { get; }
		float TriggerStrength { get; }
		Vector3 WorldPosition { get; }
		Quaternion WorldRotation { get; }
		IHoverCursorIdle Idle { get; }

		RaycastResult? BestRaycastResult { get; set; }
		float MaxItemHighlightProgress { get; set; }
		float MaxItemSelectionProgress { get; set; }
		List<StickySelectionInfo> ActiveStickySelections { get; }

	}

}
