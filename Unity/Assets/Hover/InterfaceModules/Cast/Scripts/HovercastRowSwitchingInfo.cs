using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemDataSelectable))]
	public class HovercastRowSwitchingInfo : MonoBehaviour {

		public enum RowEntryType {
			Immediate,
			FromOutside,
			FromInside
		}

		public bool NavigateBack = false;
		public HoverLayoutArcRow NavigateToRow = null;
		public RowEntryType RowEntryTransition = RowEntryType.Immediate;

	}

}
