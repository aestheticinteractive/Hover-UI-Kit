using System;
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
		public HoverLayoutArcRow RootRow;
		public HoverLayoutArcRow ActiveRow;
		public HoverLayoutArcRow PreviousRow;
		public HoverItemData OpenItem;
		public HoverItemData TitleItem;
		public HoverItemData BackItem;
		public bool IsOpen = true;

		public UnityEvent OnOpenToggledEvent;
		public HovercastRowEvent OnRowSwitchedEvent;


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
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastRowTitle rowTitle = ActiveRow.GetComponent<HovercastRowTitle>();

			TitleItem.Label = (rowTitle == null ? "" : rowTitle.RowTitle);
			BackItem.IsEnabled = (ActiveRow != RootRow);
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

			HoverLayoutArcRow targetRow =
				(switchInfo.UsePreviousActiveRow ? PreviousRow : switchInfo.TargetRow);

			if ( targetRow == null ) {
				Debug.LogError("Could not transition to null/missing row.", this);
				return;
			}

			PreviousRow = ActiveRow;
			ActiveRow = targetRow;

			OnRowSwitchedEvent.Invoke(switchInfo.RowEntryTransition);
		}

	}

}
