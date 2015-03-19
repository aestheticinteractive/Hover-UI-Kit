using System.Collections.Generic;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class HoverboardGrid : MonoBehaviour {

		public int Columns = 3;
		public float RowOffset;
		public float ColumnOffset;

		private ItemGrid vGrid;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemGrid GetGrid() {
			if ( vGrid == null ) {
				vGrid = new ItemGrid(GetChildItems);
				vGrid.DisplayContainer = gameObject;
				vGrid.Cols = Columns;
				vGrid.RowOffset = RowOffset;
				vGrid.ColOffset = ColumnOffset;
			}

			return vGrid;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var items = new List<IBaseItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardItem hni = tx.GetChild(i).GetComponent<HoverboardItem>();
				IBaseItem item = hni.GetItem();

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
