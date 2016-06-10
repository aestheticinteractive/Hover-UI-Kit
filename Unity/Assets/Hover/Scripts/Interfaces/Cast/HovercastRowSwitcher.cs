using Hover.Items;
using Hover.Layouts.Arc;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(SelectableItem))]
	public class HovercastRowSwitcher : MonoBehaviour {
		
		public enum RowEntryTransitionType {
			Immediate,
			FromOutside,
			FromInside
		}

		public HoverLayoutArcRow TargetRow;
		public bool UsePreviousActiveRow = false;
		public RowEntryTransitionType RowEntryTransition = RowEntryTransitionType.Immediate;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			GetComponent<SelectableItem>().OnSelectedEvent.AddListener(HandleItemSelected);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(ISelectableItem pItem) {
			SendMessageUpwards("OnRowSwitched", this, SendMessageOptions.RequireReceiver);
		}

	}

}
