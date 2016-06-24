using System;
using System.Collections.Generic;
using Hover.Layouts.Arc;
using Hover.Items;
using Hover.Utils;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HovercastInterface : MonoBehaviour, ITreeUpdateable {
		
		[Serializable]
		public class HovercastRowEvent : UnityEvent<HovercastRowSwitchingInfo.RowEntryType> {}

		public Transform RowContainer;
		public HoverLayoutArcRow ActiveRow;
		public HoverLayoutArcRow PreviousRow;
		public HoverItemData OpenItem;
		public HoverItemData TitleItem;
		public HoverItemData BackItem;
		public bool IsOpen = true;

		public UnityEvent OnOpenToggledEvent;
		public HovercastRowEvent OnRowSwitchedEvent;

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
				RowContainer = gameObject.transform.FindChild("Rows");
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
		public void OnOpenToggled(ISelectableItem pItem) {
			IsOpen = !IsOpen;
			OnOpenToggledEvent.Invoke();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(ISelectableItem pItem) {
			HovercastRowSwitchingInfo switchInfo = 
				pItem.gameObject.GetComponent<HovercastRowSwitchingInfo>();

			if ( switchInfo == null ) {
				Debug.LogError("Selected item requires a '"+
					typeof(HovercastRowSwitchingInfo).Name+"' component.", pItem.gameObject);
				return;
			}

			if ( PreviousRow != null ) {
				PreviousRow.gameObject.SetActive(false);
			}

			HoverLayoutArcRow targetRow;

			if ( switchInfo.NavigateBack ) {
				if ( vRowHistory.Count == 0 ) {
					Debug.LogWarning("Can't navigate back. No rows left in history.");
					return;
				}

				targetRow = vRowHistory.Pop();
			}
			else if ( switchInfo.NavigateToRow == null ) {
				Debug.LogError("Could not navigate to null/missing row.", switchInfo);
				return;
			}
			else {
				targetRow = switchInfo.NavigateToRow;
				vRowHistory.Push(ActiveRow);
				//Debug.Log("Added row to history ("+vRowHistory.Count+"): "+ActiveRow, ActiveRow);
			}

			PreviousRow = ActiveRow;
			ActiveRow = targetRow;

			OnRowSwitchedEvent.Invoke(switchInfo.RowEntryTransition);
		}

	}

}
