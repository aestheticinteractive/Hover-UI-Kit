using Hover.Cursors;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastBackCursorTrigger : MonoBehaviour, ITreeUpdateable {
		
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

			if ( vIsTriggered && BackTriggerCursor.TriggerStrength < TriggerAgainThreshold ) {
				vIsTriggered = false;
				return;
			}

			if ( vIsTriggered || BackTriggerCursor.TriggerStrength < 1 ) {
				return;
			}

			cast.NavigateBack();
			vIsTriggered = true;
		}

	}

}
