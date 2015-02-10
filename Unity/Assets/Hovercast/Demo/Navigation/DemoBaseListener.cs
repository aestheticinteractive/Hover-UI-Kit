using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using Hovercast.Demo.Settings;
using UnityEngine;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public abstract class DemoBaseListener<T> : HovercastNavItemListener<T> where T : NavItem {

		protected DemoEnvironment Enviro { get; private set; }
		protected ArcSegmentSettings ArcSegSett { get; private set; }
		protected InteractionSettings InteractSett { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			Enviro = GameObject.Find("DemoEnvironment")
				.GetComponent<DemoEnvironment>();

			DemoSettingsProvider settProv = GameObject.Find("DemoEnvironment/MenuItems")
				.GetComponent<DemoSettingsProvider>();

			ArcSegSett = settProv.GetArcSegmentSettings(null);
			InteractSett = settProv.GetInteractionSettings();
		}

	}

}
