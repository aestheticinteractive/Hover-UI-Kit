using System.Collections.Generic;
using Hover.Board.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Demo.BoardKeys.PrefabBuilders {

	/*================================================================================================*/
	public class DemoPrefabSplitKeyboard : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			HoverboardPanel leftPanel = BuildLeftPanel(gameObject);
			HoverboardPanel rightPanel = BuildRightPanel(gameObject);

			leftPanel.gameObject.transform.localPosition = new Vector3(-0.9f, 0, 0);
			leftPanel.gameObject.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, Vector3.forward)*
				Quaternion.AngleAxis(40, Vector3.back);

			rightPanel.gameObject.transform.localPosition = new Vector3(0.9f, 0, 0);
			rightPanel.gameObject.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, Vector3.forward)*
				Quaternion.AngleAxis(-40, Vector3.back);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static HoverboardPanel BuildLeftPanel(GameObject pParent) {
			var panelObj = new GameObject("LeftPanel");
			panelObj.transform.SetParent(pParent.transform, false);

			var panel = panelObj.AddComponent<HoverboardPanel>();

			var rowList = new List<HoverboardGrid>();
			HoverboardGrid row;

			for ( int i = 0 ; i < 5 ; ++i ) {
				var rowObj = new GameObject("Row"+i);
				rowObj.transform.SetParent(panelObj.transform, false);

				row = rowObj.AddComponent<HoverboardGrid>();
				row.Columns = 100;
				row.RowOffset = i-2.5f;
				rowList.Add(row);
			}

			row = rowList[0];
			row.ColumnOffset = -5.5f;
			BuildItem(row, "1", 1);
			BuildItem(row, "2", 1);
			BuildItem(row, "3", 1);
			BuildItem(row, "4", 1);
			BuildItem(row, "5", 1);
			BuildItem(row, "6", 1);

			row = rowList[1];
			row.ColumnOffset = -5;
			BuildItem(row, "Q", 1);
			BuildItem(row, "W", 1);
			BuildItem(row, "E", 1);
			BuildItem(row, "R", 1);
			BuildItem(row, "T", 1);

			row = rowList[2];
			row.ColumnOffset = -4.5f;
			BuildItem(row, "A", 1);
			BuildItem(row, "S", 1);
			BuildItem(row, "D", 1);
			BuildItem(row, "F", 1);
			BuildItem(row, "G", 1);

			row = rowList[3];
			row.ColumnOffset = -6;
			BuildItem(row, "^", 2);
			BuildItem(row, "Z", 1);
			BuildItem(row, "X", 1);
			BuildItem(row, "C", 1);
			BuildItem(row, "V", 1);

			row = rowList[4];
			row.ColumnOffset = -3.5f;
			BuildItem(row, " ", 4);

			return panel;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static HoverboardPanel BuildRightPanel(GameObject pParent) {
			var panelObj = new GameObject("RightPanel");
			panelObj.transform.SetParent(pParent.transform, false);

			var panel = panelObj.AddComponent<HoverboardPanel>();

			var rowList = new List<HoverboardGrid>();
			HoverboardGrid row;

			for ( int i = 0 ; i < 5 ; ++i ) {
				var rowObj = new GameObject("Row"+i);
				rowObj.transform.SetParent(panelObj.transform, false);

				row = rowObj.AddComponent<HoverboardGrid>();
				row.Columns = 100;
				row.RowOffset = i-2.5f;
				rowList.Add(row);
			}

			row = rowList[0];
			row.ColumnOffset = 0;
			BuildItem(row, "7", 1);
			BuildItem(row, "8", 1);
			BuildItem(row, "9", 1);
			BuildItem(row, "0", 1);
			BuildItem(row, "BACK", 2);

			row = rowList[1];
			row.ColumnOffset = -0.5f;
			BuildItem(row, "Y", 1);
			BuildItem(row, "U", 1);
			BuildItem(row, "I", 1);
			BuildItem(row, "O", 1);
			BuildItem(row, "P", 1);

			row = rowList[2];
			row.ColumnOffset = 0;
			BuildItem(row, "H", 1);
			BuildItem(row, "J", 1);
			BuildItem(row, "K", 1);
			BuildItem(row, "L", 1);
			BuildItem(row, "ENTER", 2);

			row = rowList[3];
			row.ColumnOffset = -0.5f;
			BuildItem(row, "B", 1);
			BuildItem(row, "N", 1);
			BuildItem(row, "M", 1);
			BuildItem(row, ".", 1);
			BuildItem(row, "^", 2);

			row = rowList[4];
			row.ColumnOffset = 0;
			BuildItem(row, " ", 4);

			return panel;
		}

		/*--------------------------------------------------------------------------------------------*/
		private static HoverboardItem BuildItem(HoverboardGrid pGrid, string pLabel, int pWidth) {
			var itemObj = new GameObject(pLabel);
			itemObj.transform.SetParent(pGrid.gameObject.transform, false);

			var item = itemObj.AddComponent<HoverboardItem>();
			item.Type = SelectableItemType.Selector;
			item.Label = pLabel;
			item.Width = pWidth;
			return item;
		}

	}

}
