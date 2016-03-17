using System.Collections.Generic;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class HoverboardPanel : MonoBehaviour {

		public bool IsEnabled = true;
		public bool IsVisible = true;

		private ItemPanel vPanel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemPanel GetPanel() {
			if ( vPanel == null ) {
				vPanel = new ItemPanel(GetChildLayouts);
				vPanel.DisplayContainer = gameObject;
				vPanel.IsEnabled = IsEnabled;
				vPanel.IsVisible = IsVisible;
			}

			return vPanel;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemLayout[] GetChildLayouts() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var layouts = new List<IItemLayout>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardLayout hgi = tx.GetChild(i).GetComponent<HoverboardLayout>();
				layouts.Add(hgi.GetLayout());
			}

			return layouts.ToArray();
		}

	}

}
