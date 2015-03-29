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
				vPanel = new ItemPanel(GetChildGrids);
				vPanel.DisplayContainer = gameObject;
				vPanel.IsEnabled = IsEnabled;
				vPanel.IsVisible = IsVisible;
			}

			return vPanel;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemGrid[] GetChildGrids() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var grids = new List<IItemGrid>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardGrid hgi = tx.GetChild(i).GetComponent<HoverboardGrid>();
				grids.Add(hgi.GetGrid());
			}

			return grids.ToArray();
		}

	}

}
