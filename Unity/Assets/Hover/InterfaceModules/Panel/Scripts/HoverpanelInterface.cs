using System;
using System.Collections.Generic;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverpanelInterface : TreeUpdateableBehavior {

		[Serializable]
		public class HoverpanelRowEvent : UnityEvent<HoverpanelRowSwitchingInfo.RowEntryType> {}

		[SerializeField]
		[FormerlySerializedAs("ActiveRow")]
		private HoverLayoutRectRow _ActiveRow;

		[SerializeField]
		[FormerlySerializedAs("PreviousRow")]
		private HoverLayoutRectRow _PreviousRow;

		public HoverpanelRowEvent OnRowSwitchedEvent = new HoverpanelRowEvent();

		private readonly Stack<HoverLayoutRectRow> vRowHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverpanelInterface() {
			vRowHistory = new Stack<HoverLayoutRectRow>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutRectRow ActiveRow {
			get => _ActiveRow;
			set => this.UpdateValueWithTreeMessage(ref _ActiveRow, value, "ActiveRow");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutRectRow PreviousRow {
			get => _PreviousRow;
			set => this.UpdateValueWithTreeMessage(ref _PreviousRow, value, "PreviousRow");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ActiveRow == null ) {
				ActiveRow = GetComponentInChildren<HoverLayoutRectRow>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Start() {
			base.Start();
			PreviousRow = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			//do nothing...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(IItemDataSelectable pItemData) {
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
