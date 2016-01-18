using Hover.Common.Items;
using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Demo.Common {

	/*================================================================================================*/
	public class DemoItemEventHandler : MonoBehaviour {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleHierarchyLevelChanged(int pDirection) {
			Debug.Log("Hierarchy Level Changed: Direction="+pDirection);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleHierarchyItemSelected(IItemGroup pGroup, ISelectableItem pItem) {
			Debug.Log("Hierarchy Item Selected: GroupTitle="+pGroup.Title+", "+GetItemString(pItem));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemSelected(ISelectableItem pItem) {
			Debug.Log("Item Selected: "+GetItemString(pItem));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemDeselected(ISelectableItem pItem) {
			Debug.Log("Item Deselected: "+GetItemString(pItem));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChangedBool(ISelectableItem<bool> pItem) {
			Debug.Log("Item Value Changed: "+GetItemString(pItem)+", Value="+pItem.Value);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void HandleItemValueChangedFloat(ISelectableItem<float> pItem) {
			Debug.Log("Item Value Changed: "+GetItemString(pItem)+", Value="+pItem.Value);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private string GetItemString(ISelectableItem pItem) {
			return "AutoId="+pItem.AutoId+", Label="+pItem.Label;
		}

	}

}
