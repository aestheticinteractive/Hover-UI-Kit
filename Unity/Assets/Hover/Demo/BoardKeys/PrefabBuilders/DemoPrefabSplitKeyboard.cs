using System.Collections.Generic;
using Hover.Board.Display;
using Hover.Board.Items;
using UnityEngine;

namespace Hover.Demo.BoardKeys.PrefabBuilders {

	/*================================================================================================*/
	public class DemoPrefabSplitKeyboard : MonoBehaviour {

		private List<HoverboardPanel> vPanels;
		private List<HoverboardLayout> vLayouts;
		private List<HoverboardItem> vItems;
		private bool vAreRenderersRemoved;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPanels = new List<HoverboardPanel>();
			vLayouts = new List<HoverboardLayout>();
			vItems = new List<HoverboardItem>();

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
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vAreRenderersRemoved ) {
				return;
			}

			vAreRenderersRemoved = true;

			//renderers and Ui* classes are created by Hover VR components at runtime

			foreach ( HoverboardPanel panel in vPanels ) {
				Destroy(panel.gameObject.GetComponent<UiPanel>());
			}

			foreach ( HoverboardLayout layout in vLayouts ) {
				Destroy(layout.gameObject.GetComponent<UiLayout>());
			}

			foreach ( HoverboardItem item in vItems ) {
				Destroy(item.gameObject.transform.FindChild("Renderer").gameObject);
				Destroy(item.gameObject.GetComponent<UiItem>());
			}

			Destroy(gameObject.GetComponent<DemoPrefabSplitKeyboard>());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HoverboardPanel BuildLeftPanel(GameObject pParent) {
			var panelObj = new GameObject("LeftPanel");
			panelObj.transform.SetParent(pParent.transform, false);

			var panel = panelObj.AddComponent<HoverboardPanel>();
			vPanels.Add(panel);

			var rowList = new List<HoverboardLayout>();
			HoverboardLayout row;

			for ( int i = 0 ; i < 5 ; ++i ) {
				var rowObj = new GameObject("Row"+i);
				rowObj.transform.SetParent(panelObj.transform, false);

				row = rowObj.AddComponent<HoverboardLayout>();
				row.Anchor = HoverboardLayout.AnchorType.MiddleRight;
				row.GrowDirection = HoverboardLayout.DirectionType.Right;
				row.Position = new Vector2(0, i-2.5f);
				rowList.Add(row);
				vLayouts.Add(row);
			}

			row = rowList[0];
			row.Position.x = 0.5f;
			BuildItem(row, "1", 1);
			BuildItem(row, "2", 1);
			BuildItem(row, "3", 1);
			BuildItem(row, "4", 1);
			BuildItem(row, "5", 1);
			BuildItem(row, "6", 1);

			row = rowList[1];
			row.Position.x = 0;
			BuildItem(row, "Q", 1);
			BuildItem(row, "W", 1);
			BuildItem(row, "E", 1);
			BuildItem(row, "R", 1);
			BuildItem(row, "T", 1);

			row = rowList[2];
			row.Position.x = 0.5f;
			BuildItem(row, "A", 1);
			BuildItem(row, "S", 1);
			BuildItem(row, "D", 1);
			BuildItem(row, "F", 1);
			BuildItem(row, "G", 1);

			row = rowList[3];
			row.Position.x = 0;
			BuildItem(row, "^", 2);
			BuildItem(row, "Z", 1);
			BuildItem(row, "X", 1);
			BuildItem(row, "C", 1);
			BuildItem(row, "V", 1);

			row = rowList[4];
			row.Position.x = 0.5f;
			BuildItem(row, " ", 4);

			return panel;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverboardPanel BuildRightPanel(GameObject pParent) {
			var panelObj = new GameObject("RightPanel");
			panelObj.transform.SetParent(pParent.transform, false);

			var panel = panelObj.AddComponent<HoverboardPanel>();
			vPanels.Add(panel);

			var rowList = new List<HoverboardLayout>();
			HoverboardLayout row;

			for ( int i = 0 ; i < 5 ; ++i ) {
				var rowObj = new GameObject("Row"+i);
				rowObj.transform.SetParent(panelObj.transform, false);

				row = rowObj.AddComponent<HoverboardLayout>();
				row.Anchor = HoverboardLayout.AnchorType.MiddleLeft;
				row.GrowDirection = HoverboardLayout.DirectionType.Right;
				row.Position = new Vector2(0, i-2.5f);
				rowList.Add(row);
				vLayouts.Add(row);
			}

			row = rowList[0];
			row.Position.x = 0;
			BuildItem(row, "7", 1);
			BuildItem(row, "8", 1);
			BuildItem(row, "9", 1);
			BuildItem(row, "0", 1);
			BuildItem(row, "BACK", 2);

			row = rowList[1];
			row.Position.x = -0.5f;
			BuildItem(row, "Y", 1);
			BuildItem(row, "U", 1);
			BuildItem(row, "I", 1);
			BuildItem(row, "O", 1);
			BuildItem(row, "P", 1);

			row = rowList[2];
			row.Position.x = 0;
			BuildItem(row, "H", 1);
			BuildItem(row, "J", 1);
			BuildItem(row, "K", 1);
			BuildItem(row, "L", 1);
			BuildItem(row, "ENTER", 2);

			row = rowList[3];
			row.Position.x = -0.5f;
			BuildItem(row, "B", 1);
			BuildItem(row, "N", 1);
			BuildItem(row, "M", 1);
			BuildItem(row, ".", 1);
			BuildItem(row, "^", 2);

			row = rowList[4];
			row.Position.x = 0;
			BuildItem(row, " ", 4);

			return panel;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverboardItem BuildItem(HoverboardLayout pLayout, string pLabel, int pWidth) {
			var itemObj = new GameObject(pLabel);
			itemObj.transform.SetParent(pLayout.gameObject.transform, false);

			var item = itemObj.AddComponent<HoverboardItem>();
			item.Type = HoverboardItem.HoverboardItemType.Selector;
			item.Label = pLabel;
			item.Width = pWidth;

			vItems.Add(item);
			return item;
		}

	}

}
