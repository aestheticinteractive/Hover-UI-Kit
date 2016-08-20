using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastBackCursorTrigger : MonoBehaviour, ITreeUpdateable, ISettingsController {
		
		public CursorType BackTriggerCursorType;

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

			ICursorData cursorData = cast.GetComponent<HoverCursorFollower>()
				.CursorDataProvider.GetCursorData(BackTriggerCursorType);
			float triggerStrength = cursorData.TriggerStrength;

			UpdateTrigger(cast, triggerStrength);
			UpdateOverrider(cast.BackItem, triggerStrength);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrigger(HovercastInterface pCast, float pTriggerStrength) {

			if ( vIsTriggered && pTriggerStrength < TriggerAgainThreshold ) {
				vIsTriggered = false;
				return;
			}

			if ( vIsTriggered || pTriggerStrength < 1 ) {
				return;
			}

			pCast.NavigateBack();
			vIsTriggered = true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateOverrider(HoverItemDataSelector pBackItem, float pTriggerStrength) {
			HoverRendererIndicatorOverrider rendInd =
				pBackItem.GetComponent<HoverRendererIndicatorOverrider>();

			if ( rendInd == null ) {
				return;
			}

			float minStren = (vIsTriggered ? TriggerAgainThreshold : 0);
			float stren = pTriggerStrength;

			rendInd.Controllers.Set(HoverRendererIndicatorOverrider.MinHightlightProgressName, this);
			rendInd.Controllers.Set(HoverRendererIndicatorOverrider.MinSelectionProgressName, this);

			rendInd.MinHightlightProgress = stren;
			rendInd.MinSelectionProgress = Mathf.InverseLerp(minStren, 1, stren);
		}

	}

}
