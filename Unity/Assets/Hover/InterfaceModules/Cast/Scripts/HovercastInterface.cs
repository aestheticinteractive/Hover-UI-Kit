using System;
using System.Collections.Generic;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HovercastInterface : MonoBehaviour, ITreeUpdateable {
		
		[Serializable]
		public class HovercastRowEvent : UnityEvent<HovercastRowSwitchingInfo.RowEntryType> {}

		public Transform RowContainer;
		public HoverLayoutArcRow ActiveRow;
		public HoverLayoutArcRow PreviousRow;
		public HoverItemDataSelector OpenItem;
		public HoverItemDataText TitleItem;
		public HoverItemDataSelector BackItem;
		public bool IsOpen = true;

		public UnityEvent OnOpenToggledEvent = new UnityEvent();
		public HovercastRowEvent OnRowSwitchedEvent = new HovercastRowEvent();

		public readonly Stack<HoverLayoutArcRow> vRowHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastInterface() {
			vRowHistory = new Stack<HoverLayoutArcRow>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( RowContainer == null ) {
				RowContainer = gameObject.transform.Find("Rows");
			}

			if ( ActiveRow == null ) {
				ActiveRow = GetComponentInChildren<HoverLayoutArcRow>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			PreviousRow = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastRowTitle rowTitle = ActiveRow.GetComponent<HovercastRowTitle>();

			TitleItem.Label = (rowTitle == null ? "" : rowTitle.RowTitle);
			BackItem.IsEnabled = (vRowHistory.Count > 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnOpenToggled(IItemDataSelectable pItemData) {
			IsOpen = !IsOpen;
			OnOpenToggledEvent.Invoke();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(IItemDataSelectable pItemData) {
			HovercastRowSwitchingInfo switchInfo = 
				pItemData.gameObject.GetComponent<HovercastRowSwitchingInfo>();

			if ( switchInfo == null ) {
				Debug.LogError("Selected item requires a '"+
					typeof(HovercastRowSwitchingInfo).Name+"' component.", pItemData.gameObject);
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
		public void NavigateBack(HovercastRowSwitchingInfo.RowEntryType pRowEntry=
													HovercastRowSwitchingInfo.RowEntryType.FromInside) {
			if ( vRowHistory.Count == 0 ) {
				Debug.LogWarning("Can't navigate back. No rows left in history.");
				return;
			}

			PreviousRow = ActiveRow;
			ActiveRow = vRowHistory.Pop();

			OnRowSwitchedEvent.Invoke(pRowEntry);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void NavigateToRow(HoverLayoutArcRow pRow, 
													HovercastRowSwitchingInfo.RowEntryType pRowEntry) {
			if ( pRow == null ) {
				Debug.LogError("Could not navigate to null/missing row.");
				return;
			}

			vRowHistory.Push(ActiveRow);
			//Debug.Log("Added row to history ("+vRowHistory.Count+"): "+ActiveRow, ActiveRow);

			PreviousRow = ActiveRow;
			ActiveRow = pRow;

			OnRowSwitchedEvent.Invoke(pRowEntry);
		}

	}

}
