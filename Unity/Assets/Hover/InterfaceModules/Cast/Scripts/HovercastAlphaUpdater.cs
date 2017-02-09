using System.Collections.Generic;
using Hover.Core.Items;
using Hover.Core.Layouts.Arc;
using Hover.Core.Utils;
using Hover.RendererModules.Alpha;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HovercastOpenTransitioner))]
	[RequireComponent(typeof(HovercastRowTransitioner))]
	[RequireComponent(typeof(HovercastActiveDirection))]
	public class HovercastAlphaUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		private readonly List<IItemData> vItemDataResults;
		private float vDirectionAlpha;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastAlphaUpdater() {
			vItemDataResults = new List<IItemData>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateDirectionAlpha();
			UpdateWithTransitions();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDirectionAlpha() {
			HovercastActiveDirection activeDir = gameObject.GetComponent<HovercastActiveDirection>();

			vDirectionAlpha = Mathf.InverseLerp(activeDir.InactiveOutsideDegree,
				activeDir.FullyActiveWithinDegree, activeDir.CurrentDegree);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithTransitions() {
			HovercastOpenTransitioner open = gameObject.GetComponent<HovercastOpenTransitioner>();
			HovercastRowTransitioner row = gameObject.GetComponent<HovercastRowTransitioner>();
			HovercastInterface cast = gameObject.GetComponent<HovercastInterface>();

			float openProg = open.TransitionProgressCurved;
			float openAlpha = (cast.IsOpen ? openProg : 1-openProg);
			float prevAlpha = openAlpha*(1-row.TransitionProgressCurved);
			float activeAlpha = openAlpha*row.TransitionProgressCurved;
			
			FadeItem(cast.OpenItem, 1);
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
		private void FadeItem(IItemData pItemData, float pAlpha) {
			HoverAlphaRendererUpdater rendUp = 
				pItemData.gameObject.GetComponentInChildren<HoverAlphaRendererUpdater>();

			if ( rendUp == null ) {
				return;
			}

			rendUp.Controllers.Set(HoverAlphaRendererUpdater.MasterAlphaName, this);
			rendUp.MasterAlpha = vDirectionAlpha*pAlpha;
		}

	}

}
