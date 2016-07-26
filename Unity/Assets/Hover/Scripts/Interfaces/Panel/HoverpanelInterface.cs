using System;
using System.Collections.Generic;
using Hover.Items;
using Hover.Layouts.Rect;
using Hover.Utils;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Hover.Interfaces.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverpanelInterface : MonoBehaviour, ITreeUpdateable {
		
		[Serializable]
		public class HoverpanelRowEvent : UnityEvent<HoverpanelRowSwitchingInfo.RowEntryType> {}

		public HoverLayoutRectRow ActiveRow;
		public HoverLayoutRectRow PreviousRow;

		public HoverpanelRowEvent OnRowSwitchedEvent;

		public readonly Stack<HoverLayoutRectRow> vRowHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverpanelInterface() {
			vRowHistory = new Stack<HoverLayoutRectRow>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ActiveRow == null ) {
				ActiveRow = GetComponentInChildren<HoverLayoutRectRow>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			PreviousRow = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(ISelectableItemData pItemData) {
			HoverpanelRowSwitchingInfo switchInfo = 
				pItemData.gameObject.GetComponent<HoverpanelRowSwitchingInfo>();

			if ( switchInfo == null ) {
				Debug.LogError("Selected item requires a '"+
					typeof(HoverpanelRowSwitchingInfo).Name+"' component.", pItemData.gameObject);
				return;
			}

			if ( PreviousRow != null ) {
				PreviousRow.gameObject.SetActive(false);
			}

			if ( switchInfo.NavigateBack ) {
				NavigateBack(switchInfo.RowEntryTransition);
			}
			else {
				NavigateToRow(switchInfo.NavigateToRow, switchInfo.RowEntryTransition);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void NavigateBack(HoverpanelRowSwitchingInfo.RowEntryType pRowEntry=
												HoverpanelRowSwitchingInfo.RowEntryType.SlideFromTop) {
			if ( vRowHistory.Count == 0 ) {
				Debug.LogWarning("Can't navigate back. No rows left in history.");
				return;
			}

			PreviousRow = ActiveRow;
			ActiveRow = vRowHistory.Pop();

			OnRowSwitchedEvent.Invoke(pRowEntry);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void NavigateToRow(HoverLayoutRectRow pRow, 
													HoverpanelRowSwitchingInfo.RowEntryType pRowEntry) {
			if ( pRow == null ) {
				Debug.LogError("Could not navigate to null/missing row.");
				return;
			}

			vRowHistory.Push(ActiveRow);

			PreviousRow = ActiveRow;
			ActiveRow = pRow;

			OnRowSwitchedEvent.Invoke(pRowEntry);
		}

	}

}
