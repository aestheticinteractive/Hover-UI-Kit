using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverChildItemsFinder))]
	public class HoverChildItemsEnabler : MonoBehaviour, ITreeUpdateable {

		public bool AreItemsEnabled = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			List<HoverItemData> items = GetComponent<HoverChildItemsFinder>().ChildItems;

			for ( int i = 0 ; i < items.Count ; i++ ) {
				HoverItemData item = items[i];
				//TODO: item.Controllers.Set(HoverItemData.IsEnabledName, this);
				item.IsEnabled = AreItemsEnabled;
			}
		}

	}

}
