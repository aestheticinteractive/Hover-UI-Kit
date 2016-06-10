using System;
using Hover.Layouts.Arc;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastInterface : MonoBehaviour {

		[Serializable]
		public class HovercastRowEvent : UnityEvent<HovercastRowSwitcher.RowEntryType> {}

		public HoverLayoutArcStack ArcStack;
		public HoverLayoutArcRow ActiveRow;
		public HoverLayoutArcRow PreviousRow;

		public HovercastRowEvent OnRowTransitionEvent;


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

			PreviousRow = ActiveRow;
			ActiveRow = targetRow;

			OnRowTransitionEvent.Invoke(pSwitcher.RowEntryTransition);
		}

	}

}
