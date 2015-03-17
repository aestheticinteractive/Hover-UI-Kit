using System.Linq;
using Hover.Board;
using Hover.Board.Navigation;
using Hover.Cast.Items;
using UnityEngine;

namespace Hover.Demo.HoverboardDemo.Navigation {

	/*================================================================================================*/
	public abstract class DemoBaseListener<T> : HovercastItemListener<T> 
																	where T : Cast.Navigation.NavItem {

		protected DemoEnvironment Enviro { get; private set; }
		protected HoverboardSetup HoverboardSetup { get; private set; }
		protected GameObject KeyboardObj { get; private set; }
		protected NavPanel[] NavPanels { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			Enviro = GameObject.Find("DemoEnvironment").GetComponent<DemoEnvironment>();
			HoverboardSetup = GameObject.Find("Hoverboard").GetComponent<HoverboardSetup>();
			KeyboardObj = GameObject.Find("SplitKeyboard");

			NavPanels = KeyboardObj
				.GetComponentsInChildren<HoverboardPanelProvider>()
				.Select(x => x.GetPanel())
				.ToArray();
		}

	}

}
