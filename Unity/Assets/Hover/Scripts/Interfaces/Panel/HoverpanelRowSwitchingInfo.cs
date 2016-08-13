using Hover.Items;
using Hover.Items.Types;
using Hover.Layouts.Rect;
using UnityEngine;

namespace Hover.Interfaces.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemDataSelectable))]
	public class HoverpanelRowSwitchingInfo : MonoBehaviour {

		public enum RowEntryType {
			Immediate,
			SlideFromTop,
			SlideFromBottom,
			SlideFromFront,
			SlideFromBack,
			SlideFromLeft,
			SlideFromRight,
			RotateFromTop,
			RotateFromBottom,
			RotateFromLeft,
			RotateFromRight
		}

		public bool NavigateBack = false;
		public HoverLayoutRectRow NavigateToRow = null;
		public RowEntryType RowEntryTransition = RowEntryType.Immediate;

	}

}
