using System;
using System.Collections.Generic;
using Hover.Core.Items.Types;
using Hover.Core.Layouts.Arc;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HovercastInterface : TreeUpdateableBehavior {

		[Serializable]
		public class HovercastRowEvent : UnityEvent<HovercastRowSwitchingInfo.RowEntryType> {}

		[SerializeField]
		[FormerlySerializedAs("RowContainer")]
		private Transform _RowContainer;

		[SerializeField]
		[FormerlySerializedAs("ActiveRow")]
		private HoverLayoutArcRow _ActiveRow;

		[SerializeField]
		[FormerlySerializedAs("PreviousRow")]
		private HoverLayoutArcRow _PreviousRow;

		[SerializeField]
		[FormerlySerializedAs("OpenItem")]
		private HoverItemDataSelector _OpenItem;

		[SerializeField]
		[FormerlySerializedAs("TitleItem")]
		private HoverItemDataText _TitleItem;

		[SerializeField]
		[FormerlySerializedAs("BackItem")]
		private HoverItemDataSelector _BackItem;

		[SerializeField]
		[FormerlySerializedAs("IsOpen")]
		private bool _IsOpen = true;

		public UnityEvent OnOpenToggledEvent = new UnityEvent();
		public HovercastRowEvent OnRowSwitchedEvent = new HovercastRowEvent();

		private readonly Stack<HoverLayoutArcRow> vRowHistory;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastInterface() {
			vRowHistory = new Stack<HoverLayoutArcRow>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Transform RowContainer {
			get => _RowContainer;
			set => this.UpdateValueWithTreeMessage(ref _RowContainer, value, "RowContainer");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcRow ActiveRow {
			get => _ActiveRow;
			set => this.UpdateValueWithTreeMessage(ref _ActiveRow, value, "ActiveRow");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcRow PreviousRow {
			get => _PreviousRow;
			set => this.UpdateValueWithTreeMessage(ref _PreviousRow, value, "PreviousRow");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverItemDataSelector OpenItem {
			get => _OpenItem;
			set => this.UpdateValueWithTreeMessage(ref _OpenItem, value, "OpenItem");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverItemDataText TitleItem {
			get => _TitleItem;
			set => this.UpdateValueWithTreeMessage(ref _TitleItem, value, "TitleItem");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverItemDataSelector BackItem {
			get => _BackItem;
			set => this.UpdateValueWithTreeMessage(ref _BackItem, value, "BackItem");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsOpen {
			get => _IsOpen;
			set => this.UpdateValueWithTreeMessage(ref _IsOpen, value, "IsOpen");
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
		public override void Start() {
			base.Start();
			PreviousRow = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			HovercastRowTitle rowTitle = ActiveRow.GetComponent<HovercastRowTitle>();

			TitleItem.Label = (rowTitle == null ? "" : rowTitle.RowTitle);
			BackItem.IsEnabled = (IsOpen && vRowHistory.Count > 0);

			if ( !IsOpen ) {
				RowContainer.gameObject.SetActive(false);
			}
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
				Debug.LogWarning("Can't navigate back. No rows left in history.", this);
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
				Debug.LogError("Could not navigate to null/missing row.", this);
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
