using System.Collections.Generic;
using UnityEngine;

namespace Hoverboard.Core.Navigation {

	/*================================================================================================*/
	public class HoverboardGridProvider : MonoBehaviour {

		public bool IsVisible = true;
		public int Columns = 3;
		public float RowOffset;
		public float ColumnOffset;

		private NavGrid vGrid;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavGrid GetGrid() {
			if ( vGrid == null ) {
				vGrid = new NavGrid(GetChildItems, gameObject, IsVisible,
					Columns, RowOffset, ColumnOffset);
			}

			return vGrid;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private NavItem[] GetChildItems() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var items = new List<NavItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardNavItem hni = tx.GetChild(i).GetComponent<HoverboardNavItem>();
				NavItem item = hni.GetItem();

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
