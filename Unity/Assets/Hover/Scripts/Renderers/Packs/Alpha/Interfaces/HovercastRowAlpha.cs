using System.Collections.Generic;
using Hover.Interfaces.Cast;
using Hover.Items;
using Hover.Layouts.Arc;
using Hover.Renderers.Packs.Alpha.Arc;
using Hover.Renderers.Packs.Alpha.Rect;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Interfaces {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HovercastRowTransitioner))]
	public class HovercastRowAlpha : MonoBehaviour, ITreeUpdateable {

		private List<HoverItemData> vItemDataResults;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
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
				HoverAlphaRendererArcSlider slider = itemData.gameObject
					.GetComponentInChildren<HoverAlphaRendererArcSlider>();
				HoverAlphaRendererArcButton button = itemData.gameObject
					.GetComponentInChildren<HoverAlphaRendererArcButton>();

				itemData.IsEnabled = (pAlpha >= 1);
				if ( slider != null ) { slider.DisabledAlpha = pAlpha; }
				if ( button != null ) { button.DisabledAlpha = pAlpha; }
			}
		}
	}

}
