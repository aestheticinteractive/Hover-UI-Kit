using System.Collections;
using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemDataSelectable))]
	public class HoverSelectorItemAutoFire : MonoBehaviour {

		[Range(0, 1)]
		public float DelaySeconds = 0.25f;

		private WaitForSeconds vWaitForSec;
		private float vPrevDelaySec;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevDelaySec = -1;

			HoverItemDataSelectable selItem = GetComponent<HoverItemDataSelectable>();
			selItem.OnSelectedEvent.AddListener(i => StartCoroutine(UpdateForAutoFire(i)));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IEnumerator UpdateForAutoFire(IItemDataSelectable pSelItem) {
			if ( DelaySeconds != vPrevDelaySec ) {
				vWaitForSec = new WaitForSeconds(DelaySeconds);
				vPrevDelaySec = DelaySeconds;
			}

			yield return vWaitForSec;

			HoverItemData item = (HoverItemData)pSelItem;
			item.GetComponent<HoverItemSelectionState>().ReleaseSelectionPrevention();
		}

	}

}