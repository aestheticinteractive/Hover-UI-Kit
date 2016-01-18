using Hover.Common.Items;
using UnityEngine;

namespace Hover.Demo.Common {

	/*================================================================================================*/
	public class DemoItemEventHandler : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelected(ISelectableItem pItem) {
			Debug.Log("Item Selected: "+pItem.AutoId+" / "+pItem.Label);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemDeselected(ISelectableItem pItem) {
			Debug.Log("Item Deselected: "+pItem.AutoId+" / "+pItem.Label);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChangedBool(ISelectableItem<bool> pItem) {
			Debug.Log("Item Value Changed: "+pItem.AutoId+" / "+pItem.Label+" / "+pItem.Value);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChangedFloat(ISelectableItem<float> pItem) {
			Debug.Log("Item Value Changed: "+pItem.AutoId+" / "+pItem.Label+" / "+pItem.Value);
		}

	}

}
