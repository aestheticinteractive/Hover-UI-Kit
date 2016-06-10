using Hover.Layouts.Arc;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastInterface : MonoBehaviour {

		public HoverLayoutArcStack ArcStack;
		public HoverLayoutArcRow ActiveRow;
		public HoverLayoutArcRow PreviousRow;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ArcStack == null ) {
				ArcStack = GetComponentInChildren<HoverLayoutArcStack>();
			}

			if ( ActiveRow == null ) {
				ActiveRow = GetComponentInChildren<HoverLayoutArcRow>();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(HovercastRowSwitcher pSwitcher) { //via SendMessageUpwards()
			if ( PreviousRow != null ) {
				PreviousRow.gameObject.SetActive(false);
			}

			HoverLayoutArcRow targetRow =
				(pSwitcher.UsePreviousActiveRow ? PreviousRow : pSwitcher.TargetRow);

			if ( targetRow == null ) {
				Debug.LogError("Could not transition to null/missing row.", this);
				return;
			}

			//TODO: build transitions based on "pSwitcher.RowEntryTransition"

			ActiveRow.gameObject.SetActive(false);
			PreviousRow = ActiveRow;

			ActiveRow = targetRow;
			ActiveRow.gameObject.SetActive(true);
		}

	}

}
