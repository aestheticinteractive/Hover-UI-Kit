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
			HovercastInterface cast = gameObject.GetComponent<HovercastInterface>();
			HovercastRowTransitioner trans = gameObject.GetComponent<HovercastRowTransitioner>();

			if ( cast.PreviousRow != null && cast.PreviousRow.gameObject.activeSelf ) {
				FadeRow(cast.PreviousRow, 1-trans.TransitionProgress);
			}

			if ( cast.ActiveRow.gameObject.activeSelf ) {
				FadeRow(cast.ActiveRow, trans.TransitionProgress);
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void FadeRow(HoverLayoutArcRow pRow, float pAlpha) {
			pRow.GetComponentsInChildren(true, vItemDataResults);

			for ( int i = 0 ; i < vItemDataResults.Count ; i++ ) {
				HoverItemData itemData = vItemDataResults[i];
				HoverAlphaRenderer rend = itemData.gameObject
					.GetComponentInChildren<HoverAlphaRenderer>();

				itemData.IsEnabled = (pAlpha >= 1);

				rend.Controllers.Set(HoverAlphaRenderer.DisabledAlphaName, this);
				rend.DisabledAlpha = Mathf.Lerp(0, rend.EnabledAlpha, pAlpha);
			}
		}
	}

}
