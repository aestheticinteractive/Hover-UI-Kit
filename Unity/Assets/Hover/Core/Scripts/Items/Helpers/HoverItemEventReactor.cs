using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemData))]
	public class HoverItemEventReactor : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void EnableWithBoolValue(IItemDataSelectable<bool> pItemData) {
			GetComponent<HoverItemData>().IsEnabled = pItemData.Value;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void DisableWithBoolValue(IItemDataSelectable<bool> pItemData) {
			GetComponent<HoverItemData>().IsEnabled = !pItemData.Value;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ShowWithBoolValue(IItemDataSelectable<bool> pItemData) {
			GetComponent<HoverItemData>().gameObject.SetActive(pItemData.Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HideWithBoolValue(IItemDataSelectable<bool> pItemData) {
			GetComponent<HoverItemData>().gameObject.SetActive(!pItemData.Value);
		}

	}

}
