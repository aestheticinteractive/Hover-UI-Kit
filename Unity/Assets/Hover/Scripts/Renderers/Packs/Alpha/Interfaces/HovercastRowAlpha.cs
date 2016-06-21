using System.Collections.Generic;
using Hover.Interfaces.Cast;
using Hover.Items;
using Hover.Layouts.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Interfaces {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HovercastOpenTransitioner))]
	[RequireComponent(typeof(HovercastRowTransitioner))]
	public class HovercastRowAlpha : MonoBehaviour, ITreeUpdateable, ISettingsController {

		private List<HoverItemData> vItemDataResults;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vItemDataResults = new List<HoverItemData>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateWithTransitions();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithTransitions() {
			HovercastOpenTransitioner open = gameObject.GetComponent<HovercastOpenTransitioner>();
			HovercastRowTransitioner row = gameObject.GetComponent<HovercastRowTransitioner>();
			HovercastInterface cast = gameObject.GetComponent<HovercastInterface>();

			float openProg = open.TransitionProgressCurved;
			float openAlpha = (cast.IsOpen ? openProg : 1-openProg);
			float prevAlpha = openAlpha*(1-row.TransitionProgressCurved);
			float activeAlpha = openAlpha*row.TransitionProgressCurved;
			
			FadeItem(cast.BackItem, openAlpha);
			FadeItem(cast.TitleItem, openAlpha);
			FadeRow(cast.PreviousRow, prevAlpha);
			FadeRow(cast.ActiveRow, activeAlpha);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FadeRow(HoverLayoutArcRow pRow, float pAlpha) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			pRow.GetComponentsInChildren(true, vItemDataResults);

			for ( int i = 0 ; i < vItemDataResults.Count ; i++ ) {
				FadeItem(vItemDataResults[i], pAlpha);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FadeItem(HoverItemData pItemData, float pAlpha) {
			HoverAlphaRenderer rend = pItemData.gameObject.GetComponentInChildren<HoverAlphaRenderer>();

			pItemData.IsEnabled = (pAlpha >= 1); //TODO: move to HovercastRowTransitioner

			rend.Controllers.Set(HoverAlphaRenderer.DisabledAlphaName, this);
			rend.DisabledAlpha = Mathf.Lerp(0, rend.EnabledAlpha, pAlpha);
		}

	}

}
