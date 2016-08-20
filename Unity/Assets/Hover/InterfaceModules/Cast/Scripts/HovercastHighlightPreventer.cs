using System.Collections.Generic;
using Hover.Core.Items;
using Hover.Core.Items.Managers;
using Hover.Core.Layouts.Arc;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HovercastOpenTransitioner))]
	[RequireComponent(typeof(HovercastRowTransitioner))]
	public class HovercastHighlightPreventer : MonoBehaviour, ITreeUpdateable {

		private const string PreventKey = "HovercastTransition";

		private readonly List<HoverItemHighlightState> vItemHighStateResults;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastHighlightPreventer() {
			vItemHighStateResults = new List<HoverItemHighlightState>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastOpenTransitioner open = gameObject.GetComponent<HovercastOpenTransitioner>();
			HovercastRowTransitioner row = gameObject.GetComponent<HovercastRowTransitioner>();
			HovercastInterface cast = gameObject.GetComponent<HovercastInterface>();
			bool preventHigh = (open.IsTransitionActive || row.IsTransitionActive);
			
			UpdateItem(cast.OpenItem, row.IsTransitionActive);
			UpdateItem(cast.BackItem, preventHigh);
			UpdateItem(cast.TitleItem, preventHigh);
			UpdateRow(cast.PreviousRow, preventHigh);
			UpdateRow(cast.ActiveRow, preventHigh);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRow(HoverLayoutArcRow pRow, bool pPreventHighlight) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			pRow.GetComponentsInChildren(true, vItemHighStateResults);

			for ( int i = 0 ; i < vItemHighStateResults.Count ; i++ ) {
				UpdateHighState(vItemHighStateResults[i], pPreventHighlight);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItem(IItemData pItemData, bool pPreventHighlight) {
			HoverItemHighlightState highState = pItemData.gameObject
				.GetComponent<HoverItemHighlightState>();

			if ( highState == null ) {
				return;
			}

			UpdateHighState(highState, pPreventHighlight);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateHighState(HoverItemHighlightState pHighState, bool pPreventHighlight) {
			pHighState.PreventHighlightViaDisplay(PreventKey, pPreventHighlight);
		}

	}

}
