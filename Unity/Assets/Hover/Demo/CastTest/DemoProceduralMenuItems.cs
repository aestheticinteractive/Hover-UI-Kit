using System;
using Hover.Cast.Items;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.CastTest {

	/*================================================================================================*/
	public class DemoProceduralMenuItems : MonoBehaviour {

		// Any code that creates items for Hovercast/Hoverboard must be scheduled earlier than
		// HovercastSetup/HoverboardSetup in Unity's "Script Execution Order". The items must be created
		// in the Awake() method, so that the items are already present when the Awake() method is 
		// called in HovercastSetup/HoverboardSetup.

		private HovercastItemHierarchy vHoverHierarchy;
		private HovercastItem[] vHoverItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHoverHierarchy = gameObject.AddComponent<HovercastItemHierarchy>();
			vHoverHierarchy.Title = "Hovercast VR";

			vHoverItems = new HovercastItem[4];

			for ( int i = 0 ; i < vHoverItems.Length ; ++i ) {
				var itemObj = new GameObject("Item "+i);
				itemObj.transform.SetParent(gameObject.transform, false);

				HovercastItem hoverItem = itemObj.AddComponent<HovercastItem>();
				hoverItem.Label = "Item "+i;
				hoverItem.Type = HovercastItem.HovercastItemType.Radio;
				hoverItem.RadioValue = (i == 0);
				vHoverItems[i] = hoverItem;

				IRadioItem radioItem = (IRadioItem)hoverItem.GetItem();
				radioItem.OnSelected += HandleRadioItemSelected;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			IBaseItem item = vHoverItems[3].GetItem();
			item.Label = "T = "+(DateTime.UtcNow.Ticks/TimeSpan.TicksPerSecond%1000);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleRadioItemSelected(ISelectableItem pItem) {
			Debug.Log("Radio Item Selected! "+pItem.Label);
		}

	}

}
