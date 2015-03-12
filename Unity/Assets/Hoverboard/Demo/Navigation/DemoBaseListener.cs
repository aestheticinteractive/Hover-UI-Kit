using System.Linq;
using Hoverboard.Core;
using Hoverboard.Core.Navigation;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public abstract class DemoBaseListener<T> : HovercastNavItemListener<T>
														where T : Hovercast.Core.Navigation.NavItem {

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
