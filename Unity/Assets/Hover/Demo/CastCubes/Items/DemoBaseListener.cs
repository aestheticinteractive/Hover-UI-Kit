using Hover.Cast;
using Hover.Cast.Custom;
using Hover.Cast.Items;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================*/
	public abstract class DemoBaseListener<T> : HovercastItemListener<T> where T : ISelectableItem {

		protected DemoEnvironment Enviro { get; private set; }
		protected HovercastSetup CastSetup { get; private set; }
		protected HovercastItemVisualSettings ItemSett { get; private set; }
		protected InteractionSettings InteractSett { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			const string env = "DemoEnvironment";

			Enviro = GameObject.Find(env).GetComponent<DemoEnvironment>();
			CastSetup = GameObject.Find("Hovercast").GetComponent<HovercastSetup>();
			ItemSett = CastSetup.DefaultItemVisualSettings;
			InteractSett = CastSetup.InteractionSettings.GetSettings();
		}

	}

}
