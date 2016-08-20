using System.Collections.Generic;
using Hover.Core.Items.Managers;
using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverpanelInterface))]
	[RequireComponent(typeof(HoverpanelRowTransitioner))]
	public class HoverpanelHighlightPreventer : MonoBehaviour, ITreeUpdateable {

		private const string PreventKey = "HoverpanelTransition";

		private readonly List<HoverItemHighlightState> vItemHighStateResults;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverpanelHighlightPreventer() {
			vItemHighStateResults = new List<HoverItemHighlightState>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverpanelRowTransitioner row = gameObject.GetComponent<HoverpanelRowTransitioner>();
			HoverpanelInterface panel = gameObject.GetComponent<HoverpanelInterface>();
			bool preventHigh = (row.IsTransitionActive);
			
			UpdateRow(panel.PreviousRow, preventHigh);
			UpdateRow(panel.ActiveRow, preventHigh);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRow(HoverLayoutRectRow pRow, bool pPreventHighlight) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			pRow.GetComponentsInChildren(true, vItemHighStateResults);

			for ( int i = 0 ; i < vItemHighStateResults.Count ; i++ ) {
				UpdateHighState(vItemHighStateResults[i], pPreventHighlight);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateHighState(HoverItemHighlightState pHighState, bool pPreventHighlight) {
			pHighState.PreventHighlightViaDisplay(PreventKey, pPreventHighlight);
		}

	}

}
