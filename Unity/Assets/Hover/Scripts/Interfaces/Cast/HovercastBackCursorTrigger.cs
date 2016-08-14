using Hover.Cursors;
using Hover.Items.Types;
using Hover.Renderers;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastBackCursorTrigger : MonoBehaviour, ITreeUpdateable, ISettingsController {
		
		public HoverCursorData BackTriggerCursor;

		[Range(0, 1)]
		public float TriggerAgainThreshold = 0.5f;

		private bool vIsTriggered;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastInterface cast = gameObject.GetComponent<HovercastInterface>();
			
			if ( !cast.BackItem.IsEnabled ) {
				return;
			}

			UpdateTrigger(cast);
			UpdateOverrider(cast.BackItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrigger(HovercastInterface pCast) {
			if ( vIsTriggered && BackTriggerCursor.TriggerStrength < TriggerAgainThreshold ) {
				vIsTriggered = false;
				return;
			}

			if ( vIsTriggered || BackTriggerCursor.TriggerStrength < 1 ) {
				return;
			}

			pCast.NavigateBack();
			vIsTriggered = true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateOverrider(HoverItemDataSelector pBackItem) {
			HoverRendererIndicatorOverrider rendInd =
				pBackItem.GetComponent<HoverRendererIndicatorOverrider>();

			if ( rendInd == null ) {
				return;
			}

			float minStren = (vIsTriggered ? TriggerAgainThreshold : 0);
			float stren = BackTriggerCursor.TriggerStrength;

			rendInd.Controllers.Set(HoverRendererIndicatorOverrider.MinHightlightProgressName, this);
			rendInd.Controllers.Set(HoverRendererIndicatorOverrider.MinSelectionProgressName, this);

			rendInd.MinHightlightProgress = stren;
			rendInd.MinSelectionProgress = Mathf.InverseLerp(minStren, 1, stren);
		}

	}

}
