using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemData))]
	public class HoverItemEventReactor : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void EnableWithBoolValue(HoverItemDataSelectableBool pItemData) {
			GetComponent<HoverItemData>().IsEnabled = pItemData.Value;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void DisableWithBoolValue(HoverItemDataSelectableBool pItemData) {
			GetComponent<HoverItemData>().IsEnabled = !pItemData.Value;
		}

	}

}
