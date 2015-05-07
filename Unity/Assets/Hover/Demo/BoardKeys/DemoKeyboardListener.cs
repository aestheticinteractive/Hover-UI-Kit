using System.Linq;
using Hover.Board;
using Hover.Board.Items;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Demo.BoardKeys {

	/*================================================================================================*/
	public class DemoKeyboardListener : MonoBehaviour {

		private DemoEnvironment vEnviro;
		private DemoTextField vTextField;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vEnviro = GameObject.Find("DemoEnvironment").GetComponent<DemoEnvironment>();
			vTextField = GameObject.Find("DemoTextField").GetComponent<DemoTextField>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			ItemPanel[] itemPanels = GameObject.Find("Hoverboard")
				.GetComponentInChildren<HoverboardSetup>()
				.Panels
				.Select(x => x.GetPanel())
				.ToArray();

			foreach ( ItemPanel itemPanel in itemPanels ) {
				foreach ( IItemLayout itemLayout in itemPanel.Layouts ) {
					foreach ( IBaseItem item in itemLayout.Items ) {
						ISelectableItem selItem = (item as ISelectableItem);

						if ( selItem == null ) {
							continue;
						}

						selItem.OnSelected += HandleItemSelected;
					}
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(ISelectableItem pItem) {
			if ( pItem.Label == "^" ) {
				return;
			}
			
			if ( pItem.Label.Length == 1 ) {
				vEnviro.AddLetter(pItem.Label[0]);
				vTextField.AddLetter(pItem.Label[0]);
				return;
			}

			if ( pItem.Label.ToLower() == "back" ) {
				vEnviro.RemoveLatestLetter();
				vTextField.RemoveLatestLetter();
			}

			if ( pItem.Label.ToLower() == "enter" ) {
				vTextField.ClearLetters();
			}
		}

	}

}
