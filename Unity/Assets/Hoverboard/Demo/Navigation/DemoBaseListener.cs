using System.Linq;
using Hoverboard.Core.Navigation;
using Hoverboard.Devices.Leap;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public abstract class DemoBaseListener<T> : HovercastNavItemListener<T>
														where T : Hovercast.Core.Navigation.NavItem {

		protected DemoEnvironment Enviro { get; private set; }
		protected HoverboardLeapInputProvider LeapInputProv { get; private set; }
		protected NavPanel[] NavPanels { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			Enviro = GameObject.Find("DemoEnvironment").GetComponent<DemoEnvironment>();

			LeapInputProv = GameObject.Find("HandController")
				.GetComponent<HoverboardLeapInputProvider>();

			NavPanels = GameObject.Find("SplitKeyboard")
				.GetComponentsInChildren<HoverboardPanelProvider>()
				.Select(x => x.GetPanel())
				.ToArray();
		}

	}

}
